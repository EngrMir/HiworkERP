
//Done by Tamal
//For parent staff Major Subject] list

angular.module("hiworkApp").component('staffmajorsubjects', {

    templateUrl: 'app/Components/StaffMajorSubject/Template/StaffMajorSubList.html',
    controller: staffmajorsubController

})

function staffmajorsubController($scope, $uibModal, appSettings, AppStorage, sessionFactory,loginFactory, $filter, ajaxService,$state) {

    this.staffmajorsub = { Name: "", Code: "", IsActive: false };

  
    $scope.rowCollection = [];
    $scope.staffmajorsubcoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }


    this.init = function () {
        GetAllMajorSubjectList();
    };

    function GetAllMajorSubjectList() {
        var staffmajorsub = {};
        staffmajorsub.CurrentUserID = currentUser.CurrentUserID;
        staffmajorsub.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(staffmajorsub, "staffmajorsub/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.staffmajorsubcoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("staffmajorsubAdded", function (event, response) {
        GetAllMajorSubjectList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMajorSubjectList();
    });
    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.staffmajorsubcoll = null;
        $scope.staffmajorsubcoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addStaffMajorsubs",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.staffmajorsub = { Name: "", Code: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.staffmajorsub;
                    }
                    else {
                        this.staffmajorsub = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.staffmajorsub;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for staff Major Subject list pop up window

angular.module('hiworkApp').component('addStaffMajorsubs', {
    templateUrl: 'app/Components/StaffMajorSubject/Template/addEditStaffMajorSub.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addstaffmajorsubController
});

function addstaffmajorsubController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

   

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveSubject = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "") {
            return;

        }

        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }
        ajaxService.AjaxPostWithData($ctrl.modalData, "staffmajorsub/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("staffmajorsubAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving staffmajorsub');
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

