myApp.controller('ApplyNominationCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.searchQuery = {};

    $scope.getProductionType = function () {
        $http.get(root + 'api/ProductionTypes').then(function success(response) {
            $scope.productionTypes = response.data;
        }, function error() { });
    }

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
        }, function error() { });
    }

    $scope.getNominations = function () {
        $scope.nominationsNotFound = false;
        $scope.isLoading = true;
        $http.post(root + 'api/VoteFunding/GetAllNominations', $scope.searchQuery).then(function success(response) {
            $scope.isLoading = false;
            $scope.nominations = response.data;
            if ($scope.nominations.length == 0) $scope.nominationsNotFound = true;
            console.log('$scope.nominations', $scope.nominations);

        }, function error() {
            $scope.isLoading = false;
            $scope.nominationsNotFound = true;
        });
    }

    $scope.getProductionType();
    $scope.getCountries();
    $scope.getNominations();

    $scope.productionClicked = function (id) {
        $scope.searchQuery.ProductionTypeId = id;
        $scope.getNominations();
    }

    $scope.countryClicked = function (id) {
        $scope.searchQuery.CountryId = id;
        $scope.getNominations();
    }

    $scope.clearFilters = function () {
        if ($scope.searchQuery.ProductionTypeId || $scope.searchQuery.CountryId) {
            $scope.searchQuery = {};
            $scope.getNominations();
        }
    }

    $scope.openApplyFor = function (nomination) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/voting/apply-for-nomination.html',
            controller: 'SelectProjectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                nomination: function () {
                    return nomination;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }

    $scope.openView = function (nomination) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/voting/nomination-view-detail-template.html',
            controller: 'NominationViewDetailCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                nomination: function () {
                    return nomination;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }
});

myApp.controller('SelectProjectCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, $uibModalInstance, toaster, $ngConfirm, nomination) {
    $scope.nomination = nomination;
    
    $scope.getProjects = function () {
        $http.get(root + 'api/Projects/GetProjectsForNomination').then(function success(response) {
            $scope.projectsList = response.data;
            console.log($scope.projectsList);
        }, function error() { });
    }

    $scope.applyVoteFunding = function () {
        console.log($scope.project);
        if ($scope.project.IsVoteFundingApplied) {
            toaster.pop({
                type: 'info',
                title: '',
                body: 'Vote funding is already applied for this project.',
            });
            return false;
        }
        if (!$scope.project.OnBoardingCompleted) {
            toaster.pop({
                type: 'info',
                title: '',
                body: 'Please complete the onboarding for this project to apply vote funding.',
            });
            return false;
        }

        $ngConfirm({
            title: 'Apply for vote funding?',
            content: 'Are you sure to apply for vote funding?',
            autoClose: 'cancel|8000',
            buttons: {
                apply: {
                    text: 'Apply',
                    btnClass: 'btn-primary',
                    action: function () {

                        var voteFunding = { Id: $scope.nomination.Id, ProjectId: $scope.project.Id };

                        $http.post(root + 'api/VoteFunding/ApplyVoteFunding/', voteFunding).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: 'Applied successfully.',
                                });
                                $scope.cancel();
                            }
                        }, function error(err) {
                            toaster.pop({
                                type: 'error',
                                title: '',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $uibModalInstance.rendered.then(function () {
        $scope.getProjects();
    });

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});

myApp.controller('NominationViewDetailCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, $uibModalInstance, toaster, $ngConfirm, nomination) {
    $scope.nomination = nomination;

    
    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});