//For  bank Branch list
angular.module("hiworkApp").component('brankbranch', {
    templateUrl: 'App/Components/BankBranch/Template/bankBranchList.html',
    controller: bankBranchController
});


function bankBranchController($scope, $uibModal, appSettings, AppStorage, loginFactory, sessionFactory, $filter, ajaxService,$state) {
    this.bankbranch = { Name: "",BankID:"", Code: "",SwiftCode:"", Address: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    this.init = function () {
        GetAllBankBranch();
    };

    function GetAllBankBranch() {
        var bankbranch = {};
        bankbranch.CurrentUserID = currentUser.CurrentUserID;
        bankbranch.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bankbranch, "bankbranch/list", onGetData, onGetError);
    }


    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("bankBranchAdded", function (event, response) {  
        GetAllBankBranch();
    });

    $scope.$on("dataDeleted", function (event, response) { 
        GetAllBankBranch();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditBankBranch",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.bankBranch = { Name: "", BankID: "", Code: "", SwiftCode: "", Address: "", IsActive: false };
                        return this.bankBranch;
                    }
                    else {
                        this.bankBranch = obj;
                        return this.bankBranch;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for bank branch pop up window

angular.module('hiworkApp').component('addEditBankBranch', {
    templateUrl: 'App/Components/BankBranch/Template/addEditBankBranch.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addBankBranchController
});

function addBankBranchController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);


    $ctrl.getBanks = function () {
        var bank = {};
        bank.CurrentUserID = currentUser.CurrentUserID;
        bank.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bank, "bank/list", $ctrl.onGetBankData, $ctrl.onGetError);
    }


    //$ctrl.onGetDataCur = function (response) {
    //    $ctrl.currencyList = [];
    //    $ctrl.currencyList = response;
    //};

    $ctrl.onGetBankData = function (response) {
        $ctrl.BankList = [];
        $ctrl.BankList = response;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.Save = function () {
        $scope.isTriedSave = true;
        if ( !$ctrl.modalData.Code || !$ctrl.modalData.SwiftCode ||
            $ctrl.modalData.SwiftCode == "" || $ctrl.modalData.Code == "" ||
            !$ctrl.modalData.Address || !$ctrl.modalData.BankID || $ctrl.modalData.BankID == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "bankbranch/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("bankBranchAdded", response);
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

        $ctrl.getBanks();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

