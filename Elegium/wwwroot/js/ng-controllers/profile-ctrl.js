myApp.controller('ProfileController', ['$scope', '$filter', '$http', '$uibModal','$ngConfirm', 'toaster', function ($scope, $filter, $http, $uibModal, $ngConfirm, toaster) {

    $(document).ready(function () {

        //$('.multiselect').multiselect({
        //    enableFiltering: true,
        //    enableCaseInsensitiveFiltering: true,
        //    maxHeight: 200
        //});

        var navListItems = $('div.setup-panel div a'),
            allWells = $('.setup-content'),
            allNextBtn = $('.nextBtn');

        allWells.hide();

        navListItems.click(function (e) {
            e.preventDefault();
            var $target = $($(this).attr('href')),
                $item = $(this);

            if (!$item.hasClass('disabled')) {
                navListItems.removeClass('btn-success').addClass('btn-default');
                $item.addClass('btn-success');
                allWells.hide();
                $target.show();
                $target.find('input:eq(0)').focus();
            }
        });

        allNextBtn.click(function () {
            var curStep = $(this).closest(".setup-content"),
                curStepBtn = curStep.attr("id"),
                nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
                curInputs = curStep.find("input[type='text'],input[type='url']"),
                isValid = true;

            $(".form-group").removeClass("has-error");
            for (var i = 0; i < curInputs.length; i++) {
                if (!curInputs[i].validity.valid) {
                    isValid = false;
                    $(curInputs[i]).closest(".form-group").addClass("has-error");
                }
            }

            if (isValid) nextStepWizard.removeAttr('disabled').trigger('click');
        });

        $('div.setup-panel div a.btn-success').trigger('click');
    });
    //Navigatate to some specific tab by checking url
    if (window.location.href.indexOf('#self-promotion') >= 0) {
        $('#selfPromotion-tab').click()
    }

    //data API methods

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

    $scope.getWorkingPositions = function () {
        $http.get(root + 'api/WorkingPositions').then(function success(response) {
            $scope.workingPositions = response.data;
            console.log($scope.workingPositions);
        }, function error() { });
    }

    $scope.getLanguageLevels = function () {
        $http.get(root + 'api/LanguageLevels').then(function success(response) {
            $scope.languageLevels = response.data;
            console.log($scope.languageLevels);
        }, function error() { });
    }

    $scope.getLanguages = function () {
        $http.get(root + 'api/Languages').then(function success(response) {
            $scope.languages = response.data;
            console.log($scope.languages);
        }, function error() { });
    }

    $scope.getCompanyTypes = function () {
        $http.get(root + 'api/CompanyTypes').then(function success(response) {
            $scope.companyTypes = response.data;
            console.log($scope.companyTypes);
        }, function error() { });
    }

    $scope.getPromotionCategories = function () {
        $http.get(root + 'api/PromotionCategories').then(function success(response) {
            $scope.promotionCategories = response.data;
            console.log($scope.promotionCategories);
        }, function error() { });
    }

    $scope.getSkills = function () {
        $http.get(root + 'api/Skills').then(function success(response) {
            $scope.skills = response.data;
            console.log($scope.skills);
        }, function error() { });
    }

    $scope.getSkillLevels = function () {
        $http.get(root + 'api/SkillLevels').then(function success(response) {
            $scope.skillLevels = response.data;
            console.log($scope.skillLevels);
        }, function error() { });
    }

    $scope.getEquipmentCategories = function () {
        $http.get(root + 'api/EquipmentCategories/GetEquipmentCategoryShort').then(function success(response) {
            $scope.equipmentCategories = response.data;
            console.log('equipmentCategories:', $scope.equipmentCategories);
        }, function error() { });
    }

    //Get all the user resources that are equipement
    $scope.getResources = function () {
        $http.get(root + 'api/Resources/GetUserEquipments').then(function success(response) {
            $scope.resourcesList = response.data;
            console.log('resources:', $scope.resourcesList);
        }, function error() { });
    }

    $scope.addSkill = function () {
        $scope.userSkills.unshift({
            Name: '',
            Description: ''
        });
    }

    $scope.deleteSkill = function (item) {
        $scope.userSkills.splice($scope.userSkills.indexOf(item), 1);
    }
    $scope.userOtherLanguages = {};
    $scope.getUserProfile = function () {
        $http.get(root + 'api/UserProfiles/GetCurrentUserProfile').then(function success(response) {
            console.log(response.data);
            $scope.userProfile = response.data.UserProfile;
            $scope.userSkills = response.data.UserAdditionalSkills;
            $scope.userCredits = $filter('orderBy')(response.data.UserCredits, 'Year', true);
            $scope.userEquipments = response.data.UserEquipments;
            $scope.userPromotionCategory = response.data.UserPromotionCategory;
            $scope.userOtherLanguages.langs = response.data.UserOtherLanguages;
            console.log('userequip:', $scope.userEquipments);

            if ($scope.userProfile.OtherLanguage)
                $scope.userProfile.OtherLanguage = $scope.userProfile.OtherLanguage.split(',').map(Number);
            if ($scope.userProfile.PromotionCategories)
                $scope.userProfile.PromotionCategories = $scope.userProfile.PromotionCategories.split(',').map(Number);
            else
                $scope.userProfile.PromotionCategories = [];

            $scope.UserPhoto = "data:image/jpg;base64," + $scope.userProfile.Photo;
            $scope.CompanyPhoto = "data:image/jpg;base64," + $scope.userProfile.CompanyLogo;
            console.log($scope.userProfile);
        }, function error() { });
    }

    $scope.saveUserProfile = function () {

        $scope.userProfileData = angular.copy($scope.userProfile);

        if ($scope.userProfileData.OtherLanguage && $scope.userProfileData.OtherLanguage.length > 0)
            $scope.userProfileData.OtherLanguage = $scope.userProfileData.OtherLanguage.join(',');

        if ($scope.userProfileData.PromotionCategories && $scope.userProfileData.PromotionCategories.length > 0)
            $scope.userProfileData.PromotionCategories = $scope.userProfileData.PromotionCategories.join(',');
        else
            $scope.userProfileData.PromotionCategories = null;

        var formData = {
            "UserProfile": $scope.userProfileData,
            "UserCredits": $scope.userCredits,
            "UserEquipments": $scope.userEquipments,
            "UserImage": $scope.UserPhoto,
            "CompanyImage": $scope.CompanyPhoto,
            "UserPromotionCategory": $scope.userPromotionCategory,
            "UserOtherLanguages": $scope.userOtherLanguages.langs,
            "UserAdditionalSkills": $scope.userSkills
        };
        //formData.append('UserProfile', $scope.userProfileData);
        //formData.append('UserImage', 'hello I am mubi');
        //console.log($scope.userProfileData);
        $http.post(root + 'api/UserProfiles/SaveUserProfile', formData).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'User profile updated successfully!',
                });
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }


    //Other logical methods

    $scope.openCroppie = function () {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/croppie-playground.html',
            controller: 'CroppiePlayCtrl',
            size: 'lg',
            resolve: {
                title: function () {
                    return "Upload profile photo";
                },
                image: function () {
                    return $scope.UserPhoto;
                },
                width: function () { return 300 },
                height: function () { return 300 }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.UserPhoto = data;
        });
    }

    $scope.openCroppieCompany = function () {
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/croppie-playground.html',
            controller: 'CroppiePlayCtrl',
            size: 'lg',
            resolve: {
                title: function () {
                    return "Upload company photo";
                },
                image: function () {
                    return $scope.CompanyPhoto;
                },
                width: function () { return 300 },
                height: function () { return 300 }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.CompanyPhoto = data;
        });
    }

    $scope.toggleSelection = function toggleSelection(itemId) {
        var idx = $scope.userPromotionCategory.indexOf(itemId);
        //if ($scope.userProfile.PromotionCategories)
        //    idx = 
        //else
        //    $scope.userProfile.PromotionCategories = [];

        // Is currently selected
        if (idx > -1) {
            $scope.userPromotionCategory.splice(idx, 1);
        }

        // Is newly selected
        else {
            $scope.userPromotionCategory.push(itemId);
        }
    };


    $scope.addCredit = function addCredit() {
        //if (!$scope.userCredits == null || $scope.userCredits == undefined) $scope.userCredits = [];
        $scope.userCredits.unshift({
            Year: new Date().getFullYear().toString(),
            Workplace: '',
            Job: ''
        });
        console.log($scope.userCredits);
    };

    $scope.deleteCredit = function deleteCredit(credit) {
        $scope.userCredits.splice($scope.userCredits.indexOf(credit), 1);
    }

    //$scope.userEquipments = [];
    $scope.resource = {};

    $scope.addResource = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/offer-resources/add-resource-template.html',
            controller: 'ResourceCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Add a new Equipment';
                },
                resource: function () {
                    return $scope.resource;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {
            $scope.getResources();
        });
    }

    $scope.editResource = function (resource) {
        var resourceVM = angular.copy(resource);

        if (resourceVM.HireOrSale == 'S')
            resourceVM.HireOrSale = true; //sale
        else
            resourceVM.HireOrSale = false; //hire


        if (resourceVM.LendingType == 'H')
            resourceVM.LendingType = true; //hire with operator
        else
            resourceVM.LendingType = false; //available for rent

        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/offer-resources/add-resource-template.html',
            controller: 'ResourceCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Edit Equipment';
                },
                resource: function () {
                    return resourceVM;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {
            $scope.getResources();
        });
    }

    $scope.deleteResource = function (id) {
        $ngConfirm({
            title: 'Delete Resource?',
            content: 'Are you sure to delete this Resource? This action can not be revert back.',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Delete Resource',
                    btnClass: 'btn-red',
                    action: function () {
                        $scope.deleteConfirmResource(id);
                    }
                },
                cancel: function () {

                }
            }
        });

    }

    $scope.deleteConfirmResource = function (id) {
        $http.delete(root + 'api/Resources/DeleteResource/' + id).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Resource Deleted!',
                });
                $scope.getResources();
            }
        }, function error() { });
    }

    $scope.addEquipment = function addEquipment() {
        $scope.equipmentQuery = "";
        $scope.userEquipments.unshift({
            EquipmentName: 'Item ' + Number($scope.userEquipments.length + 1),
            EquipmentCategoryId: '',
            LendingType: '1'
        });
        console.log($scope.userEquipments);
    }

    $scope.deleteEquipment = function deleteEquipment(equipment) {
        $scope.userEquipments.splice($scope.userEquipments.indexOf(equipment), 1);
    }

    // Methods call here only

    $scope.getCountries();
    $scope.getCities();
    $scope.getWorkingPositions();
    $scope.getLanguageLevels();
    $scope.getLanguages();
    $scope.getCompanyTypes();
    $scope.getPromotionCategories();
    $scope.getSkills();
    $scope.getSkillLevels();
    $scope.getEquipmentCategories();

    $scope.getResources();

    $scope.getUserProfile();

    //fileupload

    $scope.photoModalOpeningAction = '';
    $scope.albumId = '';

    $scope.getUserPhotos = function () {
        $http.get(root + 'api/UserProfiles/GetUserPhotos?albumId=' + $scope.albumId).then(function success(response) {
            $scope.userPhotos = response.data.Records;
            $scope.totalUserPhotos = $scope.userPhotos.length;
        }, function error() { });
    }

    $scope.getUserAlbums = function () {
        $http.get(root + 'api/UserProfiles/GetUserAlbums').then(function success(response) {
            $scope.userAlbums = response.data.Records;
            $scope.totalUserAlbums = response.data.Count;
        }, function error() { });
    }

    $scope.deleteFile = function (obj) {
        $ngConfirm({
            title: 'Delete File?',
            content: 'Are you sure to delete this File? This action can not be revert back.',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Delete File',
                    btnClass: 'btn-red',
                    action: function () {
                        $http.delete(root + 'api/UserProfiles/DeleteFile/' + obj.Id + "/" + obj.Type + "/" + obj.FileId).then(function success(response) {
                            if (response.status == 200) {
                                gritterAlert("Success", response.data.Message, response.data.success);
                                if (obj.Type == 'P') {
                                    $scope.getUserPhotos();
                                    $scope.getUserAlbums();
                                }
                                else if (obj.Type == 'A') {
                                    $scope.getUserAudios();
                                    $scope.getUserAudioAlbums();
                                } else {
                                    $scope.getUserVideos();
                                    $scope.getUserVideoAlbums();
                                }
                            }
                        }, function error() { });
                    }
                },
                cancel: function () {

                }
            }
        });
        
    }

    $scope.deleteAlbum = function (obj) {
        $ngConfirm({
            title: 'Delete Album?',
            content: 'Are you sure to delete this Album? This action can not be revert back.',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Delete Album',
                    btnClass: 'btn-red',
                    action: function () {
                        $http.delete(root + 'api/UserProfiles/DeleteAlbum/' + obj.Id + "/" + obj.Type).then(function success(response) {
                            if (response.status == 200) {
                                gritterAlert("Success", response.data.Message, response.data.success);
                                if (obj.Type == 'P') {
                                    $scope.getUserPhotos();
                                    $scope.getUserAlbums();
                                }
                                else if (obj.Type == 'A') {
                                    $scope.getUserAudios();
                                    $scope.getUserAudioAlbums();
                                } else {
                                    $scope.getUserVideos();
                                    $scope.getUserVideoAlbums();
                                }
                            }
                        }, function error() { });
                    }
                },
                cancel: function () {

                }
            }
        });
        
    }

    $scope.getUserPhotos();
    $scope.getUserAlbums();

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



    var fileInput = document.getElementById('fileUpload'); //FOR PHOTOS AND PHOTOS ALBUM


    fileInput.addEventListener("change", function (e) {
        // Get the selected file from the input element
        var files = e.target.files;

        var modalInstance = null;
        if ($scope.photoModalOpeningAction == 'P') {
            modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: root + 'js/ng-templates/photos-upload-template.html',
                controller: 'PhotosUploadCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    files: function () {
                        return files;
                    }
                }
            });
        } else {

            modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: root + 'js/ng-templates/photos-album-template.html',
                controller: 'AlbumPhotosUploadCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    files: function () {
                        return files;
                    },
                    albumId: function () {
                        return $scope.albumId;
                    },
                    albumName: function () {
                        return $scope.albumName;
                    }
                }
            });
        }

        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.albumId = '';
            $scope.albumName = '';
            $scope.getUserPhotos();
            $scope.getUserAlbums();
            //$scope.UserPhoto = data;
        });
    });

    $scope.openPhoto = function (id) {
        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/photoViewer.html',
            controller: 'portfolioGalleryCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                obj: function () {
                    return id;
                }
            }
        });
    }

    $scope.openFileSelector = function (id) {
        if (id == 'A') {
            $scope.albumId = '';
            $scope.albumName = '';
        }
        $scope.photoModalOpeningAction = id;
        fileInput.click();
    }

    $scope.addNewPhotosToAlbum = function (id) {
        $scope.photoModalOpeningAction = id;
        fileInput.click();
    }


    //video related functions

    $scope.videoAlbumId = '';

    $scope.getUserVideos = function () {
        $http.get(root + 'api/UserProfiles/GetUserVideos?albumId=' + $scope.videoAlbumId).then(function success(response) {
            $scope.userVideos = response.data.Records;
            $scope.totalUserVideos = $scope.userVideos.length;
        }, function error() { });
    }

    $scope.getUserVideoAlbums = function () {
        $http.get(root + 'api/UserProfiles/GetVideoAlbums').then(function success(response) {
            $scope.userVideoAlbums = response.data.Records;
            $scope.totalUserVideoAlbums = response.data.Count;
        }, function error() { });
    }
    //getAlbumVideos

    $scope.getAlbumVideos = function (p) {
        $scope.videoAlbumId = p.Id;
        $scope.videoAlbumName = p.Name;
        $scope.getUserVideos();
    }
    //getAllUserVideos

    $scope.getAllUserVideos = function () {
        $scope.videoAlbumId = '';
        $scope.videoAlbumName = '';
        $scope.getUserVideos();
    }
    //addNewVideosToAlbum

    $scope.addNewVideosToAlbum = function (id) {
        $scope.videoModalOpeningAction = id;
        videoInput.click();
    }

    $scope.getUserVideoAlbums();
    $scope.getUserVideos();

    var videoInput = document.getElementById('videoUpload'); //for videos and videos album
    $scope.openVideoFileSelector = function (id) {
        if (id == 'A') {
            $scope.videoAlbumId = '';
            $scope.videoAlbumName = '';
        }
        $scope.videoModalOpeningAction = id;
        videoInput.click();
    }

    videoInput.addEventListener("change", function (e) {
        // Get the selected file from the input element
        var files = e.target.files;

        var modalInstance = null;
        if ($scope.videoModalOpeningAction == 'P') {
            modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: root + 'js/ng-templates/videos-upload-template.html',
                controller: 'VideosUploadCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    files: function () {
                        return files;
                    }
                }
            });
        } else {

            modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: root + 'js/ng-templates/videos-album-template.html',
                controller: 'AlbumVideosUploadCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    files: function () {
                        return files;
                    },
                    albumId: function () {
                        return $scope.videoAlbumId;
                    },
                    albumName: function () {
                        return $scope.videoAlbumName;
                    }
                }
            });
        }

        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.videoAlbumId = '';
            $scope.videoAlbumName = '';
            $scope.getUserVideos();
            $scope.getUserVideoAlbums();
            //$scope.UserPhoto = data;
        });
    });

    $scope.openVideo = function (id) {
        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/videoViewer.html',
            controller: 'portfolioGalleryCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                obj: function () {
                    return id;
                }
            }
        });
    }
    //end video related functions

    //audio related functions

    $scope.audioAlbumId = '';

    $scope.getUserAudios = function () {
        $http.get(root + 'api/UserProfiles/GetUserAudios?albumId=' + $scope.audioAlbumId).then(function success(response) {
            $scope.userAudios = response.data.Records;
            $scope.totalUserAudios = $scope.userAudios.length;
        }, function error() { });
    }

    $scope.getUserAudioAlbums = function () {
        $http.get(root + 'api/UserProfiles/GetAudioAlbums').then(function success(response) {
            $scope.userAudioAlbums = response.data.Records;
            $scope.totalUserAudioAlbums = response.data.Count;
        }, function error() { });
    }
    //getAlbumVideos

    $scope.getAlbumAudios = function (p) {
        $scope.audioAlbumId = p.Id;
        $scope.audioAlbumName = p.Name;
        $scope.getUserAudios();
    }
    //getAllUserVideos

    $scope.getAllUserAudios = function () {
        $scope.audioAlbumId = '';
        $scope.audioAlbumName = '';
        $scope.getUserAudios();
    }
    //addNewVideosToAlbum

    $scope.addNewAudiosToAlbum = function (id) {
        $scope.audioModalOpeningAction = id;
        audioInput.click();
    }

    $scope.getUserAudioAlbums();
    $scope.getUserAudios();

    var audioInput = document.getElementById('audioUpload'); //for videos and videos album
    $scope.openAudioFileSelector = function (id) {
        if (id == 'A') {
            $scope.audioAlbumId = '';
            $scope.audioAlbumName = '';
        }
        $scope.audioModalOpeningAction = id;
        audioInput.click();
    }

    audioInput.addEventListener("change", function (e) {
        // Get the selected file from the input element
        var files = e.target.files;

        var modalInstance = null;
        if ($scope.audioModalOpeningAction == 'P') {
            modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: root + 'js/ng-templates/audios-upload-template.html',
                controller: 'AudiosUploadCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    files: function () {
                        return files;
                    }
                }
            });
        } else {

            modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: root + 'js/ng-templates/audios-album-template.html',
                controller: 'AlbumAudiosUploadCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    files: function () {
                        return files;
                    },
                    albumId: function () {
                        return $scope.audioAlbumId;
                    },
                    albumName: function () {
                        return $scope.audioAlbumName;
                    }
                }
            });
        }

        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            $scope.audioAlbumId = '';
            $scope.audioAlbumName = '';
            $scope.getUserAudios();
            $scope.getUserAudioAlbums();
            //$scope.UserPhoto = data;
        });
    });

    $scope.openAudio = function (id) {
        modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: root + 'js/ng-templates/audioViewer.html',
            controller: 'portfolioGalleryCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                obj: function () {
                    return id;
                }
            }
        });
    }
    //end audio related functions

    $scope.makeMediaFavorite = function (obj, index) {
        $http.post(root + 'api/UserProfiles/MediaFavorite', obj).then(function success(response) {
            obj = response.data;
            if (response.status == 200) {
                if (obj.Type == 'P')
                    $scope.userPhotos[index] = obj;
                else if (obj.Type == 'A')
                    $scope.userAudios[index] = obj;
                else
                    $scope.userVideos[index] = obj;
            }
        }, function error() { });
    }

    $scope.changeMediaPrivacy = function (obj, index) {
        $http.post(root + 'api/UserProfiles/ChangeMediaPrivacy', obj).then(function success(response) {
            console.log(response.data, 'aa');
            obj = response.data;
            if (response.status == 200) {
                if (obj.Type == 'P')
                    $scope.userPhotos[index] = obj;
                else if (obj.Type == 'A')
                    $scope.userAudios[index] = obj;
                else
                    $scope.userVideos[index] = obj;
            }
        }, function error() { });
    }

    $scope.makeAlbumFavorite = function (obj, index) {
        $http.post(root + 'api/UserProfiles/AlbumFavorite', obj).then(function success(response) {
            obj = response.data;
            if (response.status == 200) {
                if (obj.Type == 'P') {
                    $scope.getUserPhotos();
                    $scope.userAlbums[index] = obj;
                }
                else if (obj.Type == 'A') {
                    $scope.userAudioAlbums[index] = obj;
                    $scope.getUserAudios();
                }
                else {
                    $scope.userVideoAlbums[index] = obj;
                    $scope.getUserVideos();
                }
            }
        }, function error() { });
    }

    $scope.changeAlbumPrivacy = function (obj, index) {
        $http.post(root + 'api/UserProfiles/ChangeAlbumPrivacy', obj).then(function success(response) {
            obj = response.data;
            if (response.status == 200) {
                if (obj.Type == 'P')
                    $scope.userAlbums[index] = obj;
                else if (obj.Type == 'A')
                    $scope.userAudioAlbums[index] = obj;
                else
                    $scope.userVideoAlbums[index] = obj;
            }
        }, function error() { });
    }

    $scope.refreshAlbums = function (id) {
        if (id == 'P') {
            $scope.getUserPhotos();
            $scope.getUserAlbums();
        }
        else if (id == 'A') {
            $scope.getUserAudios();
            $scope.getUserAudioAlbums();
        } else {
            $scope.getUserVideos();
            $scope.getUserVideoAlbums();
        }
    }

    $scope.setClass = function (id) {
        if (id == 'PP')
            $scope.backgroundPublicPreview = {
                'background': '#f1f1f1',
            }
        else {
            $scope.backgroundPublicPreview = {}
        }
    }

    $scope.options = {
        inputClass: 'border-0',
        format: 'hexString'
    };


    $scope.opacity = [0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1];

    $scope.uploadBgImage = function (event) {

        if (event.target.files && event.target.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#bgImage').attr('src', e.target.result);
            }

            reader.readAsDataURL(event.target.files[0]);

            var fd = new FormData();
            //Take the first selected file
            fd.append("file", event.target.files[0]);


            //$http.post(root + 'api/UserProfiles/UploadBGImage', fd).success(function () {
            //}).error(function () {
            //});
            $http({
                url: root + "api/UserProfiles/UploadBGImage",
                dataType: 'json',
                method: 'post',
                data: fd,
                headers: {
                    'Content-Type': undefined
                }
            })
                .then(function loginSuccessCallback(response) { });
        }

    };

    $scope.openBgFileSelector = function () {
        $('#bgSelector').click();
    }

}]);

