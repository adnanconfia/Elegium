myApp.controller('professionalDetailCtrl', function ($scope, $filter, $http, $uibModal, toaster,$stateParams) {
    var pathname = window.location.pathname.split("/");
    var userId = $stateParams.userId;

    $scope.albumId = '';
    $scope.albumName = '';

    $scope.videoAlbumId = '';
    $scope.videoAlbumName = '';

    $scope.audioAlbumId = '';
    $scope.audioAlbumName = '';

    $http.get(root + 'api/ProfessionalDetail/GetUserData/' + userId).then((response) => {
        $scope.url = window.location.href;
        console.log($scope.url);
        $scope.userPhotos = response.data.photos;
        $scope.userAlbums = response.data.photoAlbums;

        $scope.userVideos = response.data.videos;
        $scope.userVideoAlbums = response.data.videoAlbums;

        $scope.userAudioAlbums = response.data.audioAlbums;
        $scope.userAudios = response.data.audios;

        $scope.userProfile = response.data.userProfile;
        $scope.userEquipment = response.data.userEquipment;
        $scope.userCredits = response.data.userCredits;
        $scope.otherLanguages = response.data.otherLanguages;
        $scope.promotionCategories = response.data.promotionCategories;
        $scope.userSkills = response.data.userSkills;
        $scope.userCategories = response.data.userCategories;

        $scope.userLoggedIn = response.data.userLoggedIn;
        $scope.loggedInUser = response.data.loggedInUser;
    },
        () => {

        });

    $http.get(root + 'api/ProfessionalDetail/GetUserEquipments/' + userId).then(function success(response) {
        $scope.resourcesList = response.data;
        console.log('resources:', $scope.resourcesList);
    }, function error() { });

    $scope.toggleFollowing = function (user) {
        $http.get(root + 'api/UserFollowings/ToggleUserFollowing/' + user.UserId).then(function success(response) {
            if (response.status == 200) {
                if (response.data == "following") {
                    $scope.userProfile.Following = true;
                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'You are now following ' + user.FirstName + ' ' + user.LastName,
                    });
                }
                else {
                    $scope.userProfile.Following = false;
                }
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }

    $scope.openFile = function (id) {
        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/galleryViewer.html',
            controller: 'galleryCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                obj: function () {
                    return id;
                },
                userId: function () {
                    return userId;
                }
            }
        });
    }


    $scope.contactUser = function (id) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/professionalDetail/message-user-template.html',
            controller: 'contactUserCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                userProfile: function () {
                    return $scope.userProfile;
                },
                userLoggedIn: function () {
                    return $scope.userLoggedIn;
                },
                loggedInUser: function () {
                    return $scope.loggedInUser;
                }
            }
        });
    }

    $scope.forwardUser = function (id) {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/professionalDetail/forward-user-template.html',
            controller: 'forwardCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                userProfile: function () {
                    return $scope.userProfile;
                },
                userLoggedIn: function () {
                    return $scope.userLoggedIn;
                },
                loggedInUser: function () {
                    return $scope.loggedInUser;
                },
                url: function () {
                    return window.location.href;
                }
            }
        });
    }

    $scope.openHireRequest = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/professionalDetail/send-hirerequest-template.html',
            controller: 'SendHireRequestCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                userProfile: function () {
                    return $scope.userProfile;
                },
                userLoggedIn: function () {
                    return $scope.userLoggedIn;
                },
                loggedInUser: function () {
                    return $scope.loggedInUser;
                },
                url: function () {
                    return window.location.href;
                }
            }
        });
    }

    $scope.print = () => {
        printJS('printJS-form', 'html')
    }


    $scope.getUserPhotos = function () {
        $http.get(root + 'api/ProfessionalDetail/GetUserPhotos?albumId=' + $scope.albumId + '&&userId=' + userId).then(function success(response) {
            $scope.userPhotos = response.data.Records;
            $scope.totalUserPhotos = $scope.userPhotos.length;
        }, function error() { });
    }


    $scope.getUserVideos = function () {
        $http.get(root + 'api/ProfessionalDetail/GetUserVideos?albumId=' + $scope.videoAlbumId + '&&userId=' + userId).then(function success(response) {
            $scope.userVideos = response.data.Records;
            $scope.totalUserVideos = $scope.userVideos.length;
        }, function error() { });
    }

    $scope.getUserAudios = function () {
        $http.get(root + 'api/ProfessionalDetail/GetUserAudios?albumId=' + $scope.audioAlbumId + '&&userId=' + userId).then(function success(response) {
            $scope.userAudios = response.data.Records;
            $scope.totalUserAudios = $scope.userAudios.length;
        }, function error() { });
    }

    $scope.getAlbumPhotos = function (p) {
        $scope.albumId = p.Id;
        $scope.albumName = p.Name;
        $scope.getUserPhotos();
    }

    $scope.getAllUserPhotos = function () {
        $scope.albumId = '';
        $scope.albumName = '';
        $scope.getUserPhotos();
    }

    $scope.getAlbumVideos = function (p) {
        $scope.videoAlbumId = p.Id;
        $scope.videoAlbumName = p.Name;
        $scope.getUserVideos();
    }

    $scope.getAllUserVideos = function () {
        $scope.videoAlbumId = '';
        $scope.videoAlbumName = '';
        $scope.getUserVideos();
    }

    $scope.getAlbumAudios = function (p) {
        $scope.audioAlbumId = p.Id;
        $scope.audioAlbumName = p.Name;
        $scope.getUserAudios();
    }

    $scope.getAllUserAudios = function () {
        $scope.audioAlbumId = '';
        $scope.audioAlbumName = '';
        $scope.getUserAudios();
    }


});

