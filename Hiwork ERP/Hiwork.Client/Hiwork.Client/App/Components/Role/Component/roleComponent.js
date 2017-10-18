//For parent role list
angular.module("hiworkApp").component('roles', {
    templateUrl: 'app/Components/Role/Template/roleList.html',
    controller: roleController
})

function roleController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.role = { Name: "", Description: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
   
    //if (!loginFactory.IsAuthenticated())
    //    $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllRoles();
    };
    function GetAllRoles() {
        var role = {};
        role.CurrentUserID = currentUser.CurrentUserID;
        role.CurrentCulture = currentCulture;      
        ajaxService.AjaxPostWithData(role, "role/list", onGetData, onGetError);
    }
    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };
    var onGetError = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    };
    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };
    $scope.$on("roleAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
       
    });
    $scope.$on("dataDeleted", function (event, response) {
      
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    
    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditRole",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.role = { Name: "", Description: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.role;
                    }
                    else {
                        this.role = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.role;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for Role pop up window
angular.module('hiworkApp').component('addEditRole', {       
        templateUrl: 'app/Components/Role/Template/addEditRole.html',
        bindings: {        
            modalInstance: "<",
            resolve: "<"
        },   
        controller: addRoleController
});

function addRoleController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveRole = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "role/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("roleAdded", response);
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