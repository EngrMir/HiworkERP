angular.module("hiworkApp").service('applicationService', ['$http', 'ajaxService', function ($http, ajaxService) {
    this.getAppInfo = function (model, successFunction, errorFunction) {
        ajaxService.AjaxPostWithData(model, "app/getApplicationInfo", successFunction, errorFunction);
    };
}]);