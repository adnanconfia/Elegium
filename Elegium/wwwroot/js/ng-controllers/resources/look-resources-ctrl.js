myApp.controller('LookResourcesCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $window, $state) {

    $scope.searchQuery = {};

    $scope.savedResourcesList = [];
    $scope.favoriteResourcesList = [];
    $scope.resourcesList = [];

    //Data fetch methods here
    $scope.getEquipmentCategories = function () {
        $http.get(root + 'api/EquipmentCategories/GetEquipmentCategoryShort').then(function success(response) {
            $scope.equipmentCategories = response.data;
            //console.log('equipmentCategories:', $scope.equipmentCategories);
        }, function error() { });
    }

    $scope.getCurrencies = function () {
        $http.get(root + 'api/Currencies').then(function success(response) {
            $scope.currencies = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.getResourceConditions = function () {
        $http.get(root + 'api/ResourceConditions').then(function success(response) {
            $scope.resourceConditions = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getAllResources = function () {
        $http.post(root + 'api/Resources/GetAllResources', $scope.searchQuery).then(function success(response) {
            $scope.resourcesList = response.data;
            console.log($scope.resourcesList);
        }, function error() { });
    }

    $scope.getSavedResources = function () {
        $http.get(root + 'api/Resources/GetSavedResources').then(function success(response) {
            $scope.savedResourcesList = response.data;
            console.log($scope.savedResourcesList);
        }, function error() { });
    }

    $scope.getFavoriteResources = function () {
        $http.get(root + 'api/Resources/GetFavoriteResources').then(function success(response) {
            $scope.favoriteResourcesList = response.data;
            console.log($scope.favoriteResourcesList);
        }, function error() { });
    }




    $scope.getEquipmentCategories();
    $scope.getCurrencies();
    $scope.getCountries();
    $scope.getResourceConditions();
    $scope.getAllResources();
    $scope.getSavedResources();
    $scope.getFavoriteResources();


    //Other logical functions here


    $scope.conditionSelected = function (condId) {
        $scope.searchQuery.ConditionId = condId;
        $scope.getAllResources();
    }

    $scope.hireOrSaleSelected = function (rentId) {
        $scope.searchQuery.HireOrSale = rentId;
        if (rentId == 'H') $scope.searchQuery.ConditionId = 0;
        $scope.getAllResources();
    }

    $scope.search = function () {
        if ($scope.searchQuery.HireOrSale == 'H') {
            $scope.searchQuery.SalePrice = 0;
        }
        else {
            $scope.searchQuery.RentalPrice = 0;
            $scope.searchQuery.Insured = false;
        }
        $scope.getAllResources();
    }

    $scope.clearFilters = function () {
        $scope.searchQuery = {};
    }

    $scope.toggleSave = function (resource) {
        resource.IsSaved = !resource.IsSaved;
        
        angular.forEach($scope.resourcesList, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.Id == resource.Id) {
                    if (resource.IsSaved)
                        $scope.savedResourcesList.push(value);
                    else
                        $scope.savedResourcesList.splice(value, 1);

                    keepGoing = false;
                }
            }
        });
        $http.get(root + 'api/Resources/ToggleSavedResource/' + resource.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }

    $scope.toggleFavorite = function (resource) {
        resource.IsFavorite = !resource.IsFavorite;

        angular.forEach($scope.resourcesList, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.Id == resource.Id) {
                    if (resource.IsFavorite)
                        $scope.favoriteResourcesList.push(value);
                    else
                        $scope.favoriteResourcesList.splice(value, 1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/Resources/ToggleFavoriteResource/' + resource.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }



    $scope.openResourceDetail = function (id) {
        $state.go('resourcedetail', { resourceId: id });
    }

    $scope.openUserDetail = function (id) {
        $state.go('professionaldetails', { userId: id });
    }

    $scope.sendEmailToResourceOwner = function (user) {
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