angular.module("hiworkApp").component('dashboard', {
    templateUrl: 'app/Components/Dashboard/Template/dashboard.html',
    controllerAs: "vm",
    bindings: {
        currencyList: "="
    },
    controller: dashboardController
})

function dashboardController($scope, AppStorage, appSettings, sessionFactory, ajaxService, $http) {

    var vm = this;    
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    var BaseModel = {};
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    vm.dashboardData = [];
    vm.Currency = {};

    vm.DashboardMenuCulture = [
        { Name_en: "Change Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 1 },
        { Name_en: "Sending Email", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 2 },
        { Name_en: "Cancel Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 3 }
    ];

    vm.$onInit = function () {
        if (vm.currencyList) {
            for (i = 0; i < vm.currencyList.length; i++) {
                if (vm.currencyList[i].Code == "USD") {
                    vm.Currency = vm.currencyList[i];
                    vm.changeCurrency();
                    break;
                }
            }
        }
        ajaxService.AjaxPostWithData(BaseModel, "dashboardsuperadmin/getdata", vm.onGetEstimationDetails, vm.errorFunction);
    }

    vm.onGetEstimationDetails = function (data) {
        if (data.length > 0) {
            vm.dashboardData = data[0];
        }
        debugger;
    }

    vm.errorFunction = function (error) {
        debugger;
    }
}