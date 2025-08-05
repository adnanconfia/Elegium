var myApp = angular.module('myApp', [
    'ui.bootstrap',
    'ui.calendar',
    'ngSanitize',
    'ui.select',
    'angularCroppie',
    'color.picker',
    'ngPatternRestrict',
    'toaster',
    'ngAnimate',
    'cp.ngConfirm',
    'ngPatternCheck',
    'SignalR',
    'angular-toArrayFilter',
    'ui.router',
    'uiBreadcrumbs',
    'rx',
    'angular.filter',
    'angular.chosen',
    'ui.router.state.events',
    'dndLists',
    'angularMoment',
    'xp-timepicker',
    'angular-svg-round-progressbar',
    'ngIntlTelInput'
])
    .directive('dragMe', dragMe)
    .directive('dropOnMe', function ($http) { return dropOnMe($http) })
    .directive('copyToClipboard', function () {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                elem.click(function () {
                    if (attrs.copyToClipboard) {
                        var $temp_input = $("<input>");
                        $("body").append($temp_input);
                        $temp_input.val(attrs.copyToClipboard).select();
                        document.execCommand("copy");
                        $temp_input.remove();
                        gritterAlert("", 'Copied to clipboard!', '');
                    }
                });
            }
        };
    }).directive('dateFormat', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                //Angular 1.3 insert a formater that force to set model to date object, otherwise throw exception.
                //Reset default angular formatters/parsers
                ngModelCtrl.$formatters.length = 0;
                ngModelCtrl.$parsers.length = 0;
            }
        };
    })
    .directive('httpRequestLoader', ['$http', function ($http) {
        return {
            restrict: 'E',
            template: '<div class="loading-icon"></div>',
            link: function (scope, element, attrs) {

                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };
                scope.$watch(scope.isLoading, function (value) {
                    if (value)
                        element.removeClass('ng-hide');
                    else
                        element.addClass('ng-hide');
                });
            }
        }
    }]).directive('equal', [
        function () {

            var link = function ($scope, $element, $attrs, ctrl) {

                var validate = function (viewValue) {
                    var comparisonModel = $attrs.equal;
                    console.log(viewValue + ':' + comparisonModel);

                    if (!viewValue || !comparisonModel) {
                        // It's valid because we have nothing to compare against
                        ctrl.$setValidity('equal', true);
                    }

                    // It's valid if model is lower than the model we're comparing against
                    console.log(viewValue === comparisonModel);
                    ctrl.$setValidity('equal', viewValue === comparisonModel);
                    return viewValue;
                };

                ctrl.$parsers.unshift(validate);
                ctrl.$formatters.push(validate);

                $attrs.$observe('equal', function (comparisonModel) {
                    return validate(ctrl.$viewValue);
                });

            };

            return {
                require: 'ngModel',
                link: link
            };
        }]).directive('scrollToBottom', function ($timeout, $window) {
            return {
                scope: {
                    scrollToBottom: "="
                },
                restrict: 'A',
                link: function (scope, element, attr) {
                    scope.$watchCollection('scrollToBottom', function (newVal, oldVal) {
                        console.log(newVal, oldVal);
                        if ((newVal === oldVal && newVal.hasNewMessage) || (newVal !== oldVal && newVal.hasNewMessage)) {
                            element[0].scrollTop = element[0].scrollHeight;
                        } else {
                            console.log((newVal !== oldVal && newVal.hasNewMessage), (newVal === oldVal && newVal.hasNewMessage))
                        }
                        // console.log(newVal, oldVal);
                        //if (attr.hasNewMessage) {
                        //    element[0].scrollTop = element[0].scrollHeight;
                        //    //if (newVal.hasNewMessage) {
                        //    //    $timeout(function () {
                        //    //        element[0].scrollTop = element[0].scrollHeight;
                        //    //    }, 50);
                        //    //}
                        //} else {
                        //    element[0].scrollTop = 50;
                        //}
                    }, function (val) {
                        console.log(val, 'bilal');
                    }, true);
                }
            };
        }).directive('imgPreload', ['$rootScope', function ($rootScope) {
            return {
                scope: {
                    imgSrc: "="
                },
                restrict: 'A',
                link: function (scope, element, attrs) {
                    scope.$watch('imgSrc', function (oldVal, newVal) {
                        var img = element.find('img')[0];

                        if (element.find('img')[1]) {
                            element.find('img')[1].remove();
                        }
                        img.classList.remove('ng-hide');
                        var fileUrl = root + 'api/UserProfiles/GetFile/' + scope.imgSrc.FileId + '/1000/500';
                        var imgLarge = new Image();
                        imgLarge.src = fileUrl;
                        imgLarge.onload = function () {
                            imgLarge.classList.add('loaded');
                            img.classList.add('ng-hide');
                        };
                        element[0].appendChild(imgLarge);
                    });
                }
            };
        }]).directive('whenScrolledBottom', function ($timeout, $window) {
            return function (scope, elm, attr) {
                var raw = elm[0];
                elm.bind('scroll', function () {
                    console.log(raw.scrollTop, raw.offsetHeight, raw.scrollHeight);
                    if (Math.ceil(raw.scrollTop + raw.offsetHeight) >= raw.scrollHeight) {
                        scope.$apply(attr.whenScrolledBottom).then(function () {
                            raw.scrollTop = raw.scrollHeight - raw.offsetHeight;
                        }, (error) => {
                            console.log('error');
                        });
                    }
                });
            };
        }).directive('whenScrolled', ['$timeout', function ($timeout) {
            return function (scope, elm, attr) {
                var raw = elm[0];
                elm.bind('scroll', function () {
                    if (raw.scrollTop == 0) {
                        var currentScrollHeight = raw.scrollHeight;
                        console.log(currentScrollHeight);
                        scope.$apply(attr.whenScrolled).then(function () {
                            raw.scrollTop = 100;
                        }, (error) => {
                            console.log('error');
                        });
                    }
                });
            };
        }]).directive('scrollBottomOn', ['$timeout', function ($timeout) {

            return {
                scope: {
                    scrollBottomOn: "="
                },
                restrict: 'A',
                link: function (scope, element, attr) {
                    scope.$watch(attr.scrollBottomOn, function (newVal, oldVal) {
                        console.log(attr.scrollBottomOn, 'adsasdasd');
                        if (newVal) {
                            element[0].scrollTop = element[0].scrollHeight;
                        }
                    }, function (val) {
                        console.log(val, 'bilal');
                    }, true);
                }
            };
            //return function (scope, elm, attr) {
            //    scope.$watch(attr.scrollBottomOn, function (value) {
            //        console.log(attr.scrollBottomOn, 'bilal');
            //        if (value) {
            //            $timeout(function () {
            //                elm[0].scrollTop = elm[0].scrollHeight;
            //            });
            //        }
            //    });
            //}
        }]).directive('listToBottom', function ($timeout, $window) {
            return {
                scope: {
                    listToBottom: "="
                },
                restrict: 'A',
                link: function (scope, element, attr) {
                    scope.$watchCollection('listToBottom', function (newVal) {
                        if (newVal) {
                            $timeout(function () {
                                element[0].scrollTop = element[0].scrollHeight;
                            }, 50);
                        }

                    });
                }
            };
        }).directive('contenteditable', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ctrl) {
                    // view -> model
                    element.bind('blur', function () {
                        scope.$apply(function () {
                            var html = element.html();
                            if (html == '<br>') {
                                html = '';
                            }
                            ctrl.$setViewValue(html);
                        });
                    });

                    // model -> view
                    ctrl.$render = function () {
                        element.html(ctrl.$viewValue);
                    };
                    // load init value from DOM
                    ctrl.$render();
                }
            };
        }).directive('changeOnBlur', function () {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, elm, attrs, ngModelCtrl) {
                    if (attrs.type === 'radio' || attrs.type === 'checkbox')
                        return;
                    var expressionToCall = attrs.changeOnBlur;

                    var oldValue = null;
                    elm.bind('focus', function () {
                        scope.$apply(function () {
                            oldValue = elm.html().replace(/&nbsp;/g, "");
                            console.log(oldValue, 'old val');
                        });
                    })
                    elm.bind('blur', function () {
                        scope.$apply(function () {
                            var newValue = elm.html().replace(/&nbsp;/g, "");
                            console.log(newValue, 'new val');
                            //if (newValue !== oldValue) {
                                scope.$eval(expressionToCall);
                            //}
                            //alert('changed ' + oldValue);
                        });
                    });
                }
            };
        });

