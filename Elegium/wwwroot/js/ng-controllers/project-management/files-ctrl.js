myApp.controller('files-ctrl', function ($stateParams, $scope, $http, $state, $window, $window, $uibModal, $timeout) {
    $timeout(function () {
        //$state.reload();
        //links tabs
        $scope.linkObj = {
            Name: "",
            Url: "",
            DocumentCategoryId: parseInt($stateParams.docCatId)
        };

        $scope.links = [];

        $scope.tabContentLength = {

        };



        $http.get(root + 'api/DocumentCategories/GetDocumentCategoryObj/' + $stateParams.docCatId)
            .then(
                resp => {
                    if ($state.$current.name == 'documents.documentcategory.files.fileProfile') {
                        $state.$current.parent.parent.data.label = resp.data.DocumentName;
                    } else {
                        $state.$current.parent.data.label = resp.data.DocumentName;
                    }
                    $state.$current.data.label = resp.data.Name;
                    //$scope.title = resp.data.Name;
                    $scope.documentCatObj = resp.data;
                    console.log($state.$current, 'files');
                },
                err => {

                });

        $scope.getLinks = function () {
            $http.get(root + 'api/Links/GetLinks/' + $stateParams.docCatId)
                .then(
                    resp => {
                        $scope.links = resp.data;
                    },
                    err => {

                    }
                );
        }

        $http.get(root + 'api/Documents/GetDocumentCategorySummary/' + $stateParams.docCatId).then(resp => {
            $scope.tabContentLength = resp.data;
            $scope.projectId = $scope.tabContentLength.projectId;
        }, err => {
        });

        $scope.cancelLinkCreation = function () {
            $scope.linkObj.Name = "";
            $scope.linkObj.Url = "";
        }

        $scope.createLink = function () {
            $http.post(root + 'api/Links/PostLink', $scope.linkObj).then(resp => {
                $scope.getLinks();
                $scope.linkObj.Name = "";
                $scope.linkObj.Url = "";
                $scope.tabContentLength.linksCount = $scope.tabContentLength.linksCount + 1;
            }, err => {
            });
        }

        $scope.deleteLink = function (link) {
            var index = $scope.links.indexOf(link);
            if (index > -1)
                $scope.links.splice(index, 1);
            $http.post(root + 'api/Links/DeleteLink/' + link.Id).then(resp => {
                //$scope.getLinks();
                $scope.tabContentLength.linksCount = $scope.tabContentLength.linksCount - 1;
                if ($scope.tabContentLength.linksCount < 0) {
                    $scope.tabContentLength.linksCount = 0;
                }
            }, err => {
            });
        }

        $scope.openLink = function (url) {
            $window.open(url, '_blank');
        }

        $scope.getLinks();

        //end links tab


        //start comments tab

        $scope.comments = [];
        $scope.commentObj = {
            Text: "",
            newComment: true,
            DocumentCategoryId: parseInt($stateParams.docCatId),
            MentionUsers: null,
            ProjectId: parseInt($stateParams.id)
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
                        DocumentCategoryId: parseInt($stateParams.docCatId),
                        MentionUsers: o,
                        Id: 0,
                        MarkupText: markup,
                        ProjectId: parseInt($stateParams.id)
                    };
                    $http.post(root + 'api/Comments/PostComment', $scope.commentObj).then(resp => {
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
            $http.get(root + 'api/Comments/GetComments/' + $stateParams.docCatId).then(resp => {
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
            $http.post(root + 'api/Comments/DeleteComment/' + obj.Id).then(resp => {

            }, err => {
            });
        }

        $scope.getComments();
        //end comments tab

        //tasks tab


        $scope.tasks = [];

        $scope.goToTaskProfile = function (taskId) {
            $state.go('tasks.mytasks.taskprofile', { taskId: taskId, id: $stateParams.id  });
        }

        var initializeTaskObj = function () {
            $scope.taskObj = {
                AssignedTo: [],
                Description: "",
                Title: "",
                HasDeadline: false,
                LinkedToSection: false,
                DocumentCategoryId: parseInt($stateParams.docCatId),
                Section: null,
                Deadline: "",
                Id: 0,
                ProjectId: parseInt($stateParams.id),
                Section: "",
                HasSection: false,
                SectionUrl: "",
                Section: ""
            }
        }

        initializeTaskObj();

        $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + $stateParams.id).then(resp => {
            $scope.projectUsersAndGroups = resp.data;
        }, err => {
        });

        $scope.groupUsers = function (item) {
            return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
        }

        $scope.cancelTaskCreation = function () {
            initializeTaskObj();
            if (!$scope.taskObj.HasDeadline)
                $('#toggleCalendarDeadline').trigger('click');
        }

        $scope.getTasks = function () {
            $http.get(root + 'api/ProjectTasks/GetProjectTasks/' + $stateParams.docCatId).then(resp => {
                $scope.tasks = resp.data;
            }, err => {
            });
        }

        $scope.createTask = function () {
            $scope.tabContentLength.tasksCount = $scope.tabContentLength.tasksCount + 1;
            $http.post(root + 'api/ProjectTasks/PostProjectTask/', $scope.taskObj).then(resp => {
                initializeTaskObj();
                $scope.tasks.push(resp.data);
            }, err => {
            });
        }

        $scope.deleteTask = function (event, task) {
            //event.stopPropagation();
            var index = $scope.tasks.indexOf(task);
            if (index > -1)
                $scope.tasks.splice(index, 1);

            $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
                $scope.tabContentLength.tasksCount = $scope.tabContentLength.tasksCount - 1;
                if ($scope.tabContentLength.tasksCount < 0) {
                    $scope.tabContentLength.tasksCount = 0;
                    //$scope.$apply(function () {
                    //});
                }
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

        $scope.getTasks();
        //end tasks tab

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
                    fileObj.DocumentCategoryId = parseInt($stateParams.docCatId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    fileObj.Default = file.name == 'thumbnail.png' || $scope.isItDocDacThumbnail ? true : false;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinUppy = true;
                    });
                    $http.post(root + 'api/DocumentFiles/PostDocumentFiles', $scope.uploadedFiles).then(
                        function success(resp) {
                            $scope.isItDocDacThumbnail = false;
                            if (resp.data.length > 0) {
                                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                resp.data.forEach((file) => {
                                    $scope.files.push(file);
                                    if (file.Default)
                                        $scope.defaultImage = file.FileId;
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
            console.log('err',err);
        }
        $scope.hasFilesinUppy = false;

        $('#projectFile').on('change', function (event) {
            $scope.isItDocDacThumbnail = false;
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
                $scope.isItDocDacThumbnail = false;
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

        $scope.getDocumentCatFiles = function () {
            $http.get(root + 'api/DocumentFiles/GetDocumentFiles/' + $stateParams.docCatId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                $scope.defaultImage = resp.data.FileId;
                $scope.files = resp.data.list;
            }, err => {
            });
        }

        $scope.setDocCatThumbnail = function () {
            //document.getElementById('idDocCatThumbnailFile').click();
            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/croppie-playground.html',
                controller: 'CroppiePlayCtrl',
                size: 'lg',
                resolve: {
                    title: function () {
                        return "Select Thumbnail";
                    },
                    image: function () {
                        return $scope.ProjectLogo;
                    },
                    width: function () { return 750 },
                    height: function () { return 300 }
                }
            });
            modalInstance.result.then(function () {
                //on ok button press 
            }, function (data) {
                    let promiseSource = fetch(data.source);
                    let promiseImage = fetch(data.image);
                    Promise.all([promiseSource, promiseImage]).then((results) => {
                        let sourceBlobPromise = results[0].blob();
                        let imageBlobPromise = results[1].blob();
                        Promise.all([sourceBlobPromise, imageBlobPromise]).then((blobs) => {
                            const file1 = new File([blobs[0]], "source.png", { type: "image/png" });
                            const file2 = new File([blobs[1]], "thumbnail.png", { type: "image/png" });
                            addFilesToUppy([file1, file2]);
                        });
                    });
            });
        }

        $scope.isItDocDacThumbnail = false;

        $('#idDocCatThumbnailFile').on('change', function (event) {
            addFilesToUppy(event.target.files);
            $scope.isItDocDacThumbnail = true;
        });

        $scope.getDocumentCatFiles();

        $scope.openProjectFileViewer = function (file) {
            //console.log(project);
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/documents-and-files/files-and-documents-viewer.html',
                controller: 'projectFileViewerCtrl',
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
            $http.post(root + 'api/DocumentFiles/DeleteDocumentFiles/' + file.Id).then(resp => {
                //$scope.getLinks();
                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                if ($scope.tabContentLength.filesCount < 0) {
                    $scope.tabContentLength.filesCount = 0;
                }
            }, err => {
            });
        }

        $scope.makeDefault = function (event, file) {
            event.stopPropagation();
            $http.post(root + 'api/DocumentFiles/MakeDefault/' + file.Id + '/' + file.DocumentCategoryId).then(resp => {
                //$scope.getLinks();
                $scope.defaultImage = file.FileId;
            }, err => {
            });
        }
    });

    // }

    //end files and images tab
}).controller('projectFileViewerCtrl', function (fileObj, $http, $scope, $uibModalInstance, projectUsersAndGroups, $state, $timeout, projectId, $stateParams) {
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
                $http.get(root + 'api/DocumentFiles/NextPrevFileId/' + $scope.file.Id + '/' + flag + '/' + $scope.file.DocumentCategoryId).then(resp => {
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

            $scope.navigateToFileDetailsView = function (file) {
                $state.go("documents.documentcategory.files.fileProfile", { fileId: file.Id, name: file.Name });
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