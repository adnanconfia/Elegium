myApp.controller('AnnouncementsController', function ($stateParams, $scope, $http, $state, $window, $window, $uibModal, $timeout, MenuService, $state) {

    $scope.projectId = $stateParams.id;
    var projectId = parseInt($stateParams.id);
    $scope.announcements = [];

    $http.get(root + 'api/Announcements/GetAnnouncements/' + projectId).then((succ) => {
        $scope.announcements = succ.data;
    }, (err) => {

    })

    $scope.createNewAnnouncement = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/announcements/create-announcement.html',
            controller: 'CreateAnnouncementCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create a new announcement';
                },
                projectItem: function () {
                    return null;
                },
                projectId: function () {
                    return projectId;
                }

            }
        });
        modalInstance.result.then(function (data) {
        }, function (data) {
            if (data)
                $scope.announcements.push(data);
        });
    }

    $scope.deleteAnnouncement = function (event, announcement) {
        event.stopPropagation();
        $http.post(root + 'api/Announcements/DeleteAnnouncement/' + announcement.Id).then(resp => {
            var index = $scope.announcements.indexOf(announcement);
            if (index > -1)
                $scope.announcements.splice(index, 1);
        }, err => {
        });
    }

    $scope.goToAnnouncementProfile = function (id) {
        $state.go('announcements.announcementprofile', { announcementId: id });
    }
}).controller('CreateAnnouncementCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $stateParams, $http, $timeout, toaster, title, projectItem, ProjectService, projectId) {

    $uibModalInstance.rendered.then(function () {

        $scope.uppy = new Uppy.Core({ autoProceed: false })
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


        $scope.title = title;
        $scope.announcement = {};

        $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + projectId).then(resp => {
            $scope.projectUsersAndGroups = resp.data;
        }, err => {
        });

        $scope.groupUsers = function (item) {
            return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'User groups' : 'Users'));
        }

        $scope.createannouncement = function () {
            $scope.announcement.ProjectId = projectId;
            $http.post(root + 'api/Announcements/PostAnnouncement/', $scope.announcement).then(resp => {
                // 
                var announcementId = resp.data.Id;
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
                            fileObj.AnnouncementId = announcementId;
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            $scope.uploadedFiles.push(fileObj);
                        });
                        if ($scope.uploadedFiles.length > 0) {
                            $scope.$apply(function () {
                                $scope.hasFilesinUppy = true;
                            });
                            $http.post(root + 'api/Announcements/PostAnnouncementFiles', $scope.uploadedFiles).then(
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
            }, err => {
            });
        }

        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }
    });
});