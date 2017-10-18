//For parent companyBusiness list
angular.module("hiworkApp").component('agentbusiness', {
    templateUrl: 'app/Components/AgentBusiness/Template/agentBusinessList.html',
    controller: agentBusinessController
})

function agentBusinessController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.agentbusiness = { Code: "", Name: "", IsActive: false, CurrentCulture: "" };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    this.agentbusiness.CurrentCulture = currentCulture;
    this.init = function () {
        GetAllAgentBusiness();
    };
    function GetAllAgentBusiness() {
        var agentbusiness = {};
        agentbusiness.CurrentUserID = currentUser.CurrentUserID;
        agentbusiness.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(agentbusiness, "agentbusiness/list", onGetData, onGetError);
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
    $scope.$on("AgentBusinessAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditAgentBusiness",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.agentbusiness = { Code: "", Name: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.agentbusiness;
                    }
                    else {
                        this.agentbusiness = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.agentbusiness;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for companyBusiness pop up window
angular.module('hiworkApp').component('addEditAgentBusiness', {
    templateUrl: 'app/Components/AgentBusiness/Template/addEditAgentBusiness.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addEditAgentBusinessController
});

function addEditAgentBusinessController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveAgentBusiness = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "agentbusiness/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("AgentBusinessAdded", response);
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