
//Done by Tamal
//For parent staff Technical Fields Component

angular.module("hiworkApp").component('masterstfields', {

    templateUrl: 'app/Components/MasterStaffTechnicalFields/Template/MasterStaffTechnicalFieldsList.html',
    controller: masterstfController

})

function masterstfController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.masterstfclass = { Name: "", Code: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.mstfieldsColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllMSTFList();
    };

    function GetAllMSTFList() {
        var masterstf = {};
        masterstf.CurrentUserID = currentUser.CurrentUserID;
        masterstf.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(masterstf, "mstechfields/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.mstfieldsColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("masterstfAdded", function (event, response) {
        GetAllMSTFList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMSTFList();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.mstfieldsColl = null;
        $scope.mstfieldsColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addmasterETF",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.masterstfclass = { Name: "", Code: "", IsActive: false };
                        return this.masterstfclass;
                    }
                    else {
                        this.masterstfclass = obj;
                        return this.masterstfclass;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for Master Staff Technical Fields

angular.module('hiworkApp').component('addmasterETF', {
    templateUrl: 'app/Components/MasterStaffTechnicalFields/Template/addEditMasterStaffTechnicalFields.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterstfController
});

function addmasterstfController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);



    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveMSTFC = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "") {
            return;
        }



        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        ajaxService.AjaxPostWithData($ctrl.modalData, "mstechfields/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("masterstfAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving mastere trans fields');
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