myApp.controller('PhotosUploadCtrl', function ($scope, $uibModalInstance, $http, files) {
    $scope.Files = [];
    var uppy = null;
    $uibModalInstance.rendered.then(function () {
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    allowedFileTypes: ['.bmp', '.jpg', '.jpeg', '.png', '.gif']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        uppy.on('complete', (result) => {
            var files = Array.from(result.successful);
            $scope.Files = [];
            files.forEach((file) => {
                // file: { id, name, type, ... }
                // progress: { uploader, bytesUploaded, bytesTotal }
                var resp = file.response.uploadURL;
                var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                var fileObj = {};
                fileObj.FileId = id;
                fileObj.Name = file.name;
                fileObj.Type = 'P';
                fileObj.Size = file.size;
                fileObj.ContentType = file.type;
                $scope.Files.push(fileObj);
            });

            if ($scope.Files.length > 0) {
                $http.post(root + 'api/UserProfiles/PostMediaFiles', $scope.Files).then(function success(response) {
                    console.log(response);
                    if (response.status == 200) {
                        gritterAlert("Success", "Files have been uploaded successfully!", true);
                    }
                }, function error() { });
            }
        })

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            uppy.addFile({
                name: file.name,
                type: file.type,
                data: file,
                source: 'Local',
                isRemote: false,
            });
        }
        uppy.run();
    });

    $scope.closeModal = function () {
        $uibModalInstance.dismiss('cancel');
    }

});

