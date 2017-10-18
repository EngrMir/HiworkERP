
angular.module("hiworkApp").component('designation', {

    templateUrl: 'App/Components/Designation/Template/designationList.html',
    controller: designationController
});


function designationController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.designation = { CountryID: "", BranchID: "", DivisionID: "", DepartmentID: "", TeamID: "", Code: "", Name: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllDesignation();
    };

    function GetAllDesignation() {
        var designation = {};
        designation.CurrentUserID = currentUser.CurrentUserID;
        designation.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(designation, "designation/list", onGetData, toastr.error);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    $scope.$on("designationAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    //$scope.searchText = function () {
    //    var value = angular.element('#search').val();
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.displayColl = null;
    //    $scope.displayColl = [].concat(data);
    //};

    this.open = function (obj, title) {
        var binding = {};
        binding.component = "addEditDesignation";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.designation : obj;
        binding.resolve.title = function () {
            return title;
        };
        $uibModal.open(binding);
    };

}


//for bank Account pop up window

angular.module('hiworkApp').component('addEditDesignation', {

    templateUrl: 'App/Components/Designation/Template/addEditDesignation.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: addDesignationController
});

function addDesignationController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.GetFormData = function () {
        var model = new Object();
        model.CurrentCulture = currentCulture;
        model.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(model, "designation/formdata", onGetFormData, toastr.error);
    };

    var onGetFormData = function (response) {
        $ctrl.countryList = response.countryList;
        $ctrl.branchList = response.branchList;
        $ctrl.divisionList = response.divisionList;
        $ctrl.departmentList = response.departmentList;
        $ctrl.teamList = response.teamList;
    };

    $ctrl.SaveDesignation = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.CountryID || !$ctrl.modalData.BranchID || !$ctrl.modalData.DivisionID || !$ctrl.modalData.DepartmentID || !$ctrl.modalData.TeamID || !$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.CountryID == "" || $ctrl.modalData.BranchID == "" || $ctrl.modalData.DivisionID == "" || $ctrl.modalData.DepartmentID == "" || $ctrl.modalData.TeamID == "" || $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "")
            return;
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData($ctrl.modalData, "designation/save", successOnSaving, toastr.error);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("designationAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.countryList = [];
        $ctrl.branchList = [];
        $ctrl.divisionList = [];
        $ctrl.departmentList = [];
        $ctrl.teamList = [];
        $ctrl.GetFormData();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

