
//Done by Tamal
//For parent  Transpro Language Price list

angular.module("hiworkApp").component('translanprice', {

    templateUrl: 'app/Components/TransproLanguagePrice/Template/TransproLanguagePriceList.html',
    controller: translpController
})


function translpController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.tlp = {
        SourceLanguageID: "", TargetLanguageID: "", SpecialityFieldID: "", SubSpecialityFieldID: "", CurrencyID: 0,
        IsLightPrice: true, IsBusinessPrice: true, IsExpertPrice: true,
        IsActive: true, IsDeleted: false, PriceDetailsList: []
    };
    var editobj = null;

    $scope.rowCollection = [];
    $scope.tlpColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.$onInit = function () {
        GetAlltransProLanguage();
    };

    function GetAlltransProLanguage() {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(BaseModel, "transprolpc/list", onGetData, toastr.error);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.tlpColl = [].concat($scope.rowCollection);
    };

    $scope.$on("tlpAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.tlpColl = [].concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.tlpColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        var binding = {};
        binding.component = "addEdittranslp";
        binding.windowClass = "transproprice-dialog";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.tlp : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };
}



//for pop up window

angular.module('hiworkApp').component('addEdittranslp', {

    templateUrl: 'app/Components/TransproLanguagePrice/Template/addEditTransproLanguagePrice.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: addtlp
});

function addtlp($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.DefaultViewID = null;

    $ctrl.GetTransproLanguagePriceViewList = function () {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(BaseModel, "transprolpc/viewlist", $ctrl.onGetViewData, $ctrl.onGetError);
    };

    $ctrl.onGetViewData = function (response) {
        $ctrl.CurrencyList = [];
        $ctrl.CurrencyList = response.CurrencyList;
        $ctrl.LanguageList = [];
        $ctrl.LanguageList = response.LanguageList;
        $ctrl.SpecializedFieldList = [];
        $ctrl.SpecializedFieldList = response.SpecializedFieldList;
        $ctrl.SubSpecializedFieldList = [];
        $ctrl.SubSpecializedFieldList = response.SubSpecializedFieldList;
        $ctrl.DeliveryPlanList = [];
        $ctrl.DeliveryPlanList = response.DeliveryPlanList;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    // Save Function //
    $ctrl.SaveTransproPriceSettings = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.SourceLanguageID || !$ctrl.modalData.TargetLanguageID
            || $ctrl.modalData.SourceLanguageID == "" || $ctrl.modalData.TargetLanguageID == ""
            || !$ctrl.modalData.CurrencyID || $ctrl.modalData.CurrencyID == ""
            || !$ctrl.modalData.SpecialityFieldID || $ctrl.modalData.SpecialityFieldID == ""
            || !$ctrl.modalData.SubSpecialityFieldID || $ctrl.modalData.SubSpecialityFieldID == "") {
            return;
        }

        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        var i;
        for (i = 0; i < $ctrl.modalData.PriceDetailsList.length; i += 1) {
            if ($ctrl.modalData.PriceDetailsList[i].ID == $ctrl.DefaultViewID) {
                $ctrl.modalData.PriceDetailsList[i].IsDefaultForView = true;
            }
            else {
                $ctrl.modalData.PriceDetailsList[i].IsDefaultForView = false;
            }
        }
        ajaxService.AjaxPostWithData($ctrl.modalData, "transprolpc/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("tlpAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorOnSaving = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.GetTransproLanguagePriceViewList();
        var i;
        for (i = 0; i < $ctrl.modalData.PriceDetailsList.length; i += 1) {
            if ($ctrl.modalData.PriceDetailsList[i].IsDefaultForView == true) {
                $ctrl.DefaultViewID = $ctrl.modalData.PriceDetailsList[i].ID;
                break;
            }
        }
    };

    $ctrl.AddNewRow = function () {
        var Details = {
            ID: "", PriceCategoryID: $ctrl.modalData.ID, DeliveryPlanID: "",
            IsMarkedForDelete: false,
            LightPrice: 0, BusinessPrice: 0, ExpertPrice: 0, IsDefaultForView: false, SortBy: 1
        };
        $ctrl.modalData.PriceDetailsList.push(Details);
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

