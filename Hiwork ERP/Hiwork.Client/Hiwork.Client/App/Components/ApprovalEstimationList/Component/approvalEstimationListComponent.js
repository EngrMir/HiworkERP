angular.module("hiworkApp").component("approvalestimationList", {
    templateUrl: "App/Components/ApprovalEstimationList/Template/approvalEstimationList.html",
    controllerAs: "vm",
    controller: approvalEstimationListController
});
approvalEstimationListController.$inject = ['$scope', '$stateParams', '$filter', 'appSettings', 'AppStorage', 'sessionFactory', 'ajaxService', '$state', "EstimationType"];


function approvalEstimationListController($scope, $stateParams, $filter, appSettings, AppStorage, sessionFactory, ajaxService, $state, EstimationType) {

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
        onRegisterApi: function(gridApi) {
            $scope.gridApi = gridApi;
        },
        columnDefs:
            [
                { field: 'EstimationNo', displayName: "Estimation No", cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a href="" ng-click="grid.appScope.defineRoute(row.entity)">{{COL_FIELD}}</a></div>' },
                { field: 'RegistrationDate', displayName: 'Registration Date', cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'ClientName', displayName: "Company" },
                { field: 'EstimationTypeName', displayName: "Type"},
                { field: 'EstimationStatusName', displayName: "Status" },
                { field: 'CurrencyName', displayName: "Currency" },
                { field: 'CurrencyName', displayName: "Action", cellTemplate: '<button class="btn btn-approval-req btn-primary pull-left btn-xs"  ng-click="grid.appScope.approve(row.entity)">Approve</button>' }
            ]
    };

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

    $scope.approve = function (Entity) {
        ajaxService.AjaxPostWithData(Entity, 'estimation/approverequest', function (r) {
            toastr.success($filter('translator')('REQUESTAPPROVED'));
            $state.transitionTo($state.current, $stateParams, {
                reload: true,
                inherit: false,
                notify: true
            });
        }, errorFunction);
    };

    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    vm.$onInit = function () {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "estimation/approvallist", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        $scope.gridOptions.data = response;
    };

    $scope.edit = function (ID) {
        console.log(ID)
    };
}
