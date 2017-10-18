

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Master_CompanyIndustryClassificationItem Entity
 * Date             :   30-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('companyindustryclassificationitem', {

    templateUrl: 'app/Components/CompanyIndustryClassificationItem/Template/ciciList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', ciciController]
});


function ciciController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.cici = { Name: "", Description: "", IndustryClassificationId: "", SortBy: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    //if (loginFactory.IsAuthenticated() == false) {
    //    $state.go("login");
    //}

    this.$onInit = function () {
        GetAllCompanyIndustryClassificationItem();
    };

    function GetAllCompanyIndustryClassificationItem() {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "companyindustryclassificationitem/list", onGetData, onErrorToastMessage);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    $scope.$on("companyIndustryClassificationItemAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    //$scope.searchText = function () {
    //    //var value = angular.element('#search').val();
    //    var value = $scope.searchkey;
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.displayColl = null;
    //    $scope.displayColl = [].concat(data);
    //};

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditCompanyIndustryClassificationItem",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.cici = { Name: "", Description: "", IndustryClassificationId: "", SortBy: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.cici;
                    }
                    else {
                        this.cici = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.cici;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}


// for pop up window

angular.module('hiworkApp').component('addEditCompanyIndustryClassificationItem', {

    templateUrl: 'app/Components/CompanyIndustryClassificationItem/Template/addEditCici.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditCiciController]
});


function addEditCiciController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.GetCompanyIndustryClassificationList = function () {
        $ctrl.enableSave = false;
        let BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "companyindustryclassification/list", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        $ctrl.originalClassifications = [];
        $ctrl.originalClassifications = response;
        $ctrl.filterClassificationList();
        $ctrl.enableSave = true;
    };

    $ctrl.filterClassificationList = function () {
        $ctrl.cicList = [];
        var logic = function (arg_cic) {
            if (arg_cic.IsActive == false) {
                if ($ctrl.modalData.IndustryClassificationId == arg_cic.Id) {
                    $ctrl.modalData.IndustryClassificationId = null;
                }
            }
            return arg_cic.IsActive;
        };
        $ctrl.cicList = _.filter($ctrl.originalClassifications, logic);
    };

    $ctrl.SaveCompanyIndustryClassificationItem = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if (!$ctrl.modalData.IndustryClassificationId || $ctrl.modalData.IndustryClassificationId == "")
            return;

        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData($ctrl.modalData, "companyindustryclassificationitem/save", successOnSaving, onErrorToastMessage);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("companyIndustryClassificationItemAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.GetCompanyIndustryClassificationList();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

