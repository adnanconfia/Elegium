myApp.controller('documents-ctrl', function ($stateParams, $scope, $http, pId, $state, $uibModal) {
    $scope.documents = [];
    $scope.getDocuments = function () {
        $http.get(root + 'api/Documents/GetDocuments/' + pId.prop).then(resp => {
            $scope.documents = resp.data;
        }, err => {
        });
    }

    $scope.updateRoom = function (doc) {
        $http.post(root + 'api/Documents/PostDocuments', doc).then(
            resp => {
                $scope.getDocuments();
            },
            err => {
            });
    }

    $scope.createRoom = function () {
        $http.post(root + 'api/Documents/PostDocuments', $scope.data).then(
            resp => {
                $scope.data = {
                    Name: "",
                    Description: "",
                    ProjectId: parseInt(pId.prop),
                    Icon: 'texture'
                };
                $scope.getDocuments();
            },
            err => {
            });
    }
    $scope.data = {
        Name: "",
        Description: "",
        ProjectId: parseInt(pId.prop),
        Icon: 'texture'
    };
    $scope.cancelRoomCreation = function () {
        $scope.data = {
            Name: "",
            Description: "",
            ProjectId: parseInt(pId.prop)
        };
    }

    $scope.goToDocument = function (doc, event) {
        if (doc.Id == -1 || event.target.classList.contains('ignore-click')) {
            return;
        }

        $state.transitionTo('documents.documentcategory', { docId: doc.Id, docName: doc.Name }, {
            location: true,
            inherit: true,
            relative: $state.$current,
            notify: false
        });
        // $state.go("documents.documentcategory", {  });
    }

    $scope.changeIcon = function (doc) {
        //console.log(project);
        $http.get(root + 'api/Documents/GetMaterialIcons/').then(function success(response) {
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/documents-and-files/changeicon.html',
                controller: 'iconchangectrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    title: function () {
                        return 'Change icon';
                    },
                    document: function () {
                        return doc;
                    },
                    iconsList: function () {
                        return response.data;
                    }
                }
            });
            modalInstance.result.then(function () {
            }, function (data) {
                console.log(document, 'from modal');
                //$scope.getProjects();
            });
        }, function error() { });


    }

    $scope.deleteDoc = function (doc) {
        $http.post(root + 'api/Documents/DeleteDocuments/' + doc.Id)
            .then(
                suc => {
                    $scope.getDocuments();
                },
                err => {

                });
    }

    $scope.getDocuments();
}).controller('iconchangectrl', function ($scope, $http, $uibModalInstance, title, document, iconsList) {
    $scope.icons = iconsList;
    $scope.title = title;

    $scope.changeDocumentCategoryIcon = function (icon) {
        document.Icon = icon;
        $http.post(root + 'api/Documents/ChangeIcon', document).then(
            suc => {
                $uibModalInstance.dismiss(document);
            },
            err => {
            });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
});