/**
 * Created by Mahfuz on 07/24/2017.
 */
(function (module) {

    var USERKEY = "utoken";

    var currentUserToken = function (localStorage) {

        var saveUser = function () {
            localStorage.add(USERKEY, profile);
        };

        var removeUser = function () {
            localStorage.remove(USERKEY);
        };

        var initialize = function () {
            var user = {
                username: "",
                token: "",
                get loggedIn() {
                    return this.token ? true : false;
                }
            };

            var localUser = localStorage.get(USERKEY);
            if (localUser) {
                user.username = localUser.username;
                user.token = localUser.token;
            }
            return user;
        }

        var profile = initialize();

        return {
            save: saveUser,
            remove: removeUser,
            profile: profile
        };
    };

    module.factory("currentUserToken", currentUserToken);

}(angular.module("hiworkApp")));