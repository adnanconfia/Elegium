myApp.controller('VotingSettingsCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.nominationSetting = {};
    $scope.votingSetting = {};
    $scope.nomination = {};
    $scope.CreateOrEdit = "Create";

    $scope.getProductionType = function () {
        $http.get(root + 'api/ProductionTypes').then(function success(response) {
            $scope.productionTypes = response.data;
        }, function error() { });
    }

    $scope.getCurrencies = function () {
        $http.get(root + 'api/Currencies').then(function success(response) {
            $scope.currencies = response.data;
        }, function error() { });
    }

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.getVotingSettings = function () {
        $http.get(root + 'api/VotingSettings').then(function success(response) {
            $scope.settings = response.data;
            angular.forEach($scope.settings, function (item) {
                if (item.SettingCode == 'NOMINATION_STARTED') {
                    $scope.nominationSetting = item;
                    $scope.nominationSetting.SettingValue = $scope.nominationSetting.SettingValue == 'Y' ? true : false;
                }
                else if (item.SettingCode == 'VOTING_STARTED') {
                    $scope.votingSetting = item;
                    $scope.votingSetting.SettingValue = $scope.votingSetting.SettingValue == 'Y' ? true : false;
                }
            });
            console.log('$scope.settings', $scope.settings);
        }, function error() { });
    }

    $scope.getNominations = function () {
        $http.get(root + 'api/Nominations').then(function success(response) {
            $scope.nominations = response.data;
            console.log('$scope.nominations', $scope.nominations);
        }, function error() { });
    }

    
    $scope.createNomination = function () {
        $http.post(root + 'api/Nominations', $scope.nomination).then(function success(response) {
            console.log('response', response);
            $scope.nominations.push(response.data);
            toaster.pop({
                type: 'success',
                title: '',
                body: 'Nomination is added successfully.',
            });

            $scope.resetCreateNominationFields();

        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: '',
                body: err.data,
            });
        });
    }

    $scope.updateNomination = function () {
        $http.put(root + 'api/Nominations/' + $scope.nomination.Id, $scope.nomination).then(function success(response) {
            console.log('response', response);
            //$scope.nominations.push(response.data);
            toaster.pop({
                type: 'success',
                title: '',
                body: 'Nomination is updated successfully.',
            });

            $scope.resetCreateNominationFields();

        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: '',
                body: err.data,
            });
        });
    }

    $scope.startFinalVoting = function (nomination) {
        $ngConfirm({
            title: 'Start final voting?',
            content: 'Are you sure to start final voting for (' + nomination.Name + ') ?',
            autoClose: 'cancel|9000',
            buttons: {
                yes: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        nomination.IsVotingStarted = true;
                        $http.put(root + 'api/Nominations/' + nomination.Id, nomination).then(function success(response) {
                            toaster.pop({
                                type: 'success',
                                body: 'Successfully done.',
                            });

                        }, function error(err) {
                            nomination.IsVotingStarted = false;
                            toaster.pop({
                                type: 'error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.finishFinalVoting = function (nomination) {
        $ngConfirm({
            title: 'Finish final voting?',
            content: 'Are you sure to finish final voting for (' + nomination.Name + ') ?',
            autoClose: 'cancel|9000',
            buttons: {
                yes: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        nomination.IsVotingFinished = true;
                        $http.put(root + 'api/Nominations/' + nomination.Id, nomination).then(function success(response) {
                            toaster.pop({
                                type: 'success',
                                body: 'Successfully done.',
                            });

                        }, function error(err) {
                            nomination.IsVotingFinished = false;
                            toaster.pop({
                                type: 'error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.resetCreateNominationFields = function () {
        $scope.CreateOrEdit = "Create";
        $('#collapseExample').collapse("hide")
        $scope.nomination = {};
    }

    $scope.deleteNomination = function (nomination) {

        $ngConfirm({
            title: 'Delete Nomination?',
            content: 'Are you sure to delete this nomination?',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Delete',
                    btnClass: 'btn-danger',
                    action: function () {
                        var index = $scope.nominations.indexOf(nomination);
                        $scope.nominations.splice(index, 1);

                        $http.delete(root + 'api/Nominations/' + nomination.Id).then(function success(response) {
                            console.log('response', response);
                        }, function error(err) {
                            toaster.pop({
                                type: 'error',
                                title: '',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });

    }

    $scope.editNomination = function (nomination) {
        $scope.nomination = nomination;
        $scope.nomination.StartDate = new Date($filter('date')(nomination.StartDate, 'dd-MMM-yyyy'));
        $scope.nomination.EndDate = new Date($filter('date')(nomination.EndDate, 'dd-MMM-yyyy'));
        $scope.CreateOrEdit = "Edit";
        $('#collapseExample').collapse("show");
    }


    $scope.saveSetting = function (setting) {
        var mySetting = angular.copy(setting);
        mySetting.SettingValue = mySetting.SettingValue == true ? 'Y' : 'N';
        $http.put(root + 'api/VotingSettings/' + mySetting.Id, mySetting).then(function success(response) {
            console.log('response', response);
        }, function error() { });
    }


    //$scope.getVotingSettings();
    $scope.getProductionType();
    $scope.getCurrencies();
    $scope.getCountries();
    $scope.getNominations();
    

    $scope.openProjectsApplied = function (nomination) {
        if (nomination.ProjectsAppliedCount > 0) {
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/voting/projects-applied-template.html',
                controller: 'ProjectsAppliedCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    nomination: function () {
                        return nomination;
                    }
                }
            });
            modalInstance.result.then(function () {

            }, function (data) {

            });
        }
    }

    $scope.openApproveResults = function (nomination) {
        if (nomination.ProjectsAppliedCount > 0) {
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/voting/approve-results-template.html',
                controller: 'ApproveResultsCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    nomination: function () {
                        return nomination;
                    }
                }
            });
            modalInstance.result.then(function () {

            }, function (data) {

            });
        }
    }

});

myApp.controller('ProjectsAppliedCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, $ngConfirm, nomination) {
    $scope.nomination = nomination;

    $scope.getNominationApplications = function () {
        $http.get(root + 'api/VoteFunding/GetAllNominationApplications/' + nomination.Id).then(function success(response) {
            $scope.nominationApplications = response.data;
            console.log('$scope.nominationApplications', $scope.nominationApplications);
        }, function error() {
            $scope.nominationsNotFound = true;
        });
    }

    $uibModalInstance.rendered.then(function () {
        $scope.getNominationApplications();
    });

    $scope.nominateToFinalVoting = function (application) {
        $ngConfirm({
            title: 'Nominate to final voting?',
            content: 'Are you sure to nominate (' + application.ProjectName + ') to final voting?',
            autoClose: 'cancel|9000',
            buttons: {
                yes: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        application.IsSelectedForFinalVoting = true;
                        $http.get(root + 'api/VoteFunding/NominateToFinalVoting/' + application.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    body: 'Successfully done.',
                                });
                            }
                        }, function error(err) {
                            application.IsSelectedForFinalVoting = false;
                            toaster.pop({
                                type: 'error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.removeFromFinalVoting = function (application) {
        $ngConfirm({
            title: 'Remove from final voting?',
            content: 'Are you sure to remove (' + application.ProjectName + ') from final voting?',
            autoClose: 'cancel|9000',
            buttons: {
                yes: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        application.IsSelectedForFinalVoting = false;
                        $http.get(root + 'api/VoteFunding/RemoveFromFinalVoting/' + application.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    body: 'Successfully done.',
                                });
                            }
                        }, function error(err) {
                            application.IsSelectedForFinalVoting = true;
                            toaster.pop({
                                type: 'error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.openUsersVoted = function (application) {
        if (application.UsersVotedCount > 0) {
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/voting/users-voted-nomination-template.html',
                controller: 'UsersVotedNominationCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    application: function () {
                        return application;
                    }
                }
            });
            modalInstance.result.then(function () {

            }, function (data) {

            });
        }
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});

myApp.controller('UsersVotedNominationCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, application) {
    $scope.application = application;

    $uibModalInstance.rendered.then(function () {
        $scope.getRecentVotes = function () {
            $http.get(root + 'api/VoteFunding/GetRecentVotes/' + application.Id).then(function success(response) {
                $scope.recentVotes = response.data;
                console.log('$scope.recentVotes', $scope.recentVotes);
            }, function error() { });
        }

        $scope.getRecentVotes();
    });


    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});

myApp.controller('ApproveResultsCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, $ngConfirm, nomination) {
    $scope.nomination = nomination;

    $scope.getNominationApplications = function () {
        $http.get(root + 'api/VoteFunding/GetAllFinalVotingProjects/' + nomination.Id).then(function success(response) {
            $scope.nominationApplications = response.data;
            console.log('$scope.nominationApplications', $scope.nominationApplications);
        }, function error() {
            $scope.nominationsNotFound = true;
        });
    }

    $uibModalInstance.rendered.then(function () {
        $scope.getNominationApplications();
    });

    $scope.makeWinner = function (application) {
        $ngConfirm({
            title: 'Make the project winner?',
            content: 'Are you sure to make (' + application.ProjectName + ') the winner ?',
            autoClose: 'cancel|9000',
            buttons: {
                yes: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        application.IsSelectedForFinalVoting = true;
                        $http.get(root + 'api/VoteFunding/MakeWinner/' + application.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    body: 'Successfully done.',
                                });
                                $scope.getNominationApplications();
                            }
                        }, function error(err) {
                            application.IsSelectedForFinalVoting = false;
                            toaster.pop({
                                type: 'error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.approveResults = function () {
        $ngConfirm({
            title: 'Approve results?',
            content: 'Are you sure to approve results for ' + nomination.Name + '? This action cannot be undone',
            autoClose: 'cancel|9000',
            buttons: {
                yes: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        nomination.IsResultApproved = true;
                        $http.put(root + 'api/Nominations/' + nomination.Id, nomination).then(function success(response) {
                            toaster.pop({
                                type: 'success',
                                body: 'Successfully done.',
                            });

                        }, function error(err) {
                            nomination.IsResultApproved = false;
                            toaster.pop({
                                type: 'error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.openUsersVoted = function (application) {
        if (application.UsersVotedCount > 0) {
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/voting/users-voted-final-template.html',
                controller: 'UsersVotedFinalCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    application: function () {
                        return application;
                    }
                }
            });
            modalInstance.result.then(function () {

            }, function (data) {

            });
        }
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});

myApp.controller('UsersVotedFinalCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, application) {
    $scope.application = application;

    $uibModalInstance.rendered.then(function () {
        $scope.getRecentVotes = function () {
            $http.get(root + 'api/VoteFunding/GetRecentFinalVotes/' + application.Id).then(function success(response) {
                $scope.recentVotes = response.data;
                console.log('$scope.recentVotes', $scope.recentVotes);
            }, function error() { });
        }

        $scope.getRecentVotes();
    });


    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }
});