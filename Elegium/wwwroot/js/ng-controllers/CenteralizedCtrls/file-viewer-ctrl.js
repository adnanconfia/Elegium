myApp.controller('fileViewerCtrl', function ($http, $state, $scope, toaster, $stateParams, $uibModalInstance, fileObj, allFiles, projectUsersAndGroups, fileProfileLink, $state, $timeout) {
    $scope.hasloaded = false;
    

    $uibModalInstance.rendered.then(function () {
        $scope.hasloaded = true;

        
        
        setTimeout(function () {
            $scope.file = fileObj;
            $scope.allFiles = allFiles;
            $scope.projectUsersAndGroups = projectUsersAndGroups;
            $scope.sendFileAsMessage = {
                SendTo: [],
                Message: ""
            };

            $scope.goFileDetailViewUrl = function () {
                $state.go('home.fileProfile', { fileId: $scope.file.Id, name: $scope.file.Name, id: $stateParams.id });
                $scope.cancel();
            }
            //$timeout(function () {
            //}, 100);

            $scope.nextPrev = function (flag) {

                $scope.sendFileAsMessage = {
                    SendTo: [],
                    Message: ""
                };

                var currIndex = $scope.allFiles.indexOf($scope.file);

                if (flag == 'N') {
                    if (currIndex == $scope.allFiles.length - 1) {
                        $scope.file = $scope.allFiles[0];
                    }
                    else {
                        $scope.file = $scope.allFiles[currIndex + 1];
                    }

                    $scope.fileComments = [];
                    $scope.fileCommentObj = {
                        Text: "",
                        newComment: true,
                        DocumentFileId: parseInt($scope.file.Id),
                        MentionUsers: null,
                        ProjectId: parseInt($stateParams.id)
                    };
                }
                else if (flag == 'P') {
                    if (currIndex == 0) {
                        $scope.file = $scope.allFiles[$scope.allFiles.length - 1];
                    }
                    else {
                        $scope.file = $scope.allFiles[currIndex - 1];
                    }

                    $scope.fileComments = [];
                    $scope.fileCommentObj = {
                        Text: "",
                        newComment: true,
                        DocumentFileId: parseInt($scope.file.Id),
                        MentionUsers: null,
                        ProjectId: parseInt($stateParams.id)
                    };
                }

                $scope.getFileComments();
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
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'Picture has been sent!',
                    });
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

            //$scope.navigateToFileDetailsView = function (file) {
            //    $state.go(fileProfileLink, { fileId: file.Id, name: file.Name });
            //    $scope.cancel();
            //}



            //Start comments code
            $scope.fileComments = [];
            $scope.fileCommentObj = {
                Text: "",
                newComment: true,
                DocumentFileId: parseInt($scope.file.Id),
                MentionUsers: null,
                ProjectId: parseInt($stateParams.id)
            };

            $scope.postFileComment = function () {
                //let projectId = window.location.hash.split('/')[1]
                //return;
                $('#fileCommentTextArea').mentionsInput("val", function (e) {
                    e != "" && $('#fileCommentTextArea').mentionsInput("getMentions", function (o) {
                        //console.log(o,'1111');
                        var res = getAllMatches(/(@)\[(.*?)\]\((.*?):(.*?)\)/g, e);
                        var markup = e;
                        res.forEach(function (item) {
                            e = e.replace(item[0],
                                item[3] == 'user' ? '<a href="#/professionaldetails/' + item[4] + '" class="font-weight-bold">' + item[2] + '</a>' :

                                    (item[3] == 'units' ? '<a href=#/' + parseInt($stateParams.id) + '/crew class="font-weight-bold">' + item[2] + '</a>' :

                                        (item[3] == 'groups' ? '<a href=#/' + parseInt($stateParams.id) + '/crew class="font-weight-bold">' + item[2] + '</a>' : 'Users')))
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
        }, 150);

    });
});