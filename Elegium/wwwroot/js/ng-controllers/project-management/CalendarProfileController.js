myApp.controller('CalendarProfileController', function ($stateParams, $scope, $http, $state, $window, $window, $uibModal, $timeout) {

    $timeout(function () {

        var calendarId = parseInt($stateParams.calendarId);

        $scope.tabContentLength = {
            filesCount: 0,
            commentsCount: 0,
            tasksCount: 0
        };

        $http.get(root + 'api/Calendar/GetEvent/' + calendarId).then(resp => {
            $scope.event = resp.data.list;

            $scope.event.StartDate = new Date($scope.event.StartDate);
            $scope.event.EndDate = new Date($scope.event.EndDate);

            $scope.showTime = $scope.event.StartTime != '' || $scope.event.EndTime != '';

            $state.current.data.label = resp.data.list.Title;
            $scope.tabContentLength = resp.data.tabContentLength;

            $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + $stateParams.id).then(resp => {
                $scope.projectUsersAndGroups = resp.data;
            }, err => {
            });

            $http.get(root + 'api/CalenderCategories/GetCalenderCategories/' + $stateParams.id).then(resp => {
                //console.log(resp);
                $scope.categories = resp.data;

            }, err => {

            });
        },
            err => {

            }
        );

        $scope.selectCategory = function (id) {
            $scope.event.CalenderCategoryId = id;
        }

        $scope.tasks = [];

        $scope.groupUsers = function (item) {
            return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
        }

        $http.get(root + 'api/Calendar/GetTasks/' + calendarId).then(resp => {
            $scope.tasks = resp.data;
        },
            err => {

            }
        );

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
                EventId: parseInt(calendarId),
                Section: "",
                HasSection: false,
                SectionUrl: "",
                Section: ""
            }
        }

        initializeTaskObj();

        $scope.cancelTaskCreation = function () {
            initializeTaskObj();
            if (!$scope.taskObj.HasDeadline)
                $('#deadline-calendar').removeClass('show');

            if (!$scope.taskObj.HasSection)
                $('#section-selector').removeClass('show');
        }

        $scope.createTask = function () {
            $http.post(root + 'api/Calendar/PostTask/', $scope.taskObj).then(resp => {
                initializeTaskObj();
                $scope.tabContentLength.tasksCount = $scope.tabContentLength.tasksCount + 1;
                $scope.tasks.push(resp.data);
            }, err => {
            });
        }

        $scope.updateCalendar = function (task) {
            $http.post(root + 'api/Calendar/PostEvent/', $scope.event).then(resp => {
                $state.current.data.label = resp.data.Title;
            }, err => {
            });
        }

        $scope.changeTaskStatus = function (event, task) {
            event.stopPropagation();
            $http.post(root + 'api/ProjectTasks/ChangeProjectTaskStatus/', task).then(resp => {
                var index = $scope.tasks.indexOf(task);
                if (index > -1)
                    $scope.tasks[index] = resp.data;
            }, err => {
            });
        }

        $scope.deleteTask = function (event, task) {
            //event.stopPropagation();
            $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
                var index = $scope.tasks.indexOf(task);
                if (index > -1)
                    $scope.tasks.splice(index, 1);
            }, err => {
            });
        }

        $scope.goToTaskProfile = function (taskId) {
            $state.go('tasks.mytasks.taskprofile', { taskId: taskId, id: $stateParams.id });
        }

        //files and images

        $scope.files = [];
        $scope.openProjectFileDialog = function () {
            $('#projectFile').click();
        }
        try {
            $scope.uppy = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'ringring',
                    target: '#uppy-uploader',
                    allowMultipleUploads: true,
                    //trigger: '#uppy-uploader',
                    metaFields: [],
                    //trigger: '#uppy-select-files',
                    inline: true,
                    height: 200,
                    //defaultTabIcon: defaultTabIcon,
                    showLinkToFileUploadResult: false,
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
                    proudlyDisplayPoweredByUppy: false,
                    //onRequestCloseModal: () => $scope.closeUppyModal(),
                    showSelectedFiles: true,
                    showRemoveButtonAfterComplete: false,
                    //locale: defaultLocale,
                    browserBackButtonClose: false,
                    theme: 'light'
                })
                .use(Uppy.Tus,
                    {
                        endpoint: root + 'files/',
                        resume: true,
                        retryDelays: [0, 1000, 3000, 5000],
                        chunkSize: 5242880
                    });

            $scope.uppy.on('complete', (result) => {
                var files = Array.from(result.successful);
                $scope.uploadedFiles = [];
                files.forEach((file) => {
                    // file: { id, name, type, ... }
                    // progress: { uploader, bytesUploaded, bytesTotal }
                    var resp = file.response.uploadURL;
                    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                    var fileObj = {};
                    fileObj.FileId = id;
                    fileObj.Name = file.name;
                    //fileObj.Type = 'V';
                    fileObj.EventId = parseInt(calendarId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    //fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinUppy = true;
                    });
                    $http.post(root + 'api/Calendar/PostFiles', $scope.uploadedFiles).then(
                        function success(resp) {
                            if (resp.data.length > 0) {
                                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                resp.data.forEach((file) => {
                                    $scope.files.push(file);
                                });
                                $scope.uppy.reset();
                            }
                        }
                        , function error() { });
                }
            });
            $scope.uppy.on('file-removed', (file, reason) => {
                $timeout(() => {
                    $scope.$apply(() => {
                        $scope.hasFilesinUppy = $scope.uppy.getFiles().length > 0
                    });
                });
            });
        }
        catch (err) {
            console.log('err', err);
        }
        $scope.hasFilesinUppy = false;

        $('#projectFile').on('change', function (event) {
            addFilesToUppy(event.target.files);
        });

        var holder = document.getElementById('draggableContainer');
        var lastTarget;
        //bilal
        if (holder) {
            holder.ondragenter = function (e) {
                lastTarget = e.target;
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
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
                this.className = ' project-hover-files';
            }

            holder.ondragleave = function (e) {
                e.preventDefault();
                e.preventDefault();
                if (lastTarget === e.target) {
                    this.className = '';
                }
            };

            holder.ondragend = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = '';
            }

            holder.ondrop = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = 'card bg-none b-none';
                addFilesToUppy(e.dataTransfer.files);
            };
        }

        var addFilesToUppy = function (files) {
            //$scope.$apply(function () {
            $scope.hasFilesinUppy = true;
            //});

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
        }

        $scope.filesPaging = {
            page: 1,
            size: 12
        }

        $scope.getTaskFiles = function () {
            $http.get(root + 'api/Calendar/GetFiles/' + calendarId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                $scope.defaultImage = resp.data.FileId;
                $scope.files = resp.data.list;
            }, err => {
            });
        }

        $scope.getTaskFiles();

        $scope.openProjectFileViewer = function (file) {
            //console.log(project);
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/documents-and-files/files-and-documents-viewer.html',
                controller: 'calendarFileViewerCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    fileObj: function () {
                        return file;
                    },
                    projectUsersAndGroups: function () {
                        return $scope.projectUsersAndGroups
                    },
                    projectId: function () {
                        return $scope.projectId;
                    }
                }
            });
            modalInstance.result.then(function () {
            }, function (data) {
                console.log(data, 'from modal');
                //$scope.getProjects();
            });
        }

        $scope.deleteFile = function ($event, file) {
            $event.stopPropagation();
            var index = $scope.files.indexOf(file);
            if (index > -1)
                $scope.files.splice(index, 1);
            $http.post(root + 'api/Calendar/DeleteFiles/' + file.Id).then(resp => {
                //$scope.getLinks();
                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                if ($scope.tabContentLength.filesCount < 0) {
                    $scope.tabContentLength.filesCount = 0;
                }
            }, err => {
            });
        }

        //end files and images



        //start comments tab

        $scope.comments = [];
        var initCommentObj = function () {
            $scope.commentObj = {
                Text: "",
                newComment: true,
                EventId: parseInt(calendarId),
                MentionUsers: null
            };
        }

        initCommentObj();



        $('textarea.mention').mentionsInput({
            onDataRequest: function (mode, query, callback) {
                $http.get(root + 'api/Comments/GetMentions/' + $stateParams.id + '/' + query).then(resp => {
                    responseData = _.filter(resp.data, function (item) { return item.name.toLowerCase().indexOf(query.toLowerCase()) > -1 });
                    callback.call(this, responseData);
                }, err => {
                    alert('something went wrong');
                });
            }
        });

        $scope.sendComment = function () {
            $('textarea.mention').mentionsInput("val", function (e) {
                e != "" && $('textarea.mention').mentionsInput("getMentions", function (o) {
                    var res = getAllMatches(/(@)\[(.*?)\]\((.*?):(.*?)\)/g, e);
                    var markup = e;
                    res.forEach(function (item) {
                        e = e.replace(item[0],
                            item[3] == 'user' ? '<a href="#/professionaldetails/' + item[4] + '" class="font-weight-bold">' + item[2] + '</a>' :

                                (item[3] == 'units' ? '<a href=#/' + $scope.projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' :

                                    (item[3] == 'groups' ? '<a href=#/' + $scope.projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' : 'Users')))
                    });
                    $scope.commentObj = {
                        Text: e,
                        newComment: false,
                        EventId: parseInt(calendarId),
                        MentionUsers: o,
                        Id: 0,
                        MarkupText: markup
                    };
                    $http.post(root + 'api/Calendar/PostComment', $scope.commentObj).then(resp => {
                        $scope.comments.push(resp.data);

                        //$scope.commentObj.Text = "";
                        $('textarea.mention').mentionsInput("reset");
                        $scope.tabContentLength.commentsCount = $scope.tabContentLength.commentsCount + 1;
                        initCommentObj();
                    }, err => {
                    });
                });
            });
        }

        $scope.getComments = function () {
            //GetComments
            $http.get(root + 'api/Calendar/GetComments/' + calendarId).then(resp => {
                if (resp.data.length > 0)
                    $scope.commentObj.newComment = false;
                $scope.comments = resp.data;
            }, err => {
            });
        }

        $scope.deleteComment = function (obj) {
            var index = $scope.comments.indexOf(obj);
            if (index > -1) {
                $scope.comments.splice(index, 1);
            }
            $scope.tabContentLength.commentsCount = $scope.tabContentLength.commentsCount - 1;
            if ($scope.tabContentLength.commentsCount < 0) {
                $scope.tabContentLength.commentsCount = 0;
                $scope.commentObj.newComment = true;
            }
            $http.post(root + 'api/Calendar/DeleteComment/' + obj.Id).then(resp => {

            }, err => {
            });
        }

        $scope.getComments();
        //end comments tab

    });
}).controller('calendarFileViewerCtrl', function (fileObj, $http, $scope, $uibModalInstance, projectUsersAndGroups, $state, $timeout, projectId, $stateParams) {
    $scope.hasloaded = false;
    $uibModalInstance.rendered.then(function () {
        $scope.hasloaded = true;
        $timeout(function () {
            $scope.file = fileObj;
            $scope.projectUsersAndGroups = projectUsersAndGroups;
            $scope.sendFileAsMessage = {
                SendTo: [],
                Message: ""
            };
            $scope.nextPrev = function (flag) {
                $scope.sendFileAsMessage = {
                    SendTo: [],
                    Message: ""
                };
                $http.get(root + 'api/Calendar/NextPrevFileId/' + $scope.file.Id + '/' + flag + '/' + $scope.file.EventId).then(resp => {
                    $scope.file = resp.data;
                    $scope.fileComments = [];
                    $scope.fileCommentObj = {
                        Text: "",
                        newComment: true,
                        DocumentFileId: parseInt($scope.file.Id),
                        MentionUsers: null,
                        ProjectId: parseInt($stateParams.id)
                    };
                    $scope.getFileComments();
                }, err => {
                });
            }
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }

            $scope.groupUsers = function (item) {
                return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
            }

            $scope.sendFileInMessage = function () {
                $scope.sendFileAsMessage.FileObj = $scope.file;
                $http.post(root + 'api/DocumentFiles/SendPrivateMessage/', $scope.sendFileAsMessage).then(resp => {
                    $scope.sendFileAsMessage = {
                        SendTo: [],
                        Message: "",
                        FileObj: {}
                    };
                    $('#closeNewMessageFile').trigger('click');
                }, err => {
                });
            }
            $scope.cancelSendFileInMessage = function () {
                $scope.sendFileAsMessage = {
                    SendTo: [],
                    Message: "",
                    FileObj: {}
                };
            }

            $scope.goToTaskProfile = function (taskId) {
                $state.go('tasks.mytasks.taskprofile', { taskId: taskId, id: $stateParams.id });
            }

            $scope.navigateToFileDetailsView = function (file) {
                $state.go("calendar.calendarprofile.fileProfile", { fileId: file.Id, name: file.Name });
                $scope.cancel();
            }
            //start comments tab
            console.log($scope.fileComments, 'bilal');
            $scope.fileComments = [];
            $scope.fileCommentObj = {
                Text: "",
                newComment: true,
                DocumentFileId: parseInt($scope.file.Id),
                MentionUsers: null,
                ProjectId: parseInt($stateParams.id)
            };

            $scope.postFileComment = function () {
                $('#fileCommentTextArea').mentionsInput("val", function (e) {
                    e != "" && $('#fileCommentTextArea').mentionsInput("getMentions", function (o) {
                        //console.log(o,'1111');
                        var res = getAllMatches(/(@)\[(.*?)\]\((.*?):(.*?)\)/g, e);
                        var markup = e;
                        res.forEach(function (item) {
                            e = e.replace(item[0],
                                item[3] == 'user' ? '<a href="#/professionaldetails/' + item[4] + '" class="font-weight-bold">' + item[2] + '</a>' :

                                    (item[3] == 'units' ? '<a href=#/' + projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' :

                                        (item[3] == 'groups' ? '<a href=#/' + projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' : 'Users')))
                        });
                        $scope.fileCommentObj = {
                            Text: e,
                            newComment: false,
                            DocumentFileId: parseInt($scope.file.Id),
                            MentionUsers: o,
                            Id: 0,
                            MarkupText: markup,
                            ProjectId: parseInt($stateParams.id)
                        };
                        $http.post(root + 'api/FileComments/PostComment', $scope.fileCommentObj).then(resp => {
                            $scope.fileCommentObj = angular.copy(resp.data);
                            $scope.fileComments.push(resp.data);

                            $scope.fileCommentObj.Text = "";
                            $('#fileCommentTextArea').mentionsInput("reset");
                        }, err => {
                        });
                    });
                });
            }

            $scope.getFileComments = function () {
                //GetComments
                $http.get(root + 'api/FileComments/GetComments/' + $scope.file.Id).then(resp => {
                    if (resp.data.length > 0)
                        $scope.fileCommentObj.newComment = false;
                    $scope.fileComments = resp.data;
                }, err => {
                });
            }

            $scope.deleteFileComment = function (obj) {
                var index = $scope.fileComments.indexOf(obj);
                if (index > -1) {
                    $scope.fileComments.splice(index, 1);
                }
                $http.post(root + 'api/FileComments/DeleteComment/' + obj.Id).then(resp => {

                }, err => {
                });
            }

            $scope.getFileComments();

            $('#fileCommentTextArea').mentionsInput({
                onDataRequest: function (mode, query, callback) {
                    $http.get(root + 'api/Comments/GetMentions/' + $stateParams.id + '/' + query).then(resp => {
                        responseData = _.filter(resp.data, function (item) { return item.name.toLowerCase().indexOf(query.toLowerCase()) > -1 });
                        callback.call(this, responseData);
                    }, err => {
                        alert('something went wrong');
                    });
                }
            });
            //end comments tab
        }, 100);
    });
});