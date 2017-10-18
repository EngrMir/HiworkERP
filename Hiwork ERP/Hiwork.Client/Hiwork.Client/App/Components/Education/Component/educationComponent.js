//For parent Education list
angular.module("hiworkApp").component('educations', {
    templateUrl: 'app/Components/Education/Template/educationList.html',
    controller: EducationController
})

function EducationController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.Education = { Name: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllEducations();
    };
    function GetAllEducations() {
        var Education = {};
        Education.CurrentUserID = currentUser.CurrentUserID;
        Education.CurrentCulture = currentCulture;      
        ajaxService.AjaxPostWithData(Education, "education/list", onGetData, onGetError);
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
    $scope.$on("educationAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
       
    });
    $scope.$on("dataDeleted", function (event, response) {
      
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    
    this.open = function (obj, title) {        
        $uibModal.open({
            component: "addEditEducation",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.Education = { Name: "", Description: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.Education;
                    }
                    else {
                        this.Education = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.Education;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for Education pop up window
angular.module('hiworkApp').component('addEditEducation', {       
        templateUrl: 'app/Components/Education/Template/addEditEducation.html',
        bindings: {        
            modalInstance: "<",
            resolve: "<"
        },   
        controller: addEducationController
});

function addEducationController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveEducation = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "education/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("educationAdded", response);
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