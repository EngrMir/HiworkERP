angular.module('hiworkApp').component('chooseemailwindow', {
    templateUrl: 'app/Components/Email/chooseEmailTemplate.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: createemailController,
    controllerAs: "vm"
});
createemailController.$inject = ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', '$http'];
function createemailController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $http) {
    //edit start
    var vm = this;
    vm.model = {};

    //edit end
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    vm.$onInit = function () {
        vm.title = vm.resolve.title;
        vm.model.TemplateSubject = null;
        vm.model.TemplateGroup = null;
        vm.model.ParentGroupID = null;
        vm.model.TemplateBody = null;
        vm.model.checkvalue = false;
        vm.LoadEmailTemplateGroupName();
    }

    vm.LoadEmailTemplateGroupName = function () {
        var BaseModel = new Object();
        BaseModel.ParentCheckValue = vm.model.checkvalue;
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "email/list", Onsuccess, errorOnSaving);
    };

    var Onsuccess = function (response) {
        vm.GroupIDList = response;
    };
    vm.Close = function () {
        vm.modalInstance.close(vm.modalData);
    };

    vm.Dismiss = function () {
        vm.modalInstance.dismiss("cancel");
    };
    vm.saveEmail = function () {
        vm.model.CurrentUserID = currentUser.CurrentUserID;
        vm.model.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(vm.model, "email/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };

}