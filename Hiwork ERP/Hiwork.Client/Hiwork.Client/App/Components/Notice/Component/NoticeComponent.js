angular.module("hiworkApp").component("notice", {

    templateUrl: 'app/Components/Notice/Template/Notice.html',
    controller: noticeController,
    controllerAs: "vm"

});
noticeController.$inject = ['$scope','appSettings', 'AppStorage', 'sessionFactory', 'ajaxService'];
function noticeController($scope,appSettings, AppStorage, sessionFactory, ajaxService) {
    var vm = this;
    vm.model = {};
    $scope.rowCollection = [];
  
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    var BaseModel = new Object();

    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;

    vm.isDisabled = true;

    vm.$onInit = function () {
        vm.LoadNoticeList();
        GetPriorityList();
        vm.checkboxSelection = null;
        vm.model.Priority = null;
        vm.model.PriorityName = null;
        vm.model.RegisteredDate = null;
        vm.model.Title = null;
        vm.model.NoticeURL = null;
        vm.model.Description = null;
        vm.model.ClientDisplayStatus = null;
       
        vm.model.ClientDisplayStatusName = null;
        vm.model.StaffDisplayStatus = null;
        vm.model.StaffDisplayStatusName = null;
        vm.model.PartnerDisplayStatus = null;
        vm.model.PartnerDisplayStatusName = null;
        vm.isDescriptionHidden = true;
    };

    vm.saveData=function()
    {
        vm.model.CreatedBy = currentUser.CurrentUserID;
        vm.model.CurrentCulture = currentCulture;

        switch (vm.ClientRadio) {
            case "ShowClient":
                vm.model.ClientDisplayStatus = 1;
                break;
            case "HideClient":
                vm.model.ClientDisplayStatus = 2;
                break;
            case "shClient":
                vm.model.ClientDisplayStatus = 3;
                break;
        }

        switch (vm.StaffRadio) {
            case "ShowTranslator":
                vm.model.StaffDisplayStatus = 1;
                break;
            case "HideTranslator":
                vm.model.StaffDisplayStatus = 2;
                break;
            case "shTranslator":
                vm.model.StaffDisplayStatus = 3;
                break;
        }

        switch (vm.PartnerRadio) {
            case "ShowPartner":
                vm.model.PartnerDisplayStatus = 1;
                break;
            case "HidePartner":
                vm.model.PartnerDisplayStatus = 2;
                break;
            case "shPartner":
                vm.model.PartnerDisplayStatus = 3;
                break;
        }

        ajaxService.AjaxPostWithData(vm.model, "notice/save", successOnSaving, errorOnSaving);
        vm.LoadNoticeList();
    }
    
    vm.LoadNoticeList = function () {     
        ajaxService.AjaxPostWithData(BaseModel, "notice/list", successOnLoading, errorOnLoading);
    }

    var GetPriorityList = function()
    {
        ajaxService.AjaxPostWithData(BaseModel, "notice/priority", OnGetData, errorOnLoading);
    }

    var OnGetData =function(response)
    {
        vm.SetPriority = response;
        
    }
    var successOnLoading = function (response) {
        $scope.rowCollection = response
        vm.NoticeList = response;
       

    }
    var errorOnLoading=function(message)
    {
        toastr.error('Loading Failed');
    }
    var successOnSaving = function (response) {
        toastr.success('Insertion Successful');
        vm.LoadNoticeList();
        //call
    };

    var errorOnSaving = function (response) {
        toastr.error('Insertion Failed');
    };
    vm.edit = function (notice) {
        vm.model = angular.copy(notice);
        vm.model.RegisteredDate = new Date(notice.RegisteredDate);

        switch (vm.model.ClientDisplayStatus) {
            case 1:
                vm.ClientRadio = "ShowClient";
                break;
            case 2:
                vm.ClientRadio = "HideClient";
                break;
            case 3:
                vm.ClientRadio = "shClient";
                break;
        }

        switch (vm.model.StaffDisplayStatus) {
            case 1:
                vm.StaffRadio = "ShowTranslator";
                break;
            case 2:
                vm.StaffRadio = "HideTranslator";
                break;
            case 3:
                vm.StaffRadio = "shTranslator";
                break;
        }

        switch (vm.model.PartnerDisplayStatus) {
            case 1:
                vm.PartnerRadio = "ShowPartner";
                break;
            case 2:
                vm.PartnerRadio = "HidePartner";
                break;
            case 3:
                vm.PartnerRadio = "shPartner";
                break;
        }
    }
    //$scope.$on("dataDeleted", function (event, response) {
    //    vm.LoadNoticeList();
    //});
    //vm.SetPriority = [
    //   { Name: "General", No: 1 },
    //   { Name: "Important", No: 2 }
    //];
    vm.isCheckboxSelected = function (status) {
        
        if (status.checkboxSelection == true) {
            vm.isDisabled = false;
        }
        else {
            vm.isDisabled = true;
        }

    };
    $scope.$on("dataDeleted", function (event, response) {
        vm.LoadNoticeList();
    });
   
    vm.hideDescription = function () {
        if (vm.isDescriptionHidden) {
            vm.isDescriptionHidden = false;
        }
        else {
            vm.isDescriptionHidden = true;
        }
    }
    //vm.delete = function (notice) {
    //    ajaxService.AjaxPostWithData(notice, "notice/delete", onDelete, errorOnLoading);
    //};

    //var onDelete = function (response) {
    //    vm.LoadNoticeList();
    //};


}