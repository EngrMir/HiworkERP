angular.module("hiworkApp").service("employeeService", ['$q', '$timeout', '$http', 'ajaxService', 'appSettings', 'AppStorage', 'sessionFactory', 'ajaxService', function ($q, $timeout, $http, ajaxService, appSettings, AppStorage, sessionFactory, ajaxService) {

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);


    var BaseModel = {}
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    var service = {
        getCompany: getCompanyList, 
        getCompanyConfigData:GetConfigData
    };

    return service;

    function getEmployeeList() {
        return httpPost("employee/list", BaseModel);
    }


    function GetConfigData()
    {
        return httpPost("company/config", BaseModel);
    }

    function httpExecute(requestUrl, method, data) {
       
        return $http({
            url: appSettings.API_BASE_URL + requestUrl,
            method: method,
            data: data
        }).then(function (response) {
            return response.data;
        }, function (error) {
            exceptionHandler.handleException(error);
        });
    }

    function httpPost(url, data) {
        return httpExecute(url, 'POST', data);
    }
}]);