myApp.controller('VotingResultCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm) {
    $scope.getFinalVotingProjects = function () {
        $scope.projectsNotFound = false;
        $scope.isLoading = true;
        $http.get(root + 'api/VoteFunding/GetAllProjectsResult').then(function success(response) {
            $scope.isLoading = false;
            $scope.finalVotingProjects = response.data;
            if ($scope.finalVotingProjects.length == 0) $scope.projectsNotFound = true;
            console.log('$scope.finalVotingProjects', $scope.finalVotingProjects);
        }, function error() {
                $scope.finalVotingProjects = null;
                $scope.projectsNotFound = true;
                $scope.isLoading = false;
        });
    }

    $scope.getFinalVotingProjects();
});