myApp.controller('AlbumPhotosUploadCtrl', function ($scope, $uibModalInstance, $http, files, albumId, albumName) {

    $scope.Files = [];
    var uppy = null;
    $scope.data = {
        albumId: albumId,
        albumName: albumName,
        type: 'P'
    };

    $uibModalInstance.rendered.then(function () {
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    allowedFileTypes: ['.bmp', '.jpg', '.jpeg', '.png', '.gif']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            uppy.addFile({
                name: file.name,
                type: file.type,
                data: file,
                source: 'Local',
                isRemote: false,
            });
        }
        uppy.run();
    });

    $scope.Cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.save = function () {
        uppy.upload().then((result) => {
            var files = Array.from(result.successful);
            $scope.Files = [];
            if (files.length > 0) {
                $http.post(root + 'api/UserProfiles/CreateAlbum', $scope.data).then(function success(response) {
                    console.log(response);
                    if (response.status == 200) {
                        gritterAlert("Success", response.data.Message, response.data.success);
                        $scope.data.albumId = response.data.albumId;

                        files.forEach((file) => {
                            // file: { id, name, type, ... }
                            // progress: { uploader, bytesUploaded, bytesTotal }
                            var resp = file.response.uploadURL;
                            var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                            var fileObj = {};
                            fileObj.FileId = id;
                            fileObj.Name = file.name;
                            fileObj.Type = 'P';
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            fileObj.AlbumId = $scope.data.albumId;
                            $scope.Files.push(fileObj);
                        });

                        if ($scope.Files.length > 0) {
                            $http.post(root + 'api/UserProfiles/PostMediaFiles', $scope.Files).then(function success(response) {
                                console.log(response);
                                if (response.status == 200) {
                                    gritterAlert("Success", "Files have been uploaded successfully!", true);
                                }
                            }, function error() { });
                        }
                    }
                }, function error() {
                    alert('something went wrong!');
                });
            }
        });
    }
});

