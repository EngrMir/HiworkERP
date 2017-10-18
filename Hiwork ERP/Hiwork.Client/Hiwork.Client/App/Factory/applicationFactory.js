/**
 * Created by Tomas on 28-July-17.
 */
angular.module("hiworkApp").factory("applicationFactory", function ($location, $http, sessionFactory, applicationService,AppStorage) {
    var appData = null;
    var onGetData = function (response) {
        appData = response;
        sessionFactory.SetObject(AppStorage.appData, appData.data);
    };
    var onError = function (message) {
    };

    return {
        GetAppInfo: function (model) {
            applicationService.getAppInfo(model, onGetData, onError);
        },
        AppData: function () {
            return appData;
        }
    };
});