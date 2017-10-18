

angular.module("hiworkApp").component("advancedStaffSearchList", {
    templateUrl: "App/Components/AdvancedStaffSearch/Template/advancedStaffSearchList.html",
    controllerAs: "vm",
    controller: advancedStaffSearchList
});
advancedStaffSearchList.$inject = ['$scope', 'appSettings', 'AppStorage', 'sessionFactory', 'ajaxService', '$state', '$stateParams'];


function advancedStaffSearchList($scope, appSettings, AppStorage, sessionFactory, ajaxService, $state, $stateParams) {

    var vm = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

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
        enablePaging: true,
        paginationPageSizes: [10, 25, 50, 100],
        paginationPageSize: 10,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        },
        //rowHeight: 80,
        //showGridFooter: true,
        columnDefs:
            [
                { field: 'ID', visible: false, displayName: "ID" },
                { field: 'Name', displayName: "Staff Name", cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()">{{COL_FIELD}}</div>' },
                { field: 'BirthDate', displayName: "BirthDate", cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'Gender', displayName: 'Gender', filterCellFiltered: true },
                { field: 'LivingCountry', displayName: "Living Country" },
                { field: 'PostalCode', displayName: "Postal Code" }
            ]
    };

    //$scope.gridOptions = {
    //    excludeProperties: '__metadata',
    //    enableHorizontalScrollbar: 0,
    //    enableVerticalScrollbar: 0,
    //    enableRowSelection: true,
    //    enableRowHeaderSelection: true,
    //    multiSelect: false,
    //    enableSorting: true,
    //    enableGridMenu: true,
    //    saveFocus: false,
    //    saveScroll: true,
    //    rowBorderTop: '1px solid lightgrey',
    //    enableSelectAll: true,
    //    enableFiltering: true,
    //    enablePaging: true,
    //    paginationPageSizes: [10, 25, 50, 100],
    //    paginationPageSize: 10,
    //    onRegisterApi: function(gridApi) {
    //        $scope.gridApi = gridApi;
    //    },
    //    //rowHeight: 80,
    //    //showGridFooter: true,
    //    columnDefs:
    //        [
    //            { field: 'ID', visible: false, displayName: "ID" },
    //            { field: 'Name', displayName: "Staff Name", cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a href="" ng-click="grid.appScope.defineRoute(row.entity)">{{COL_FIELD}}</a></div>' },
    //            { field: 'BirthDate', displayName: "BirthDate", cellFilter: 'date:"dd/MM/yyyy"' },
    //            { field: 'Gender', displayName: 'Gender', filterCellFiltered: true },
    //            { field: 'LivingCountry', displayName: "Living Country" },
    //            { field: 'PostalCode', displayName: "Postal Code" }
    //        ]
    //};

    $scope.defineRoute = function (Entity) {
        var route;
        switch (Entity.EstimationType) {
            case EstimationType.Translation:
                route = "TranslationEstimation";
                break;
            case EstimationType.Project:
                route = "TaskQuotationInput";
                break;
            case EstimationType.DTP:
                route = "DTPEstimation";
                break;
            case EstimationType.ShortTermDispatch:
                route = "ShortTermEstimation";
                break;
            case EstimationType.Transcription:
                route = "TranscriptionEstimation";
                break;
            case EstimationType.OverheadCost:
                route = "OverheadCostQuotation";
                break;
        }
        $state.go(route, { "id": Entity.ID, "Estimation": Entity });
    };


    vm.$onInit = function () {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        if ($stateParams.MODEL != null) {
            vm.AdvancedStaffSearchModel = $stateParams.MODEL;
            ajaxService.AjaxPostWithData(vm.AdvancedStaffSearchModel, "advancedstaffsearchlist/search", onGetData, onErrorToastMessage);
        }
    };

    var onGetData = function (response) {
        console.log(response);
        $scope.gridOptions.data = response;
    };

    $scope.edit = function (ID) {
        console.log(ID)
    };
}
