
//Done by Tamal
//For parent staff Master Estimation ServiceType 

angular.module("hiworkApp").component('meservicetypes', {

    templateUrl: 'app/Components/EstimationServiceType/Template/MasterEstimationServiceTypeList.html',
    controller: meservicetypeController

})

function meservicetypeController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.masterestimationservicetypes = { Name: "", Code: "", Type: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.meservicecoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllMEServiceType();
    };

    function GetAllMEServiceType() {
        var masterestimationservicetypes = {};
        masterestimationservicetypes.CurrentUserID = currentUser.CurrentUserID;
        masterestimationservicetypes.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(masterestimationservicetypes, "estimationServiceType/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.meservicecoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("masterestimationservicetypeAdded", function (event, response) {
        GetAllMEServiceType();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMEServiceType();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.meservicecoll = null;
        $scope.meservicecoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addmasterEstimationServiceType",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.masterestimationservicetypes = { Name: "", Code: "", Type: "", IsActive: false };
                        return this.masterestimationservicetypes;
                    }
                    else {
                        this.masterestimationservicetypes = obj;
                        return this.masterestimationservicetypes;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for MasterEstimationServiceType Add Edit pop up window

angular.module('hiworkApp').component('addmasterEstimationServiceType', {
    templateUrl: 'app/Components/EstimationServiceType/Template/addEditMasterEstimationServiceType.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterestimationservicetypeController
});

function addmasterestimationservicetypeController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $http) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    init();
    function init() {
        var objClass = {};
        objClass.CurrentUserID = currentUser.CurrentUserID;
        objClass.CurrentCulture = currentCulture;
        $http({ method: 'POST', url: appSettings.API_BASE_URL + "estimationType/list", data: objClass }).
            then(function (response) {
                $ctrl.estimationCategories = response.data;
            },function(error){

            });
    }

   
    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };
    $ctrl.typeChanged = function (selectType) {
        $ctrl.modalData.Type = selectType;
    };
    $ctrl.saveMEST = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code || !$ctrl.modalData.Type ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "" || $ctrl.modalData.Type == "") {
            return;
        }
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        ajaxService.AjaxPostWithData($ctrl.modalData, "estimationServiceType/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("masterestimationservicetypeAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving masterestimation ServiceType');
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.Type = $ctrl.modalData.EstimationType ? $ctrl.modalData.EstimationType.ID : 0;
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