var root = $('#rootPath').attr('href');
var currentUserId = $('#currUserId').html();




myApp.factory('ProjectService', function ($http, $q) {

    var visibilityAreaList = [];
    var projectMgmtPhases = [];
    var projectFundersRequired = [];
    var companyTypes = [];
    var Roles = [];

    var ProjectService = {};

    ProjectService.getAllProjects = function () {
        return $http.get(root + 'api/Projects/GetProjectsName');
    }

    ProjectService.getVisibilityAreas = function () {
        var deferred = $q.defer();
        if (visibilityAreaList.length > 0) {
            // we have data already. we can avoid going to server
            deferred.resolve(visibilityAreaList);
        } else {
            $http.get(root + 'api/VisibilityAreas').then(function success(response) {
                visibilityAreaList = response.data;
                deferred.resolve(response.data);
            }, function error() { }).catch(function (err) { console.log(err); });;
        }
        return deferred.promise;
    }

    ProjectService.getProjectManagementPhases = function () {
        var deferred = $q.defer();
        if (projectMgmtPhases.length > 0) {
            // we have data already. we can avoid going to server
            deferred.resolve(projectMgmtPhases);   // staff holds all the data
        } else {
            $http.get(root + 'api/ProjectManagementPhases').then(function success(response) {
                projectMgmtPhases = response.data;
                deferred.resolve(response.data);
            }, function error() { });
        }
        return deferred.promise;
    }

    ProjectService.getProjectFundersRequired = function () {
        var deferred = $q.defer();
        if (projectFundersRequired.length > 0) {
            // we have data already. we can avoid going to server
            deferred.resolve(projectFundersRequired);   // staff holds all the data
        } else {
            $http.get(root + 'api/FundersRequireds').then(function success(response) {
                projectFundersRequired = response.data;
                deferred.resolve(response.data);
            }, function error() { });
        }
        return deferred.promise;
    }

    ProjectService.getCompanyTypes = function () {
        var deferred = $q.defer();
        if (companyTypes.length > 0) {
            // we have data already. we can avoid going to server
            deferred.resolve(companyTypes);   // staff holds all the data
        } else {
            $http.get(root + 'api/CompanyTypes').then(function success(response) {
                companyTypes = response.data;
                deferred.resolve(response.data);
            }, function error() { });
        }
        return deferred.promise;
    }

    ProjectService.getRoles = function () {
        var deferred = $q.defer();
        if (Roles.length > 0) {
            // we have data already. we can avoid going to server
            deferred.resolve(Roles);   // staff holds all the data
        } else {
            $http.get(root + 'api/admin/GetRoles').then(function success(response) {
                Roles = response.data;
                deferred.resolve(response.data);
            }, function error() { });
        }
        return deferred.promise;
    }

    ProjectService.DeleteProject = function (id) {
        return $http.post(root + 'api/Projects/DeleteProject/' + id);
    }

    return ProjectService;
});


