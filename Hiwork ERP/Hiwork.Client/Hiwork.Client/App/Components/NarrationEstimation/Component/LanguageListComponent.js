angular.module("hiworkApp").component('languageList', {
    templateUrl: "App/Components/NarrationEstimation/Template/LanguageList.html",
    controllerAs: "vm",
    bindings: {


    },
    controller: languageListController
});

function languageListController(StaffRegistrationService) {

    var vm = this;

    vm.$onInit = function () {
        StaffRegistrationService.getLanguage().then(function (response) {

            vm.narrationLanguage = response;
        });
    }

}