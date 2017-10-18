angular.module('hiworkApp').component('createemailwindow', {
    templateUrl: 'app/Components/Email/createEmailTemplate.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: createemailController,
    controllerAs: "vm"
});
createemailController.$inject = ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', '$http', '$state'];
function createemailController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $http,$state) {
    //edit start
    var vm = this;
    vm.model = {};

    //edit end
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    vm.$onInit = function () {
        vm.title = vm.resolve.title;
        vm.model.Subject = null;
        vm.model.Name = null;
        vm.model.ParentID = null;
        vm.model.Body = null;
        vm.GroupVisibleValue = true;
        vm.TemplateVisibleValue = true;
        vm.TitleVisibleValue = true;
        vm.BodyVisibleValue = true;
        vm.CreateGroupVisibl = false;
        vm.NewButtonVisible = true;
        vm.CreateTemplateVisible = false;
        vm.model.EmailBodyVisibleValue = true;
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
        var lengtn = vm.GroupIDList.lengtn;
        vm.GroupNameList = [];
        var index = 0;
        for (i = 0; i < vm.GroupIDList.length; i++) {
            if(vm.GroupIDList[i].ParentID ==null)
            {
                vm.GroupNameList[index] = vm.GroupIDList[i];
                index++;
            }
            
        }
    };
    vm.Close = function () {
        vm.modalInstance.close(vm.modalData);
    };

    vm.Dismiss = function () {
        vm.modalInstance.dismiss("cancel");

    };
    vm.saveEmail = function () {
        if (vm.model.ItemList != null && vm.model.TemplateItemList != null && vm.model.Subject !=null && vm.model.Body !=null)
        {
            vm.model.CurrentUserID = currentUser.CurrentUserID;
            vm.model.CurrentCulture = currentCulture;
            vm.model.ID = vm.model.TemplateItemList.ID;
            vm.model.Name = vm.model.TemplateItemList.Name;
            vm.model.ParentID = vm.model.TemplateItemList.ParentID;
            ajaxService.AjaxPostWithData(vm.model, "email/update", successOnSaving, errorOnSaving);
        }
        else
        {
            return;
        }
    };

    var successOnSaving = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
        vm.Dismiss();
        $state.go("emailWindow");
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };

    vm.saveEmailGroupName = function()
    {
        if (vm.model.NewGroupName != null)
        {
            vm.model.CurrentUserID = currentUser.CurrentUserID;
            vm.model.CurrentCulture = currentCulture;
            vm.model.Name = vm.model.NewGroupName;
            ajaxService.AjaxPostWithData(vm.model, "email/save", successOnGroupNameSaving, errorOnGroupNameSaving);
            vm.LoadEmailTemplateGroupName();
            vm.GroupVisibleValue = true;
            vm.TemplateVisibleValue = true;
            vm.TitleVisibleValue = true;
            vm.BodyVisibleValue = true;
            vm.CreateGroupVisible = false;
            vm.CreateTemplateVisible = false;
            
        }
        else
        {
            return;
        }
    }

    var successOnGroupNameSaving= function(response)
    {
        vm.GroupIDList = response;
        var lengtn = vm.GroupIDList.lengtn;
        vm.GroupNameList = [];
        var index = 0;
        for (i = 0; i < vm.GroupIDList.length; i++) {
            if (vm.GroupIDList[i].ParentID == null) {
                vm.GroupNameList[index] = vm.GroupIDList[i];
                index++;
            }

        }
        toastr.success($filter('translator')('DATASAVED'));
    }
    
    var errorOnGroupNameSaving = function(message)
    {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    }

    vm.saveEmailTemplateName = function ()
    {
        if (vm.model.NewTemplateName != null)
        {
            vm.model.CurrentUserID = currentUser.CurrentUserID;
            vm.model.CurrentCulture = currentCulture;
            vm.model.ParentID = vm.model.ItemList.ID;
            vm.model.Name = vm.model.NewTemplateName;
            ajaxService.AjaxPostWithData(vm.model, "email/save", successOnTemplateNameSaving, errorOnTemplateNameSaving);
            vm.GroupVisibleValue = true;
            vm.TemplateVisibleValue = true;
            vm.TitleVisibleValue = true;
            vm.BodyVisibleValue = true;
            vm.CreateGroupVisible = false;
            vm.CreateTemplateVisible = false;
            vm.NewButtonVisible = true;
        }
        else
        {
            return;
        }
    }

    var successOnTemplateNameSaving = function (response) {
        vm.GroupIDList = response;
        var lengtn = vm.GroupIDList.lengtn;
        vm.GroupNameList = [];
        var index = 0;
        for (i = 0; i < vm.GroupIDList.length; i++) {
            if (vm.GroupIDList[i].ParentID == null) {
                vm.GroupNameList[index] = vm.GroupIDList[i];
                index++;
            }

        }
        toastr.success($filter('translator')('DATASAVED'));
    }

    var errorOnTemplateNameSaving = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    }

    vm.CallSaveEmailGroupName = function () {
        vm.model.EmailBodyVisibleValue = true;
        vm.GroupVisibleValue = false;
        vm.TemplateVisibleValue = false;
        vm.TitleVisibleValue = false;
        vm.BodyVisibleValue = false;
        vm.CreateGroupVisible = true;
        vm.CreateTemplateVisible = false;

    }


    vm.CallSaveEmailTemplateName = function () {
        vm.model.EmailBodyVisibleValue = true;
        vm. NewButtonVisible = false;
        vm.GroupVisibleValue = true;
        vm.TemplateVisibleValue = false;
        vm.TitleVisibleValue = false;
        vm.BodyVisibleValue = false;
        vm.CreateGroupVisible = false;
        vm.CreateTemplateVisible = true;

    }

    vm.getEmailTemplateList = function()
    {
        var key = vm.model.ItemList.ID;
        var filters = _.filter(vm.GroupIDList, function (f) {
            if (f.ParentID == null) {

            }
            else {
                return f.ParentID == key;
            }
        });
        vm.EmailTemplateList = [];
        vm.EmailTemplateList = filters;

        if (vm.model.ItemList.ID != null && vm.model.TemplateItemList.ID != null) {
            vm.model.EmailBodyVisibleValue = false;
            vm.model.TitleVisibleValue = false;
        }

    }
 
}