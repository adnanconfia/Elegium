myApp.controller('AdminController', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.getSummary = function () {
        $http.get(root + 'api/admin/GetUsersSummary').then(function success(response) {
            $scope.summaryObj = response.data.summaryObj;
        }, function error(response) {
            console.log(response);
        });
    }
    $scope.flag = '';
    $scope.title = 'All Users';

    $scope.getUsers = function (flag) {
        $scope.flag = flag;
        if (flag == "A")
            $scope.title = 'Active Users';
        else if (flag == 'I')
            $scope.title = 'Inctive Users';
        else if (flag == 'B')
            $scope.title = 'Banned Users';
        else
            $scope.title = 'All Users';
        $http.get(root + 'api/admin/GetUsers?flag=' + flag).then(function success(response) {
            $scope.users = response.data.data;
        }, function error(response) {
            console.log(response);
        });
    }

    $scope.ChangeBanActive = function (user, index, action) {
        $http.post(root + 'api/Admin/BanUnBanUser?id=' + user.UserId + "&&action=" + action).then(function success(response) {
            if (response.data.success) {
                user = response.data.userObj;
                $scope.users[index] = user;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: response.data.Msg,
                });
                $scope.getSummary();
                $scope.getUsers($scope.flag);
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: response.data.Msg,
                });
            }
        }, function error(response) {
            console.log(response);
        });
    }

    $scope.editUser = function (user, index) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/admin-module/edit-user-template.html',
            controller: 'editUserCtrl',
            size: 'lg',
            resolve: {
                userObj: function () {
                    return user;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.users[index] = data;
        });
    }

    $scope.editUser = function (user, index) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/admin-module/edit-user-template.html',
            controller: 'editUserCtrl',
            size: 'lg',
            resolve: {
                userObj: function () {
                    return user;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.users[index] = data;
            $scope.getSummary();
        });
    }

    $scope.addUser = function (user, index) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/admin-module/add-user-template.html',
            controller: 'addUserCtrl',
            size: 'lg',
            resolve: {
                userObj: function () {
                    return user;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.users.push(data);
            $scope.getSummary();
        });
    }

    $scope.viewProjects = function (user) {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/admin-module/user-wise-projects.html',
            controller: 'userProjectsCtrl',
            size: 'lg',
            resolve: {
                userObj: function () {
                    return user;
                }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            //$scope.users.push(data);
            //$scope.getSummary();
        });
    }

    $scope.copyToClipboard = function (user) {
        var copyElement = document.createElement("textarea");
        copyElement.style.position = 'fixed';
        copyElement.style.opacity = '0';
        copyElement.textContent = window.location.origin + '#/professionaldetails/' + decodeURI(user.UserId);
        var body = document.getElementsByTagName('body')[0];
        body.appendChild(copyElement);
        copyElement.select();
        document.execCommand('copy');
        body.removeChild(copyElement);
        toaster.pop({
            type: 'success',
            title: 'Success',
            body: 'Profile link has been copied!',
        });
    }

    //

    $scope.deleteUser = function (user) {
        $ngConfirm({
            title: 'Delete ' + user.Name + "'s" + ' profile?',
            contentUrl: root + 'js/ng-templates/admin-module/delete-form.html',
            autoClose: 'cancel|20000',
            icon: 'fa fa-check-circle-o text-center',
            type: 'blue',
            theme: 'modern',
            columnClass: 'medium',
            buttons: {
                sayMyName: {
                    text: 'Delete',
                    disabled: true,
                    btnClass: 'btn-danger',
                    action: function (scope) {
                        $http.post(root + 'api/Admin/DeleteUser', { Id: user.UserId, Password: scope.password }).then(function success(response) {
                            if (response.data.success) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: response.data.Msg,
                                });
                                $scope.getSummary();
                                $scope.getUsers();
                            } else {
                                toaster.pop({
                                    type: 'error',
                                    title: 'Error',
                                    body: response.data.Msg,
                                });
                                return false;
                            }
                        });
                        //$ngConfirm('Hello <strong>' + scope.username + '</strong>, i hope you have a great day!');
                    }
                },
                cancel: function () {
                }
            },
            onScopeReady: function (scope) {
                var self = this;
                scope.textChange = function () {
                    if (scope.password)
                        self.buttons.sayMyName.setDisabled(false);
                    else
                        self.buttons.sayMyName.setDisabled(true);
                }
            }
        })
    };

    $scope.getSummary();
    $scope.getUsers($scope.flag);

}).controller('editUserCtrl', function ($scope, $uibModalInstance, $http, userObj, ProjectService, toaster) {
    $scope.title = 'Edit - ' + userObj.Name;
    $scope.userObj = userObj;

    ProjectService.getCompanyTypes().then(function (response) {
        $scope.companyTypes = response;
    });

    ProjectService.getRoles().then(function (response) {
        $scope.roles = response;
    });

    $scope.ok = function () {
        $http.post(root + 'api/Admin/UpdateUser', $scope.userObj).then(function success(response) {
            if (response.data.success) {
                $scope.userObj = response.data.userObj;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: response.data.Msg,
                });
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: response.data.Msg,
                });
            }
        });
    }

    $scope.Cancel = function () {
        $uibModalInstance.dismiss($scope.userObj);
    }

    $scope.ChangePasswordCheck = function (user) {
        if (!user.ChangePassword) {
            user.Password = '';
            user.ConfirmPassword = '';
        }
    }
}).controller('addUserCtrl', function ($scope, $uibModalInstance, $http, ProjectService, toaster) {
    $scope.title = 'Create New User';
    $scope.userObj = {};

    ProjectService.getRoles().then(function (response) {
        $scope.roles = response;
    });

    ProjectService.getCompanyTypes().then(function (response) {
        $scope.companyTypes = response;
    });

    $scope.ok = function () {
        $http.post(root + 'api/Admin/AddUser', $scope.userObj).then(function success(response) {
            if (response.data.success) {
                $scope.userObj = response.data.userObj;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: response.data.Msg,
                });
                $uibModalInstance.dismiss($scope.userObj);
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: response.data.Msg,
                });
            }
        });
    }

    $scope.Cancel = function () {
        $uibModalInstance.close();
    }
}).controller('userProjectsCtrl', function ($scope, $uibModalInstance, $http, ProjectService, toaster, userObj) {
    $scope.title = userObj.Name + "'s Projects";
    $scope.userObj = userObj;

    $http.get(root + 'api/Admin/GetUserProjects?id=' + $scope.userObj.UserId).then(function success(response) {
        $scope.projectsList = response.data;
        console.log(response, '1');
    });

    $scope.ok = function () {
        $http.post(root + 'api/Admin/AddUser', $scope.userObj).then(function success(response) {
            if (response.data.success) {
                $scope.userObj = response.data.userObj;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: response.data.Msg,
                });
                $uibModalInstance.dismiss($scope.userObj);
            } else {
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: response.data.Msg,
                });
            }
        });
    }

    $scope.Cancel = function () {
        $uibModalInstance.close();
    }
});