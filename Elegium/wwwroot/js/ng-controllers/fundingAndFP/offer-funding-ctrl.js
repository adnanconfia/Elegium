myApp.controller('OfferFundingCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $window, $state) {

    var modalInstance = null;


    //Data fetch methods here
    $scope.getCurrentUserPreferences = function () {
        $http.get(root + 'api/UserFundingAndFP/GetCurrentUserFundingAndFP').then(function success(response) {
            $scope.preferences = response.data;
            if ($scope.preferences) {
                if ($scope.preferences.Type) {
                    if ($scope.preferences.Type == 'F') $scope.preferences.Type = false;
                    else $scope.preferences.Type = true;
                }

                $scope.getPublicProjects();
            }
            console.log($scope.preferences);
        }, function error(err) {
            if (err.status == 404)
                $scope.openPreferences();
        });
    }

    $scope.getPublicProjects = function () {
        //GetPublicProjectsForFundingAndFP
        $http.post(root + 'api/PublicProjects/GetPublicProjectsForFundingAndFP').then(function success(response) {
            $scope.projectsList = response.data;
            console.log($scope.projectsList);
        });
    }



    $scope.getCurrentUserPreferences();

    //END -- Data fetch methods here



    $scope.openPreferences = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/offer-funding/preferences-template.html',
            controller: 'PreferencesCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Set your preferences';
                },
                preferences: function () {
                    return $scope.preferences;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {
            //$scope.getProjects();
            $scope.getCurrentUserPreferences();
        });
    }


    $scope.openProjectDetail = function (id) {
        $state.go('publicprojectdetail', { projectId: id });
    }

    $scope.openUserDetail = function (id) {
        $state.go('professionaldetails', { userId: id });
    }

    $scope.sendRequest = function (project, $event) {
        $event.stopPropagation();
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/offer-funding/funding-request-template.html',
            controller: 'OfferFundingRequestCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Set your preferences';
                },
                project: function () {
                    return project;
                }
            }
        });
    }

});

myApp.controller('OfferFundingRequestCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, project, ProjectService) {

    console.log(project);
    $scope.data = {
        dto: project,
        OfferOrLooking: "O",
        ProjectId: project.Project.Id,
        Status: "P",
        OwnerId: project.ProjectOwner.UserId,
        FundingOrFP: project.ProjectPartnerRequirement.NeedFinancialParticipation ? 'FP' : 'F'
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.send = function () {
        $http.post(root + 'api/FundingFPRequest/CreateRequest', $scope.data).then(function (resp) {
            $uibModalInstance.dismiss('close');
        }, function (err) {
            console.log(err);
            alert(err.Message);
        });
    }
});


myApp.controller('PreferencesCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, preferences, ProjectService) {

    $scope.title = title;


    //Data fetch methods here

    $scope.getCurrencies = function () {
        $http.get(root + 'api/Currencies').then(function success(response) {
            $scope.currencies = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            //console.log($scope.countries);
        }, function error() { });
    }

    $scope.getCities = function () {
        $http.get(root + 'api/Cities/GetCities').then(function success(response) {
            $scope.cities = response.data;
            console.log($scope.cities);
        }, function error() { });
    }

    $scope.getProductionType = function () {
        $http.get(root + 'api/ProductionTypes').then(function success(response) {
            $scope.productionTypes = response.data;
            //console.log($scope.productionTypes);
        }, function error() { });
    }

    ProjectService.getProjectManagementPhases().then(function (response) {
        $scope.ProjectManagementPhaseList = response;
    });


    $scope.getCurrencies();
    $scope.getCountries();
    //$scope.getCities();
    $scope.getProductionType();

    //END--Data fetch methods here

    $scope.preferences = preferences;

    $scope.save = function () {

        $scope.preferencesVM = angular.copy($scope.preferences);

        if ($scope.preferencesVM.Type) $scope.preferencesVM.Type = 'FP';
        else $scope.preferencesVM.Type = 'F';

        console.log($scope.preferencesVM);
        $http.post(root + 'api/UserFundingAndFP/SaveOrUpdateUserFundingAndFP', $scope.preferencesVM).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Preferences saved successfully!',
                });
                $scope.cancel();
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});