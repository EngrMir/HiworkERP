

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Master_StaffDevelopmentSkillItem Entity
 * Date             :   19-July-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('staffdevskillitem', {

    templateUrl: 'app/Components/StaffDevelopmentSkillItem/Template/staffDevSkillItemList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', stafDevSkillItemController]
});


function stafDevSkillItemController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.stafdevskillitem = { Name: "", Description: "", IndustryClassificationId: "", SortBy: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.$onInit = function () {
        if (loginFactory.IsAuthenticated() == false) {
            $state.go("login");
        }
        GetAllStaffDevelopmentSkillItem();
    };

    function GetAllStaffDevelopmentSkillItem() {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "staffdevelopmentskillitem/list", onGetData, onErrorToastMessage);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    $scope.$on("staffDevSkillItemAdded", function (event, response) {
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
        var binding = {};
        binding.component = "addEditStaffDevSkillItem";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.stafdevskillitem : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };

}


// for pop up window

angular.module('hiworkApp').component('addEditStaffDevSkillItem', {

    templateUrl: 'app/Components/StaffDevelopmentSkillItem/Template/addEditStaffDevSkillItem.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditStaffDevSkillItemController]
});


function addEditStaffDevSkillItemController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.GetStaffDevelopmentSkillList = function () {
        $ctrl.enableSave = false;
        let BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "staffdevelopmentskill/list", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        $ctrl.originalStaffDevSkillList = [];
        $ctrl.originalStaffDevSkillList = response;
        $ctrl.filterStaffDevSkillList();
        $ctrl.enableSave = true;
    };

    $ctrl.filterStaffDevSkillList = function () {
        $ctrl.staffDevSkillList = [];
        var logic = function (arg) {
            if (arg.IsActive == false) {
                if ($ctrl.modalData.DevelopmentSkillID == arg.ID) {
                    $ctrl.modalData.DevelopmentSkillID = null;
                }
            }
            return arg.IsActive;
        };
        $ctrl.staffDevSkillList = _.filter($ctrl.originalStaffDevSkillList, logic);
    };

    $ctrl.eee = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if (!$ctrl.modalData.DevelopmentSkillID || $ctrl.modalData.DevelopmentSkillID == "")
            return;

        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData($ctrl.modalData, "staffdevelopmentskillitem/save", successOnSaving, onErrorToastMessage);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("staffDevSkillItemAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.GetStaffDevelopmentSkillList();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

