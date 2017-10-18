angular.module('hiworkApp').directive("confirmWindow", ["$uibModal", function ($uibModal) {

    var getConfirmAlertModalTemplate = function (message) {
        var modalTemplate = "<div class=\"modal-body\"> " + message + "</div> " +
                            "<div class=\"modal-footer modal-footer-buttons\"> " +
                             "<button class=\"btn btn-danger\" type=\"button\" ng-click=\"closeConfirmAlertModal()\">{{'CLOSE' | translator}}</button> " +
                                "<button class=\"btn btn-success\" type=\"button\" ng-click=\"okConfirmAlertModal(true)\">{{'CONFIRM' | translator}}</button> " +
                               
                            "</div>";

        return modalTemplate;
    }

    return {
        restrict: "A",
        scope: {
            text: "@modalTitleNew",
            confirmAction: "&"
        },
        link: function (scope, element, attrs) {
            element.bind("click", function ($event) {
                //$event.target.blur();

                var message = attrs.modalMessage,
                    title = attrs.modalTitle,
                    value = attrs.confirmValue,
                    reqData = attrs.confirmData,
                    reqUrl = attrs.confirmUrl
               
                $uibModal.open({
                    backdrop: "static",
                    template: getConfirmAlertModalTemplate(title),
                    controller: ["$scope", "$http", "$filter", "$rootScope","appSettings","$uibModalInstance",
                        function ($scope, $http, $filter, $rootScope,appSettings, $uibModalInstance) {
                        $scope.okConfirmAlertModal = function (confirmed) {
                            if (confirmed) {                             
                                $http.post(appSettings.API_BASE_URL + reqUrl, reqData).then(function (response) {
                                    toastr.success($filter('translator')('DATADELETED'));                                   
                                    $uibModalInstance.close('ok');
                                    $rootScope.$broadcast("dataDeleted", response);
                                }, function (error) {
                                    toastr.error($filter('translator')('ERRORDBOPERATION'))
                                    $uibModalInstance.close('ok');
                                });
                            }
                        };
                        $scope.closeConfirmAlertModal = function () {
                            $uibModalInstance.close('ok');
                        };
                    }]
                });
            });
        }
    }
}]);