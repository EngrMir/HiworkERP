//For parent businesscategory list
angular.module("hiworkApp").component('bcategories', {
    templateUrl: 'app/Components/BusinessCategory/Template/businessCategoryList.html',
    controller: businesscategoryController
})

function businesscategoryController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.businesscategory = { Name: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllBusinessCategories();
    };
    function GetAllBusinessCategories() {
        var businesscategory = {};
        businesscategory.CurrentUserID = currentUser.CurrentUserID;
        businesscategory.CurrentCulture = currentCulture;      
        ajaxService.AjaxPostWithData(businesscategory, "businesscategory/list", onGetData, onGetError);
    }
    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };
    var onGetError = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };
    $scope.$on("businesscategoryAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
       
    });
    $scope.$on("dataDeleted", function (event, response) {
      
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    
    this.open = function (obj, title) {        
        $uibModal.open({
            component: "addEditBusinesscategory",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.businesscategory = { Name: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.businesscategory;
                    }
                    else {
                        this.businesscategory = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.businesscategory;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for businesscategory pop up window
angular.module('hiworkApp').component('addEditBusinesscategory', {
    templateUrl: 'app/Components/BusinessCategory/Template/addEditBusinessCategory.html',
        bindings: {        
            modalInstance: "<",
            resolve: "<"
        },   
        controller: addBusinessCategoryController
});

function addBusinessCategoryController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveBusinessCategory = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "businesscategory/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("businesscategoryAdded", response);
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
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}