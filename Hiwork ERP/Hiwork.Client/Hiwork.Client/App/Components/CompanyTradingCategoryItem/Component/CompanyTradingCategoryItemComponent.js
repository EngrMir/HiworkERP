
//Done by Tamal
//For parent master Company Trading Category

angular.module("hiworkApp").component('mctradingcategoryitem', {

    templateUrl: 'app/Components/CompanyTradingCategoryItem/Template/CompanyTradingCategoryItemList.html',
    controller: mctradingcategoryitemController

})

function mctradingcategoryitemController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.mastercompanytradingcategoryitem = { Name: "", TradingCategoryID: "", Description: "", SortBy: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.masterctcitemColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    //if (loginFactory.IsAuthenticated() == false) {
    //    $state.go("login");
    //}

    this.init = function () {
        GetAllMCTCitemList();
    };

    function GetAllMCTCitemList() {
        var mastercompanytradingItemcategory = {};
        mastercompanytradingItemcategory.CurrentUserID = currentUser.CurrentUserID;
        mastercompanytradingItemcategory.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(mastercompanytradingItemcategory, "mcompanytradcategoryitem/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.masterctcitemColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("mastercompanytradcategoryItemAdded", function (event, response) {
        GetAllMCTCitemList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMCTCitemList();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.masterctcitemColl = null;
        $scope.masterctcitemColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addCompanyTCategoryItem",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.mastercompanytradingcategoryitem = { Name: "", TradingCategoryID: "", Description: "", SortBy: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.mastercompanytradingcategoryitem;
                    }
                    else {
                        this.mastercompanytradingcategoryitem = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.mastercompanytradingcategoryitem;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for Company Trading Category Item Add Edit pop up window

angular.module('hiworkApp').component('addCompanyTCategoryItem', {
    templateUrl: 'app/Components/CompanyTradingCategoryItem/Template/addEditCompanyTradingCategoryItem.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterestimationCTCitemController
});

function addmasterestimationCTCitemController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);


    $ctrl.getCompanyTradingCategory = function () {
        var CTC = {};
        CTC.CurrentUserID = currentUser.CurrentUserID;
        CTC.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(CTC, "mcompanytradcatagory/list", $ctrl.onGetData, $ctrl.onGetError);

    }
    $ctrl.onGetData = function (response) {
        $ctrl.ctcList = [];
        $ctrl.ctcList = response;
    };


    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveMCompanyTCList = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description || !$ctrl.modalData.SortBy ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "" || $ctrl.modalData.SortBy == "") {
            {
                return;
            }
            if (!$ctrl.modalData.TradingCategoryID || $ctrl.modalData.TradingCategoryID == "")
                return;
        }
        ajaxService.AjaxPostWithData($ctrl.modalData, "mcompanytradcategoryitem/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("mastercompanytradcategoryItemAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {

        toastr.error('Error in saving masterestimation Company Trading Category item ');
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

        $ctrl.getCompanyTradingCategory();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

