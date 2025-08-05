myApp.controller('VotingResultDetailCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams) {

    var nominationId = $stateParams.votingResultId;

    $scope.remainingTime = [];


    $scope.getNominationDetail = function () {
        $http.get(root + 'api/VoteFunding/GetProjectResultDetail/' + nominationId).then(function success(response) {
            $scope.nomination = response.data;
            $scope.getFollowers();
            console.log('$scope.nomination', $scope.nomination);
        }, function error() { });
    }

    $scope.getRecentVotes = function () {
        $http.get(root + 'api/VoteFunding/GetRecentFinalVotes/' + nominationId).then(function success(response) {
            $scope.recentVotes = response.data;
            console.log('$scope.recentVotes', $scope.recentVotes);
        }, function error() { });
    }

    $scope.getFollowers = function () {
        $http.get(root + 'api/PublicProjects/GetFollowers/' + $scope.nomination.ProjectId).then(function success(response) {
            $scope.followers = response.data;
            console.log('$scope.followers', $scope.followers);
        }, function error() { });
    }

    $scope.getNominationDetail();
    $scope.getRecentVotes();


    //other functions
    $scope.openFaceBookSharer = function () {
        window.open('https://www.facebook.com/sharer.php?u=' + window.location.href, '_blank');
    }
    $scope.openTwitterSharer = function () {
        window.open('https://twitter.com/intent/tweet?url=' + window.location.href, '_blank');
    }
    $scope.openPinterestSharer = function () {
        window.open('https://pinterest.com/pin/create/button/?url=' + window.location.href, '_blank');
    }
    $scope.openLinkedinSharer = function () {
        window.open('https://www.linkedin.com/shareArticle?mini=true&url=' + window.location.href, '_blank');
    }

});