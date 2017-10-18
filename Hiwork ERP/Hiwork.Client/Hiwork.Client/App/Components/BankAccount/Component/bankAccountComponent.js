//For  bank Account list
angular.module("hiworkApp").component('brankaccount', {
    templateUrl: 'App/Components/BankAccount/Template/bankAccountList.html',
    controller: bankAccountController
});


function bankAccountController($scope, $uibModal, appSettings, AppStorage, sessionFactory,loginFactory, $filter, ajaxService,$state) {
    this.bankAccount = { BankID: "", BankBranchID: "", AccountTypeID: "", AccountNo: "", AccountName: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    if (!loginFactory.IsAuthenticated())
        $state.go("login");

    this.init = function () {
        GetAllBankAccount();
    };

    function GetAllBankAccount() {
        var bankAccount = {};
        bankAccount.CurrentUserID = currentUser.CurrentUserID;
        bankAccount.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bankAccount, "bankaccount/list", onGetData, onGetError);
    }


    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("bankAccAdded", function (event, response) {
  
        GetAllBankAccount();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllBankAccount();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditBankAccount",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.bankaccount = { BankID: "", BankBranchID: "", AccountTypeID: "", AccountNo: "", AccountName: "", IsActive: false };
                        return this.bankaccount;
                    }
                    else {
                        this.bankaccount = obj;
                        return this.bankaccount;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for bank Account pop up window

angular.module('hiworkApp').component('addEditBankAccount', {
    templateUrl: 'App/Components/BankAccount/Template/addEditBankAccount.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addBankAccountController
});

function addBankAccountController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getBanks = function () {
        var bank = {};
        bank.CurrentUserID = currentUser.CurrentUserID;
        bank.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bank, "bank/list", $ctrl.onGetData, $ctrl.onGetError);
    }
    $ctrl.getBankBranch = function () {
        var bankBranch = {};
        bankBranch.CurrentUserID = currentUser.CurrentUserID;
        bankBranch.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bankBranch, "bankbranch/list", $ctrl.onGetDataCur, $ctrl.onGetError);
    }
    $ctrl.getBankAccType = function () {
        var bankAccType = {};
        bankAccType.CurrentUserID = currentUser.CurrentUserID;
        bankAccType.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(bankAccType, "bankAccountType/list", $ctrl.onGetDataAccType, $ctrl.onGetError);
    }


    $ctrl.onGetDataCur = function (response) {
        $ctrl.bankBranchList = [];
        $ctrl.bankBranchList = response;
    };

    $ctrl.onGetData = function (response) {
        $ctrl.bankList = [];
        $ctrl.bankList = response;
    };

    $ctrl.onGetDataAccType = function (response) {
        $ctrl.bankAccountTypeList = [];
        $ctrl.bankAccountTypeList = response;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.Save = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.BankID || !$ctrl.modalData.BankBranchID || !$ctrl.modalData.AccountTypeID || !$ctrl.modalData.AccountNo || !$ctrl.modalData.AccountName ||
            $ctrl.modalData.BankID == "" || $ctrl.modalData.BankBranchID == "" || $ctrl.modalData.AccountTypeID == "" || $ctrl.modalData.AccountNo == "" || $ctrl.modalData.AccountName == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "bankaccount/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("bankAccAdded", response);
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
        $ctrl.getBankBranch();
        $ctrl.getBankAccType();
    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

