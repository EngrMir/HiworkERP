
//Done by Tamal
//For parent Department list  list

angular.module("hiworkApp").component('departments', {

    templateUrl: 'app/Components/Department/Template/departmentList.html',
    controller: departmentController
})


function departmentController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.department = {
        Name: "", Code: "", CountryID: "", BranchID: "", Description: "",
        DivisionID: "",
        IsActive: false
    };

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
        GetAllDepartments();
    };

    function GetAllDepartments() {
        var department = {};
        department.CurrentUserID = currentUser.CurrentUserID;
        department.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(department, "department/list", onGetData, toastr.error);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.departmentColl = [].concat($scope.rowCollection);
    };

    $scope.$on("departmentAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.departmentColl = [].concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.departmentColl = [].concat($scope.rowCollection);
    });

    //$scope.searchText = function () {
    //    var value = $scope.searchkey;
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.departmentColl = null;
    //    $scope.departmentColl = [].concat(data);
    //};

    this.open = function (obj, title) {
        var binding = {};
        binding.component = "addEditDepartment";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.department : obj;
        binding.resolve.title = function () {
            return title;
        };
        $uibModal.open(binding);
    };

}



//for Department pop up window

angular.module('hiworkApp').component('addEditDepartment', {

    templateUrl: 'app/Components/Department/Template/addEditDepartment.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: addDepartmentController
});

function addDepartmentController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.GetFormData = function () {
        var model = new Object();
        model.CurrentCulture = currentCulture;
        model.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(model, "department/formdata", onGetFormData, toastr.error);
    };

    var onGetFormData = function (response) {
        $ctrl.countryList = response.countryList;
        $ctrl.branchList = response.branchList;
      //  $ctrl.divisionList = response.divisionList;
       // $ctrl.filterBranches();
    };

    $ctrl.filterBranches = function () {
        var filter = _.filter($ctrl.originalBranch, function (f) {
            return $ctrl.modalData.CountryID == f.Country.Id;
        });
        $ctrl.branchList = filter;
    };


    // Save Function //
    $ctrl.saveDepartment = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "")
            return;
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        if (!$ctrl.modalData.Branch) {
            toastr.error("Please select a branch office.");
            return;
        }
        $ctrl.modalData.Country = $ctrl.modalData.Branch.Country;
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData($ctrl.modalData, "department/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("departmentAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorOnSaving = function (response) {
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('ERRORDBOPERATION'));
    };
    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.branchList = [];
        $ctrl.countryList = [];
        $ctrl.divisionList = [];
        $ctrl.originalBranch = [];
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

