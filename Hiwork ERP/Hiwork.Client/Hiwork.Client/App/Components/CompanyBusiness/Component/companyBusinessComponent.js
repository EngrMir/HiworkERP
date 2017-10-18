//For parent companyBusiness list
angular.module("hiworkApp").component('companybusiness', {
    templateUrl: 'app/Components/CompanyBusiness/Template/companyBusinessList.html',
    controller: companyBusinessController
})

function companyBusinessController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.companyBusiness = { Code: "", Name: "", IsActive: false, CurrentCulture: "" };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    //if (!loginFactory.IsAuthenticated())
    //    $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    this.companyBusiness.CurrentCulture = currentCulture;
    this.init = function () {
        GetAlljobCategories();
    };
    function GetAlljobCategories() {
        var companyBusiness = {};
        companyBusiness.CurrentUserID = currentUser.CurrentUserID;
        companyBusiness.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(companyBusiness, "companybusiness/list", onGetData, onGetError);
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
    $scope.$on("companyBusinessAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditcompanyBusiness",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.companyBusiness = { Code: "", Name: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.companyBusiness;
                    }
                    else {
                        this.companyBusiness = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.companyBusiness;
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
angular.module('hiworkApp').component('addEditcompanyBusiness', {
    templateUrl: 'app/Components/CompanyBusiness/Template/addEditCompanyBusiness.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addcompanyBusinessController
});

function addcompanyBusinessController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveCompanyBusiness = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "companybusiness/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("companyBusinessAdded", response);
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