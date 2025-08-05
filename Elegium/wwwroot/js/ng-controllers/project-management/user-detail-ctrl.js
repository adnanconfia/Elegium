myApp.controller('UserDetailController', function ($scope, $rootScope, $window, $filter, $http, $uibModal, $timeout, toaster, $ngConfirm, $stateParams, $state) {
    console.log('state params:', $stateParams);
    $scope.crewId = $stateParams.userId;
    $scope.title = $stateParams.title;
    $scope.crewUser = {};
    $scope.isPositionEditMode = false;
    $scope.data = {
        crewPositions: []
    }
    $scope.workingPositions = [];
    $scope.editUserProfile = false;

    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
        }, function error() { });
    }

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.getCities = function () {
        $http.get(root + 'api/Cities/GetCities').then(function success(response) {
            $scope.cities = response.data;
            $scope.companyCities = response.data;
            $scope.studioCities = response.data;
            console.log($scope.cities);
        }, function error() { });
    }

    $scope.getCitiesOfCountry = function (countryId, name) {
        $http.get(root + 'api/Cities/GetCitiesOfCountry?countryId=' + countryId).then(function success(response) {
            if (name == 'personal')
                $scope.cities = response.data;
            else if (name == 'company')
                $scope.companyCities = response.data;
            else if (name == 'studio')
                $scope.studioCities = response.data;
            console.log($scope.cities);
        }, function error() { });
    }

    $scope.editUser = function (crewUser) {
        if (currentUserId == crewUser) {
            $window.open(root + 'profile', '_blank');
        }
        else {
            $scope.editUserProfile = !$scope.editUserProfile;
        }
        //console.log(crewUser, currentUserId);
    }

    $rootScope.getCrews = function () {
        $http.get(root + 'api/ProjectCrews/GetProjectCrewsById?crewId=' + $scope.crewId).then(function success(response) {
            //console.log('$scope.crewList', response);
            $scope.crewUser = response.data;

            $scope.userProfile = $scope.crewUser.CrewUserProfile;


            //Contract tab things

            if ($scope.crewUser.Crew.ContainsLOI)
                $scope.crewUser.Crew.ContainsLOI = new Date($scope.crewUser.Crew.ContainsLOI);
            if ($scope.crewUser.Crew.ContainsDealMemo)
                $scope.crewUser.Crew.ContainsDealMemo = new Date($scope.crewUser.Crew.ContainsDealMemo);
            if ($scope.crewUser.Crew.ContractCreated)
                $scope.crewUser.Crew.ContractCreated = new Date($scope.crewUser.Crew.ContractCreated);
            if ($scope.crewUser.Crew.ContractSent)
                $scope.crewUser.Crew.ContractSent = new Date($scope.crewUser.Crew.ContractSent);
            if ($scope.crewUser.Crew.ContractSigned)
                $scope.crewUser.Crew.ContractSigned = new Date($scope.crewUser.Crew.ContractSigned);
            console.log(' $scope.crewUser', $scope.crewUser);
        }, function error(err) {

            console.log('$scope.crewList', err);
        });
    }

    $scope.updateUserProfile = function (user) {
        $http.post(`${root}api/ProjectCrews/UpdateUserProfile`, user).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Information updated successfully!',
                });
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

    $rootScope.editModePosition = function (name, projectCrew) {
        $scope.isPositionEditMode = true;
    }

    $rootScope.cancelModePosition = function (name, projectCrew) {
        $scope.isPositionEditMode = false;
        $rootScope.getCrews();
    }

    $scope.updateCrewPosition = function (record) {
        console.log('$scope.crewPositions', $scope.data.crewPositions);
        $scope.ProjectCrewModel = {
            ProjectCrew: record,
            UserProfile: null,
            ProjectCrewPositions: $scope.data.crewPositions,
            ProjectCrewUnits: null
        }
        $http.post(`${root}api/ProjectCrews/UpdateCrewPosition`, $scope.ProjectCrewModel).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'User Group updated successfully!',
                });
                $rootScope.getCrews();
                $scope.isPositionEditMode = false;
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

    $scope.saveMe = function (crew) {
        console.log(crew);
        $scope.updateAccessRights(crew);
    }

    $rootScope.updateRights = function (name, projectCrew) {
        $rootScope.changeRights(name, projectCrew);
        $scope.updateAccessRights(projectCrew);
    }
    $rootScope.classRights = function (rights) {
        if (rights == 'V')
            return 'fa fa-eye';
        else if (rights == 'E')
            return 'fa fa-pencil-square-o';
        else
            return 'fa fa-lock';
    }
    $rootScope.userRightsCls = function (rights) {

        if (rights == 'V')
            return '';
        else if (rights == 'E')
            return '';
        else
            return 'low-opacity';
    }

    $rootScope.getCrews();
    $scope.getWorkingPositions();
    $scope.getCountries();
    $scope.getCities();

    //**********8 Start Code for  Contract Draft Draft
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
    $scope.tabContentLength = {

    };
    $scope.filesPaging = {
        page: 1,
        size: 12
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

            $scope.uppy.on('complete', (result) => {
                var files = Array.from(result.successful);
                $scope.uploadedFiles = [];
                files.forEach((file) => {
                    var resp = file.response.uploadURL;
                    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                    var fileObj = {};
                    fileObj.FileId = id;
                    fileObj.Name = file.name;
                    fileObj.ProjectCrewId = parseInt($scope.crewId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinDraft = true;
                    });
                    $http.post(root + 'api/ProjectCrews/PostDraftContracts', $scope.uploadedFiles).then(
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
                    fileObj.ProjectCrewId = parseInt($scope.crewId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    $scope.uploadedFiles.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $scope.$apply(function () {
                        $scope.hasFilesinContract = true;
                    });
                    $http.post(root + 'api/ProjectCrews/PostContractDocuments', $scope.uploadedFiles).then(
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
        $http.post(root + 'api/ProjectCrews/DeleteDraftContracts/' + file.Id).then(resp => {
            //$scope.getLinks();
            $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
            if ($scope.tabContentLength.filesCount < 0) {
                $scope.tabContentLength.filesCount = 0;
            }
        }, err => {
        });
    }

    $scope.getDraftContractFiles = function () {
        $http.get(root + 'api/ProjectCrews/GetDraftContractsFiles/' + $scope.crewId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {

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
        $http.get(root + 'api/ProjectCrews/GetContractDocuments/' + $scope.crewId + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {

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
    //**********8 End Code for  Contract Draft Draft


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

});