//videos

myApp.controller('VideosUploadCtrl', function ($rootScope, $scope, $uibModalInstance, $http, files) {
    $scope.Files = [];
    var uppy = null;
    $uibModalInstance.rendered.then(function () {
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    allowedFileTypes: ['video/*']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        uppy.on('complete', (result) => {
            var files = Array.from(result.successful);
            $scope.Files = [];
            files.forEach((file) => {
                // file: { id, name, type, ... }
                // progress: { uploader, bytesUploaded, bytesTotal }
                var resp = file.response.uploadURL;
                var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                var fileObj = {};
                fileObj.FileId = id;
                fileObj.Name = file.name;
                fileObj.Type = 'V';
                fileObj.Size = file.size;
                fileObj.ContentType = file.type;
                $scope.Files.push(fileObj);
            });

            if ($scope.Files.length > 0) {
                $http.post(root + 'api/UserProfiles/PostMediaFiles', $scope.Files).then(function success(response) {
                    console.log(response);
                    if (response.status == 200) {
                        gritterAlert("Success", "Files have been uploaded successfully!", true);
                    }
                }, function error() { });
            }
        })

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            uppy.addFile({
                name: file.name,
                type: file.type,
                data: file,
                source: 'Local',
                isRemote: false,
            });
        }
        uppy.run();
    });

    $scope.closeModal = function () {
        $uibModalInstance.dismiss('cancel');
    }

});

