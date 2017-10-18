angular.module('hiworkApp').component('projectinitiation', {
    templateUrl: 'App/Components/ProjectInitiation/Template/projectinitTemplate.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: projectInitiationController
});

projectInitiationController.$inject = ['$scope', '$rootScope', 'appSettings', 'AppStorage', 'sessionFactory', '$filter', 'ajaxService', '$http', '$uibModal'];
function projectInitiationController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $http, $uibModal) {

    var $ctrl = this;
    $ctrl.modalData = {};
    var baseViewModel = {}
    var EstimationList = [];
    $ctrl.dataList = [];
    var Count = 0;

    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);

    $ctrl.getEstimation = function () {
        ajaxService.AjaxPostWithData(baseViewModel, "estimation/projectlist", onGetData, onGetError);
    }
    $ctrl.GetEstimationProjectNextId = function () {
        ajaxService.AjaxPostWithData(baseViewModel, "estimationproject/nextId", onGetNextId, onGetError);
    }
    var onGetNextId = function (response) {
        $ctrl.modalData.ProjectNo = response;
    }
    var onGetData = function (response) {
        $ctrl.dataList = [];
        $ctrl.dataList = response;
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $ctrl.$onInit = function () {
        debugger
        baseViewModel.CurrentUserID = currentUser.CurrentUserID;
        baseViewModel.CurrentCulture = currentCulture;
        baseViewModel.ApplicationId = appSettings.ApplicationId;
        $ctrl.title = $ctrl.resolve.title;
        $ctrl.isReadOnly = $ctrl.resolve.isReadOnly;
        $ctrl.GetEstimationProjectNextId();
        $ctrl.getEstimation();
    };


    $("#allselect").on('change', function () {
        if ($(this).is(':checked')) {
            $(this).attr('value', 'true');
        } else {
            $(this).attr('value', 'false');
        }

        $('#checkbox-value').text($('#checkbox1').val());
    });


    $ctrl.allItemsSelectionChanged = function (chk) {
        EstimationList = [];
        var item = document.getElementsByID("item");
        if ($('#allselect').prop('checked')) {
            for (var i = 0; i < item.length; i++) {
                item[i].setAttribute("checked", "checked");
                var ID = $ctrl.dataList[i].ID;
                EstimationList.push(ID);
            }
        } else {
            EstimationList = [];
            for (var i = 0; i < item.length; i++) {
                item[i].removeAttribute("checked");

            }
        }


    }
    $ctrl.addEstimation = function (ID) {

        var idx = EstimationList.indexOf(ID);
        if (idx == -1) {
            EstimationList.push(ID);
            Count++;

        } else {
            EstimationList.splice(ID, 1);
            Count--;
        }
        CompareEsitmationList();
    }

    function CompareEsitmationList() {
        if (Count == $ctrl.dataList.length) {
            $('#allselect').prop('checked', true);
        }
        else if ($('#allselect').prop('checked')) {
            $('#allselect').prop('checked', false);
        }
    }

    $ctrl.Save = function () {
        if (!$ctrl.modalData.Name || $ctrl.modalData.Name == "" || EstimationList.length <= 0)
            return;
        $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
        $ctrl.modalData.CurrentCulture = currentCulture;
        $ctrl.modalData.ApplicationId = appSettings.ApplicationId;
        $ctrl.modalData.EstimationList = EstimationList;
        ajaxService.AjaxPostWithData($ctrl.modalData, "estimationproject/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        toastr.success('Project creation success');
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    var errorOnSaving = function (message) {
        toastr.error('Project creation failed');
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}