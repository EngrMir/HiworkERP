/// <reference path="../Templates/ConfirmationAlert.html" />
//(function (module) {

//    var confirmationAlertAll = function ($uibModal) {
//        return function (model) {

//            var options = {
//                templateUrl: "App/Components/UserInfo/Templates/ConfirmationAlert.html",
//                controller: function ($uibModalInstance) {
//                    this.model = model;
//                    this.deleteRow = function () {
//                        $uibModalInstance.dismiss();
//                    }
//                },
//                controllerAs: "vm"
//            };

//            return $uibModal.open(options).result;
//        };
//    };

//    module.factory("confirmationAlertAll", confirmationAlertAll);

//}(angular.module("hiworkApp")));
