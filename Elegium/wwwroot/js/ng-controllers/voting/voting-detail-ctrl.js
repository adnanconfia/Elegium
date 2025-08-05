myApp.controller('VotingDetailCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams) {

    var nominationId = $stateParams.votingsId;

    $scope.remainingTime = [];


    $scope.getNominationDetail = function () {
        $http.get(root + 'api/VoteFunding/GetFinalNominationDetail/' + nominationId).then(function success(response) {
            $scope.nomination = response.data;
            //$scope.remainingTime.push({
            //    "StartDateTime": new Date(),
            //    "EndDateTime": new Date($scope.nomination.EndDateTime),
            //});
            //compute($scope.remainingTime);
            console.log('$scope.nomination', $scope.nomination);
        }, function error() { });
    }

    $scope.getRecentVotes = function () {
        $http.get(root + 'api/VoteFunding/GetRecentFinalVotes/' + nominationId).then(function success(response) {
            $scope.recentVotes = response.data;
            console.log('$scope.recentVotes', $scope.recentVotes);
        }, function error() { });
    }

    $scope.getNominationDetail();
    $scope.getRecentVotes();

    $scope.startvoting = function () {
        $scope.votingStarted = true;
        $scope.step = 1;
    }

    $scope.editVote = function () {
        $scope.votingStarted = true;
        $scope.votingFinished = false;
        $scope.step = 1;
        $scope.totalScore = null;
    }

    $scope.voted = function ($event, param) {
        console.log($event.currentTarget.innerHTML);
        param.Score = Number($event.currentTarget.innerHTML);

        if ($scope.step < 4) $scope.step += 1;
        else {
            console.log($scope.nomination.VotingParameters);
            $scope.totalScore = 0;
            angular.forEach($scope.nomination.VotingParameters, function (item) {
                $scope.totalScore += Number(item.Score);
            });
            $scope.totalScore = ($scope.totalScore / $scope.nomination.VotingParameters.length).toFixed(2);
            $scope.votingFinished = true;
            $scope.votingStarted = false;
            $scope.nomination.TotalScore = Number($scope.totalScore);
        }
    }

    $scope.confirmVote = function () {
        $scope.nomination.AlreadyVoted = true;
        //$scope.addUserToRecentVoters();
        $http.post(root + 'api/VoteFunding/SaveNominationFinalVoting', $scope.nomination).then(function success(response) {
            $scope.nomination.AverageScore = response.data.averageScore;
            $scope.addUserToRecentVoters(response.data.userDetails)
            console.log(response.data);
        }, function error() { });

    }

    $scope.addUserToRecentVoters = function (user) {
        var newRecentVote = {
            FinalVoteDetails: [],
            TotalScore: $scope.nomination.TotalScore,
            UserVotedId: user.UserVotedId,
            UserVotedLocation: user.UserVotedLocation,
            UserVotedName: user.UserVotedName,
        }

        angular.forEach($scope.nomination.VotingParameters, function (item) {
            newRecentVote.FinalVoteDetails.push({
                Score: item.Score,
                VotingParameter: { Name: item.Name }
            });
        });

        $scope.recentVotes.push(newRecentVote);
    }




    //other logical functions
    var compute = function (dates) {
        for (var i in dates) {
            dates[i].hours = $filter('date')(dates[i]["EndDateTime"] - dates[i]["StartDateTime"], 'hh');
            dates[i].minutes = $filter('date')(dates[i]["EndDateTime"] - dates[i]["StartDateTime"], 'mm');
            dates[i].days = $filter('date')(dates[i]["EndDateTime"] - dates[i]["StartDateTime"], 'dd');
        }

    }
});