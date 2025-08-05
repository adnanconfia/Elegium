myApp.controller('OnBoardingController', function ($scope, $rootScope, $filter, $http, $uibModal, $timeout, OnBoardingService, ProjectService, $stateParams) {

    console.log($stateParams.onboardingprojectId, 'aa');

    $scope.searchQuery = {};


    ProjectService.getAllProjects().then((suc) => {
        $scope.projectsList = suc.data;
        if ($stateParams.onboardingprojectId) {
            //$timeout(() => {
            $scope.ProjectId = parseInt($stateParams.onboardingprojectId);
            $scope.getProjectOnboarding($scope.ProjectId);
            // }, 1000);
        }
        else {
            $scope.ProjectId = $scope.projectsList[0].Id;
            $scope.getProjectOnboarding($scope.ProjectId);
        }
    });
    $scope.getProjectOnboarding = function (id) {
        OnBoardingService.GetAllOnBoarding(id).then((resp) => {
            $scope.OnBoardingDtoList = resp.data.OnBoardingDtoList;
            $scope.projectDto = resp.data.Project;
        }, (errResp) => { })
    }

    $scope.SubmitForOnboarding = function () {
        OnBoardingService.SubmitForOnboarding($scope.projectDto).then((resp) => {
            $scope.projectDto.OnBoardingCompleted = true;
        }, (errResp) => { });
    }
});