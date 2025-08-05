myApp.controller('messageCtrl', function ($scope, $http, Hub, $timeout, $q, $stateParams) {
    var pathname = window.location.pathname.split("/");
    var threadId = $stateParams.threadId;
    $scope.hubStatus = '';
    $scope.GetConversationThreads = function () {
        $http.get(root + 'api/Messages/GetConversationThreads').then(resp => {

            $scope.myConversationThreads = resp.data.myThreads;
            if ($scope.myConversationThreads.length > 0 && threadId) {
                var convoObj = undefined;
                $scope.myConversationThreads.forEach((convo) => {
                    if (convo.ThreadId == threadId) {
                        convoObj = convo;
                    }
                });
                if (convoObj) {
                    $scope.getChats(convoObj);
                }
            }
        }, resp => {

        });
    }

    var holder = document.getElementById('draggableContainer');
    var lastenter;
    //bilal

    holder.ondragenter = function (e) {
        lastenter = e.target;
        e.preventDefault();
        e.preventDefault();
        this.className = ' hover-files';
        return false;
    };

    holder.ondragstart = function (e) {
        e.preventDefault();
        e.stopPropagation();
        return false;
    }

    holder.ondragover = function (e) {
        e.preventDefault();
        e.preventDefault();
        this.className = ' hover-files';
    }

    holder.ondragleave = function (e) {
        e.preventDefault();
        e.preventDefault();
        if (lastenter === e.target) {
            this.className = 'card bg-none b-none';
        }
    };

    holder.ondragend = function (e) {
        e.preventDefault();
        e.stopPropagation();
        this.className = 'card bg-none b-none';
    }

    holder.ondrop = function (e) {
        e.preventDefault();
        e.stopPropagation();
        this.className = 'card bg-none b-none';
        addFilesToUppy(e.dataTransfer.files);
    };

    $scope.postMsg = function () {
        var msg = {
            ReceiverId: $scope.ReceiverId,
            ThreadId: $scope.ThreadId,
            Text: $scope.Text,
            When: new Date(),
            MessageId: uuidv4(),
            IsItFromMe: true
        };
        $scope.$parent.myHub.SendPrivateMessage(msg);
        $scope.Text = '';
    }

    $scope.$parent.myHub.on('UpdateMessagePhotos', function (resp) {
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
    $scope.Files = [];
    $scope.$parent.myHub.on('MyMessageSent', function (resp) {
        $scope.$apply(function () {
            var convo = $scope.myConversationThreads.filter(function (p) {
                return p.ThreadId == resp.ThreadId && p.UserId == resp.ReceiverId;
            })[0];
            convo.When = resp.When;
            convo.MessageId = resp.MessageId;
            convo.Text = resp.Text;
            convo.FriendlyTime = resp.FriendlyTime;
            convo.Read = convo.Opened ? true : false;

            if ($scope.uppy.getFiles().length > 0) {
                resp.Files = [];
                $scope.uppy.run();
                $scope.uppy.upload().then((result) => {
                    var files = Array.from(result.successful);
                    $scope.Files = [];
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
                        $scope.Files.push(fileObj);
                        resp.Files.push(id);
                    });

                    if ($scope.Files.length > 0) {
                        $http.post(root + 'api/Messages/SaveMessageFiles', $scope.Files).then(function success(response) {
                            $scope.uppy.reset();
                            resp.FilesCount = $scope.Files.length;
                            $scope.Messages[$scope.openedThreadId].push(resp);
                            $scope.$parent.myHub.SendPhotoIdsToUser(resp);
                        }, function error() { });
                    }
                });
            } else {
                $scope.Messages[$scope.openedThreadId].push(resp);
            }
            $scope.hasNewMessage = true;
        });
    });

    $scope.$parent.myHub.on('UpdateUserStatus', function (resp) {
        $scope.$apply(function () {
            var openedThreadId = resp.openedThreadId;
            var convo = $scope.myConversationThreads.filter(function (p) {
                return p.ThreadId == resp.msgDto.ThreadId && (p.UserId == resp.msgDto.ReceiverId || p.UserId == resp.msgDto.SenderId);
            })[0];


            if (convo) {
                console.log(convo, resp.Status);
                if (resp.Status == 'Typing...') {
                    convo.isTyping = true;
                } else {
                    convo.isTyping = false;
                }
                if ($scope.Messages[openedThreadId]) {
                    $scope.status = resp.Status;
                }
            }
        });
    });


    var typingInterval = 3000;
    var typingTimer;
    $scope.UserTypingChatBox = function (event) {

        if (event.which === 13 && $scope.Text != '') {
            clearInterval(typingTimer);
            typingTimer = setTimeout(doneTyping, typingInterval);
            $scope.postMsg();
        }

        if (keyPressCount++ % 10 == 0) {
            clearInterval(typingTimer);
            typingTimer = setTimeout(doneTyping, typingInterval);
            var msg = {
                ReceiverId: $scope.ReceiverId,
                ThreadId: $scope.ThreadId,
                Text: $scope.Text,
                When: new Date(),
                MessageId: uuidv4(),
                IsItFromMe: true
            };
            $scope.$parent.myHub.UserIsTypingPrivateChat(msg);
        }
    }


    $('#uppyFileDialog').on('change', function (event) {
        addFilesToUppy(event.target.files);
    });


    function addFilesToUppy(files) {
        Array.from(files).forEach((a) => {
            try {
                $scope.uppy.addFile({
                    data: a,
                    name: a.name,
                    type: a.type
                });
            }
            catch (erro) {
                console.log(erro, 'bilal');
            }
        });

        $scope.$apply(function () {
            if ($scope.uppy.getFiles().length > 0)
                $scope.hasFilesInUppy = true;
        });
    }

    $scope.openFileFileDialog = function () {
        $('#uppyFileDialog').click();
    }

    $scope.hasFilesInUppy = false;

    function doneTyping() {
        var msg = {
            ReceiverId: $scope.ReceiverId,
            ThreadId: $scope.ThreadId,
            Text: $scope.Text,
            When: new Date(),
            MessageId: uuidv4(),
            IsItFromMe: true
        };
        $scope.$parent.myHub.UserDoneTypingPrivateChat(msg);
    }

    $scope.$parent.myHub.on('UserOnLine', function (resp) {
        $scope.$apply(function () {
            angular.forEach($scope.myConversationThreads, function (p) {
                if (p.UserId == resp) {
                    p.Online = true;
                    if ($scope.Messages[p.ThreadId + '_' + p.UserId])
                        $scope.status = 'Active Now';
                }
            });
        });
    });

    $scope.$parent.myHub.on('UserOffLine', function (resp) {
        $scope.$apply(function () {
            angular.forEach($scope.myConversationThreads, function (p) {
                if (p.UserId == resp.UserId) {
                    p.Online = false;
                    if ($scope.Messages[p.ThreadId + '_' + p.UserId])
                        $scope.status = resp.Status;
                }
            });
        });
    });

    function getUserLastSeen() {
        if (!($scope.ReceiverId == '' || $scope.ThreadId == '')) {
            var msg = {
                ReceiverId: $scope.ReceiverId,
                ThreadId: $scope.ThreadId,
                Text: $scope.Text,
                When: new Date(),
                MessageId: uuidv4(),
                IsItFromMe: true
            };
            $scope.$parent.myHub.GetLastSeenPrivateChat(msg);
        } else {
            //console.log('a');
        }
    }

    $scope.$parent.myHub.on('ReceiveMessage', function (resp) {

        $scope.$apply(function () {
            var msg = resp.msgDto;
            var openedThreadId = resp.openedThreadId;
            if ($scope.Messages[openedThreadId]) {
                $scope.Messages[openedThreadId].push(msg);//
                $scope.hasNewMessage = true;
            }
            var convo = $scope.myConversationThreads.filter(function (p) {
                return p.ThreadId == msg.ThreadId && p.UserId == msg.SenderId;
            })[0];

            //console.log(msg, resp);
            if (convo == null || convo == undefined) {
                $scope.myConversationThreads.push({
                    When: msg.When,
                    MessageId: msg.MessageId,
                    Text: msg.Text,
                    Read: false,
                    ThreadId: msg.ThreadId,
                    UserId: msg.SenderId,
                    UnreadMsgs: resp.UnreadCount,
                    FriendlyTime: msg.FriendlyTime,
                    Name: msg.SenderName
                });
            } else {

                convo.When = msg.When;
                convo.MessageId = msg.MessageId;
                convo.Text = msg.Text;
                convo.FriendlyTime = msg.FriendlyTime;
                convo.UnreadMsgs = convo.Opened ? 0 : resp.UnreadCount
            }

            //console.log($scope.myConversationThreads);
        });
    });

    $scope.openedThreadId = '';

    $scope.Messages = {};
    $scope.myConversationThreads = [];
    $scope.ConversationName = '';
    $scope.ReceiverId = '';
    $scope.Text = '';
    $scope.ThreadId = '';

    var lastSeenTimer;
    $scope.deleteMsg = function (obj) {
        $http.post(root + 'api/Messages/DeleteMsg', obj)
            .then((resp) => {
                for (var i = 0; i < $scope.Messages[$scope.openedThreadId].length; i++)
                    if ($scope.Messages[$scope.openedThreadId][i].MessageId === obj.MessageId) {
                        $scope.Messages[$scope.openedThreadId].splice(i, 1);
                        break;
                    }
                //
            }, (resp) => { })//
    }
    $scope.getChats = function (c) {
        $scope.open = true;
        if (!c.Opened) {
            $timeout(function () {
                $scope.uppy = new Uppy.Core({
                    restrictions: {
                        allowedFileTypes: ['image/*']
                    }
                })
                    .use(Uppy.Dashboard, {
                        id: 'Dashboard',
                        target: '#uppy-uploader',
                        //trigger: '#uppy-uploader',
                        metaFields: [],
                        //trigger: '#uppy-select-files',
                        inline: true,
                        height: 200,
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
                        fileManagerSelectionType: 'files',
                        proudlyDisplayPoweredByUppy: false,
                        onRequestCloseModal: () => $scope.closeUppyModal(),
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

                $scope.uppy.on('file-removed', (file, reason) => {
                    $timeout(() => {
                        $scope.$apply(() => {
                            $scope.hasFilesInUppy = $scope.uppy.getFiles().length > 0
                        });
                    });
                });
            }, 1000);


            keyPressCount = 0;
            $scope.ConversationName = c.Name;
            $scope.ReceiverId = c.UserId;
            $scope.ThreadId = c.ThreadId;
            clearInterval(lastSeenTimer);

            c.Read = true;
            $scope.openedThreadId = c.ThreadId + '_' + c.UserId;
            angular.forEach($scope.myConversationThreads, function (p) {
                p.Opened = false; //set them all to false
            });
            c.Opened = true;
            c.UnreadMsgs = 0;
            c.PageIndex = 1;

            $scope.pageIndex = 1;
            $scope.loading = false;
            $scope.lastRecord = false;
            $scope.hasNewMessage = true;
            $scope.convoObj = c;

            $http.post(root + 'api/Messages/GetConversationMessages', c).then(resp => {
                $scope.Messages[c.ThreadId + '_' + c.UserId] = resp.data;
                lastSeenTimer = setTimeout(getUserLastSeen, typingInterval);
            }, resp => {
            });
        }
    }

    $scope.closeDrawer = function () {
        $scope.open = false;
    }

    $scope.GetConversationThreads();

    $scope.deleteChat = function () {
        if ($scope.Messages[$scope.ThreadId + '_' + $scope.ReceiverId]) {
            delete $scope.Messages[$scope.ThreadId + '_' + $scope.ReceiverId];
            $http.post(root + 'api/Messages/DeleteConversation', { ThreadId: $scope.ThreadId, UserId: $scope.ReceiverId }).then(function (resp) {
                $scope.Messages[$scope.ThreadId + '_' + $scope.ReceiverId] = [];
            }, function (resp) {
                //console.log(resp);
            });
        }
    }
    $scope.pageIndex = 1;
    $scope.loading = false;
    $scope.lastRecord = false;
    $scope.hasNewMessage = true;
    //
    $scope.getNextPageData = function () {
        $scope.pageIndex = $scope.pageIndex + 1
        var deferred = $q.defer();
        if (!$scope.loading && !$scope.lastRecord) {
            $scope.loading = true;
            // simulate an ajax request
            $http.post(root + 'api/Messages/GetConversationMessages', {
                ThreadId: $scope.ThreadId,
                UserId: $scope.ReceiverId,
                PageIndex: $scope.pageIndex
            }).then(resp => {
                if (resp.data.length == 0) {
                    $scope.lastRecord = true;
                } else {
                    $scope.lastRecord = false;
                }
                $scope.loading = false;

                $scope.hasNewMessage = false;
                //});
                $.merge(resp.data, $scope.Messages[$scope.openedThreadId]);
                $scope.Messages[$scope.openedThreadId] = resp.data;
                deferred.resolve();
            });
        } else {
            deferred.reject();
        }
        return deferred.promise;
    }

    //$scop

})