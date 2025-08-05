myApp.controller('ExternalUserDetailController', function ($scope, $rootScope, $window, $filter, $http, $uibModal, $timeout, toaster, $ngConfirm, $stateParams, $state) {
    console.log('state params:', $stateParams);
    $scope.userId = parseInt($stateParams.userId);
    $scope.externalUser = {};
    $scope.workingPositions = [];
    $scope.departments = [];
    $scope.countries = [];


    //change progile image
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
                width: function () { return 300 },
                height: function () { return 300 }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.isItDocDacThumbnail = true;
            fetch(data).then(res => res.blob())
                .then(blob => {
                    const file = new File([blob], "thumbnail.png", { type: "image/png" });
                    addExternalUserFilesToUppy([file]);
                });
        });
    }
    $('#idDocCatThumbnailFile').on('change', function (event) {
        addExternalUserFilesToUppy(event.target.files);
            $scope.isItDocDacThumbnail = true;
        });


    $scope.openFileViewer = function (file, allFiles) {
        //console.log(project);
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/centralized-templates/files-viewer.html',
            controller: 'fileViewerCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                fileObj: function () {
                    return file;
                },
                allFiles: function () {
                    return allFiles;
                }
            }
        });
        modalInstance.result.then(function () {
        }, function (data) {
            console.log(data, 'from modal');
            //$scope.getProjects();
        });
    }


    $scope.editExternalUser = false;
    $scope.editExternalUserNote = false;
    $scope.editNote = function () {
        $scope.editExternalUserNote = true;
    }
    $scope.editUser = function () {
        $scope.editExternalUser = true;
    }
    $scope.getExternalUser = function () {
        $http.get(root + 'api/ProjectExternalUsers/GetExternalUserById?userId=' + $scope.userId).then(function success(response) {
            //console.log('$scope.crewList', response);
            $scope.externalUser = response.data;
            $scope.editExternalUser = false;
            console.log('$scope.externalUser',$scope.externalUser);
            //Contract tab things

            if ($scope.externalUser.ContainsLOI)
                $scope.externalUser.ContainsLOI = new Date($scope.externalUser.ContainsLOI);
            if ($scope.externalUser.ContainsDealMemo)
                $scope.externalUser.ContainsDealMemo = new Date($scope.externalUser.ContainsDealMemo);
            if ($scope.externalUser.ContractCreated)
                $scope.externalUser.ContractCreated = new Date($scope.externalUser.ContractCreated);
            if ($scope.externalUser.ContractSent)
                $scope.externalUser.ContractSent = new Date($scope.externalUser.ContractSent);
            if ($scope.externalUser.ContractSigned)
                $scope.externalUser.ContractSigned = new Date($scope.externalUser.ContractSigned);
           // console.log(' $scope.crewUser', $scope.crewUser);
        }, function error(err) {

            console.log('$scope.crewList', err);
        });
    }
    $scope.close = function () {
        $scope.editExternalUser = false;
        $scope.editExternalUserNote = false;
        $scope.getExternalUser();
    }
    $scope.updateExternalUser = function (user) {
        console.log('user', user);
        $http.post(`${root}api/ProjectExternalUsers/UpdateExternalUser`, $scope.externalUser).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Information updated successfully!',
                });
                $scope.getExternalUser();
                $scope.editExternalUserNote = false;
            }
        }, function error(err) {
            console.log(err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }
    //Code for external User
    $scope.editExternalUserContact = false;
    $scope.externalUserContacts = [];
    $scope.addExternalUserContact = function () {
        var record = { IsEdit: true, IsNew: true }
        $scope.externalUserContacts.unshift(record);
        console.log($scope.externalUserContacts);
    }
    $scope.editExternalUserContact = function (record) {
        record.IsEdit = true;
    }
    $scope.closeExternalUserContact = function () {
        $scope.getExternalUserContact();
    }
    $scope.getExternalUserContact = function () {
        $http.get(root + 'api/ProjectExternalUsers/GetExternalUserContacts?userId=' + $scope.userId).then(function success(response) {
           
            $scope.externalUserContacts = response.data;
            console.log(' $scope.externalUserContacts', $scope.externalUserContacts);
        }, function error(err) {

            console.log('$scope.crewList', err);
        });
    }
    $scope.updateExternalUserContact = function (user) {
        console.log('user', user);
        $http.post(`${root}api/ProjectExternalUsers/UpdateExternalUserContact`, user).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Contact information updated successfully!',
                });
                $scope.getExternalUserContact();
                user.IsEdit = false;
            }
        }, function error(err) {
            console.log(err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }
    $scope.createExternalUserContact = function (user) {
       
        user.ExternalUserId = $scope.userId;
        console.log('user', user);
        $http.post(`${root}api/ProjectExternalUsers/CreateExternalUserContact`, user).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Contact has bee created successfully!',
                });
                $scope.getExternalUserContact();
                user.IsEdit = false;
            }
        }, function error(err) {
            console.log(err);
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }
    $scope.deleteExternalUserContact = function (record) {
        $http.delete(root + 'api/ProjectExternalUsers/DeleteExternalUserContact/' + record.Id).then(function success(response) {
            //$scope.crewList = response.data.data;
            toaster.pop({
                type: 'success',
                title: 'Success',
                body: 'Contact deleted successfully!',
            });
            $scope.getExternalUserContact();
        }, function error() { });
    }
    $scope.getExternalUserContact();
       //**********8 Start Code for  Images & Files**********
    $scope.externalUserFiles = [];
    $scope.hasFilesinExternalUser = false;
    $scope.openExternalUserFilesDialog = function () {
        $('#externalUserFilesFile').click();
    }
    $scope.tabContentLength = {

    };
    $scope.filesPaging = {
        page: 1,
        size: 12
    }
    $(document).ready(function () {
        $('#externalUserFilesFile').on('change', function (event) {
            addExternalUserFilesToUppy(event.target.files);
        });

        try {

            //Contract uppy things
            $scope.externalUserFiles_uppy = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'ringring',
                    target: '#uppy-uploader',
                    allowMultipleUploads: true,
                    metaFields: [],
                    inline: true,
                    height: 200,
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
                    showSelectedFiles: true,
                    showRemoveButtonAfterComplete: false,
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

            $scope.externalUserFiles_uppy.on('complete', (result) => {
                var files = Array.from(result.successful);
                $scope.uploadedFiles = [];
                files.forEach((file) => {
                    var resp = file.response.uploadURL;
                    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                    var fileObj = {};
                    fileObj.FileId = id;
                    fileObj.Name = file.name;
                    fileObj.ExternalUserId = parseInt($scope.userId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinExternalUser = true;
                    });
                    $http.post(root + 'api/ExternalUserFiles/PostExternalUserFiles', $scope.uploadedFiles).then(
                        function success(resp) {
                            if (resp.data.length > 0) {
                                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                resp.data.forEach((file) => {
                                    $scope.externalUserFiles.push(file);
                                    if ($scope.isItDocDacThumbnail)
                                        $scope.defaultImage = file.FileId;
                                });
                                $scope.externalUserFiles_uppy.reset();
                            }
                        }
                        , function error() { });
                }
            });
            $scope.externalUserFiles_uppy.on('file-removed', (file, reason) => {
                $timeout(() => {
                    $scope.$apply(() => {
                        $scope.hasFilesinExternalUser = $scope.externalUserFiles_uppy.getFiles().length > 0
                    });
                });
            });


        }
        catch (err) {
            console.log(err);
        }
        
        $scope.deleteExternalUserFile = function ($event, file) {
            $event.stopPropagation();
            var index = $scope.externalUserFiles.indexOf(file);
            if (index > -1)
                $scope.externalUserFiles.splice(index, 1);
            $http.post(root + 'api/ExternalUserFiles/DeleteExternalUserFile/' + file.Id).then(resp => {
                //$scope.getLinks();
                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                if ($scope.tabContentLength.filesCount < 0) {
                    $scope.tabContentLength.filesCount = 0;
                }
            }, err => {
            });
        }
        $scope.getExternalUserFiles = function () {
            $http.get(root + 'api/ExternalUserFiles/GetExternalUserFiles/' + $scope.userId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {

                $scope.externalUserFiles = resp.data.dbList;
                $scope.defaultImage = resp.data.FileId
            }, err => {
            });
        }

        $scope.getExternalUserFiles();
        $scope.hasFilesinExternalUser = false;
        $scope.hasFilesinContract = false;
    });
    //*********** COntract & draft file *************************
    $scope.files = [];
    $scope.contractFiles = [];
    $scope.hasFilesinDraft = false;
    $scope.hasFilesinContract = false;
    $scope.openProjectFileDialog = function () {
        $('#contractDraftFile').click();
    }
    $scope.openContractFileDialog = function () {
        $('#contractFile').click();
    }

    $(document).ready(function () {
        $('#contractDraftFile').on('change', function (event) {
            addFilesToUppy(event.target.files);
        });

        $('#contractFile').on('change', function (event) {
            addFilesToUppyContract(event.target.files);
        });

        try {

            //Contract draft uppy things
            $scope.uppy = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'ringring',
                    target: '#uppy-uploader-draft',
                    allowMultipleUploads: true,
                    metaFields: [],
                    inline: true,
                    height: 200,
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
                    showSelectedFiles: true,
                    showRemoveButtonAfterComplete: false,
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
                    var resp = file.response.uploadURL;
                    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                    var fileObj = {};
                    fileObj.FileId = id;
                    fileObj.Name = file.name;
                    fileObj.ExternalUserId = parseInt($scope.userId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinDraft = true;
                    });
                    $http.post(root + 'api/ExternalUserDraftFile/PostExternalUserDraftFiles', $scope.uploadedFiles).then(
                        function success(resp) {
                            if (resp.data.length > 0) {
                                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                resp.data.forEach((file) => {
                                    $scope.files.push(file);
                                    if ($scope.isItDocDacThumbnail)
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
                        $scope.hasFilesinDraft = $scope.uppy.getFiles().length > 0
                    });
                });
            });



            //Contract documents uppy things
            $scope.uppyContract = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'uppy-contract',
                    target: '#uppy-uploader-contract',
                    allowMultipleUploads: true,
                    metaFields: [],
                    inline: true,
                    height: 200,
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
                    showSelectedFiles: true,
                    showRemoveButtonAfterComplete: false,
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

            $scope.uppyContract.on('complete', (result) => {
                var files = Array.from(result.successful);
                $scope.uploadedFiles = [];
                files.forEach((file) => {
                    var resp = file.response.uploadURL;
                    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                    var fileObj = {};
                    fileObj.FileId = id;
                    fileObj.Name = file.name;
                    fileObj.ExternalUserId = parseInt($scope.userId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinContract = true;
                    });
                    $http.post(root + 'api/ExternalUserContractFile/PostExternalUserContractFiles', $scope.uploadedFiles).then(
                        function success(resp) {
                            if (resp.data.length > 0) {
                                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                resp.data.forEach((file) => {
                                    $scope.contractFiles.push(file);
                                });
                                $scope.uppyContract.reset();
                            }
                        }
                        , function error() { });
                }
            });
            $scope.uppyContract.on('file-removed', (file, reason) => {
                $timeout(() => {
                    $scope.$apply(() => {
                        $scope.hasFilesinContract = $scope.uppyContract.getFiles().length > 0
                    });
                });
            });
        }
        catch (err) {
            console.log(err);
        }

        $scope.hasFilesinDraft = false;
        $scope.hasFilesinContract = false;
    });

