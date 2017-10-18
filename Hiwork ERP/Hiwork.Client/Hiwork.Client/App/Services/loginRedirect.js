/**
 * Created by Mahfuz on 07/24/2017.
 */
(function (module) {
    var loginRedirect = function () {
        var loginUrl = "login";
        var lastPath = "";
        var lastStatus = "";

        this.setLoginUrl = function (value) {
            loginUrl = value;
        };

        this.$get = function ($q, $injector, $rootScope, currentUserToken, sessionFactory) {
            return {
                responseError: function (response) {
                    if (response.status == 401) {
                        if (lastStatus !== 401)
                            lastPath = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);

                        $injector.get('$state').transitionTo(loginUrl);
                        lastStatus = response.status;

                        sessionFactory.ClearAll();
                        currentUserToken.profile.username = "";
                        currentUserToken.profile.token = "";
                        currentUserToken.remove();
                        //$state.go(loginUrl);
                    }
                    else {
                        lastPath = loginUrl;
                    }
                    return $q.reject(response);
                },

                redirectPreLogin: function (response) {
                    debugger;
                    if (lastPath) {
                        //$state.go(lastPath);
                        $injector.get('$state').transitionTo(lastPath);
                        lastPath = "";
                    } else {
                        if (JSON.parse(localStorage.utoken.valueOf('route')).route != "") {
                            $injector.get('$state').transitionTo(JSON.parse(localStorage.utoken.valueOf('route')).route);
                        }
                        else {
                            $injector.get('$state').transitionTo("dashboard");
                        }
                    }
                }
            };
        };
    };

    module.provider("loginRedirect", loginRedirect);
    module.config(function ($httpProvider) {
        $httpProvider.interceptors.push("loginRedirect");
    });

}(angular.module("hiworkApp")))