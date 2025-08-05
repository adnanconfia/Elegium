myApp.controller('PublicProjectController', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $window, $state) {

    //vairables here
    $scope.hideFilters = true;
    //$scope.productionId = 0;
    //$scope.budget = '';
    //$scope.languageId = 0;
    //$scope.countryId = 0;
    //$scope.cityId = 0;

    $scope.savedProjectsList = [];
    $scope.favoriteProjectsList = [];
    $scope.projectsList = [];


    $scope.searchQuery = {};


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

    $scope.getCities = function () {
        $http.get(root + 'api/Cities/GetCities').then(function success(response) {
            $scope.cities = response.data;
            //console.log($scope.cities);
        }, function error() { });
    }

    $scope.getCitiesOfCountry = function (countryId) {
        $http.get(root + 'api/Cities/GetCitiesOfCountry?countryId=' + countryId).then(function success(response) {
            $scope.cities = response.data;
            //console.log($scope.cities);
        }, function error() { });
    }

    $scope.getProjects = function () {
        $http.post(root + 'api/PublicProjects/SearchPublicProjects', $scope.searchQuery).then(function success(response) {
            $scope.projectsList = response.data;
            console.log($scope.projectsList);
        }, function error() { });
    }

    $scope.getSavedProjects = function () {
        $http.get(root + 'api/PublicProjects/GetSavedProjects').then(function success(response) {
            $scope.savedProjectsList = response.data;
            //console.log($scope.savedProjectsList);
        }, function error() { });
    }

    $scope.getFavoriteProjects = function () {
        $http.get(root + 'api/PublicProjects/GetFavoriteProjects').then(function success(response) {
            $scope.favoriteProjectsList = response.data;
            //console.log($scope.favoriteProjectsList);
        }, function error() { });
    }

    $scope.getProductionType();
    $scope.getLanguages();
    $scope.getCountries();
    $scope.getCities();
    $scope.getProjects();
    $scope.getSavedProjects();
    $scope.getFavoriteProjects();

    //END - data fetch functions here


    //Other logicical functions

    $scope.showProjectOwnerLooking = function (project) {
        console.log('this is project:',project);
        if (project.ProjectPartners.length > 0 ||
            project.ProjectPartnerRequirement.NeedFinancialParticipation ||
            (project.ProjectFunding && project.ProjectFunding.Amount)) {
            return true;
        }
        else
            return false;
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

    $scope.openProjectDetail = function (id) {
        $state.go('publicprojectdetail', { projectId: id });
        //$window.open(root + 'PublicProjectDetail/' + id, '_blank');
    }

    $scope.toggleSave = function (proj) {
        proj.IsSaved = !proj.IsSaved;
        angular.forEach($scope.projectsList, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.Project.Id == proj.Id) {
                    if (proj.IsSaved)
                        $scope.savedProjectsList.push(value);
                    else
                        $scope.savedProjectsList.splice(value,1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/PublicProjects/ToggleSavedProject/' + proj.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });

    }

    $scope.toggleFavorite = function (proj) {
        proj.IsFavorite = !proj.IsFavorite;

        angular.forEach($scope.projectsList, function (value, key) {
            var keepGoing = true;
            if (keepGoing) {
                if (value.Project.Id == proj.Id) {
                    if (proj.IsFavorite)
                        $scope.favoriteProjectsList.push(value);
                    else
                        $scope.favoriteProjectsList.splice(value,1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/PublicProjects/ToggleFavoriteProject/' + proj.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }

    $scope.sendEmailToProjectOwner = function (user) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/professionalDetail/message-user-template.html',
            controller: 'contactUserCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                userProfile: function () {
                    return user;
                }
            }
        });
    }

    $scope.openProjectDetail = function (id) {
        $state.go('publicprojectdetail', { projectId: id });
        //$window.open(root +'PublicProjectDetail/'+id, '_blank');
    }

});

myApp.controller('contactUserCtrl', function ($scope, $http, $uibModalInstance, userProfile) {
    $scope.userProfile = userProfile;

    $uibModalInstance.rendered.then(function () {
        $scope.getCountries();
    });

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.Send = function () {
        var formData = {
            "UserProfile": $scope.userProfile,
            "UserMessages": $scope.data
        }
        $http.post(root + 'api/ProfessionalDetail/SaveMessage', formData).then(function success(response) {
            if (response.status == 200) {
                gritterAlert(response.data.suc, response.data.Message, response.data.success);
                if (response.data.success)
                    $scope.cancel();
            }
        }, function error() { });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});