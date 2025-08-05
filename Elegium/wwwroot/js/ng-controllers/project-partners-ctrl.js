myApp.controller('ProjectPartnersController', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, ProjectService) {

    //vairables here
    $scope.hideFilters = true;

    $scope.searchQuery = {};

    $scope.savedProjectsList = [];
    $scope.favoriteProjectsList = [];
    $scope.projectsList = [];
    var modalInstance = null;

    //data fetch functions here below

    $scope.getProductionType = function () {
        $http.get(root + 'api/ProductionTypes').then(function success(response) {
            $scope.productionTypes = response.data;
            //console.log($scope.productionTypes);
        }, function error() { });
    }

    $scope.getLanguages = function () {
        $http.get(root + 'api/Languages').then(function success(response) {
            $scope.languages = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            //console.log($scope.countries);
        }, function error() { });
    }

    $scope.getProjects = function () {
        $http.post(root + 'api/PublicProjects/SearchPublicProjects', $scope.searchQuery).then(function success(response) {
            $scope.projectsList = response.data;
            console.log($scope.projectsList);
        }, function error() { });
    }

    $scope.getSavedProjects = function () {
        $http.get(root + 'api/PublicProjects/GetSavedProjectPartners').then(function success(response) {
            $scope.savedProjectsList = response.data;
            //console.log($scope.savedProjectsList);
        }, function error() { });
    }

    $scope.getFavoriteProjects = function () {
        $http.get(root + 'api/PublicProjects/GetFavoriteProjectPartners').then(function success(response) {
            $scope.favoriteProjectsList = response.data;
            //console.log($scope.favoriteProjectsList);
        }, function error() { });
    }

    ProjectService.getProjectManagementPhases().then(function (response) {
        $scope.ProjectManagementPhaseList = response;
        console.log($scope.ProjectManagementPhaseList);
    });

    $scope.getProductionType();
    $scope.getLanguages();
    $scope.getCountries();
    $scope.getProjects();
    $scope.getSavedProjects();
    $scope.getFavoriteProjects();

    //Other logical functions here below

    $scope.getManagmentPhaseName = function (phaseId) {
        var keepGoing = true;
        var phaseName = '';
        angular.forEach($scope.ProjectManagementPhaseList, function (item, key) {
            if (keepGoing) {
                if (item.Id == phaseId) {
                    keepGoing = false;
                    phaseName = item.Name;
                }
            }
        });
        return phaseName;
    }

    $scope.searchCat = function (id) {
        //$scope.searchQuery = {};
        $scope.searchQuery.ProductionTypeId = id;
        $scope.getProjects();
    }

    $scope.showHideFilters = function () {
        $scope.hideFilters = !$scope.hideFilters;
    }

    $scope.search = function () {
        //console.log($scope.productionId);
        $scope.getProjects();
    }

    $scope.clearFilters = function () {
        $scope.searchQuery = {};
        $scope.getProjects();
    }

    $scope.toggleSave = function (proj) {
        proj.IsSavedPartner = !proj.IsSavedPartner;
        angular.forEach($scope.projectsList, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.Project.Id == proj.Id) {
                    if (proj.IsSavedPartner)
                        $scope.savedProjectsList.push(value);
                    else
                        $scope.savedProjectsList.splice($scope.savedProjectsList.indexOf(value), 1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/PublicProjects/ToggleSavedProjectPartner/' + proj.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });

    }

    $scope.toggleFavorite = function (proj) {
        proj.IsFavoritePartner = !proj.IsFavoritePartner;

        angular.forEach($scope.projectsList, function (value, key) {
            var keepGoing = true;
            if (keepGoing) {
                if (value.Project.Id == proj.Id) {
                    if (proj.IsFavoritePartner)
                        $scope.favoriteProjectsList.push(value);
                    else
                        $scope.favoriteProjectsList.splice($scope.favoriteProjectsList.indexOf(value), 1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/PublicProjects/ToggleFavoriteProjectPartner/' + proj.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }

    $scope.sendRequest = function (project) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/project-partner/project-partner-hirerequest-template.html',
            controller: 'ProjectPartnerRequestCtrl',
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

myApp.controller('ProjectPartnerRequestCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, project, ProjectService) {
    $scope.project = project;
    console.log(project);
    $scope.data = {
        ProjectId: project.Project.Id,
        Status: "P",
        OwnerId: project.ProjectOwner.UserId
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.send = function () {
        $http.post(root + 'api/ProjectPartnerRequest/CreateRequest', $scope.data).then(function (resp) {
            $uibModalInstance.dismiss('close');
        }, function (err) {
            console.log(err);
            alert(err.Message);
        });
    }
});