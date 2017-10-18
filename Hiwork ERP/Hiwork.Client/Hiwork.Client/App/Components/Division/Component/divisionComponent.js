

/* ******************************************************************************************************************
 * AngularJS 1.5 Component for Company_Division Entity
 * Date             :   Unknown
 * By               :   Ashis
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two component to be programmed
 * First Component  :   Parent component which will be responsible to list all categories in a table to user
 * Second Component :   Child component which will be responsible for Add/Edit specific category for an user
 * *****************************************************************************************************************/





var onErrorToastMessage = function (message) {
    if (!message) {
        message = "Failed to connect with HiWork Web Server";
    }
    toastr.error(message);
};



// Parent Component

angular.module("hiworkApp").component('divisions', {

    templateUrl: 'app/Components/Division/Template/divisionList.html',
    controller: ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', divisionController]
});


function divisionController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.division = { Name: "", Code: "", CountryId: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    //if (loginFactory.IsAuthenticated() == false)
    //{
    //    $state.go("login");
    //}

    this.$onInit = function () {
        GetAllDivisions();
    };

    function GetAllDivisions() {
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "division/list", onGetData, onErrorToastMessage);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    $scope.$on("divisionAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    });
    
    $scope.$on("dataDeleted", function (event, response) {
        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    //$scope.searchText = function () {
    //    //var value = angular.element('#search').val();
    //    var value = $scope.searchkey;
    //    var data = $filter('filter')($scope.rowCollection, value);
    //    $scope.displayColl = null;
    //    $scope.displayColl = [].concat(data);
    //};

    
    this.open = function (obj, title) {
        var binding = {};
        binding.component = "addEditDivision";
        binding.resolve = {};
        binding.resolve.modalData = obj == null ? this.division : obj;
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };
}


//for Division pop up window

angular.module('hiworkApp').component('addEditDivision', {

    templateUrl: 'app/Components/Division/Template/addEditDivision.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', addDivisionController]
});


function addDivisionController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getCountries = function () {
        $ctrl.enableSave = false;
        var BaseModel = {};
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "country/list", onGetData, onErrorToastMessage);
    };

    var onGetData = function (response) {
        $ctrl.originalCountry = [];
        $ctrl.originalCountry = response;
        $ctrl.filterCountry();
        $ctrl.enableSave = true;
    };

    $ctrl.filterCountry = function () {
        $ctrl.countryList = [];
        var logic = function (arg_country) {
            if (arg_country.IsActive == false) {
                if ($ctrl.modalData.CountryId == arg_country.Id) {
                    $ctrl.modalData.CountryId = null;
                }
            }
            return arg_country.IsActive;
        };
        $ctrl.countryList = _.filter($ctrl.originalCountry, logic);
    };

    $ctrl.saveDivision = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "")
            return;
        if ($ctrl.modalData.Name.length > 100) {
            toastr.error($filter('translator')('ERRORLENGTHNAME'));
            return;
        }
        if ($ctrl.modalData.Code.length > 5) {
            toastr.error($filter('translator')('ERRORLENGTH'));
            return;
        }

        if (!$ctrl.modalData.CountryId || $ctrl.modalData.CountryId == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "division/save", successOnSaving, onErrorToastMessage);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("divisionAdded", response);
        $ctrl.Close();
        toastr.success($filter('translator')('DATASAVED'));
    };

    $ctrl.$onInit = function () {
        $ctrl.enableSave = false;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        $ctrl.getCountries();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