myApp.run(function ($rootScope, MenuService, $state, $location) {
    $rootScope.$on('$stateChangeSuccess', function (evt, toState, toParams, fromState, fromParams) {
        //$rootScope.$state = toState;
        $('.tooltip').tooltip('hide');
        //if (toState.name == 'name') {
        if (!toParams.id) {
            MenuService.set(toParams.id);
        }
        //}
    });

});


dragMe.$inject = [];

function dragMe() {
    var DDO = {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.prop('draggable', true);
            element.on('dragstart', function (event) {
                event.dataTransfer = event.originalEvent.dataTransfer;
                var j = JSON.stringify(scope.p);
                event.dataTransfer.setData("foo", j);
            });
        }
    };
    return DDO;
}
dropOnMe.$inject = [];
function dropOnMe($http) {
    var DDO = {
        restrict: 'A',
        scope: {
            someCtrlFn: '&callbackFn',
            obj: '='
        },
        link: function (scope, element, attrs) {
            element.on('dragover', function (event) {
                event.preventDefault();
            });
            element.on('drop', function (event) {
                event.preventDefault();
                event.dataTransfer = event.originalEvent.dataTransfer;
                var fileObj = event.dataTransfer.getData("foo");
                $http.get(root + 'api/UserProfiles/MoveFileInAlbum/' + scope.obj.Id + '/' + JSON.parse(fileObj).Id).then(
                    function success(response) {
                        scope.someCtrlFn({ arg1: scope.obj.Type });
                        gritterAlert("Success", response.data.Message, response.data.success);
                    }, function error() {
                    });
            });
        }
    };
    return DDO;
}

