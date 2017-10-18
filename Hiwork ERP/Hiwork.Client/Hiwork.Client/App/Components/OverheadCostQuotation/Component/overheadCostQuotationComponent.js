
angular.module("hiworkApp").component('overheadCost', {
    templateUrl: 'app/Components/OverheadCostQuotation/Template/overheadCostQuotation.html',
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
    controller: overheadCostQuotationController
})

function overheadCostQuotationController($scope, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, $http, $stateParams, $parse, $localStorage) {
    var vm = this;
    vm.rowCollection = [];
    vm.CostQuotationModel = {};
    vm.EstimationAction = {};
    vm.RemarksState = new Array(9);
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    vm.CostQuotationModel.CurrentUserID = currentUser.CurrentUserID;
    vm.CostQuotationModel.CurrentCulture = currentCulture;
    vm.CostQuotationModel.ApplicationId = appSettings.ApplicationId;
    vm.CostQuotationModel.ExclTaxCost = 0;
    vm.CostQuotationModel.TaxEstimate = 0;
    vm.PostCode = null;
    vm.CostQuotationModel.CostQuotationItems = [];
    vm.CostQuotationModel.ActionItems = [];
    vm.Currency = {};

    vm.culturalProperties = ["ClientAddress", "BillingCompanyName", "BillingAddress", "DeliveryCompanyName", "DeliveryAddress", "CoordinatorNotes", "Remarks", "OtherItemName"];

    vm.ActionTypeList = [
        { Name_en: "Change Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 1 },
        { Name_en: "Sending Email", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 2 },
        { Name_en: "Cancel Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 3 }
    ];

    vm.ClientStatusList = [
        { Name_en: "New", Name_jp: "New", Name_cn: "New", Name_fr: "New", Name_kr: "New", Name_tl: "New", No: 1 },
        { Name_en: "Existing", Name_jp: "Existing", Name_cn: "Existing", Name_fr: "Existing", Name_kr: "Existing", Name_tl: "Existing", No: 2 }
    ];

    var DummyEstimationDetailsModel = {
        ID: 0,
        ExcludeTax: false,
        Contents: '',
        UnitPrice1: 0,
        PageCount1: 0,
        DiscountRate: 10,
        Discount1: 0,
        Total: 0,
        Index: 0,
        SourceLanguageID: 0,
        TargetLanguageID: 0
    };

    vm.$onInit = function () {
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        vm.CostQuotationModel.CostQuotationItems = [].concat(vm.rowCollection);
        if (vm.currencyList) {
            for (i = 0; i < vm.currencyList.length; i++) {
                if (vm.currencyList[i].Code == "USD") {
                    vm.Currency = vm.currencyList[i];
                    vm.changeCurrency();
                    break;
                }
            }
        }
        if ($stateParams.id != "" && $stateParams.id != null) {
            var i, j;
            manageState();
            mapCulturalItems();
            //Date formation
            vm.CostQuotationModel.InquiryDate = new Date(vm.CostQuotationModel.InquiryDate);
            vm.CostQuotationModel.FirstDeliveryDate = new Date(vm.CostQuotationModel.FirstDeliveryDate);
            vm.CostQuotationModel.FinalDeliveryDate = new Date(vm.CostQuotationModel.FinalDeliveryDate);
            vm.CostQuotationModel.CurrentCulture = currentCulture;
            //
            vm.CostQuotationModel.IsActionBtnDisable = false;
            ajaxService.AjaxPostWithData(BaseModel, "overheadcostquotation/list/" + vm.CostQuotationModel.ID, onGetEstimationDetails, errorFunction);
            j = 0;
            if (vm.CostQuotationModel.RemarksCoordinatorType) {
                for (i = 0; i < vm.CostQuotationModel.RemarksCoordinatorType.length; i += 2) {
                    if (vm.CostQuotationModel.RemarksCoordinatorType.charAt(i) == '1') {
                        vm.RemarksState[j] = true;
                    }
                    j += 1;
                }
            }
            if (vm.currencyList) {
                for (i = 0; i < vm.currencyList.length; i++) {
                    if (vm.currencyList[i].Id == vm.CostQuotationModel.CurrencyID) {
                        vm.Currency = vm.currencyList[i];
                        vm.changeCurrency();
                        break;
                    }
                }
            }
            loadEstimationActionList();
        } else {
            var indx = 0;
            for (var i = 0; i < 1; i++) {
                indx++;
                var item = angular.copy(DummyEstimationDetailsModel);
                item.Index = indx;
                vm.rowCollection.push(item);
                vm.CostQuotationModel.CostQuotationItems = [].concat(vm.rowCollection);
            }
            vm.CostQuotationModel.IsActionBtnDisable = true;
        }
    };

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.OverheadCostQuotation = $stateParams.Estimation;
            }
            vm.CostQuotationModel = $localStorage.OverheadCostQuotation;
        }
    }

    var mapCulturalItems = function () {
        for (var x = 0; x < vm.culturalProperties.length; x++) {
            var the_variable = 'vm.CulturalItem.' + vm.culturalProperties[x];
            var val = vm.CostQuotationModel[vm.culturalProperties[x]];
            var model = $parse(the_variable);
            model.assign($scope, val);
        }
    }

    var onGetEstimationDetails = function (response) {
        if (response) {
            for (var i = 0; i < response.length; i++) {
                var totalPrice = (response[i].UnitPrice1 ? (response[i].UnitPrice1 * response[i].PageCount1) : 0);
                var discountRate = totalPrice === 0 ? 0 : ((response[i].Discount1 * 100) / totalPrice);
                DummyEstimationDetailsModel = {
                    ID: response[i].ID,
                    ExcludeTax: response[i].ExcludeTax,
                    Contents: response[i].Contents,
                    UnitPrice1: response[i].UnitPrice1,
                    PageCount1: response[i].PageCount1,
                    DiscountRate: discountRate,
                    Discount1: response[i].Discount1,
                    Total: totalPrice - response[i].Discount1,
                    SourceLanguageID: response[i].SourceLanguageID,
                    TargetLanguageID: response[i].TargetLanguageID
                };
                var item = angular.copy(DummyEstimationDetailsModel);
                vm.rowCollection.push(item);
                vm.CostQuotationModel.CostQuotationItems = [].concat(vm.rowCollection);

                vm.calculateAmount();
            }
        }
    };

    vm.changeCurrency = function () {
        vm.CostQuotationModel.CurrencyID = vm.Currency.Id;
    };

    $scope.$on("selectedOutwardSales", function (event, response) {
        vm.CostQuotationModel.OutwardSalesID = response[0].ID;
    });
    $scope.$on("selectedLargeSales", function (event, response) {
        vm.CostQuotationModel.LargeSalesID = response[0].ID;
    });
    $scope.$on("selectedApprovals", function (event, response) {
        vm.CostQuotationModel.ApprovalID = response[0].ID;
    });
    $scope.$on("selectedRepresentative", function (event, response) {
        vm.CostQuotationModel.SalesPersonID = response[0].ID;
    });
    $scope.$on("selectedprepQuotaion", function (event, response) {
        vm.CostQuotationModel.AssistantID = response[0].ID;
    });
    $scope.$on("selectedCoordinator", function (event, response) {
        vm.CostQuotationModel.CoordinatorID = response[0].ID;
    });
    $scope.$on("selectedClient", function (event, response) {
        vm.CostQuotationModel.ClientID = response[0].ID;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.CostQuotationModel.TradingID = response[0].Id;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.CostQuotationModel.AffiliateTeamID = response[0].Id;
    });

    vm.addItem = function () {
        DummyEstimationDetailsModel = {
            ID: 0,
            ExcludeTax: false,
            Contents: '',
            UnitPrice1: 0,
            PageCount1: 0,
            DiscountRate: 10,
            Discount1: 0,
            Total: 0,
            Index: 0,
            SourceLanguageID: 0,
            TargetLanguageID: 0
        };
        var item = angular.copy(DummyEstimationDetailsModel);
        vm.rowCollection.push(item);
        vm.CostQuotationModel.CostQuotationItems = [].concat(vm.rowCollection);
    };

    vm.prepareQuotationPdf = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.CostQuotationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.CostQuotationModel.TaxEstimation = vm.TaxEstimate;
        vm.CostQuotationModel.QuotationInclTax = vm.TotalCost;
        vm.CostQuotationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.CostQuotationModel.ConsumptionOnTax = vm.ConsumptionTax;
        
        $http.post(appSettings.API_BASE_URL + 'overheadcostquotation/generatepdf', vm.CostQuotationModel, { responseType: 'arraybuffer' })
          .then(function (data) {
              console.log(data);
              var file = new Blob([data.data], { type: 'application/pdf' });
              //fileURL = URL.createObjectURL(file);
              //window.open(fileURL);
              saveAs(file, 'Quotation.pdf');
          });
    };

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

    vm.saveEstimationAction = function (flag) {
        if (!flag) {
            vm.EstimationAction.OperationBy = currentUser.CurrentUserID;
            vm.EstimationAction.EstimationID = vm.CostQuotationModel.ID;
            var model = {
                'EstimationAction': vm.EstimationAction,
                'CulturalItem': vm.CulturalItem,
                'CurrentUserID': currentUser.CurrentUserID,
                'ApplicationID': appSettings.ApplicationId,
                'Culture': currentCulture
            };
            ajaxService.AjaxPostWithData(model, "estimationaction/save", function (response) {
                toastr.success($filter('translator')('DATASAVED'));
                loadEstimationActionList();
                vm.EstimationAction.NextActionDate = '';
                vm.EstimationAction.ActionDetails = '';
                vm.EstimationAction.ActionType = '';
            }, errorFunction);
        }
    };

    vm.registerQuotaion = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.CostQuotationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.CostQuotationModel.TaxEstimation = vm.TaxEstimate;
        vm.CostQuotationModel.GrandTotal = vm.TotalCost;
        vm.CostQuotationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.CostQuotationModel.ConsumptionOnTax = vm.ConsumptionTax;

        var model = {
            'Estimation': vm.CostQuotationModel,
            'EstimationDetails': vm.CostQuotationModel.CostQuotationItems,
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };

        ajaxService.AjaxPostWithData(model, "overheadcostquotation/save", successFunction, errorFunction);
    };

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    vm.removeTableRow = function (item) {
        vm.CostQuotationModel.CostQuotationItems.splice(vm.CostQuotationModel.CostQuotationItems.indexOf(item), 1);
        vm.rowCollection.splice(vm.rowCollection.indexOf(item), 1);
        if (vm.CostQuotationModel.CostQuotationItems.length === 0) {
            vm.CostQuotationModel.ExcludedTaxCost = 0;
            vm.TotalCost = 0;
        }
        vm.calculateAmount();
    };

    vm.calculateDiscountRateOrValue = function (f, type) {
        var flag = '';
        f.DiscountRate = f.DiscountRate > 50 ? 50 : f.DiscountRate === 0 ? 10 : f.DiscountRate;
        var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
        if (type === 'rate') {
            f.Discount1 = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
            flag = 'd';
            //if (f.Discount1 > totalPrice) {
            //    f.DiscountRate = 0;
            //    return false;
            //}
        } else {
            f.Discount1 = f.Discount1 > (totalPrice / 2) ? (totalPrice / 2) : f.Discount1;
            f.DiscountRate = totalPrice === 0 ? 0 : ((f.Discount1 * 100) / totalPrice);
            flag = 'r';
        }
        vm.calculateAmount(null, flag);
    };

    var loadEstimationActionList = function () {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.CostQuotationModel.ID, function (res) {
            if (res) {
                var actionItems = [];
                for (var x = 0; x < res.length; x++) {
                    var actionItem = {
                        ID: res[x].ID,
                        EstimationID: res[x].EstimationID,
                        Date: res[x].NextActionDate,
                        Updatedby: res[x].OperandName,
                        Detail: res[x].ActionDetails
                    };
                    actionItems.push(actionItem);
                }
                vm.CostQuotationModel.ActionItems = [].concat(actionItems);
            }
        }, errorFunction);
    };

    /*
    This single method manages all calculations for overhead quptation's table data.
    */
    vm.calculateAmount = function (item, flag) {
        vm.TaxEstimate = 0; vm.ExclTaxQuotation = 0; vm.ConsumptionTax = 0;
        vm.ExclTaxCost = 0; vm.TotalCost = 0; vm.AverageUnitPrice = 0; vm.Subtotal = 0;
        var count = 0;
        var itemIndex = 0;
        var totalUnitPrice = 0;
        _.forEach(vm.CostQuotationModel.CostQuotationItems, function (f) {
            if (f.UnitPrice1 > 0) {
                f.DiscountRate = f.DiscountRate === 0 ? 10 : f.DiscountRate;
                totalUnitPrice += f.UnitPrice1;
                var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
                var totalDiscount = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
                f.ItemTotal = totalPrice - totalDiscount;
                vm.Subtotal += f.ItemTotal;
                if (flag === 'r' || typeof flag === 'undefined') {
                    f.Discount1 = totalDiscount;
                }
                vm.TotalCost += f.ItemTotal;
                if (!f.ExcludeTax) {
                    vm.TaxEstimate += f.ItemTotal;
                } else {
                    vm.ExclTaxQuotation += f.ItemTotal;
                    vm.ConsumptionTax = (vm.ExclTaxQuotation * 8) / 100;
                    vm.TotalCost = vm.TotalCost + vm.ConsumptionTax;
                }
                count++;
            }
            itemIndex++;
            f.Index = itemIndex;
        });
        vm.AverageUnitPrice = totalUnitPrice / vm.CostQuotationModel.CostQuotationItems.length;
        if (typeof vm.CostQuotationModel.ExcludedTaxCost != 'undefined') {
            vm.TotalCost = vm.TotalCost - vm.CostQuotationModel.ExcludedTaxCost;
        }
    };

    /*
    This method triggers at the time of clicking upload button.
    */
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

    /*
    After uploading a file, this event read that file's necessary data.
    */
    $scope.$on("receiveDocumentData", function (event, response) {
        vm.CostQuotationModel.AttachedMaterialFileName = response.Document.FileName;
        vm.CostQuotationModel.AttachedMaterialDownloadURL = response.Document.DownloadURL;
        vm.CostQuotationModel.Document.TranslationText = response.Document.TranslationText;
        vm.CostQuotationModel.Document.WordCount = response.Document.WordCount;
    });

    vm.deleteEstimationAction = function (item) {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/delete/" + item.ID, function (response) {
            toastr.success($filter('translator')('DATASAVED'));
            loadEstimationActionList();
        }, errorFunction);
    }
}