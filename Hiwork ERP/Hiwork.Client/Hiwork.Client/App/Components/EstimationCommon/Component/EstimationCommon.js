
angular.module("hiworkApp").component('estimationCommon', {
    templateUrl: 'app/Components/EstimationCommon/Template/EstimationCommon.html',
    controllerAs: "vm",
    bindings: {
        routes: "=",
        services: "=",
        languages: "=",
        businessCategories: "=",
        specializedFields: "=",
        subSpecializedFields: "=",
        currencyList: "=",
        translationCertificateSettingsList: "="
    },
    controller: estimationCommonController
});

function estimationCommonController($scope, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, EstimationType, EstimationStatus) {

    var vm = this;
    vm.rowCollection = [];
    vm.EstimationModel = {};
    vm.RemarksState = new Array(9);

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
    vm.EstimationModel.CurrentCulture = currentCulture;
    vm.EstimationModel.ApplicationId = appSettings.ApplicationId;
    vm.EstimationModel.EstimationItems = [];
    vm.Currency = {};
    //vm.EstimationModel.TradingID = {};
    //vm.EstimationModel.ProjectID = {};
    //vm.EstimationModel.AffiliateTeamID = {};
    //vm.EstimationModel.ClientID = {};

    vm.ActionTypeList = [
        { Name: "Change Quotation", No: 1 },
        { Name: "Sending Email", No: 2 },
        { Name: "Cancel Quotation", No: 3 }
    ];

    vm.ClientStatusList = [
        { Name: "New", No: 1 },
        { Name: "Existing", No: 2 }
    ];

    var NewDocument = {
        EstimateID: "",
        EstimateDetailsID: "",
        FileName: "",
        WordCount: 0,
        TranslationText: "",
        DownloadURL: ""
    };
    var DummyEstimationDetailsModel = {
        UnitPrice1: 0, UnitPrice2: 0, UnitPrice3: 0, UnitPrice4: 0, UnitPrice5: 0,
        PageCount1: 0, PageCount2: 0, PageCount3: 0, PageCount4: 0, PageCount5: 0,
        Discount1: 0, Discount2: 0, Discount3: 0, Discount4: 0, Discount5: 0,
        SourceLanguageID: "", TargetLanguageID: "", ServiceTypeID: "", ExcludeTax: false,
        ItemTotalAmount: 0, ItemTotalDiscount: 0, ItemTotal: 0,
        Document: _.clone(NewDocument)
    };

    vm.$onInit = function () {
        for (i = 0; i < 3; i++) {
            var item = angular.copy(DummyEstimationDetailsModel);
            vm.rowCollection.push(item);
        }
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        vm.EstimationModel.EstimationItems = [].concat(vm.rowCollection);

        for (i = 0; i < vm.currencyList.length; i++) {
            if (vm.currencyList[i].Code == "USD") {
                vm.Currency = vm.currencyList[i];
                vm.changeCurrency();
                break;
            }
        }
        vm.EstimationModel.AverageUnitPrice = 0;
        vm.EstimationModel.ActualUnitPrice = 0;
        vm.EstimationModel.DiscountTotal = 0;
        vm.EstimationModel.TaxEstimation = 0;
        vm.EstimationModel.QuotationInclTax = 0;
        vm.EstimationModel.QuotationExclTax = 0;
        vm.EstimationModel.ConsumptionOnTax = 0;
        vm.EstimationModel.ExcludedTaxCost = 0;
        vm.EstimationModel.IssuedByTranslator = 0;
        vm.EstimationModel.IssuedByCompany = 0;
        vm.EstimationModel.PriceEstimation = 0;
        vm.EstimationModel.OtherItemName = "";
        vm.EstimationModel.OtherItemUnitPrice = 0;
        vm.EstimationModel.OtherItemNumber = 0;
        vm.EstimationModel.OtherAmount = 0;
    };

    vm.changeCurrency = function () {
        vm.EstimationModel.CurrencyID = vm.Currency.Id;
    };

    $scope.$on("selectedOutwardSales", function (event, response) {
        vm.EstimationModel.OutwardSalesID = response[0].ID;
    });
    $scope.$on("selectedLargeSales", function (event, response) {
        vm.EstimationModel.LargeSalesID = response[0].ID;
    });
    $scope.$on("selectedApprovals", function (event, response) {
        vm.EstimationModel.ApprovalID = response[0].ID;
    });
    $scope.$on("selectedRepresentative", function (event, response) {
        vm.EstimationModel.SalesPersonID = response[0].ID;
    });
    $scope.$on("selectedprepQuotaion", function (event, response) {
        vm.EstimationModel.AssistantID = response[0].ID;
    });
    $scope.$on("selectedCoordinator", function (event, response) {
        vm.EstimationModel.CoordinatorID = response[0].ID;
    });
    $scope.$on("selectedClient", function (event, response) {
        var Company = response[0];

        vm.EstimationModel.ClientID = response[0].ID;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.EstimationModel.TradingID = response[0].Id;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.EstimationModel.AffiliateTeamID = response[0].Id;
    });

    vm.addItem = function () {
        var item = angular.copy(DummyEstimationDetailsModel);
        vm.rowCollection.push(item);
        vm.EstimationModel.EstimationItems = [].concat(vm.rowCollection);
    };

    vm.registerQuotaion = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.EstimationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.EstimationModel.EstimationType = EstimationType.Translation;
        vm.EstimationModel.EstimationStatus = EstimationStatus.Created;
        ajaxService.AjaxPostWithData(vm.EstimationModel, "estimation/save", successFunction, errorFunction);
    };

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    vm.uploadFile = function (title, document, pos) {
        var binding = {};
        binding.component = "estimationFileSelection";
        binding.resolve = {};
        binding.resolve.modalData = {};
        binding.resolve.modalData.Document = document;
        binding.resolve.modalData.docIndex = pos;
        binding.resolve.modalData.listento = "receiveDocumentData";
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };

    $scope.$on("receiveDocumentData", function (event, response) {
        var item = vm.EstimationModel.EstimationItems[response.Position];
        item.Document = response.Document;
    });


    vm.openEmailWindow = function () {
        var title = "Quotation Email"
        $uibModal.open({
            component: "emailWindow",
            transclude: true,
            resolve: {
                title: function () {
                    return title;
                }
            },
            size: 'lg'
        });
    };


}