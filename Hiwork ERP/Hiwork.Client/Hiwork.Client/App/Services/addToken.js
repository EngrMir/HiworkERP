/**
 * Created by Mahfuz on 07/24/2017.
 */
(function (module) {

    var addToken = function (currentUserToken, $q) {

        return {
            request: function (config) {

                //currentUserToken.profile.token = "";
                if (currentUserToken.profile.token) {
                    config.headers.Authorization = "Bearer " + currentUserToken.profile.token;
                }
                return $q.when(config);
            }
        };
    };

    module.factory("addToken", addToken);
    module.config(function ($httpProvider) {
        $httpProvider.interceptors.push("addToken");
    });

})(angular.module("hiworkApp"));