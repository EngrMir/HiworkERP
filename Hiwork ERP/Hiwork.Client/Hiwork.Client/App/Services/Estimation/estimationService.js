angular.module("hiworkApp").factory('estimationService', ['$q', '$timeout', '$http', 'ajaxService', 'appSettings', 'AppStorage', 'sessionFactory', 'exceptionHandler', function ($q, $timeout, $http, ajaxService, appSettings, AppStorage, sessionFactory, exceptionHandler) {

    var service = {
        estimationRoutes: estimationRoute,
        estimationServices: estimationService,
        estimationLanguages: estimationLanguage,
        businessCategories: businessCategories,
        specializedFields: specializedFields,
        subSpecializedFields: subSpecializedFields,
        currencyList: currencyList,
        translationCertificateSettingsList: translationCertificateSettingsList,
        manipulateUserAccess: manipulateUserAccess
    };

    var requestConfig = {
        headers: {
            'X-ZUMO-APPLICATION': 'GSECUHNQOOrCwgRHFFYLXWiViGnXNV88'
        }
    };

    var dataObject = {};
    dataObject.CurrentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    dataObject.CurrentUserID = sessionFactory.GetObject(AppStorage.userData).CurrentUserID;
    dataObject.ApplicationId = appSettings.ApplicationId;
    dataObject.Role = sessionFactory.GetObject(AppStorage.userData).Role;
    return service;

    function estimationRoute(data) {
        var url = "estimationRoute/list";
        return httpPost(url, dataObject);
    }
    function estimationService(data) {
        var url = "estimationServiceType/list/" + data;
        return httpPost(url, dataObject);
    }
    function estimationLanguage(data) {
        var url = "language/list";
        return httpPost(url, dataObject);
    }
    function businessCategories(data) {
        var url = "businesscategory/list";
        return httpPost(url, dataObject);
    }
    function specializedFields(data) {
        var url = "estSpecializedFields/list";
        return httpPost(url, dataObject);
    }
    function subSpecializedFields(data) {
        var url = "estimationssf/list";
        return httpPost(url, dataObject);
    }
    function currencyList(data) {
        var url = "currency/list";
        return httpPost(url, dataObject);
    }
    function translationCertificateSettingsList(data) {
        var url = "estimation/translationcertificatesettingslist";
        return httpPost(url, dataObject);
    }

    function httpExecute(requestUrl, method, data) {
        return $http({
            url: appSettings.API_BASE_URL + requestUrl,
            method: method,
            data: data,
            headers: requestConfig.headers
        }).then(function (response) {
            return response.data;
        }, function (error) {           
            exceptionHandler.handleException(error);
        });
    }

    function httpDelete(url) {
        return httpExecute(url, 'DELETE');
    }

    function httpGet(url) {
        return httpExecute(url, 'GET');
    }

    function httpPost(url, data) {
        return httpExecute(url, 'POST', data);
    }

    function manipulateUserAccess(data, createdby) {
        var url = appSettings.API_BASE_URL + 'estimationroleaccess/getitem';        
        var userid = data.UserID;
        $http({ method: 'POST', url: appSettings.API_BASE_URL + 'estimationroleaccess/getitem', data: data }).then(function (r) {
            if (r.data) {
                var arritems = r.data.Options.split(',');
                if (arritems.length) {
                    arritems.forEach(function (ai) {
                        var elmitem = ai.split(':');
                        var tobool = Boolean(Number(elmitem[1]));
                        var element = angular.element(document).find('.' + elmitem[0]);
                        if (elmitem[0] == "btn-approval") {                            
                            if (createdby == userid) {
                                angular.element(document).find('.' + elmitem[0]).attr('disabled', tobool);
                            }
                        }
                        else {
                            if (elmitem[1]) {
                                angular.element(document).find('.' + elmitem[0]).removeAttr('disabled');//.attr('disabled', tobool);
                            }
                            else {
                                angular.element(document).find('.' + elmitem[0]).attr('disabled', true);
                            }
                        }
                    });
                }
            }
        }, function (error) { });
    }

}]);