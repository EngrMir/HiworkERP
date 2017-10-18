angular.module("hiworkApp").component("companylist", {
    templateUrl: "App/Components/Company/Template/companyList.html",
    bindings: {
        companyData: "="
    },
    controllerAs: "vm",
    controller: companyListController

})


companyListController.$inject = ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state'];

function companyListController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    var vm = this;

    vm.gridOptions = {
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
        paginationPageSizes: [10, 25, 50, 100],
        paginationPageSize: 10,
        //rowHeight: 80,
       // showGridFooter: true,
        columnDefs: [
                    { field: 'RegistrationID', displayName: $filter('translator')('ID'), enableFiltering: true },
                    { field: 'Name', displayName: $filter('translator')('NAME'), cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a ng-class="col.colIndex()"><a ui-sref="CompanyRegistration.companyInfo({ID:row.entity.ID})">{{COL_FIELD}}</a></div>' },
                    { field: 'RegistrationNo' },
                    { field: 'ClientNo'},
                    { field: 'ClientID' },
                    { field: 'WebSiteURL'},
                    { field: 'Capital' },
                     { field: 'ClientLocation', displayName:'Location' },
                    { field: 'EstablishedDate', displayName: 'Established Date', cellFilter: 'date:"dd/MM/yyyy"' },
                    //{
                    //    field: 'edit', name: $filter('translator')('ACTION'), enableFiltering: false, enableSorting: false, disableColumnMenu: true, enableColumnMenus: false,
                    //    cellTemplate: '<div><a ng-show="!row.entity.editrow" class="btn text-warning" ng-click="grid.appScope.edit(row.entity.ID)"><i class="fa fa-edit"></i></a>' +  //Edit Button
                    //        '<a ng-show="!row.entity.editrow" class="btn text-danger" ng-click="grid.appScope.delete(row.entity.ID)"><i class="fa fa-trash-o"></i></a>' + //Delete Button
                    //    '</div>', width: 100
                    //}
                    ],
            exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
            return docDefinition;
            },
            exporterPdfHeader: { text: "Company List", style: 'headerStyle' },
    }

    vm.$onInit = function () {
        vm.gridOptions.data = vm.companyData;
    }

    $scope.edit = function (ID) {
        console.log(ID)
        
        }
}
