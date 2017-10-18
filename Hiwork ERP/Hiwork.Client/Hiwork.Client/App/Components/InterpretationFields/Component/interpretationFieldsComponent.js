//For parent interpretationFields list
angular.module("hiworkApp").component('interpretationfields', {
    templateUrl: 'App/Components/InterpretationFields/Template/interpretationFieldsList.html',
    controller: interpretationFieldsController
})

function interpretationFieldsController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.interpretationFields = { Name: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllinterpretationFieldss();
    };
    function GetAllinterpretationFieldss() {
        var interpretationFields = {};
        interpretationFields.CurrentUserID = currentUser.CurrentUserID;
        interpretationFields.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(interpretationFields, "interpretationfields/list", onGetData, onGetError);
    }
    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };
    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };
    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };
    $scope.$on("interpretationFieldsAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditInterpretationFields",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.interpretationFields = { Name: "", Description: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.interpretationFields;
                    }
                    else {
                        this.interpretationFields = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.interpretationFields;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for interpretationFields pop up window
angular.module('hiworkApp').component('addEditInterpretationFields', {
    templateUrl: 'App/Components/InterpretationFields/Template/addEditInterpretationFields.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addInterpretationFieldsController
});

function addInterpretationFieldsController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveInterpretationFields = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "interpretationfields/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("interpretationFieldsAdded", response);
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