myApp.controller('LookFundingCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, ProjectService, $window, $state) {

    var modalInstance = null;
    $scope.searchQuery = {};

    $scope.fundingAndFPList = [];
    $scope.savedFundingAndFPList = [];
    $scope.favoriteFundingAndFPList = [];


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

    $scope.getFundingAndFP = function () {
        $http.post(root + 'api/UserFundingAndFP/GetFundingAndFP', $scope.searchQuery).then(function success(response) {
            $scope.fundingAndFPList = response.data;
            console.log($scope.fundingAndFPList);
        }, function error(err) {

        });
    }

    $scope.getSavedFundingAndFP = function () {
        $http.get(root + 'api/UserFundingAndFP/GetSavedFundingAndFP').then(function success(response) {
            $scope.savedFundingAndFPList = response.data;
            console.log($scope.fundingAndFPList);
        }, function error(err) {

        });
    }

    $scope.getFavoriteFundingAndFP = function () {
        $http.get(root + 'api/UserFundingAndFP/GetFavoriteFundingAndFP').then(function success(response) {
            $scope.favoriteFundingAndFPList = response.data;
            console.log($scope.fundingAndFPList);
        }, function error(err) {

        });
    }


    $scope.getCurrencies();
    //$scope.getCities();
    $scope.getCountries();
    $scope.getProductionType();
    $scope.getFundingAndFP();
    $scope.getSavedFundingAndFP();
    $scope.getFavoriteFundingAndFP();


    $scope.searchFund = function (type) {
        $scope.searchQuery.Type = type;
        $scope.getFundingAndFP();
    }

    $scope.search = function () {
        $scope.getFundingAndFP();
    }

    $scope.clearFilters = function () {
        var type = $scope.searchQuery.Type;
        $scope.searchQuery = {};
        $scope.searchQuery.Type = type;
        $scope.getFundingAndFP();
    }

    $scope.toggleSave = function (fund) {
        fund.IsSaved = !fund.IsSaved;
        angular.forEach($scope.fundingAndFPList, function (value, key) {
            var keepGoing = true;
            //console.log(value);
            if (keepGoing) {
                if (value.Id == fund.Id) {
                    if (fund.IsSaved)
                        $scope.savedFundingAndFPList.push(value);
                    else
                        $scope.savedFundingAndFPList.splice($scope.savedFundingAndFPList.indexOf(value), 1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/UserFundingAndFP/ToggleSavedFundingAndFP/' + fund.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });

    }

    $scope.toggleFavorite = function (fund) {
        fund.IsFavorite = !fund.IsFavorite;

        angular.forEach($scope.fundingAndFPList, function (value, key) {
            var keepGoing = true;
            if (keepGoing) {
                if (value.Id == fund.Id) {
                    if (fund.IsFavorite)
                        $scope.favoriteFundingAndFPList.push(value);
                    else
                        $scope.favoriteFundingAndFPList.splice($scope.favoriteFundingAndFPList.indexOf(value), 1);

                    keepGoing = false;
                }
            }
        });

        $http.get(root + 'api/UserFundingAndFP/ToggleFavoriteFundingAndFP/' + fund.Id).then(function success(response) {
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

    $scope.openUserDetail = function (id) {
        $state.go('professionaldetails', { userId: id });
    }

    $scope.sendRequest = function (funding, $event) {
        $event.stopPropagation();
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/offer-funding/funding-request-template.html',
            controller: 'LookingFundingRequestCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Set your preferences';
                },
                funding: function () {
                    return funding;
                }
            }
        });
    }

});

myApp.controller('LookingFundingRequestCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, funding, ProjectService) {

    console.log(funding);
    $scope.data = {
        UserFundingAndFPDto: funding,
        OfferOrLooking: "L",
        Status: "P",
        OwnerId: funding.User.UserId,
        FundingOrFP: funding.Type
    };

    ProjectService.getAllProjects().then((suc) => {
        $scope.projectsList = suc.data;
    });

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.send = function () {
        $http.post(root + 'api/FundingFPRequest/CreateRequest', $scope.data).then(function (resp) {
            $uibModalInstance.dismiss('close');
        }, function (err) {
            alert(err);
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