myApp.controller('ProfessionalController', function ($scope, $filter, $http, $uibModal, toaster) {

    $(document).ready(function () {
        $(".card").flip({
            trigger: 'manual'
        });
    });

    $scope.searchedProfessionals = [];
    $scope.savedProfessionals = [];
    $scope.favoriteProfessionals = [];


    $scope.getPromotionCategories = function () {
        $http.get(root + 'api/PromotionCategories').then(function success(response) {
            $scope.promotionCategories = response.data;
            console.log($scope.promotionCategories);
        }, function error() { });
    }

    //Navigatate to some specific tab by checking url
    //if (window.location.href.indexOf('#all') >= 0) {

    //}
    $scope.hideFilters = true;

    $scope.searchQuery = {};

    $scope.toggleMe = function (elem) {
        console.log(elem.currentTarget);
        $(elem.currentTarget).flip('toggle');
    }

    $scope.toggle = function (professional) {
        if (professional.whichCard === "front" || !professional.whichCard) {
            professional.whichCard = "back";
        }
        else {
            professional.whichCard = "front";
        }
    }
    //$scope.toggle = function (professional) {
    //    if (!professional.isFlipped) {
    //        professional.isFlipped = true;
    //    }
    //    else {
    //        professional.isFlipped = !professional.isFlipped;
    //    }
    //}

    //Data API methods

    //$scope.getPromotionCategories = function () {
    //    $http.get(root + 'api/PromotionCategories').then(function success(response) {
    //        $scope.promotionCategories = response.data;
    //        //if ($scope.hideFilters) {
    //        //    $scope.searchQuery.PromotionCategory = $scope.promotionCategories[0].Id;
    //        //    $scope.search();
    //        //}
    //        //console.log($scope.promotionCategories);
    //    }, function error(err) { console.log(err) });
    //}

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.getCities = function () {
        $http.get(root + 'api/Cities/GetCities').then(function success(response) {
            $scope.cities = response.data;
            $scope.companyCities = response.data;
            $scope.studioCities = response.data;
            console.log($scope.cities);
        }, function error() { });
    }

    $scope.getCitiesOfCountry = function (countryId) {
        $http.get(root + 'api/Cities/GetCitiesOfCountry?countryId=' + countryId).then(function success(response) {
            $scope.cities = response.data;
            console.log($scope.cities);
        }, function error() { });
    }

    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
            console.log($scope.workingPositions);
        }, function error() { });
    }

    $scope.getSkills = function () {
        $http.get(root + 'api/Skills').then(function success(response) {
            $scope.skills = response.data;
            console.log($scope.skills);
        }, function error() { });
    }

    $scope.getSkillLevels = function () {
        $http.get(root + 'api/SkillLevels').then(function success(response) {
            $scope.skillLevels = response.data;
            console.log($scope.skillLevels);
        }, function error() { });
    }

    $scope.getSavedProfessionals = function () {
        $http.get(root + 'api/Professionals/GetSavedProfessionals').then(function success(response) {
            $scope.savedProfessionals = response.data;
        }, function error(err) { console.log(err) });
    }

    $scope.getFavoriteProfessionals = function () {
        $http.get(root + 'api/Professionals/GetFavoriteProfessionals').then(function success(response) {
            $scope.favoriteProfessionals = response.data;
        }, function error(err) { console.log(err) });
    }


    var modalInstance = null;

    $scope.openCategorySelecter = function () {

        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/professionals/category-select-template.html',
            controller: 'CategorySelectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {

            }
        });

        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.searchQuery.PromotionCategory = data;
            $scope.search();
            $scope.getPromotionCategories();
            //form = data;
        });
    }

    //$scope.getPromotionCategories();
    $scope.getCountries();
    $scope.getCities();
    $scope.getWorkingPositions();
    $scope.getSkills();
    $scope.getSkillLevels();
    $scope.getSavedProfessionals();
    $scope.getFavoriteProfessionals();

    //if (!$scope.hideFilters)
    //    $scope.openCategorySelecter();
    //else {
    //    $scope.getPromotionCategories();
    //}
    $scope.openCategorySelecter();


    //Logical methods here

    $scope.toggleSave = function (professional) {
        professional.IsSaved = !professional.IsSaved;

        angular.forEach($scope.searchedProfessionals, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.UserProfile.UserId == professional.UserProfile.UserId) {
                    if (professional.IsSaved)
                        $scope.savedProfessionals.push(value);
                    else
                        $scope.savedProfessionals.splice($scope.savedProfessionals.indexOf(value), 1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/Professionals/ToggleSavedProfessional/' + professional.UserProfile.UserId).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }

    $scope.toggleFavorite = function (professional) {
        professional.IsFavorite = !professional.IsFavorite;

        angular.forEach($scope.searchedProfessionals, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.UserProfile.UserId == professional.UserProfile.UserId) {
                    if (professional.IsFavorite)
                        $scope.favoriteProfessionals.push(value);
                    else
                        $scope.favoriteProfessionals.splice($scope.favoriteProfessionals.indexOf(value), 1);

                    keepGoing = false;
                }
            }
        });
        $http.get(root + 'api/Professionals/ToggleFavoriteProfessional/' + professional.UserProfile.UserId).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }

    $scope.clearFilters = function () {
        $scope.searchQuery = {
            PromotionCategory: $scope.searchQuery.PromotionCategory
        };
        $scope.search();
    }

    $scope.showHideFilters = function () {
        $scope.hideFilters = !$scope.hideFilters;
    }

    $scope.searchCat = function (catId) {
        $scope.searchQuery.PromotionCategory = catId;
        $scope.search();
    }

    $scope.search = function () {
        $scope.isLoading = true;
        $http.post(root + 'api/Professionals/GetProfessionals', $scope.searchQuery).then(function success(response) {
            //console.log(response.data,'professionals');
            $scope.isLoading = false;
            $scope.searchedProfessionals = response.data;
            if ($scope.searchedProfessionals.length == 0) {
                toaster.pop({
                    type: 'info',
                    title: 'Not found',
                    body: 'No artist found matching your search criteria!',
                });
            }

            //console.log($scope.searchedProfessionals);
        }, function error(err) {
            $scope.isLoading = false;
            console.log(err)
        });
    }

    $scope.sendEmailToProfessional = function (user) {
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

myApp.controller('CategorySelectCtrl', function ($scope, $uibModalInstance, $http) {

    $scope.getPromotionCategories = function () {
        $http.get(root + 'api/PromotionCategories').then(function success(response) {
            $scope.promotionCategories = response.data;
            console.log($scope.promotionCategories);
        }, function error() { });
    }

    $scope.getPromotionCategories();

    $scope.ok = function (item) {
        $uibModalInstance.dismiss(item);
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