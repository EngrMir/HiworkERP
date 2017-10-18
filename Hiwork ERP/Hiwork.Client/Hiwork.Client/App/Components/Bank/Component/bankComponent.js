//For parent bank list
angular.module("hiworkApp").component('banks', {
    templateUrl: 'App/Components/Bank/Template/bankList.html',
    controller: bankController
});


function bankController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.bank = { Name: "", Code: "", CountryId: "", CurrencyId: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);


    this.init = function () {
        GetAllBanks();
    };

    function GetAllBanks() {
        var bank = {};
        bank.CurrentUserID = currentUser.CurrentUserID;
        bank.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bank, "bank/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("bankAdded", function (event, response) {
        //$scope.rowCollection = response;
        //$scope.displayColl = [].concat($scope.rowCollection);
        GetAllBanks();
    });

    $scope.$on("dataDeleted", function (event, response) {
        //$scope.rowCollection = response;
        //$scope.displayColl = [].concat($scope.rowCollection);
        GetAllBanks();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditBank",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.bank = { Name: "", Code: "", CountryId: "", CurrencyId: "", IsActive: false };
                        return this.bank;
                    }
                    else {
                        this.bank = obj;
                        return this.bank;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for bank pop up window

angular.module('hiworkApp').component('addEditBank', {
    templateUrl: 'App/Components/Bank/Template/addEditBank.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addBankController
});

function addBankController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getCountries = function () {
        var country = {};
        country.CurrentUserID = currentUser.CurrentUserID;
        country.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(country, "country/list", $ctrl.onGetData, $ctrl.onGetError);
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
        $ctrl.countryList = [];
        $ctrl.countryList = response;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.Savebank = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "" ||
            !$ctrl.modalData.CountryId || $ctrl.modalData.CountryId == "" ||
            !$ctrl.modalData.CurrencyId || $ctrl.modalData.CurrencyId == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "bank/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("bankAdded", response);
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

        $ctrl.getCountries();
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

