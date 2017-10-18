
angular.module("hiworkApp").component('employeregistration', {
    templateUrl: 'App/Components/EmployeRegistration/Template/EmployeRegistration.html',
    controller: empolyeRegistrationController
});

empolyeRegistrationController.$inject = ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$state', '$filter', 'ajaxService', 'fileUploadService', '$stateParams']
function empolyeRegistrationController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $state, $filter, ajaxService, fileUploadService, $stateParams) {

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    var BaseModel = {}
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    $scope.IsSubmitEnable = true;
    $scope.employeeIDstatus = '';
    $scope.imageUploaded = false;
    $scope.myImage = null;
    $scope.vm = {};
    var tempBankBranchList = [];
    var tempBankList = [];


    this.$onInit = function () {
        if (loginFactory.IsAuthenticated() == false) {
            $state.go("login");
        }
        //  getAllEmployee();
        getFormData();

        if ($stateParams.ID !== null) {

            BaseModel.ID = $stateParams.ID;
            GetEmployeeByID();
        }
    };

    


    function getFormData() {
        var BaseModel = {}
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "employee/formData", onGetDataForm, onGetError);
    }

    var onGetDataForm = function (response) {
        $scope.CountryList = [];
        //  $scope.DivisionList = [];
        $scope.DesignationList = [];
        $scope.BranchList = [];
        $scope.DeptList = [];
        $scope.BankList = [];
        $scope.CountryList = response.countries;
        $scope.DivisionList = response.divisions;
        $scope.EmployeeTypeList = response.roles;
        $scope.BranchList = response.branches;
        $scope.DeptList = response.departments;
        $scope.BankList = response.banks;
        tempBankList = response.banks;
        $scope.BankBranchList = [];
        $scope.BankBranchList = response.bankbranches;
        tempBankBranchList = response.bankbranches;
        $scope.BankAccTypeList = [];
        $scope.BankAccTypeList = response.bankAccountTypes;
        $scope.LanguageList = [];
        $scope.LanguageList = response.languages;
    };

    $scope.getBankList = function () {

        var key = $scope.vm.CountryID;
        var filters = _.filter(tempBankList, function (f) {
            return f.CountryId == key;
        });
        //var newList = $filter("filter")(tempBankList, { CountryId: key });
        $scope.BankList = [];
        $scope.BankList = filters;
    };

    $scope.getBankBranchList = function () {
        var key = $scope.vm.BankID;
        var filters = _.filter(tempBankBranchList, function (f) {
            return f.BankID == key;
        });
        $scope.BankBranchList = [];
        $scope.BankBranchList = filters;
    };

  
    // Check username 
    $scope.checkEmployeeID = function () {

        if (!$scope.vm.EmployeeID || $scope.vm.EmployeeID == "")
        {
            $scope.employeeIDstatus = ' ';
            $scope.IsSubmitEnable = true;
            return;
        }
               
        $scope.vm.CurrentUserID = currentUser.CurrentUserID;
        $scope.vm.CurrentCulture = currentCulture;
        $scope.vm.ApplicationId = appSettings.ApplicationId;

        ajaxService.AjaxPostWithData($scope.vm, "employee/IsExisting", onGetStatus, onError);

    }
    var onGetStatus =function(response)
    {
        if (response == true) {
            $scope.employeeIDstatus='Employee already exist';
        }  else {
            $scope.employeeIDstatus = ' ';
            $scope.IsSubmitEnable = false;
        }
       // $scope.employeeIDstatus = response;
    }
 



    $scope.submitEmployeeForm = function (vm) {

        $scope.isTriedSave = true;

      
        if (!vm.Name || !vm.EmployeeID || !vm.Password || !vm.CountryID || !vm.BranchOfficeID || !vm.Email ||
            vm.Name == "" || vm.EmployeeID == "" || vm.Password == "" || vm.CountryID == "" || vm.BranchOfficeID == "" || vm.Email == "") {
            return;
        }
        $scope.vm.CurrentUserID = currentUser.CurrentUserID;
        $scope.vm.CurrentCulture = currentCulture;
        $scope.vm.ApplicationId = appSettings.ApplicationId;

        ajaxService.AjaxPostWithData($scope.vm, "employee/save", onSuccess, onError);
    };

    $scope.edit = function (row) {
        $scope.vm = angular.copy(row);
        $scope.vm.photo = angular.copy(row.Photo);
        $scope.vm.JoiningDate = new Date(row.JoiningDate);
        $scope.vm.LeavingDate = new Date(row.LeavingDate);
        $scope.vm.BirthDate = new Date(row.BirthDate);
        $scope.vm.ClockInTime = new Date(row.ClockInTime);
        $scope.vm.ClockOutTime = new Date(row.ClockOutTime);
        $location.hash('changeCountry');

        // call $anchorScroll()
        $anchorScroll();
    };


    function GetEmployeeByID() {
        ajaxService.AjaxPostWithData(BaseModel, "employee/list", onGetEmployeeByID, onGetError);
    }

    var onGetEmployeeByID = function (response) {
            var data = response[0];
            $scope.vm = angular.copy(data);
            $scope.vm.photo = angular.copy(data.Photo);
            $scope.vm.JoiningDate = new Date(data.JoiningDate);
            $scope.vm.LeavingDate = new Date(data.LeavingDate);
            $scope.vm.BirthDate = new Date(data.BirthDate);
            $scope.vm.ClockInTime = new Date(data.ClockInTime);
            $scope.vm.ClockOutTime = new Date(data.ClockOutTime);
            $scope.IsSubmitEnable = false;
        }
        var onGetError = function (message) {
            toastr.error('Error in getting records');
        };
        $scope.delete = function (row) {
            var returnvalue = confirm("Are you sure to delete employee " + row.Name);
            if (returnvalue == true) {
                ajaxService.AjaxPostWithData(row, "employee/delete", onSuccess, onError);

                //var onSuccessDel = function (data, status, headers, config) {
                //    toastr.success('successfully delete');
                //    $scope.vm = {};
                //    getAllEmployee();
                //};

            }
        };

        var onSuccess = function (data, status, headers, config) {
            toastr.success('successfully saved');
            $scope.vm = {};
            getAllEmployee();
        };

        var onError = function (data, status, headers, config) {
            toastr.error('Error in saving records');
        };

    }

