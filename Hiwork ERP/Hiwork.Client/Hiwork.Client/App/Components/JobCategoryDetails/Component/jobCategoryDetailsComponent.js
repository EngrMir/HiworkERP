angular.module("hiworkApp").component('jobcategorydetails', {
    templateUrl: 'app/Components/JobCategoryDetails/Template/jobCategoryDetailsList.html',
    controller: jobcategoryController
})

function jobcategoryController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.jobcategorydetails = { JobCategoryId: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllJobCategoryDetails();
    };
    function GetAllJobCategoryDetails() {
        var jobcategorydetails = {};
        jobcategorydetails.CurrentUserID = currentUser.CurrentUserID;
        jobcategorydetails.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(jobcategorydetails, "jobcategorydetails/list", onGetData, onGetError);
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
    $scope.$on("jobcategorydetailsAdded", function (event, response) {
        GetAllJobCategoryDetails();
    });
    $scope.$on("dataDeleted", function (event, response) {
        GetAllJobCategoryDetails();
    });

   
    this.open = function (obj, title) {        
        $uibModal.open({
            component: "addEditJobCategoryDetails",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.jobcategorydetails = { JobCategoryId: "", Description: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.jobcategorydetails;
                    }
                    else {
                        this.jobcategorydetails = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.jobcategorydetails;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

angular.module('hiworkApp').component('addEditJobCategoryDetails', {
    templateUrl: 'app/Components/JobCategoryDetails/Template/addEditJobCategoryDetails.html',
        bindings: {        
            modalInstance: "<",
            resolve: "<"
        },   
        controller: addJobCategoryController
});

function addJobCategoryController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.getJobCategories = function () {
        var jobcategory = {};
        jobcategory.CurrentUserID = currentUser.CurrentUserID;
        jobcategory.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(jobcategory, "jobcategory/list", $ctrl.onGetData, $ctrl.onGetError);
    }

    $ctrl.onGetData = function (response) {
        $ctrl.jobcategoryList = [];
        $ctrl.jobcategoryList = response;
    };
    $ctrl.saveJobCategoryDetails = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Description || $ctrl.modalData.Description == "" || !$ctrl.modalData.JobCategoryId || $ctrl.modalData.JobCategoryId == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "jobcategorydetails/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("jobcategorydetailsAdded", response);
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
        $ctrl.getJobCategories();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}