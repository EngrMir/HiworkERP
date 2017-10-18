
//Done by Tamal
//For parent staff Master Estimation Routes 

angular.module("hiworkApp").component('meroutes', {

    templateUrl: 'app/Components/EstimationRoutes/Template/MasterEstimationRoutesList.html',
    controller: meroutesController

})

function meroutesController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService,$state) {

    this.masterestimationroutes = { Name: "", Code: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.merColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllMERList();
    };

    function GetAllMERList() {
        var masterestimationroutes = {};
        masterestimationroutes.CurrentUserID = currentUser.CurrentUserID;
        masterestimationroutes.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(masterestimationroutes, "estimationRoute/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.merColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("masterestimationroutesAdded", function (event, response) {
        GetAllMERList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMERList();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.merColl = null;
        $scope.merColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addmasterEstimationRoutes",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.masterestimationroutes = { Name: "", Code: "", IsActive: false };
                        return this.masterestimationroutes;
                    }
                    else {
                        this.masterestimationroutes = obj;
                        return this.masterestimationroutes;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for MasterEstimationRoutes Add Edit pop up window

angular.module('hiworkApp').component('addmasterEstimationRoutes', {
    templateUrl: 'app/Components/EstimationRoutes/Template/addEditMasterEstimationRoutes.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterestimationroutesController
});

function addmasterestimationroutesController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);



    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveMER = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "") {
            return;
        }



        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        ajaxService.AjaxPostWithData($ctrl.modalData, "estimationRoute/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("masterestimationroutesAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving masterestimationRoutes');
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

