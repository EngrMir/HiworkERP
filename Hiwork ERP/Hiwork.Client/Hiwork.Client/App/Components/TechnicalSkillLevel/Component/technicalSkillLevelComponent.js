//For parent Technical Skill Level list
angular.module("hiworkApp").component('technicalskilllevel', {
    templateUrl: 'App/Components/TechnicalSkillLevel/Template/technicalSkillLevelList.html',
    controller: technicalskilllevelController
})

function technicalskilllevelController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {
    this.technicalskilllevel = { Code: "", Name: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
   // this.technicalskilllevel.CurrentCulture = currentCulture;
    this.init = function () {
        GetAllTechnicalSkillLevel();
    };
    function GetAllTechnicalSkillLevel() {
        var technicalskilllevel = {};
        technicalskilllevel.CurrentUserID = currentUser.CurrentUserID;
        technicalskilllevel.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(technicalskilllevel, "technicalskilllevel/list", onGetData, onGetError);
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
    $scope.$on("technicalSkillLevelAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditTechnicalSkillLevel",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.technicalskilllevel = { Code: "", Name: "", IsActive: false };
                        return this.technicalskilllevel;
                    }
                    else {
                        this.technicalskilllevel = obj;
                        return this.technicalskilllevel;
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
angular.module('hiworkApp').component('addEditTechnicalSkillLevel', {
    templateUrl: 'App/Components/TechnicalSkillLevel/Template/addEditTechnicalSkillLevel.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addEdittechnicalskilllevelController
});

function addEdittechnicalskilllevelController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveTechnicalSkillLevel = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "" || !$ctrl.modalData.Code || $ctrl.modalData.Code == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "technicalskilllevel/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("technicalSkillLevelAdded", response);
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