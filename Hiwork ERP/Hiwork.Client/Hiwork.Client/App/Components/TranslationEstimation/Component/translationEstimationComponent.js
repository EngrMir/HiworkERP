
angular.module("hiworkApp").component('translationEstimation', {
    templateUrl: 'app/Components/TranslationEstimation/Template/translationEstimation.html',
    controllerAs: "vm",
    bindings: {
        routes: "=",
        services: "=",
        languages: "=",
        businessCategories: "=",
        specializedFields: "=",
        subSpecializedFields: "=",
        currencyList: "=",
        translationCertificateSettingsList: "=",
        manipulateUserAccess: "="
    },
    controller: translationEstimationController
});

function translationEstimationController($http, $scope, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, EstimationType, EstimationStatus, $stateParams, $localStorage, $state, $location) {

    var vm = this;
    vm.rowCollection = [];
    vm.CompanyDepartment = {};
    vm.CompanyDepartmentList = [];
    vm.EstimationModel = {};
    vm.RemarksState = new Array(9);
    vm.EstimationAction = {};

    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    vm.EstimationModel.EstimationItems = [];
    vm.EstimationModel.ActionItems = [];
    vm.Currency = {};
    //vm.EstimationModel.TradingID = {};
    //vm.EstimationModel.ProjectID = {};
    //vm.EstimationModel.AffiliateTeamID = {};
    //vm.EstimationModel.ClientID = {};
    vm.ButtonTitles = ['Registration / Update', 'Order Details', 'Temporary Registration', 'Project initiation', 'Approval Request', 'Approval', 'Quotation Email', 'Quotation Request', 'Confirmation Email', 'Delete'];

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
        ID: "",
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
        BasicTime: 0, AdditionalBasicAmount: 0, ExtraTime: 0, LastnightTime: 0, MovingTime: 0, BasicAmount: 0,
        ExtensionAmount: 0, ExtraAmount: 0, LateAtNightAmount: 0, MovingAmount: 0, NumberOfDays: 0, NumberOfPeople: 0,
        OtherAmount: 0, CertificateAmount: 0,
        ItemTotalAmount: 0, ItemTotalDiscount: 0, ItemTotal: 0, IsMarkedForDelete: false,
        Document: _.clone(NewDocument)
    };
    // ------------------
    vm.EstimationAccessModel = {
        UserID: sessionFactory.GetObject(AppStorage.userData).CurrentUserID,
        EstimationTypeID: 1,
        EstimationStatusID: 0
    }
    // ------------------
    vm.$onInit = function () {
        // ------------------
        estimationService.manipulateUserAccess(vm.EstimationAccessModel, vm.EstimationModel.CreatedBy);
        // ------------------

        vm.FixOriginalLanguage = false;
        vm.FixTargetLanguage = false;
        vm.TotalPriceForEstimationDetails = 0;

        var i;
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        vm.EstimationModel.EstimationItems.push(angular.copy(DummyEstimationDetailsModel));
        if (vm.currencyList) {
            for (i = 0; i < vm.currencyList.length; i++) {
                if (vm.currencyList[i].Code == "USD") {
                    vm.Currency = vm.currencyList[i];
                    vm.changeCurrency();
                    break;
                }
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
        vm.EstimationModel.IsActionBtnDisable = true;

        if ($stateParams.id != "" && $stateParams.id != null) {
            //var test = vm.manipulateUserAccess;
            //debugger;
            var i, j;
            manageState();
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

            vm.EstimationModel.IsActionBtnDisable = false;
            loadEstimationActionList();
            BaseModel.ID = vm.EstimationModel.ClientID;
            ajaxService.AjaxPostWithData(BaseModel, "company/departmentlist", onGetCompanyDepartment, errorFunction);
            ajaxService.AjaxPostWithData(BaseModel, "estimationdetails/list/" + vm.EstimationModel.ID, onGetEstimationDetails, errorFunction);

            // ------------------
            vm.EstimationAccessModel.EstimationStatusID = vm.EstimationModel.EstimationStatus ? vm.EstimationModel.EstimationStatus : 0;
            estimationService.manipulateUserAccess(vm.EstimationAccessModel, vm.EstimationModel.CreatedBy);
            // ------------------            
        }
    };

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.TranslationEstimation = $stateParams.Estimation;
            }
            vm.EstimationModel = $localStorage.TranslationEstimation;
        }
    }

    var onGetEstimationDetails = function (response) {
        var i, j;
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
        for (index = 0; index < length; index += 1) {
            if (vm.EstimationModel.EstimationItems[index].ID == response[0].EstimateDetailsID) {
                vm.EstimationModel.EstimationItems[index].Document = response[0];
                break;
            }
        }
    };

    vm.FixLanguagesForEstimationDetails = function () {
        var StateOriginal = vm.FixOriginalLanguage;
        var StateTarget = vm.FixTargetLanguage;
        var EstimationItemCount = vm.EstimationModel.EstimationItems.length;
        var SourceLanguage, TargetLanguage;
        var index, j;

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

        for (index = 0; index < vm.EstimationModel.EstimationItems.length; index += 1) {
            for (j = 0; j < vm.languages.length; j += 1) {
                if (vm.languages[j].ID == vm.EstimationModel.EstimationItems[index].SourceLanguageID) {
                    vm.EstimationModel.EstimationItems[index].SourceLanguageName = vm.languages[j].Name;
                }
                if (vm.languages[j].ID == vm.EstimationModel.EstimationItems[index].TargetLanguageID) {
                    vm.EstimationModel.EstimationItems[index].TargetLanguageName = vm.languages[j].Name;
                }
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
        vm.EstimationModel.TradingID = response[0].ID;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.EstimationModel.TeamName = response[0].Name;
        vm.EstimationModel.TeamID = response[0].ID;
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
        //if (vm.TotalPriceForEstimationDetails >= 300000) {
        //    toastr.error("Large sales could not found.");
        //    return false;
        //}
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

    vm.SimulateEstimation = function (title) {
        var binding = {};
        binding.component = "estimationSimulation";
        binding.resolve = {};
        binding.resolve.modalData = {};
        binding.resolve.modalData.Currency = vm.Currency;
        binding.resolve.modalData.EstimationDetailsList = vm.EstimationModel.EstimationItems;
        binding.resolve.title = function () { return title; };
        binding.windowClass = "simulation-dialog";
        binding.transclude = true;
        $uibModal.open(binding);
    };

    $scope.$on("receiveDocumentData", function (event, response) {
        var item = vm.EstimationModel.EstimationItems[response.Position];
        item.Document.FileName = response.Document.FileName;
        item.Document.TranslationText = response.Document.TranslationText;
        item.Document.DownloadURL = response.Document.DownloadURL;
        item.Document.WordCount = response.Document.WordCount;
    });

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

    vm.saveEstimationAction = function (flag) {
        if (!flag) {
            vm.EstimationAction.CurrentUserID = currentUser.CurrentUserID;
            vm.EstimationAction.CurrentCulture = currentCulture;
            vm.EstimationAction.ApplicationId = appSettings.ApplicationId;
            vm.EstimationAction.EstimationID = vm.TaskQuotationModel.ID;
            ajaxService.AjaxPostWithData(vm.EstimationAction, "estimationaction/save", function (response) {
                toastr.success($filter('translator')('DATASAVED'));
                loadEstimationActionList();
                vm.EstimationAction.NextActionDate = '';
                vm.EstimationAction.ActionDetails = '';
                vm.EstimationAction.ActionType = '';
            }, errorFunction);
        }
    };

    vm.prepareQuotationPdf = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.EstimationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.EstimationModel.TaxEstimation = vm.TaxEstimate;
        vm.EstimationModel.QuotationInclTax = vm.TotalCost;
        vm.EstimationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.EstimationModel.ConsumptionOnTax = vm.ConsumptionTax;

        $http.post(appSettings.API_BASE_URL + 'estimation/generatepdf', vm.EstimationModel, { responseType: 'arraybuffer' })
          .then(function (data) {
              console.log(data);
              var file = new Blob([data.data], { type: 'application/pdf' });
              //fileURL = URL.createObjectURL(file);
              //window.open(fileURL);
              var header = data.headers();
              saveAs(file, header.filename.replace(/"/g, ''));
          });
    };

    vm.calculateAmount = function (item) {

        var totalPrice = (item.UnitPrice1 ? (item.UnitPrice1 * item.PageCount1) : 0) + (item.UnitPrice2 ? (item.UnitPrice2 * item.PageCount2) : 0) + (item.UnitPrice3 ? (item.UnitPrice3 * item.PageCount3) : 0) + (item.UnitPrice4 ? (item.UnitPrice4 * item.PageCount4) : 0) + (item.UnitPrice5 ? (item.UnitPrice5 * item.PageCount5) : 0);
        //var totalPages = (item.PageCount1 ? item.PageCount1 : 0) + (item.PageCount2 ? item.PageCount2 : 0) + (item.PageCount3 ? item.PageCount3 : 0) + (item.PageCount4 ? item.PageCount4 : 0) + (item.PageCount5 ? item.PageCount5 : 0);
        var totalDiscount = (item.Discount1 ? (item.UnitPrice1 * item.PageCount1 * item.Discount1) / 100 : 0) + (item.Discount2 ? (item.UnitPrice2 * item.PageCount2 * item.Discount2) / 100 : 0) + (item.Discount3 ? (item.UnitPrice3 * item.PageCount3 * item.Discount3) / 100 : 0) + (item.Discount4 ? (item.UnitPrice4 * item.PageCount4 * item.Discount4) / 100 : 0) + (item.Discount5 ? (item.UnitPrice5 * item.PageCount5 * item.Discount5) / 100 : 0);
        item.ItemTotalAmount = totalPrice;
        item.ItemTotalDiscount = totalDiscount;
        item.ItemTotal = item.ItemTotalAmount - totalDiscount;

        vm.AvgUnitPrice1 = 0; vm.AvgUnitPrice2 = 0; vm.AvgUnitPrice3 = 0;
        vm.AvgUnitPrice4 = 0; vm.AvgUnitPrice5 = 0;

        vm.AvgUnitPriceDiscount1 = 0; vm.AvgUnitPriceDiscount2 = 0; vm.AvgUnitPriceDiscount3 = 0;
        vm.AvgUnitPriceDiscount4 = 0; vm.AvgUnitPriceDiscount5 = 0;
        var count1 = 0; count2 = 0; count3 = 0; count4 = 0, count5 = 0;
        vm.TotalPriceForEstimationDetails = 0;

        _.forEach(vm.EstimationModel.EstimationItems, function (f) {
            if (f.UnitPrice1 > 0) {
                vm.AvgUnitPrice1 += f.UnitPrice1;
                vm.AvgUnitPriceDiscount1 += f.UnitPrice1 * (f.Discount1 / 100);
                count1++;
            }
            if (f.UnitPrice2 > 0) {
                vm.AvgUnitPrice2 += f.UnitPrice2;
                vm.AvgUnitPriceDiscount2 += f.UnitPrice2 * (f.Discount2 / 100);
                count2++;
            }
            if (f.UnitPrice3 > 0) {
                vm.AvgUnitPrice3 += f.UnitPrice3;
                vm.AvgUnitPriceDiscount3 += f.UnitPrice3 * (f.Discount3 / 100);
                count3++;
            }
            if (f.UnitPrice4 > 0) {
                vm.AvgUnitPrice4 += f.UnitPrice4;
                vm.AvgUnitPriceDiscount4 += f.UnitPrice4 * (f.Discount4 / 100);
                count4++;
            }
            if (f.UnitPrice5 > 0) {
                vm.AvgUnitPrice5 += f.UnitPrice5;
                vm.AvgUnitPriceDiscount5 += f.UnitPrice5 * (f.Discount5 / 100);
                count5++;
            }
            vm.TotalPriceForEstimationDetails += f.ItemTotal;
        });

        vm.AvgUnitPrice1 = vm.AvgUnitPrice1 / count1;
        vm.AvgUnitPrice2 = vm.AvgUnitPrice2 / count2;
        vm.AvgUnitPrice3 = vm.AvgUnitPrice3 / count3;
        vm.AvgUnitPrice4 = vm.AvgUnitPrice4 / count4;
        vm.AvgUnitPrice5 = vm.AvgUnitPrice5 / count5;

        vm.AvgUnitPriceDiscount1 = vm.AvgUnitPriceDiscount1 / count1;
        vm.AvgUnitPriceDiscount2 = vm.AvgUnitPriceDiscount2 / count2;
        vm.AvgUnitPriceDiscount3 = vm.AvgUnitPriceDiscount3 / count3;
        vm.AvgUnitPriceDiscount4 = vm.AvgUnitPriceDiscount4 / count4;
        vm.AvgUnitPriceDiscount5 = vm.AvgUnitPriceDiscount5 / count5;

        vm.ActualUnitPrice1 = vm.AvgUnitPrice1 - vm.AvgUnitPriceDiscount1;
        vm.ActualUnitPrice2 = vm.AvgUnitPrice2 - vm.AvgUnitPriceDiscount2;
        vm.ActualUnitPrice3 = vm.AvgUnitPrice3 - vm.AvgUnitPriceDiscount3;
        vm.ActualUnitPrice4 = vm.AvgUnitPrice4 - vm.AvgUnitPriceDiscount4;
        vm.ActualUnitPrice5 = vm.AvgUnitPrice5 - vm.AvgUnitPriceDiscount5;

        vm.estimateTranslation();
        vm.estimateOtherAmount();
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

    vm.estimateOtherAmount = function () {
        vm.EstimationModel.OtherAmount = vm.EstimationModel.OtherItemUnitPrice * vm.EstimationModel.OtherItemNumber;
        vm.estimateGrandCalculation();
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
        $state.go('TranslationOrderDetails', { 'id': vm.EstimationModel.ID, 'Estimation': vm.EstimationModel });
    }

    //var applyButtonState = function () {
    //    var classes = ['.btn-order-details', '.btn-approval-req', '.btn-approval', '.btn-quotation-email', '.btn-quotation-request', '.btn-conf-email', '.btn-delete'];
    //    classes.forEach(function (clazz) {
    //        angular.element(document.querySelector(clazz)).attr('disabled', false);
    //    });
    //    if (vm.EstimationModel.EstimationStatusName === 'Confirmed') {
    //        classes = ['.btn-approval-req', '.btn-approval', '.btn-conf-email'];
    //        vm.ButtonTitles[4] = 'Approval Request Sent';
    //        vm.ButtonTitles[5] = 'Approved';
    //        vm.ButtonTitles[8] = 'Confirmation Email Sent';
    //    } else if (vm.EstimationModel.EstimationStatusName === 'Approved') {
    //        classes = ['.btn-approval-req', '.btn-approval'];
    //        vm.ButtonTitles[4] = 'Approval Request Sent';
    //        vm.ButtonTitles[5] = 'Approved';
    //    } else if (vm.EstimationModel.EstimationStatusName === 'RequestedForApproval') {
    //        classes = ['.btn-approval-req', '.btn-conf-email'];
    //        vm.ButtonTitles[4] = 'Approval Request Sent';
    //    } else if (vm.EstimationModel.EstimationStatusName === 'Created') {
    //        classes = ['.btn-approval', '.btn-conf-email'];
    //    }
    //    classes.forEach(function (clazz) {
    //        angular.element(document.querySelector(clazz)).attr('disabled', true);
    //    });
    //}

    //vm.manageApproval = function (action) {
    //    vm.EstimationModel.PageUrl = $location.absUrl();
    //    vm.EstimationModel.CurrentCulture = currentCulture;
    //    vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
    //    ajaxService.AjaxPostWithData(vm.EstimationModel, 'estimation/' + action, function (r) {
    //        if (action === 'approvalrequest') {
    //            culVar = 'APPROVALREQUESTSENT';
    //            vm.EstimationModel.EstimationStatusName = 'RequestedForApproval';
    //            applyButtonState();
    //        } else if (action === 'approverequest') {
    //            culVar = 'REQUESTAPPROVED';
    //            vm.EstimationModel.EstimationStatusName = 'Approved';
    //            applyButtonState();
    //        }
    //        debugger;
    //        toastr.success($filter('translator')(culVar));
    //    }, errorFunction);
    //}

    //vm.confirmationEmail = function (action) {
    //    vm.EstimationModel.CurrentCulture = currentCulture;
    //    vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
    //    ajaxService.AjaxPostWithData(vm.EstimationModel, 'estimation/emailconfirmation', function (r) {
    //        vm.EstimationModel.EstimationStatusName = 'Confirmed';
    //        applyButtonState();
    //        toastr.success($filter('translator')('CONFIRMATIONEMAIL'));
    //        debugger;
    //    }, errorFunction);
    //}

    vm.tempRegistration = function () {
        vm.EstimationModel.IsTemporaryRegistration = vm.EstimationModel.IsTemporaryRegistration === true ? false : true;
    }

    vm.quotationRequest = function () {
        var title = "Quotation Request"
        $uibModal.open({
            component: "chooseqrwindow",
            transclude: true,
            resolve: {
                title: function () {
                    return title;
                },
                estimationId: function () {
                    return vm.EstimationModel.ID;
                }
            },
            size: ''
        });
    };

    //var manipulateRoleAccess = function (data) {
    //    debugger;
    //    $http({ method: 'POST', url: appSettings.API_BASE_URL + 'estimationroleaccess/getitem', data: data }).then(function (r) {
    //        if (r.data) {
    //            debugger;
    //            var arritems = r.data.Options.split(',');
    //            if (arritems.length) {
    //                arritems.forEach(function (ai) {
    //                    var elmitem = ai.split(':');
    //                    var tobool = Boolean(Number(elmitem[1]));
    //                    angular.element(document).find('.' + elmitem[0]).removeAttr('disabled').attr('disabled', tobool);
    //                });
    //            }
    //        }
    //    }, function (error) { });
    //}
    //manipulateRoleAccess();
}

