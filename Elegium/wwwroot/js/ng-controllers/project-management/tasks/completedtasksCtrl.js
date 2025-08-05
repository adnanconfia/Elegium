myApp.controller('completedtasksCtrl', function ($stateParams, $scope, $http, $state, $window, $window, $uibModal, $timeout, MenuService) {

    $scope.projectId = $stateParams.id;

    var initializeTaskObj = function () {
        $scope.taskObj = {
            AssignedTo: [],
            Description: "",
            Title: "",
            HasDeadline: false,
            LinkedToSection: false,
            DocumentCategoryId: null,
            Section: null,
            Deadline: "",
            Id: 0,
            ProjectId: parseInt($stateParams.id),
            Section: "",
            HasSection: false,
            SectionUrl: "",
            Section: ""
        }
    }

    initializeTaskObj();

    $scope.mytasks = [];

    $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + $stateParams.id).then(resp => {
        $scope.projectUsersAndGroups = resp.data;
    }, err => {
    });

    $scope.groupUsers = function (item) {
        return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
    }

    $http.get(root + 'api/Tasks/GetMyCompletedTasks/' + $stateParams.id).then(resp => {
        $scope.mytasks = resp.data;

        $http.get(root + 'api/Documents/GetProjectWiseUserMenu/' + $stateParams.id).then(function success(response) {
            $scope.sections = response.data.projectUserMenu;
        }, function error() {
        });

        $http.get(root + 'api/Tasks/GetProjectObjects/' + $stateParams.id).then(function success(response) {
            $scope.objects = response.data;
        }, function error() {
        });

    }, err => {
    });

    $scope.createTask = function () {
        $http.post(root + 'api/ProjectTasks/PostProjectTask/', $scope.taskObj).then(resp => {
            initializeTaskObj();
            $scope.mytasks.push(resp.data);
        }, err => {
        });
    }

    $scope.goToTaskProfile = function (taskId) {
        $state.go('tasks.mytasks.taskprofile', { taskId: taskId });
    }

    $scope.cancelTaskCreation = function () {
        if ($scope.taskObj.HasDeadline)
            $('#toggleCalendarDeadline').trigger('click');

        if ($scope.taskObj.HasSection)
            $('#section-selector').removeClass('show');

        initializeTaskObj();
    }

    $scope.changeTaskStatus = function (event, task) {
        event.stopPropagation();
        $http.post(root + 'api/ProjectTasks/ChangeProjectTaskStatus/', task).then(resp => {
            var index = $scope.mytasks.indexOf(task);
            if (index > -1)
                $scope.mytasks[index] = resp.data;
        }, err => {
        });
    }

    $scope.deleteTask = function (event, task) {
        //event.stopPropagation();
        $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
            var index = $scope.mytasks.indexOf(task);
            if (index > -1)
                $scope.mytasks.splice(index, 1);
        }, err => {
        });
    }
});