function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function getAllMatches(regex, text) {
    if (regex.constructor !== RegExp) {
        throw new Error('not RegExp');
    }

    var res = [];
    var match = null;

    if (regex.global) {
        while (match = regex.exec(text)) {
            res.push(match);
        }
    }
    else {
        if (match = regex.exec(text)) {
            res.push(match);
        }
    }

    return res;
}


myApp.provider("flipConfig", function () {

    var cssString =
        "<style> \
    .{{flip}} {float: left; overflow: hidden; width: {{width}}; height: {{height}}; }\
    .{{flip}}Panel { \
    position: absolute; \
    width: {{width}}; height: {{height}}; \
    -webkit-backface-visibility: hidden; \
    backface-visibility: hidden; \
    transition: -webkit-transform {{tempo}}; \
    transition: transform {{tempo}}; \
    -webkit-transform: perspective( 800px ) rotateY( 0deg ); \
    transform: perspective( 800px ) rotateY( 0deg ); \
    } \
    .{{flip}}HideBack { \
    -webkit-transform:  perspective(800px) rotateY( 180deg ); \
    transform:  perspective(800px) rotateY( 180deg ); \
    } \
    .{{flip}}HideFront { \
    -webkit-transform:  perspective(800px) rotateY( -180deg ); \
    transform:  perspective(800px) rotateY( -180deg ); \
    } \
    </style> \
    ";

    var _tempo = "0.5s";
    var _width = "100px";
    var _height = "100px";

    var _flipClassName = "flip";

    var _flipsOnClick = true;

    this.setTempo = function (tempo) {
        _tempo = tempo;
    };

    this.setDim = function (dim) {
        _width = dim.width;
        _height = dim.height;
    }

    this.setClassName = function (className) {
        _flipClassName = className;
    };

    this.flipsOnClick = function (bool) {
        _flipsOnClick = bool;
    }

    this.$get = function ($interpolate) {

        var interCss = $interpolate(cssString);
        var config = {
            width: _width,
            height: _height,
            tempo: _tempo,
            flip: _flipClassName
        };

        document.head.insertAdjacentHTML("beforeend", interCss(config));

        return {
            classNames: {
                base: _flipClassName,
                panel: _flipClassName + "Panel",
                hideFront: _flipClassName + "HideFront",
                hideBack: _flipClassName + "HideBack"
            },
            flipsOnClick: _flipsOnClick
        }
    };
});

myApp.factory('httpRequestInterceptor', function () {
    return {
        request: function (config) {

            config.headers['ProjectId'] = window.location.hash.split('/').length >= 2 ? window.location.hash.split('/')[1] : 0;

            return config;
        }
    };
}).config(function ($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
});

myApp.config(function (flipConfigProvider) {
    flipConfigProvider.setClassName("flipperCosmic");
    flipConfigProvider.setTempo("0.5s");
    flipConfigProvider.setDim({ width: "300px", height: "300px" });
    flipConfigProvider.flipsOnClick(false);
});

