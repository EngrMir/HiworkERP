angular.module("hiworkApp").component('webordermodule', {
    templateUrl: 'app/Components/WebOrder/Template/webOrderModule.html',
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
    controller: webOrderModuleController
})

function webOrderModuleController($scope, AppStorage, appSettings, sessionFactory, ajaxService, $stateParams) {
    var vm = this;
    vm.model = {};
    vm.model.MessageItem = [];
    var MessageModel = {
        ID: 0, SenderID: "", SenderName: "", ReceiverName: "", ReceiverID: "",
        Title: "", Body: "", Status: 0, ReadDate: "", ReplyDate: "", AttachedFile: "",
        AttachedSize: 0, DownloadURL: "", OrderID: "", IsDeleted: false, CreatedBy: 0,
        CreatedDate:"", UpdatedBy:0, UpdatedDate:""
    }
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
                { field: 'Title', displayName: "Title", cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a href="" ng-click="grid.appScope.defineRoute(row.entity)">{{COL_FIELD}}</a></div>' },
                { field: 'ID', displayName: "ID" },
                { field: 'Status', displayName: "Status" },
                { field: 'ReadDate', displayName: 'Read Date', cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'CreatedDate', displayName: 'Reply Date', cellFilter: 'date:"dd/MM/yyyy"' }
            ]
    };

    vm.$onInit=function()
    {
        debugger;
        vm.model = $stateParams.OrderInformation;
        vm.model.MessageItem = $stateParams.OrderInformation.MessageList;
        $scope.gridOptions.data = $stateParams.OrderInformation.MessageList;
        // vm.model.MessageItem.push(angular.copy(MessageModel));
        vm.EstimateCalculation();
        vm.model.CompletionDate = new Date(vm.model.CompletionDate);
        vm.model.CreatedDate = new Date(vm.model.CreatedDate);
        vm.model.DeliveryDate = new Date(vm.model.DeliveryDate);
        vm.model.EndDate = new Date(vm.model.EndDate);
        vm.model.OrderDate = new Date(vm.model.OrderDate);
        vm.model.RequestDate = new Date(vm.model.RequestDate);
        vm.model.StartDate = new Date(vm.model.StartDate);
        vm.model.UpdatedDate = new Date(vm.model.UpdatedDate);

    }
    vm.EstimateCalculation =function()
    {
        vm.model.EstimatedPrice = vm.model.WordCount * vm.model.UnitPrice;
    }

    $scope.defineRoute=function(Entity)
    {
        vm.model.MessageBody = Entity.Body;
    }
}