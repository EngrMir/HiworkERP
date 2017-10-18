angular.module("hiworkApp").component('bcategorydetails', {
    templateUrl: 'app/Components/BusinessCategoryDetails/Template/businessCategoryDetailsList.html',
    controller: businesscategoryController
})

function businesscategoryController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.businesscategorydetails = { BusinessCategoryId: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllBusinessCategoryDetails();
    };
    function GetAllBusinessCategoryDetails() {
        var businesscategorydetails = {};
        businesscategorydetails.CurrentUserID = currentUser.CurrentUserID;
        businesscategorydetails.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(businesscategorydetails, "businesscategorydetails/list", onGetData, onGetError);
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
    $scope.$on("businesscategorydetailsAdded", function (event, response) {
        GetAllBusinessCategoryDetails();
    });
    $scope.$on("dataDeleted", function (event, response) {
        GetAllBusinessCategoryDetails();
    });

   
    this.open = function (obj, title) {        
        $uibModal.open({
            component: "addEditBusinesscategorydetails",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.businesscategorydetails = { BusinessCategoryId: "", Description: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.businesscategorydetails;
                    }
                    else {
                        this.businesscategorydetails = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.businesscategorydetails;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

angular.module('hiworkApp').component('addEditBusinesscategorydetails', {
    templateUrl: 'app/Components/BusinessCategoryDetails/Template/addEditBusinessCategoryDetails.html',
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
    $ctrl.getBusinessCategories = function () {
        var businesscategory = {};
        businesscategory.CurrentUserID = currentUser.CurrentUserID;
        businesscategory.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(businesscategory, "businesscategory/list", $ctrl.onGetData, $ctrl.onGetError);
    }

    $ctrl.onGetData = function (response) {
        $ctrl.businesscategoryList = [];
        $ctrl.businesscategoryList = response;
    };
    $ctrl.saveBusinessCategoryDetails = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Description || $ctrl.modalData.Description == "" || !$ctrl.modalData.BusinessCategoryId || $ctrl.modalData.BusinessCategoryId == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "businesscategorydetails/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("businesscategorydetailsAdded", response);
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
        $ctrl.getBusinessCategories();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}