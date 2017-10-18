angular.module('hiworkApp').component('chooseqrwindow', {
    templateUrl: 'app/Components/QuotationRequest/quotationRequestTemplate.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: quotationRequestController,
    controllerAs: "vm"
});
quotationRequestController.$inject = ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', '$http'];
function quotationRequestController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $http) {
    var vm = this;
    vm.model = {};
    vm.PageIndex = 1;
    vm.PageSize = 30;
    vm.dataList = [];
    var flag = true;

    var elm = angular.element(document).find('.company-container')[0];
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    vm.$onInit = function () {
        vm.title = vm.resolve.title;
        vm.estimationId = vm.resolve.estimationId;
        vm.loadCompanies();
    }

    vm.loadCompanies = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        BaseModel.PageIndex = vm.PageIndex;
        BaseModel.PageSize = vm.PageSize;
        ajaxService.AjaxPostWithData(BaseModel, "company/estimationrequestedcompanylist/" + vm.estimationId, function (r) {
            vm.dataList = r;
        }, function () { });
    };

    vm.sendRequest = function () {
        var estimation = {
            ID: vm.resolve.estimationId
        };
        var data = vm.dataList.filter(function (item) {
            return item.IsSelected === true;
        });
        var model = {
            'CompanyModels': data,
            'Estimation': estimation,
            'CurrentUserID': currentUser.CurrentUserID,
            'Culture': currentCulture
        };
        ajaxService.AjaxPostWithData(model, 'company/sendsequest', function () {
            vm.Close();
        }, errorOnSaving);
    }

    vm.Close = function () {
        vm.modalInstance.close(vm.modalData);
    };

    vm.Dismiss = function () {
        vm.modalInstance.dismiss("cancel");
    };

    var successOnSaving = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
}