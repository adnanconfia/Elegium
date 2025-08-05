myApp.controller('croppieWithUrlCtrl', ['$scope', '$uibModalInstance', 'title', 'width', 'height', 'imgUrl', function ($scope, $uibModalInstance, title, width, height, imgUrl) {
    $uibModalInstance.rendered.then(function () {
        $scope.title = title;
        $scope.options = {
            viewport: {
                width: width,
                height: height
            },
            boundary: {
                width: $(".modal-body").width(),
                height: $(".modal-body").width() / 2
            }
        };

        $scope.imgUrl = imgUrl;

        $scope.cropped = {
            source: imgUrl
        };

        $scope.ok = function () {
            $uibModalInstance.dismiss($scope.cropped.image);
        }

        $scope.Cancel = function () {
            $uibModalInstance.close();
        }
    });
}])