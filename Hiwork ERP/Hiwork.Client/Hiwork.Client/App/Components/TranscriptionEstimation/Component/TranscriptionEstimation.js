angular.module("hiworkApp").component('transcriptionEstimation', {
    templateUrl: 'app/Components/TranscriptionEstimation/Template/transcriptionEstimation.html',
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
    controller: transcriptionEstimationController
})

function transcriptionEstimationController($scope, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, $http, $stateParams, $localStorage, $parse, $state, EstimationDefaultStatus, $location) {
    
    var vm = this;
    vm.rowCollection = [];
    vm.TranscriptionEstimationModel = {};
    vm.EstimationAction = {};
    vm.RemarksState = new Array(9);
    var BaseModel = {};
    vm.CulturalItem = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    vm.TranscriptionEstimationModel.IsActionBtnDisable = true;
    vm.ActionTypeList = [
        { Name_en: "Change Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 1 },
        { Name_en: "Sending Email", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 2 },
        { Name_en: "Cancel Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 3 }
    ];

    var dummyEstimationDetailsModel = {
        ID: 0
        , SourceLanguageID: ''
        , TargetLanguageID: ''
        , Contents: ''
        , UnitPrice1: 0
        , LengthMinute: 0
        , EstimationPrice: 0
        , DiscountRate: 0
        , Discount1: 0
        , Total: 0
        , WithTranslation: false
        , ExcludeTax: false
        , IsMarkedForDelete: false
    };
    var dummyActionItemsModel = {        
        EstimationID: ''
        , NextActionDate: ''
        , ActionType: 0
        , OperationBy: ''
        , OperationDate: ''
        , ActionDetails: ''
    }
    vm.ClientStatusList = [
        { Name: "New", No: 1 },
        { Name: "Existing", No: 2 }
    ];

    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    vm.TranscriptionEstimationModel.CurrentUserID = currentUser.CurrentUserID;
    vm.TranscriptionEstimationModel.CurrentCulture = currentCulture;
    vm.TranscriptionEstimationModel.ApplicationId = appSettings.ApplicationId;
    vm.TranscriptionEstimationModel.ExclTaxCost = 0;
    vm.TranscriptionEstimationModel.TaxEstimate = 0;
    vm.TranscriptionEstimationItems = [];
    vm.TranscriptionEstimationModel.ActionItems = [];
    vm.TranscriptionEstimationModel.EstimationAction = _.clone(dummyActionItemsModel);
    vm.Currency = {};
    vm.HeaderBtnAttr = { };
    vm.HeaderBtnAttr = EstimationDefaultStatus.init;
    vm.culturalProperties = ["BillingAddress", "ClientAddress", "BillingCompanyName", "DeliveryCompanyName", "DeliveryAddress", "Remarks", "CoordinatorNotes", "QuotationNotes"];
    vm.ButtonTitles = ['Registration / Update', 'Order Details', 'Temporary Registration', 'Project initiation', 'Approval Request', 'Approval', 'Quotation Email', 'Quotation Request', 'Confirmation Email', 'Delete'];

    vm.$onInit = function () {
         
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        vm.TranscriptionEstimationItems = [].concat(vm.rowCollection);

        for (i = 0; i < vm.currencyList.length; i++) {
            if (vm.currencyList[i].Code == "USD") {
                vm.Currency = vm.currencyList[i];
                vm.changeCurrency();
                break;
            }
        }
        if ($stateParams.id != "" && $stateParams.id != null) {
            manageState();
            mapCulturalItems();
            vm.TranscriptionEstimationModel.InquiryDate = new Date(vm.TranscriptionEstimationModel.InquiryDate);
            vm.TranscriptionEstimationModel.FirstDeliveryDate = new Date(vm.TranscriptionEstimationModel.FirstDeliveryDate);
            vm.TranscriptionEstimationModel.FinalDeliveryDate = new Date(vm.TranscriptionEstimationModel.FinalDeliveryDate);
            
            vm.TranscriptionEstimationModel.IsActionBtnDisable = false;            
            ajaxService.AjaxPostWithData(BaseModel, "transcriptionestimation/list/" + vm.TranscriptionEstimationModel.ID, onGetEstimationDetails, errorFunction);
            if (vm.currencyList) {
                for (i = 0; i < vm.currencyList.length; i++) {
                    if (vm.currencyList[i].Id == vm.TranscriptionEstimationModel.CurrencyID) {
                        vm.Currency = vm.currencyList[i];
                        vm.changeCurrency();
                        break;
                    }
                }
            }
            if (vm.TranscriptionEstimationModel.RemarksCoordinatorType) {
                for (i = 0, j=0; i < vm.TranscriptionEstimationModel.RemarksCoordinatorType.length; i += 2) {
                    if (vm.TranscriptionEstimationModel.RemarksCoordinatorType.charAt(i) == '1') {
                        vm.RemarksState[j] = true;
                    }
                    j += 1;
                }
            }
            vm.HeaderBtnAttr = vm.TranscriptionEstimationModel.PageButtonAttribute;
            loadEstimationActionList();
        } else {
            var indx = 0;
            for (i = 0; i < 1; i++) {
                indx++;
                var item = angular.copy(dummyEstimationDetailsModel);
                item.Index = indx;
                vm.rowCollection.push(item);
                vm.TranscriptionEstimationItems = [].concat(vm.rowCollection);
            }
            vm.TranscriptionEstimationModel.IsActionBtnDisable = true;
        }
        debugger;
    };

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.TranscriptionEstimationModel = $stateParams.Estimation;
            }
            vm.TranscriptionEstimationModel = $localStorage.TranscriptionEstimationModel;
        }
    }

    var mapCulturalItems = function () {
        for (var x = 0; x < vm.culturalProperties.length; x++) {
            var the_variable = 'vm.CulturalItem.' + vm.culturalProperties[x];
            var val = vm.TranscriptionEstimationModel[vm.culturalProperties[x]];
            var model = $parse(the_variable);
            model.assign($scope, val);
        }
    }

    var loadEstimationActionList = function () {        
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.TranscriptionEstimationModel.ID, function (res) {
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
                vm.TranscriptionEstimationModel.ActionItems = [].concat(actionItems);
            }
        }, errorFunction);
    };

    vm.orderDetails = function () {
         
        if ((vm.TranscriptionEstimationModel.ID != 'undefined') || (vm.TranscriptionEstimationModel.ID != null)) {
            $state.go('TranscriptionOrderDetails', { 'id': vm.TranscriptionEstimationModel.ID, 'Estimation': vm.TranscriptionEstimationModel });
        }
        else {
            $state.go('TranscriptionOrderDetails', { 'id': null, 'Estimation': null });
        }
    }

    var onGetEstimationDetails = function (response) {
         
        if (response) {
            for (var i = 0; i < response.length; i++) {
                var totalPrice = (response[i].UnitPrice1 ? (response[i].UnitPrice1 * response[i].LengthMinute) : 0);
                var discountRate = ((response[i].Discount1 * 100) / totalPrice);
                
                DummyEstimationDetailsModel = {
                    ID: response[i].ID
                    , SourceLanguageID: response[i].SourceLanguageID
                    , TargetLanguageID: response[i].TargetLanguageID
                    , Contents: response[i].Contents
                    , UnitPrice1: response[i].UnitPrice1
                    , LengthMinute: response[i].LengthMinute
                    , EstimationPrice: response[i].EstimationPrice
                    , DiscountRate: discountRate
                    , Discount1: response[i].Discount1
                    , Total: totalPrice
                    , WithTranslation: response[i].WithTranslation
                    , ExcludeTax: response[i].ExcludeTax
                    , IsMarkedForDelete: response[i].IsMarkedForDelete
                };

                var item = angular.copy(DummyEstimationDetailsModel);
                vm.rowCollection.push(item);
                vm.TranscriptionEstimationItems = [].concat(vm.rowCollection);
                vm.calculateAmount();
            }
        }
    };

    vm.saveEstimationAction = function (flag) {
        if (!flag) {
            vm.EstimationAction.OperationBy = currentUser.CurrentUserID;
            vm.EstimationAction.EstimationID = vm.TranscriptionEstimationModel.ID;
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

    vm.changeCurrency = function () {
        vm.TranscriptionEstimationModel.CurrencyID = vm.Currency.Id;
    };
    $scope.$on("selectedOutwardSales", function (event, response) {
        vm.TranscriptionEstimationModel.OutwardSalesID = response[0].ID;
    });
    $scope.$on("selectedLargeSales", function (event, response) {
        vm.TranscriptionEstimationModel.LargeSalesID = response[0].ID;
    });
    $scope.$on("selectedApprovals", function (event, response) {
        vm.TranscriptionEstimationModel.ApprovalID = response[0].ID;
    });
    $scope.$on("selectedRepresentative", function (event, response) {
        vm.TranscriptionEstimationModel.SalesPersonID = response[0].ID;
    });
    $scope.$on("selectedprepQuotaion", function (event, response) {
        vm.TranscriptionEstimationModel.AssistantID = response[0].ID;
    });
    $scope.$on("selectedCoordinator", function (event, response) {
        vm.TranscriptionEstimationModel.CoordinatorID = response[0].ID;
    });
    $scope.$on("selectedClient", function (event, response) {
        vm.TranscriptionEstimationModel.ClientID = response[0].ID;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.TranscriptionEstimationModel.TradingID = response[0].ID;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.TranscriptionEstimationModel.TeamID = response[0].ID;
    });

    vm.addItem = function () {
        var item = angular.copy(dummyEstimationDetailsModel);
        vm.rowCollection.push(item);
        vm.TranscriptionEstimationItems = [].concat(vm.rowCollection);
    };
    vm.removeTableRow = function (item) {
        item.IsMarkedForDelete = true;
        //vm.TranscriptionEstimationItems.splice(vm.TranscriptionEstimationItems.indexOf(item), 1);
        //vm.rowCollection.splice(vm.rowCollection.indexOf(item), 1);
        
        vm.calculateAmount();
    };
    vm.prepareQuotationPdf = function () {
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.TranscriptionEstimationModel.RemarksCoordinatorType = RemarksString.trim();
        vm.TranscriptionEstimationModel.TaxEstimation = vm.TaxEstimate;
        vm.TranscriptionEstimationModel.QuotationInclTax = vm.TotalCost;
        vm.TranscriptionEstimationModel.QuotationExclTax = vm.ExclTaxQuotation;
        vm.TranscriptionEstimationModel.ConsumptionOnTax = vm.ConsumptionTax;
        vm.TranscriptionEstimationModel.ExcludeTax = vm.ExclTaxCost;

        $http.post(appSettings.API_BASE_URL + 'transcriptionestimation/generatepdf', vm.TranscriptionEstimationModel, { responseType: 'arraybuffer' })
          .then(function (data) {
              var file = new Blob([data.data], { type: 'application/pdf' });
              fileURL = URL.createObjectURL(file);
              //window.open(fileURL);
              saveAs(file, 'Estimation.pdf');
          });
    };
    vm.openEmailWindow = function () {
        var title = "Estimation Email"
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

    vm.registerQuotaion = function () {
         
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.TranscriptionEstimationModel.SubtotalAfterDiscount = vm.SubTotal;
        vm.TranscriptionEstimationModel.AverageUnitPrice = vm.AvgUnitPrice;
        vm.TranscriptionEstimationModel.QuotationInclTax = vm.TotalPriceIncludeTax;
        vm.TranscriptionEstimationModel.QuotationExclTax = vm.TotalPriceExcludeTax;
        vm.TranscriptionEstimationModel.ConsumptionOnTax = vm.ConsumptionTax;        
        vm.TranscriptionEstimationModel.GrandTotal = vm.TotalWithTax;
        vm.TranscriptionEstimationModel.RemarksCoordinatorType = RemarksString.trim();
        
        var model = {
            'Estimation': vm.TranscriptionEstimationModel,
            'EstimationDetails': vm.TranscriptionEstimationItems,            
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };

        ajaxService.AjaxPostWithData(model, "transcriptionestimation/save", successFunction, errorFunction);
    };

    vm.deleteEstimation = function () {
         
        var index;
        var RemarksString = "";
        var Status = 0;
        for (index = 0; index < vm.RemarksState.length; index += 1) {
            Status = vm.RemarksState[index] ? 1 : 0;
            RemarksString = RemarksString + Status.toString() + " ";
        }
        vm.TranscriptionEstimationModel.SubtotalAfterDiscount = vm.SubTotal;
        vm.TranscriptionEstimationModel.AverageUnitPrice = vm.AvgUnitPrice;
        vm.TranscriptionEstimationModel.QuotationInclTax = vm.TotalPriceIncludeTax;
        vm.TranscriptionEstimationModel.QuotationExclTax = vm.TotalPriceExcludeTax;
        vm.TranscriptionEstimationModel.ConsumptionOnTax = vm.ConsumptionTax;
        vm.TranscriptionEstimationModel.GrandTotal = vm.TotalWithTax;
        vm.TranscriptionEstimationModel.RemarksCoordinatorType = RemarksString.trim();

        var model = {
            'Estimation': vm.TranscriptionEstimationModel,
            'EstimationDetails': vm.TranscriptionEstimationItems,
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };

        ajaxService.AjaxPostWithData(model, "transcriptionestimation/delete", successDeleteFunction, errorFunction);
    };
    
    var successDeleteFunction = function (response) {
        toastr.success($filter('translator')('DATADELETED'));
        $stateParams.id = null;
        $stateParams.Estimation = null;
        $state.reload();
    };
    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };
    var onGetData = function (response) {
        vm.PostCode = response.PostCode;
        setTimeout(function () {
            $scope.$broadcast('export-pdf', {});
        }, 200);
    };
    var onGetError = function (response) {

    };
    vm.calcDiscountAmount = function (item) {
        item.Discount1 = ((item.EstimationPrice / 100) * item.DiscountRate);
        vm.calculateAmount(item);
    };
    vm.calcDiscountRate = function (item) {
        item.DiscountRate = ((item.Discount1 * 100) / item.EstimationPrice);
        vm.calculateAmount(item);
    };
    vm.calculateAmount = function (item) {
        
        vm.SubTotal = 0;
        vm.TotalPriceIncludeTax = 0;
        vm.TotalPriceExcludeTax = 0;
        vm.ConsumptionTax = 0;
        vm.TotalWithoutTax = 0;
        vm.TotalWithTax = 0;        
        vm.EstimationPrice = 0;
        vm.DiscountRate = 0.0;
        var count = 0;
        var sumUnitPrice = 0;
        if ((vm.TranscriptionEstimationModel.ExcludedTaxCost == 'undefined') ||
            (vm.TranscriptionEstimationModel.ExcludedTaxCost == null)) {
            vm.TranscriptionEstimationModel.ExcludedTaxCost = 0;
        }
        _.forEach(vm.TranscriptionEstimationItems, function (f) {
            if (!f.IsMarkedForDelete) {
                if (f.Contents.length > 0) {
                    if (f.UnitPrice1 > 0) {
                        if (vm.TranscriptionEstimationModel.ExcludeTax == 'undefined') {
                            vm.TranscriptionEstimationModel.ExcludeTax = 0;
                        }
                        f.EstimationPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.LengthMinute) : 0);
                        f.Total = (f.EstimationPrice - f.Discount1);
                        vm.SubTotal += f.Total;

                        if (f.ExcludeTax) {
                            vm.TotalPriceExcludeTax += f.Total;
                            vm.ConsumptionTax = ((vm.TotalPriceExcludeTax * 8) / 100);
                        } else {
                            vm.TotalPriceIncludeTax += f.Total;
                        }
                        vm.TotalWithoutTax = vm.TotalPriceExcludeTax + vm.TotalPriceIncludeTax + vm.TranscriptionEstimationModel.ExcludedTaxCost;
                        vm.TotalWithTax = vm.ConsumptionTax + vm.TotalWithoutTax;
                        count++;
                        sumUnitPrice += f.UnitPrice1;
                        vm.AvgUnitPrice = sumUnitPrice / count;
                    }
                }
            }
        });
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
        vm.TranscriptionEstimationModel.AttachedMaterialFileName = response.Document.FileName;
        vm.TranscriptionEstimationModel.AttachedMaterialDownloadURL = response.Document.DownloadURL;
    });

    vm.tempRegistration = function () {
        vm.TranscriptionEstimationModel.IsTemporaryRegistration = vm.TranscriptionEstimationModel.IsTemporaryRegistration === true ? false : true;
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


    vm.manageApproval = function (action) {
        vm.TranscriptionEstimationModel.PageUrl = $location.absUrl();
        vm.TranscriptionEstimationModel.CurrentCulture = currentCulture;
        vm.TranscriptionEstimationModel.CurrentUserID = currentUser.CurrentUserID;
        ajaxService.AjaxPostWithData(vm.TranscriptionEstimationModel, 'estimation/' + action, function (r) {
            if (action === 'approvalrequest') {
                culVar = 'APPROVALREQUESTSENT';
                vm.TranscriptionEstimationModel.EstimationStatusName = 'RequestedForApproval';
                vm.HeaderBtnAttr = r.PageButtonAttribute;
            } else if (action === 'approverequest') {
                culVar = 'REQUESTAPPROVED';
                vm.TranscriptionEstimationModel.EstimationStatusName = 'Approved';
                vm.HeaderBtnAttr = r.PageButtonAttribute;
            }
            vm.TranscriptionEstimationModel.EstimationStatus = r.EstimationStatusID;
            toastr.success($filter('translator')(culVar));
        }, errorFunction);
    }
}