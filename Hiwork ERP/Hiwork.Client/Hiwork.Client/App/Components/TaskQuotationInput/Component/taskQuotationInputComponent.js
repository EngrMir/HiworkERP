
angular.module("hiworkApp").component('taskQuotation', {
    templateUrl: 'app/Components/TaskQuotationInput/Template/taskQuotationInput.html',
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
    controller: taskQuotationInputController
})

function taskQuotationInputController($scope, $location, $state, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, $http, $stateParams, $parse, $localStorage) {
    var vm = this;
    vm.rowCollection = [];
    vm.TaskQuotationModel = {};
    vm.EstimationAction = {};
    vm.RemarksState = new Array(9);
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    debugger;
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    vm.TaskQuotationModel.CurrentUserID = currentUser.CurrentUserID;
    vm.TaskQuotationModel.CurrentCulture = currentCulture;
    vm.TaskQuotationModel.ApplicationId = appSettings.ApplicationId;
    vm.TaskQuotationModel.ExclTaxCost = 0;
    vm.TaskQuotationModel.TaxEstimate = 0;
    vm.PostCode = null;
    vm.TaskQuotationModel.TaskQuotationItems = [];
    vm.TaskQuotationModel.ActionItems = [];
    vm.Currency = {};

    vm.culturalProperties = ["ClientAddress", "BillingCompanyName", "BillingAddress", "DeliveryCompanyName", "DeliveryAddress", "CoordinatorNotes", "Remarks", "OtherItemName"];
    vm.ButtonTitles = ['Registration / Update', 'Order Details', 'Temporary Registration', 'Project initiation', 'Approval Request', 'Approval', 'Quotation Email', 'Quotation Request', 'Confirmation Email', 'Delete'];

    vm.ActionTypeList = [
        { Name_en: "Change Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 1 },
        { Name_en: "Sending Email", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 2 },
        { Name_en: "Cancel Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 3 }
    ];

    //vm.ClientStatusList = [
    //    { Name_en: "New", Name_jp: "New", Name_cn: "New", Name_fr: "New", Name_kr: "New", Name_tl: "New", No: 1 },
    //    { Name_en: "Existing", Name_jp: "Existing", Name_cn: "Existing", Name_fr: "Existing", Name_kr: "Existing", Name_tl: "Existing", No: 2 }
    //];
    vm.ClientStatusList = [
        { Name: "New", No: 1 },
        { Name: "Existing", No: 2 }
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
    vm.EstimationAccessModel = {
        UserID: sessionFactory.GetObject(AppStorage.userData).CurrentUserID,
        EstimationTypeID: 1,
        EstimationStatusID: 0
    }
    vm.$onInit = function () {
        estimationService.manipulateUserAccess(vm.TaskQuotationModel);
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        vm.TaskQuotationModel.TaskQuotationItems = [].concat(vm.rowCollection);
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
            //mapCulturalItems();
            //Date formation
            vm.TaskQuotationModel.InquiryDate = new Date(vm.TaskQuotationModel.InquiryDate);
            vm.TaskQuotationModel.FirstDeliveryDate = new Date(vm.TaskQuotationModel.FirstDeliveryDate);
            vm.TaskQuotationModel.FinalDeliveryDate = new Date(vm.TaskQuotationModel.FinalDeliveryDate);
            vm.TaskQuotationModel.CurrentCulture = currentCulture;
           
            vm.TaskQuotationModel.IsActionBtnDisable = false;
            ajaxService.AjaxPostWithData(BaseModel, "quotationestimationdetails/list/" + vm.TaskQuotationModel.ID, onGetEstimationDetails, errorFunction);
            j = 0;
            if (vm.TaskQuotationModel.RemarksCoordinatorType) {
                for (i = 0; i < vm.TaskQuotationModel.RemarksCoordinatorType.length; i += 2) {
                    if (vm.TaskQuotationModel.RemarksCoordinatorType.charAt(i) == '1') {
                        vm.RemarksState[j] = true;
                    }
                    j += 1;
                }
            }
            if (vm.currencyList) {
                for (i = 0; i < vm.currencyList.length; i++) {
                    if (vm.currencyList[i].Id == vm.TaskQuotationModel.CurrencyID) {
                        vm.Currency = vm.currencyList[i];
                        vm.changeCurrency();
                        break;
                    }
                }
            }
            $localStorage.TaskEstimation = $stateParams.Estimation;
            $localStorage.TaskID = $stateParams.id;
            loadEstimationActionList();
            vm.EstimationAccessModel.EstimationStatusID = vm.TaskQuotationModel.EstimationStatus ? vm.TaskQuotationModel.EstimationStatus : 0;
            estimationService.manipulateUserAccess(vm.EstimationAccessModel);
        } else {
            var indx = 0;
            for (var i = 0; i < 1; i++) {
                indx++;
                var item = angular.copy(DummyEstimationDetailsModel);
                item.Index = indx;
                vm.rowCollection.push(item);
                vm.TaskQuotationModel.TaskQuotationItems = [].concat(vm.rowCollection);
            }
            vm.TaskQuotationModel.IsActionBtnDisable = true;
        }
        applyButtonState();
    };

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.QuotationEstimation = $stateParams.Estimation;
            }
            vm.TaskQuotationModel = $localStorage.QuotationEstimation;
        }
    }

    var mapCulturalItems = function () {
        for (var x = 0; x < vm.culturalProperties.length; x++) {
            var the_variable = 'vm.CulturalItem.' + vm.culturalProperties[x];
            var val = vm.DTPEstimation[vm.culturalProperties[x]];
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
                vm.TaskQuotationModel.TaskQuotationItems = [].concat(vm.rowCollection);

                vm.calculateAmount();
            }
        }
    };

    vm.changeCurrency = function () {
        vm.TaskQuotationModel.CurrencyID = vm.Currency.Id;
    };

    $scope.$on("selectedOutwardSales", function (event, response) {
        vm.TaskQuotationModel.OutwardSalesID = response[0].ID;
    });
    $scope.$on("selectedLargeSales", function (event, response) {
        vm.TaskQuotationModel.LargeSalesID = response[0].ID;
    });
    $scope.$on("selectedApprovals", function (event, response) {
        vm.TaskQuotationModel.ApprovalID = response[0].ID;
    });
    $scope.$on("selectedRepresentative", function (event, response) {
        vm.TaskQuotationModel.SalesPersonID = response[0].ID;
    });
    $scope.$on("selectedprepQuotaion", function (event, response) {
        vm.TaskQuotationModel.AssistantID = response[0].ID;
    });
    $scope.$on("selectedCoordinator", function (event, response) {
        vm.TaskQuotationModel.CoordinatorID = response[0].ID;
    });
    $scope.$on("selectedClient", function (event, response) {
        vm.TaskQuotationModel.ClientID = response[0].ID;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.TaskQuotationModel.TradingID = response[0].Id;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.TaskQuotationModel.AffiliateTeamID = response[0].Id;
    });

    vm.addItem = function () {
        DummyEstimationDetailsModel = {
            ID: 0,
            ExcludeTax: false,
            Contents: '',
            UnitPrice1: 0,
            PageCount1: 0,
            DiscountRate: 0,
            Discount1: 0,
            Total: 0,
            Index: 0,
            SourceLanguageID: 0,
            TargetLanguageID: 0
        };
        var item = angular.copy(DummyEstimationDetailsModel);
        vm.rowCollection.push(item);
        vm.TaskQuotationModel.TaskQuotationItems = [].concat(vm.rowCollection);
    };

    vm.prepareQuotationPdf = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.TaskQuotationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.TaskQuotationModel.TaxEstimation = vm.TaxEstimate;
        vm.TaskQuotationModel.QuotationInclTax = vm.TotalCost;
        vm.TaskQuotationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.TaskQuotationModel.ConsumptionOnTax = vm.ConsumptionTax;
        
        $http.post(appSettings.API_BASE_URL + 'taskquotation/generatepdf', vm.TaskQuotationModel, { responseType: 'arraybuffer' })
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
            vm.EstimationAction.EstimationID = vm.TaskQuotationModel.ID;
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
                vm.EstimationAction = null;
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
        vm.TaskQuotationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.TaskQuotationModel.TaxEstimation = vm.TaxEstimate;
        vm.TaskQuotationModel.GrandTotal = vm.TotalCost;
        vm.TaskQuotationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.TaskQuotationModel.ConsumptionOnTax = vm.ConsumptionTax;

        var model = {
            'Estimation': vm.TaskQuotationModel,
            'EstimationDetails': vm.TaskQuotationModel.TaskQuotationItems,
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };
       ajaxService.AjaxPostWithData(model, "taskquotation/save", successFunction, errorFunction);
    };

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    vm.removeTableRow = function (item) {
        vm.TaskQuotationModel.TaskQuotationItems.splice(vm.TaskQuotationModel.TaskQuotationItems.indexOf(item), 1);
        vm.rowCollection.splice(vm.rowCollection.indexOf(item), 1);
        if (vm.TaskQuotationModel.TaskQuotationItems.length === 0) {
            vm.TaskQuotationModel.ExcludedTaxCost = 0;
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
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.TaskQuotationModel.ID, function (res) {
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
                vm.TaskQuotationModel.ActionItems = [].concat(actionItems);
            }
        }, errorFunction);
    };

    vm.calculateAmount = function (item, flag) {
        vm.TaxEstimate = 0; vm.ExclTaxQuotation = 0; vm.ConsumptionTax = 0;
        vm.ExclTaxCost = 0; vm.TotalCost = 0; vm.AverageUnitPrice = 0; vm.Subtotal = 0;
        var count = 0;
        var itemIndex = 0;
        var totalUnitPrice = 0;
        _.forEach(vm.TaskQuotationModel.TaskQuotationItems, function (f) {
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
        vm.AverageUnitPrice = totalUnitPrice / vm.TaskQuotationModel.TaskQuotationItems.length;
        if (typeof vm.TaskQuotationModel.ExcludedTaxCost != 'undefined') {
            vm.TotalCost = vm.TotalCost - vm.TaskQuotationModel.ExcludedTaxCost;
        }
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
        vm.TaskQuotationModel.AttachedMaterialFileName = response.Document.FileName;
        vm.TaskQuotationModel.AttachedMaterialDownloadURL = response.Document.DownloadURL;
        vm.TaskQuotationModel.Document.TranslationText = response.Document.TranslationText;
        vm.TaskQuotationModel.Document.WordCount = response.Document.WordCount;
    });

    vm.deleteEstimationAction = function (item) {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/delete/" + item.ID, function (response) {
            toastr.success($filter('translator')('DATASAVED'));
            loadEstimationActionList();
        }, errorFunction);
    }

    vm.orderDetails = function () {
        $state.go('TaskOrderDetails', { 'id': vm.TaskQuotationModel.ID, 'Estimation': vm.TaskQuotationModel });
    }

    var applyButtonState = function () {
        var classes = ['.btn-order-details', '.btn-approval-req', '.btn-approval', '.btn-quotation-email', '.btn-quotation-request', '.btn-conf-email', '.btn-delete'];
        classes.forEach(function (clazz) {
          angular.element(document.querySelector(clazz)).attr('disabled', false);
        });
        if (vm.TaskQuotationModel.EstimationStatusName === 'Under_estimation') {
            classes = ['.btn-order-details', '.btn-approval', '.btn-quotation-email', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];        
        }
        else if (vm.TaskQuotationModel.EstimationStatusName === 'Waiting_for_approval') {
            classes = ['.btn-order-details', '.btn-approval-req', '.btn-quotation-email', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
        }
        else if (vm.TaskQuotationModel.EstimationStatusName === 'Approved') {
            classes = ['.btn-order-details', '.btn-approval', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
            vm.ButtonTitles[5] = 'Approved';
        }
        else if (vm.TaskQuotationModel.EstimationStatusName === 'Waiting_for_confirmation') {
            classes = ['.btn-order-details', '.btn-approval', '.btn.btn-primary.pull-left.order'];
        }
        else if (vm.TaskQuotationModel.EstimationStatusName === 'Ordered' || vm.TaskQuotationModel.EstimationStatus > 5) {
            classes = ['.btn-temp-reg', '.btn.btn-primary.pull-left.initi', '.btn-approval-req', '.btn-approval', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
            vm.ButtonTitles[5] = 'Approved';
            vm.ButtonTitles[8] = 'Confirmation Email Sent';
        } 
        classes.forEach(function (clazz) {
           angular.element(document.querySelector(clazz)).attr('disabled', true);
        });
    }

    vm.manageApproval = function (action) {
        vm.TaskQuotationModel.PageUrl = $location.absUrl();
        vm.TaskQuotationModel.CurrentCulture = currentCulture;
        vm.TaskQuotationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.TaskQuotationModel, 'estimation/' + action, function (r) {
            if (action === 'approvalrequest') {
                culVar = 'APPROVALREQUESTSENT';
                vm.TaskQuotationModel.EstimationStatusName = 'RequestedForApproval';
                applyButtonState();
            } else if (action === 'approverequest') {
                culVar = 'REQUESTAPPROVED';
                vm.TaskQuotationModel.EstimationStatusName = 'Approved';
                applyButtonState();
            }
            toastr.success($filter('translator')(culVar));
        }, errorFunction);
    }

    vm.confirmationEmail = function (action) {
        vm.TaskQuotationModel.CurrentCulture = currentCulture;
        vm.TaskQuotationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.TaskQuotationModel, 'estimation/emailconfirmation', function (r) {
            vm.TaskQuotationModel.EstimationStatusName = 'Confirmed';
            applyButtonState();
            toastr.success($filter('translator')('CONFIRMATIONEMAIL'));
        }, errorFunction);
    }
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

    vm.orderStatus = function () {
       vm.TaskQuotationModel.CurrentCulture = currentCulture;
       vm.TaskQuotationModel.CurrentUserID = currentUser.CurrentUserID;
       ajaxService.AjaxPostWithData(vm.TaskQuotationModel, 'estimation/orderstatus', function (r) {
           toastr.success($filter('translator')('ORDERSTATUS'));
       }, errorFunction);
    };
    vm.orderLoss = function () {
        vm.TaskQuotationModel.CurrentCulture = currentCulture;
        vm.TaskQuotationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.TaskQuotationModel, 'estimation/orderloss', function (r) {
            toastr.success($filter('translator')('ORDERSTATUSLOSS'));
        }, errorFunction);
    }
    vm.tempRegistration = function () {
        vm.TaskQuotationModel.IsTemporaryRegistration = vm.TaskQuotationModel.IsTemporaryRegistration === true ? false : true;
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
                    return vm.TaskQuotationModel.ID;
                }
            },
            size: ''
        });
    };
}

