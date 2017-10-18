
//For parent country list
angular.module("hiworkApp").component('country', {
    templateUrl: 'app/Components/Country/Template/countryList.html',
    controller: countryController
})


function countryController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.country = { Name: "", CurrencyID: 0, IsTrading: "", Code: "", IsActive: false };
   

    $scope.rowCollection = [];
    $scope.countrycoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }
    this.init = function () {
        GetAllCountries();
    };

    function GetAllCountries() {
        var country = {};
        country.CurrentUserID = currentUser.CurrentUserID;
        country.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(country, "country/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.countrycoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("countryAdded", function (event, response) {
        GetAllCountries();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllCountries();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.countrycoll = null;
        $scope.countrycoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditCountry",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.country = { Name: "", CurrencyID: 0, IsTrading: "", Code: "", SortBy: "", IsActive: false };
                        return this.country;
                    }
                    else {
                        this.country = obj;
                        return this.country;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for country pop up window
angular.module('hiworkApp').component('addEditCountry', {
    templateUrl: 'app/Components/Country/Template/addEditCountry.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addCountryController
});

function addCountryController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getCurrency = function () {
        var Currency = {};
        Currency.CurrentUserID = currentUser.CurrentUserID;
        Currency.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(Currency, "currency/list", $ctrl.onGetData, $ctrl.onGetError);
    };

    $ctrl.onGetData = function (response) {
        $ctrl.curList = [];
        $ctrl.curList = response;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting Records');
    };


    $ctrl.save = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code || 
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "" )
            return;
        //if ($ctrl.modalData.Name.length > 100) {
        //    toastr.error($filter('translator')('ERRORLENGTHNAME'));
        //    return;
        //}
        //if ($ctrl.modalData.Code.length > 5) {
        //    toastr.error($filter('translator')('ERRORLENGTH'));
        //    return;
        //}

        if (!$ctrl.modalData.CurrencyID || $ctrl.modalData.CurrencyID == "")
            return;


        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

        ajaxService.AjaxPostWithData($ctrl.modalData, "country/save", successOnSaving, errorOnSaving);
       
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("countryAdded", response);
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

        $ctrl.getCurrency();
    }


    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}




