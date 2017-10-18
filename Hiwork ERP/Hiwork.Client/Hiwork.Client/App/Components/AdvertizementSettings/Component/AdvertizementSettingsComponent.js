
angular.module("hiworkApp").component("advertizementsettings", {

    templateUrl: 'app/Components/AdvertizementSettings/Template/AdvertizementSettings.html',
    controller: advertizementcomponentController,
    controllerAs: "vm"
});
advertizementcomponentController.$inject = ['$scope', 'appSettings', 'AppStorage', 'sessionFactory', 'ajaxService', 'fileUploadService'];

function advertizementcomponentController($scope, appSettings, AppStorage, sessionFactory, ajaxService, fileUploadService) {

    var vm = this;
    vm.ImageFile = null;
    vm.IsEdit = false;
    vm.model = new Object();
    vm.model.BackgroundImageFile = null;
    vm.model.CreatedBy = null;
    vm.model.CurrentCulture = null;
    vm.model.Title = null;
    vm.model.Description = null;
    vm.model.ValidDateTime = null;
    vm.model.BackgroundColor = null;
    $scope.rowCollection = [];
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    var currentFileName = null;


    vm.LoadAdvertizementSettings = LoadAdvertizementSettings ;
    
    function LoadAdvertizementSettings() {

        ajaxService.AjaxPostWithData(BaseModel, "advertizementsettings/list", successOnLoading, errorOnLoading);
    }
  
    vm.$onInit = function () {
       LoadAdvertizementSettings();
    }

    vm.saveData = function () {
        fileUploadService.uploadFileToUrl(vm.ImageFile, appSettings.API_BASE_URL + "fileUpload/uploadAdvertisingPhotos",
        currentUser.CurrentUserID, currentCulture, onSuccessImageUpload, onErrorImageUpload);
        //if (vm.IsEdit == false) {
        //    fileUploadService.uploadFileToUrl(vm.ImageFile, appSettings.API_BASE_URL + "fileUpload/uploadAdvertisingPhotos",
        //    currentUser.CurrentUserID, currentCulture, onSuccessImageUpload, onErrorImageUpload);
        //}
        //else {
        //    debugger;
        //    ajaxService.AjaxPostWithData(vm.model, "advertizementsettings/save", successOnSaving, errorOnSaving);
        //}
    };

    var onSuccessImageUpload = function (response) {
        debugger;
        // --------------------------
        if (response != null) {
            if (response != vm.currentFileName) {
                vm.model.BackgroundImageFile = response;
            }
            else {
                vm.model.BackgroundImageFile = vm.currentFileName;
            }
        }
        //ajaxService.AjaxPostWithData(vm.model, "advertizementsettings/save", successOnSaving, errorOnSaving);
        // --------------------------

        toastr.success("Successfully uploaded image file on server, Path: " + response.data);
        vm.model.CreatedBy = currentUser.CurrentUserID;
        vm.model.CurrentCulture = currentCulture; 
        vm.model.BackgroundImageFile = response.data;
        ajaxService.AjaxPostWithData(vm.model, "advertizementsettings/save", successOnSaving, errorOnSaving);
    };
    var onErrorImageUpload = function (response) {
        toastr.error("Error occured while uploading image to server");
    };

    var successOnSaving = function (response) {
        toastr.success('Insertion Successful');
        vm.LoadAdvertizementSettings();
    };
    var errorOnSaving = function (response) {
        toastr.error('Insertion Failed');
    };
    vm.edit = function (list)
    {
        debugger;
        vm.currentFileName = list.BackgroundImageFile;
        vm.model = angular.copy(list);
        vm.model.ValidDateTime = new Date(list.ValidDateTime);
        vm.IsEdit = true;

    }

    $scope.$on("dataDeleted", function (event, response) {
        LoadAdvertizementSettings();
    });



    var successOnLoading=function(response)
    {
        //debugger;
        $scope.rowCollection = response
        vm.Advertizementlist = response;
        

    }
    var errorOnLoading = function (message) {
        
        toastr.error('Loading Failed');
    }
}