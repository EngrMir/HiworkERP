angular.module("hiworkApp").component('staffList', {
    templateUrl: 'app/Components/StaffList/Template/staffList.html',
    controllerAs: "vm",
    //bindings: {
    //    routes: "=",
    //    services: "=",
    //    languages: "=",
    //    businessCategories: "=",
    //    specializedFields: "=",
    //    subSpecializedFields: "=",
    //    currencyList: "="
    //},
    controller: staffListController
})


function staffListController($scope, appSettings, AppStorage, sessionFactory, ajaxService, $state, EstimationType) {
    var vm = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    vm.tmk = [];
    vm.condition = {};
    vm.condition =
        {
            branch: '',
            type: '',
            eid: '',
            resign: '',
            role: '',
            author: '',
            ename: ''
        };

    vm.SearchStaff = function () {
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
        ajaxService.AjaxPostWithData(BaseModel, "Stafflist/conlist/" + vm.tmk, onGetData, onGetError);
    };
    $scope.gridOptions = {
        excludeProperties: '__metadata',
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        multiSelect: false,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        saveFocus: false,
        saveScroll: true,
        rowBorderTop: '1px solid lightgrey',
        enableSelectAll: true,
        //enablePaging: true,
        //paginationPageSizes: [25, 30, 85, 30],
        //exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red', margin: [0, 2, 0, 0] },
        //paginationPageSize: 10,
        paginationPageSize: 20,
        enablePaginationControls: false,
        paginationCurrentPage: 1,
        onRegisterApi: function(gridApi) {
            $scope.gridApi = gridApi;
        },
        columnDefs:
            [
                { field: 'StaffNo', displayName: "Staff Number", width: '15%', exporterPdfAlign: 'center', cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a href="" ng-click="grid.appScope.getID(row.entity)">{{COL_FIELD}}</a></div>' },
                { field: 'Name', width: '25%', displayName: "Staff Name" },
                { field: 'BirthDate', width: '20%', displayName: "BirthDate", cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'Gender', width: '10%', displayName: 'Gender', filterCellFiltered: true, exporterPdfAlign: 'center' },
                { field: 'LivingCountry', width: '15%', displayName: "Living Country" },
                { field: 'PostalCode', width: '15%', displayName: "Postal Code", exporterPdfAlign: 'center' }
            ],
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfHeader: { text: "Staff Information", style: 'headerStyle' },
        exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 25, bold: true, italics: true, exporterPdfAlign: 'center', margin: [225, 10, 0, 0] };
            return docDefinition;
        },
        exporterPdfOrientation: 'portrait',
        exporterPdfPageSize: 'LETTER',
        exporterPdfMaxGridWidth: 470,
    };
    vm.$onInit = function () {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "Stafflist/list", onGetData, onErrorToastMessage);
    };

    $scope.getTableHeight = function () {
        var rowHeight = 4; // your row height
        var headerHeight = 4; // your header height
        return {
            height: ($scope.gridOptions.data.length * rowHeight + headerHeight) + "px"
        };
    };

    $scope.getID = function (Entity) {
        ajaxService.AjaxPostWithData({ "Value": Entity.StaffNo }, "Stafflist/getid", onGetDataRedirect, onErrorToastMessage);
    };
    var onGetDataRedirect = function (response) {
        $state.go("StaffRegistration", { "staffNo": response[0].Id });
    };

    var onGetData = function (response) {
        $scope.gridOptions.data = response;
    };

    var onErrorToastMessage = function (response) {
        onErrorToastMessage("Not Found.");
    };

    $scope.edit = function (ID) {
        console.log(ID)
    };
}