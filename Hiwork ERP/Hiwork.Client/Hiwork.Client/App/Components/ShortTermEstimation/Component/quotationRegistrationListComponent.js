

angular.module("hiworkApp").component("estimationList", {
    templateUrl: "App/Components/ShortTermEstimation/Template/translationEstimationList.html",
    controllerAs: "vm",
    controller: estimationListController
});
estimationListController.$inject = ['$scope', 'appSettings', 'AppStorage', 'sessionFactory', 'ajaxService'];


function estimationListController($scope, appSettings, AppStorage, sessionFactory, ajaxService) {

    var vm = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    vm.estimationList = {

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

        //rowHeight: 80,
        //showGridFooter: true,

        columnDefs:
            [
                { field: 'EstimationNo', displayName: "Estimation No", cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a ui-sref="TranslationEstimation({Estimation:row.entity})">{{COL_FIELD}}</a></div>' },
                { field: 'RegistrationDate', displayName: 'Registration Date', cellFilter: 'date:"dd/MM/yyyy"' },
                { field: 'ClientName', displayName: "Company" },
                { field: 'EstimationTypeName', displayName: "Type" },
                { field: 'EstimationStatusName', displayName: "Status" },
                { field: 'CurrencyName', displayName: "Currency" }
            ]
    };

    vm.$onInit = function () {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "quotation/list", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        vm.estimationList.data = response;
    };

    $scope.edit = function (ID) {
        console.log(ID)
    };
}
