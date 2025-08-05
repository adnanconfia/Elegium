myApp.controller('ResourcesRequestsList', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $window, $state) {
    $http.get(root + 'api/ProjectResource/GetMyOffers').then(
        (suc) => {
            $scope.myOffers = suc.data;
        },
        (err) => {
            alert(err);
        }
    );

    $http.get(root + 'api/ProjectResource/GetMyRequests').then(
        (suc) => {
            $scope.myRequests = suc.data;
        },
        (err) => {
            alert(err);
        }
    )

    $scope.takeAction = function (action, offer) {
        offer.Action = action;
        $ngConfirm({
            title: action == 'A' ? 'Accept Offer?' : action == "R"? 'Reject Offer' : 'Withdraw offer',
            content: action == 'A' ? 'Are you sure to accept this offer?' : action == "R" ? 'Are you sure want to reject this offer?' : 'Are you sure want to withdraw this offer?',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Yes',
                    btnClass: 'btn-primary',
                    action: function () {
                        $http.post(root + 'api/ProjectResource/TakeResourceRequestAction/', offer).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: action == 'A' ? 'Offer accepted' : action == "R" ? 'Offer rejected' : 'Offer canceled',
                                });
                                offer.Status = action;
                            }
                        }, function error() { });
                    }
                },
                cancel: function () {

                }
            }
        });
    }
});