//For Unit Price list
//Done by tamal
angular.module("hiworkApp").component('unitprices', {
    templateUrl: 'App/Components/UnitPrice/Template/UnitPriceList.html',
    controller: unitpricecontroller
});


function unitpricecontroller($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.unitPrice = { UnitID: "", EstimationTypeID: "", SourceLanguageID: "", TargetLanguageID: "", CurrencyID: "", GeneralUnitPrice: "", SpecialUnitPrice: "", PatentUnitPrice: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.unitColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    //if (!loginFactory.IsAuthenticated())
    //    $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);


    this.init = function () {
        GetAllUnitPrices();
    };

    function GetAllUnitPrices() {
        var UnitPrice = {};
        UnitPrice.CurrentUserID = currentUser.CurrentUserID;
        UnitPrice.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(UnitPrice, "UnitPrice/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.unitColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("UnitPriceAdded", function (event, response) {
        GetAllUnitPrices();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllUnitPrices();
    });



    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditUnitPrice",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.unitPrice = { UnitID: "", EstimationTypeID: "", SourceLanguageID: "", TargetLanguageID: "", CurrencyID: "", GeneralUnitPrice: "", SpecialUnitPrice: "", PatentUnitPrice: "", IsActive: false };
                        return this.unitPrice;
                    }
                    else {
                        this.unitPrice = obj;
                        return this.unitPrice;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}
//for UnitPrice pop up window  

angular.module('hiworkApp').component('addEditUnitPrice', {
    templateUrl: 'App/Components/UnitPrice/Template/addEditUnitPrice.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addUnitPriceController
});

function addUnitPriceController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getLanguage = function () {
        var language = {};
        language.CurrentUserID = currentUser.CurrentUserID;
        language.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(language, "language/list", $ctrl.onGetData, $ctrl.onGetError);
    }
    $ctrl.getCurrency = function () {
        var currency = {};
        currency.CurrentUserID = currentUser.CurrentUserID;
        currency.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(currency, "currency/list", $ctrl.onGetDataCur, $ctrl.onGetError);
    }
    $ctrl.getServicetype = function () {
        var service = {};
        service.CurrentUserID = currentUser.CurrentUserID;
        service.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(service, "estimationType/list", $ctrl.onGetDataEst, $ctrl.onGetError);
    }
    $ctrl.getUnit = function () {
        var unit = {};
        unit.CurrentUserID = currentUser.CurrentUserID;
        unit.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(unit, "unit/list", $ctrl.onGetDataUn, $ctrl.onGetError);
    }

    $ctrl.onGetDataEst = function (response) {
        $ctrl.estimationList = [];
        $ctrl.estimationList = response;
    };
    $ctrl.onGetDataUn = function (response) {
        $ctrl.unitList = [];
        $ctrl.unitList = response;
    };
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

    $ctrl.SaveUnitPrice = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.UnitID || !$ctrl.modalData.UnitID
            || $ctrl.modalData.EstimationTypeID == "" || $ctrl.modalData.EstimationTypeID == ""
            || !$ctrl.modalData.SourceLanguageID || $ctrl.modalData.SourceLanguageID == ""
            || !$ctrl.modalData.TargetLanguageID || $ctrl.modalData.TargetLanguageID == ""
            || !$ctrl.modalData.CurrencyID || $ctrl.modalData.CurrencyID == ""
            || !$ctrl.modalData.GeneralUnitPrice || $ctrl.modalData.GeneralUnitPrice == ""
            || !$ctrl.modalData.SpecialUnitPrice || $ctrl.modalData.SpecialUnitPrice == ""
            || !$ctrl.modalData.PatentUnitPrice || $ctrl.modalData.PatentUnitPrice == ""
           
            )
    {
          return;
        }
       if ($ctrl.modalData.SourceLanguageID == $ctrl.modalData.TargetLanguageID)
       {
           toastr.error($filter('translator')('SOURCELANGUAGEIDANDTARGETLANGUAGEID'));
            return ;
        }
     
        ajaxService.AjaxPostWithData($ctrl.modalData, "UnitPrice/save", successOnSaving, errorOnSaving);
    }
    //ajaxService.AjaxPostWithData($ctrl.modalData, "UnitPrice/matchin", successOnSaving, errorOnSaving);
    var successOnSaving = function (response) {
        $rootScope.$broadcast("UnitPriceAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error("This selected items already exist ");
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

        $ctrl.getLanguage();
        $ctrl.getCurrency();
        $ctrl.getUnit();
        $ctrl.getServicetype();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

