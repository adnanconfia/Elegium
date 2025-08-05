myApp.controller('OfferResourcesCtrl', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $window) {

    var modalInstance = null;
    $scope.resource = {};

    //Data fetch methods here
    $scope.getResources = function () {
        $http.get(root + 'api/Resources/GetUserResources').then(function success(response) {
            $scope.resourcesList = response.data;
            console.log($scope.resourcesList);
        }, function error() { });
    }



    $scope.getResources();
    //END - Data fetch methods here



    //Other logical methods here

    $scope.addResource = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/offer-resources/add-resource-template.html',
            controller: 'AddResourceCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Add a new Resource';
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
            controller: 'AddResourceCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Edit Resource';
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

    //$scope.openResourceDetail = function (id) {
    //    $window.open(root + 'Resources/Detail/' + id, '_blank');
    //}

});


myApp.controller('AddResourceCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, resource) {

    $scope.title = title;
    $scope.resource = resource;
    $scope.hideIsEquipment = false;
    $scope.labelName = 'Resource';
    console.log('resource:',$scope.resource);
    var uppy = null;

    $scope.getResourcePhotos = function () {
        $http.get(root + 'api/Resources/GetResourcePhotos?resourceId='+$scope.resource.Id).then(function success(response) {
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
        uppy = new Uppy.Core()
            .use(Uppy.Dashboard, {
                inline: true,
                height: 200,
                hideUploadButton: true,
                target: '.DashboardContainer',
                theme: 'light',
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