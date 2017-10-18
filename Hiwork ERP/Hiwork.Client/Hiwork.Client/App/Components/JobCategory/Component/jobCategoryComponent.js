//For parent jobCategory list
angular.module("hiworkApp").component('jobcategories', {
    templateUrl: 'app/Components/JobCategory/Template/jobCategoryList.html',
    controller: jobCategoryController
})

function jobCategoryController($scope, $uibModal, appSettings, AppStorage, sessionFactory,loginFactory, $filter, ajaxService, $state) {
    this.jobCategory = { Name: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAlljobCategories();
    };
    function GetAlljobCategories() {
        var jobCategory = {};
        jobCategory.CurrentUserID = currentUser.CurrentUserID;
        jobCategory.CurrentCulture = currentCulture;      
        ajaxService.AjaxPostWithData(jobCategory, "jobcategory/list", onGetData, onGetError);
    }
    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };
    var onGetError = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };
    $scope.$on("jobCategoryAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
       
    });
    $scope.$on("dataDeleted", function (event, response) {
      
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    
    this.open = function (obj, title) {        
        $uibModal.open({
            component: "addEditJobCategory",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.jobCategory = { Name: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.jobCategory;
                    }
                    else {
                        this.jobCategory = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.jobCategory;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for jobCategory pop up window
angular.module('hiworkApp').component('addEditJobCategory', {
        templateUrl: 'app/Components/JobCategory/Template/addEditJobCategory.html',
        bindings: {        
            modalInstance: "<",
            resolve: "<"
        },   
        controller: addjobCategoryController
});

function addjobCategoryController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.savejobCategory = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "jobcategory/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("jobCategoryAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}