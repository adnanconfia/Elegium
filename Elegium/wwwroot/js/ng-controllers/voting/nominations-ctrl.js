myApp.controller('NominationsCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.searchQuery = {};

    $scope.getNominationApplications = function () {
        $scope.nominationsNotFound = false;
        $scope.isLoading = true;
        $http.post(root + 'api/VoteFunding/GetAllNominationApplications', $scope.searchQuery).then(function success(response) {
            $scope.isLoading = false;
            $scope.nominationApplications = response.data;
            if ($scope.nominationApplications.length == 0) $scope.nominationsNotFound = true;
            console.log('$scope.nominationApplications', $scope.nominationApplications);
            
        }, function error() {
                $scope.isLoading = false;
                $scope.nominationsNotFound = true;
        });
    }

    $scope.getProductionType = function () {
        $http.get(root + 'api/ProductionTypes').then(function success(response) {
            $scope.productionTypes = response.data;
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

    $scope.getNominationApplications();
    $scope.getProductionType();
    $scope.getLanguages();
    $scope.getCountries();



    //other functions
    $scope.openFaceBookSharer = function (id) {
        window.open('https://www.facebook.com/sharer.php?u=' + window.location.href +'/nominationdetail/'+id, '_blank');
    }
    $scope.openTwitterSharer = function (id) {
        window.open('https://twitter.com/intent/tweet?url=' + window.location.href + '/nominationdetail/' +id, '_blank');
    }
    $scope.openPinterestSharer = function (id) {
        window.open('https://pinterest.com/pin/create/button/?url=' + window.location.href + '/nominationdetail/' +id, '_blank');
    }
    $scope.openLinkedinSharer = function (id) {
        window.open('https://www.idedin.com/shareArticle?mini=true&url=' + window.location.href + '/nominationdetail/' +id, '_blank');
    }

    $scope.productionClicked = function (id) {
        $scope.searchQuery.ProductionTypeId = id;
        $scope.getNominationApplications();
    }

    $scope.countryClicked = function (id) {
        $scope.searchQuery.CountryId = id;
        $scope.getNominationApplications();
    }

    $scope.languageClicked = function (id) {
        $scope.searchQuery.LanguageId = id;
        $scope.getNominationApplications();
    }

    $scope.clearFilters = function () {
        if ($scope.searchQuery.ProductionTypeId || $scope.searchQuery.CountryId || $scope.searchQuery.LanguageId) {
            $scope.searchQuery = {};
            $scope.getNominationApplications();
        }
    }
});