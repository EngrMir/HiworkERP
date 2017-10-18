
angular.module("hiworkApp").component('penlty', {
    templateUrl: 'App/Components/Penalty/Template/penaltyList.html',
    controller: penaltyController
});


function penaltyController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.bank = { Name: "", Code: "", CountryId: "", CurrencyId: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    var BaseviewModel = {};
    BaseviewModel.CurrentUserID = currentUser.CurrentUserID;
    BaseviewModel.CurrentCulture = currentCulture;


    this.init = function () {
        GetAllPenalty();
    };

    function GetAllPenalty() {
        ajaxService.AjaxPostWithData(BaseviewModel, "penalty/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("penaltyAdded", function (event, response) {
        GetAllPenalty();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllPenalty();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditPenalty",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.penalty = { ApplicationId: "", CategoryName: "", Name: "", Score: "", Contents: "", Response: "", IsActive: false };
                        return this.penalty;
                    }
                    else {
                        this.penalty = obj;
                        return this.penalty;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for bank pop up window

angular.module('hiworkApp').component('addEditPenalty', {
    templateUrl: 'App/Components/Penalty/Template/addEditPenalty.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addPenaltyController
});

function addPenaltyController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    var baseViewModel = {};
    baseViewModel.CurrentUserID = currentUser.CurrentUserID;
    baseViewModel.CurrentCulture = currentCulture;

    $ctrl.getConfig = function () {
        ajaxService.AjaxPostWithData(baseViewModel, "penalty/config", $ctrl.onGetConfigData, $ctrl.onGetError);
    }


    $ctrl.onGetConfigData = function (response) {
        $ctrl.applicationList = [];
        $ctrl.categoryList = [];
        $ctrl.applicationList = response.ApplicationList;
        $ctrl.categoryList = response.PenaltyCategoryList;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.SavePenalty = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.ApplicationId || $ctrl.modalData.ApplicationId == "" ||
            !$ctrl.modalData.CategoryNo || $ctrl.modalData.CategoryNo == "" ||
            !$ctrl.modalData.Name || $ctrl.modalData.Name == "" ||
            !$ctrl.modalData.Contents || $ctrl.modalData.Contents == "" ||
            !$ctrl.modalData.Response || $ctrl.modalData.Response == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "penalty/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("penaltyAdded", response);
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

        $ctrl.getConfig();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

