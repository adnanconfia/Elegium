angular
    .module('xp-timepicker', [])
    .directive('xpTimepicker', timepicker)
    .directive('xpDatepicker', datepicker)
    .directive('sticky', sticky);

function sticky($timeout) {
    return function (scope, element, attrs) {
        $timeout(function () {
            var requestForm = document.getElementById('requestForm');
            new Datepair(requestForm);
        });
    };
}

function timepicker() {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            model: '=ngModel'
        },
        link: function ($scope, element, attrs, ngModelCtrl) {
            //console.log('change');
            element.timepicker({
                showDuration: true,
                timeFormat: 'g:i a'
            });
            element.on('changeTime', function () {
                $scope.$apply(function () {
                    $scope.model = element.val();
                });
            });

        }
    };
}

function datepicker() {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.datepicker({
                'format': 'dd/mm/yyyy',
                'autoclose': true
            });
        }
    };
}

//angular
//    .module('app', ['xp-timepicker'])
//    .controller('AppCtrl', function ($scope) {
//        $scope.time = '7:00pm';
//    });
