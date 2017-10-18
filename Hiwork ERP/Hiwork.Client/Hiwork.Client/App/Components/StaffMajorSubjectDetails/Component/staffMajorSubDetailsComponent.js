
//Done by Tamal
//For parent staff Major Subject deatils list

angular.module("hiworkApp").component('staffmajorsubjecstdetails', {

    templateUrl: 'app/Components/StaffMajorSubjectDetails/Template/StaffMajorSubDetailsList.html',
    controller: staffmajorsubdetailsController

})

function staffmajorsubdetailsController($scope, $uibModal, appSettings, AppStorage, sessionFactory,loginFactory, $filter, ajaxService,$state) {

    this.staffmajorsubdetail = { Name: "", StaffMajorSubjectID: "", Description: "", SortBy: "", IsActive: false };


    $scope.rowCollection = [];
    $scope.staffmajorsubdetailscoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    if (loginFactory.IsAuthenticated() == false) {
        $state.go("login");
    }

    this.init = function () {
        GetAllMajorSubjectDetailsList();
    };

    function GetAllMajorSubjectDetailsList() {
        var staffmajorsubdetails = {};
        staffmajorsubdetails.CurrentUserID = currentUser.CurrentUserID;
        staffmajorsubdetails.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(staffmajorsubdetails, "smsd/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.staffmajorsubdetailscoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.$on("staffmajorsubdetailsAdded", function (event, response) {
        GetAllMajorSubjectDetailsList();
    });

    $scope.$on("dataDeleted", function (event, response) {
        GetAllMajorSubjectDetailsList();
    });

    $scope.searchText = function () {
        var value = angular.element('#search').val();
        var data = $filter('filter')($scope.rowCollection, value);
        $scope.staffmajorsubdetailscoll = null;
        $scope.staffmajorsubdetailscoll = [].concat(data);
    };

    this.open = function (obj, title) {
        $uibModal.open({
            component: "addStaffMajorsubsdetails",
            resolve: {
                modalData: function () {
                    if (obj == null) {
                        this.staffmajorsubdetail = { Name: "", StaffMajorSubjectID: "", Description: "", SortBy: "", IsActive: false };
                        title = $filter('translator')('ADDNEWITEM');
                        return this.staffmajorsubdetail;
                    }
                    else {
                        this.staffmajorsubdetail = obj;
                        title = $filter('translator')('EDITITEM');
                        return this.staffmajorsubdetail;
                    }
                },
                title: function () {
                    return title;
                }
            }
        });
    };

}


//for staff Major Subject list pop up window

angular.module('hiworkApp').component('addStaffMajorsubsdetails', {
    templateUrl: 'app/Components/StaffMajorSubjectDetails/Template/addEditStaffMajorSubDetails.html',
    bindings: {
        modalInstance: "<",
        resolve: "<"
    },
    controller: addstaffmajorsubdetailsController
});

function addstaffmajorsubdetailsController($scope, $rootScope, appSettings, AppStorage, sessionFactory, $filter, ajaxService) {

    var $ctrl = this;
    currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    currentUser = sessionFactory.GetObject(AppStorage.userData);
   

    $ctrl.getMajorSubject = function () {
        var MajorSubject = {};
        MajorSubject.CurrentUserID = currentUser.CurrentUserID;
        MajorSubject.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(MajorSubject, "staffmajorsub/list", $ctrl.onGetData, $ctrl.onGetError);

    }
        $ctrl.onGetData = function (response) {
            $ctrl.msdList = [];
            $ctrl.msdList = response;
        };

        $ctrl.onGetError = function (message) {
            toastr.error('Error in getting records');
        };

        $ctrl.saveSubjectDetails = function () {
            $scope.isTriedSave = true;
            if (!$ctrl.modalData.Name || !$ctrl.modalData.Description || !$ctrl.modalData.SortBy || 
                $ctrl.modalData.Name == "" || $ctrl.modalData.Description == "" || $ctrl.modalData.SortBy == "")
                return;
            if (!$ctrl.modalData.StaffMajorSubjectID || $ctrl.modalData.StaffMajorSubjectID == "")
                return;
            ajaxService.AjaxPostWithData($ctrl.modalData, "smsd/save", successOnSaving, errorOnSaving);
        }

        var successOnSaving = function (response) {
            $rootScope.$broadcast("staffmajorsubdetailsAdded", response);
            $ctrl.modalInstance.close($ctrl.modalData);
            toastr.success($filter('translator')('DATASAVED'));
        };

        var errorOnSaving = function (message) {
            
            toastr.error('Error in saving staff major sub details');
        };

        $ctrl.$onInit = function () {
            $ctrl.title = $ctrl.resolve.title;
            $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
            $ctrl.modalData.CurrentUserID = currentUser.CurrentUserID;
            $ctrl.modalData.CurrentCulture = currentCulture;


            $ctrl.getMajorSubject();

        }

        $ctrl.Close = function () {
            $ctrl.resolve.modalData = null;
            $ctrl.modalInstance.close($ctrl.modalData);
        };

        $ctrl.Dismiss = function () {
            $ctrl.modalInstance.dismiss("cancel");
        };
    }
