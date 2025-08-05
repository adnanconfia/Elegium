myApp.controller('CrewController', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams) {

    var projectId = parseInt($stateParams.id);
    console.log($stateParams.id, 'saeed');
    var modalInstance = null;




    // $scope.saad = {name:'saad'};


    //Start Code for User Group
    $scope.isEditModeUserGroup = false;
    $scope.projectUserGroups = [];
    $scope.projectUserGroup = { Name: '', ProjectId: projectId };
    $scope.createUserGroup = function () {
        $scope.projectUserGroup = { Name: '', ProjectId: projectId };
        $scope.isEditModeUserGroup = true;
        setTimeout(function () { $("#txtUserGroupName").focus(); }, 100);
    }
    $scope.getProjectUserGroups = function () {
        $http.get(root + 'api/ProjectUserGroups/GetUserGroups?ProjectId=' + projectId).then(function success(response) {
            $scope.projectUserGroups = response.data;
            console.log('$scope.projectUnits', $scope.projectUnits);
        }, function error() { });
    }
    $scope.saveGroupName = function () {
        if ($scope.projectUserGroup.Name) {
            $scope.projectUserGroup.IsInCrewList = true;
            $http.post(root + 'api/ProjectUserGroups/SaveorUpdateProjectUserGroup', $scope.projectUserGroup).then(function success(response) {
                if (response.status == 200) {
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'User Group saved successfully!',
                    });
                    $scope.getProjectUserGroups();
                    $rootScope.getAllCrews();
                    $scope.isEditModeUserGroup = false;
                }
            }, function error(err) {
                console.log(err);
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: err.data,
                });
            });
        }
        else {
            $scope.isEditModeUserGroup = false;
        }

    }
    $scope.deleteProjectUserGroup = function (record) {
        $http.delete(root + 'api/ProjectUserGroups/DeleteProjectUserGroups/' + record.Id).then(function success(response) {
            //$scope.crewList = response.data.data;
            toaster.pop({
                type: 'success',
                title: 'Success',
                body: 'User Group deleted successfully!',
            });
            $scope.userGroup = response.data;//Temp
            $scope.getProjectUserGroups();
            $rootScope.getAllCrews();
            console.log('$scope.userGroup', response);
        }, function error() { });
    }
    $scope.editModeProjectUserGroup = function (record) {
        record.IsEdit = true;
        setTimeout(function () { $("#txtUserGroupName_" + record.Id).focus(); }, 100);
    }
    $scope.cancelEditProjectUserGroup = function (record) {
        $scope.getProjectUserGroups();
    }
    $scope.editProjectUserGroup = function (record) {
        if (record.Name) {

            $http.post(root + 'api/ProjectUserGroups/UpdateProjectUserGroup', record).then(function success(response) {
                if (response.status == 200) {
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'User Group updated successfully!',
                    });
                    $scope.getProjectUserGroups();
                    $rootScope.getAllCrews();
                    record.IsEdit = false;
                }
            }, function error(err) {
                console.log(err);
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: err.data,
                });
            });
        }
        else {
            $scope.getProjectUserGroups();
            record.IsEdit = false;
        }
    }
    //End Code for User Group
    //start Code for Project Units
    $scope.isEditProjectUnit = false;
    $scope.projectUnit = { Name: '', ProjectId: projectId };
    $scope.projectUnits = [];
    $scope.createProjectUnit = function () {
        $scope.projectUnit.Name = '';
        $scope.isEditProjectUnit = true;

        setTimeout(function () { $("#txtUnitName").focus(); }, 100);

    }
    $scope.openProjectUnits = function (record) {
        console.log(record);
        //modalInstance = $uibModal.open({
        //    animation: false,
        //    templateUrl: root + 'js/ng-templates/crew/project-units-template.html',
        //    controller: 'ProjectUnitsCtrl',
        //    size: 'lg',
        //    backdrop: 'static',
        //    resolve: {
        //        title: function () {
        //            return record.Name;
        //        },
        //        projectItem: function () {
        //            return null;
        //        }
        //    }
        //});
        //modalInstance.result.then(function () {

        //}, function (data) {

        //});
    }
    $scope.getProjectUnits = function () {
        $http.get(root + 'api/ProjectUnits/GetProjectUnits?ProjectId=' + projectId).then(function success(response) {
            $scope.projectUnits = response.data;
            console.log($scope.projectUnits);
        }, function error() { });
    }
    $scope.saveProjectUnit = function () {
        if ($scope.projectUnit.Name) {

            $http.post(root + 'api/ProjectUnits/SaveorUpdateProjectUnit', $scope.projectUnit).then(function success(response) {
                if (response.status == 200) {
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'Unit saved successfully!',
                    });
                    $scope.getProjectUnits();
                    $scope.isEditProjectUnit = false;
                }
            }, function error(err) {
                console.log(err);
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: err.data,
                });
            });
        }
        else {
            $scope.isEditProjectUnit = false;
        }
    }
    $scope.deleteProjectUnit = function (record) {
        $http.delete(root + 'api/ProjectUnits/DeleteProjectUnit/' + record.Id).then(function success(response) {
            //$scope.crewList = response.data.data;

            $scope.getProjectUnits();
            console.log('$scope.userGroup', response);
        }, function error() { });
    }
    $scope.editModeProjectUnit = function (record) {
        record.IsEdit = true;

        setTimeout(function () { $("#txtUnitName_" + record.Id).focus(); }, 100);

    }
    $scope.canceleditProjectUnit = function (record) {
        $scope.getProjectUnits();
    }
    $scope.editProjectUnit = function (record) {
        if (record.Name) {

            $http.post(root + 'api/ProjectUnits/UpdateProjectUnit', record).then(function success(response) {
                if (response.status == 200) {
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'Unit updated successfully!',
                    });
                    $scope.getProjectUnits();
                    record.IsEdit = false;
                }
            }, function error(err) {
                console.log(err);
                toaster.pop({
                    type: 'error',
                    title: 'Error',
                    body: err.data,
                });
            });
        }
        else {
            record.IsEdit = false;
        }
    }
    //End code for project Unit

    //Start code for Access Rights
    $rootScope.updateRights = function (name, projectCrew) {
        $rootScope.changeRights(name, projectCrew);
        $scope.updateAccessRights(projectCrew);
    }
    $scope.updateAccessRights = function (crew) {

        $http.post(root + 'api/ProjectCrews/UpdateProjectCrew', crew).then(function success(response) {
            if (response.status == 200) {
                //toaster.pop({
                //    type: 'success',
                //    title: 'Success',
                //    body: 'Rights Updated',
                //});


            }
        }, function error(err) {
            console.log(err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }

    //Start code for External Contacts
    $scope.externalUsers = [];
    $scope.externalUser = {};
    $rootScope.getExternalUsers = function () {
        $http.get(root + 'api/ProjectExternalUsers/GetProjectExternalUsers?ProjectId=' + projectId).then(function success(response) {
            $scope.externalUsers = response.data;


            // $scope.userGroup = response.data.data;//Temp
            console.log('externalUsers:', $scope.externalUsers);
        }, function error() { });
    }

    $scope.createExternalUser = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/crew/create-external-user-template.html',
            controller: 'CreateExternalUserCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'External Contact';
                },
                projectItem: function () {
                    return null;
                },
                projectId: function () {
                    return projectId;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }
    $scope.deleteExternalContact = function (record) {
        console.log(record);
        $http.delete(root + 'api/ProjectExternalUsers/DeleteExternalUser/' + record.externalUser.Id).then(function success(response) {
            //$scope.crewList = response.data.data;

            $rootScope.getExternalUsers();
            console.log('$scope.userGroup', response);
        }, function error() { });
    }
    //End code for External User
    //Start code for project Crew
    $scope.crewList = [];
    $scope.allCrewList = [];
    $rootScope.getAllCrews = function () {
        $http.get(root + 'api/ProjectCrews/GetAllProjectCrews?ProjectId=' + projectId).then(function success(response) {
           
            $scope.allCrewList = response.data;
            console.log('$rootScope.getAllCrews', $scope.allCrewList);
            // $scope.userGroup = response.data.data;//Temp
            console.log($scope.crewList);
        }, function error(err) {

            console.log('$scope.crewList', err);
        });
    }
    $rootScope.getCrews = function () {
        $http.get(root + 'api/ProjectCrews/GetProjectCrews?ProjectId=' + projectId).then(function success(response) {
            console.log('$scope.crewList', response);
            $scope.crewList = response.data;

            // $scope.userGroup = response.data.data;//Temp
            console.log($scope.crewList);
        }, function error(err) {

            console.log('$scope.crewList', err);
        });
    }
    $rootScope.getAllCrews();
    $rootScope.getCrews();
    $scope.getProjectUnits();
    $scope.getProjectUserGroups();
    $rootScope.getExternalUsers();

    $scope.deleteCrew = function (record) {
        record.IsActive = false;
        $http.post(root + 'api/ProjectCrews/UpdateProjectCrew', record).then(function success(response) {
            //$scope.crewList = response.data.data;

            $rootScope.getAllCrews();
            $rootScope.getCrews();
            $rootScope.GetOnBoarding();
            //console.log('$scope.userGroup', response);
        }, function error() { });
    }

    $scope.openInviteCrewUser = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/crew/invite-crew-user-template.html',
            controller: 'InviteCrewUserCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Invite User';
                },
                projectItem: function () {
                    return null;
                },
                projectId: function () {
                    return projectId;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }

    //$scope.changeRights = function (user,name, value) {

    //    if (name == 'CrewRights') {
    //        user.CrewRights = $rootScope.toggleRights(value);
    //    } else if (name == 'AnnouncementRights') {
    //        user.AnnouncementRights = $rootScope.toggleRights(value);
    //    } else if (name == 'AddressBookRights') {
    //        user.AddressBookRights = $rootScope.toggleRights(value);
    //    } else if (name == 'CompanyCalendarRights') {
    //        user.CompanyCalendarRights = $rootScope.toggleRights(value);
    //    } else if (name == 'ProductionCalendarRights') {
    //        user.ProductionCalendarRights = $rootScope.toggleRights(value);
    //    } else if (name == 'ActorDatabaseRights') {
    //        user.ActorDatabaseRights = $rootScope.toggleRights(value);
    //    } else if (name == 'LocationDatabaseRights') {
    //        user.LocationDatabaseRights = $rootScope.toggleRights(value);
    //    } else if (name == 'CostumeDatabaseRights') {
    //        user.CostumeDatabaseRights = $rootScope.toggleRights(value);
    //    } else if (name == 'ProbItemDatabaseRights') {
    //        user.ProbItemDatabaseRights = $rootScope.toggleRights(value);
    //    } else if (name == 'CreateDeleteProjectsRights') {
    //        user.CreateDeleteProjectsRights = $rootScope.toggleRights(value);
    //    } else if (name == 'SubscriptionsRights') {
    //        user.SubscriptionsRights = $rootScope.toggleRights(value);
    //    } else if (name == 'SettingsRights') {
    //        user.SettingsRights = $rootScope.toggleRights(value);
    //    }

    //}

    $rootScope.changeRights = function (name, projectCrew) {
        console.log('projectCrew', projectCrew);
        if (name == 'CrewRights') {
            projectCrew.CrewRights = $rootScope.toggleRights(projectCrew.CrewRights);
        } else if (name == 'AnnouncementRights') {
            projectCrew.AnnouncementRights = $rootScope.toggleRights(projectCrew.AnnouncementRights);
        } else if (name == 'AddressBookRights') {
            projectCrew.AddressBookRights = $rootScope.toggleRights(projectCrew.AddressBookRights);
        } else if (name == 'CompanyCalendarRights') {
            projectCrew.CompanyCalendarRights = $rootScope.toggleRights(projectCrew.CompanyCalendarRights);
        } else if (name == 'ProductionCalendarRights') {
            projectCrew.ProductionCalendarRights = $rootScope.toggleRights(projectCrew.ProductionCalendarRights);
        } else if (name == 'ActorDatabaseRights') {
            projectCrew.ActorDatabaseRights = $rootScope.toggleRights(projectCrew.ActorDatabaseRights);
        } else if (name == 'LocationDatabaseRights') {
            projectCrew.LocationDatabaseRights = $rootScope.toggleRights(projectCrew.LocationDatabaseRights);
        } else if (name == 'CostumeDatabaseRights') {
            projectCrew.CostumeDatabaseRights = $rootScope.toggleRights(projectCrew.CostumeDatabaseRights);
        } else if (name == 'ProbItemDatabaseRights') {
            projectCrew.ProbItemDatabaseRights = $rootScope.toggleRights(projectCrew.ProbItemDatabaseRights);
        } else if (name == 'CreateDeleteProjectsRights') {
            projectCrew.CreateDeleteProjectsRights = $rootScope.toggleRights(projectCrew.CreateDeleteProjectsRights);
        } else if (name == 'SubscriptionsRights') {
            projectCrew.SubscriptionsRights = $rootScope.toggleRights(projectCrew.SubscriptionsRights);
        } else if (name == 'SettingsRights') {
            projectCrew.SettingsRights = $rootScope.toggleRights(projectCrew.SettingsRights);
        }

    }
    $rootScope.toggleRights = function (rights) {
        if (rights == 'V')
            return 'E';
        else if (rights == 'E')
            return 'L';
        else
            return 'V';
    }

    $rootScope.classRights = function (rights) {
        if (rights == 'V')
            return 'fa fa-eye';
        else if (rights == 'E')
            return 'fa fa-pencil-square-o';
        else
            return 'fa fa-lock';
    }
    $rootScope.userRightsCls = function (rights) {

        if (rights == 'V')
            return '';
        else if (rights == 'E')
            return '';
        else
            return 'low-opacity';
    }

});
//Invite user
myApp.controller('InviteCrewUserCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, title, projectItem, ProjectService, projectId) {

    $scope.title = title;
    $scope.workingPositions = [];
    $scope.units = [];
    $scope.data = { crewPositons: null, crewUnits: null };
    $scope.projectCrew = { ProjectId: projectId };
    $scope.user = {};
    $scope.save = function () {
        // return;

        $scope.projectCrew.ProjectId = projectId;
        $scope.ProjectCrewModel = {
            ProjectCrew: $scope.projectCrew,
            UserProfile: $scope.user,
            ProjectCrewPositions: $scope.crewPositons,
            ProjectCrewUnits: $scope.data.crewUnits
        }
        console.log($scope.ProjectCrewModel);
        // return;
        $http.post(root + 'api/ProjectCrews/SaveOrUpdateProjectCrew', $scope.ProjectCrewModel).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Project saved successfully!',
                });
                $rootScope.getAllCrews();
                $rootScope.getCrews();
                $rootScope.GetOnBoarding();
                console.log('response', response);
                $scope.cancel();
            }
        }, function error(err) {
            console.log('err', err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }

    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
        }, function error() { });
    }
    $scope.getUnits = function () {
        $http.get(root + 'api/Units/GetUnits?projectId=' + projectId).then(function success(response) {
            console.log('response', response);
            $scope.units = response.data;
        }, function error() { });
    }

    $scope.getWorkingPositions();
    $scope.getUnits();




});