myApp.controller('AlbumVideosUploadCtrl', function ($rootScope, $scope, $uibModalInstance, $http, files, albumId, albumName) {

    $scope.Files = [];
    var uppy = null;
    $scope.data = {
        albumId: albumId,
        albumName: albumName,
        type: 'V'
    };

    $uibModalInstance.rendered.then(function () {
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    allowedFileTypes: ['video/*']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            uppy.addFile({
                name: file.name,
                type: file.type,
                data: file,
                source: 'Local',
                isRemote: false,
            });
        }
        uppy.run();
    });

    $scope.Cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.save = function () {
        uppy.upload().then((result) => {
            var files = Array.from(result.successful);
            $scope.Files = [];
            if (files.length > 0) {
                $http.post(root + 'api/UserProfiles/CreateAlbum', $scope.data).then(function success(response) {
                    console.log(response);
                    if (response.status == 200) {
                        gritterAlert("Success", response.data.Message, response.data.success);
                        $scope.data.albumId = response.data.albumId;

                        files.forEach((file) => {
                            // file: { id, name, type, ... }
                            // progress: { uploader, bytesUploaded, bytesTotal }
                            var resp = file.response.uploadURL;
                            var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                            var fileObj = {};
                            fileObj.FileId = id;
                            fileObj.Name = file.name;
                            fileObj.Type = 'V';
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            fileObj.AlbumId = $scope.data.albumId;
                            $scope.Files.push(fileObj);
                        });

                        if ($scope.Files.length > 0) {
                            $http.post(root + 'api/UserProfiles/PostMediaFiles', $scope.Files).then(function success(response) {
                                console.log(response);
                                if (response.status == 200) {
                                    gritterAlert("Success", "Files have been uploaded successfully!", true);
                                }
                            }, function error() { });
                        }
                    }
                }, function error() {
                    alert('something went wrong!');
                });
            }
        });
    }
});

