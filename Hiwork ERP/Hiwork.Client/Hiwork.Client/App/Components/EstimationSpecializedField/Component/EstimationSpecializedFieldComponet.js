
//Done by Tamal
//For parent staff Master Estimation SpecializedField 

angular.module("hiworkApp").component('mesfields', {

    templateUrl: 'app/Components/EstimationSpecializedField/Template/MasterEstimationSpecializedFieldList.html',
    controller: mesFieldsController

})

function mesFieldsController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.masterestimationsfields = { Name: "", Code: "", IsLightPrice: true, IsBusinessPrice: true, IsExpertPrice: true, IsActive: false };


    $scope.rowCollection = [];
    $scope.mesfColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllMESFList();
    };

    function GetAllMESFList() {
        var masterestimationsf = {};
        masterestimationsf.CurrentUserID = currentUser.CurrentUserID;
        masterestimationsf.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(masterestimationsf, "estSpecializedFields/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.mesfColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("masterestimationsfAdded", function (event, response) {
        GetAllMESFList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMESFList();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.mesfColl = null;
        $scope.mesfColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addmasterEstimationSF",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.masterestimationsfields = { Name: "", Code: "", IsActive: false };
                        return this.masterestimationsfields;
                    }
                    else {
                        this.masterestimationsfields = obj;
                        return this.masterestimationsfields;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for MasterEstimation Specialized Field Add Edit pop up window

angular.module('hiworkApp').component('addmasterEstimationSF', {
    templateUrl: 'app/Components/EstimationSpecializedField/Template/addEditMasterEstimationSpecializedField.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterestimationsfController
});

function addmasterestimationsfController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);



    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveMESF = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "") {
            return;
        }

        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        ajaxService.AjaxPostWithData($ctrl.modalData, "estSpecializedFields/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("masterestimationsfAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving data');
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

