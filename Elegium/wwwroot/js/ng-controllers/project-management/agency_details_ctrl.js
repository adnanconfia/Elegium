
myApp.controller('agency_details_ctrl', function ($scope, $rootScope, $filter, $http, $timeout, $uibModal, toaster, $ngConfirm,$stateParams, $state, orderByFilter) {
 
    angular.element(document).ready(function () {
        $timeout(function () {
            var charId = parseInt($stateParams.charId);
            $scope.charId = parseInt($stateParams.charId);
            var projectId = parseInt($stateParams.id);
            $scope.projectId = projectId;
            if (parseInt(charId) === 0) {
                window.location.href = root + "#/" + projectId + "/cast";
            }
            $scope.actors = [];
            $rootScope.getagency = () => {
                $scope.getAgency();
          
            }

            $scope.getAgency = () => {
                $scope.actors = [];
                $http.get(root + 'api/ScenesandScript/GetAgency?projectid=' + projectId).then(function (response) {
                    if (response.status === 200) {

                        console.log($scope.agency);
                        response.data.forEach(function (item) {
                            if (item.Agency.Id == $scope.charId) {
                                item.files = [];
                                $http.get(root + 'api/ScenesandScript/GetAgencyFiles/' + parseInt(item.Agency.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                                    $scope.defaultImage = resp.data.FileId;
                                    $scope.files1 = resp.data.list;
                                    item.FileCount = $scope.files1.length;
                                    angular.forEach($scope.files1, function (item1) {
                                        if (item1.Default === true) {
                                            item.Default = item1;

                                        }
                                    });
                                    if (item.FileCount > 0) {

                                        item.HasFile = true;
                                        item.files = $scope.files1;
                                        //item.FileId = $scope.Files[0].FileId;
                                    } else {
                                        item.HasFile = false;
                                    }
                                    $scope.agency = item;
                                    $scope.agency.Agency.Country = { "name": $scope.agency.Agency.Country }
                                }

                                    , err => {
                                    });
                                 $http.get(root + 'api/ScenesandScript/GetTalent?projectid=' + projectId).then(function (response) {
                                    if (response.status === 200) {
                                        $scope.Talent = response.data;

                                        $scope.Talent.forEach(function (item) {
                                            if (item.AgencyId === $scope.charId) {
                                                item.TalentId = item.Id;
                                                console.log(item);
                                                $http.get(root + 'api/ScenesandScript/GetTalentFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                                                    $scope.defaultImage = resp.data.FileId;
                                                    $scope.files1 = resp.data.list;
                                                    item.FileCount = $scope.files1.length;

                                                    if (item.FileCount > 0) {
                                                        console.log($scope.files1);
                                                        item.HasFile = true;
                                                        item.file = $scope.files1[0];
                                                        //item.FileId = $scope.Files[0].FileId;
                                                    } else {
                                                        item.HasFile = false;
                                                    }
                                                }, err => {
                                                });
                                                $scope.actors.push(item);
                                            }
                                            });

                                    }
                                }, function (error) { });
                                 $http.get(root + 'api/ScenesandScript/GetActor?projectid=' + projectId).then(function (response) {
                                    if (response.status === 200) {
                                        $scope.Actor = response.data;

                                        $scope.Actor.forEach(function (item) {
                                            if (item.AgencyId === $scope.charId) {
                                                item.ActorId = item.Id;
                                                $http.get(root + 'api/ScenesandScript/GetActorFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                                                    $scope.defaultImage = resp.data.FileId;
                                                    $scope.files1 = resp.data.list;
                                                    item.FileCount = $scope.files1.length;
                                                    if (item.FileCount > 0) {
                                                        console.log($scope.files1);
                                                        item.HasFile = true;
                                                        item.file = $scope.files1[0];
                                                        //item.FileId = $scope.Files[0].FileId;
                                                    } else {
                                                        item.HasFile = false;
                                                    }
                                                }, err => {
                                                });
                                                $scope.actors.push(item);
                                            }
                                        });
                                    }
                                }, function (error) { });
                            }
                        });
                        initializeTaskObj();
                    }
                }, function (error) { });
            }
            $scope.openProjectFileDialog1 = function () {
                //$scope.isItDocDacThumbnail = true;
                //$('#projectFile').click();
                //document.getElementById('idDocCatThumbnailFile').click();
                var modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: root + 'js/ng-templates/croppie-playground.html',
                    controller: 'CroppiePlayCtrlCustom',
                    size: 'lg',
                    resolve: {
                        title: function () {
                            return "Select Thumbnail";
                        },
                        image: function () {
                            return $scope.agency.Default;
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
            $scope.DeleteAgency = (Agency) => {
                Agency.Agency.Is_Deleted = true;
                if (Agency.Agency.Country) {
                    Agency.Agency.Country = Agency.Agency.Country.name;
                } else {
                    Agency.Agency.Country = null;
                }
                var agencyDto = {
                    "Agency": Agency.Agency,
                    "AgencyContact": Agency.AgencyContact
                }
                console.log($scope.agencyDto);
                $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
                    if (resp.status === 200) {
                        $rootScope.getAgency();
                        window.location.href = root + "#/" + projectId + "/cast";
                    }
                }, function error() { });
            }
            $scope.crop = (docCat) => {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: root + 'js/ng-templates/croppieWithUrl.html',
                    controller: 'croppieWithUrlCtrl',
                    size: 'lg',
                    resolve: {
                        title: function () {
                            return "Adjust photo view";
                        },
                        width: function () { return 200 },
                        height: function () { return 200 },
                        imgUrl: function () {
                            return '/api/UserProfiles/DownloadFile/' + docCat.FileId;
                        }
                    }
                }); modalInstance.result.then(function () {
                    //on ok button press 
                }, function (data) {
                    document.getElementById('img' + docCat.Id).style.backgroundImage = 'url(' + data + ')';
                    $http.post(root + 'api/DocumentCategories/PostFileThumbnail', {
                        FileId: docCat.FileId,
                        FileArray: data
                    }).then(
                        resp => {
                        },
                        err => {
                        });
                });
            }
            $scope.showDel = (id, type) => {

                $scope.charIndex = id + type;
            }
            $scope.hideDel = () => {
                $scope.charIndex = -1;
            }
            $scope.Editinfo = false;
            $scope.show_Edit_info = () => {
                $scope.Editinfo = true;
            }
            $scope.EditContact = false;
            $scope.show_contact_info = () => {
                $scope.EditContact = true;
            }
            $scope.hideContact = () => {
                $scope.EditContact = false;
            }
            $scope.CreatContact = (contact) => {
                $scope.agency.AgencyContact.push(contact);
                $scope._contact = {};
                $scope.UpdateAgency($scope.agency);
            }
            $scope.charIndex = -1;
            $scope.showDel = (id, type) => {
                $scope.charIndex = id + type;
            }
 
            var modalInstance = null;
            $scope.create_Talent = function (ch) {
                modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: root + 'js/ng-templates/Cast/create-CommonTalent.html',
                    controller: 'CreateCommontTalentCtrl',
                    size: 'md',
                    backdrop: 'static',
                    resolve: {
                        title: function () {
                            return 'Create actor or talent ';
                        },
                        projectItem: function () {
                            return null;
                        },
                        projectId: function () {
                            return $scope.projectId;
                        },
                        Name: function () {
                            return "";
                        },
                        Agency: function () {
                            return ch;
                        }

                    }
                });
                modalInstance.result.then(function () {

                }, function (data) {

                });
            }

            //Tasks

            var initializeTaskObj = function () {
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
                    Section: "",
                    HasSection: false,
                    SectionUrl: "",

                    AgencyId: parseInt($scope.charId)
                }




                $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + $stateParams.id).then(resp => {
                    $scope.projectUsersAndGroups = resp.data;
                }, err => {
                });

                $scope.groupUsers = function (item) {
                    return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
                }

                $http.get(root + 'api/Tasks/GetAllTasks/' + $stateParams.id).then(resp => {
                    $scope.mytasks = resp.data;
                    $scope.mytasks = $scope.mytasks.filter(function (item) {


                        if (item.AgencyId == parseInt($scope.charId)) {
                            //console.log(exist, item.Id);
                            return item;
                        }
                    });
                    $http.get(root + 'api/Documents/GetProjectWiseUserMenu/' + $stateParams.id).then(function success(response) {
                        $scope.sections = response.data.projectUserMenu;
                    }, function error() {
                    });

                    $http.get(root + 'api/Tasks/GetProjectObjects/' + $stateParams.id).then(function success(response) {
                        $scope.objects = response.data;
                    }, function error() {
                    });

                }, err => {
                });
                $scope.createTask = function () {
                    $http.post(root + 'api/ProjectTasks/PostProjectTask/', $scope.taskObj).then(resp => {
                        initializeTaskObj();
                        //$scope.mytasks.push(resp.data);
                    }, err => {
                    });
                }

                $scope.goToTaskProfile = function (taskId) {
                    $state.go('tasks.mytasks.taskprofile', { taskId: taskId, id: projectId });
                }

                $scope.cancelTaskCreation = function () {
                    if ($scope.taskObj.HasDeadline)
                        $('#toggleCalendarDeadline').trigger('click');

                    if ($scope.taskObj.HasSection)
                        $('#section-selector').removeClass('show');

                    initializeTaskObj();
                }

                $scope.changeTaskStatus = function (event, task) {
                    event.stopPropagation();
                    $http.post(root + 'api/ProjectTasks/ChangeProjectTaskStatus/', task).then(resp => {
                        var index = $scope.mytasks.indexOf(task);
                        if (index > -1)
                            $scope.mytasks[index] = resp.data;
                    }, err => {
                    });
                }

                $scope.deleteTask = function (event, task) {
                    //event.stopPropagation();
                    $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
                        var index = $scope.mytasks.indexOf(task);
                        if (index > -1)
                            $scope.mytasks.splice(index, 1);
                    }, err => {
                    });
                }



            }
            initializeTaskObj();
            $scope.ShowDel = (id, type) => {
                $scope.charIndex = id + type;
            }
            $scope.hideDel = () => {
                $scope.charIndex = -1;
            }
            $scope.DeleteActor = (Actor) => {
                if (Actor.TalentId) {
                    Actor.AgencyId = null;
                    $http.post(root + 'api/scenesandscript/CreateTalent/', Actor).then(function (resp) {
                        if (resp.status === 200) {
                            $rootScope.getagency();
                        }
                    }, function error() { });
                } else {

                    Actor.AgencyId = null;
                    $http.post(root + 'api/scenesandscript/CreateActor/', Actor).then(function (resp) {
                        if (resp.status === 200) {
                            $rootScope.getagency();
                        }
                    }, function error() { });
                }
            }
            $scope.deleteAgencyContact = (id) => {
                $http.delete(root + 'api/ScenesandScript/DeleteAgencyContact/'+ id).then(function (resp) {
                    if (resp.status === 200) {
                        $rootScope.getAgency();
                        $scope.getAgency();
                    }
                }, function error() { });
            }
            $scope.UpdateAgency = (Agency) => {
                Agency.Agency.Country = Agency.Agency.Country.name;
                var agencyDto = {
                    "Agency": Agency.Agency,
                    "AgencyContact": Agency.AgencyContact
                }
                console.log($scope.agencyDto);
                $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
                    if (resp.status === 200) {
                        $scope.Editinfo = false;
                        $scope.EditContact = false;
                        $rootScope.getAgency();
                        $scope.getAgency();
                    }
                }, function error() { });
            }
            $scope.getAgency();
            $scope.openFileViewer = function (file, allFiles) {
                //console.log(project);
                setTimeout(function () {
                    $("[uib-modal-window='modal-window']").css('z-index', '9999999');
                }, 1000);
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
                        },
                        projectUsersAndGroups: function () {
                            return $scope.projectUsersAndGroups
                        },
                        fileProfileLink: function () {
                            return 'scenesandscripts.scene_details.fileProfile'
                        }
                    }
                });
                modalInstance.result.then(function () {
                }, function (data) {
                    console.log(data, 'from modal');
                    //$scope.getProjects();
                });

            }

            //Files
            var holder;
            var holderchk = false;

            $scope.tabContentLength = {

            };

            holder = document.getElementById('draggableContainer');
            if (holder != null) {
                holderchk = true;
            } else {
                holderchk = false;
            }
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
                        var validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                        if (validImageTypes.includes(file.type)) {
                            // invalid file type code goes here.
                            fileObj.Type = "image";
                        }
                        fileObj.Default = file.name == 'thumbnail.png' ? true : false;
                        fileObj.SceneId = null;
                        $scope.uploadedFiles.push(fileObj);
                        //$scope.files.push(fileObj);
                    });
                    if ($scope.uploadedFiles.length > 0) {
                        $scope.$apply(function () {
                            $scope.hasFilesinUppy = true;
                        });

                    }
                    if ($scope.uploadedFiles) {
                        if ($scope.uploadedFiles.length > 0) {


                            angular.forEach($scope.uploadedFiles, function (item) {

                                item.AgencyId = parseInt($scope.charId);

                            });
                            $http.post(root +'api/DocumentFiles/PostCastDocumentFiles', $scope.uploadedFiles).then(
                                function success(resp) {
                                    console.log(resp.data);
                                    if (resp.data.length > 0) {

                                        $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                        resp.data.forEach((file) => {
                                            $scope.agency.files.push(file);

                                            if (file.Default)
                                                $scope.defaultImage = file.FileId;
                                        });
                                        $scope.uppy.reset();

                                        $rootScope.getAgency();
                                        $scope.getAgency();
                                    }
                                }

                                , function error() { });
                        }
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
                $scope.isItDocDacThumbnail = false;
                addFilesToUppy(event.target.files);
            });


            var lastTarget;

            if (holderchk) {
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
                size: 1200
            }

            $scope.deleteFile = function ($event, file) {
                $event.stopPropagation();
                var index = $scope.files.indexOf(file);
                if (index > -1)
                    $scope.Actor.files.splice(index, 1);

                $http.post(root + 'api/DocumentFiles/DeleteDocumentFiles/' + file.Id).then(resp => {
                    //$scope.getLinks();
                    $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                    if ($scope.tabContentLength.filesCount < 0) {
                        $scope.tabContentLength.filesCount = 0;
                    }

                    $rootScope.getactor();
                    $scope.getactor();
                }, err => {
                });

            }

            $scope.countries = [
                { name: 'Afghanistan', code: 'AF' },
                { name: 'Åland Islands', code: 'AX' },
                { name: 'Albania', code: 'AL' },
                { name: 'Algeria', code: 'DZ' },
                { name: 'American Samoa', code: 'AS' },
                { name: 'Andorra', code: 'AD' },
                { name: 'Angola', code: 'AO' },
                { name: 'Anguilla', code: 'AI' },
                { name: 'Antarctica', code: 'AQ' },
                { name: 'Antigua and Barbuda', code: 'AG' },
                { name: 'Argentina', code: 'AR' },
                { name: 'Armenia', code: 'AM' },
                { name: 'Aruba', code: 'AW' },
                { name: 'Australia', code: 'AU' },
                { name: 'Austria', code: 'AT' },
                { name: 'Azerbaijan', code: 'AZ' },
                { name: 'Bahamas', code: 'BS' },
                { name: 'Bahrain', code: 'BH' },
                { name: 'Bangladesh', code: 'BD' },
                { name: 'Barbados', code: 'BB' },
                { name: 'Belarus', code: 'BY' },
                { name: 'Belgium', code: 'BE' },
                { name: 'Belize', code: 'BZ' },
                { name: 'Benin', code: 'BJ' },
                { name: 'Bermuda', code: 'BM' },
                { name: 'Bhutan', code: 'BT' },
                { name: 'Bolivia', code: 'BO' },
                { name: 'Bosnia and Herzegovina', code: 'BA' },
                { name: 'Botswana', code: 'BW' },
                { name: 'Bouvet Island', code: 'BV' },
                { name: 'Brazil', code: 'BR' },
                { name: 'British Indian Ocean Territory', code: 'IO' },
                { name: 'Brunei Darussalam', code: 'BN' },
                { name: 'Bulgaria', code: 'BG' },
                { name: 'Burkina Faso', code: 'BF' },
                { name: 'Burundi', code: 'BI' },
                { name: 'Cambodia', code: 'KH' },
                { name: 'Cameroon', code: 'CM' },
                { name: 'Canada', code: 'CA' },
                { name: 'Cape Verde', code: 'CV' },
                { name: 'Cayman Islands', code: 'KY' },
                { name: 'Central African Republic', code: 'CF' },
                { name: 'Chad', code: 'TD' },
                { name: 'Chile', code: 'CL' },
                { name: 'China', code: 'CN' },
                { name: 'Christmas Island', code: 'CX' },
                { name: 'Cocos (Keeling) Islands', code: 'CC' },
                { name: 'Colombia', code: 'CO' },
                { name: 'Comoros', code: 'KM' },
                { name: 'Congo', code: 'CG' },
                { name: 'Congo, The Democratic Republic of the', code: 'CD' },
                { name: 'Cook Islands', code: 'CK' },
                { name: 'Costa Rica', code: 'CR' },
                { name: 'Cote D\'Ivoire', code: 'CI' },
                { name: 'Croatia', code: 'HR' },
                { name: 'Cuba', code: 'CU' },
                { name: 'Cyprus', code: 'CY' },
                { name: 'Czech Republic', code: 'CZ' },
                { name: 'Denmark', code: 'DK' },
                { name: 'Djibouti', code: 'DJ' },
                { name: 'Dominica', code: 'DM' },
                { name: 'Dominican Republic', code: 'DO' },
                { name: 'Ecuador', code: 'EC' },
                { name: 'Egypt', code: 'EG' },
                { name: 'El Salvador', code: 'SV' },
                { name: 'Equatorial Guinea', code: 'GQ' },
                { name: 'Eritrea', code: 'ER' },
                { name: 'Estonia', code: 'EE' },
                { name: 'Ethiopia', code: 'ET' },
                { name: 'Falkland Islands (Malvinas)', code: 'FK' },
                { name: 'Faroe Islands', code: 'FO' },
                { name: 'Fiji', code: 'FJ' },
                { name: 'Finland', code: 'FI' },
                { name: 'France', code: 'FR' },
                { name: 'French Guiana', code: 'GF' },
                { name: 'French Polynesia', code: 'PF' },
                { name: 'French Southern Territories', code: 'TF' },
                { name: 'Gabon', code: 'GA' },
                { name: 'Gambia', code: 'GM' },
                { name: 'Georgia', code: 'GE' },
                { name: 'Germany', code: 'DE' },
                { name: 'Ghana', code: 'GH' },
                { name: 'Gibraltar', code: 'GI' },
                { name: 'Greece', code: 'GR' },
                { name: 'Greenland', code: 'GL' },
                { name: 'Grenada', code: 'GD' },
                { name: 'Guadeloupe', code: 'GP' },
                { name: 'Guam', code: 'GU' },
                { name: 'Guatemala', code: 'GT' },
                { name: 'Guernsey', code: 'GG' },
                { name: 'Guinea', code: 'GN' },
                { name: 'Guinea-Bissau', code: 'GW' },
                { name: 'Guyana', code: 'GY' },
                { name: 'Haiti', code: 'HT' },
                { name: 'Heard Island and Mcdonald Islands', code: 'HM' },
                { name: 'Holy See (Vatican City State)', code: 'VA' },
                { name: 'Honduras', code: 'HN' },
                { name: 'Hong Kong', code: 'HK' },
                { name: 'Hungary', code: 'HU' },
                { name: 'Iceland', code: 'IS' },
                { name: 'India', code: 'IN' },
                { name: 'Indonesia', code: 'ID' },
                { name: 'Iran, Islamic Republic Of', code: 'IR' },
                { name: 'Iraq', code: 'IQ' },
                { name: 'Ireland', code: 'IE' },
                { name: 'Isle of Man', code: 'IM' },
                { name: 'Israel', code: 'IL' },
                { name: 'Italy', code: 'IT' },
                { name: 'Jamaica', code: 'JM' },
                { name: 'Japan', code: 'JP' },
                { name: 'Jersey', code: 'JE' },
                { name: 'Jordan', code: 'JO' },
                { name: 'Kazakhstan', code: 'KZ' },
                { name: 'Kenya', code: 'KE' },
                { name: 'Kiribati', code: 'KI' },
                { name: 'Korea, Democratic People\'s Republic of', code: 'KP' },
                { name: 'Korea, Republic of', code: 'KR' },
                { name: 'Kuwait', code: 'KW' },
                { name: 'Kyrgyzstan', code: 'KG' },
                { name: 'Lao People\'s Democratic Republic', code: 'LA' },
                { name: 'Latvia', code: 'LV' },
                { name: 'Lebanon', code: 'LB' },
                { name: 'Lesotho', code: 'LS' },
                { name: 'Liberia', code: 'LR' },
                { name: 'Libyan Arab Jamahiriya', code: 'LY' },
                { name: 'Liechtenstein', code: 'LI' },
                { name: 'Lithuania', code: 'LT' },
                { name: 'Luxembourg', code: 'LU' },
                { name: 'Macao', code: 'MO' },
                { name: 'Macedonia, The Former Yugoslav Republic of', code: 'MK' },
                { name: 'Madagascar', code: 'MG' },
                { name: 'Malawi', code: 'MW' },
                { name: 'Malaysia', code: 'MY' },
                { name: 'Maldives', code: 'MV' },
                { name: 'Mali', code: 'ML' },
                { name: 'Malta', code: 'MT' },
                { name: 'Marshall Islands', code: 'MH' },
                { name: 'Martinique', code: 'MQ' },
                { name: 'Mauritania', code: 'MR' },
                { name: 'Mauritius', code: 'MU' },
                { name: 'Mayotte', code: 'YT' },
                { name: 'Mexico', code: 'MX' },
                { name: 'Micronesia, Federated States of', code: 'FM' },
                { name: 'Moldova, Republic of', code: 'MD' },
                { name: 'Monaco', code: 'MC' },
                { name: 'Mongolia', code: 'MN' },
                { name: 'Montserrat', code: 'MS' },
                { name: 'Morocco', code: 'MA' },
                { name: 'Mozambique', code: 'MZ' },
                { name: 'Myanmar', code: 'MM' },
                { name: 'Namibia', code: 'NA' },
                { name: 'Nauru', code: 'NR' },
                { name: 'Nepal', code: 'NP' },
                { name: 'Netherlands', code: 'NL' },
                { name: 'Netherlands Antilles', code: 'AN' },
                { name: 'New Caledonia', code: 'NC' },
                { name: 'New Zealand', code: 'NZ' },
                { name: 'Nicaragua', code: 'NI' },
                { name: 'Niger', code: 'NE' },
                { name: 'Nigeria', code: 'NG' },
                { name: 'Niue', code: 'NU' },
                { name: 'Norfolk Island', code: 'NF' },
                { name: 'Northern Mariana Islands', code: 'MP' },
                { name: 'Norway', code: 'NO' },
                { name: 'Oman', code: 'OM' },
                { name: 'Pakistan', code: 'PK' },
                { name: 'Palau', code: 'PW' },
                { name: 'Palestinian Territory, Occupied', code: 'PS' },
                { name: 'Panama', code: 'PA' },
                { name: 'Papua New Guinea', code: 'PG' },
                { name: 'Paraguay', code: 'PY' },
                { name: 'Peru', code: 'PE' },
                { name: 'Philippines', code: 'PH' },
                { name: 'Pitcairn', code: 'PN' },
                { name: 'Poland', code: 'PL' },
                { name: 'Portugal', code: 'PT' },
                { name: 'Puerto Rico', code: 'PR' },
                { name: 'Qatar', code: 'QA' },
                { name: 'Reunion', code: 'RE' },
                { name: 'Romania', code: 'RO' },
                { name: 'Russian Federation', code: 'RU' },
                { name: 'Rwanda', code: 'RW' },
                { name: 'Saint Helena', code: 'SH' },
                { name: 'Saint Kitts and Nevis', code: 'KN' },
                { name: 'Saint Lucia', code: 'LC' },
                { name: 'Saint Pierre and Miquelon', code: 'PM' },
                { name: 'Saint Vincent and the Grenadines', code: 'VC' },
                { name: 'Samoa', code: 'WS' },
                { name: 'San Marino', code: 'SM' },
                { name: 'Sao Tome and Principe', code: 'ST' },
                { name: 'Saudi Arabia', code: 'SA' },
                { name: 'Senegal', code: 'SN' },
                { name: 'Serbia and Montenegro', code: 'CS' },
                { name: 'Seychelles', code: 'SC' },
                { name: 'Sierra Leone', code: 'SL' },
                { name: 'Singapore', code: 'SG' },
                { name: 'Slovakia', code: 'SK' },
                { name: 'Slovenia', code: 'SI' },
                { name: 'Solomon Islands', code: 'SB' },
                { name: 'Somalia', code: 'SO' },
                { name: 'South Africa', code: 'ZA' },
                { name: 'South Georgia and the South Sandwich Islands', code: 'GS' },
                { name: 'Spain', code: 'ES' },
                { name: 'Sri Lanka', code: 'LK' },
                { name: 'Sudan', code: 'SD' },
                { name: 'Suriname', code: 'SR' },
                { name: 'Svalbard and Jan Mayen', code: 'SJ' },
                { name: 'Swaziland', code: 'SZ' },
                { name: 'Sweden', code: 'SE' },
                { name: 'Switzerland', code: 'CH' },
                { name: 'Syrian Arab Republic', code: 'SY' },
                { name: 'Taiwan, Province of China', code: 'TW' },
                { name: 'Tajikistan', code: 'TJ' },
                { name: 'Tanzania, United Republic of', code: 'TZ' },
                { name: 'Thailand', code: 'TH' },
                { name: 'Timor-Leste', code: 'TL' },
                { name: 'Togo', code: 'TG' },
                { name: 'Tokelau', code: 'TK' },
                { name: 'Tonga', code: 'TO' },
                { name: 'Trinidad and Tobago', code: 'TT' },
                { name: 'Tunisia', code: 'TN' },
                { name: 'Turkey', code: 'TR' },
                { name: 'Turkmenistan', code: 'TM' },
                { name: 'Turks and Caicos Islands', code: 'TC' },
                { name: 'Tuvalu', code: 'TV' },
                { name: 'Uganda', code: 'UG' },
                { name: 'Ukraine', code: 'UA' },
                { name: 'United Arab Emirates', code: 'AE' },
                { name: 'United Kingdom', code: 'GB' },
                { name: 'United States', code: 'US' },
                { name: 'United States Minor Outlying Islands', code: 'UM' },
                { name: 'Uruguay', code: 'UY' },
                { name: 'Uzbekistan', code: 'UZ' },
                { name: 'Vanuatu', code: 'VU' },
                { name: 'Venezuela', code: 'VE' },
                { name: 'Vietnam', code: 'VN' },
                { name: 'Virgin Islands, British', code: 'VG' },
                { name: 'Virgin Islands, U.S.', code: 'VI' },
                { name: 'Wallis and Futuna', code: 'WF' },
                { name: 'Western Sahara', code: 'EH' },
                { name: 'Yemen', code: 'YE' },
                { name: 'Zambia', code: 'ZM' },
                { name: 'Zimbabwe', code: 'ZW' }
            ];

        }, 100);
    });
});