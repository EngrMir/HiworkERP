

/* ******************************************************************************************************************
 * AngularJS 1.5 Directive for common selection window to select files and display file infos
 * Date             :   21-July-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/


/* ******************************************************************************************************************
 * In this javascript file, there will be two angularjs entity to be programmed
 * First directive  :   The main directive which will be used directly by programmers to incorporate common selection
 * Second Component :   Child component which will be responsible for Add/Edit selection
 * *****************************************************************************************************************/





//angular.module("hiworkApp").directive("estimationFileSelector", estimationFileSelectorDirective);

//function estimationFileSelectorDirective() {

//    function estimationFileSelectController($scope, fileUploadService, sessionFactory, AppStorage, appSettings) {

//        $scope.File = {};
//        $scope.FileList = [];
//        $scope.CountWordFromFile = 0;
//        $scope.Content = null;

//        $scope.UploadFile = function () {
//            var currentUser = sessionFactory.GetObject(AppStorage.userData);
//            var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
//            var UploadURL = appSettings.API_BASE_URL + "fileUpload/uploadDocument";
//            fileUploadService.uploadFileToUrl($scope.File, UploadURL, currentUser.CurrentUserID, currentCulture, onSuccess, onError);
//        };

//        var onSuccess = function (response) {
//            var docList = response.data;
//            $scope.CountWordFromFile = docList[0].WordCount;
//            $scope.Content = docList[0].Content;
//        };

//        var onError = function (response) {
//            if (!response.data) {
//                response = "Unidentified error occured";
//            }
//            toastr.error(response.data);
//        };
//    }

//    // Create the Directive Definition Object (DDO) and return it to the angular system
//    var ddo = {};
//    ddo.restrict = "E";
//    ddo.templateUrl = "App/Directives/EstimationFileSelector/Template/viewEstimationFileSelector.html";
//    ddo.scope = {};
//    ddo.controller = ["$scope", "fileUploadService", "sessionFactory", "AppStorage", "appSettings", estimationFileSelectController];
//    return ddo;
//};


angular.module('hiworkApp').component('estimationFileSelection', {

    templateUrl: 'App/Directives/EstimationFileSelector/Template/viewEstimationFileSelector.html',
    bindings: { modalInstance: "<", resolve: "<" },
    controller: ["$rootScope", "fileUploadService", "sessionFactory", "AppStorage", "appSettings", ctrlEstimationFileSelection]
});


function ctrlEstimationFileSelection($rootScope, fileUploadService, sessionFactory, AppStorage, appSettings) {

    var $ctrl = this;

    $ctrl.$onInit = function () {
        $ctrl.File = {};
        $ctrl.WordCount = 0;
        $ctrl.TranslationText = "";
        $ctrl.DocList = [];
        $ctrl.title = $ctrl.resolve.title;

        $ctrl.modalData = angular.copy($ctrl.resolve.modalData);
        $ctrl.modalData.Document = $ctrl.resolve.modalData.Document;

        if ($ctrl.modalData.Document != null) {
            $ctrl.DocList.push($ctrl.modalData.Document);
            $ctrl.WordCount = $ctrl.DocList[0].WordCount;
            $ctrl.TranslationText = $ctrl.DocList[0].TranslationText;
        }
        $ctrl.listento = $ctrl.modalData.listento;
    };

    $ctrl.UploadFile = function () {
        var currentUser = sessionFactory.GetObject(AppStorage.userData);
        var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
        var UploadURL = appSettings.API_BASE_URL + "fileUpload/uploadDocument";
        fileUploadService.uploadFileToUrl($ctrl.File, UploadURL, currentUser.CurrentUserID, currentCulture, onSuccess, onError);
    };

    var onSuccess = function (response) {
        $ctrl.DocList = response.data;
        $ctrl.WordCount = $ctrl.DocList[0].WordCount;
        $ctrl.TranslationText = $ctrl.DocList[0].TranslationText;
    };

    var onError = function (response) {
        if (!response.data) {
            response = "Unidentified error occured";
        }
        toastr.error(response.data);
    };

    $ctrl.OK = function () {
        $rootScope.$broadcast($ctrl.listento, { "Document": $ctrl.DocList[0], "Position": $ctrl.modalData.docIndex });
        $ctrl.Close();
    };

    $ctrl.Close = function () {
        $ctrl.resolve.modalData = null;
        $ctrl.modalInstance.close($ctrl.modalData);
    };

    $ctrl.Dismiss = function () {
        $ctrl.modalInstance.dismiss("cancel");
    };
}

