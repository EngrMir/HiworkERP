//For hiworkLanguagePrice list
angular.module("hiworkApp").component('hiworklanguageprices', {
    templateUrl: 'App/Components/HiworkLanguagePrice/Template/hiworkLanguagePriceList.html',
    controller: hiworkLanguagePriceController
});


function hiworkLanguagePriceController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.hiworkLanguagePrice = { SourceLanguageID: "", TargetLanguageID: "", CurrencyId: "", GeneralPrice: "", SpecialPrice: "", PatentPrice: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    //if (!loginFactory.IsAuthenticated())
    //    $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);


    this.init = function () {
        GetAllhiworkLanguagePrices();
    };

    function GetAllhiworkLanguagePrices() {
        var hiworkLanguagePrice = {};
        hiworkLanguagePrice.CurrentUserID = currentUser.CurrentUserID;
        hiworkLanguagePrice.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(hiworkLanguagePrice, "hiworklanguageprice/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("hiworkLanguagePriceAdded", function (event, response) {
        //$scope.rowCollection = response;
        //$scope.displayColl = [].concat($scope.rowCollection);
        GetAllhiworkLanguagePrices();
    });

    $scope.$on("dataDeleted", function (event, response) {
        //$scope.rowCollection = response;
        //$scope.displayColl = [].concat($scope.rowCollection);
        GetAllhiworkLanguagePrices();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditHiworkLanguagePrice",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.hiworkLanguagePrice = { SourceLanguageID: "", TargetLanguageID: "", CurrencyId: "", GeneralPrice: "", SpecialPrice: "", PatentPrice: "", IsActive: false };
                        return this.hiworkLanguagePrice;
                    }
                    else {
                        this.hiworkLanguagePrice = obj;
                        return this.hiworkLanguagePrice;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}
//for hiworkLanguagePrice pop up window  

angular.module('hiworkApp').component('addEditHiworkLanguagePrice', {
    templateUrl: 'App/Components/HiworkLanguagePrice/Template/addEditHiworkLanguagePrice.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addHiworkLanguagePriceController
});

function addHiworkLanguagePriceController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getLanguages = function () {
            var language = {};
            language.CurrentUserID = currentUser.CurrentUserID;
            language.CurrentCulture = currentCulture;
            ajaxService.AjaxPostWithData(language, "language/list", $ctrl.onGetData, $ctrl.onGetError);
        }

    $ctrl.getCurrencies = function () {
        var currency = {};
        currency.CurrentUserID = currentUser.CurrentUserID;
        currency.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(currency, "currency/list", $ctrl.onGetDataCur, $ctrl.onGetError);
    }
    $ctrl.onGetDataCur = function (response) {
        $ctrl.currencyList = [];
        $ctrl.currencyList = response;
    };
    $ctrl.onGetData = function (response) {
        $ctrl.languageList = [];
        $ctrl.languageList = response;
    };
    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.SaveHiworkLanguagePrice = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.SourceLanguageID || !$ctrl.modalData.TargetLanguageID || $ctrl.modalData.SourceLanguageID == "" || $ctrl.modalData.TargetLanguageID == "" || !$ctrl.modalData.GeneralPrice || $ctrl.modalData.GeneralPrice == "" ||
              !$ctrl.modalData.CurrencyId || $ctrl.modalData.CurrencyId == "")
            return;
        ajaxService.AjaxPostWithData($ctrl.modalData, "hiworklanguageprice/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("hiworkLanguagePriceAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

        $ctrl.getLanguages();
        $ctrl.getCurrencies();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

