myApp.controller('ProjectDashboardCtrl', function ($stateParams, $scope, $filter, $http, $uibModal, toaster, $state) {

    $scope.showSidePanel = false;
    $scope.dashboardNeedSave = false;

    $scope.projectId = $stateParams.id;


    $scope.getDashbaordPanels = function () {
        $http.get(root + 'api/ProjectDashboardPanels/GetDashboardPanels/' + $scope.projectId).then(function success(response) {

            var container = { items: [], effectAllowed: 'move' };
            angular.forEach(response.data, function (value, i) {
                container.items.push(
                    {
                        id: value.PanelId,
                        label: value.PanelLabel,
                        effectAllowed: value.EffectAllowed
                    }
                );
            });
            $scope.dashbaordPanels = container;
            console.log('$scope.dashbaordPanels:', $scope.dashbaordPanels);
        }, function error() { });
    }

    $scope.getUserDashbaordPanels = function () {
        $http.get(root + 'api/ProjectDashboardPanels/GetUserDashboardPanels/' + $scope.projectId).then(function success(response) {

            var container = { items: [], effectAllowed: 'move' };
            angular.forEach(response.data, function (value, i) {
                container.items.push(
                    {
                        id: value.PanelId,
                        label: value.PanelLabel,
                        effectAllowed: value.EffectAllowed
                    }
                );
            });

            $scope.userDashbaordPanels = container;
            $scope.userDashbaordPanels_Old = angular.copy($scope.userDashbaordPanels);

            console.log('$scope.userDashbaordPanels:', $scope.userDashbaordPanels);
        }, function error() { });
    }



    $scope.getProjectsForMenu = function () {
        $http.get(root + 'api/Projects/GetProjects').then(function success(response) {
            $scope.projectsListForMenu = response.data;
        }, function error() { });
    }

    $scope.getMyCreatedTasks = function () {
        $http.get(root + 'api/Tasks/GetMyCreatedTasks/' + $scope.projectId).then(resp => {
            $scope.myCreatedtasks = resp.data;
            console.log('$scope.mytasks:', $scope.mytasks);
        }, err => {
        });
    }

    $scope.getMyTasks = function () {
        $http.get(root + 'api/Tasks/GetMyTasks/' + $scope.projectId).then(resp => {
            $scope.mytasks = resp.data;
        }, err => {
        });
    }

    $scope.getDashboardStats = function () {
        $http.get(root + 'api/ProjectDashboardPanels/GetDashboardStats/' + $scope.projectId).then(resp => {
            console.log(resp.data);
            $scope.dashboardStats = resp.data;
        }, err => {
        });
    }

    $scope.getAllCrews = function () {
        $http.get(root + 'api/ProjectDashboardPanels/GetAllCrews/' + $scope.projectId).then(resp => {
            console.log(resp.data);
            $scope.allCrews = resp.data;
        }, err => {
        });
    }



    $scope.getDashbaordPanels();
    $scope.getUserDashbaordPanels();

    $scope.getProjectsForMenu();
    $scope.getMyCreatedTasks();
    $scope.getMyTasks();
    $scope.getDashboardStats();
    //$scope.getAllCrews();

    console.log('$scope.$parent.Top5Messages:', $scope.$parent.Top5Messages);


    //Other logical things here

    $scope.goToTaskProfile = function (taskId, projectId) {
        $state.go('tasks.mytasks.taskprofile', { id: projectId, taskId: taskId });
    }

    $scope.changeTaskStatus = function (event, task) {
        event.stopPropagation();
        $http.post(root + 'api/ProjectTasks/ChangeProjectTaskStatus/', task).then(resp => {
            var index = $scope.myCreatedtasks.indexOf(task);
            if (index > -1)
                $scope.myCreatedtasks[index] = resp.data;
        }, err => {
        });
    }

    $scope.deleteTask = function (event, task) {
        $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
            var index = $scope.myCreatedtasks.indexOf(task);
            if (index > -1)
                $scope.myCreatedtasks.splice(index, 1);
        }, err => {
        });
    }

    $scope.savePanelSettings = function () {
        var items = [];
        angular.forEach($scope.userDashbaordPanels.items, function (item, index) {
            items.push(
                {
                    PanelId: item.id,
                    PanelLabel: item.label,
                    Sort: index + 1,
                    ProjectId: Number($scope.projectId),
                    EffectAllowed: item.effectAllowed
                }
            );
        });

        $http.post(root + 'api/ProjectDashboardPanels/UpdateUserDashboardPanels/' + $scope.projectId, items).then(resp => {
            console.log(resp);
            if (resp.status == 200) {
                toaster.pop({
                    type: 'success',
                    body: 'Dashbaord settings saved.',
                });
                $scope.dashboardNeedSave = false;
                $scope.userDashbaordPanels_Old = angular.copy($scope.userDashbaordPanels);
            }
        }, err => {
        });

        console.log(items);
    }






    //Drag and drop panels logic below here

    $scope.deleteMeFromList = function (index) {
        var item = $scope.userDashbaordPanels.items[index];
        console.log(item);
        $scope.dashbaordPanels.items.push(
            {
                id: item.id,
                label: item.label,
                effectAllowed: item.effectAllowed
            }
        );
        $scope.userDashbaordPanels.items.splice(index, 1)

        console.log($scope.dashbaordPanels);
    }

    $scope.dragoverCallback = function (index, external, type, callback) {
        $scope.logListEvent('dragged over', index, external, type);
        // Invoke callback to origin for container types.
        if (type == 'container' && !external) {
            console.log('Container being dragged contains ' + callback() + ' items');
        }
        return index < 10; // Disallow dropping in the third row.
    };

    $scope.dropCallback = function (index, item, external, type) {
        $scope.logListEvent('dropped at', index, external, type);
        // Return false here to cancel drop. Return true if you insert the item yourself.
        return item;
    };

    $scope.logEvent = function (message) {
        console.log(message);
    };

    $scope.logListEvent = function (action, index, external, type) {
        var message = external ? 'External ' : '';
        message += type + ' element was ' + action + ' position ' + index;
        console.log(message);
    };

    
    $scope.$watch('userDashbaordPanels', function (model) {
        $scope.modelAsJson = angular.toJson(model,false);
        console.log('I am watching it:', $scope.modelAsJson);
        if (angular.toJson(model, false) != angular.toJson($scope.userDashbaordPanels_Old, false))
            $scope.dashboardNeedSave = true;
        else
            $scope.dashboardNeedSave = false;

    }, true);

    $scope.$watch('dashboardNeedSave', function (model) {
        if (model) {
            toaster.pop({
                type: 'info',
                body: 'Please save dashboard settings before leaving this page.',
            });
        }

    }, true);

    console.log('$scope.container:', $scope.container);

    //#region bilals work related to announcement

    $http.get(root + 'api/Announcements/GetUserWiseAnnouncements').then(function success(response) {
        $scope.announcements = response.data;
    }, function error() { });

    $scope.goToAnnouncementProfile = function (pId, aId) {
        $state.go('announcements.announcementprofile', { id: pId, announcementId: aId });
    }

    //#endregion
});