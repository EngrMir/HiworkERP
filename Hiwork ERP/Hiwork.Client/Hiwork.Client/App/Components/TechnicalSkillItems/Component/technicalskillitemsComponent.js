

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Master_StaffTechnicalSkillItems Entity
 * Date             :   10-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('techskillitems', {

    templateUrl: 'app/Components/TechnicalSkillItems/Template/tsiList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', tsiController]
});


function tsiController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.item = { Name: "", Description: "", TechnicalSkillCategoryId: "", SortBy: "", IsActive: false };

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
        GetAllTechnicalSkillItems();
    };

    function GetAllTechnicalSkillItems() {
        let BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "tsi/list", onSuccess, onErrorToastMessage);
    }

    var onSuccess = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    };

    $scope.$on("tsiAdded", function (event, response) {
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
        binding.component = "addEditTSI";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.item : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };
}



// Child component for Add/Edit popup dialog

angular.module("hiworkApp").component('addEditTSI', {

    templateUrl: 'app/Components/TechnicalSkillItems/Template/addEditTSI.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditTSI]
});


function addEditTSI($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);



    /*************************************************************************************************************/
    /************************************    LOADING OF TECHNICALSKILLITEMS   ************************************/
    /*************************************************************************************************************/

    $ctrl.loadTSCList = function () {
        $ctrl.enableSave = false;
        let BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "tsc/list", onGetTSC, onErrorToastMessage);
    };

    var onGetTSC = function (response) {
        $ctrl.originalTSC = new Array();
        $ctrl.originalTSC = response;
        $ctrl.filterTSClist();
        $ctrl.enableSave = true;
    };

    $ctrl.filterTSClist = function () {
        $ctrl.tscList = [];
        var logic = function (arg_tsc) {
            if (arg_tsc.IsActive == false) {
                if ($ctrl.modalData.TechnicalSkillCategoryId == arg_tsc.Id) {
                    $ctrl.modalData.TechnicalSkillCategoryId = null;
                }
            }
            return arg_tsc.IsActive;
        };
        $ctrl.tscList = _.filter($ctrl.originalTSC, logic);
    };



    /*************************************************************************************************************/
    /*********************************    HANDLING SAVE OF TECHNICALSKILLITEMS    ********************************/
    /*************************************************************************************************************/

    $ctrl.saveTSI = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "") {
            return;
        }
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if (!$ctrl.modalData.TechnicalSkillCategoryId || $ctrl.modalData.TechnicalSkillCategoryId == "")
        {
            return;
        }

        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData($ctrl.modalData, "tsi/save", n_onSuccess, onErrorToastMessage);
    };

    var n_onSuccess = function (response) {
        $rootScope.$broadcast("tsiAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.loadTSCList();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}