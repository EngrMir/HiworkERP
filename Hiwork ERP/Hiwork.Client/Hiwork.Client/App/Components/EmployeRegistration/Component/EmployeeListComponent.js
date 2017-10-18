angular.module("hiworkApp").component('employeelist', {
    templateUrl: 'App/Components/EmployeRegistration/Template/EmployeeList.html',
    controller: empolyeeListController,
    controllerAs: "vm"
});

empolyeeListController.$inject = ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state'];

function empolyeeListController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state, $stateParams) {
    var vm = this;    
    vm.tmk = [];
    vm.condition = {};
    vm.condition =
        {
            branch: '',
            type: 0,
            eid: '',
            resign: false,
            role: '',
            author: '',
            ename: ''
        };
  
    vm.SearchEmployee = function () {
        if (vm.condition.type == null)
            vm.condition.type = 0;
        vm.tmk[0] = vm.condition.branch;
        vm.tmk[1] = vm.condition.type;
        vm.tmk[2] = vm.condition.eid;
        vm.tmk[3] = vm.condition.resign;
        vm.tmk[4] = vm.condition.role;
        vm.tmk[5] = vm.condition.author;
        vm.tmk[6] = vm.condition.ename;
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "employee/conlist/" + vm.tmk, onGetDataEmployee, onGetError);
    };
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.$onInit = function ()
    {
        getAllEmployee();
        vm.GetRoleList();
    }

    $scope.gridOptions = {
        excludeProperties: '__metadata',
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        multiSelect: true,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        saveFocus: false,
        saveScroll: true,
        rowBorderTop: '1px solid lightgrey',
        enableSelectAll: true,
        enablePaging: true,
        paginationPageSizes: [0, 0, 0, 0],
        paginationPageSize: 10,
        rowHeight: 80,
        columnDefs: [
                    //{ field: 'EmployeeNo', displayName: $filter('translator')('EMPLOYEENO') },
                     { field: 'EmployeeID', width: '8%', displayName: $filter('translator')('EMPLOYEEID'), cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a ui-sref="EmployeRegistration({ID:row.entity.ID})">{{COL_FIELD}}</a></div>' },
                     { field: 'Name', width: '8%', displayName: $filter('translator')('NAME') },
                     { field: 'ClockInTime', width: '10%', displayName: $filter('translator')('ATTEND'), cellFilter: 'date:"hh:mm:ss a"' },
                     { field: 'ClockOutTime', width: '10%', displayName: $filter('translator')('LEAVE'), cellFilter: 'date:"hh:mm:ss a"' },
                     { field: 'BirthDate', width: '10%', displayName: $filter('translator')('BIRTHDATE'), cellFilter: 'date:"dd/MM/yyyy"' },
                     { field: 'CountryName', width: '8%', displayName: $filter('translator')('COUNTRY') },
                     { field: 'BranchName', width: '8%', displayName: $filter('translator')('BRANCH') },
                     { field: 'DepartmentName', width: '8%', displayName: $filter('translator')('DEPARTMENT') },
                     //{ field: 'TeamName', displayName: $filter('translator')('TEAM') },
                     { field: 'EmployeeTypeName', width: '8%', displayName: $filter('translator')('EMPLOYEETYPE') },
                     { field: 'JoiningDate', width: '10%', displayName: $filter('translator')('JOININGDATE'), cellFilter: 'date:"dd/MM/yyyy"' },
                      { field: 'photo', width: '8%', displayName: $filter('translator')('PHOTO'), enableFiltering: false, enableColumnMenus: false, maxWidth: 80, enableSorting: false, cellTemplate: "<img width=\"80px\" height=\"80px\" ng-src=\"{{row.entity.Photo}}\" lazy-src>" },
                     {
                         field: 'edit', width: '5%', name: $filter('translator')('ACTION'), enableFiltering: false, enableSorting: false, disableColumnMenu: true, enableColumnMenus: false,
                         cellTemplate: '<div><a ng-show="!row.entity.editrow" class="btn text-warning" ng-click="grid.appScope.edit(row.entity.ID)"><i class="fa fa-edit"></i></a>' +  //Edit Button
                             '<a ng-show="!row.entity.editrow" class="btn text-danger" ng-click="grid.appScope.delete(row.entity)"><i class="fa fa-trash-o"></i></a>' + //Delete Button
                         '</div>', width: 100, enableHiding: true, exporterSuppressExport: true
                     }
                        ],
        exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 25, bold: true, italics: true, exporterPdfAlign: 'center', margin: [300, 10, 0, 0] };
            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
            return docDefinition;
        },
        exporterCsvFilename: 'EmployeeList.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterSuppressColumns: ['photo', 'EmployeeTypeName', 'DepartmentName'],
        exporterPdfMaxGridWidth: 650,
        exporterPdfTableStyle: { margin: [0, 0, 0, 0], widths: ['auto', 'auto', '*', '*', 'auto', 'auto', 'auto', 'auto'] },
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterPdfHeader: { text: "Employee Information", style: 'headerStyle' },
        exporterPdfFooter: function (currentPage, pageCount) {
            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        },

    };
    function getAllEmployee() {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "employee/list", onGetDataEmployee, onGetError);
    }

    var onGetDataEmployee = function (response) {
        $scope.gridOptions.data = [];
        $scope.gridOptions.data = response;
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    vm.GetRoleList = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "employee/rolelist", onGetRoleList, onGetError);
    }

    var onGetRoleList = function (response) {
        vm.RoleList = response;
    }
}