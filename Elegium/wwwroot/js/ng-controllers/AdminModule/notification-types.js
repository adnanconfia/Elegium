myApp.controller('notificationTypesCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.notificationTypes = [];

    $scope.getNotificationTypes = function () {
        $http.get(root + 'api/NotificationTypes/GetNotificationType').then(function success(response) {
            $scope.notificationTypes = response.data;
        }, function error(response) {
            console.log(response);
        });
    }

    $http.get(root + 'api/NotificationTypes/GetNotificationKind').then(function success(response) {
        $scope.notificationKind = response.data;
    }, function error(response) {
        console.log(response);
    });

    $scope.getNotificationTypes();

    $scope.addNotificationType = function () {
        $scope.notificationTypes.unshift({
            Id: uuidv4(),
            Name: '',
            Title: '',
            Template: '',
            Type: '',
            Action: 'I'
        });
    }

    function uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    $scope.deleteNotificationType = function (item) {
        if (item.Action == 'I')
            $scope.notificationTypes.splice($scope.notificationTypes.indexOf(item), 1);
        else
            item.Action = 'D';

        for (i = 0; i < $scope.notificationTypes.length; i++) {
            $scope.verifyDuplicate(i);
        }
    }

    $scope.verifyDuplicate = function (index) {
        var sorted, i, isDuplicate;
        sorted = $scope.notificationTypes.filter((subject) => subject.Action != 'D').concat().sort(function (a, b) {
            if (a.Type > b.Type) return 1;
            if (a.Type < b.Type) return -1;
            return 0;
        });
        for (i = 0; i < $scope.notificationTypes.filter((subject) => subject.Action != 'D').length; i++) {
           // if ($scope.notificationTypes[i].Action != 'D') {
                isDuplicate = ((sorted[i - 1] && sorted[i - 1].Type == sorted[i].Type) || (sorted[i + 1] && sorted[i + 1].Type == sorted[i].Type));
            $scope.userForm['NotificationType' + index].$setValidity('NotificationType' + index, !isDuplicate);
            //}
        }
    };

    $scope.Save = function () {
        $http.post(root + 'api/NotificationTypes/PostNotificationType', $scope.notificationTypes).then(function success(response) {
            if (response.data.success) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: response.data.Msg,
                });
                $scope.notificationTypes = response.data.List;
            } else {
                toaster.pop({
                    type: 'Error',
                    title: 'Error',
                    body: response.data.Msg,
                });
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }
});