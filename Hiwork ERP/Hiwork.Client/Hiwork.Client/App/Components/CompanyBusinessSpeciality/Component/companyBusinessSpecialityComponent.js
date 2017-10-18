//For parent companyBusiness list
angular.module("hiworkApp").component('companybusinessspeciality', {
    templateUrl: 'app/Components/CompanyBusinessSpeciality/Template/companyBusinessSpecialityList.html',
    controller: companyBusinessSpecialityController
})

function companyBusinessSpecialityController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.companybusinessspeciality = { Code: "", Name: "", IsActive: false, CurrentCulture: "" };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    this.companybusinessspeciality.CurrentCulture = currentCulture;
    this.init = function () {
        GetAllCompanyBusinessSpeciality();
    };
    function GetAllCompanyBusinessSpeciality() {
        var companybusinessspeciality = {};
        companybusinessspeciality.CurrentUserID = currentUser.CurrentUserID;
        companybusinessspeciality.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(companybusinessspeciality, "businessspeciality/list", onGetData, onGetError);
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
    $scope.$on("companyBusinessSpecialityAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditCompanyBusinessSpeciality",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.companybusinessspeciality = { Code: "", Name: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.companybusinessspeciality;
                    }
                    else {
                        this.companybusinessspeciality = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.companybusinessspeciality;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for companyBusiness pop up window
angular.module('hiworkApp').component('addEditCompanyBusinessSpeciality', {
    templateUrl: 'app/Components/CompanyBusinessSpeciality/Template/addEditCompanyBusinessSpeciality.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addcompanyBusinessSpecialityController
});

function addcompanyBusinessSpecialityController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveCompanyBusinessSpeciality = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "businessspeciality/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("companyBusinessSpecialityAdded", response);
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