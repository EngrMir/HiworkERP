
angular.module("hiworkApp").component('transOrder', {
    templateUrl: 'app/Components/TranslationEstimationOrderDetails/Template/translationEstimationOrderDetails.html',
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
    controller: translationOrderDetailsController
})

function translationOrderDetailsController($scope, $compile, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, $http, $stateParams, $parse, $localStorage) {
    var vm = this;
    vm.rowCollection = [];
    vm.EstimationModel = {};
    vm.EstimationAction = {};
    vm.RemarksState = new Array(9);
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;
    vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
    vm.EstimationModel.CurrentCulture = currentCulture;
    vm.EstimationModel.ApplicationId = appSettings.ApplicationId;
    vm.EstimationModel.ExclTaxCost = 0;
    vm.EstimationModel.TaxEstimate = 0;
    vm.PostCode = null;
    vm.EstimationModel.EstimationItems = [];
    vm.EstimationModel.ActionItems = [];
    vm.OrderModel = {};
    vm.OrderModel.OrderDetails = [];
    vm.OrderDetails = [];
    vm.Currency = {};
    vm.TempArray = [];
    var calEvents = [];
    var defaultCalDate = null;

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    //vm.culturalProperties = ["ClientAddress", "BillingCompanyName", "BillingAddress", "DeliveryCompanyName", "DeliveryAddress", "CoordinatorNotes", "Remarks", "OtherItemName"];

    vm.ActionTypeList = [
        { Name_en: "Change Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 1 },
        { Name_en: "Sending Email", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 2 },
        { Name_en: "Cancel Quotation", Name_jp: "Change Quotation", Name_cn: "Change Quotation", Name_fr: "Change Quotation", Name_kr: "Change Quotation", Name_tl: "Change Quotation", No: 3 }
    ];

    vm.ClientStatusList = [
        { Name_en: "New", Name_jp: "New", Name_cn: "New", Name_fr: "New", Name_kr: "New", Name_tl: "New", No: 1 },
        { Name_en: "Existing", Name_jp: "Existing", Name_cn: "Existing", Name_fr: "Existing", Name_kr: "Existing", Name_tl: "Existing", No: 2 }
    ];

    vm.$onInit = function () {
        if ($stateParams.id != "" && $stateParams.id != null && $localStorage.TranslationEstimation != null && $stateParams.id === $localStorage.TranslationEstimation.ID) {
            vm.EstimationModel = $localStorage.TranslationEstimation;
            vm.EstimationModel.InquiryDate = moment(vm.EstimationModel.InquiryDate).format('DD/MM/YYYY');
            vm.EstimationModel.FirstDeliveryDate = moment(vm.EstimationModel.FirstDeliveryDate).format('DD/MM/YYYY');
            //vm.EstimationModel.FinalDeliveryDate = moment(vm.EstimationModel.FinalDeliveryDate).format('DD/MM/YYYY');
            vm.EstimationModel.CurrentCulture = currentCulture;
            vm.EstimationModel.IsActionBtnDisable = false;
            //load order and order details
            loadOrder();

            ajaxService.AjaxPostWithData(BaseModel, "orderdetails/estimationorderdetailslist/" + vm.EstimationModel.ID, function (r) {
                if (r) {
                    vm.EstimationModel.EstimationItems = [].concat(r);
                    for (var x = 0; x < vm.EstimationModel.EstimationItems.length; x++) {
                        vm.EstimationModel.EstimationItems[x].DeliveryDate = new Date(vm.EstimationModel.EstimationItems[x].DeliveryDate);
                    }
                    if (vm.EstimationModel.EstimationItems.length > 0) {
                        prepareEventObject(r, null, false);
                    }
                }
            }, errorFunction);
            //loadEstimationActionList();
        } else {
            toastr.error("Estimation could not found");
        }
    };

    var loadOrder = function () {
        ajaxService.AjaxPostWithData(BaseModel, "order/orderitem/" + vm.EstimationModel.ID, function (r) {
            if (r.RegistrationID > 0) {
                vm.OrderModel = r;
                vm.OrderModel.ComplainDate = new Date(vm.OrderModel.ComplainDate);
                vm.OrderModel.ResponseComplainDate = new Date(vm.OrderModel.ResponseComplainDate);
                loadOrderDetails();
            }
        }, errorFunction);
    }

    var loadOrderDetails = function () {
        if (vm.OrderModel.ID) {
            ajaxService.AjaxPostWithData(BaseModel, "orderdetails/list/" + vm.OrderModel.ID, function (r) {
                if (r) {
                    vm.OrderDetails = [].concat(r);
                }
            }, errorFunction);
        }
    }

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
        vm.EstimationModel.ClientID = response[0].ID;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.EstimationModel.TradingID = response[0].Id;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.EstimationModel.AffiliateTeamID = response[0].Id;
    });
    $scope.$on("selectedStaff", function (event, response) {
        //vm.EstimationModel.StaffID = response[0].ID;
        vm.EstimationModel.StaffID[0] = response[0].ID;
        vm.OrderDetails = vm.OrderDetails.filter(function (item) {
            return item.EstimationDetailsID !== response[0].Item.EstimationDetailID;
        });
        var obj = {
            OrderID: vm.OrderModel.ID,
            EstimationDetailsID: response[0].Item.EstimationDetailsID,
            EstimationID: vm.EstimationModel.ID,
            StaffID: response[0].ID,
            WorkingStatus: response[0].Item.Condition,
            DeliveryDate: response[0].Item.DeliveryDate,
            EvaluationAmount: 0,
            PaymentAmount: 0,
            AgencyCommission: 0,
            ComplainDetails: 'test',
            RemitanceDate: new Date(),
            DepositAmount: 0,

            StaffNo: '12121',
            StaffName: response[0].StaffName,
            EmailAddress: response[0].StaffEmailID,
            UnitPrice1: response[0].Item.UnitPrice1,
            PageCount1: response[0].Item.PageCount1,
            Evaluation: 'test',
            Penalty: 'test',
            Residence: 'test'
        };
        vm.OrderDetails.push(obj);
    });

    //vm.prepareQuotationPdf = function () {
    //    var index;
    //    var RemarksString = "";
    //    var Status = 0;
    //    for (index = 0; index < vm.RemarksState.length; index += 1) {
    //        Status = vm.RemarksState[index] ? 1 : 0;
    //        RemarksString = RemarksString + Status.toString() + " ";
    //    }
    //    vm.EstimationModel.RemarksCoordinatorType = RemarksString.trim();
    //    vm.EstimationModel.TaxEstimation = vm.TaxEstimate;
    //    vm.EstimationModel.QuotationInclTax = vm.TotalCost;
    //    vm.EstimationModel.QuotationExclTax = vm.ExclTaxQuotation;
    //    vm.EstimationModel.ConsumptionOnTax = vm.ConsumptionTax;
        
    //    $http.post(appSettings.API_BASE_URL + 'taskquotation/generatepdf', vm.EstimationModel, { responseType: 'arraybuffer' })
    //      .then(function (data) {
    //          console.log(data);
    //          var file = new Blob([data.data], { type: 'application/pdf' });
    //          //fileURL = URL.createObjectURL(file);
    //          //window.open(fileURL);
    //          saveAs(file, 'Quotation.pdf');
    //      });
    //};

    //vm.saveEstimationAction = function (flag) {
    //    if (!flag) {
    //        vm.EstimationAction.OperationBy = currentUser.CurrentUserID;
    //        vm.EstimationAction.EstimationID = vm.EstimationModel.ID;
    //        var model = {
    //            'EstimationAction': vm.EstimationAction,
    //            'CulturalItem': vm.CulturalItem,
    //            'CurrentUserID': currentUser.CurrentUserID,
    //            'ApplicationID': appSettings.ApplicationId,
    //            'Culture': currentCulture
    //        };
    //        ajaxService.AjaxPostWithData(model, "estimationaction/save", function (response) {
    //            toastr.success($filter('translator')('DATASAVED'));
    //            loadEstimationActionList();
    //            vm.EstimationAction.NextActionDate = '';
    //            vm.EstimationAction.ActionDetails = '';
    //            vm.EstimationAction.ActionType = '';
    //        }, errorFunction);
    //    }
    //};

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    //vm.removeTableRow = function (item) {
    //    vm.EstimationModel.TaskQuotationItems.splice(vm.EstimationModel.TaskQuotationItems.indexOf(item), 1);
    //    vm.rowCollection.splice(vm.rowCollection.indexOf(item), 1);
    //    if (vm.EstimationModel.TaskQuotationItems.length === 0) {
    //        vm.EstimationModel.ExcludedTaxCost = 0;
    //        vm.TotalCost = 0;
    //    }
    //    vm.calculateAmount();
    //};

    //vm.calculateDiscountRateOrValue = function (f, type) {
    //    var flag = '';
    //    f.DiscountRate = f.DiscountRate > 50 ? 50 : f.DiscountRate === 0 ? 10 : f.DiscountRate;
    //    var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
    //    if (type === 'rate') {
    //        f.Discount1 = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
    //        flag = 'd';
    //        //if (f.Discount1 > totalPrice) {
    //        //    f.DiscountRate = 0;
    //        //    return false;
    //        //}
    //    } else {
    //        f.Discount1 = f.Discount1 > (totalPrice / 2) ? (totalPrice / 2) : f.Discount1;
    //        f.DiscountRate = totalPrice === 0 ? 0 : ((f.Discount1 * 100) / totalPrice);
    //        flag = 'r';
    //    }
    //    vm.calculateAmount(null, flag);
    //};

    //var loadEstimationActionList = function () {
    //    ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.EstimationModel.ID, function (res) {
    //        if (res) {
    //            var actionItems = [];
    //            for (var x = 0; x < res.length; x++) {
    //                var actionItem = {
    //                    ID: res[x].ID,
    //                    EstimationID: res[x].EstimationID,
    //                    Date: res[x].NextActionDate,
    //                    Updatedby: res[x].OperandName,
    //                    Detail: res[x].ActionDetails
    //                };
    //                actionItems.push(actionItem);
    //            }
    //            vm.EstimationModel.ActionItems = [].concat(actionItems);
    //        }
    //    }, errorFunction);
    //};

    //vm.calculateAmount = function (item, flag) {
    //    vm.TaxEstimate = 0; vm.ExclTaxQuotation = 0; vm.ConsumptionTax = 0;
    //    vm.ExclTaxCost = 0; vm.TotalCost = 0; vm.AverageUnitPrice = 0; vm.Subtotal = 0;
    //    var count = 0;
    //    var itemIndex = 0;
    //    var totalUnitPrice = 0;
    //    _.forEach(vm.EstimationModel.TaskQuotationItems, function (f) {
    //        if (f.UnitPrice1 > 0) {
    //            f.DiscountRate = f.DiscountRate === 0 ? 10 : f.DiscountRate;
    //            totalUnitPrice += f.UnitPrice1;
    //            var totalPrice = (f.UnitPrice1 ? (f.UnitPrice1 * f.PageCount1) : 0);
    //            var totalDiscount = (f.DiscountRate ? (totalPrice * f.DiscountRate) / 100 : 0);
    //            f.ItemTotal = totalPrice - totalDiscount;
    //            vm.Subtotal += f.ItemTotal;
    //            if (flag === 'r' || typeof flag === 'undefined') {
    //                f.Discount1 = totalDiscount;
    //            }
    //            vm.TotalCost += f.ItemTotal;
    //            if (!f.ExcludeTax) {
    //                vm.TaxEstimate += f.ItemTotal;
    //            } else {
    //                vm.ExclTaxQuotation += f.ItemTotal;
    //                vm.ConsumptionTax = (vm.ExclTaxQuotation * 8) / 100;
    //                vm.TotalCost = vm.TotalCost + vm.ConsumptionTax;
    //            }
    //            count++;
    //        }
    //        itemIndex++;
    //        f.Index = itemIndex;
    //    });
    //    vm.AverageUnitPrice = totalUnitPrice / vm.EstimationModel.TaskQuotationItems.length;
    //    if (typeof vm.EstimationModel.ExcludedTaxCost != 'undefined') {
    //        vm.TotalCost = vm.TotalCost - vm.EstimationModel.ExcludedTaxCost;
    //    }
    //};

    //vm.uploadFile = function (title, document, pos) {
    //    var binding = {};
    //    binding.component = "estimationFileSelection";
    //    binding.resolve = {};
    //    binding.resolve.modalData = {};
    //    binding.resolve.modalData.Document = document;
    //    binding.resolve.modalData.docIndex = pos;
    //    binding.resolve.modalData.listento = "receiveDocumentData";
    //    binding.resolve.title = function () { return title; };
    //    $uibModal.open(binding);
    //};

    //$scope.$on("receiveDocumentData", function (event, response) {
    //    vm.EstimationModel.AttachedMaterialFileName = response.Document.FileName;
    //    vm.EstimationModel.AttachedMaterialDownloadURL = response.Document.DownloadURL;
    //    vm.EstimationModel.Document.TranslationText = response.Document.TranslationText;
    //    vm.EstimationModel.Document.WordCount = response.Document.WordCount;
    //});

    //vm.deleteEstimationAction = function (item) {
    //    ajaxService.AjaxPostWithData(BaseModel, "estimationaction/delete/" + item.ID, function (response) {
    //        toastr.success($filter('translator')('DATASAVED'));
    //        loadEstimationActionList();
    //    }, errorFunction);
    //}

    vm.saveOrder = function () {
        vm.OrderModel.RegistrationID = vm.EstimationModel.RegistrationID;
        vm.OrderModel.EstimationID = vm.EstimationModel.ID;
        var model = {
            'Estimation': vm.EstimationModel,
            'Order': vm.OrderModel,
            'OrderDetails': vm.OrderDetails,
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };
        ajaxService.AjaxPostWithData(model, "orderdetails/save", successFunction, errorFunction);
    }

    vm.displayInCalendar = function (dt) {
        calEvents = [];
        //prepareEventObject(vm.EstimationModel.EstimationItems, dt, true);
        var month = ('0' + (dt.getMonth() + 1)).slice(-2);
        var day = ('0' + dt.getDate()).slice(-2);
        var year = dt.getFullYear();
        var calDate = year + '-' + month + '-' + day;
        //var targetElm = document.querySelectorAll("[data-date='" + calDate + "']")[0];
        //var allElm = document.querySelectorAll(".fc-day");
        //allElm.forEach(function (elm) {
        //    elm.classList.remove('selected-cal-box');
        //});
        //targetElm.classList.remove('selected-cal-box');
        //targetElm.classList.add('selected-cal-box');

        //$scope.addEvent = function () {
        //    calEvents.push({
        //        title: 'Open Sesame',
        //        start: new Date(dt),
        //        allDay: true,
        //        stick: true
        //    });
        //};

        //uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', dt);
    };

    var prepareEventObject = function (items, edate, isForce) {
        for (var x = 0; x < items.length; x++) {
            var obj = {
                title: items[x].SourceLanguage + '->' + items[x].TargetLanguage,
                start: new Date(items[x].DeliveryDate),
                allDay: true,
                stick: true
            }
            if (defaultCalDate === null) {
                defaultCalDate = items[x].DeliveryDate;
            }
            calEvents.push(obj);
        }
        if (isForce) {
            uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', edate);
        } else {
            uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', defaultCalDate);
        }
    };
    
    $scope.renderCalender = function (calendar) {
        $timeout(function () {
            if (uiCalendarConfig.calendars[calendar]) {
                uiCalendarConfig.calendars[calendar].fullCalendar('render');
            }
        });
    };

    //$scope.remove = function (index) {
    //    $scope.events.splice(index, 1);
    //};
    
    $scope.eventRender = function (event, element, view) {
        element.attr({
            'tooltip': event.title,
            'tooltip-append-to-body': true
        });
        $compile(element)($scope);
    };
    
    $scope.uiConfig = {
        calendar: {
            height: 450,
            editable: false,
            header: {
                left: 'title',
                center: '',
                right: 'today prev,next'
            },
            eventClick: $scope.alertOnEventClick,
            eventDrop: $scope.alertOnDrop,
            eventResize: $scope.alertOnResize,
            eventRender: $scope.eventRender
        }
    };

    $scope.eventSources = [calEvents];
}