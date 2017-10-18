angular.module("hiworkApp").component('staffknowledgefielditems', {
    templateUrl: 'app/Components/StaffKnowledgeFieldItem/Template/staffKnowledgeFieldItemList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', staffKnowledgeFieldItemController]
});


function staffKnowledgeFieldItemController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.staffKnowledgeFieldItem = { Name: "", Description: "", KnowledgeFieldID: "", SortBy: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.$onInit = function () {
        GetAllStaffKnowledgeFieldItem();
    };

    function GetAllStaffKnowledgeFieldItem() {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "staffknowledgefielditem/list", onGetData, onErrorToastMessage);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    $scope.$on("staffknowledgefielditemAdded", function (event, response) {
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
        binding.component = "addEditStaffKnowledgeFieldItem";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.staffKnowledgeFieldItem : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };

}


// for pop up window

angular.module('hiworkApp').component('addEditStaffKnowledgeFieldItem', {

    templateUrl: 'app/Components/StaffKnowledgeFieldItem/Template/addEditStaffKnowledgeFieldItem.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditStaffKnowledgeFieldItemController]
});


function addEditStaffKnowledgeFieldItemController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.GetStaffKnowledgeFieldItemList = function () {
        $ctrl.enableSave = false;
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "staffknowledgefield/list", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        $ctrl.originalClassifications = [];
        $ctrl.originalClassifications = response;
        $ctrl.filterStaffKnowledgeFieldList();
        $ctrl.enableSave = true;
    };

    $ctrl.filterStaffKnowledgeFieldList = function () {
        $ctrl.fieldList = [];
        var logic = function (arg_cic) {
            if (arg_cic.IsActive == false) {
                if ($ctrl.modalData.KnowledgeFieldID == arg_cic.Id) {
                    $ctrl.modalData.KnowledgeFieldID = null;
                }
            }
            return arg_cic.IsActive;
        };
        $ctrl.fieldList = _.filter($ctrl.originalClassifications, logic);
    };

    $ctrl.SaveStaffKnowledgeFieldItem = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if (!$ctrl.modalData.KnowledgeFieldID || $ctrl.modalData.KnowledgeFieldID == "")
            return;

        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData($ctrl.modalData, "staffknowledgefielditem/save", successOnSaving, onErrorToastMessage);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("staffknowledgefielditemAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.GetStaffKnowledgeFieldItemList();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