myApp.controller('portfolioGalleryCtrl', function ($scope, $http, $uibModalInstance, obj) {
    $scope.data = obj;
    console.log($scope.data);
    $scope.ok = function () {
        $uibModalInstance.close(files);
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.next = function () {
        $http.get(root + 'api/UserProfiles/NextPrevFileId/' + $scope.data.FileId + '/' + 'N' + '/' + $scope.data.Type).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }

    $scope.prev = function () {
        $http.get(root + 'api/UserProfiles/NextPrevFileId/' + $scope.data.FileId + '/' + 'P' + '/' + $scope.data.Type).then(
            function success(response) {
                $scope.data = response.data.Record;
            }, function error() {
            });
    }
});


//end videos

//audios

myApp.controller('AudiosUploadCtrl', function ($rootScope, $scope, $uibModalInstance, $http, files) {
    $scope.Files = [];
    var uppy = null;
    $uibModalInstance.rendered.then(function () {
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    allowedFileTypes: ['audio/*']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        uppy.on('complete', (result) => {
            var files = Array.from(result.successful);
            $scope.Files = [];
            files.forEach((file) => {
                // file: { id, name, type, ... }
                // progress: { uploader, bytesUploaded, bytesTotal }
                var resp = file.response.uploadURL;
                var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                var fileObj = {};
                fileObj.FileId = id;
                fileObj.Name = file.name;
                fileObj.Type = 'A';
                fileObj.Size = file.size;
                fileObj.ContentType = file.type;
                $scope.Files.push(fileObj);
            });

            if ($scope.Files.length > 0) {
                $http.post(root + 'api/UserProfiles/PostMediaFiles', $scope.Files).then(function success(response) {
                    console.log(response);
                    if (response.status == 200) {
                        gritterAlert("Success", "Files have been uploaded successfully!", true);
                    }
                }, function error() { });
            }
        })

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            uppy.addFile({
                name: file.name,
                type: file.type,
                data: file,
                source: 'Local',
                isRemote: false,
            });
        }
        uppy.run();
    });

    $scope.closeModal = function () {
        $uibModalInstance.dismiss('cancel');
    }

});

