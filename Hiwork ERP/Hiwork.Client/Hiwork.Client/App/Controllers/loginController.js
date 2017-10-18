angular.module("hiworkApp").controller("loginController", function ($scope, $state, Notify, loginFactory, translatorService, AppStorage, sessionFactory, $window, $http, appSettings, exceptionHandler, oauth, currentUserToken, alerting, loginRedirect) {
    $scope.UserName = "";
    $scope.Password = "";
    currentUserToken.profile.token = "";

    //$scope.user = currentUser.profile;

    $scope.user = currentUserToken.profile;

    $scope.languages = sessionFactory.GetSessionObject(AppStorage.cultureData);

    if (!$scope.languages)
        GetCulture();
    else
        Init();

    function GetCulture() {
        $http({
            method: "GET",
            url: appSettings.API_BASE_URL + "culture/list"
        }).then(function mySuccess(response) {
            $scope.languages = response.data;
            sessionFactory.SetSessionObject(AppStorage.cultureData, response.data);
            Init();
        }, function myError(error) {
            //$scope.myWelcome = response.statusText;
            exceptionHandler.handleException(error);

        });
    }
    function Init() {
        var language = sessionFactory.GetData(AppStorage.currentLanguage);
        if (!language || angular.isUndefined(language)) {
            language = 'en';
            translatorService.setCurrentLanguage(language);
        }
        $scope.selectLanguage = language;
    }

    $scope.login = function () {
        $scope.isTriedForLogin = true;
        if ($scope.UserName == "" || $scope.Password == "")
            return;

        var user = createLoginCredentials();
        //$scope.user = currentUser.profile;


        oauth.login(user.UserName, user.Password, sessionFactory.GetData(AppStorage.currentLanguage))
           .then(loginRedirect.redirectPreLogin)
           .catch(alerting.errorHandler("Could not login")
        );
        user.password = "";
    };

    $scope.signOut = function () {
        oauth.logout();
    };


    $scope.$on(Notify.LOGIN_UNSUCCESSFUL, function (event, response) {
        $scope.isInvalidUser = true;
        $scope.loginErrorText = response.Message;

    });
    $scope.$on(Notify.LOGIN_SUCCESSFUL, function (event, response) {
        $state.go("dashboard");
    });
    var createLoginCredentials = function () {

        var params = {
            UserName: $scope.UserName,
            Password: $scope.Password,
            ApplicationId: appSettings.ApplicationId,
            CurrentCulture: sessionFactory.GetData(AppStorage.currentLanguage)
        };

        return params
    };
    $scope.languageChanged = function (selectLanguage) {
        translatorService.setCurrentLanguage(selectLanguage);
    };

});