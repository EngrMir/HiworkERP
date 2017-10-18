
//For parent Language list
angular.module("hiworkApp").component('languages', {

    templateUrl: 'app/Components/Language/Template/languageList.html',
    controller: languageController

})


function languageController($scope, $uibModal, appSettings, loginFactory, AppStorage, sessionFactory, $filter, ajaxService,$state) {

    this.language = { Name: "", Code: "", IsActive: false };

    //$scope.branchlist = [];
    $scope.rowCollection = [];
    $scope.LangCollection = [];
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    this.init = function () {
        GetAllLanguages();
    };

    function GetAllLanguages() {
        var language = {};
        language.CurrentUserID = currentUser.CurrentUserID;
        language.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(language, "language/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.LangCollection = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("languageAdded", function (event, response) {
        GetAllLanguages();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllLanguages();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.langcoll = null;
        $scope.langcoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditLanguage",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.language = { Name: "", Code: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.language;
                    }
                    else {
                        this.language = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.language;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for branch pop up window

angular.module('hiworkApp').component('addEditLanguage', {
    templateUrl: 'app/Components/Language/Template/addEditLanguage.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addLanguageController
});

function addLanguageController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);


    $ctrl.save = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "")
            return;
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }
        ajaxService.AjaxPostWithData($ctrl.modalData, "language/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("languageAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving branch');
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

