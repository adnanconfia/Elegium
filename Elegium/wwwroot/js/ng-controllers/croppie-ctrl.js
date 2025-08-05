myApp.controller('CroppiePlayCtrl', ['$scope', '$uibModalInstance', 'title', 'image', 'width', 'height', function ($scope, $uibModalInstance, title, image, width, height) {

    //$scope.title = title;
    //$scope.myWidth = width;
    //$scope.myHeight = height;

    ////$scope.image = image == null ? 'a' : image;
    //$scope.cropped = {
    //    source: 'a',
       
    //};

    //$scope.loaded = true;

    //setTimeout(function () {
    //    // Assign blob to component when selecting a image
    //    $('#upload').on('change', function () {
    //        var input = this;

    //        if (input.files && input.files[0]) {
    //            var reader = new FileReader();

    //            reader.onload = function (e) {
    //                // bind new Image to Component
    //                $scope.cropped.source = e.target.result;
    //                $scope.$apply();
    //            }

    //            reader.readAsDataURL(input.files[0]);
    //        }


    //    });
    //}, 1000);

    //$scope.ok = function () {
    //    $uibModalInstance.dismiss($scope.cropped.image);
    //}

    //$scope.Cancel = function () {
    //    $uibModalInstance.close();
    //}

    $uibModalInstance.opened.then(function () {


        $scope.title = title;
        //$scope.viewportWidth = 0;
        //$scope.viewportHeight = 0;

        //$scope.boundaryHeight = width;
        //$scope.boundaryWidth = height;

        //$scope.image = image == null ? 'a' : image;
        $scope.cropped = {
            source: 'a',
            image: null
        };

        $scope.loaded = false;

        setTimeout(function () {
            $scope.boundaryWidth = $(".modal-body").width()// - ($(".modal-body").width() / 100 * 25);

            $scope.boundaryHeight = $scope.boundaryWidth - ($scope.boundaryWidth / 100 * 50);

            $scope.viewportWidth = $scope.boundaryHeight - ($scope.boundaryHeight / 100 * 25);

            $scope.viewportHeight = $scope.boundaryHeight - ($scope.boundaryHeight / 100 * 25);

            $scope.loaded = true;
            // Assign blob to component when selecting a image
            $('#upload').on('change', function () {
                var input = this;

                if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        // bind new Image to Component
                        $scope.cropped.source = e.target.result;
                        $scope.$apply();
                    }

                    reader.readAsDataURL(input.files[0]);
                }


            });
        }, 1000);

        $scope.ok = function () {
            $uibModalInstance.dismiss($scope.cropped);
        }

        $scope.Cancel = function () {
            $uibModalInstance.close();
        }
    });
}]);