myApp.controller('ProjectDisputeCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    var modalInstance = null;

    $scope.getDisputes = function () {
        $http.get(root + 'api/ProjectDisputes/GetDisputes').then(function success(response) {
            $scope.disputesList = response.data;
            console.log($scope.disputesList);
        }, function error() { });
    }

    $scope.getDisputes();

    //Other logical functions here

    $scope.reject = function (dispute) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/project/dispute-project-template.html',
            controller: 'DisputeProjectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Rejecting dispute for ' + dispute.Project.Name;
                },
                projectItem: function () {
                    return dispute.Project;
                }
            }
        });
        modalInstance.result.then(function (data) {
            //console.log(data);
            if (data) {
                var disputeData = {
                    ProjectId: dispute.Project.Id,
                    Description: data,
                    UserId: dispute.Project.UserId
                }
                $http.post(root + 'api/ProjectDisputes/RejectDispute', disputeData).then(function success(response) {
                    if (response.status == 200) {
                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Dispute rejected',
                        });
                        dispute.Status = 3;
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

    $scope.accept = function (dispute) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/project/dispute-project-template.html',
            controller: 'DisputeProjectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Accepting dispute for ' + dispute.Project.Name;
                },
                projectItem: function () {
                    return dispute.Project;
                }
            }
        });
        modalInstance.result.then(function (data) {
            var disputeData = {
                ProjectId: dispute.Project.Id,
                Description: data,
                UserId: dispute.Project.UserId
            }
            $http.post(root + 'api/ProjectDisputes/ApproveDispute', disputeData).then(function success(response) {
                if (response.status == 200) {
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'Dispute approved',
                    });
                    dispute.Status = 2;
                }
                //console.log($scope.project);
            }, function error(err) {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: err.data,
                });
            });
        }, function (data) {
            //console.log(data);
        });
    }

});


myApp.controller('MyDisputeCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    var modalInstance = null;

    $scope.getDisputes = function () {
        $http.get(root + 'api/ProjectDisputes/GetMyDisputes').then(function success(response) {
            $scope.disputesList = response.data;
            console.log($scope.disputesList);
        }, function error() { });
    }

    $scope.getDisputes();

    //Other logical functions here
    $scope.disputeAgain = function (dispute) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/project/dispute-project-template.html',
            controller: 'DisputeProjectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Disputing again for ' + dispute.Project.Name;
                },
                projectItem: function () {
                    return dispute.Project;
                }
            }
        });
        modalInstance.result.then(function (data) {
            var disputeData = {
                ProjectId: dispute.Project.Id,
                Description: data,
                UserId: dispute.Project.UserId
            }
            $http.post(root + 'api/ProjectDisputes/DisputeAgain', disputeData).then(function success(response) {
                if (response.status == 200) {
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'Dispute submitted again',
                    });
                    dispute.Status = 1;
                }
                //console.log($scope.project);
            }, function error(err) {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: err.data,
                });
            });
        }, function (data) {
            //console.log(data);
        });
    }
});