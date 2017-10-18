//For parent culture list
angular.module("hiworkApp").component('culture', {
    templateUrl: 'app/Components/Culture/Template/cultureList.html', 
    controller: cultureController
})

function cultureController($scope, $uibModal, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    this.role = { Name: "", Code: "", IsActive: false };
    $scope.rowCollection = [];
    $scope.displayColl = [];
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetAllCultures();
    };
    function GetAllCultures() {
        var culture = {};
        culture.CurrentUserID = currentUser.CurrentUserID;
        culture.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(culture, "culture/list", onGetData, onGetError);
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
    $scope.$on("cultureAdded", function (event, response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);

    });
    $scope.$on("dataDeleted", function (event, response) {

        $scope.rowCollection = response.data;
        $scope.displayColl = [].concat($scope.rowCollection);
    });

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditCulture",
            transclude: true,
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.culture = { Name: "", Code: "", IsActive: false };
                        return this.culture;
                    }
                    else {
                        this.culture = obj;
                        return this.culture;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };
}

//for culture pop up window
angular.module('hiworkApp').component('addEditCulture', {
    templateUrl: 'app/Components/Culture/Template/addEditCulture.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addCultureController
});

function addCultureController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveCulture = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "culture/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("cultureAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorOnSaving = function (message) {
        //Error handling goes here.
        toastr.error('Error in saving culture');
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
