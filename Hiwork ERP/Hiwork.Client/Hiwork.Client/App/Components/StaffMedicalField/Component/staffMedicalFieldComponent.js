//For parent StaffMedicalField list
angular.module("hiworkApp").component('staffmedicalfield', {
    templateUrl: 'App/Components/StaffMedicalField/Template/staffMedicalFieldList.html',
    controller: staffMedicalFieldController
})

function staffMedicalFieldController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.staffMedicalField = { Name: "", Description: "", Type: 0, IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllStaffMedicalFields();
    };
    function GetAllStaffMedicalFields() {
        var staffMedicalField = {};
        staffMedicalField.CurrentUserID = currentUser.CurrentUserID;
        staffMedicalField.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(staffMedicalField, "staffmedicalfield/list", onGetData, onGetError);
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
    $scope.$on("staffMedicalFieldAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addStaffMedicalField",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.StaffMedicalField = { Name: "", Description: "", Type: 0, IsActive: false };
                        return this.StaffMedicalField;
                    }
                    else {
                        this.StaffMedicalField = obj;
                        return this.StaffMedicalField;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for StaffMedicalField pop up window
angular.module('hiworkApp').component('addStaffMedicalField', {
    templateUrl: 'App/Components/StaffMedicalField/Template/addStaffMedicalField.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addStaffMedicalFieldController
});

function addStaffMedicalFieldController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveStaffMedicalField = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "staffmedicalfield/save", successOnSaving, errorOnSaving);
    }
    $ctrl.types = [{ ID: 0, Name: 'All' }, { ID: 1, Name: 'Transpro' }, { ID: 2, Name: 'EditingPro' }];
    var successOnSaving = function (response) {
        $rootScope.$broadcast("staffMedicalFieldAdded", response);
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