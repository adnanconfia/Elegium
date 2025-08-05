myApp.controller('ProfessionalHireRequestCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    //data fetch functions here below

    $scope.getRequests = function () {
        $http.get(root + 'api/ProfessionalHireRequests/GetProfessionalHireRequests').then(function success(response) {
            $scope.requestsList = response.data;
            console.log($scope.requestsList);
        }, function error() { });
    }

    $scope.getRequests();

    //Other logical functions here

    $scope.accept = function (request) {
        $ngConfirm({
            title: 'Accept Offer?',
            content: 'Are you sure to accept this Offer?',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        $http.get(root + 'api/ProfessionalHireRequests/AcceptProfessionalHireRequest/' + request.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: 'Offer accepted',
                                });
                                request.Status = 2;
                            }
                        }, function error() { });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.reject = function (request) {
        $ngConfirm({
            title: 'Reject Offer?',
            content: 'Are you sure to reject this Offer?',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Yes',
                    btnClass: 'btn-danger',
                    action: function () {
                        $http.get(root + 'api/ProfessionalHireRequests/RejectProfessionalHireRequest/' + request.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: 'Offer rejected',
                                });
                                request.Status = 3;
                            }
                        }, function error() { });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.sendEmailToProjectOwner = function (userId) {
        console.log(userId);
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/professionalDetail/message-user-template.html',
            controller: 'contactUserCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                userId: function () {
                    return userId;
                }
            }
        });
    }

});


myApp.controller('MyHireRequestCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    //data fetch functions here below

    $scope.getRequests = function () {
        $http.get(root + 'api/ProfessionalHireRequests/GetMyProfessionalHireRequests').then(function success(response) {
            $scope.requestsList = response.data;
            console.log($scope.requestsList);
        }, function error() { });
    }

    $scope.getRequests();

    //Other logical functions here

});


myApp.controller('contactUserCtrl', function ($scope, $http, $uibModalInstance, userId) {
    //$scope.userProfile = userProfile;

    $uibModalInstance.rendered.then(function () {
        $scope.getCountries();
        $scope.getUserData(userId);
    });

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.getUserData = function (id) {
        $http.get(root + 'api/ProfessionalDetail/GetUserData/' + id).then((response) => {
            $scope.userProfile = response.data.userProfile;
        },
            () => {

            });
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