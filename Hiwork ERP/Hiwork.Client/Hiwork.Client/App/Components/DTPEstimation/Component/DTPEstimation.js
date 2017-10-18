angular.module("hiworkApp").component('dtpEstimation', {
    templateUrl: 'app/Components/DTPEstimation/Template/DTPEstimation.html',
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
    controller: dTPEstimationController
})

function dTPEstimationController($scope, $state, $location, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, $http, $stateParams, $parse, $localStorage) {
    var vm = this;
    vm.rowCollection = [];
    vm.DTPEstimation = {};
    vm.EstimationAction = {};
    vm.RemarksState = new Array(6);
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    vm.DTPEstimation.CurrentUserID = currentUser.CurrentUserID;
    vm.DTPEstimation.CurrentCulture = currentCulture;
    vm.DTPEstimation.ApplicationId = appSettings.ApplicationId;
    vm.DTPEstimation.ExclTaxCost = 0;
    vm.DTPEstimation.TaxEstimate = 0;
    vm.PostCode = null;
    vm.DTPEstimationItems = [];
    vm.DTPEstimation.ActionItems = [];
    vm.Versions = [];
    vm.FileTypes = [];
    vm.Contents = [];
    vm.Currency = {};
    vm.CulturalItem = {};

    vm.ButtonTitles = ['Registration / Update', 'Order Details', 'Temporary Registration', 'Project initiation', 'Approval Request', 'Approval', 'Quotation Email', 'Quotation Request', 'Confirmation Email', 'Delete'];

    var dummyFileDocument = {
        FileName: "",
        DownloadURL: ""
    };
    vm.DTPEstimation.Document = _.clone(dummyFileDocument);

    vm.deliveryFileType = ["Illustrator", "InDesign", "Photoshop", "Word (MS)", "Excel (MS)", "PowerPoint (MS)", "JPG", "PDF", "GIF", "Others"];
    vm.version = ["CC", "CS6", "CS5", "CS4", "CS3", "CS2", "10", "GIF"];
    vm.workContent = ["Change Text", "Fix Design", "Prepare .psd", "Outline", "Complecated Design (Map)", "Create New Design"];
    vm.culturalProperties = ["ClientAddress", "BillingCompanyName", "BillingAddress", "DeliveryCompanyName", "DeliveryAddress", "CoordinatorNotes", "Remarks", "OtherItemName"];

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
        DiscountRate: 0,
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
        vm.DTPEstimationItems = [].concat(vm.rowCollection);
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
            vm.DTPEstimation.InquiryDate = new Date(vm.DTPEstimation.InquiryDate);
            vm.DTPEstimation.FirstDeliveryDate = new Date(vm.DTPEstimation.FirstDeliveryDate);
            vm.DTPEstimation.FinalDeliveryDate = new Date(vm.DTPEstimation.FinalDeliveryDate);
            vm.DTPEstimation.CurrentCulture = currentCulture;
            //
            vm.DTPEstimation.IsActionBtnDisable = false;
            ajaxService.AjaxPostWithData(BaseModel, "quotationestimationdetails/list/" + vm.DTPEstimation.ID, onGetEstimationDetails, errorFunction);
            loadFileTypesAndContents();
            j = 0;
            if (vm.DTPEstimation.RemarksCoordinatorType) {
                for (i = 0; i < vm.DTPEstimation.RemarksCoordinatorType.length; i += 2) {
                    if (vm.DTPEstimation.RemarksCoordinatorType.charAt(i) == '1') {
                        vm.RemarksState[j] = true;
                    }
                    j += 1;
                }
            }
            if (vm.currencyList) {
                for (i = 0; i < vm.currencyList.length; i++) {
                    if (vm.currencyList[i].Id == vm.DTPEstimation.CurrencyID) {
                        vm.Currency = vm.currencyList[i];
                        vm.changeCurrency();
                        break;
                    }
                }
            }
            $localStorage.dtpID = $stateParams.id;
            loadEstimationActionList();
            vm.EstimationAccessModel.EstimationStatusID = vm.DTPEstimation.EstimationStatus ? vm.DTPEstimation.EstimationStatus : 0;
            estimationService.manipulateUserAccess(vm.EstimationAccessModel);
        } else {
            var indx = 0;
            for (var i = 0; i < 1; i++) {
                indx++;
                var item = angular.copy(DummyEstimationDetailsModel);
                item.Index = indx;
                vm.rowCollection.push(item);
                vm.DTPEstimationItems = [].concat(vm.rowCollection);
            }
            vm.DTPEstimation.IsActionBtnDisable = true;
        }
        applyButtonState();
    };

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.DtpEstimation = $stateParams.Estimation;
            }
            vm.DTPEstimation = $localStorage.DtpEstimation;
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

    var loadFileTypesAndContents = function () {
        ajaxService.AjaxPostWithData(BaseModel, "dtpestimation/estimationfiletypelist/" + vm.DTPEstimation.ID, function (res) {
            if (res) {
                for (var x = 0; x < res.length; x++) {
                    var ft = res[x].FileType;
                    var index = vm.deliveryFileType.findIndex(x=>x === ft);
                    var the_variable = 'vm.FileTypes' + index;
                    var model = $parse(the_variable);
                    model.assign($scope, 1);
                    vm.Versions[index] = res[x].Version;
                }
            }
        }, errorFunction);
        ajaxService.AjaxPostWithData(BaseModel, "dtpestimation/estimationworkcontentlist/" + vm.DTPEstimation.ID, function (res) {
            if (res) {
                for (var x = 0; x < res.length; x++) {
                    var wc = res[x].WorkContent;
                    var index = vm.workContent.findIndex(x=>x === wc);
                    var the_variable = 'vm.Contents' + index;
                    var model = $parse(the_variable);
                    model.assign($scope, 1);
                }
            }
        }, errorFunction);
    }

    var onGetEstimationDetails = function (response) {
        if (response) {
            for (var i = 0; i < response.length; i++) {
                var totalPrice = (response[i].UnitPrice1 ? (response[i].UnitPrice1 * response[i].PageCount1) : 0);
                var discountRate = ((response[i].Discount1 * 100) / totalPrice);
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
                vm.DTPEstimationItems = [].concat(vm.rowCollection);

                vm.calculateAmount();
            }
        }
    };

    vm.changeCurrency = function () {
        vm.DTPEstimation.CurrencyID = vm.Currency.Id;
    };

    $scope.$on("selectedOutwardSales", function (event, response) {
        vm.DTPEstimation.OutwardSalesID = response[0].ID;
    });
    $scope.$on("selectedLargeSales", function (event, response) {
        vm.DTPEstimation.LargeSalesID = response[0].ID;
    });
    $scope.$on("selectedApprovals", function (event, response) {
        vm.DTPEstimation.ApprovalID = response[0].ID;
    });
    $scope.$on("selectedRepresentative", function (event, response) {
        vm.DTPEstimation.SalesPersonID = response[0].ID;
    });
    $scope.$on("selectedprepQuotaion", function (event, response) {
        vm.DTPEstimation.AssistantID = response[0].ID;
    });
    $scope.$on("selectedCoordinator", function (event, response) {
        vm.DTPEstimation.CoordinatorID = response[0].ID;
    });
    $scope.$on("selectedClient", function (event, response) {
        vm.DTPEstimation.ClientID = response[0].ID;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.DTPEstimation.TradingID = response[0].Id;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.DTPEstimation.AffiliateTeamID = response[0].Id;
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
        vm.DTPEstimationItems = [].concat(vm.rowCollection);
    };

    vm.prepareQuotationPdf = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.DTPEstimation.RemarksCoordinatorType = RemarksString.trim();
        vm.DTPEstimation.TaxEstimation = vm.TaxEstimate;
        vm.DTPEstimation.QuotationInclTax = vm.TotalCost;
        vm.DTPEstimation.QuotationExclTax = vm.ExclTaxQuotation;
        vm.DTPEstimation.ConsumptionOnTax = vm.ConsumptionTax;

        $http.post(appSettings.API_BASE_URL + 'taskquotation/generatepdf', vm.DTPEstimation, { responseType: 'arraybuffer' })
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
            vm.EstimationAction.EstimationID = vm.DTPEstimation.ID;
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
        vm.DTPEstimation.RemarksCoordinatorType = RemarksString.trim();
        vm.DTPEstimation.TaxEstimation = vm.TaxEstimate;
        vm.DTPEstimation.GrandTotal = vm.TotalCost;
        vm.DTPEstimation.QuotationExclTax = vm.ExclTaxQuotation;
        vm.DTPEstimation.ConsumptionOnTax = vm.ConsumptionTax;

        var itemContents = document.getElementsByClassName('chk-content');
        var itemFiletype = document.getElementsByClassName('chk-filetype');
        var typeItems = [];
        var contentItems = [];
        for (var x = 0; x < itemFiletype.length; x++) {
            if (itemFiletype[x].checked) {
                var obj = {
                    FileType: itemFiletype[x].value,
                    Version: vm.Versions[x]
                };
                typeItems.push(obj);
            }
        }
        for(var x = 0; x < itemContents.length; x++) {
            if (itemContents[x].checked) {
                var obj = {
                    WorkContent: itemContents[x].value
                };
                contentItems.push(obj);
            }
        }
        vm.DTPEstimation.DeliveryFileTypes = [].concat(typeItems);
        vm.DTPEstimation.WorkContents = [].concat(contentItems);

        var model = {
            'Estimation': vm.DTPEstimation,
            'EstimationDetails': vm.DTPEstimationItems,
            'FileTypes': vm.DTPEstimation.DeliveryFileTypes,
            'WorkContents': vm.DTPEstimation.WorkContents,
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };
        ajaxService.AjaxPostWithData(model, "dtpestimation/save", successFunction, errorFunction);
    };

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    vm.removeTableRow = function (item) {
        vm.DTPEstimationItems.splice(vm.DTPEstimationItems.indexOf(item), 1);
        vm.rowCollection.splice(vm.rowCollection.indexOf(item), 1);
        if (vm.DTPEstimationItems.length === 0) {
            vm.DTPEstimation.ExcludedTaxCost = 0;
            vm.TotalCost = 0;
        }
        vm.calculateAmount();
    };

    vm.calculateDiscountRateOrValue = function (f, type) {
        var flag = '';
        var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
        if (type === 'rate') {
            f.Discount1 = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
            flag = 'd';
            //if (f.Discount1 > totalPrice) {
            //    f.DiscountRate = 0;
            //    return false;
            //}
        } else {
            f.DiscountRate = ((f.Discount1 * 100) / totalPrice);
            flag = 'r';
        }
        vm.calculateAmount(null, flag);
    };

    var loadEstimationActionList = function () {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.DTPEstimation.ID, function (res) {
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
                vm.DTPEstimation.ActionItems = [].concat(actionItems);
            }
        }, errorFunction);
    };

    vm.calculateAmount = function (item, flag) {
        vm.TaxEstimate = 0; vm.ExclTaxQuotation = 0; vm.ConsumptionTax = 0;
        vm.ExclTaxCost = 0; vm.TotalCost = 0; vm.AverageUnitPrice = 0; vm.Subtotal = 0;
        var count = 0;
        var itemIndex = 0;
        var totalUnitPrice = 0;
        _.forEach(vm.DTPEstimationItems, function (f) {
            if (f.UnitPrice1 > 0) {
                var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
                totalUnitPrice += f.UnitPrice1;
                var totalDiscount = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
                f.ItemTotal = totalPrice - totalDiscount;
                vm.Subtotal += f.ItemTotal;
                if (flag === 'r') {
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
        vm.AverageUnitPrice = totalUnitPrice / vm.DTPEstimationItems.length;
        if (typeof vm.DTPEstimation.ExcludedTaxCost != 'undefined') {
            vm.TotalCost = vm.TotalCost - vm.DTPEstimation.ExcludedTaxCost;
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
        vm.DTPEstimation.AttachedMaterialFileName = response.Document.FileName;
        vm.DTPEstimation.AttachedMaterialDownloadURL = response.Document.DownloadURL;
        vm.DTPEstimation.Document.TranslationText = response.Document.TranslationText;
        vm.DTPEstimation.Document.WordCount = response.Document.WordCount;
    });

    vm.deleteEstimationAction = function (item) {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/delete/" + item.ID, function (response) {
            toastr.success($filter('translator')('DATASAVED'));
            loadEstimationActionList();
        }, errorFunction);
    }

    vm.orderDetails = function () {
        $state.go('DTPOrderDetails', { 'id': vm.DTPEstimation.ID, 'Estimation': vm.DTPEstimation });
    }

    var applyButtonState = function () {
        var classes = ['.btn-order-details', '.btn-approval-req', '.btn-approval', '.btn-quotation-email', '.btn-quotation-request', '.btn-conf-email', '.btn-delete'];
        classes.forEach(function (clazz) {
            angular.element(document.querySelector(clazz)).attr('disabled', false);
        });
        if (vm.DTPEstimation.EstimationStatusName === 'Under_estimation') {
            classes = ['.btn-order-details', '.btn-approval', '.btn-quotation-email', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
        }
        else if (vm.DTPEstimation.EstimationStatusName === 'Waiting_for_approval') {
            classes = ['.btn-order-details', '.btn-approval-req', '.btn-quotation-email', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
        }
        else if (vm.DTPEstimation.EstimationStatusName === 'Approved') {
            classes = ['.btn-order-details', '.btn-approval', '.btn-conf-email', '.btn.btn-primary.pull-left.order'];
            vm.ButtonTitles[4] = 'Approval Request Sent';
            vm.ButtonTitles[5] = 'Approved';
        }
        else if (vm.DTPEstimation.EstimationStatusName === 'Waiting_for_confirmation') {
            classes = ['.btn-order-details', '.btn-approval', '.btn.btn-primary.pull-left.order'];
        }
        else if (vm.DTPEstimation.EstimationStatusName === 'Ordered' || vm.DTPEstimation.EstimationStatus > 5) {
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
        vm.DTPEstimation.PageUrl = $location.absUrl();
        vm.DTPEstimation.CurrentCulture = currentCulture;
        vm.DTPEstimation.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.DTPEstimation, 'estimation/' + action, function (r) {
            if (action === 'approvalrequest') {
                culVar = 'APPROVALREQUESTSENT';
                vm.DTPEstimation.EstimationStatusName = r.EstimationStatusName;//'Waiting_for_approval';
                applyButtonState();
            } else if (action === 'approverequest') {
                culVar = 'REQUESTAPPROVED';
                vm.DTPEstimation.EstimationStatusName = r.EstimationStatusName;// 'Approved';
                applyButtonState();
            }
            toastr.success($filter('translator')(culVar));
        }, errorFunction);
    }

    vm.orderStatus = function () {
        vm.DTPEstimation = $localStorage.DtpEstimation;
        vm.DTPEstimation.CurrentCulture = currentCulture;
        vm.DTPEstimation.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.DTPEstimation, 'estimation/orderstatus', function (r) {
            toastr.success($filter('translator')('ORDERSTATUS'));
        }, errorFunction);
    };
    vm.orderLoss = function () {
        vm.DTPEstimation = $localStorage.DtpEstimation;
        vm.DTPEstimation.CurrentCulture = currentCulture;
        vm.DTPEstimation.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.DTPEstimation, 'estimation/orderloss', function (r) {
            toastr.success($filter('translator')('ORDERSTATUSLOSS'));
        }, errorFunction);
    }

    vm.confirmationEmail = function (action) {
        vm.DTPEstimation.CurrentCulture = currentCulture;
        vm.DTPEstimation.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.DTPEstimation, 'estimation/emailconfirmation', function (r) {
            vm.DTPEstimation.EstimationStatusName = 'Confirmed';
            applyButtonState();
            toastr.success($filter('translator')('CONFIRMATIONEMAIL'));
        }, errorFunction);
    }

    vm.tempRegistration = function () {
        vm.DTPEstimation.IsTemporaryRegistration = vm.DTPEstimation.IsTemporaryRegistration === true ? false : true;
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
                    return vm.DTPEstimation.ID;
                }
            },
            size: ''
        });
    };
}