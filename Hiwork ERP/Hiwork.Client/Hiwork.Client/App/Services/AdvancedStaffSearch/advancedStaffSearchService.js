angular.module("hiworkApp").factory('advancedStaffSearchService', ['$q', '$timeout', '$http', 'ajaxService', 'appSettings', 'AppStorage', 'sessionFactory', 'exceptionHandler', function ($q, $timeout, $http, ajaxService, appSettings, AppStorage, sessionFactory, exceptionHandler) {

    var service = {
        sourceofregistration: sourceofregistration,
        language: language,
        languagelevel: languagelevel,
        age: age,
        nationalitygroup: nationalitygroup,
        nationality: nationality,
        visatype: visatype,
        visaexpire: visaexpire,
        snsaccount: snsaccount,
        dtp: dtp,
        officetype: officetype,
        webtype: webtype,
        translationtools: translationtools,
        toolname: toolname,
        design: design,
        softwarename: softwarename,
        tin: tin,
        iin: iin,
        nin: nin,
        narrationperformance: narrationperformance
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

    return service;

    function sourceofregistration(data) {
        var url = "advancedStaffSearch/getsourceofregistration";
        return httpPost(url, dataObject);
    }
    function language(data) {
        var url = "advancedStaffSearch/getlanguage";
        return httpPost(url, dataObject);
    }
    function languagelevel(data) {
        var url = "advancedStaffSearch/getlanguagelevel";
        return httpPost(url, dataObject);
    }    
    function age(data) {
        var url = "advancedStaffSearch/getage";
        return httpPost(url, dataObject);
    }
    function nationalitygroup(data) {
        var url = "advancedStaffSearch/getnationalitygroup";
        return httpPost(url, dataObject);
    }
    function nationality(data) {
        var url = "advancedStaffSearch/getnationality";
        return httpPost(url, dataObject);
    }
    function visatype(data) {
        var url = "advancedStaffSearch/getvisaType";
        return httpPost(url, dataObject);
    }
    function visaexpire(data) {
        var url = "advancedStaffSearch/getvisaexpire";
        return httpPost(url, dataObject);
    }
    function snsaccount(data) {
        var url = "advancedStaffSearch/getsnsaccount";
        return httpPost(url, dataObject);
    }
    function dtp(data) {
        var url = "advancedStaffSearch/getdtp";
        return httpPost(url, dataObject);
    }
    function officetype(data) {
        var url = "advancedStaffSearch/getofficetype";
        return httpPost(url, dataObject);
    }
    function webtype(data) {
        var url = "advancedStaffSearch/getwebtype";
        return httpPost(url, dataObject);
    }
    function translationtools(data) {
        var url = "advancedStaffSearch/gettranslationtools";
        return httpPost(url, dataObject);
    }
    function toolname(data) {
        var url = "advancedStaffSearch/gettoolname";
        return httpPost(url, dataObject);
    }
    function design(data) {
        var url = "advancedStaffSearch/getdesign";
        return httpPost(url, dataObject);
    }
    function softwarename(data) {
        var url = "advancedStaffSearch/getsoftwarename";
        return httpPost(url, dataObject);
    }
    function tin(data) {
        var url = "advancedStaffSearch/gettin";
        return httpPost(url, dataObject);
    }
    function iin(data) {
        var url = "advancedStaffSearch/getiin";
        return httpPost(url, dataObject);
    }
    function nin(data) {
        var url = "advancedStaffSearch/getnin";
        return httpPost(url, dataObject);
    }
    function narrationperformance(data) {
        var url = "advancedStaffSearch/getnarrationperformance";
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

}]);