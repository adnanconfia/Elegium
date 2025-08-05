myApp.controller('GroupController', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams, $state) {

    $scope.groupId = $stateParams.groupId;
    $scope.projectId = $stateParams.id;

    $http.get(`${root}api/ProjectUserGroups/GetProjectGroup/${$scope.groupId}`).then(function success(response) {
        $state.$current.data.label = response.data.Name;
    }, function error() { });

   

    $scope.getAllUsers = function () {
        $http.get(`${root}api/ProjectCrewGroups/GetAllUsers?groupId=${$scope.groupId}&&projectId=${$scope.projectId}`).then(function success(response) {
            $scope.allUsers = response.data;
        }, function error() { });
    }

    $scope.getGroupUsers = function () {
        $http.get(root + 'api/ProjectCrewGroups/GetGroupUsers?groupId=' + $scope.groupId).then(function success(response) {
            $scope.users = response.data;
        }, function error() { });
    }

    $scope.addUserToGroup = function (record) {
        if (record.IsCrewUserGroup == false) {
            $http.post(`${root}api/ProjectCrewGroups/AddUserToGroup?userId=${record.Id}&&groupId=${$scope.groupId}`).then(function success(response) {
                $scope.getGroupUsers();
                $scope.getAllUsers();
            }, function error() { });
        }
    }

    $scope.deleteGroupUser = function (record) {
        $http.delete(root + 'api/ProjectCrewGroups/DeleteGroupUser/' + record.Id).then(function success(response) {
            $scope.getGroupUsers();
            $scope.getAllUsers();
        }, function error() { });
    }

    $scope.users = [];
    $scope.allUsers = [];

    $scope.title = $stateParams.title;
    $scope.getAllUsers();
    $scope.getGroupUsers();
    //code for external

    $scope.externalUsers = [];
    $scope.allExternalUsers = [];


    $scope.addExternalUserToGroup = function (record) {


        if (record.IsExternalUserGroup == false) {

            $http.post(`${root}api/ProjectCrewGroups/AddExternalUserToGroup?userId=${record.Id}&&groupId=${$scope.groupId}`).then(function success(response) {
                
                $scope.getGroupExternalUsers();
                $scope.getAllExternalUsers();
            }, function error() { });
        }

    }
    $scope.getGroupExternalUsers = function () {
        $http.get(root + 'api/ProjectCrewGroups/GetGroupExternalUsers?groupId=' + $scope.groupId).then(function success(response) {
            $scope.externalUsers = response.data;
        }, function error() { });
    }
    $scope.getAllExternalUsers = function () {
        $http.get(`${root}api/ProjectCrewGroups/GetAllExternalUsers?groupId=${$scope.groupId}&&projectId=${$scope.projectId}`).then(function success(response) {
            $scope.allExternalUsers = response.data;
            console.log(' $scope.allExternalUsers', response.data, $scope.allExternalUsers);
        }, function error() { });
    }
    $scope.deleteGroupExternalUser = function (record) {
        $http.delete(root + 'api/ProjectCrewGroups/DeleteGroupExternalUsers/' + record.Id).then(function success(response) {
            $scope.getGroupExternalUsers();
            $scope.getAllExternalUsers();
        }, function error() { });
    }
    $scope.getGroupExternalUsers();
    $scope.getAllExternalUsers();
});