myApp
    .directive("flip", function (flipConfig) {

        function setDim(element, width, height) {
            if (width) {
                element.style.width = width;
            }
            if (height) {
                element.style.height = height;
            }
        }

        return {
            restrict: "E",
            controller: function ($scope, $element, $attrs) {

                $attrs.$observe("flipShow", function (newValue) {
                    console.log(newValue);
                    if (newValue === "front") {
                        showFront();
                    }
                    else if (newValue === "back") {
                        showBack();
                    }
                    else {
                        console.warn("FLIP: Unknown side.");
                    }
                });

                var self = this;
                self.front = null,
                    self.back = null;


                function showFront() {
                    self.front.removeClass(flipConfig.classNames.hideFront);
                    self.back.addClass(flipConfig.classNames.hideBack);
                }

                function showBack() {
                    self.back.removeClass(flipConfig.classNames.hideBack);
                    self.front.addClass(flipConfig.classNames.hideFront);
                }

                self.init = function () {
                    self.front.addClass(flipConfig.classNames.panel);
                    self.back.addClass(flipConfig.classNames.panel);

                    showFront();

                    if (flipConfig.flipsOnClick) {
                        self.front.on("click", showBack);
                        self.back.on("click", showFront);
                    }
                }

            },

            link: function (scope, element, attrs, ctrl) {

                var width = attrs.flipWidth,
                    height = attrs.flipHeight;

                element.addClass(flipConfig.classNames.base);

                if (ctrl.front && ctrl.back) {
                    [element, ctrl.front, ctrl.back].forEach(function (el) {
                        setDim(el[0], width, height);
                    });
                    ctrl.init();
                } else {
                    console.error("FLIP: 2 panels required.");
                }
            }
        }

    })
    .directive("flipPanel", function () {
        return {
            restrict: "E",
            require: "^flip",
            link: function (scope, element, attrs, flipCtr) {
                if (!flipCtr.front) {
                    flipCtr.front = element;
                } else if (!flipCtr.back) {
                    flipCtr.back = element;
                } else {
                    console.error("FLIP: Too many panels.");
                }
            }
        }
    })
    .directive('tooltip', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.hover(function () {
                    // on mouseenter
                    element.tooltip('show');
                }, function () {
                    // on mouseleave
                    element.tooltip('hide');
                });
            }
        };
    })
    .directive('googleplace', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, model) {
                var options = {
                    types: []
                };
                scope.gPlace = new google.maps.places.Autocomplete(element[0], options);
                google.maps.event.addListener(scope.gPlace, 'place_changed', function () {
                    scope.$apply(function () {
                        model.$setViewValue(element.val());
                    });
                });
            }
        };
    }).directive('ngDraggable', function ($document) {
        return {
            restrict: 'A',
            scope: {
                dragOptions: '=ngDraggable'
            },
            link: function (scope, elem, attr) {
                var startX, startY, x = 0, y = 0,
                    start, stop, drag, container;

                var width = elem[0].offsetWidth,
                    height = elem[0].offsetHeight;

                // Obtain drag options
                if (scope.dragOptions) {
                    start = scope.dragOptions.start;
                    drag = scope.dragOptions.drag;
                    stop = scope.dragOptions.stop;
                    var id = scope.dragOptions.container;
                    if (id) {
                        container = document.getElementById(id).getBoundingClientRect();
                    }
                }

                // Bind mousedown event
                elem.on('mousedown', function (e) {
                    e.preventDefault();
                    startX = e.clientX - elem[0].offsetLeft;
                    startY = e.clientY - elem[0].offsetTop;
                    $document.on('mousemove', mousemove);
                    $document.on('mouseup', mouseup);
                    if (start) start(e);
                });

                // Handle drag event
                function mousemove(e) {
                    y = e.clientY - startY;
                    x = e.clientX - startX;
                    setPosition();
                    if (drag) drag(e);
                }

                // Unbind drag events
                function mouseup(e) {

                    $document.unbind('mousemove', mousemove);
                    $document.unbind('mouseup', mouseup);
                    if (stop) stop(e);
                }

                // Move element, within container if provided
                function setPosition() {
                    if (container) {
                        //if (x < container.left) {
                        //    x = container.left;
                        //} else if (x > container.right - width) {
                        //    x = container.right - width;
                        //}
                        if (y < container.top) {
                            y = container.top;
                        } else if (y > container.bottom - height) {
                            y = container.bottom - height;
                        }
                    }

                    elem.css({
                        top: y < 0 ? 0 : y > (window.innerHeight - height * 5) ? (window.innerHeight - height * 5) : y + 'px',
                        //left: x + 'px'
                    });
                }
            }
        }

    });
