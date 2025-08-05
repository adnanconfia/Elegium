(function () {
    "use strict";
    angular.module('ngIntlTelInput', [])
        .provider('ngIntlTelInput', function () {
            var me = this;
            var props = {};
            var setFn = function (obj) {
                if (typeof obj === 'object') {
                    for (var key in obj) {
                        props[key] = obj[key];
                    }
                }
            };
            me.set = setFn;

            me.$get = ['$log', function ($log) {
                return Object.create(me, {
                    init: {
                        value: function (elm) {
                            if (!window.intlTelInputUtils) {
                                $log.warn('intlTelInputUtils is not defined. Formatting and validation will not work.');
                            }
                            elm.intlTelInput(props);
                        }
                    },
                });
            }];
        }).directive('ngIntlTelInput', ['ngIntlTelInput', '$log',
            function (ngIntlTelInput, $log) {
                return {
                    restrict: 'A',
                    require: 'ngModel',
                    link: function (scope, elm, attr, ctrl) {
                        // Warning for bad directive usage.
                        if (attr.type !== 'text' || elm[0].tagName !== 'INPUT') {
                            $log.warn('ng-intl-tel-input can only be applied to a *text* input');
                            return;
                        }
                        // Override default country.
                        if (attr.defaultCountry) {
                            ngIntlTelInput.set({ defaultCountry: attr.defaultCountry });
                        }
                        // Initialize.
                        ngIntlTelInput.init(elm);
                        // Validation.
                        ctrl.$validators.ngIntlTelInput = function (value) {
                            return elm.intlTelInput("isValidNumber");
                        };
                        // Set model value to valid, formatted version.
                        ctrl.$parsers.push(function (value) {
                            return elm.intlTelInput('getNumber').replace(/[^\d]/, '');
                        });
                        // Set input value to model value and trigger evaluation.
                        ctrl.$formatters.push(function (value) {
                            if (value) {
                                value = value.charAt(0) === '+' || '+' + value;
                                elm.intlTelInput('setNumber', value);
                            }
                            return value;
                        });
                    }
                };
            }]);

}).call(this);