myApp.controller('AlbumAudiosUploadCtrl', function ($rootScope, $scope, $uibModalInstance, $http, files, albumId, albumName) {

    $scope.Files = [];
    var uppy = null;
    $scope.data = {
        albumId: albumId,
        albumName: albumName,
        type: 'A'
    };

    $uibModalInstance.rendered.then(function () {
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    allowedFileTypes: ['audio/*']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            uppy.addFile({
                name: file.name,
                type: file.type,
                data: file,
                source: 'Local',
                isRemote: false,
            });
        }
        uppy.run();
    });

    $scope.Cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.save = function () {
        uppy.upload().then((result) => {
            var files = Array.from(result.successful);
            $scope.Files = [];
            if (files.length > 0) {
                $http.post(root + 'api/UserProfiles/CreateAlbum', $scope.data).then(function success(response) {
                    console.log(response);
                    if (response.status == 200) {
                        gritterAlert("Success", response.data.Message, response.data.success);
                        $scope.data.albumId = response.data.albumId;

                        files.forEach((file) => {
                            // file: { id, name, type, ... }
                            // progress: { uploader, bytesUploaded, bytesTotal }
                            var resp = file.response.uploadURL;
                            var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                            var fileObj = {};
                            fileObj.FileId = id;
                            fileObj.Name = file.name;
                            fileObj.Type = 'A';
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            fileObj.AlbumId = $scope.data.albumId;
                            $scope.Files.push(fileObj);
                        });

                        if ($scope.Files.length > 0) {
                            $http.post(root + 'api/UserProfiles/PostMediaFiles', $scope.Files).then(function success(response) {
                                console.log(response);
                                if (response.status == 200) {
                                    gritterAlert("Success", "Files have been uploaded successfully!", true);
                                }
                            }, function error() { });
                        }
                    }
                }, function error() {
                    alert('something went wrong!');
                });
            }
        });
    }
});
//end audios

