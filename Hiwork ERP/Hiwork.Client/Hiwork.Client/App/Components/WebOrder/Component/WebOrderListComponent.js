angular.module("hiworkApp").component('weborderlist', {
    templateUrl: 'app/Components/WebOrder/Template/webOrderList.html',
    controllerAs: "vm",
    bindings: {
        routes: "=",
        services: "=",
        languages: "=",
        businessCategories: "=",
        specializedFields: "=",
        subSpecializedFields: "=",
        currencyList: "="
    },
    controller: webOrderListController
})

function webOrderListController($scope, AppStorage, appSettings, sessionFactory, ajaxService, $stateParams, $state) {
    
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
                { field: 'OrderNo', displayName: "Order No", cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a href="" ng-click="grid.appScope.defineRoute(row.entity)">{{COL_FIELD}}</a></div>' },
                { field: 'OrderDate', displayName: 'Order Date', cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'OrderStatusName', displayName: "Order Status" },
                { field: 'ApplicationName', displayName: "Application Name" },
                { field: 'StartDate', displayName: 'Start Date', cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'CompletionDate', displayName: 'Completion Date', cellFilter: 'date:"dd/MM/yyyy"' }
            ]
    };

    $scope.defineRoute = function (Entity) {
        var route;
        route = "WebOrderModule";
        $state.go(route, { "id": Entity.ID, "OrderInformation": Entity });

    };


    vm.$onInit = function () {
        debugger;
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = 2;
        BaseModel.cultureId = currentCulture;
        ajaxService.AjaxPostWithData(BaseModel, "order/getweborders", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        $scope.gridOptions.data = response;
    };

    $scope.edit = function (ID) {
        console.log(ID)
    };

    vm.goForSearch=function()
    {
        debugger;
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = 2;
        BaseModel.cultureId = currentCulture;
        BaseModel.orderNo=vm.OrderNo;
        BaseModel.orderId =vm.OrderID;
        BaseModel.translationType=vm.TranslatorType;
        BaseModel.srcLangId = vm.SourceLanguageID;
        BaseModel.trgLangId = vm.TargetLanguageID;
        BaseModel.specialFieldId=vm.SpecialFieldID;
        BaseModel.clientId=vm.ClientID;
        BaseModel.translatorId=vm.TranslatorID;
        BaseModel.orderStatus=vm.OrderStatus;
        BaseModel.startDate = vm.StartDate;
        BaseModel.endDate = vm.EndDate;
        BaseModel.firstDateMonth=vm.FirstDateMonth;
        BaseModel.lastDateMonth = vm.LastDateMonth;
        ajaxService.AjaxPostWithData(BaseModel, "order/getweborders", onGetData, onErrorToastMessage);
    }

    vm.CompanyList = [
       { Name: 'Corporate', ID: 1 },
       { Name: 'Individuals',ID:2 }
    ]

}