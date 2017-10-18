

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Application Entity
 * Date             :   14-July-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/




// Parent Component

angular.module("hiworkApp").component('application', {

    templateUrl: 'app/Components/Application/Template/applicationList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', appController]
});


function appController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.application = { Name: "", Code: "", Description: "", Logo: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.$onInit = function () {
        GetAllApplications();
    };

    function GetAllApplications() {
        let BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "app/list", onSuccess, onErrorToastMessage);
    }

    var onSuccess = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    };

    $scope.$on("applicationAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    });

    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = new Array().concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditApplication",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.application = { Name: "", Code: "", Description: "", Logo: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.application;
                    }
                    else {
                        this.application = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.application;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}



// Child component for Add/Edit popup dialog

angular.module("hiworkApp").component('addEditApplication', {

    templateUrl: 'app/Components/Application/Template/addEditApplication.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addEditApplication]
});


function addEditApplication($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.SaveApplication = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "") {
            return;
        }
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData($ctrl.modalData, "app/save", n_onSuccess, onErrorToastMessage);
    };

    var n_onSuccess = function (response) {
        $rootScope.$broadcast("applicationAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}