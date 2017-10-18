angular.module("hiworkApp").component('categories', {
    templateUrl: 'App/Components/Category/Template/categoryList.html',
    controller: categoryController
})


function categoryController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.category = { Name: "", Description: "", IsActive: false };

    //$scope.categorylist = [];
    $scope.rowCollection = [];
    $scope.displayColl = [];
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    if (!loginFactory.IsAuthenticated())
        $state.go("login");
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);


    this.init = function () {
        GetAllCagegories();
    };

    function GetAllCagegories() {
        var category = {};
        category.CurrentUserID = currentUser.CurrentUserID;
        category.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(category, "category/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.displayColl = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("categoryAdded", function (event, response) {
        //$scope.rowCollection = response;
        //$scope.displayColl = [].concat($scope.rowCollection);
        GetAllCagegories();
    });

    $scope.$on("dataDeleted", function (event, response) {
        //$scope.rowCollection = response;
        //$scope.displayColl = [].concat($scope.rowCollection);
        GetAllCagegories();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.displayColl = null;
        $scope.displayColl = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addEditCategory",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.category = { Name: "", Description: "", IsActive: false };
                        return this.category;
                    }
                    else {
                        this.category = obj;
                        return this.category;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for category pop up window

angular.module('hiworkApp').component('addEditCategory', {
    templateUrl: 'app/Components/Category/Template/addEditCategory.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addcategoryController
});

function addcategoryController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {
    var $ctrl = this;
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.saveCategory = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "")
            return;

        ajaxService.AjaxPostWithData($ctrl.modalData, "category/save", successOnSaving, errorOnSaving);
    }
    var successOnSaving = function (response) {
        $rootScope.$broadcast("categoryAdded", response);
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