myApp.controller('CreateExternalUserCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, title, projectItem, ProjectService, projectId) {

    $scope.title = title;
    $scope.workingPositions = [];
    $scope.units = [];
    $scope.data = { crewPositons: null, crewUnits: null };
    $scope.projectExternalUser =    { ProjectId: projectId };
    $scope.user = {};
    //projectId
    $scope.save = function () {
        // return;
        $scope.projectExternalUser.ProjectId = projectId;
        console.log($scope.ProjectCrewModel);
        // return;
        $http.post(root + 'api/ProjectExternalUsers/SaveorUpdateExternalUser', $scope.projectExternalUser).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'External Contact Added successfully!',
                });
                $rootScope.getExternalUsers();
                $scope.cancel();
            }
        }, function error(err) {
            console.log(err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }


    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
        }, function error() { });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }



    $scope.getWorkingPositions();

});

myApp.controller('ProjectUnitsCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $http, toaster, title, projectItem, ProjectService) {

    $scope.title = title;
    $scope.workingPositions = [];
    $scope.units = [];
    $scope.data = { crewPositons: null, crewUnits: null };
    $scope.projectExternalUser = { ProjectId: 4 };
    $scope.user = {};
    $scope.save = function () {
        // return;
        $scope.projectExternalUser.ProjectId = 4;
        console.log($scope.ProjectCrewModel);
        // return;
        $http.post(root + 'api/ProjectExternalUsers/SaveorUpdateExternalUser', $scope.projectExternalUser).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'External Contact Added successfully!',
                });
                $rootScope.getExternalUsers();
                $scope.cancel();
            }
        }, function error(err) {
            console.log(err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }



});