myApp.controller('UnitController', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams, $state) {
    console.log('unitid:', $stateParams.unitId);
    console.log('$stateParams', $stateParams);
    $scope.unitId = $stateParams.unitId;
    $scope.projectId = $stateParams.id;
    $scope.users = [];
    $scope.allUsers = [];
    $scope.isEditModeUser = false;
    $scope.addUser = function () {
        $scope.isEditModeUser = true;
    }
    $scope.title = $stateParams.title;
    $state.$current.data.label = $stateParams.title;
    $scope.closeEditMode = function () {
        $scope.isEditModeUser = false;

    }
    $scope.addUserToUnit = function (record) {
      
        
        if (record.IsCrewUserUnit == false) {
        
            $http.post(`${root}api/ProjectUnits/AddUserToUnit1?userId=${record.Id}&&unitId=${$scope.unitId}`).then(function success(response) {
                //$scope.crewList = response.data.data;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'User added successfully!',
                });
                $scope.getUnitUsers();
                $scope.getAllUsers();
            }, function error() { });
        }
       
    }
    $scope.getUnitUsers = function () {
        $http.get(root + 'api/ProjectUnits/GetUnitUsers?unitId=' + $scope.unitId).then(function success(response) {
            $scope.users = response.data;
            console.log('$scope.projectUnits', response.data,$scope.users);
        }, function error() { });
    }
    $scope.getAllUsers = function () {
        $http.get(`${root}api/ProjectUnits/GetAllUsers?unitId=${$scope.unitId}&&projectId=${$scope.projectId}`).then(function success(response) {
            $scope.allUsers = response.data;
            console.log(' $scope.getAllUsers', response.data,$scope.allUsers);
        }, function error() { });
    }
    $scope.deleteUnitUser = function (record) {
        $http.delete(root + 'api/ProjectUnits/DeleteUnitUsers/' + record.Id).then(function success(response) {
            //$scope.crewList = response.data.data;
            toaster.pop({
                type: 'success',
                title: 'Success',
                body: 'User deleted successfully!',
            });
            $scope.getUnitUsers();
            $scope.getAllUsers();
        }, function error() { });
    }
    $scope.getUnitUsers();
    $scope.getAllUsers();
    //code for external
    $scope.isEditModeExternalUser = false;

    $scope.externalUsers = [];
    $scope.allExternalUsers = [];
    $scope.addExternalUser = function () {
        $scope.isEditModeExternalUser = true;
    }
    $scope.closeEditModeExternalUser = function () {
        $scope.isEditModeExternalUser = false;

    }
    $scope.addExternalUserToUnit = function (record) {


        if (record.IsExternalUserUnit == false) {

            $http.post(`${root}api/ProjectUnits/AddExternalUserToUnit?userId=${record.Id}&&unitId=${$scope.unitId}`).then(function success(response) {
                //$scope.crewList = response.data.data;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'User added successfully!',
                });
                $scope.getUnitExternalUsers();
                $scope.getAllExternalUsers();
            }, function error() { });
        }

    }
    $scope.getUnitExternalUsers = function () {
        $http.get(root + 'api/ProjectUnits/GetUnitExternalUsers?unitId=' + $scope.unitId).then(function success(response) {
            $scope.externalUsers = response.data;
            console.log('$scope.projectUnits', response.data, $scope.externalUsers);
        }, function error() { });
    }
    $scope.getAllExternalUsers = function () {
        $http.get(`${root}api/ProjectUnits/GetAllExternalUsers?unitId=${$scope.unitId}&&projectId=${$scope.projectId}`).then(function success(response) {
            $scope.allExternalUsers = response.data;
            console.log(' $scope.allExternalUsers', response.data, $scope.allExternalUsers);
        }, function error() { });
    }
    $scope.deleteUnitExternalUser = function (record) {
        $http.delete(root + 'api/ProjectUnits/DeleteUnitExternalUsers/' + record.Id).then(function success(response) {
            //$scope.crewList = response.data.data;
            toaster.pop({
                type: 'success',
                title: 'Success',
                body: 'User deleted successfully!',
            });
            $scope.getUnitExternalUsers();
            $scope.getAllExternalUsers();
        }, function error() { });
    }
    $scope.getUnitExternalUsers();
    $scope.getAllExternalUsers();
});