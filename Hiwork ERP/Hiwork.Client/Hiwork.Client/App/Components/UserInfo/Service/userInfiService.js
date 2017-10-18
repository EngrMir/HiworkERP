angular.module("hiworkApp").service('UserInfoService', ['$http', 'ajaxService', function ($http, ajaxService) {

    this.getUser = function (successFunction, errorFunction) {
        ajaxService.AjaxGet("user/GetUser", successFunction, errorFunction);
    };

    this.getUserTypeList = function (successFunction, errorFunction) {
        ajaxService.AjaxPost("user/getUserTypeList", successFunction, errorFunction);
    };

    this.updateUser = function (user, successFunction, errorFunction) {
        ajaxService.AjaxPost(user, "user/UpdateUser", successFunction, errorFunction);
    };

    this.addUser = function (user, successFunction, errorFunction) {
        ajaxService.AjaxPost(user, "user/addUser", successFunction, errorFunction);
    }
}]);