//Contract draft files things
$scope.deleteDraftContractFile = function ($event, file) {
    $event.stopPropagation();
    var index = $scope.files.indexOf(file);
    if (index > -1)
        $scope.files.splice(index, 1);
    $http.post(root + 'api/ExternalUserDraftFile/DeleteExternalUserDraftFile/' + file.Id).then(resp => {
        //$scope.getLinks();
        $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
        if ($scope.tabContentLength.filesCount < 0) {
            $scope.tabContentLength.filesCount = 0;
        }
    }, err => {
    });
}

$scope.getDraftContractFiles = function () {
    $http.get(root + 'api/ExternalUserDraftFile/GetExternalUserDraftFiles/' + $scope.userId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {

        $scope.files = resp.data;
    }, err => {
    });
}

$scope.getDraftContractFiles();

//Contractual documents things

$scope.deleteContractFile = function ($event, file) {
    $event.stopPropagation();
    var index = $scope.contractFiles.indexOf(file);
    if (index > -1)
        $scope.contractFiles.splice(index, 1);
    $http.post(root + 'api/ProjectCrews/DeleteContractDocument/' + file.Id).then(resp => {

    }, err => {
    });
}

$scope.getContractFiles = function () {
    $http.get(root + 'api/ExternalUserContractFile/GetExternalUserContractFiles/' + $scope.userId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {

        $scope.contractFiles = resp.data;
    }, err => {
    });
}

    $scope.getContractFiles();



    $timeout(function () {

        //Draft contract holder
        var holder = document.getElementById('draggableContainer');
        var lastTarget;
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

        //Contract holder
        var holder_contract = document.getElementById('draggableContainerContract');
        var lastTarget_contract;
        if (holder_contract) {
            holder_contract.ondragenter = function (e) {
                lastTarget_contract = e.target;
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
                return false;
            };

            holder_contract.ondragstart = function (e) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }

            holder_contract.ondragover = function (e) {
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
            }

            holder_contract.ondragleave = function (e) {
                e.preventDefault();
                e.preventDefault();
                if (lastTarget === e.target) {
                    this.className = '';
                }
            };

            holder_contract.ondragend = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = '';
            }

            holder_contract.ondrop = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = 'card bg-none b-none';
                addFilesToUppyContract(e.dataTransfer.files);
            };
        }

        //EExter COntract FIles draggableContainerFiles
        var holder_files = document.getElementById('draggableContainerFiles');
        var lastTarget_contract;
        if (holder_files) {
            holder_files.ondragenter = function (e) {
                lastTarget_contract = e.target;
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
                return false;
            };

            holder_files.ondragstart = function (e) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }

            holder_files.ondragover = function (e) {
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
            }

            holder_files.ondragleave = function (e) {
                e.preventDefault();
                e.preventDefault();
                if (lastTarget === e.target) {
                    this.className = '';
                }
            };

            holder_files.ondragend = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = '';
            }

            holder_files.ondrop = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = 'card bg-none b-none';
                addExternalUserFilesToUppy(e.dataTransfer.files);
            };
        }

    }, 100);

    var addFilesToUppy = function (files) {
        //$scope.$apply(function () {
        $scope.hasFilesinDraft = true;
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

    var addFilesToUppyContract = function (files) {
        //$scope.$apply(function () {
        $scope.hasFilesinContract = true;
        //});

        Array.from(files).forEach((a) => {
            try {
                $scope.uppyContract.addFile({
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
    var addExternalUserFilesToUppy = function (files) {
        //$scope.$apply(function () {
        $scope.hasFilesinExternalUser = true;
        //});

        Array.from(files).forEach((a) => {
            try {
                $scope.externalUserFiles_uppy.addFile({
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

    //Load comboes
    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
        }, function error() { });
    }
    $scope.getDepartments = function () {
        $http.get(root + 'api/Departments').then(function success(response) {
            $scope.departments = response.data;
        }, function error() { });
    }
    $scope.getCountries = function () {
        $http.get(root + 'api/Departments').then(function success(response) {
            $scope.departments = response.data;
        }, function error() { });
    }
    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }
    $scope.getCountries();
    $scope.getDepartments();
    $scope.getWorkingPositions();
    $scope.getExternalUser();
});