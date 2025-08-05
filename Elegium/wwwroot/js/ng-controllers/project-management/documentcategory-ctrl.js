myApp.controller('documentcategory-ctrl', function ($stateParams, $scope, $http, $stateParams, $state, $uibModal) {
    $scope.documentCategories = [];
    $scope.data = {
        objectName: ""
    };



    $http.get(root + 'api/Documents/GetDocument/' + $stateParams.docId)
        .then(
            resp => {
                $state.current.data.label = resp.data.Name;
                $scope.title = resp.data.Name;
                $scope.document = resp.data;
            },
            err => {

            });


    $scope.openCroppie = function (docCat) {
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
        });
        modalInstance.result.then(function () {
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

    $scope.cancelObjectCreation = function () {
        $scope.data.objectName = "";
    }//

    $scope.getDocumentCategories = function () {

        $http.get(root + 'api/DocumentCategories/GetDocumentCategory/' + $stateParams.docId).then(resp => {
            $scope.documentCategories = resp.data;


        }, err => {
        });
    }

    $scope.callback = function () {
        //$(".imgLiquidFill").imgLiquid({
        //    fill: true,
        //    horizontalAlign: "center",
        //    verticalAlign: "top"
        //});
    }

    $scope.saveOrUpdateCatName = function (docCat) {
        $http.post(root + 'api/DocumentCategories/PostDocumentCategory', docCat).then(
            resp => {
                $scope.getDocumentCategories();
                $scope.cancelObjectCreation();
            },
            err => {
            });
    }

    $scope.createObj = function () {
        $scope.saveOrUpdateCatName({
            Id: -1,
            Name: $scope.data.objectName,
            DocumentId: parseInt($stateParams.docId),
            Icon: 'texture'
        });
    }

    $scope.goToFiles = function (docCat, event) {
        if (docCat.Id == -1 || event.target.classList.contains('ignore-click')) {
            return;
        }
        $state.go("documents.documentcategory.files", { docCatId: docCat.Id });
    }

    $scope.deleteDocCat = function (docCat) {
        $http.post(root + 'api/DocumentCategories/DeleteDocumentCategory/' + docCat.Id)
            .then(
                suc => {
                    $scope.getDocumentCategories();
                },
                err => {

                });
    }

    $scope.getDocumentCategories();

});