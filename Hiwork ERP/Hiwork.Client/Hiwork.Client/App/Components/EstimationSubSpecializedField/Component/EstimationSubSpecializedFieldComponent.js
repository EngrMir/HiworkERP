
//Done by Tamal
//For parent Estimation Sub Specialized Field deatils list

angular.module("hiworkApp").component('estisubspecializedfield', {

    templateUrl: 'app/Components/EstimationSubSpecializedField/Template/SubSpecializedFieldList.html',
    controller: EstiSubSpecialController

})

function EstiSubSpecialController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state) {

    this.EstiSubSp = { Name: "", Code: "", SpecializedField: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.estiSubSpcoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllEstiSubSpecialized();
    };

    function GetAllEstiSubSpecialized() {
        var estiSubSpecializedDetails = {};
        estiSubSpecializedDetails.CurrentUserID = currentUser.CurrentUserID;
        estiSubSpecializedDetails.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(estiSubSpecializedDetails, "estimationssf/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.estiSubSpcoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("estimationSSfieldadded", function (event, response) {
        GetAllEstiSubSpecialized();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllEstiSubSpecialized();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.estiSubSpcoll = null;
        $scope.estiSubSpcoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addestimationSubSpecialized",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.EstiSubSp = { Name: "", Code: "", SpecializedField: "", IsActive: false };
                        return this.EstiSubSp;
                    }
                    else {
                        this.EstiSubSp = obj;
                        return this.EstiSubSp;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for  Estimation Sub Specialized list pop up window

angular.module('hiworkApp').component('addestimationSubSpecialized', {
    templateUrl: 'app/Components/EstimationSubSpecializedField/Template/addEditSubSpecializedField.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addessubSpController
});

function addessubSpController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);


    $ctrl.getSpecificField = function () {
        var EstimationSpecializedField = {};
        EstimationSpecializedField.CurrentUserID = currentUser.CurrentUserID;
        EstimationSpecializedField.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(EstimationSpecializedField, "estSpecializedFields/list", $ctrl.onGetData, $ctrl.onGetError);

    }
    $ctrl.onGetData = function (response) {
        $ctrl.mecList = [];
        $ctrl.mecList = response;
    };

    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.saveSubSpecilizedField = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Code ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Code == "") {
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

        if (!$ctrl.modalData.SpecializedField || $ctrl.modalData.SpecializedField == "")
            return;
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;

        ajaxService.AjaxPostWithData($ctrl.modalData, "estimationssf/save", successOnSaving, errorOnSaving);
    }

    var successOnSaving = function (response) {
        $rootScope.$broadcast("estimationSSfieldadded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorOnSaving = function (message) {

        toastr.error('Error in saving Estimation Sub Specialized');
    };

    $ctrl.$onInit = function () {
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;


        $ctrl.getSpecificField();

    }

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}
