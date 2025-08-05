myApp.controller('fileProfile-ctrl', function ($stateParams, $scope, $http, $stateParams, $state, $timeout) {
    $timeout(function () {
        $scope.fileDetailContentLength = {

        };

        $http.get(root + 'api/DocumentFiles/GetFileDetailSummary/' + $stateParams.fileId).then(
            resp => {
                $scope.fileDetailContentLength = resp.data;
                $scope.projectId = $stateParams.id;
            },
            err => {
            }
        );

        $http.get(root + 'api/DocumentFiles/GetDocumentFile/' + $stateParams.fileId)
            .then(
                resp => {
                    //console.log($state.current.data.label, $state.current);
                    //setTimeout(function () {
                    if ($state.$current.name == 'documents.documentcategory.files.fileProfile' || $state.$current.name == 'tasks.mytasks.taskprofile.fileProfile') {
                        $state.$current.data.label = resp.data.dto.Name;
                        //$state.$current.parent.data.label = resp.data.docCatName.Name;
                    }
                    $scope.fileObj = resp.data.dto;
                },
                err => {
                });

        $scope.updateFileName = function () {
            console.log($scope.fileObj);
            if ($scope.fileObj.Name && $scope.fileObj.Name != '') {
                $http.post(root + 'api/DocumentFiles/UpdateFileName', $scope.fileObj).then(resp => {
                    $state.current.data.label = $scope.fileObj.Name;
                    $scope.fileObj = resp.data;
                }, err => {
                });
            }
        }
        //file task code starts here

        $scope.fileTaskObj = {
            AssignedTo: [],
            Description: "",
            Title: "",
            HasDeadline: false,
            DocumentFilesId: parseInt($stateParams.fileId),
            Deadline: "",
            Id: 0,
            ProjectId: parseInt($stateParams.id)
        }

        $scope.fileTasks = [];

        $scope.goToTaskProfile = function (taskId) {
            $state.go('tasks.mytasks.taskprofile', { taskId: taskId, id: $stateParams.id });
        }

        $scope.cancelFileTaskCreation = function () {
            $scope.fileTaskObj = {
                AssignedTo: [],
                Description: "",
                Title: "",
                HasDeadline: false,
                DocumentFilesId: parseInt($stateParams.fileId),
                Deadline: "",
                Id: 0,
                ProjectId: parseInt($stateParams.id)
            }
            if (!$scope.fileTaskObj.HasDeadline)
                $('#toggleCalendarDeadline').trigger('click');
        }

        $scope.getFileTasks = function () {
            $http.get(root + 'api/FileTasks/GetFileTasks/' + $stateParams.fileId).then(resp => {
                $scope.fileTasks = resp.data;
            }, err => {
            });
        }

        $scope.createFileTask = function () {
            $scope.fileDetailContentLength.fileTasksCount = $scope.fileDetailContentLength.fileTasksCount + 1;
            $http.post(root + 'api/FileTasks/PostFileTask/', $scope.fileTaskObj).then(resp => {
                $scope.fileTaskObj = {
                    AssignedTo: [],
                    Description: "",
                    Title: "",
                    HasDeadline: false,
                    DocumentFilesId: parseInt($stateParams.fileId),
                    Deadline: "",
                    Id: 0,
                    ProjectId: parseInt($stateParams.id)
                };
                $scope.fileTasks.push(resp.data);
            }, err => {
            });
        }

        $scope.deleteFileTask = function (event, task) {
            event.stopPropagation();
            var index = $scope.fileTasks.indexOf(task);
            if (index > -1)
                $scope.fileTasks.splice(index, 1);

            $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
                $scope.fileDetailContentLength.fileTasksCount = $scope.fileDetailContentLength.fileTasksCount + 1;
                if ($scope.fileDetailContentLength.fileTasksCount < 0) {
                    $scope.fileDetailContentLength.fileTasksCount = 0;
                }
            }, err => {
            });
        }

        $scope.changeFileTaskStatus = function (event, task) {
            event.stopPropagation();
            $http.post(root + 'api/FileTasks/ChangeFileTaskStatus/', task).then(resp => {
                var index = $scope.fileTasks.indexOf(task);
                if (index > -1)
                    $scope.fileTasks[index] = resp.data;
            }, err => {
            });
        }

        $scope.getFileTasks();

        //file task code ends here


        //start comments tab

        $scope.fileComments = [];
        $scope.fileCommentObj = {
            Text: "",
            newComment: true,
            DocumentFileId: parseInt($stateParams.fileId),
            MentionUsers: null,
            ProjectId: parseInt($stateParams.id)
        };

        $scope.sendFileComment = function () {
            $('textarea.mention').mentionsInput("val", function (e) {
                e != "" && $('textarea.mention').mentionsInput("getMentions", function (o) {
                    var res = getAllMatches(/(@)\[(.*?)\]\((.*?):(.*?)\)/g, e);
                    var markup = e;
                    console.log(res, 'saeed');
                    res.forEach(function (item) {
                        e = e.replace(item[0],
                            item[3] == 'user' ? '<a href="#/professionaldetails/' + item[4] + '" class="font-weight-bold">' + item[2] + '</a>' :

                                (item[3] == 'units' ? '<a href=#/' + $scope.projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' :

                                    (item[3] == 'groups' ? '<a href=#/' + $scope.projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' : 'Users')))
                    });
                    $scope.fileCommentObj = {
                        Text: e,
                        newComment: false,
                        DocumentFileId: parseInt($stateParams.fileId),
                        MentionUsers: o,
                        Id: 0,
                        MarkupText: markup,
                        ProjectId: parseInt($stateParams.id)
                    };
                    $http.post(root + 'api/FileComments/PostComment', $scope.fileCommentObj).then(resp => {
                        $scope.fileCommentObj = angular.copy( resp.data);
                        $scope.fileComments.push(resp.data);
                        $scope.fileCommentObj.Text = "";
                        $('textarea.mention').mentionsInput("reset");
                        $scope.fileDetailContentLength.fileCommentsCount = $scope.fileDetailContentLength.fileCommentsCount + 1;
                        $scope.fileCommentObj.newComment = false;
                    }, err => {
                    });
                });
            });
        }

        $scope.getFileComments = function () {
            //GetComments
            $http.get(root + 'api/FileComments/GetComments/' + $stateParams.fileId).then(resp => {
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
            $scope.fileDetailContentLength.fileCommentsCount = $scope.fileDetailContentLength.fileCommentsCount - 1;
            if ($scope.fileDetailContentLength.fileCommentsCount < 0) {
                $scope.fileDetailContentLength.fileCommentsCount = 0;
                $scope.fileCommentObj.newComment = true;
            }
            $http.post(root + 'api/FileComments/DeleteComment/' + obj.Id).then(resp => {

            }, err => {
            });
        }

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

        $scope.getFileComments();
        //end comments tab


        //file comment code starts here

        //files and images tab
        $scope.versionFiles = [];
        $scope.openVersionFileDialog = function () {
            $('#newVersionFile').click();
        }
        try {
            $scope.uppyVersion = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'Dashboard1',
                    target: '#uppy-uploader-newversion',
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

            $scope.uppyVersion.on('complete', (result) => {
                var files = Array.from(result.successful);
                $scope.uploadedVersions = [];
                files.forEach((file) => {
                    // file: { id, name, type, ... }
                    // progress: { uploader, bytesUploaded, bytesTotal }
                    var resp = file.response.uploadURL;
                    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                    var fileObj = {};
                    fileObj.FileId = id;
                    fileObj.Name = file.name;
                    //fileObj.Type = 'V';
                    fileObj.DocumentFileId = parseInt($stateParams.fileId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    //fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedVersions.push(fileObj);
                });
                if ($scope.uploadedVersions.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasNewVersionFilesinUppy = true;
                    });
                    $http.post(root + 'api/VersionFiles/PostDocumentFiles', $scope.uploadedVersions).then(
                        function success(resp) {
                            if (resp.data.list.length > 0) {
                                $scope.fileDetailContentLength.fileVersionsCount = $scope.fileDetailContentLength.fileVersionsCount + $scope.uploadedVersions.length;
                                //resp.data.list.forEach((file) => {
                                    $scope.versionFiles = resp.data.list;
                                //});
                                $scope.defaultImage = resp.data.DefaultImage;
                                $scope.uppyVersion.reset();
                            }
                        }
                        , function error() { });
                }
            });
            $scope.uppyVersion.on('file-removed', (file, reason) => {
                
                    $scope.hasNewVersionFilesinUppy = $scope.uppyVersion.getFiles().length > 0
                //});
            });
        }
        catch (err) {
            console.log(err);
        }
        $scope.hasNewVersionFilesinUppy = false;

        $('#newVersionFile').on('change', function (event) {
            addVersionsToUppy(event.target.files);
        });

        var holder = document.getElementById('draggableContainerVersions');
        var lastTarget;

        //bilal
        if (holder) {
            console.log(holder);
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
                addVersionsToUppy(e.dataTransfer.files);
            };
        }

        var addVersionsToUppy = function (files) {
            $scope.$apply(function () {
                $scope.hasNewVersionFilesinUppy = true;
            });

            Array.from(files).forEach((a) => {
                try {
                    $scope.uppyVersion.addFile({
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

        $scope.versionFilesPaging = {
            page: 1,
            size: 12
        }

        $scope.getVersionFiles = function () {
            $http.get(root + 'api/VersionFiles/GetFileVersions/' + $stateParams.fileId + '/' + $scope.versionFilesPaging.page + '/' + $scope.versionFilesPaging.size).then(resp => {
                $scope.defaultImage = resp.data.FileId;
                $scope.versionFiles = resp.data.dbList;
            }, err => {
            });
        }

        $scope.getVersionFiles();

        $scope.deleteVersionFile = function ($event, file) {
            $event.stopPropagation();
            var index = $scope.versionFiles.indexOf(file);
            if (index > -1)
                $scope.versionFiles.splice(index, 1);
            $http.post(root + 'api/VersionFiles/DeleteVersionFiles/' + file.Id).then(resp => {
                //$scope.getLinks();
                $scope.fileDetailContentLength.fileVersionsCount = $scope.fileDetailContentLength.fileVersionsCount - 1;
                if ($scope.fileDetailContentLength.fileVersionsCount < 0) {
                    $scope.fileDetailContentLength.fileVersionsCount = 0;
                }
            }, err => {
            });
        }

        //file comment code ends here
    });
});