

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Master_CompanyIndustryClassification Entity
 * Date             :   29-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('companyindustryclassification', {

    templateUrl: 'app/Components/CompanyIndustryClassification/Template/compIndClassList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', cicController]
});


function cicController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.cic_obj = { Name: "", Code: "", SortBy: "", IsActive: false };

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
        GetAllCompanyIndustryClassification();
    };

    function GetAllCompanyIndustryClassification() {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "companyindustryclassification/list", onSuccess, onErrorToastMessage);
    }

    var onSuccess = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    };

    $scope.$on("companyIndustryClassificationAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    });

    //$scope.searchText = function () {
    //    var value = $scope.searchkey;
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.displayColl = null;
    //    $scope.displayColl = new Array().concat(data);
    //};

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditCompanyIndustryClassification",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.cic_obj = { Name: "", Code: "", SortBy: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.cic_obj;
                    }
                    else {
                        this.cic_obj = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.cic_obj;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}



// Child component for Add/Edit popup dialog

angular.module("hiworkApp").component('addEditCompanyIndustryClassification', {

    templateUrl: 'app/Components/CompanyIndustryClassification/Template/addEditCompIndClass.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditCompIndClass]
});


function addEditCompIndClass($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.saveCompanyIndustryClassification = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "") {
            return;
        }
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData($ctrl.modalData, "companyindustryclassification/save", n_onSuccess, onErrorToastMessage);
    };

    var n_onSuccess = function (response) {
        $rootScope.$broadcast("companyIndustryClassificationAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}