//For parent staffknowledgefield list
angular.module("hiworkApp").component('staffknowledgefields', {
    templateUrl: 'App/Components/StaffKnowledgeField/Template/staffKnowledgeFieldList.html',
    controller: staffKnowledgeFieldController
})

function staffKnowledgeFieldController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.staffKnowledgeField = { Name: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllstaffKnowledgeFields();
    };
    function GetAllstaffKnowledgeFields() {
        var staffKnowledgeField = {};
        staffKnowledgeField.CurrentUserID = currentUser.CurrentUserID;
        staffKnowledgeField.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(staffKnowledgeField, "staffknowledgefield/list", onGetData, onGetError);
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
    $scope.$on("staffKnowledgeFieldAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditStaffKnowledgeField",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.staffKnowledgeField = { Name: "", Description: "", IsActive: false };
                        return this.staffKnowledgeField;
                    }
                    else {
                        this.staffKnowledgeField = obj;
                        return this.staffKnowledgeField;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for staffKnowledgeField pop up window
angular.module('hiworkApp').component('addEditStaffKnowledgeField', {
    templateUrl: 'App/Components/StaffKnowledgeField/Template/addEditStaffKnowledgeField.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addStaffKnowledgeFieldController
});

function addStaffKnowledgeFieldController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveStaffKnowledgeField = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "staffknowledgefield/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("staffKnowledgeFieldAdded", response);
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