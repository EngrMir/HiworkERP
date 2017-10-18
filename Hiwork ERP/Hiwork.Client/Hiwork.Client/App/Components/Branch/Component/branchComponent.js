
//Done by Tamal
//For parent branch list

angular.module("hiworkApp").component('branches', {

    templateUrl: 'app/Components/Branch/Template/branchList.html',
    controller: branchController

})


function branchController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.branch = { Name: "", Code: "", CountryId: "", IsActive: false };

    
    $scope.rowCollection = [];
    $scope.branchcoll = [];
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllBranches();
    };

    function GetAllBranches() {
        var branch = {};
        branch.CurrentUserID = currentUser.CurrentUserID;
        branch.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(branch, "branch/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.branchcoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("branchAdded", function (event, response) {
      GetAllBranches();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllBranches();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.branchcoll = null;
        $scope.branchcoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditBranch",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.branch = { Name: "", Code: "", CountryId: "", IsActive: false };
                        return this.branch;
                    }
                    else {
                        this.branch = obj;
                        return this.branch;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for branch pop up window

angular.module('hiworkApp').component('addEditBranch', {
    templateUrl: 'app/Components/Branch/Template/addEditBranch.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addBranchController
});

function addBranchController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getCountries = function () {
        var country = {};
        country.CurrentUserID = currentUser.CurrentUserID;
        country.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(country, "country/list", $ctrl.onGetData, $ctrl.onGetError);
    }

    $ctrl.onGetData = function (response) {
        $ctrl.countryList = [];
        $ctrl.countryList = response;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveBranch = function () {
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
        if (!$ctrl.modalData.CountryId || $ctrl.modalData.CountryId == "")
            return;
        ajaxService.AjaxPostWithData($ctrl.modalData, "branch/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("branchAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving branch');
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

        $ctrl.getCountries();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

