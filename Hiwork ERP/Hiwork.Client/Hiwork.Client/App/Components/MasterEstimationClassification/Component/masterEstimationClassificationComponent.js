
//Done by Tamal
//For parent staff Master Estimation Classification Subject list

angular.module("hiworkApp").component('meclassifications', {

    templateUrl: 'app/Components/MasterEstimationType/Template/EstimationTypeList.html',
    controller: meclassificationController

})

function meclassificationController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.masterestimationclass = { Name: "", Code: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.mecColl = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllMECList();
    };

    function GetAllMECList() {
        var masterestimationclass = {};
        masterestimationclass.CurrentUserID = currentUser.CurrentUserID;
        masterestimationclass.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(masterestimationclass, "EstimationType/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.mecColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("masterestimationclassAdded", function (event, response) {
        GetAllMECList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMECList();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.mecColl = null;
        $scope.mecColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addmasterEstimationClass",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.masterestimationclass = { Name: "", Code: "", IsActive: false };
                        return this.masterestimationclass;
                    }
                    else {
                        this.masterestimationclass = obj;
                        return this.masterestimationclass;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for EstimationType Add Edit pop up window

angular.module('hiworkApp').component('addmasterEstimationClass', {
    templateUrl: 'app/Components/EstimationType/Template/addEditEstimationType.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addmasterestimationclassController
});

function addmasterestimationclassController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);



    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveMEC = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "")
        {
            return;
        }
            


        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        ajaxService.AjaxPostWithData($ctrl.modalData, "EstimationType/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("masterestimationclassAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving masterestimationclass');
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

