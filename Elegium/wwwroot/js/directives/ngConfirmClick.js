(function () {
    'use strict';

    myApp
        .directive('ngConfirmClick', ngConfirmClick);

    ngConfirmClick.$inject = ['$ngConfirm'];

    function ngConfirmClick($ngConfirm) {
        // Usage:
        //     <ngConfirmBtn></ngConfirmBtn>
        // Creates:
        // 
        var directive = {
            link: link
        };
        return directive;

        function link(scope, element, attr) {
            var msg = attr.ngConfirmClick || "Are you sure?";
            var title = attr.ngConfirmTitle || "Are you sure?"
            var clickAction = attr.confirmedClick;
            element.bind('click', function (event) {
                event.stopPropagation();
                $ngConfirm({
                    title: title,
                    content: msg,
                    autoClose: 'cancel|8000',
                    buttons: {
                        deleteProject: {
                            text: 'Yes',
                            btnClass: 'btn-danger',
                            action: function () {
                                scope.$eval(clickAction);
                            }
                        },
                        cancel: function () {

                        }
                    }
                });
            });
        }
    }
})();