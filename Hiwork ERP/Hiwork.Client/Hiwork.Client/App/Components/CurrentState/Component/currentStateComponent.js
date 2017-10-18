//For parent currentstate list
angular.module("hiworkApp").component('currentstates', {
    templateUrl: 'app/Components/CurrentState/Template/currentStateList.html',
    controller: currentstateController
})

function currentstateController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService) {
    this.currentstate = { Name: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllCurrentstates();
    };
    function GetAllCurrentstates() {
        var currentstate = {};
        currentstate.CurrentUserID = currentUser.CurrentUserID;
        currentstate.CurrentCulture = currentCulture;      
        ajaxService.AjaxPostWithData(currentstate, "currentstate/list", onGetData, onGetError);
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
    $scope.$on("currentstateAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
       
    });
    $scope.$on("dataDeleted", function (event, response) {
      
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    
    this.open = function (obj, title) {        
        $uibModal.open({
            component: "addEditCurrentState",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.currentstate = { Name: "", Description: "", IsActive: false };
                        return this.currentstate;
                    }
                    else {
                        this.currentstate = obj;
                        return this.currentstate;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for currentstate pop up window
angular.module('hiworkApp').component('addEditCurrentState', {
    templateUrl: 'app/Components/CurrentState/Template/addEditCurrentState.html',
        bindings: {        
            modalInstance: "<",
            resolve: "<"
        },   
        controller: addCurrentStateController
});

function addCurrentStateController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveCurrentState = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "currentstate/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("currentstateAdded", response);
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