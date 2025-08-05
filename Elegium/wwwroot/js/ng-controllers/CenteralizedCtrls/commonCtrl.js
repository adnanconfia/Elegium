myApp.controller('commonCtrl', function ($scope, $rootScope, $ngConfirm, Hub, $http, $timeout, $uibModal, $q, $anchorScroll, $filter, MenuService, $state, $transitions, OnBoardingService, ProjectService) {

    var typingInterval = 3000;

    $rootScope.progressColors = ['#df6c4f', '#ecd06f', '#3c948b', '#1a99aa', '#048d90', '#06a16d', '#08b152'];
    $rootScope.progressBG = '#fff';
    $scope.bgColorOpacity = '';
    $rootScope.hexToRgbA = function (hex, opacity) {
        var c;
        if (/^#([A-Fa-f0-9]{3}){1,2}$/.test(hex) && hex) {
            c = hex.substring(1).split('');
            if (c.length == 3) {
                c = [c[0], c[0], c[1], c[1], c[2], c[2]];
            }
            c = '0x' + c.join('');
            return 'rgba(' + [(c >> 16) & 255, (c >> 8) & 255, c & 255].join(',') + ',' + (opacity ? opacity : 0) + ')';
        }
        //throw new Error('Bad Hex');
    }

    var typingTimer;
    $scope.currUser = $('#currUserId').html();
    Array.prototype.toLowerCase = function () {
        var i = this.length;
        while (--i >= 0) {
            if (typeof this[i] === "string") {
                this[i] = this[i].toLowerCase();
            }
        }
        return this;
    };

    $scope.maxOnboarding = 100;
    $scope.currentOnboarding = 100;


    $scope.onboardingDragOptions = {
        start: function (e) {
            //console.log("STARTING");
        },
        drag: function (e) {
            //console.log("DRAGGING");
        },
        stop: function (e) {
            //console.log("STOPPING");
            //e.stopImmediatePropagation();
            //e.preventDefault();
        },
    }


    $rootScope.GetOnBoarding = function () {
        OnBoardingService.GetOnBoarding().then((resp) => {
            console.log('onboard resp:', resp);
            if (resp.data.createNew) {
                $scope.createNewProject = resp.data.createNew;
            }
            else if (resp.data.complete) {
                $scope.onBoardingDone = true;
            }
            else {
                $scope.onboardingOverlay = resp.data.OnBoardingDtoList;
                $scope.onboardingProject = resp.data.Project;
                $scope.maxOnboarding = resp.data.OnBoardingDtoList.length;
                $scope.currentOnboarding = $filter('filter')(resp.data.OnBoardingDtoList,
                    { completed: true },
                    true // ==========> this is for exact match
                ).length;
            }
        });
    }

    $scope.GetOnBoarding();

    $scope.getSrefUrl = function (name, params) {
        return $state.href(name, params);
    }

    $scope.gotoUrl = function (name, params) {
        $state.go(name, { onboardingprojectId: $scope.onboardingProject.Id });
    }

    $scope.hexToRgbA = function (hex, opacity) {
        var c;
        if (/^#([A-Fa-f0-9]{3}){1,2}$/.test(hex) && hex) {
            c = hex.substring(1).split('');
            if (c.length == 3) {
                c = [c[0], c[0], c[1], c[1], c[2], c[2]];
            }
            c = '0x' + c.join('');
            return 'rgba(' + [(c >> 16) & 255, (c >> 8) & 255, c & 255].join(',') + ',' + (opacity ? opacity : 0) + ')';
        }
        //throw new Error('Bad Hex');
    }

    //cinematic design change end

    $rootScope.isItMessageScreen = false;

    $scope.$watch('$state', function (newval, oldval) {
        $rootScope.isItMessageScreen = (newval && newval.name == 'messages' ? true : false);
    });

    $transitions.onSuccess({}, function (transition) {
        $timeout(() => {
            $('.tooltip').tooltip('hide');
        }, 1000);
    });


    //$rootScope.$state.name == 'messages';
    //alert($rootScope.isItMessageScreen)
    $scope.Top5Messages = {};
    $scope.myHub = new Hub('HubName', {
        rootPath: root + '/chatHub',
        autoReconnect: true,
        reconnectTimeout: 5000,
        methods: [
            'SendPrivateMessage',
            'UserIsTypingPrivateChat',
            'UserDoneTypingPrivateChat',
            'GetLastSeenPrivateChat',
            'SendPhotoIdsToUser'
        ],
        listeners: {
            SendPrivateMessage: function (success) {
                alert('aa');
            }
        },
        stateChanged: function (newState) {
            $scope.$apply(function () {
                $scope.hubStatus = newState.newState
            });
            switch (newState) {
                //    .....
            }
        }
    });

    $scope.getProjectsForMenu = function () {
        $http.get(root + 'api/Projects/GetProjects').then(function success(response) {
            $scope.projectsListForMenu = response.data;
        }, function error() { });
    }

    $scope.DeleteProject = function (id) {
        $ngConfirm({
            title: 'Delete Project?',
            content: 'Are you sure want to delete project? This cannot be undone.',
            autoClose: 'cancel|8000',
            buttons: {
                apply: {
                    text: 'Delete',
                    btnClass: 'btn-red',
                    action: function () {
                        ProjectService.DeleteProject(id).then((succ) => {
                            $scope.getProjectsForMenu();
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $http.get(root + 'api/UserProfiles/GetCompanyName').then(function success(response) {
        $scope.companyName = response.data;
    }, function error() { });

    $scope.goToProductionModule = function (projectId) {
        //MenuService.set(projectId);
        $state.go('home', { id: projectId });
    }

    //$scope.BoxShadow = 'rgba(255,255,0,1)';



    $scope.loadProductionModule = false;
    $scope.projectId = undefined;
    $scope.options = {
        inputClass: 'border-0',
        format: 'hexString'
    };
    $scope.opacity = [0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1];
    $rootScope.BackgroundThings = {
        BackgroundImage: 'url(api/UserProfiles/GetBGImage/U/1/' + uuidv4() + ')',
        BackgroundOpacity: '0',
        BackgroundColor: '#FFFF00',
        ProjectId: $scope.projectId,
        GlassMode: false,
        CinematicMode: false,
        DarkMode: false
    }
    $scope.uploadBgImage = function (event) {
        var fd = new FormData();
        fd.append("ProjectId", $scope.projectId ?? 0);
        fd.append("BackgroundOpacity", $rootScope.BackgroundThings.BackgroundOpacity);
        fd.append("BackgroundColor", $rootScope.BackgroundThings.BackgroundColor);

        fd.append("GlassMode", $rootScope.BackgroundThings.GlassMode);
        fd.append("CinematicMode", $rootScope.BackgroundThings.CinematicMode);
        fd.append("DarkMode", $rootScope.BackgroundThings.DarkMode);
        if (event && event.target.files && event.target.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $rootScope.BackgroundThings.BackgroundImage = 'url(' + e.target.result + ')';
            }
            reader.readAsDataURL(event.target.files[0]);
            fd.append("file", event.target.files[0]);
        }
        $http({
            url: root + "api/UserProfiles/UploadBGImage",
            method: 'POST',
            data: fd,
            headers: {
                'Content-Type': undefined
            }
        })
            .then(function loginSuccessCallback(response) { });
    };

    $scope.$watch('projectId', function () {
        $http.get(root + 'api/UserProfiles/GetBgColorOpacity/' + (typeof ($scope.projectId) == 'undefined' ? 0 : $scope.projectId)).then((suc) => {
            $timeout(() => {
                $rootScope.BackgroundThings = suc.data;
                console.log($rootScope.BackgroundThings);
                if (suc.data.GlassMode) {
                    $('body').addClass('glass-mode');
                    $('.btn-glassmode').prop("checked", true);
                }
                else {
                    $('body').removeClass('glass-mode');
                    $('.btn-glassmode').prop("checked", false);
                }

                if (suc.data.CinematicMode) {
                    $('body').addClass('cinematic-mode');
                    $('.btn-cinematicmode').prop("checked", true);
                }
                else {
                    $scope.bgImage = {};
                    $rootScope.BackgroundThings.BackgroundImage = '';
                    $('body').removeClass('cinematic-mode');
                    $('.btn-cinematicmode').prop("checked", false);
                }

                if (suc.data.DarkMode) {
                    $rootScope.progressBG = '#2b3035';
                    $('body').addClass('dark-mode');
                    $('.btn-darkmode').prop("checked", true);
                }
                else {
                    $('body').removeClass('dark-mode');
                    $('.btn-darkmode').prop("checked", false);
                }

            }, 300);

            $(".setting_switch .btn-glassmode").on('change', function () {
                if (this.checked) {
                    $('body').addClass('glass-mode');

                } else {
                    $('body').removeClass('glass-mode');
                }
            });

            // Full Dark mode
            $(".setting_switch .btn-darkmode").on('change', function () {
                if (this.checked) {
                    $rootScope.progressBG = '#2b3035';
                    $('body').addClass('dark-mode');
                } else {
                    $rootScope.progressBG = '';
                    $('body').removeClass('dark-mode');
                }
            });


            //cinematic design change

            // Cinematic Mode
            $(".setting_switch .btn-cinematicmode").on('change', function () {
                if (this.checked) {
                    $http.get(root + 'api/UserProfiles/GetBgColorOpacity/' + (typeof ($scope.projectId) == 'undefined' ? 0 : $scope.projectId)).then((suc) => {
                        $timeout(() => {
                            console.log($rootScope.BackgroundThings);
                            $rootScope.BackgroundThings.BackgroundImage = suc.data.BackgroundImage;
                            console.log($rootScope.BackgroundThings);
                        }, 200);
                    });
                    $('body').addClass('cinematic-mode');
                } else {
                    $timeout(() => {
                        $scope.bgImage = {};
                        $scope.cinematicMode = "N";
                        $rootScope.BackgroundThings.BackgroundImage = '';
                        //$rootScope.BackgroundThings.CinematicMode = '';
                    }, 200);
                    $('body').removeClass('cinematic-mode');
                }
            });
        }, (err) => { });
    });

    var subscription = MenuService.subscribe(function onNext(d) { //gets user wise project menu
        //console.log((d && !$scope.loadProductionModule) || ($scope.projectId != d && $scope.projectId), d, $scope.loadProductionModule, $scope.projectId);
        if (d && (!$scope.loadProductionModule || $scope.projectId != d)) {
            $http.get(root + 'api/Documents/GetProjectWiseUserMenu/' + d).then(function success(response) {
                $scope.projectUserMenu = response.data.projectUserMenu;
                $scope.selectedProjectName = response.data.projectName;
                $scope.IsStarted = response.data.IsStarted;
                $scope.loadProductionModule = true;
                $scope.projectId = d;

                for (var i = 0; i < $scope.projectUserMenu.length; i++) {
                    $scope.$watch('projectUserMenu', function (newValue, oldValue) {
                        console.log(newValue + ":::" + oldValue);
                    }, true);
                }

            }, function error() {
            });
        }
        else if ($scope.loadProductionModule && d) {
            //console.log('prod men alredy loded');
            return;
        } else {
            $scope.loadProductionModule = false;
            $scope.projectId = undefined;
            $scope.projectUserMenu = [];
            $scope.selectedProjectName = undefined;
        }
    });

    $scope.openProjectSelector = function () {
        $('.user_div').toggleClass('open');
    }

    this.$onDestroy = function () {
        subscription.dispose();
    };


    $scope.getProjectsForMenu();
    $scope.myHub.start();

    $scope.chatBoxes = {};
    $scope.Messages = {};
    $scope.uppy = {};
    $scope.hasFilesInUppy = {};
    $scope.holder = {};
    $scope.notifications = [];
    $scope.notificationsPageIndex = 0;
    $scope.unreadNotifications = 0;
    $scope.notificationsLoading = false;
    $scope.lastNotification = false;
    $scope.getNotifications = function () {
        var deferred = $q.defer();
        $scope.notificationsPageIndex = $scope.notificationsPageIndex + 1;

        if (!$scope.notificationsLoading && !$scope.lastNotification) {
            $scope.notificationsLoading = true;
            $http.get(root + 'api/Notifications/GetNotifications/' + $scope.notificationsPageIndex).then(
                function success(response) {
                    if (response.data.records.length == 0) {
                        $scope.lastNotification = true;
                    } else {
                        $scope.lastNotification = false;
                    }
                    $scope.unreadNotifications = response.data.unreadCount;
                    $.merge($scope.notifications, response.data.records);
                    $scope.notificationsLoading = false;
                    deferred.resolve();
                },
                function error() {
                    deferred.reject();
                });
        } else {
            deferred.reject();
        }
        return deferred.promise;
    }

    $scope.getNotifications();

    $scope.markReadAndOpenUrl = function (item) {
        item.Read = true;
        $scope.unreadNotifications = $scope.unreadNotifications - 1;
        $http.post(root + 'api/Notifications/MarkRead', item).then(
            succ => {
                window.location.href = item.Url;
            },
            err => { });
    }

    $scope.markAllRead = function (event) {
        event.stopPropagation();
        event.preventDefault();
        $scope.unreadNotifications = 0;
        $scope.notifications.forEach(resp => {
            resp.Read = true;
        });
        $http.post(root + 'api/Notifications/MarkAllRead').then(
            succ => {
                //console.log('all read');
            },
            err => { });
    }

    $scope.deleteAllNotifications = function (event) {
        event.stopPropagation();
        event.preventDefault();
        $scope.unreadNotifications = 0;
        $scope.notifications = [];
        $http.post(root + 'api/Notifications/DeleteAllNotifications').then(
            succ => {
                // console.log('all deleted');
            },
            err => { });
    }
    //
    $scope.deleteNotification = function (n, event) {
        event.stopPropagation();
        event.preventDefault();
        var index = $scope.notifications.indexOf(n);
        if (index > -1) {
            $scope.notifications.splice(index, 1);
        }
        $http.post(root + 'api/Notifications/DeleteNotification', n).then(
            succ => {
                //console.log('notification deleted');
            },
            err => { });
    }

    $rootScope.openChatBox = function (receiverId) {
        if (!($rootScope.isItMessageScreen)) {
            if (!$scope.chatBoxes[receiverId]) {
                $http.post(root + 'api/Messages/GetConversationWithUser', { ThreadId: uuidv4(), UserId: receiverId, PageIndex: 1 }).then(resp => {
                    var data = resp.data;
                    //$scope.chatBoxes[data.Box.ThreadId + '_' + data.Box.UserId] = data.Box;
                    $scope.chatBoxes[data.Box.UserId] = data.Box;
                    $scope.Messages[data.Box.ThreadId + '_' + data.Box.UserId] = data.list;
                    setTimeout(getUserLastSeen, typingInterval, data.Box);

                    $scope.chatBoxPageIndex[data.Box.ThreadId + '_' + data.Box.UserId] = 1;

                    $scope.status[data.Box.ThreadId + '_' + data.Box.UserId] = {
                        loading: false,
                        lastRecord: false,
                        hasNewMessage: true
                    }

                    $timeout(function () {
                        $scope.uppy[data.Box.ThreadId + '_' + data.Box.UserId] = new Uppy.Core({
                            restrictions: {
                                allowedFileTypes: ['image/*']
                            }
                        })
                            .use(Uppy.Dashboard, {
                                id: 'Dashboard',
                                target: '#uppy' + data.Box.ThreadId + '_' + data.Box.UserId,
                                //trigger: '#uppy-uploader',
                                metaFields: [],
                                //trigger: '#uppy-select-files',
                                inline: true,
                                height: 150,
                                //defaultTabIcon: defaultTabIcon,
                                showLinkToFileUploadResult: true,
                                showProgressDetails: false,
                                hideUploadButton: true,
                                hideRetryButton: false,
                                hidePauseResumeButton: false,
                                hideCancelButton: false,
                                hideProgressAfterFinish: false,
                                note: null,
                                closeModalOnClickOutside: false,
                                closeAfterFinish: false,
                                disableStatusBar: false,
                                disableInformer: false,
                                disableThumbnailGenerator: false,
                                disablePageScrollWhenModalOpen: true,
                                animateOpenClose: true,
                                // fileManagerSelectionType: 'files',
                                proudlyDisplayPoweredByUppy: false,
                                //onRequestCloseModal: () => $scope.closeUppyModal(),
                                showSelectedFiles: true,
                                showRemoveButtonAfterComplete: false,
                                //locale: defaultLocale,
                                browserBackButtonClose: false,
                                theme: 'light',
                                restrictions: {
                                    maxFileSize: 300000,
                                    maxNumberOfFiles: 5,
                                    minNumberOfFiles: 2,
                                    allowedFileTypes: ['image/*', 'video/*']
                                }
                            }).use(Uppy.Tus,
                                {
                                    endpoint: root + 'files/',
                                    resume: true,
                                    retryDelays: [0, 1000, 3000, 5000],
                                    chunkSize: 5242880
                                });

                        $scope.uppy[data.Box.ThreadId + '_' + data.Box.UserId].on('file-removed', (file, reason) => {
                            $timeout(() => {
                                $scope.$apply(() => {
                                    $scope.hasFilesInUppy[data.Box.ThreadId + '_' + data.Box.UserId] = $scope.uppy[data.Box.ThreadId + '_' + data.Box.UserId].getFiles().length > 0
                                });
                            });
                        });

                        $('#file' + data.Box.ThreadId + '_' + data.Box.UserId).on('change', function (event) {
                            Array.from(event.target.files).forEach((a) => {
                                $scope.uppy[data.Box.ThreadId + '_' + data.Box.UserId].addFile({
                                    data: a,
                                    name: a.name,
                                    type: a.type
                                });
                            });
                            $scope.$apply(function () {
                                $scope.hasFilesInUppy[data.Box.ThreadId + '_' + data.Box.UserId] = true;
                            });
                        });

                        //initialize drag n drop

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId] = document.getElementById('drag' + data.Box.ThreadId + '_' + data.Box.UserId);
                        var lastenter;
                        //bilal

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId].ondragenter = function (e) {
                            lastenter = e.target;
                            e.preventDefault();
                            e.preventDefault();
                            this.className = ' hover-files-sm';
                            return false;
                        };

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId].ondragstart = function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            return false;
                        }

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId].ondragover = function (e) {
                            e.preventDefault();
                            e.preventDefault();
                            this.className = ' hover-files-sm';
                        }

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId].ondragleave = function (e) {
                            e.preventDefault();
                            e.preventDefault();
                            if (lastenter === e.target) {
                                this.className = '';
                            }
                        };

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId].ondragend = function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            this.className = '';
                        }

                        $scope.holder[data.Box.ThreadId + '_' + data.Box.UserId].ondrop = function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            this.className = '';
                            Array.from(e.dataTransfer.files).forEach((a) => {

                                try {
                                    $scope.uppy[data.Box.ThreadId + '_' + data.Box.UserId].addFile({
                                        data: a,
                                        name: a.name,
                                        type: a.type
                                    });
                                }
                                catch (err) {
                                    console.log(err);
                                }
                            });
                            $scope.$apply(function () {
                                if ($scope.uppy[data.Box.ThreadId + '_' + data.Box.UserId].getFiles().length > 0)
                                    $scope.hasFilesInUppy[data.Box.ThreadId + '_' + data.Box.UserId] = true;
                            });
                        };
                    });
                }, resp => {
                });
            }
        }
    }

    $scope.openFileFileDialog = function (Box) {
        $('#file' + Box.ThreadId + '_' + Box.UserId).click();
    }

    function getUserLastSeen(obj) {
        if (!(obj.UserId == '' || obj.ThreadId == '')) {
            var msg = {
                ReceiverId: obj.UserId,
                ThreadId: obj.ThreadId,
                Text: obj.Text,
                When: new Date(),
                MessageId: uuidv4(),
                IsItFromMe: true
            };
            $scope.myHub.GetLastSeenPrivateChat(msg);
        } else {
            // console.log('a');
        }
    }

    $scope.UserTypingChatBox = function (obj, keyEvent) {
        if (keyEvent.which === 13 && obj.Text != '') {
            $scope.sendMsg(obj);
        }
        if (obj.Text.length % 10 == 0) { //raise useristyping
            clearInterval(typingTimer);
            typingTimer = setTimeout(doneTyping, typingInterval, obj);
            var msg = {
                ReceiverId: obj.UserId,
                ThreadId: obj.ThreadId,
                Text: obj.Text,
                When: new Date(),
                MessageId: uuidv4(),
                IsItFromMe: true
            };
            $scope.myHub.UserIsTypingPrivateChat(msg);
        }
    }

    $scope.openUppy = function (box) {
        //console.log(box);
        var uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                id: 'Dashboard',
                target: 'body',
                metaFields: [],
                trigger: '#uppy-select-files',
                inline: false,
                width: 750,
                height: 550,
                thumbnailWidth: 280,
                //defaultTabIcon: defaultTabIcon,
                showLinkToFileUploadResult: true,
                showProgressDetails: false,
                hideUploadButton: false,
                hideRetryButton: false,
                hidePauseResumeButton: false,
                hideCancelButton: false,
                hideProgressAfterFinish: false,
                note: null,
                closeModalOnClickOutside: false,
                closeAfterFinish: false,
                disableStatusBar: false,
                disableInformer: false,
                disableThumbnailGenerator: false,
                disablePageScrollWhenModalOpen: true,
                animateOpenClose: true,
                fileManagerSelectionType: 'files',
                proudlyDisplayPoweredByUppy: true,
                //onRequestCloseModal: () => this.closeModal(),
                showSelectedFiles: true,
                showRemoveButtonAfterComplete: false,
                //locale: defaultLocale,
                browserBackButtonClose: false,
                theme: 'light'
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });
    }

    $scope.sendMsg = function (obj) {
        clearInterval(typingTimer);
        typingTimer = setTimeout(doneTyping, typingInterval, obj);
        var msg = {
            ReceiverId: obj.UserId,
            ThreadId: obj.ThreadId,
            Text: obj.Text,
            When: new Date(),
            MessageId: uuidv4(),
            IsItFromMe: true
        };
        $scope.myHub.SendPrivateMessage(msg);
        obj.Text = '';
        $scope.status[obj.ThreadId + '_' + obj.UserId].hasNewMessage = true;
        //
    }

    function doneTyping(obj) {
        var msg = {
            ReceiverId: obj.UserId,
            ThreadId: obj.ThreadId,
            Text: obj.Text,
            When: new Date(),
            MessageId: uuidv4(),
            IsItFromMe: true
        };
        $scope.myHub.UserDoneTypingPrivateChat(msg);
    }

    $scope.myHub.on('UpdateUserStatus', function (resp) {
        if (!($rootScope.isItMessageScreen)) {
            $scope.$apply(function () {
                if ($scope.chatBoxes[resp.msgDto.SenderId]) {
                    $scope.chatBoxes[resp.msgDto.SenderId].status = resp.Status;
                }

                // $('.' + resp).toggleClass('avatar-offline avatar-online');
            });
        }
    });

    $scope.myHub.on('UserOnLine', function (resp) {
        if (!($rootScope.isItMessageScreen)) {
            $scope.$apply(function () {
                if ($scope.chatBoxes[resp]) {
                    $scope.chatBoxes[resp].status = 'Active Now';
                    $scope.chatBoxes[resp].Online = true;
                }
                if ($scope.Top5Messages[resp]) {
                    $scope.Top5Messages[resp].Online = true;
                }

                $('.' + resp).removeClass('avatar-offline').addClass('avatar-online');
            });
        }
    });

    $scope.myHub.on('NotificationHandler', function (resp) {
        $scope.$apply(function () {
            $scope.unreadNotifications = $scope.unreadNotifications + 1;
            $scope.notifications.push(resp);
        });
    });

    $scope.myHub.on('UserOffLine', function (resp) {
        if (!($rootScope.isItMessageScreen)) {
            $scope.$apply(function () {
                if ($scope.chatBoxes[resp.UserId]) {
                    $scope.chatBoxes[resp.UserId].status = resp.Status;
                    $scope.chatBoxes[resp.UserId].Online = false;
                }
                if ($scope.Top5Messages[resp.UserId]) {
                    $scope.Top5Messages[resp.UserId].Online = false;

                }
                $('.' + resp.UserId).removeClass('avatar-online').addClass('avatar-offline');
            });
        }
    });

    $scope.closeChatBox = function (event, obj) {
        event.stopPropagation();
        $scope.chatBoxes[obj.UserId].Closed = true;
        $http.post(root + 'api/Messages/CloseChatBox', obj)
            .then(resp => {
                delete $scope.chatBoxes[obj.UserId];
                delete $scope.Messages[obj.ThreadId + '_' + obj.UserId];
            }, resp => {
            });
    }

    $scope.status = {};
    $scope.chatBoxPageIndex = {};
    $scope.getNextPageData = function (box) {
        $scope.chatBoxPageIndex[box.ThreadId + '_' + box.UserId] = $scope.chatBoxPageIndex[box.ThreadId + '_' + box.UserId] + 1;
        var deferred = $q.defer();
        if (!$scope.status[box.ThreadId + '_' + box.UserId].loading && !$scope.status[box.ThreadId + '_' + box.UserId].lastRecord) {
            $scope.status[box.ThreadId + '_' + box.UserId].loading = true;
            // simulate an ajax request
            $http.post(root + 'api/Messages/GetConversationWithUser', {
                ThreadId: uuidv4(),
                UserId: box.UserId,
                PageIndex: $scope.chatBoxPageIndex[box.ThreadId + '_' + box.UserId]
            }).then(resp => {

                if (resp.data.list.length == 0) {
                    $scope.status[box.ThreadId + '_' + box.UserId].lastRecord = true;
                } else {
                    $scope.status[box.ThreadId + '_' + box.UserId].lastRecord = false;
                }
                $scope.status[box.ThreadId + '_' + box.UserId].loading = false;
                $scope.status[box.ThreadId + '_' + box.UserId].hasNewMessage = false;
                $.merge(resp.data.list, $scope.Messages[box.ThreadId + '_' + box.UserId]);
                $scope.Messages[box.ThreadId + '_' + box.UserId] = resp.data.list;
                deferred.resolve();
            });
        } else {
            deferred.reject();
        }
        return deferred.promise;
    }

    $scope.Files = {};

    $scope.myHub.on('MyMessageSent', function (resp) {
        var msg = resp;
        if (!($rootScope.isItMessageScreen)) {
            $scope.$apply(function () {
                if ($scope.chatBoxes[resp.ReceiverId]) {
                    $scope.chatBoxes[resp.ReceiverId].NewChat = false;
                    var jObj = {
                        When: resp.When,
                        MessageId: resp.MessageId,
                        Text: resp.Text,
                        FriendlyTime: resp.FriendlyTime,
                        Read: true
                    };

                    if ($scope.Top5Messages[resp.ReceiverId]) {
                        $scope.Top5Messages[resp.ReceiverId].When = resp.When;
                        $scope.Top5Messages[resp.ReceiverId].MessageId = resp.MessageId;
                        $scope.Top5Messages[resp.ReceiverId].Text = resp.Text;
                        $scope.Top5Messages[resp.ReceiverId].FriendlyTime = resp.FriendlyTime;
                        $scope.Top5Messages[resp.ReceiverId].Online = true;
                        $scope.Top5Messages[resp.ReceiverId].Read = $scope.Top5Messages[resp.ReceiverId].Read ? true : false;
                        $scope.UnreadCount += $scope.Top5Messages[resp.ReceiverId].Read ? 0 : 1;
                    } else {
                        $scope.Top5Messages[msg.ReceiverId] = {
                            When: msg.When,
                            MessageId: msg.MessageId,
                            Text: msg.Text,
                            Read: true,
                            Online: true,
                            ThreadId: msg.ThreadId,
                            UserId: msg.ReceiverId,
                            UnreadMsgs: 0,
                            FriendlyTime: msg.FriendlyTime,
                            Name: msg.ReceiverName
                        };
                    }

                    if ($scope.uppy[resp.ThreadId + '_' + resp.ReceiverId]) {
                        if ($scope.uppy[resp.ThreadId + '_' + resp.ReceiverId].getFiles().length > 0) {
                            resp.Files = [];
                            $scope.uppy[resp.ThreadId + '_' + resp.ReceiverId].run();
                            $scope.uppy[resp.ThreadId + '_' + resp.ReceiverId].upload().then((result) => {
                                var files = Array.from(result.successful);
                                //console.log(files, files.length);
                                $scope.Files[resp.ThreadId + '_' + resp.ReceiverId] = [];
                                files.forEach((file) => {
                                    // file: { id, name, type, ... }
                                    // progress: { uploader, bytesUploaded, bytesTotal }
                                    var fileResp = file.response.uploadURL;
                                    var id = fileResp.substring(fileResp.lastIndexOf("/") + 1, fileResp.length);
                                    var fileObj = {};
                                    fileObj.FileId = id;
                                    fileObj.Name = file.name;
                                    fileObj.MessageId = resp.MessageId;
                                    //fileObj.Type = 'P';//
                                    fileObj.Size = file.size;
                                    fileObj.ContentType = file.type;
                                    $scope.Files[resp.ThreadId + '_' + resp.ReceiverId].push(fileObj);
                                    resp.Files.push(id);
                                });

                                if ($scope.Files[resp.ThreadId + '_' + resp.ReceiverId].length > 0) {
                                    $http.post(root + 'api/Messages/SaveMessageFiles',
                                        $scope.Files[resp.ThreadId + '_' + resp.ReceiverId]).then(
                                            function success(response) {
                                                $scope.uppy[resp.ThreadId + '_' + resp.ReceiverId].reset();
                                                resp.FilesCount =
                                                    $scope.Files[resp.ThreadId + '_' + resp.ReceiverId].length;
                                                $scope.Messages[resp.ThreadId + '_' + resp.ReceiverId].push(resp);
                                                $scope.Files[resp.ThreadId + '_' + resp.ReceiverId] = [];
                                                $scope.myHub.SendPhotoIdsToUser(resp);
                                            },
                                            function error() { });
                                }
                            });
                        } else {
                            $scope.Messages[resp.ThreadId + '_' + resp.ReceiverId].push(resp);
                        }
                    } else {
                        $scope.Messages[resp.ThreadId + '_' + resp.ReceiverId].push(resp);
                    }
                }
                else {
                    $rootScope.openChatBox(resp.ReceiverId);
                }
            });
        }
    });

    $scope.myHub.on('UpdateMessagePhotos', function (resp) {
        $scope.$apply(function () {
            if ($scope.Messages[resp.openedThreadId]) {
                var elem = $scope.Messages[resp.openedThreadId].find(a => {
                    if (a.MessageId == resp.msgDto.MessageId)
                        return a;
                });
                if (elem) {
                    elem.Files = resp.msgDto.Files;
                    elem.FilesCount = resp.msgDto.FilesCount;
                }
            }
        });
    });

    $rootScope.getClass = function (l) {
        if (l == 1)
            return ' p-2 ';
        else if (l == 2)
            return 'col-sm-6';
        else
            return 'col-sm-6';
    }

    $scope.myHub.on('ReceiveMessage', function (resp) {
        if (!($rootScope.isItMessageScreen)) {
            $scope.$apply(function () {
                var msg = resp.msgDto;
                var openedThreadId = resp.openedThreadId;
                if ($scope.Messages[openedThreadId]) {
                    $scope.Messages[openedThreadId].push(msg);//
                    $scope.chatBoxes[msg.SenderId].status = 'Active Now';
                    $scope.chatBoxes[msg.SenderId].Online = true;
                    $scope.status[openedThreadId].hasNewMessage = true;
                } else {
                    if ($scope.chatBoxes[msg.SenderId]) {
                        $scope.Messages[openedThreadId] = msg;
                        $scope.status[openedThreadId].hasNewMessage = true;
                    } else {
                        $rootScope.openChatBox(msg.SenderId);
                    }
                }
                var convo = $scope.Top5Messages[msg.SenderId];
                if (convo == null) {

                    $scope.Top5Messages[msg.SenderId] = {
                        When: msg.When,
                        MessageId: msg.MessageId,
                        Text: msg.Text,
                        Read: true,
                        Online: true,
                        ThreadId: msg.ThreadId,
                        UserId: msg.SenderId,
                        UnreadMsgs: 0,
                        FriendlyTime: msg.FriendlyTime,
                        Name: msg.SenderName
                    };
                } else {
                    convo.When = msg.When;
                    convo.MessageId = msg.MessageId;
                    convo.Text = msg.Text;
                    convo.FriendlyTime = msg.FriendlyTime;
                    convo.UnreadMsgs = convo.Opened ? 0 : resp.UnreadCount
                }


            });
        }
    });

    $scope.toggleChatBox = function (box) {
        box.Opened = !box.Opened;
    }

    $scope.myHub.on('OpenChatBox', function (resp) {
        if (!($rootScope.isItMessageScreen)) {
            $scope.$apply(function () {
                if ($scope.chatBoxes[resp.msgDto.SenderId]) {
                    $scope.chatBoxes[resp.msgDto.SenderId].status = resp.Status;
                }
            });
        }
    });

    $scope.deleteChat = function (obj) {
        delete $scope.Messages[obj.ThreadId + '_' + obj.UserId];
        $http.post(root + 'api/Messages/DeleteConversation', { ThreadId: obj.ThreadId, UserId: obj.UserId }).then(function (resp) {
            $scope.Messages[obj.ThreadId + '_' + obj.UserId] = [];
        }, function (resp) {
            //console.log(resp);
        });
    }

    $http.get(root + 'api/Messages/GetTop5Conversations').then(resp => {
        //console.log(resp.data.dict);
        $scope.Top5Messages = resp.data.dict;
        $scope.UnreadCount = resp.data.UnreadCount;
        resp.data.openChatBoxes.forEach(resp => {
            $timeout(function () {
                $rootScope.openChatBox(resp.ReceiverId);
            });
        }, resp => {
        });


        //console.log($scope.Top5Messages, 'bilal');
    }, resp => {
        //console.log(resp);
    });

    $scope.openFromTop = function (k) {
        $scope.Top5Messages[k].Read = true;
        $rootScope.openChatBox(k);
    }
    $scope.resetUnreadCount = function () {
        $scope.UnreadCount = 0;
    }

    $rootScope.openMessageFile = function (msgObj, fileId) {
        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/photoViewer.html',
            controller: 'chatGalleryCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                msgObj: function () {
                    return msgObj;
                },
                fileId: function () {
                    return fileId;
                }
            }
        });
    }
}).controller('chatGalleryCtrl', function ($scope, $http, $uibModalInstance, msgObj, fileId) {
    $scope.data = msgObj;

    $scope.data.FileId = fileId;

    //console.log($scope.data, 'open');
    $scope.ok = function () {
        $uibModalInstance.close(files);
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.next = function () {
        $http.get(root + 'api/Messages/NextPrevFileId/' + $scope.data.FileId + '/' + 'N' + '/' + $scope.data.ThreadId + '/' + $scope.data.MessageId).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }

    $scope.prev = function () {
        $http.get(root + 'api/Messages/NextPrevFileId/' + $scope.data.FileId + '/' + 'P' + '/' + $scope.data.ThreadId + '/' + $scope.data.MessageId).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }
});