//end fileupload



myApp.controller('ResourceCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, resource, $rootScope) {
    $scope.title = title;
    $scope.resource = resource;
    $scope.resource.IsEquipment = true;
    $scope.hideIsEquipment = true;
    $scope.labelName = 'Equipment';

    console.log('resource:', $scope.resource);
    var uppy = null;

    $scope.getResourcePhotos = function () {
        $http.get(root + 'api/Resources/GetResourcePhotos?resourceId=' + $scope.resource.Id).then(function success(response) {
            $scope.resourceImages = response.data.Records;
            console.log($scope.resourceImages);
        }, function error() { });
    }

    $scope.getEquipmentCategories = function () {
        $http.get(root + 'api/EquipmentCategories/GetEquipmentCategoryShort').then(function success(response) {
            $scope.equipmentCategories = response.data;
            //console.log('equipmentCategories:', $scope.equipmentCategories);
        }, function error() { });
    }

    $scope.getCurrencies = function () {
        $http.get(root + 'api/Currencies').then(function success(response) {
            $scope.currencies = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getResourceConditions = function () {
        $http.get(root + 'api/ResourceConditions').then(function success(response) {
            $scope.resourceConditions = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getEquipmentCategories();
    $scope.getCurrencies();
    $scope.getResourceConditions();

    if ($scope.resource.Id) {
        $scope.getResourcePhotos();
    }


    $scope.save = function () {
        $scope.resourceVM = angular.copy($scope.resource);

        if ($scope.resourceVM.HireOrSale)
            $scope.resourceVM.HireOrSale = 'S'; //sale
        else
            $scope.resourceVM.HireOrSale = 'H'; //hire


        if ($scope.resourceVM.LendingType)
            $scope.resourceVM.LendingType = 'H'; //hire with operator
        else
            $scope.resourceVM.LendingType = 'R'; //available for rent

        console.log($scope.resourceVM);

        $http.post(root + 'api/Resources/SaveOrUpdateResource', $scope.resourceVM).then(function success(response) {
            if (response.status == 200) {
                console.log(response.data);
                uppy.upload().then((result) => {
                    $scope.Files = [];
                    var files = Array.from(result.successful);
                    files.forEach((file) => {
                        var resp = file.response.uploadURL;
                        var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                        var fileObj = {};
                        fileObj.FileId = id;
                        fileObj.Name = file.name;
                        fileObj.Type = 'P';
                        fileObj.Size = file.size;
                        fileObj.ContentType = file.type;
                        fileObj.ResourceId = response.data.Id;
                        $scope.Files.push(fileObj);
                    });

                    if ($scope.Files.length > 0) {
                        $http.post(root + 'api/Resources/PostResourceMediaFiles', $scope.Files).then(function success(response) {
                            console.log(response);
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: 'Project saved successfully!',
                                });
                                $scope.cancel();
                            }
                        }, function error() { });
                    }
                    else {
                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Project saved successfully!',
                        });
                        $scope.cancel();
                    }
                });

            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }


    $uibModalInstance.rendered.then(function () {
        //alert($rootScope.BackgroundThings.DarkMode);
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                height: 300,
                hideUploadButton: true,
                theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                target: '.DashboardContainer',
                restrictions: {
                    maxNumberOfFiles: 10,
                    allowedFileTypes: ['.bmp', '.jpg', '.jpeg', '.png', '.gif']
                }
            }).use(Uppy.Tus,
                {
                    endpoint: root + 'files/',
                    resume: true,
                    retryDelays: [0, 1000, 3000, 5000],
                    chunkSize: 5242880
                });

        //for (var i = 0; i < files.length; i++) {
        //    var file = files[i];
        //    uppy.addFile({
        //        name: file.name,
        //        type: file.type,
        //        data: file,
        //        source: 'Local',
        //        isRemote: false,
        //    });
        //}
        uppy.run();
    });

    $scope.deleteResourcePhoto = function (obj) {
        $http.delete(root + 'api/Resources/DeleteFile/' + obj.Id + "/" + obj.Type + "/" + obj.FileId).then(function success(response) {
            if (response.status == 200) {
                gritterAlert("Success", response.data.Message, response.data.success);
                if (obj.Type == 'P') {
                    $scope.getResourcePhotos();
                }
            }
        }, function error() { });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }

});