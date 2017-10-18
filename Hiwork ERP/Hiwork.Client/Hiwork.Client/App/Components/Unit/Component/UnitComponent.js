//For parent Unit list done by tamal
angular.module("hiworkApp").component('units', {
    templateUrl: 'app/Components/Unit/Template/unitList.html',
    controller: unitController
})

function unitController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.unit = { Name: "", Code: "", SortBy:"", IsActive: false };
    $scope.rowCollection = [];
    $scope.unitColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    //if (!loginFactory.IsAuthenticated())
    //    $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllUnit();
    };
    function GetAllUnit() {
        var unit = {};
        unit.CurrentUserID = currentUser.CurrentUserID;
        unit.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(unit, "unit/list", onGetData, onGetError);
    }
    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.unitColl = [].concat($scope.rowCollection);
    };
    var onGetError = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
    //$scope.searchText = function () {
    //    var value = angular.element('#search').val();
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.displayColl = null;
    //    $scope.displayColl = [].concat(data);
    //};
    $scope.$on("unitAdded", function (event, response) {
        GetAllUnit();

    });
    $scope.$on("dataDeleted", function (event, response) {
        GetAllUnit();
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditUnit",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.unit = { Name: "", Code: "", SortBy:"", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.unit;
                    }
                    else {
                        this.unit = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.unit;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for Unit pop up window
angular.module('hiworkApp').component('addEditUnit', {
    templateUrl: 'app/Components/Unit/Template/addEditUnit.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addUnitController
});

function addUnitController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveUnit = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code || !$ctrl.modalData.SortBy ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == ""|| $ctrl.modalData.SortBy == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "unit/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("unitAdded", response);
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