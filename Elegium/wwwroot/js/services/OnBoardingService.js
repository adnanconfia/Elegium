myApp.service('OnBoardingService', function ($http) {
    this.GetOnBoarding = function () {
        return $http.get(root + 'api/Onboarding/GetOnBoarding');
    }

    this.GetAllOnBoarding = function (id) {
        return $http.get(root + 'api/Onboarding/GetAllOnBoarding/' + id);
    }

    this.SubmitForOnboarding = function (obj) {
        return $http.post(root + 'api/Onboarding/SubmitForOnboarding', obj);
    }
});