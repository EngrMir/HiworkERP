angular.module("hiworkApp").factory('UserInfoService', ['$http', 'ajaxService', 'appSettings', 'AppStorage', 'sessionFactory', function ($http, ajaxService, appSettings, AppStorage, sessionFactory) {

    var service = {
        getUser: getUser,
        getAlert: getAlert,
        getUserTypeList: getUserTypeList,
        addUser: addUser,
        getRoles: getRoles,
        getUserType: getUserType,
        checkUser: checkUser
    };

    var obj = '';
    var test = {};
    var requestConfig = {
        headers: {
            'X-ZUMO-APPLICATION': 'GSECUHNQOOrCwgRHFFYLXWiViGnXNV88'
        }
    };
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    test.CurrentUserID = currentUser.CurrentUserID;
    test.CurrentCulture = currentCulture;

    return service;

    function getAlert(msg) {
        console.log("service test usge");
        return { property1: msg };
    };

    function getRoles() {

        return $http({
            url: appSettings.API_BASE_URL + "role/list",
            method: 'POST',
            data: test,
            headers: requestConfig.headers
        }).then(function (response) {

            //appSpinner.hideSpinner();
            console.log('**response from EXECUTE', response);
            return response.data;
        });
    }

    function getUserType() {

        return $http({
            url: appSettings.API_BASE_URL + "userType/get",
            method: 'POST',
            data: test,
            headers: requestConfig.headers
        }).then(function (response) {

            //appSpinner.hideSpinner();
            console.log('**response from EXECUTE', response);
            return response.data;
        });

    }

    function getUser() {
        //ajaxService.AjaxGet("userInfo/list", successFunction, errorFunction);  

        //return ajaxService.AjaxPostWithData(this.obj, "userInfo/list", function (response) {
        //    //test = Object.assign({}, JSON.stringify(response));
        //    console.log("response:::" + JSON.stringify(response));
        //    return reponse;
        //}, function (message) {
        //    obj = message;
        //});

        return $http({
            url: appSettings.API_BASE_URL + "userInfo/list",
            method: 'POST',
            data: test,
            headers: requestConfig.headers
        }).then(function (response) {

            //appSpinner.hideSpinner();
            console.log('**response from EXECUTE', response);
            return response.data;
        });

    };

    function getUserTypeList(successFunction, errorFunction) {
        ajaxService.AjaxPost("user/getUserTypeList", successFunction, errorFunction);
    };

    function addUser(user) {
        // ajaxService.AjaxPost(user, "user/addUser", successFunction, errorFunction);

        return $http({
            url: appSettings.API_BASE_URL + "userInfo/save",
            method: 'POST',
            data: user,
            headers: requestConfig.headers
        }).then(function (response) {

            //appSpinner.hideSpinner();
            console.log('**response from EXECUTE', response);
            return response.data;
        });
    };

    function checkUser(user) {
        // ajaxService.AjaxPost(user, "user/addUser", successFunction, errorFunction);

        return $http({
            url: appSettings.API_BASE_URL + "userInfo/checkUser",
            method: 'POST',
            data: user,
            headers: requestConfig.headers
        }).then(function (response) {

            //appSpinner.hideSpinner();
            console.log('**response from EXECUTE', response);
            return response.data;
        });
    };

}]);