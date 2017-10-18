
angular.module("hiworkApp").service('ajaxService', ['$http', 'blockUI', 'appSettings', function ($http, blockUI, appSettings) {

    // setting timeout of 1 second to simulate a busy server.

    this.AjaxPost = function (data, route, successFunction, errorFunction) {
        blockUI.start();
        setTimeout(function () {
            $http.post(appSettings.API_BASE_URL + route, data).then(function (response) {
                blockUI.stop();
                successFunction(response.data);
            }, function (error) {

                blockUI.stop();
                //if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(error.data);
            });
        }, 1000);

    };

    this.AjaxPostWithNoAuthenication = function (data, route, successFunction, errorFunction) {
        blockUI.start();
        setTimeout(function () {
            $http.post(appSettings.API_BASE_URL + route, data).then(function (response) {
                blockUI.stop();
                successFunction(response.data);
            }, function (error) {

                blockUI.stop();
                //if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(error.data);
            });
        }, 1000);

    };

    this.AjaxGet = function (route, successFunction, errorFunction) {
        blockUI.start();
        setTimeout(function () {
            $http({ method: 'GET', url: appSettings.API_BASE_URL + route }).then(function (response) {
                blockUI.stop();
                successFunction(response.data);
            }, function (error) {

                blockUI.stop();
                //if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(error.data);
            });
        }, 1000);

    };

    this.AjaxGetWithData = function (data, route, successFunction, errorFunction) {
        blockUI.start();
        setTimeout(function () {
            $http({ method: 'GET', url: appSettings.API_BASE_URL + route, params: data }).then(function (response) {
                blockUI.stop();
                successFunction(response.data);
            },function(error){

                blockUI.stop();
                //if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(error.data);
            });
        }, 1000);

    };

    //AjaxPostWithData
    this.AjaxPostWithData = function (data, route, successFunction, errorFunction) {
        blockUI.start();
        setTimeout(function () {
            $http({ method: 'POST', url: appSettings.API_BASE_URL + route, data: data }).then(function (response) {
                blockUI.stop();
                successFunction(response.data);
            }, function (error) {

                blockUI.stop();
                //if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(error.data);
            });
        }, 1000);
    };

    this.AjaxGetWithNoBlock = function (data, route, successFunction, errorFunction) {
        setTimeout(function () {
            $http({ method: 'GET', url: appSettings.API_BASE_URL + route, params: data }).then(function (response) {
                blockUI.stop();
                successFunction(response.data);
            }, function (error) {

                blockUI.stop();
                //if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(error.data);
            });
        }, 0);

    }


}]);



