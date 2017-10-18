

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Master_StaffTechnicalSkillCategory Entity
 * Date             :   10-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('techskillcategory', {

    templateUrl: 'app/Components/TechnicalSkillCategory/Template/tscList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', tscController]
});


function tscController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state)
{
    this.category = { Name: "", Code: "", Type: "", SortBy: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false)
    {
        $state.go("login");
    }

    this.$onInit = function () {
        GetAllTechnicalSkillCategories();
    };

    function GetAllTechnicalSkillCategories() {
        let BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "tsc/list", onSuccess, onErrorToastMessage);
    }

    var onSuccess = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    };

    $scope.$on("tscAdded", function (event, response) {
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
        var binding = {};
        binding.component = "addEditTSC";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.category : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };
}



// Child component for Add/Edit popup dialog

angular.module("hiworkApp").component('addEditTSC', {

    templateUrl: 'app/Components/TechnicalSkillCategory/Template/addEditTSC.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditTSC]
});


function addEditTSC($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService)
{
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.saveTSC= function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code || !$ctrl.modalData.Type ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "" || $ctrl.modalData.Type == ""){
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
        ajaxService.AjaxPostWithData($ctrl.modalData, "tsc/save", n_onSuccess, onErrorToastMessage);
    };

    var n_onSuccess = function (response) {
        $rootScope.$broadcast("tscAdded", response);
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