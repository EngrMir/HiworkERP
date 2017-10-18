angular.module("hiworkApp").component('orderdetails', {
    templateUrl: 'app/Components/OrderDetails/Template/orderDetails.html',
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
    controller: orderDetailsController
})

function orderDetailsController($scope, $compile, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, $http, $stateParams, $parse, $localStorage, uiCalendarConfig, $sce, $rootScope, $state, $templateCache) {
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
    vm.CulturalItem = {};
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
        if ($stateParams.id != "" && $stateParams.id != null && $localStorage.ShortEstimation != null && $stateParams.id === $localStorage.ShortEstimation.ID) {
            vm.EstimationModel = $localStorage.ShortEstimation;
            //if (vm.EstimationModel.InquiryDate) {
            //    vm.EstimationModel.InquiryDate = moment(vm.EstimationModel.InquiryDate).format('DD/MM/YYYY');
            //}
            //if (vm.EstimationModel.FirstDeliveryDate) {
            //    vm.EstimationModel.FirstDeliveryDate = moment(vm.EstimationModel.FirstDeliveryDate).format('DD/MM/YYYY');
            //}
            //if (vm.EstimationModel.FinalDeliveryDate) {
            //    vm.EstimationModel.FinalDeliveryDate = moment(vm.EstimationModel.FinalDeliveryDate).format('DD/MM/YYYY');
            //}
           
            vm.EstimationModel.CurrentCulture = currentCulture;
            vm.EstimationModel.IsActionBtnDisable = false;
            angular.element(document.querySelector('.elm-readonly')).find('[type="text"]').attr('readonly', true);
            loadEstimationActionList();
            //load order and order details
            if (!$localStorage.HasLocalData) {
                loadOrder();

                ajaxService.AjaxPostWithData(BaseModel, "orderdetails/estimationorderdetailslist/" + vm.EstimationModel.ID, function (r) {
                    if (r) {
                        vm.EstimationModel.EstimationItems = [].concat(r);
                        for (var x = 0; x < vm.EstimationModel.EstimationItems.length; x++) {
                            var dd = vm.EstimationModel.EstimationItems[x].DeliveryDate;
                            //if (dd) {
                            //    vm.EstimationModel.EstimationItems[x].DeliveryDate = new Date(dd);
                            //}
                        }
                        $localStorage.StoredEstimationItems = vm.EstimationModel.EstimationItems;
                        if (vm.EstimationModel.EstimationItems.length > 0) {
                            prepareEventObject(r, false);
                        }
                    }
                    for (var x = 0; x < vm.EstimationModel.EstimationItems.length; x++) {
                        vm.EstimationModel.EstimationItems[x].DeliveryDate = new Date(vm.EstimationModel.EstimationItems[x].DeliveryDate);
                    }
                }, errorFunction);
            } else {
                $localStorage.HasLocalData = false;
                vm.EstimationModel.EstimationItems = $localStorage.StoredEstimationItems;
                vm.OrderDetails = $localStorage.StoredOrderDetails;
                vm.OrderModel = $localStorage.StoredOrderModel;
                if (vm.EstimationModel.EstimationItems.length > 0) {
                    setTimeout(function () {
                        prepareEventObject(vm.EstimationModel.EstimationItems, true);
                    }, 200);
                }
                $rootScope.$emit("listenToManipulateCommSelectionItem", {});
                for (var x = 0; x < vm.EstimationModel.EstimationItems.length; x++) {
                    vm.EstimationModel.EstimationItems[x].DeliveryDate = new Date(vm.EstimationModel.EstimationItems[x].DeliveryDate);
                }                
            }
            calculateOrder();
            calculateHands();
            ajaxService.AjaxPostWithData(BaseModel, "workingstatus/list", function (r) {
                if (r) {
                    vm.workstatusList = r;
                }
            }, errorFunction);
            //loadEstimationActionList();
        } else {
            toastr.error("Estimation could not found");
        }
    };
    var mapCulturalItems = function () {
        for (var x = 0; x < vm.culturalProperties.length; x++) {
            var the_variable = 'vm.CulturalItem.' + vm.culturalProperties[x];
            var val = vm.OrderModel[vm.culturalProperties[x]];
            var model = $parse(the_variable);
            model.assign($scope, val);
        }
    }
    var calculateOrder = function () {
        vm.OrderModel.QuoatedPrice = vm.EstimationModel.QuotationInclTax;
        vm.OrderModel.ConsumptionTax = vm.EstimationModel.ConsumptionTax;
        vm.OrderModel.CostExclTax = vm.EstimationModel.ExcludedTaxCost;
        vm.OrderModel.BillingAmount = vm.OrderModel.QuoatedPrice + vm.OrderModel.ConsumptionTax + vm.OrderModel.CostExclTax;
        vm.OrderModel.OriginalCost = 0;
        vm.OrderModel.WithdrawlAmount = vm.OrderModel.StaffPaymentCost - vm.OrderModel.StaffBillingCost;
        vm.OrderModel.Profit = vm.OrderModel.QuoatedPrice - vm.OrderModel.OriginalCost;
        vm.OrderModel.GrossInterestRate = (vm.OrderModel.Profit / vm.OrderModel.OriginalCost) * 100;
    }

    var calculateHands = function () {
        ajaxService.AjaxPostWithData(BaseModel, "order/profitsharesetting/", function (r) {
            if (r.ID > 0) {                
                vm.OrderModel.SCharge = (r.SCharge / 100) * vm.OrderModel.Profit;
                vm.OrderModel.BCharge = (r.BCharge / 100) * vm.OrderModel.Profit;
                vm.OrderModel.CCharge = (r.CCharge / 100) * vm.OrderModel.Profit;
                vm.OrderModel.FrontSalesCharge = (r.FrontSalesCharge / 100) * vm.OrderModel.Profit;
                vm.OrderModel.ActionSales = (r.PersonCharge2 / 100) * vm.OrderModel.Profit;
                vm.OrderModel.PartnerCharge = (r.PartnerCharge / 100) * vm.OrderModel.Profit;
                vm.OrderModel.PersonCharge1 = (r.PersonCharge1 / 100) * vm.OrderModel.Profit;
                vm.OrderModel.PersonCharge2 = (r.PersonCharge2 / 100) * vm.OrderModel.Profit;
                vm.OrderModel.IntResponse = (r.PersonCharge2 / 100) * vm.OrderModel.Profit;
                vm.OrderModel.ExtResponse = (r.PersonCharge2 / 100) * vm.OrderModel.Profit;                
                //vm.OrderModel.NetProfit = (r.FrontSalesCharge / 100) * vm.OrderModel.Profit;
                //vm.OrderModel.GrossProfit = (r.FrontSalesCharge / 100) * vm.OrderModel.Profit;
                //vm.OrderModel.NetMarginRate = (r.FrontSalesCharge / 100) * vm.OrderModel.Profit;
                //vm.OrderModel.GrossMarginRate = (r.FrontSalesCharge / 100) * vm.OrderModel.Profit;
            } else {
            } 
        }, errorFunction);
    }

    var loadOrder = function () {
        ajaxService.AjaxPostWithData(BaseModel, "order/orderitem/" + vm.EstimationModel.ID, function (r) {
           // alert(r.ID);
            if (r.RegistrationID > 0) {
                vm.OrderModel = r;

                //vm.OrderModel.ConsumptionTax = vm.EstimationModel.ConsumptionTax;//no modify
                //vm.OrderModel.CostExclTax = vm.EstimationModel.ExcludedTaxCost;
                //vm.OrderModel.QuoatedPrice = vm.EstimationModel.GrandTotal;//no modify
                //vm.OrderModel.ConsumptionTax = vm.EstimationModel.ConsumptionTax;//no modify
                //vm.OrderModel.BillingAmount = vm.OrderModel.QuoatedPrice + vm.OrderModel.ConsumptionTax + vm.OrderModel.CostExclTax;//no modify
                //vm.OrderModel.WithdrawlAmount = vm.OrderModel.BillingAmount;//no modify
                //vm.OrderModel.Profit = vm.OrderModel.;//no modify
                //vm.OrderModel.GrossInterestRate = vm.OrderModel.;//no modify
               // vm.OrderModel.BillingDate = vm.EstimationModel.FinalDeliveryDate;

                //if (vm.OrderModel.ComplainDate) {
                //    vm.OrderModel.ComplainDate = new Date(vm.OrderModel.ComplainDate);
                //}
                //if (vm.OrderModel.ResponseComplainDate) {
                //    vm.OrderModel.ResponseComplainDate = new Date(vm.OrderModel.ResponseComplainDate);
                //}
                $localStorage.StoredOrderModel = vm.OrderModel;
                loadOrderDetails();
                calculateHands();
                mapCulturalItems();
            }
        }, errorFunction);        
    }

    var loadOrderDetails = function () {
        if (vm.OrderModel.ID) {
            ajaxService.AjaxPostWithData(BaseModel, "orderdetails/list/" + vm.OrderModel.ID, function (r) {
                if (r) {
                    vm.OrderDetails = [].concat(r);
                    //for (var x = 0; x < vm.OrderDetails.length; x++) {
                    //    if (vm.OrderDetails[x].DeliveryDate) {
                    //        vm.OrderDetails[x].DeliveryDate = new Date(vm.OrderDetails[x].DeliveryDate);
                    //        vm.OrderDetails[x].DeliveryDate = moment(vm.OrderDetails[x].DeliveryDate).format('DD/MM/YYYY');
                    //    }
                    //}
                    $localStorage.StoredOrderDetails = vm.OrderDetails;
                    $rootScope.$emit("listenToManipulateCommSelectionItem", {});
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
        vm.OrderDetails = vm.OrderDetails.filter(function (item) {
            return item.EstimationDetailsID !== response[0].Item.EstimationDetailID;
        });
        var obj = {
            OrderID: vm.OrderModel.ID,
            EstimationDetailsID: response[0].Item.EstimationDetailsID,
            EstimationID: vm.EstimationModel.ID,
            StaffID: response[0].ID,
            WorkingStatus: response[0].Item.WorkingStatus,
            DeliveryDate: response[0].Item.DeliveryDate,
            PaymentAmount: 0,
            AgencyCommission: 0,
            ComplainDetails: 'test',
            RemitanceDate: new Date(),
            DepositAmount: 0,

            StaffNo: '12121',
            StaffName: response[0].FirstName + ' ' + response[0].MiddleName + ' ' + response[0].LastName,
            EmailAddress: response[0].StaffEmailID,
            PageCount1: response[0].Item.PageCount1,
            EvaluationAmount: response[0].Item.UnitPrice1,
            Penalty: 'test',
            Residence: 'test'
        };
        vm.OrderDetails.push(obj);
        $localStorage.StoredOrderDetails = vm.OrderDetails;
    });
    vm.goBack = function (id) {
        vm.EstimationModel = $localStorage.ShortEstimation;
        $state.go('ShortTermEstimation', {
            id: id
        });
    };
    vm.prepareQuotationPdf = function () {
        $http.post(appSettings.API_BASE_URL + 'taskquotation/generatepdf', vm.EstimationModel, { responseType: 'arraybuffer' })
          .then(function (data) {
              console.log(data);
              var file = new Blob([data.data], { type: 'application/pdf' });
              //fileURL = URL.createObjectURL(file);
              //window.open(fileURL);
              saveAs(file, 'Quotation.pdf');
          });
    };

    vm.saveEstimationAction = function (flag) {
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
                vm.EstimationAction = null;
                vm.CulturalItem.ActionDetails = '';
            }, errorFunction);
        }
    };

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };

    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

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

    vm.deleteEstimationAction = function (item) {
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/delete/" + item.ID, function (response) {
            toastr.success($filter('translator')('DATASAVED'));
            loadEstimationActionList();
        }, errorFunction);
    }

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
            'Culture': currentCulture,
            'EstimationDetails': vm.EstimationModel.EstimationItems
        };
        ajaxService.AjaxPostWithData(model, "orderdetails/save", successFunction, errorFunction);
    }

    vm.displayInCalendar = function (dt) {
        $localStorage.HasLocalData = true;
        $localStorage.SelectedDate = dt;

        $state.transitionTo($state.current, $stateParams, {
            reload: true,
            inherit: false,
            notify: true
        });
    };

    var prepareEventObject = function (items, isForce) {
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
        $localStorage.CalEvents = calEvents;

        if (isForce) {
            var sDate = new Date($localStorage.SelectedDate);
            $localStorage.SelectedDate = null;
            uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', sDate);
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