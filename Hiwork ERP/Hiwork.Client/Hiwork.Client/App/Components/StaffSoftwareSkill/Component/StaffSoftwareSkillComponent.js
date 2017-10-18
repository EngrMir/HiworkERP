var app = angular.module("hiworkApp");

app.component('staffsoftwareskill', {
    templateUrl: 'app/Components/StaffSoftwareSkill/Template/StaffSoftwareSkillList.html',
   
    controller: staffSoftwareSkillController

});

function staffSoftwareSkillController($scope, $uibModal, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $state, $rootScope) {

    this.staffsoftwareskill = { Name: "", Description: "", IsActive: false };

    $scope.rowCollection = [];
    $scope.Staffsoftcoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
       
    this.init = function () {
       GetStaffSoftwareSkillList();
       };

        

        function GetStaffSoftwareSkillList() {
            var staffsoftwareskill= {};
            staffsoftwareskill.CurrentUserID = currentUser.CurrentUserID;
            staffsoftwareskill.CurrentCulture = currentCulture;
            ajaxService.AjaxPostWithData(staffsoftwareskill, "StaffSoftwareSkill/list", onGetData, onGetError);
        }

        var onGetData = function (response) {
            $scope.rowCollection = response;
            $scope.Staffsoftcoll = [].concat($scope.rowCollection);
        };

        var onGetError = function (message) {
            toastr.error('Error in getting records');
        };

        $scope.$on("staffSoftwareSkillAdded", function (event, response) {
            GetStaffSoftwareSkillList();
            
        });

        $scope.$on("dataDeleted", function (event, response) {
            GetStaffSoftwareSkillList();
        });

        this.open = function (obj, title) {
            $uibModal.open({
                component: "addEditStaffSoftwareSkill",
                resolve: {
                    modalData: function () {
                        if (obj == null) {
                            this.staffsoftwareskill = { Name: "", Description: "", IsActive: false };
                            title = $filter('translator')('ADDNEW');
                            return this.staffsoftwareskill;
                        }
                        else {
                            this.staffsoftwareskill = obj;
                            title = $filter('translator')('EDITITEM');
                            return this.staffsoftwareskill;
                        }
                    },
                    title: function () {
                        return title;
                    }
                }
            });
        };

      

}
var app = angular.module("hiworkApp");

app.component('addEditStaffSoftwareSkill', {
    templateUrl: 'app/Components/StaffSoftwareSkill/Template/addEditStaffSoftwareSkill.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },

    controller: addEditController

});
function addEditController($scope, $uibModal, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $state, $rootScope) {
    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);
    $ctrl.onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    //$ctrl.saveSoftware = function () {
    //    $scope.isTriedSave = true;
    //    if (!$ctrl.modalData.Name || !$ctrl.modalData.Description  ||
    //        $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "" ) {
    //        return;
    //    }
    //    ajaxService.AjaxPostWithData($ctrl.modalData, "staffsoftwareskill/save", successOnSaving, errorOnSaving);
    //};

    //var successOnSaving = function (response) {
    //    $rootScope.$broadcast("staffSoftwareSkillAdded", response);
    //    $ctrl.modalInstance.close($ctrl.modalData);
    //    toastr.success($filter('translator')('DATASAVED'));

    //};

    $ctrl.saveSoftware = function () {
        $scope.isTriedSave = true;
        if (!$ctrl.modalData.Name || !$ctrl.modalData.Description ||
            $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "") {
            return;
        }


        ajaxService.AjaxPostWithData($ctrl.modalData, "staffsoftwareskill/save", successOnSaving, errorOnSaving);
    };

    var successOnSaving = function (response) {
        $rootScope.$broadcast("staffSoftwareSkillAdded", response);
        $ctrl.modalInstance.close($ctrl.modalData);
        toastr.success($filter('translator')('DATASAVED'));
    };

 

    var errorOnSaving = function (message) {

        toastr.error('Error in saving staffsoftwareskill ');
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