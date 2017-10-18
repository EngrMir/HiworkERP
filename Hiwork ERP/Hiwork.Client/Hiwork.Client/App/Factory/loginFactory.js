/**
 * Created by Tomas on 28-Jun-16.
 */
angular.module("hiworkApp").factory("loginFactory", function ($location, $http, sessionFactory, loginService, Notify, $rootScope, AppStorage, appSettings, currentUserToken) {
    var userData = null;
    var isAuthenticated = false;

    var configuration = {
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        }
    };

    var processToken = function (username) {
        return function (response) {
            userData = response;
            
           
            //delete userData.data.access_token;
            //$window.sessionStorage.setItem(route, userData.data.Route);
            sessionFactory.SetObject(AppStorage.userData, userData.data);

            $rootScope.$broadcast(Notify.LOGIN_SUCCESSFUL);

            currentUserToken.profile.username = username;
            currentUserToken.profile.token = response.data.access_token;
            currentUserToken.profile.route = userData.data.Route;
            currentUserToken.save();
            return username;
        }
    };

    var onGetLoginData = function (response) {
        userData = response;        
        sessionFactory.SetObject(AppStorage.userData, userData.data);
        $rootScope.$broadcast(Notify.LOGIN_SUCCESSFUL);
    };

    var loginError = function (message) {
        //Error handling goes here.
        $rootScope.$broadcast(Notify.LOGIN_UNSUCCESSFUL, message);
    };

    return {
        GetLoginData: function (data, username) {
            return $http({
                url: appSettings.API_BASE_URL + "login",
                method: 'POST',
                data: data,
                headers: configuration.headers
            }).then( processToken(username), loginError);

            //loginService.getLogin(user, onGetLoginData, loginError);
            // loginService.getUserTypeList(user,onGetLoginData, loginError);
        },
        UserData: function () {
            return userData;
        },
        IsAuthenticated: function () {
            var data = sessionFactory.GetObject(AppStorage.userData);
            if (data)
                isAuthenticated = true;
            return isAuthenticated;
        },
        CurrentLanguage: function () {
            return currentLanguage;
        }
    };
});