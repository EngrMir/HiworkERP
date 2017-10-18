var app = angular.module("hiworkApp");

app.component('inquirylist', {
    templateUrl: 'app/Components/InquiryList/Template/ContactUsList.html',

    controller: contactusController

});

function contactusController($scope, $uibModal, appSettings, AppStorage, sessionFactory, $filter, ajaxService, $state, $rootScope) {

    //this.contactus = { Name: "", DivisionID: "", TelNumber: "", Fax: "", Email: "", Comment: "" };

    $scope.rowCollection = [];
    $scope.ContactUscoll = [];

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    this.init = function () {
        GetContactUsList();
    };
    $scope.$on("dataDeleted", function (event, response) {
        GetContactUsList();
    });


    function GetContactUsList() {
        var contactus = {};
        contactus.CurrentUserID = currentUser.CurrentUserID;
        contactus.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(contactus, "Contactus/list", onGetData, onGetError);
    }

    var onGetData = function (response) {
        $scope.rowCollection = response;
        $scope.ContactUscoll = [].concat($scope.rowCollection);
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

  
    $scope.openEmailWindow = function (email) {
        var title = "Reply Email"
        $uibModal.open({
            component: "emailWindow",
            transclude: true,
            resolve: {
                title: function () {
                    return title;
                },
                toaddress: function() {
                    return email;
                 }
            },
            size: 'lg'
        });
    };
   




}