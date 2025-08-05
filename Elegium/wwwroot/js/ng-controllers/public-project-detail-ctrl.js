myApp.controller('PublicProjectDetailController', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams) {

    var pathname = window.location.pathname.split("/");
    var projectId = $stateParams.projectId;

    //data fetch functions here below

    $scope.getProjectDetail = function () {
        $http.get(root + 'api/PublicProjects/GetPublicProject/' + projectId).then(function success(response) {
            $scope.project = response.data;
            console.log($scope.project);
        }, function error() { });
    }

    $scope.getProjectDetail();

    //Other logicical functions

    $scope.showProjectOwnerLooking = function (proj) {
        if (proj.ProjectPartners.length > 0 ||
            proj.ProjectPartnerRequirement.NeedFinancialParticipation ||
            proj.ProjectFunding.Amount) {
            return true;
        }
        else
            return false;
    }

    $scope.leaveProject = function (project) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/project/dispute-project-template.html',
            controller: 'DisputeProjectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Disputing ' + project.Name;
                },
                projectItem: function () {
                    return project;
                }
            }
        });
        modalInstance.result.then(function (data) {
            console.log(data);
            if (data) {
                var disputeData = {
                    ProjectId: project.Id,
                    Description: data,
                    UserId: project.UserId
                }
                $http.post(root + 'api/PublicProjects/LeaveProject', disputeData).then(function success(response) {
                    if (response.status == 200) {
                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Dispute submitted',
                        });
                    }
                    //console.log($scope.project);
                }, function error(err) {
                    toaster.pop({
                        type: 'error',
                        title: 'Error',
                        body: err.data,
                    });
                });
            }
            else {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: 'Dispute cannot be submitted due to description not provided.',
                });
            }
        }, function (data) {
            //console.log(data);
        });
    }

});

myApp.controller('DisputeProjectCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, projectItem,) {

    $scope.title = title;
    $scope.project = projectItem;

    $scope.submit = function () {
        $uibModalInstance.close($scope.description);
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }

});