angular.module("hiworkApp").component('narrationOrder', {
    templateUrl: 'app/Components/NarrationOrderDetail/Template/NarrationOrderDetails.html',
    controllerAs: "vm",
    bindings: {
        routes: "=",
        services: "=",
        languages: "=",
        businessCategories: "=",
        specializedFields: "=",
        subSpecializedFields: "=",
        currencyList: "="
    },
    controller: NarrationOrderDetailsController
})


function NarrationOrderDetailsController() {

    var vm = this;

    vm.$onInit = function () {
        
    }
}