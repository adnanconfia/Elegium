myApp.controller('taskProfileController', function ($stateParams, $scope, $http, $state, $window, $window, $uibModal, $timeout) {

    $timeout(function () {

        $scope.tabContentLength = {
            filesCount: 0,
            commentsCount: 0,
            subTasksCount: 0
        };

        $http.get(root + 'api/Tasks/GetTask/' + $stateParams.taskId).then(resp => {
            $scope.task = resp.data.list;
            $scope.task.Deadline = new Date($scope.task.Deadline);
            $state.current.data.label = resp.data.list.Title;
            $scope.tabContentLength = resp.data.tabContentLength;


            $http.get(root + 'api/Documents/GetProjectWiseUserMenu/' + $stateParams.id).then(function success(response) {
                $scope.sections = response.data.projectUserMenu;
            }, function error() {
            });

            $http.get(root + 'api/Tasks/GetProjectObjects/' + $stateParams.id).then(function success(response) {
                $scope.objects = response.data;
            }, function error() {
            });
        },
            err => {

            }
        );

        $scope.subtasks = [];

        $http.get(root + 'api/Tasks/GetSubTasks/' + $stateParams.taskId).then(resp => {
            $scope.subtasks = resp.data;
        },
            err => {

            }
        );

        var initializeSubTaskObj = function () {
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
                ParentTaskId: parseInt($stateParams.taskId),
                Section: "",
                HasSection: false,
                SectionUrl: "",
                Section: ""
            }
        }

        initializeSubTaskObj();

        $scope.cancelTaskCreation = function () {
            initializeSubTaskObj();
            if (!$scope.taskObj.HasDeadline)
                $('#deadline-calendar').removeClass('show');

            if (!$scope.taskObj.HasSection)
                $('#section-selector').removeClass('show');
        }

        $scope.createSubTask = function () {
            $http.post(root + 'api/Tasks/PostProjectTask/', $scope.taskObj).then(resp => {
                initializeSubTaskObj();
                $scope.tabContentLength.subTasksCount = $scope.tabContentLength.subTasksCount + 1;
                $scope.subtasks.push(resp.data);
            }, err => {
            });
        }

        $scope.updateTask = function (task) {
            $http.post(root + 'api/Tasks/UpdateProjectTask/', $scope.task).then(resp => {
                $state.current.data.label = resp.data.Title;
            }, err => {
            });
        }

        $scope.changeTaskStatus = function (event, task) {
            event.stopPropagation();
            $http.post(root + 'api/ProjectTasks/ChangeProjectTaskStatus/', task).then(resp => {
                var index = $scope.subtasks.indexOf(task);
                if (index > -1)
                    $scope.subtasks[index] = resp.data;
            }, err => {
            });
        }

        $scope.deleteTask = function (event, task) {
            //event.stopPropagation();
            $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
                var index = $scope.subtasks.indexOf(task);
                if (index > -1)
                    $scope.subtasks.splice(index, 1);
            }, err => {
            });
        }

        //files and images


        //files and images tab
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
                    fileObj.ProjectTaskId = parseInt($stateParams.taskId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    //fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinUppy = true;
                    });
                    $http.post(root + 'api/Tasks/PostTaskFiles', $scope.uploadedFiles).then(
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
            $http.get(root + 'api/Tasks/GetTaskFiles/' + $stateParams.taskId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
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
                controller: 'taskFileViewerCtrl',
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
            $http.post(root + 'api/Tasks/DeleteTaskFiles/' + file.Id).then(resp => {
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
        $scope.commentObj = {
            Text: "",
            newComment: true,
            ProjectTaskId: parseInt($stateParams.taskId),
            MentionUsers: null
        };



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
                        ProjectTaskId: parseInt($stateParams.taskId),
                        MentionUsers: o,
                        Id: 0,
                        MarkupText: markup
                    };
                    $http.post(root + 'api/Tasks/PostComment', $scope.commentObj).then(resp => {
                        $scope.commentObj = resp.data;
                        $scope.comments.push(resp.data);

                        //$scope.commentObj.Text = "";
                        $('textarea.mention').mentionsInput("reset");
                        $scope.tabContentLength.commentsCount = $scope.tabContentLength.commentsCount + 1;
                        $scope.commentObj.newComment = false;
                    }, err => {
                    });
                });
            });
        }

        $scope.getComments = function () {
            //GetComments
            $http.get(root + 'api/Tasks/GetComments/' + $stateParams.taskId).then(resp => {
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
            $http.post(root + 'api/Tasks/DeleteComment/' + obj.Id).then(resp => {

            }, err => {
            });
        }

        $scope.getComments();
        //end comments tab

    });
}).controller('taskFileViewerCtrl', function (fileObj, $http, $scope, $uibModalInstance, projectUsersAndGroups, $state, $timeout, projectId, $stateParams) {
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
                $http.get(root + 'api/Tasks/NextPrevFileId/' + $scope.file.Id + '/' + flag + '/' + $scope.file.ProjectTaskId).then(resp => {
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
                $state.go('tasks.mytasks.taskprofile', { taskId: taskId });
            }

            $scope.navigateToFileDetailsView = function (file) {
                $state.go("tasks.mytasks.taskprofile.fileProfile", { fileId: file.Id, name: file.Name });
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