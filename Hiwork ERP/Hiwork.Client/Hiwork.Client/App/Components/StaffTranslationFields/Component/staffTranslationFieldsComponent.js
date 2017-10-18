//Done By tamal
//For parent TranslationField list
angular.module("hiworkApp").component('stafftranslationfields', {
    templateUrl: 'app/Components/StaffTranslationFields/Template/StaffTranslationFieldsList.html',
    controller: stafftransfieldController

})

function stafftransfieldController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.stafftrans = {
        Name: "", Description: "", SortBy: "", IsActive: false
    };

    $scope.rowCollection = [];
    $scope.stafftransfieldColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        LoadAllStaffTranslationFields();
    };

    function LoadAllStaffTranslationFields() {
        var stafftransfields = {};
        stafftransfields.CurrentUserID = currentUser.CurrentUserID;
        stafftransfields.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(stafftransfields, "stafftranslationfields/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.stafftransfieldColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("stafftransAdded", function (event, response) {
        
        LoadAllStaffTranslationFields();
    });

    $scope.$on("dataDeleted", function (event, response) {

        LoadAllStaffTranslationFields();
    });

    $scope.searchText = function () {
        
        var value = $scope.searchkey;
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.stafftransfieldColl = null;
        $scope.stafftransfieldColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditStaff",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.stafftrans = {
                            Name: "", Description: "", SortBy: "", IsActive: false
                        };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.stafftrans;
                    }
                    else {
                        this.stafftrans = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.stafftrans;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for StaffTrans pop up window

angular.module('hiworkApp').component('addEditStaff', {
    templateUrl: 'app/Components/StaffTranslationFields/Template/addEditStaffTranslationFields.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addStaffTransController
});

function addStaffTransController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData)
    // Save //

    $ctrl.saveStaffTranslationFields = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description || !$ctrl.modalData.SortBy ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "" || $ctrl.modalData.SortBy == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "stafftranslationfields/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("stafftransAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        toastr.error('Error in saving Staff Translation Fields');
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

