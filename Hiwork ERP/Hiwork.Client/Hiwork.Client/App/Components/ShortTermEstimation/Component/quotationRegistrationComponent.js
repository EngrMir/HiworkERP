angular.module("hiworkApp").component('shortterm', {
    templateUrl: 'app/Components/ShortTermEstimation/Template/quotationRegistration.html',
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
    controller: shorttermEstimationController
});

function shorttermEstimationController($scope, $http, $state, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, EstimationType, EstimationStatus, $stateParams, $localStorage, EstimationDefaultStatus, $location) {

    var vm = this;
    vm.rowCollection = [];
    vm.CompanyDepartment = {};
    vm.CompanyDepartmentList = [];
    vm.EstimationModel = {};
    vm.RemarksState = new Array(9);
    vm.EstimationAction = {};
    vm.EstimationModel.ActionItems = [];
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    vm.EstimationModel.EstimationItems = [];
    vm.Currency = {};
    vm.HeaderBtnAttr = {};
    vm.HeaderBtnAttr = EstimationDefaultStatus.init;
    vm.culturalProperties = ["BillingAddress", "ClientAddress", "BillingCompanyName", "DeliveryCompanyName", "DeliveryAddress", "Remarks", "CoordinatorNotes"];
    vm.ButtonTitles = ['Registration / Update', 'Order Details', 'Temporary Registration', 'Project initiation', 'Approval Request', 'Approval', 'Quotation Email', 'Quotation Request', 'Confirmation Email', 'Delete'];

    //vm.EstimationModel.TradingID = {};
    //vm.EstimationModel.ProjectID = {};
    //vm.EstimationModel.AffiliateTeamID = {};
    //vm.EstimationModel.ClientID = {};

    //vm.ActionTypeList = [
    //    { Name: "Change Quotation", No: 1 },
    //    { Name: "Sending Email", No: 2 },
    //    { Name: "Cancel Quotation", No: 3 }
    //];
    vm.ActionTypeList = [
       { Name_en: "Change Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 1 },
       { Name_en: "Sending Email", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 2 },
       { Name_en: "Cancel Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 3 }
    ];
    vm.ClientStatusList = [
        { Name: "New", No: 1 },
        { Name: "Existing", No: 2 }
    ];
    var dummyActionItemsModel = {
        EstimationID: ''
    , NextActionDate: ''
    , ActionType: 0
    , OperationBy: ''
    , OperationDate: ''
    , ActionDetails: ''
    }
    var NewDocument = {
        ID: "",
        EstimateID: "",
        EstimateDetailsID: "",
        FileName: "",
        WordCount: 0,
        TranslationText: "",
        DownloadURL: ""
    };
    //var DummyEstimationDetailsModel = {
    //    UnitPrice1: 0, UnitPrice2: 0, UnitPrice3: 0, UnitPrice4: 0, UnitPrice5: 0,
    //    PageCount1: 0, PageCount2: 0, PageCount3: 0, PageCount4: 0, PageCount5: 0,
    //    Discount1: 0, Discount2: 0, Discount3: 0, Discount4: 0, Discount5: 0,
    //    SourceLanguageID: "", TargetLanguageID: "", ServiceTypeID: "", Contents: "", ExcludeTax: false,BasicPrice:0,
    //    BasicTime: 0, AdditionalBasicAmount: 0, ExtraTime: 0, LastnightTime: 0, MovingTime: 0, BasicAmount: 0,
    //    ExtensionAmount: 0, ExtraAmount: 0, LateAtNightAmount: 0, MovingAmount: 0, NumberOfDays: 0, NumberOfPeople: 0,
    //    OtherAmount: 0, CertificateAmount: 0,
    //    ItemTotalAmount: 0, ItemTotalDiscount: 0, ItemTotal: 0,IsMarkedForDelete:false,
    //    Document: _.clone(NewDocument)
    //};
    var DummyEstimationDetailsModel = {
        GeneralUnitPrice: "", Contents: "", BasicAmount: 0, BasicTime: 0, AdditionalTime: 0, TotalDayHour: "", LateAtNightAmount: 0, SubTotal: 0,
        AdditionalPrice: 0, ExtensionAmount: 0, ExtensionTime: 0, LastnightTime: 0, TransferTime: 0, TransferFee: 0, NumberOfDays: 0, DiscountRate: 0,
        SourceLanguageID: "", TargetLanguageID: "", ServiceTypeID: "", ExcludeTax: false, NumberOfPeople: 0, IsMarkedForDelete: false,
        Document: _.clone(NewDocument)
    };
    // vm.EstimationModel.EstimationAction = _.clone(dummyActionItemsModel);
    vm.EstimationAccessModel = {
        UserID: sessionFactory.GetObject(AppStorage.userData).CurrentUserID,
        EstimationTypeID: 1,
        EstimationStatusID: 0
    }
    vm.$onInit = function () {
        estimationService.manipulateUserAccess(vm.EstimationAccessModel);
        vm.GetAllServiceType();
        vm.GetAllUnit();
        vm.GetAllUnitPrice();
        // vm.GetAllCountryList();

        vm.FixOriginalLanguage = false;
        vm.FixTargetLanguage = false;
        vm.TotalPriceForEstimationDetails = 0;

        var i;
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        vm.EstimationModel.EstimationItems.push(angular.copy(DummyEstimationDetailsModel));

        for (i = 0; i < vm.currencyList.length; i++) {
            if (vm.currencyList[i].Code == "USD") {
                vm.Currency = vm.currencyList[i];
                vm.changeCurrency();
                break;
            }
        }
        if ($stateParams.id != "" && $stateParams.id != null) {
            debugger;
            //alert($stateParams.Estimation);
            manageState();
            vm.HeaderBtnAttr = vm.EstimationModel.PageButtonAttribute;
            var i, j;
            //vm.EstimationModel = $stateParams.Estimation;
            vm.EstimationModel.InquiryDate = new Date(vm.EstimationModel.InquiryDate);
            vm.EstimationModel.FirstDeliveryDate = new Date(vm.EstimationModel.FirstDeliveryDate);
            vm.EstimationModel.FinalDeliveryDate = new Date(vm.EstimationModel.FinalDeliveryDate);
            j = 0;
            for (i = 0; i < vm.EstimationModel.RemarksCoordinatorType.length; i += 2) {
                if (vm.EstimationModel.RemarksCoordinatorType.charAt(i) == '1') {
                    vm.RemarksState[j] = true;
                }
                j += 1;
            }
            for (i = 0; i < vm.currencyList.length; i++) {
                if (vm.currencyList[i].Id == vm.EstimationModel.CurrencyID) {
                    vm.Currency = vm.currencyList[i];
                    vm.changeCurrency();
                    break;
                }
            }
            BaseModel.ID = vm.EstimationModel.ClientID;
            ajaxService.AjaxPostWithData(BaseModel, "company/departmentlist", onGetCompanyDepartment, errorFunction);
            ajaxService.AjaxPostWithData(BaseModel, "estimationdetails/list/" + vm.EstimationModel.ID, onGetEstimationDetails, errorFunction);
            $localStorage.ShortEstimation = $stateParams.Estimation;
            
            loadEstimationActionList();
            
            vm.EstimationAccessModel.EstimationStatusID = vm.EstimationModel.EstimationStatus ? vm.EstimationModel.EstimationStatus : 0;
            estimationService.manipulateUserAccess(vm.EstimationAccessModel);
        }
        else {
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
            var indx = 0;
            for (i = 0; i < 1; i++) {;
                indx++;
                var item = angular.copy(DummyEstimationDetailsModel);
                item.Index = indx;
                vm.rowCollection.push(item);
                vm.EstimationModel.TaskQuotationItems = [].concat(vm.rowCollection);
            }
            vm.EstimationModel.IsActionBtnDisable = true;
        }
        applyButtonState();
    };
    vm.saveQuotationEstimationAction = function (flag) {
        if (!flag) {
            vm.EstimationAction.OperationBy = currentUser.CurrentUserID;
            vm.EstimationAction.EstimationID = vm.EstimationModel.ID;
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
                vm.CulturalItem.ActionDetails = '';
            }, errorFunction);
        }

    };
    vm.deleteEstimationAction = function (item) {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/delete/" + item.ID, function (response) {
            toastr.success($filter('translator')('DATASAVED'));
            loadEstimationActionList();
        }, errorFunction);
    }
    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.EstimationModel = $stateParams.Estimation;
            }
            vm.EstimationModel = $localStorage.EstimationModel;
        }
    };
    //var mapCulturalItems = function () {
    //    for (var x = 0; x < vm.culturalProperties.length; x++) {
    //        var the_variable = 'vm.CulturalItem.' + vm.culturalProperties[x];
    //        var val = vm.EstimationModel[vm.culturalProperties[x]];
    //        var model = $parse(the_variable);
    //        model.assign($scope, val);
    //    }
    //};
    var onGetEstimationDetails = function (response) {
        var index = 0;
        var ServerLocationString = "";
        var EstmDetailsList = response;
        var length = EstmDetailsList.length;
        vm.EstimationModel.EstimationItems = response;
        while (index < length) {
            ServerLocationString = "estimationfiles/list/" + vm.EstimationModel.ID + "/" + EstmDetailsList[index].ID;
            ajaxService.AjaxPostWithData(BaseModel, ServerLocationString, onGetEstimationFiles, errorFunction);
            vm.calculateAmount(EstmDetailsList[index]);
            index += 1;
        }
    };

    var onGetEstimationFiles = function (response) {
        var index = 0;
        var length = vm.EstimationModel.EstimationItems.length;
        // for (index = 0; index < length; index += 1) {
        //if (vm.EstimationModel.EstimationItems[index].ID == response[0].EstimateDetailsID) {
        //    vm.EstimationModel.EstimationItems[index].Document = response[0];
        //    break;
        //}
        //}
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
    vm.GetAllServiceType = function () {

        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "estimationServiceType/list", onGetServiceData, onGetServiceError);

    }

    var onGetServiceData = function (response) {
        vm.ServiceTypeList = response;
    }

    var onGetServiceError = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    }


    vm.FixLanguagesForEstimationDetails = function () {
        var StateOriginal = vm.FixOriginalLanguage;
        var StateTarget = vm.FixTargetLanguage;
        var EstimationItemCount = vm.EstimationModel.EstimationItems.length;
        var index;
        if (EstimationItemCount <= 1) {
            return;
        }
        var SourceLanguageID = vm.EstimationModel.EstimationItems[0].SourceLanguageID;
        var TargetLanguageID = vm.EstimationModel.EstimationItems[0].TargetLanguageID;
        for (index = 1; index < EstimationItemCount; index += 1) {
            if (StateOriginal == true) {
                vm.EstimationModel.EstimationItems[index].SourceLanguageID = SourceLanguageID;
            }
            if (StateTarget == true) {
                vm.EstimationModel.EstimationItems[index].TargetLanguageID = TargetLanguageID;
            }
        }
    };

    vm.changeCurrency = function () {
        vm.EstimationModel.CurrencyID = vm.Currency.Id;
    };

    $scope.$on("selectedOutwardSales", function (event, response) {
        vm.EstimationModel.OutwardSalesID = response[0].ID;
        vm.EstimationModel.OutwardSalesName = response[0].Name;
    });
    $scope.$on("selectedLargeSales", function (event, response) {
        vm.EstimationModel.LargeSalesID = response[0].ID;
        vm.EstimationModel.LargeSalesName = response[0].Name;
    });
    $scope.$on("selectedApprovals", function (event, response) {
        vm.EstimationModel.ApprovalID = response[0].ID;
        vm.EstimationModel.ApprovalName = response[0].Name;
    });
    $scope.$on("selectedRepresentative", function (event, response) {
        vm.EstimationModel.SalesPersonID = response[0].ID;
        vm.EstimationModel.SalesPersonName = response[0].Name;
    });
    $scope.$on("selectedprepQuotaion", function (event, response) {
        vm.EstimationModel.AssistantID = response[0].ID;
        vm.EstimationModel.AssistantName = response[0].Name;
    });
    $scope.$on("selectedCoordinator", function (event, response) {
        vm.EstimationModel.CoordinatorID = response[0].ID;
        vm.EstimationModel.CoordinatorName = response[0].Name;
    });
    $scope.$on("selectedClient", function (event, response) {
        var Company = response[0];
        vm.EstimationModel.ClientID = response[0].ID;
        vm.EstimationModel.ClientName = response[0].Name;
        BaseModel.ID = Company.ID;
        ajaxService.AjaxPostWithData(BaseModel, "company/departmentlist", onGetCompanyDepartment, errorFunction);
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.EstimationModel.TradingName = response[0].Name;
        vm.EstimationModel.TradingID = response[0].Id;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.EstimationModel.AffiliateTeamName = response[0].Name;
        vm.EstimationModel.AffiliateTeamID = response[0].Id;
    });

    var onGetCompanyDepartment = function (response) {
        vm.CompanyDepartmentList = [];
        vm.CompanyDepartmentList = response;
        var index;
        for (index = 0; index < vm.CompanyDepartmentList.length; index += 1) {
            if (vm.CompanyDepartmentList[index].ID == vm.EstimationModel.ClientDepartmentID) {
                vm.CompanyDepartment = vm.CompanyDepartmentList[index];
                //vm.ChangeCompanyDepartment(false);
                break;
            }
        }

    };
    vm.UpdateUnitPrice = function (Item) {
        Item.BasicPrice = null;
        var i;
        var UnitPrice;
        for (i = 0; i < vm.UnitPriceList.length; i += 1) {
            UnitPrice = vm.UnitPriceList[i];
            if (UnitPrice.SourceLanguageID == Item.SourceLanguageID
                && UnitPrice.TargetLanguageID == Item.TargetLanguageID
                && UnitPrice.EstimationTypeID == EstimationType.Interpreter
                && UnitPrice.UnitID == vm.EstimationModel.UnitID) {
                Item.BasicPrice = UnitPrice.GeneralUnitPrice;
                break;
            }
        }
        vm.PriceCalculation(Item);
    };

    vm.UpdateUnitPriceByUnit = function () {
        for (i = 0; i < vm.EstimationModel.EstimationItems.length; i++) {
            vm.EstimationModel.EstimationItems[i].BasicPrice = null;
            for (j = 0; j < vm.UnitPriceList.length; j += 1) {
                UnitPrice = vm.UnitPriceList[j];
                if (UnitPrice.SourceLanguageID == vm.EstimationModel.EstimationItems[i].SourceLanguageID
                    && UnitPrice.TargetLanguageID == vm.EstimationModel.EstimationItems[i].TargetLanguageID
                    && UnitPrice.EstimationTypeID == EstimationType.Interpreter
                    && UnitPrice.UnitID == vm.EstimationModel.UnitID) {
                    vm.EstimationModel.EstimationItems[i].BasicPrice = UnitPrice.GeneralUnitPrice;
                    break;
                }
            }
            vm.PriceCalculation(vm.EstimationModel.EstimationItems[i]);
        }
    }
    vm.ChangeCompanyDepartment = function (OverrideTextValues) {
        vm.EstimationModel.ClientDepartmentID = vm.CompanyDepartment.ID;
        if (OverrideTextValues == true) {
            vm.EstimationModel.ClientPersonInCharge = vm.CompanyDepartment.InchargeName_Local;
            vm.EstimationModel.ClientEmailCC = vm.CompanyDepartment.InchargeEmail;
            vm.EstimationModel.ClientAddress = vm.CompanyDepartment.Address;
            vm.EstimationModel.ClientContactNo = vm.CompanyDepartment.PhoneNo;
            vm.EstimationModel.ClientFax = vm.CompanyDepartment.Fax;
            vm.EstimationModel.BillingCompanyName = vm.CompanyDepartment.BillingClientName;
            vm.EstimationModel.BillingTo = vm.CompanyDepartment.BillingTo;
            vm.EstimationModel.BillingEmailCC = vm.CompanyDepartment.BillingEmail;
            vm.EstimationModel.BillingAddress = vm.CompanyDepartment.BillingAddress;
            vm.EstimationModel.BillingContactNo = vm.CompanyDepartment.BillingContactNo;
            vm.EstimationModel.BillingFax = vm.CompanyDepartment.BillingFax;
            vm.EstimationModel.PaymentTerms = vm.CompanyDepartment.BillingPaymentTerms;
        }
    };
    vm.ResetDate = function (item) {
        var TotalNoOfDays = 0;
        var TotalHour = 0;
        if (item.StartDate != null && item.CompletionDate != null) {
            var milisecondsDiff = item.CompletionDate - item.StartDate;
            TotalHour = milisecondsDiff / (1000 * 60 * 60);
            item.TotalDayHour = Math.floor(milisecondsDiff / (1000 * 60 * 60)).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / (1000 * 60)) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / 1000) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 });
        }

        else {
            item.TotalDayHour = null;
        }
        TotalNoOfDays = TotalHour / 24;
        item.NumberOfDays = TotalNoOfDays;
        vm.TotalCostCalcultion(item);
    }
    vm.DiscountCalculation = function (item) {

        if (item.StartTime != null && item.FinishTime != null && item.BasicAmount != 0) {
            item.AmountOfDiscount = item.SubTotal * (item.DiscountRate / 100);
            item.TotalAfterDiscount = item.SubTotal - item.AmountOfDiscount;
        }
        else {
            return;
        }
        vm.TotalCostCalcultion(item);
    }

    vm.TotalCostCalcultion = function (item) {
        if (item.StartDate != null && item.FinishDate != null && item.BasicAmount != 0 && item.StartTime != null && item.FinishTime != null) {
            if (item.NumberOfDays == 0) {
                item.NumberOfDays = 1;
            }
            item.TotalCost = item.TotalAfterDiscount * item.NumberOfDays;
        }
    }
    vm.addItem = function () {
        var item = angular.copy(DummyEstimationDetailsModel);
        if (vm.EstimationModel.EstimationItems.length >= 1) {
            var SourceLanguageID = vm.EstimationModel.EstimationItems[0].SourceLanguageID;
            var TargetLanguageID = vm.EstimationModel.EstimationItems[0].TargetLanguageID;
            if (vm.FixOriginalLanguage == true) {
                item.SourceLanguageID = SourceLanguageID;
            }
            if (vm.FixTargetLanguage == true) {
                item.TargetLanguageID = TargetLanguageID;
            }
        }
        vm.EstimationModel.EstimationItems.push(item);
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
        vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
        vm.EstimationModel.CurrentCulture = currentCulture;
        vm.EstimationModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(vm.EstimationModel, "quotation/save", successFunction, errorFunction);
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
        item.Document.FileName = response.Document.FileName;
        item.Document.TranslationText = response.Document.TranslationText;
        item.Document.DownloadURL = response.Document.DownloadURL;
        item.Document.WordCount = response.Document.WordCount;
    });
    vm.orderStatus = function () {
        vm.EstimationModel.CurrentCulture = currentCulture;
        vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.EstimationModel, 'estimation/orderstatus', function (r) {
            toastr.success($filter('translator')('ORDERSTATUS'));
        }, errorFunction);
    };
    vm.orderLoss = function () {
        vm.EstimationModel.CurrentCulture = currentCulture;
        vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.EstimationModel, 'estimation/orderloss', function (r) {
            toastr.success($filter('translator')('ORDERSTATUSLOSS'));
        }, errorFunction);
    }
    //vm.calculateAmount = function (item) {
    //    var totalPrice = (item.Quantity ? item.Quantity : 0) * (item.SpliteRate ? item.SpliteRate : 0);
    //    var totalDiscount = (totalPrice * (item.Discount ? item.Discount : 0)) / 100;
    //    item.ItemTotal = totalPrice - totalDiscount;

    //    vm.AvgUnitPrice1 = 0; vm.AvgUnitPrice2 = 0; vm.AvgUnitPrice3 = 0;
    //    vm.AvgUnitPrice4 = 0; vm.AvgUnitPrice5 = 0;


    //    vm.TotalPriceForEstimationDetails = 0;

    //    _.forEach(vm.EstimationModel.EstimationItems, function (f) {           
    //        vm.TotalPriceForEstimationDetails += f.ItemTotal;
    //    });
    //               // vm.estimateTranslation();
    //               // vm.estimateOtherAmount();
    //};
    vm.calculateAmount = function (item) {
        vm.TaxEstimate = 0; vm.ExclTaxQuotation = 0; vm.ConsumptionTax = 0;
        vm.ExclTaxCost = 0; vm.TotalCost = 0; vm.AverageUnitPrice = 0; vm.Subtotal = 0;
        var count = 0;
        var itemIndex = 0;
        var totalUnitPrice = 0;
        _.forEach(vm.EstimationItems, function (f) {
            if (f.UnitPrice1 > 0) {
                var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
                totalUnitPrice += f.UnitPrice1;
                var totalDiscount = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
                f.ItemTotal = totalPrice - totalDiscount;
                vm.Subtotal += f.ItemTotal;
                //if (flag === 'r') {
                f.Discount1 = totalDiscount;
                // }
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
        //vm.AverageUnitPrice = totalUnitPrice / vm.EstimationItems.length;
        if (typeof vm.EstimationModel.ExcludedTaxCost != 'undefined') {
            vm.TotalCost = vm.TotalCost - vm.EstimationModel.ExcludedTaxCost;
        }
    };
    vm.estimateTranslation = function () {
        var totalIssuedByTranslator = vm.EstimationModel.IssuedByTranslator * vm.translationCertificateSettingsList[0].UnitPrice;
        var totalIssuedByCompany = vm.EstimationModel.IssuedByCompany * vm.translationCertificateSettingsList[1].UnitPrice;
        vm.EstimationModel.PriceCertification = totalIssuedByCompany + totalIssuedByTranslator;
        vm.estimateGrandCalculation();
    };

    vm.projectInitiation = function () {
        var title = "Project Initiation"
        $uibModal.open({
            component: "projectinitiation",
            transclude: true,
            resolve: {
                title: function () {
                    return title;
                }
            },
            size: 'lg'
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

    //vm.removeTableRow = function (item) {
    //    vm.EstimationModel.EstimationItems.splice(vm.EstimationModel.EstimationItems.indexOf(item), 1);
    //    vm.rowCollection.splice(vm.rowCollection.indexOf(item), 1);
    //    if (vm.EstimationModel.EstimationItems.length === 0) {
    //        vm.EstimationModel.ExcludedTaxCost = 0;
    //        vm.TotalCost = 0;
    //    }
    //    vm.calculateAmount();
    //};

    vm.estimateOtherAmount = function () {
        vm.EstimationModel.OtherAmount = vm.EstimationModel.OtherItemUnitPrice * vm.EstimationModel.OtherItemNumber;
        vm.estimateGrandCalculation();
    };

    vm.basicCalculation = function (item) {
        //item.Basic = (vm.CompletionDate - vm.StartDate).toString();
        item.Basic = vm.StartTime.toString();
        // alert(vm.StartTime.toString());
        var timeInput = vm.StartTime.toString().split("00");
        // alert(timeInput[0]);
    };

    vm.estimateGrandCalculation = function () {
        var index;
        var item;
        vm.EstimationModel.TaxEstimation = 0;
        vm.EstimationModel.QuotationExclTax = 0;

        for (index = 0; index < vm.EstimationModel.EstimationItems.length; index += 1) {
            item = vm.EstimationModel.EstimationItems[index];
            vm.EstimationModel.TaxEstimation += item.ExcludeTax == true ? 0 : item.ItemTotal;
            vm.EstimationModel.QuotationExclTax += item.ExcludeTax == true ? item.ItemTotal : 0;
        }

        vm.EstimationModel.ConsumptionOnTax = vm.EstimationModel.TaxEstimation * 0.08;
        vm.EstimationModel.QuotationInclTax = vm.EstimationModel.TaxEstimation +
            vm.EstimationModel.QuotationExclTax + vm.EstimationModel.ConsumptionOnTax +
            vm.EstimationModel.ExcludedTaxCost + vm.EstimationModel.PriceCertification +
            vm.EstimationModel.OtherAmount;
    };

    vm.orderDetails = function (item) {
        $state.go('OrderDetails', { 'id': vm.EstimationModel.ID, 'Estimation': vm.EstimationModel });
    }
    vm.GetAllUnit = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "unit/list", onGetUnitList, onGetServiceError);
    }

    var onGetUnitList = function (response) {
        vm.UnitList = response;
    }
    var onGetServiceError = function (message) {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    }


    vm.GetAllUnitPrice = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        ajaxService.AjaxPostWithData(BaseModel, "UnitPrice/list", onGetUnitPrice, errorFunction);

    };

    var onGetUnitPrice = function (response) {
        vm.UnitPriceList = [];
        vm.UnitPriceList = response;
    };

    vm.UpdateUnitPrice = function (Item) {
        Item.BasicPrice = null;
        var i;
        var UnitPrice;
        for (i = 0; i < vm.UnitPriceList.length; i += 1) {
            UnitPrice = vm.UnitPriceList[i];
            if (UnitPrice.SourceLanguageID == Item.SourceLanguageID
                && UnitPrice.TargetLanguageID == Item.TargetLanguageID
                && UnitPrice.EstimationTypeID == EstimationType.Interpreter
                && UnitPrice.UnitID == vm.EstimationModel.UnitID) {
                Item.BasicPrice = UnitPrice.GeneralUnitPrice;
                break;
            }
        }
        vm.PriceCalculation(Item);
    };

    vm.UpdateUnitPriceByUnit = function () {
        for (i = 0; i < vm.EstimationModel.EstimationItems.length; i++) {
            vm.EstimationModel.EstimationItems[i].BasicPrice = null;
            for (j = 0; j < vm.UnitPriceList.length; j += 1) {
                UnitPrice = vm.UnitPriceList[j];
                if (UnitPrice.SourceLanguageID == vm.EstimationModel.EstimationItems[i].SourceLanguageID
                    && UnitPrice.TargetLanguageID == vm.EstimationModel.EstimationItems[i].TargetLanguageID
                    && UnitPrice.EstimationTypeID == EstimationType.Interpreter
                    && UnitPrice.UnitID == vm.EstimationModel.UnitID) {
                    vm.EstimationModel.EstimationItems[i].BasicPrice = UnitPrice.GeneralUnitPrice;
                    break;
                }
            }
            vm.PriceCalculation(vm.EstimationModel.EstimationItems[i]);
        }
    }
    vm.PriceCalculation = function (item) {

        if (item.StartTime != null && item.FinishTime != null && item.BasicAmount != 0) {
            var AdditionalBasicAmount = 0;
            var BasicAmount = 0;
            var ExtensionAmount = 0;
            var TransferFeePerHour = 2000;
            var BasicTime = 1;
            BasicAmount = parseInt(item.BasicAmount);
            ExtensionAmount = (BasicAmount) * (125 / 100);
            item.ExtensionAmount = ExtensionAmount * item.ExtensionTime;
            AdditionalBasicAmount = (BasicAmount) * (100 / 100);
            item.AdditionalBasicAmount = AdditionalBasicAmount * item.AdditionalTime;

            // Transfer Time and Transfer Fee Calculation
            item.TransferAmount = item.TransferTime * TransferFeePerHour;
            //Late Night Fee Calculation
            var LateAtNightAmount = 0;
            LateAtNightAmount = BasicAmount * (150 / 100);
            item.LateAtNightAmount = LateAtNightAmount * item.LateNightTime;

            item.UnitPriceSubtotal = item.AdditionalBasicAmount + item.ExtensionAmount + item.LateAtNightAmount + item.TransferAmount;

            vm.DiscountCalculation(item);
        }

    }
    var applyButtonState = function () {       
        var classes = ['.btn-order-details', '.btn-temp-reg', '.btn-project', '.btn-approval-req', '.btn-approval', '.btn-quotation-email', '.btn-quotation-request', '.btn-conf-email', '.btn-order-loss', '.btn-delete'];
        classes.forEach(function (clazz) {
            angular.element(document.querySelector(clazz)).attr('disabled', false);
        });
        if (vm.EstimationModel.EstimationStatusName === 'Under_estimation') {
            //alert(vm.EstimationModel.EstimationStatusName+'  pp ');
            classes = ['.btn-order-details', '.btn-approval', '.btn-quotation-email', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
        }
        else if (vm.EstimationModel.EstimationStatusName === 'Waiting_for_approval') {
            classes = ['.btn-order-details', '.btn-approval-req', '.btn-quotation-email', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
        }
        else if (vm.EstimationModel.EstimationStatusName === 'Approved') {
            classes = ['.btn-order-details', '.btn-approval', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
            vm.ButtonTitles[5] = 'Approved';
        }
        else if (vm.EstimationModel.EstimationStatusName === 'Waiting_for_confirmation') {
            classes = ['.btn-order-details', '.btn-approval', '.btn.btn-primary.pull-left.order'];
        }
        else if (vm.EstimationModel.EstimationStatusName === 'Ordered' || vm.EstimationModel.EstimationStatus > 5) {
            classes = ['.btn-temp-reg', '.btn.btn-primary.pull-left.initi', '.btn-approval-req', '.btn-approval', '.btn-conf-email'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
            vm.ButtonTitles[5] = 'Approved';
            vm.ButtonTitles[8] = 'Confirmation Email Sent';
        }
        classes.forEach(function (clazz) {
            angular.element(document.querySelector(clazz)).attr('disabled', true);
        });
    }
    var loadEstimationActionList = function () {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.EstimationModel.ID, function (res) {
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
                vm.EstimationModel.ActionItems = [].concat(actionItems);
            }
        }, errorFunction);
    };
    vm.ResetTime = function (item) {
        item.NewStartTime = item.StartTime;
        item.NewFinishTime = item.FinishTime;

        vm.EstimationModel.IsLateNight = false;
        var AdditionalTime = 0;
        var BasicTime = 1;
        var totalhour = 0;
        var hourdif = 0;
        var LateNightStartHour = 11;
        var LateNightStartMeridian = "PM";
        var LateNightFinishHour = 6;
        var LateNightDif = 7;
        var LateNightFinishMeridian = "AM";
        var LateNightInverseStartMeridian = "";
        var LateNightInverseFinishMeridian = "";

        if (LateNightStartMeridian == 'PM') {
            LateNightInverseStartMeridian = 'AM';
        }
        else {
            LateNightInverseStartMeridian = 'PM';
        }

        if (LateNightFinishMeridian == 'PM') {
            LateNightInverseFinishMeridian = 'AM';
        }
        else {
            LateNightInverseFinishMeridian = 'PM';
        }


        if (item.StartTime != null && item.FinishTime != null) {

            var milisecondsDiff = item.FinishTime - item.StartTime;
            // vm.EstimationModel.TotalTimePeriod = Math.floor(milisecondsDiff / (1000 * 60 * 60)).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / (1000 * 60)) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / 1000) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 });
            var seconddif = milisecondsDiff / 1000;
            var minutedif = seconddif / 60;
            var hourdif = milisecondsDiff / (1000 * 60 * 60);


        }
        else {
            item.BasicTime = null;
        }

        // Split 24 Hour Time Format to 12 Hour Time Format

        if (item.StartTime != null && item.FinishTime != null) {
            // var starttime = document.getElementById('starttime');
            var starttime = item.StartTime;
            var starttimeSplit = starttime.toTimeString();
            var starttimeSplit = starttimeSplit.split(':'),
                StartHour,
                StartMinute,
                StartMeridian;
            StartHour = starttimeSplit[0];
            StartMinute = starttimeSplit[1];
            if (StartHour > 12) {
                StartMeridian = 'PM';
                StartHour -= 12;
            } else if (StartHour < 12) {
                StartMeridian = 'AM';
                if (StartHour == 0) {
                    StartHour = 12;
                }
            }
            else if (StartHour == 0) {
                StartHour = 12;
                StartMeridian = 'AM';
            }
            else {
                StartMeridian = 'PM';
            }

            //var finishtime = document.getElementById('finishtime');
            var finishtime = item.FinishTime;
            var finishtimeSplit = finishtime.toTimeString().split(':'),
                FinishHour,
                FinishMinute,
                FinishMeridian;
            FinishHour = finishtimeSplit[0];
            FinishMinute = finishtimeSplit[1];
            if (FinishHour > 12) {
                FinishMeridian = 'PM';
                FinishHour -= 12;
            } else if (FinishHour < 12) {
                FinishMeridian = 'AM';
                if (FinishHour == 0) {
                    FinishHour = 12;
                }
            }
            else if (FinishHour == 0) {
                FinishHour = 12;
                FinishMeridian = 'AM';
            }
            else {
                FinishMeridian = 'PM';
            }
            item.NewStartTime = StartHour + ":" + StartMinute + " " + StartMeridian;
            item.NewFinishTime = FinishHour + ":" + FinishMinute + " " + FinishMeridian;

            // Total Working hour Calculation
            var StartFloatingHour = 0;
            var FinishFloatingHour = 0;
            var NewStartHour = 0;
            var NewFinishHour = 0;
            var NewStartMinute = 0;
            var NewFinishMinute = 0;
            var Flag = true;
            NewStartMinute = parseInt(StartMinute);
            NewFinishMinute = parseInt(FinishMinute);
            NewStartHour = parseInt(StartHour);
            NewFinishHour = parseInt(FinishHour);
            var NewStartMeridian = StartMeridian;
            var NewFinishMeridian = FinishMeridian;
            var count = 0;
            var isFirsCheck = 0;
            var clockhour = NewStartHour;
            NewStartMeridian = StartMeridian;

            if (NewStartHour == NewFinishHour && NewStartMinute == NewFinishMinute && StartMeridian == FinishMeridian) {
                totalhour = 24;
                item.BasicTime = totalhour;
                count = LateNightDif;
                Flag = false;
            }
            else {
                if (hourdif < 0) {
                    totalhour = 24 + hourdif;
                    item.BasicTime = totalhour;
                }
                else {
                    totalhour = hourdif;
                    item.BasicTime = hourdif;
                }
            }

            if (NewStartHour == NewFinishHour && NewStartMinute != NewFinishMinute && StartMeridian == FinishMeridian) {
                Flag = false;
                if (NewStartMinute < NewFinishMinute) {
                    var minutedif = NewFinishMinute - NewStartMinute;
                    var totalminute = minutedif;
                    count = totalminute / 60;
                }
                else {
                    var minutedif = NewStartMinute - NewFinishMinute;
                    var totalminute = (LateNightDif * 60) - minutedif;
                    if (totalminute < 0) {
                        totalminute = totalminute + (7 * 60);
                    }
                    count = totalminute / 60;
                }
            }


            // Late Night Time Check

            for (i = 0 ; i <= totalhour + 4 ; i++) {

                if ((clockhour == 13 || clockhour == 12) && isFirsCheck != 0) {
                    if (clockhour == 13) {
                        clockhour = 1;
                    }
                    else {
                        if (NewStartMeridian == 'AM') {
                            NewStartMeridian = 'PM';
                        }
                        else {
                            NewStartMeridian = 'AM';
                        }
                    }
                }
                if (clockhour == NewFinishHour && NewStartMeridian == FinishMeridian) {
                    break;
                }

                if (clockhour >= 11 && NewStartMeridian == LateNightStartMeridian && clockhour < 12 || ((clockhour <= 6 && NewStartMeridian == LateNightFinishMeridian) || (clockhour == 12 && NewStartMeridian == LateNightFinishMeridian))) {
                    count++;
                }
                clockhour++;
                isFirsCheck++;
            };
            //if (count >= 8)
            //{
            //    count--;
            //}
            vm.EstimationModel.BaseCount = count;
            if ((NewStartMinute != NewFinishMinute) || (StartMeridian != FinishMeridian)) {
                if ((NewStartHour >= LateNightStartHour && StartMeridian == LateNightStartMeridian && NewStartHour < 12) || (NewStartHour <= LateNightFinishHour && StartMeridian == LateNightFinishMeridian) || (NewStartHour == 12 && StartMeridian == LateNightFinishMeridian) && Flag) {
                    if ((NewFinishHour > LateNightFinishHour && FinishMeridian == LateNightFinishMeridian && NewFinishHour < 12) || (NewFinishHour < 11 && FinishMeridian == LateNightStartMeridian) || (NewFinishHour == 12 && FinishMeridian == LateNightStartMeridian)) {
                        count = count - 1;
                        var minutedif = NewStartMinute;
                        var totalminute = (count * 60) - minutedif;
                        count = totalminute / 60;
                    }
                }
                if ((NewFinishHour >= LateNightStartHour && FinishMeridian == LateNightStartMeridian && NewFinishHour < 12) || (NewFinishHour <= LateNightFinishHour && FinishMeridian == LateNightFinishMeridian) || (NewFinishHour == 12 && FinishMeridian == LateNightFinishMeridian) && Flag) {
                    if ((NewStartHour > LateNightFinishHour && StartMeridian == LateNightFinishMeridian && NewStartHour < 12) || (NewStartHour < 11 && StartMeridian == LateNightStartMeridian) || (NewStartHour == 12 && StartMeridian == LateNightStartMeridian)) {
                        var minutedif = NewFinishMinute;
                        var totalminute = (count * 60) + minutedif;
                        count = totalminute / 60;
                    }
                }
                if ((NewStartHour >= LateNightStartHour && StartMeridian == LateNightStartMeridian && NewStartHour < 12) || (NewStartHour <= LateNightFinishHour && StartMeridian == LateNightFinishMeridian) || (NewStartHour == 12 && StartMeridian == LateNightFinishMeridian) && Flag) {
                    if ((NewFinishHour >= LateNightStartHour && FinishMeridian == LateNightStartMeridian && NewFinishHour < 12) || (NewFinishHour <= LateNightFinishHour && FinishMeridian == LateNightFinishMeridian) || (NewFinishHour == 12 && FinishMeridian == LateNightFinishMeridian)) {
                        if (NewStartMinute < NewFinishMinute) {
                            var minutedif = NewFinishMinute - NewStartMinute;
                            var totalminute = (count * 60) + minutedif;
                            count = totalminute / 60;
                        }
                        else {
                            var minutedif = NewStartMinute - NewFinishMinute;
                            var totalminute = (count * 60) - minutedif;
                            if (totalminute < 0) {
                                totalminute = totalminute + (7 * 60);
                            }
                            count = totalminute / 60;
                        }
                    }
                }

            }

            if (count >= 8) {
                count--;
            }
            item.LateNightTime = count;
            //Extension Time Calculation
            var ExtensionTime = 0;
            ExtensionTime = item.BasicTime - item.LateNightTime - (item.BasicTime - 8);
            item.ExtensionTime = ExtensionTime;
            //Additional Time Calculation
            item.AdditionalTime = item.BasicTime - ExtensionTime - item.LateNightTime - BasicTime;
            //Transfer Time is inputed Manually

            vm.PriceCalculation(item);
        }
    }
    vm.addItem = function () {
        //var item = angular.copy(DummyEstimationDetailsModel);
        //vm.rowCollection.push(item);
        //vm.EstimationModel.EstimationItems = [].concat(vm.rowCollection);


        var item = angular.copy(DummyEstimationDetailsModel);
        vm.EstimationModel.EstimationItems.push(item);
    };
    vm.TransFeeCalculation = function () {
        vm.Translation.TranslationSubTotal = vm.Translation.TranslationEvaluation * vm.Translation.TranslationNumberOfDay;
        vm.Translation.AccoSubTotal = vm.Translation.AccoEvaluation * vm.Translation.AccoNumberOfDay;
        vm.Translation.MealSubTotal = vm.Translation.MealEvaluation * vm.Translation.MealNumberOfDay;
        vm.Translation.AllowanceSubTotal = vm.Translation.AllowanceEvaluation * vm.Translation.AllowanceNumberOfDay;
        vm.Translation.Total = vm.Translation.TranslationSubTotal + vm.Translation.AccoSubTotal + vm.Translation.MealSubTotal + vm.Translation.AllowanceSubTotal;
    }
    vm.tempRegistration = function () {
        vm.EstimationModel.IsTemporaryRegistration = vm.EstimationModel.IsTemporaryRegistration === true ? false : true;
    }
    vm.manageApproval = function (action) {
        vm.EstimationModel.PageUrl = $location.absUrl();
        vm.EstimationModel.CurrentCulture = currentCulture;
        vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.EstimationModel, 'estimation/' + action, function (r) {
            if (action === 'approvalrequest') {
                culVar = 'APPROVALREQUESTSENT';
                vm.EstimationModel.EstimationStatusName = r.EstimationStatusName;
                applyButtonState();
            } else if (action === 'approverequest') {
                culVar = 'REQUESTAPPROVED';
                vm.EstimationModel.EstimationStatusName = r.EstimationStatusName;
                applyButtonState();
            }
            toastr.success($filter('translator')(culVar));
        }, errorFunction);
    }
    vm.prepareQuotationPdf = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        if (vm.RemarksState) {
            for (index = 0; index < vm.RemarksState.length; index += 1) {
                Status = vm.RemarksState[index] ? 1 : 0;
                RemarksString = RemarksString + Status.toString() + " ";
            }
            vm.EstimationModel.RemarksCoordinatorType = RemarksString.trim();
        }
        vm.EstimationModel.TaxEstimation = vm.TaxEstimate;
        vm.EstimationModel.QuotationInclTax = vm.TotalCost;
        vm.EstimationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.EstimationModel.ConsumptionOnTax = vm.ConsumptionTax;

        $http.post(appSettings.API_BASE_URL + 'shorttermestimation/generatepdf', vm.EstimationModel, { responseType: 'arraybuffer' })
          .then(function (data) {
              console.log(data);
              var file = new Blob([data.data], { type: 'application/pdf' });
              //fileURL = URL.createObjectURL(file);
              //window.open(fileURL);
              saveAs(file, 'Quotation.pdf');
          });
    };
}