myApp.controller('galleryCtrl', function ($scope, $http, $uibModalInstance, obj, userId) {
    $scope.data = obj;
    console.log($scope.data);
    $scope.ok = function () {
        $uibModalInstance.close(files);
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.next = function () {
        $http.get(root + 'api/ProfessionalDetail/NextPrevFileId/' + $scope.data.FileId + '/' + 'N' + '/' + $scope.data.Type + '/' + userId).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }

    $scope.prev = function () {
        $http.get(root + 'api/ProfessionalDetail/NextPrevFileId/' + $scope.data.FileId + '/' + 'P' + '/' + $scope.data.Type + '/' + userId).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }
});

myApp.controller('contactUserCtrl', function ($scope, $http, $uibModalInstance, userProfile, userLoggedIn, loggedInUser) {
    $scope.userProfile = userProfile;
    $scope.userLoggedIn = userLoggedIn;
    $scope.loggedInUser = loggedInUser;

    $uibModalInstance.rendered.then(function () {
        $scope.getCountries();
    });

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.Send = function () {
        var formData = {
            "UserProfile": $scope.userProfile,
            "UserMessages": $scope.data
        }
        $http.post(root + 'api/ProfessionalDetail/SaveMessage', formData).then(function success(response) {
            if (response.status == 200) {
                gritterAlert(response.data.suc, response.data.Message, response.data.success);
                if (response.data.success)
                    $scope.cancel();
            }
        }, function error() { });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});

myApp.controller('forwardCtrl', function ($scope, $http, $uibModalInstance, userProfile, userLoggedIn, loggedInUser, url) {
    $scope.userProfile = userProfile;
    $scope.userLoggedIn = userLoggedIn;
    $scope.loggedInUser = loggedInUser;

    $uibModalInstance.rendered.then(function () {
        $scope.getCountries();
    });

    $scope.getCountries = function () {
        $http.get(root + 'api/Countries').then(function success(response) {
            $scope.countries = response.data;
            console.log($scope.countries);
        }, function error() { });
    }

    $scope.Forward = function () {
        var formData = {
            "UserProfile": $scope.userProfile,
            "ForwardMessage": $scope.data,
            "Url": url
        }
        $http.post(root + 'api/ProfessionalDetail/ForwardProfile', formData).then(function success(response) {
            if (response.status == 200) {
                gritterAlert(response.data.suc, response.data.Message, response.data.success);
                if (response.data.success)
                    $scope.cancel();
            }
        }, function error() { });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});

myApp.controller('SendHireRequestCtrl', function ($scope, $http, $uibModalInstance, toaster, userProfile, userLoggedIn, loggedInUser, url) {
    $scope.userProfile = userProfile;
    $scope.userLoggedIn = userLoggedIn;
    $scope.loggedInUser = loggedInUser;

    var uppy = null;

    $uibModalInstance.rendered.then(function () {

        uppy = new Uppy.Core({
            restrictions: {
                maxNumberOfFiles: 3,
                //allowedFileTypes: ['.pdf', '.docx', '.doc']
            }
        })
            .use(Uppy.Dashboard, {
                inline: true,
                height: 150,
                theme: $rootScope.BackgroundThings.DarkMode?'dark':'light',
                hideUploadButton: true,
                locale: {
                    strings: { dropPaste: 'Upload your hire contract %{browse}' },
                },
                target: '.DashboardContainer',
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        uppy.run();

        $scope.getProjects();
        $scope.getWorkingPositions();
    });

    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
            console.log($scope.workingPositions);
        }, function error() { });
    }

    $scope.getProjects = function () {
        $http.get(root + 'api/Projects/GetProjectsName').then(function success(response) {
            $scope.projectsList = response.data;
            console.log($scope.projectsList);
        }, function error() { });
    }

    $scope.send = function () {

        if (!$scope.data.ProjectId || !$scope.data.WorkingPositionId || !$scope.data.Message) {
            toaster.pop({
                type: 'error',
                title: '',
                body: 'Please fill fields before sending request.',
            });
            return false;
        }

        var formData = {
            ProfessionalId: $scope.userProfile.UserId,
            ProjectId: $scope.data.ProjectId,
            WorkingPositionId: $scope.data.WorkingPositionId,
            Message: $scope.data.Message
        }
        $http.post(root + 'api/ProfessionalHireRequests/SaveProfessionalHireRequest', formData).then(function success(response) {
            if (response.status == 200) {
                if (response.data.success) {
                    uppy.upload().then((result) => {
                        $scope.Files = [];
                        var files = Array.from(result.successful);
                        files.forEach((file) => {
                            var resp = file.response.uploadURL;
                            var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                            var fileObj = {};
                            fileObj.FileId = id;
                            fileObj.Name = file.name;
                            fileObj.Type = 'F';
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            fileObj.ProfessionalHireRequestId = response.data.RequestId;
                            $scope.Files.push(fileObj);
                        });

                        if ($scope.Files.length > 0) {
                            $http.post(root + 'api/ProfessionalHireRequests/PostProfessionalHireRequestMediaFiles', $scope.Files).then(function success(res) {
                                console.log(res);
                                if (res.status == 200) {
                                    toaster.pop({
                                        type: 'success',
                                        title: response.data.suc,
                                        body: response.data.Message,
                                    });
                                    $scope.cancel();
                                }
                            }, function error() { });
                        }
                        else {
                            toaster.pop({
                                type: 'success',
                                title: response.data.suc,
                                body: response.data.Message,
                            });
                            $scope.cancel();
                        }
                    });
                }
                else {
                    toaster.pop({
                        type: 'error',
                        title: response.data.suc,
                        body: response.data.Message,
                    });
                }

            }
            else {
                toaster.pop({
                    type: 'error',
                    title: response.data.suc,
                    body: response.data.Message,
                });
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: '',
                body: err.data,
            });
        });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});