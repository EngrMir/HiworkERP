
//Done by Tamal
//For parent master Company Trading Category

angular.module("hiworkApp").component('mctradingcategory', {

    templateUrl: 'app/Components/CompanyTradingCategory/Template/CompanyTradingCategoryList.html',
    controller: mctradingcategoryController

})

function mctradingcategoryController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.mastercompanytradingcategory = { Name: "", Code: "",SortBy:"", IsActive: false };


    $scope.rowCollection = [];
    $scope.masterctcColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    //if (loginFactory.IsAuthenticated() == false) {
    //    $state.go("login");
    //}

    this.init = function () {
        GetAllMCTCList();
    };

    function GetAllMCTCList() {
        var mastercompanytradingcategory = {};
        mastercompanytradingcategory.CurrentUserID = currentUser.CurrentUserID;
        mastercompanytradingcategory.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(mastercompanytradingcategory, "mcompanytradcatagory/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.masterctcColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("mastercompanytradcatagoryAdded", function (event, response) {
        GetAllMCTCList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMCTCList();
    });

    //$scope.searchText = function () {
    //    var value = angular.element('#search').val();
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.masterctcColl = null;
    //    $scope.masterctcColl = [].concat(data);
    //};

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addCompanyTCategory",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.mastercompanytradingcategory = { Name: "", Code: "", SortBy: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.mastercompanytradingcategory;
                    }
                    else {
                        this.mastercompanytradingcategory = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.mastercompanytradingcategory;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for Company Trading Category Add Edit pop up window

angular.module('hiworkApp').component('addCompanyTCategory', {
    templateUrl: 'app/Components/CompanyTradingCategory/Template/addEditCompanyTradingCategory.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterestimationCTCController
});

function addmasterestimationCTCController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);



    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveMCompanyTC = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code || !$ctrl.modalData.SortBy ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "" || $ctrl.modalData.SortBy == "") {
            return;
        }



        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        ajaxService.AjaxPostWithData($ctrl.modalData, "mcompanytradcatagory/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("mastercompanytradcatagoryAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {

        toastr.error('Error in saving masterestimation Company Trading Category ');
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

