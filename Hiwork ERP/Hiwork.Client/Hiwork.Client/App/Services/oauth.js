/**
 * Created by Mahfuz on 07/24/2017.
 */
(function (module) {

    var oauth = function () {

        var url = "login";

        this.setUrl = function (newUrl) {
            url = newUrl;
        };

        this.$get = function ($http, formEncode, currentUserToken, loginFactory, appSettings) {

            var processToken = function (username) {
                return function (response) {
                    currentUserToken.profile.username = username;
                    currentUserToken.profile.token = response.data.access_token;
                    currentUserToken.save();
                    return username;
                }
            };

            var login = function (username, password, CurrentCulture) {

                var data = formEncode({
                    username: username,
                    password: password,
                    ApplicationId: appSettings.ApplicationId,
                    CurrentCulture: CurrentCulture,
                    grant_type: "password"
                });

                //var data = {
                //    username: username,
                //    password: password,
                //    ApplicationId: appSettings.ApplicationId,
                //    grant_type: "password"
                //};
                return loginFactory.GetLoginData(data, username);

                //return $http.post(url, data, configuration).then(processToken(username));
            };

            //var logout = function () {
            //    currentUser.profile.username = "";
            //    currentUser.profile.token = "";
            //    currentUser.remove();
            //};

            return {
                login: login,
                //logout: logout
            };
        }
    };

    module.config(function ($provide) {
        $provide.provider("oauth", oauth);
    });

}(angular.module("hiworkApp")));