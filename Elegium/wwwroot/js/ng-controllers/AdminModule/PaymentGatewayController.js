myApp.controller('PaymentGatewayController', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm) {

    $scope.paymentGateways = [];

    $scope.getPaymentMethods = function () {
        $http.get(root + 'api/PaymentGateways').then(function success(response) {
            $scope.paymentGateways = response.data;
        }, function error(response) {
            console.log(response);
        });
    }

    //public Guid Id { get; set; }
    //    public string ApiSecret { get; set; }
    //    public bool Default { get; set; }
    //    public string ApiKey { get; set; }
    //    public string Provider { get; set; }

    $scope.PaymentProviders = [
        { Id: 1, Name: "Stripe" },
        { Id: 2, Name: "2Checkout" }
    ];

    $scope.getPaymentMethods();

    $scope.addPaymentMethod = function () {
        console.log($scope.paymentGateways);
        $scope.paymentGateways.unshift({
            Id: uuidv4(),
            ApiSecret: '',
            Default: false,
            ApiKey: '',
            Provider: '',
            Action: 'I'
        });
    }

    function uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    $scope.deletePaymentMethod = function (item) {
        if (item.Action == 'I')
            $scope.paymentGateways.splice($scope.paymentGateways.indexOf(item), 1);
        else
            item.Action = 'D';

        for (i = 0; i < $scope.paymentGateways.length; i++) {
            $scope.verifyDuplicate(i);
        }
    }

    $scope.verifyDuplicate = function (index) {
        var sorted, i, isDuplicate;
        sorted = $scope.paymentGateways.filter((subject) => subject.Action != 'D').concat().sort(function (a, b) {
            if (a.Provider > b.Provider) return 1;
            if (a.Provider < b.Provider) return -1;
            return 0;
        });
        for (i = 0; i < $scope.paymentGateways.filter((subject) => subject.Action != 'D').length; i++) {
           // if ($scope.paymentGateways[i].Action != 'D') {
                isDuplicate = ((sorted[i - 1] && sorted[i - 1].Provider == sorted[i].Provider) || (sorted[i + 1] && sorted[i + 1].Provider == sorted[i].Provider));
                $scope.userForm['Provider' + index].$setValidity('Provider' + index, !isDuplicate);
            //}
        }
    };

    $scope.Save = function () {
        $http.post(root + 'api/PaymentGateways', $scope.paymentGateways).then(function success(response) {
            if (response.data.success) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: response.data.Msg,
                });
                $scope.paymentGateways = response.data.List;
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