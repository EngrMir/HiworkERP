

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Company_Team Entity
 * Date             :   Unknown
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('teams', {

    templateUrl: 'app/Components/Team/Template/teamList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', teamController]
});


function teamController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.team = {
        Name: "", Code: "", CountryId: "", BranchId: "", Description: "",
        DivisionId: "", DepartmentId: "",
        IsActive: false
    };

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
        GetAllTeams();
    };

    function GetAllTeams() {
        let BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "team/list", onGetData, onErrorToastMessage);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    $scope.$on("teamAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    //$scope.searchText = function () {
    //    var value = $scope.searchkey;
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.displayColl = null;
    //    $scope.displayColl = [].concat(data);
    //}

    this.open = function (obj, title) {
        var binding = {};
        binding.component = "addEditTeam";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.team : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };
}


//for pop up window

angular.module('hiworkApp').component('addEditTeam', {

    templateUrl: 'app/Components/Team/Template/addEditTeam.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addTeamController]
});



function addTeamController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);



    /*************************************************************************************************************/
    /*****************************************    LOADING OF FORM DATA   *****************************************/
    /*************************************************************************************************************/

    $ctrl.GetFormData = function () {
        $ctrl.enableSave = false;
        let BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "team/formdata", onGetFormData, onErrorToastMessage);
    };

    var onGetFormData = function (response) {
        $ctrl.originalDivision = response.divisionList;
        $ctrl.originalCountry = response.countryList;
        $ctrl.originalBranch = response.branchList;
        $ctrl.originalDepartment = response.departmentList;
        $ctrl.filterDivisions();
        $ctrl.filterCountries();
        $ctrl.enableSave = true;
    };

    $ctrl.filterDivisions = function () {
        $ctrl.divisionList = [];
        let logic = function (arg_division) {
            if (arg_division.IsActive == false) {
                if ($ctrl.modalData.DivisionId == arg_division.Id) {
                    $ctrl.modalData.DivisionId = null;
                }
            }
            return arg_division.IsActive;
        };
        $ctrl.divisionList = _.filter($ctrl.originalDivision, logic);
    };

    $ctrl.filterCountries = function () {
        $ctrl.countryList = [];
        let logic = function (arg_country) {
            if (arg_country.IsActive == false) {
                if ($ctrl.modalData.CountryId == arg_country.Id) {
                    $ctrl.modalData.CountryId = null;
                }
            }
            return arg_country.IsActive;
        };
        $ctrl.countryList = _.filter($ctrl.originalCountry, logic);
        $ctrl.filterBranches();
    };
    
    $ctrl.filterBranches = function () {
        $ctrl.branchList = [];
        let discovery;
        let discovery_a, discovery_b;

        // programming logic for to decide which objects we choose for out list
        let logic = function (arg_branch) {
            discovery_a = $ctrl.modalData.CountryId == arg_branch.Country.Id ? true : false;
            discovery_b = arg_branch.IsActive;

            // taking ultimate decision
            discovery = discovery_a && discovery_b;
            if (discovery == false) {
                if ($ctrl.modalData.BranchId == arg_branch.ID) {
                    $ctrl.modalData.BranchId = null;
                }
            }
            return (discovery);
        };
        $ctrl.branchList = _.filter($ctrl.originalBranch, logic);
        $ctrl.filterDepartments();
    };

    $ctrl.filterDepartments = function () {
        $ctrl.departmentList = [];
        let discovery;
        let discovery_a, discovery_b;

        let logic = function (arg_dept) {
            discovery_a = $ctrl.modalData.BranchId == arg_dept.BranchID ? true : false;
            discovery_b = arg_dept.IsActive;
            discovery = discovery_a && discovery_b;
            if (discovery == false) {
                if ($ctrl.modalData.DepartmentId == arg_dept.ID) {
                    $ctrl.modalData.DepartmentId = null;
                }
            }
            return (discovery);
        };
        $ctrl.departmentList = _.filter($ctrl.originalDepartment, logic);
    };



    /*************************************************************************************************************/
    /*******************************************    STORAGE HANDLING   *******************************************/
    /*************************************************************************************************************/

    $ctrl.saveTeam = function () {
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

        if (!$ctrl.modalData.CountryId || !$ctrl.modalData.BranchId ||
            $ctrl.modalData.CountryId == "" || $ctrl.modalData.BranchId == "" ||
            !$ctrl.modalData.DivisionId || !$ctrl.modalData.DepartmentId ||
            $ctrl.modalData.DivisionId == "" || $ctrl.modalData.DepartmentId == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "team/save", successOnSaving, onErrorToastMessage);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("teamAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };



    /*************************************************************************************************************/
    /*********************************************    MISCELLANEOUS   ********************************************/
    /*************************************************************************************************************/

    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        $ctrl.divisionList = [];
        $ctrl.countryList = [];
        $ctrl.branchList = [];
        $ctrl.departmentList = [];
        $ctrl.originalDivision = [];
        $ctrl.originalCountry = [];
        $ctrl.originalBranch = [];
        $ctrl.originalDepartment = [];
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

