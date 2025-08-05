myApp.controller('ResourceDetailCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams) {

    var pathname = window.location.pathname.split("/");
    var resourceId = $stateParams.resourceId;

    //data fetch functions here below

    $scope.getResourceDetail = function () {
        $http.get(root + 'api/Resources/GetResource/' + resourceId).then(function success(response) {
            $scope.resource = response.data;
            console.log($scope.resource);
        }, function error() { });
    }

    $scope.getResourcePhotos = function () {
        $http.get(root + 'api/Resources/GetResourcePhotos?resourceId=' + resourceId).then(function success(response) {
            $scope.resourceImages = response.data.Records;
            console.log($scope.resourceImages);
        }, function error() { });
    }

    $scope.getResourceDetail();
    $scope.getResourcePhotos();



    //Other logicals functions here

    $scope.openPhoto = function (id) {
        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/photoViewer.html',
            controller: 'photoViewerCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                obj: function () {
                    return id;
                }
            }
        });
    }

    $scope.toggleSave = function (resource) {
        resource.IsSaved = !resource.IsSaved;
        $http.get(root + 'api/Resources/ToggleSavedResource/' + resource.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });

    }

    $scope.toggleFavorite = function (resource) {
        resource.IsFavorite = !resource.IsFavorite;
        $http.get(root + 'api/Resources/ToggleFavoriteResource/' + resource.Id).then(function success(response) {
            console.log(response.data);
        }, function error() { });
    }

    $scope.hireorpurchaseresource = function (resource) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/resources/resource-hirerequest-template.html',
            controller: 'HireOrPurchaseResourceCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                resource: function () {
                    return resource;
                }
            }
        });
    }

});

myApp.controller('HireOrPurchaseResourceCtrl', function ($scope, $http, $uibModalInstance, resource, ProjectService, toaster) {
    $uibModalInstance.rendered.then(() => {

        $scope.data = {
            ResourceDto: resource
        };

        $scope.resource = resource;

        ProjectService.getAllProjects().then((suc) => {
            $scope.projectsList = suc.data;
        });


        $scope.uppy = new Uppy.Core({ autoProceed: false })
            .use(Uppy.Dashboard, {
                id: 'ringring',
                target: '.DashboardContainer',
                allowMultipleUploads: true,
                //trigger: '#uppy-uploader',
                metaFields: [],
                //trigger: '#uppy-select-files',
                inline: true,
                height: 200,
                //defaultTabIcon: defaultTabIcon,
                showLinkToFileUploadResult: false,
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
                //onRequestCloseModal: () => $scope.closeUppyModal(),
                showSelectedFiles: true,
                showRemoveButtonAfterComplete: false,
                //locale: defaultLocale,
                browserBackButtonClose: false,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
            })
            .use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        }

        $scope.sendRequest = function () {
            $http.post(root + 'api/ProjectResource/CreateResourceRequest/', $scope.data).then(resp => {

                if (typeof (resp.data.success) != 'undefined') {
                    toaster.pop({
                        type: 'error',
                        title: 'Resource busy!',
                        body: resp.data.msg,
                    });
                    return;
                }
                var ProjectResourceId = resp.data.Id;
                if ($scope.uppy.getFiles().length > 0) {
                    $scope.uppy.upload().then((result) => {
                        var files = Array.from(result.successful);
                        $scope.uploadedFiles = [];
                        files.forEach((file) => {
                            var resp = file.response.uploadURL;
                            var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                            var fileObj = {};
                            fileObj.FileId = id;
                            fileObj.Name = file.name;
                            fileObj.ProjectResourceId = ProjectResourceId;
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            $scope.uploadedFiles.push(fileObj);
                        });
                        if ($scope.uploadedFiles.length > 0) {
                            $scope.$apply(function () {
                                $scope.hasFilesinUppy = true;
                            });
                            $http.post(root + 'api/ProjectResource/PostResourceRequestFiles', $scope.uploadedFiles).then(
                                function success(resp1) {
                                    if (resp1.data.length > 0) {
                                        $uibModalInstance.dismiss(resp.data);
                                    }
                                }
                                , function error() { });
                        }
                    });
                } else {
                    $uibModalInstance.dismiss(resp.data);
                }
            });
        }

    });
});

myApp.controller('photoViewerCtrl', function ($scope, $http, $uibModalInstance, obj) {
    $scope.data = obj;
    console.log($scope.data);
    $scope.ok = function () {
        $uibModalInstance.close(files);
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.next = function () {
        $http.get(root + 'api/Resources/NextPrevFileId/' + $scope.data.FileId + '/' + 'N' + '/0/' + $scope.data.ResourceId).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }

    $scope.prev = function () {
        $http.get(root + 'api/Resources/NextPrevFileId/' + $scope.data.FileId + '/' + 'P' + '/0/' + $scope.data.ResourceId).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }
});