angular.module("hiworkApp").component('narrationEstimation', {
    templateUrl: "App/Components/NarrationEstimation/Template/NarrationEstimation.html",
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
    controller: narrationEstimationController
});


function narrationEstimationController($scope, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, EstimationType, EstimationStatus, StaffRegistrationService, $stateParams, alerting, $localStorage, $parse, $state) {

    var vm = this;

    var gridData = [];

    vm.SourceLanguageID = "";
    vm.TargetLanguageID = "";
    vm.EstimationAction = "";
    vm.culturalProperties = ["ClientAddress", "BillingCompanyName", "BillingAddress", "DeliveryCompanyName", "DeliveryAddress", "CoordinatorNotes", "Remarks", "OtherItemName"];



    var estimationDetailData = [{
        SourceLanguageID: '',
        TargetLanguageID: '',
        SourceLanguage: '',
        TargetLanguage: '',
        UnitPrice1: 0, UnitPriceTime: 0, UnitPriceSubTotal: 0,
        DiscountRate: 0, DiscountedPrice: 0, EstimationPrice: 0,
        StudioPrice: 0, StudioPriceTime: 0, EditPrice: 0,
        EditPriceTime: 0, StudioPriceSubTotal: 0, StudioPriceDiscountRate: 0,
        StudioDiscountedPrice: 0, TotalAfterDiscount: 0, NumberOfPeople: 0,
        Total: 0, PaymentRate: 0, ExpectedPayment: 0

    }]


    vm.getTableHeight = function () {
        var rowHeight = 30; // your row height
        var headerHeight = 30; // your header height
        return {
            height: (vm.gridOptions.data.length * rowHeight + headerHeight) + "px"
        };
    };

    vm.addNewRow = function () {
        vm.gridOptions.data.unshift({
            UnitPrice1: 0, UnitPriceTime: 0, UnitPriceSubTotal: 0,
            DiscountRate: 0, DiscountedPrice: 0, EstimationPrice: 0,
            StudioPrice: 0, StudioPriceTime: 0, EditPrice: 0,
            EditPriceTime: 0, StudioPriceSubTotal: 0, StudioPriceDiscountRate: 0,
            StudioDiscountedPrice: 0, TotalAfterDiscount: 0, NumberOfPeople: 0,
            Total: 0, PaymentRate: 0, ExpectedPayment: 0
        });
    }

    vm.calculateTotal = function () {
        var totalPrice = 0;

        vm.Expense.TransportationTotal = vm.Expense.TransportationUnitprice * vm.Expense.TransportationNumberOfPersons * vm.Expense.TransportationNumberOfDays;
        vm.Expense.AccomodationTotal = vm.Expense.AccomodationUnitprice * vm.Expense.AccomodationNumberOfPersons * vm.Expense.AccomodationNumberOfDays;
        vm.Expense.MealTotal = vm.Expense.MealUnitprice * vm.Expense.MealNumberOfPersons * vm.Expense.MealNumberOfDays;
        vm.Expense.AllowanceTotal = vm.Expense.AllowanceUnitprice * vm.Expense.AllowanceNumberOfPersons * vm.Expense.AllowanceNumberOfDays;
        vm.Expense.ExpensePriceInTax = vm.Expense.AccomodationTotal + vm.Expense.MealTotal + vm.Expense.AllowanceTotal;

        angular.forEach(vm.gridOptions.data, function (value, key) {
            console.log(key + ': ' + value);

            totalPrice = totalPrice + vm.gridOptions.data[key].Total;
        });

        vm.EstimationModel.TotalPriceExTax = totalPrice;
    }


    vm.calculation = function (grid, row) {

        console.log('grid' + grid.data + 'row' + row.entity);
        var totalPrice = 0;

        row.entity.UnitPriceSubTotal = row.entity.UnitPrice1 * row.entity.UnitPriceTime;
        row.entity.DiscountedPrice = row.entity.UnitPriceSubTotal * row.entity.DiscountRate / 100;
        row.entity.EstimationPrice = row.entity.UnitPriceSubTotal - row.entity.DiscountedPrice;
        row.entity.StudioPriceSubTotal = row.entity.StudioPrice * row.entity.StudioPriceTime + row.entity.EditPrice * row.entity.EditPriceTime;
        row.entity.StudioDiscountedPrice = row.entity.StudioPriceSubTotal * row.entity.DiscountRate / 100;
        row.entity.TotalAfterDiscount = row.entity.StudioPriceSubTotal - row.entity.StudioDiscountedPrice;
        row.entity.Total = row.entity.EstimationPrice * row.entity.NumberOfPeople + row.entity.TotalAfterDiscount;
        row.entity.ExpectedPayment = row.entity.Total;

        //vm.TotalPriceExTax = ro


        angular.forEach(grid.options.data, function (value, key) {
            console.log(key + ': ' + value);

            totalPrice = totalPrice + grid.options.data[key].Total;
        });

        vm.EstimationModel.TotalPriceExTax = totalPrice;
        vm.EstimationModel.ConsumptionTax = vm.EstimationModel.TotalPriceExTax * 0.08;
        vm.EstimationModel.TotalWithoutTax = vm.EstimationModel.TotalPriceExTax + vm.EstimationModel.ConsumptionTax;
        vm.EstimationModel.TotalWithTax = vm.EstimationModel.ConsumptionTax + vm.EstimationModel.TotalWithoutTax;

        vm.EstimationModel.GrossProfit = vm.EstimationModel.TotalWithTax - vm.EstimationModel.ConsumptionTax - vm.Expense.TransportationTotal - vm.Expense.AccomodationTotal - vm.Expense.MealTotal - vm.Expense.AllowanceTotal;

    };

    vm.loadOrderDetails = function () {
        $state.go("NarrationOrderDetails");
    }

    vm.gridOptions = {
        data: estimationDetailData,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        multiSelect: true,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        rowHeight: '30px',
        //rowBorderTop: '1px solid lightgrey',      
        enableSelectAll: true,
        //paginationPageSizes: [5, 3, 2],
        //paginationPageSize: 5,

        columnDefs: [
            {
                field: 'SourceLanguage',
                displayName: 'Source Language',
                width: '7%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                editDropdownValueLabel: 'Name',
                cellTemplate: '<ui-select ng-model="row.entity.SourceLanguage" theme="bootstrap" append-to-body="true"><ui-select-match placeholder="Select Language">{{$select.selected.Name }}</ui-select-match> <ui-select-choices repeat="language in col.colDef.editDropdownOptionsArray | filter: $select.search"> <span ng-bind-html="language.Name | highlight: $select.search"> </span></ui-select-choices></ui-select>',

            },
            {
                field: 'TargetLanguage',
                displayName: 'Destination Language',
                width: '7%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<ui-select ng-model="row.entity.TargetLanguage" theme="bootstrap" append-to-body="true"><ui-select-match placeholder="Select Language">{{$select.selected.Name}}</ui-select-match><ui-select-choices repeat="language in col.colDef.editDropdownOptionsArray  | filter: $select.search"> <span ng-bind-html="language.Name | highlight: $select.search"></span> </ui-select-choices></ui-select>',
            },
            {
                field: 'UnitPrice1',
                displayName: 'Unit price/hour',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.UnitPrice" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; " value="0" only-decimal > </div>'
            },
            {
                field: 'UnitPriceTime',
                displayName: 'Time(hour)',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.UnitPriceTime"  format="2" ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'UnitPriceSubTotal',
                displayName: 'SubTotal',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.UnitPriceSubTotal" format="2"   style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'
            },
            {
                field: 'DiscountRate',
                displayName: 'Discount Rate(%)',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.DiscountRate" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'DiscountedPrice',
                displayName: 'Discounted price',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.DiscountedPrice"  format="2"  style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'
            },
            {
                field: 'EstimationPrice',
                displayName: 'Estimation Price',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.EstimationPrice" format="2"   style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'

            },
            {
                field: 'StudioPrice',
                displayName: 'Studio price/hour',
                width: '5%', headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.StudioPrice" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'StudioPriceTime',
                displayName: 'Time', width: '4%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.StudioPriceTime" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'EditPrice',
                displayName: 'Edit price/hour',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.EditPrice" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'EditPriceTime',
                displayName: 'Time',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.EditPriceTime" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'StudioPriceSubTotal',
                displayName: 'Sub total',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.StudioPriceSubTotal"  format="2"  style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'

            },
            {
                field: 'StudioPriceDiscountRate',
                displayName: 'Discount Rate',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.StudioPriceDiscountRate" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal > </div>'
            },
            {
                field: 'StudioDiscountedPrice',
                displayName: 'Discounted price',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.StudioDiscountedPrice" format="2"  style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'
            },
            {
                field: 'TotalAfterDiscount',
                displayName: 'Total After Discount',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.TotalAfterDiscount" format="2"  style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'
            },
            {
                field: 'NumberOfPeople',
                displayName: 'Number of persons',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.NumberOfPeople" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal style="text-align: right;"> </div>'

            },
            {
                field: 'Total',
                displayName: 'Total',
                width: '4%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.Total" format="2"  style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'

            },
            {
                field: 'PaymentRate',
                displayName: 'Payment Rate',
                width: '4.5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.PaymentRate" format="2"  ng-change="grid.appScope.vm.calculation(grid,row)" style="border:none !important; text-align: right !important; "  value="0" only-decimal style="text-align: right;"> </div>'

            },
            {
                field: 'ExpectedPayment',
                displayName: 'Expected Payment',
                width: '5%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell',
                cellTemplate: '<div><input type="text" ng-model="row.entity.ExpectedPayment" format="2"  style="background-color: white !important; border:none !important; text-align: right !important; "  disabled > </div>'
            }
        ]
    };

    //* Mr. Roy code *//


    vm.rowCollection = [];
    vm.EstimationModel = {};
    vm.RemarksState = new Array(9);
    vm.Expense = {};

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
    vm.EstimationModel.CurrentCulture = currentCulture;
    vm.EstimationModel.ApplicationId = appSettings.ApplicationId;
    vm.EstimationModel.EstimationItems = [];
    vm.Currency = {};


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

        vm.gridOptions.columnDefs[0].editDropdownOptionsArray = vm.languages;
        vm.gridOptions.columnDefs[1].editDropdownOptionsArray = vm.languages;

        //load page in edit mode

        if ($stateParams.id != "" && $stateParams.id != null) {
            var i, j;
            manageState();
            mapCulturalItems();
            //Date formation

            vm.EstimationModel.CurrentCulture = currentCulture;
            //
            vm.EstimationAction.IsActionBtnDisable = false;
            ajaxService.AjaxPostWithData(vm.EstimationModel, "narrationestimationdetails/list/" + vm.EstimationModel.ID, onGetEstimationDetails, errorFunction);

            loadWorkContents();
            loadEstimationActionList();
            loadNarrationExpense();

        } else {
            vm.EstimationAction.IsActionBtnDisable = true;
        }

        //Load page in edit mode       

    };

    var loadNarrationExpense = function () {
        ajaxService.AjaxPostWithData(vm.EstimationModel, "narrationestimationexpenses/list/" + vm.EstimationModel.ID, function (response) {
            if (response) {
                vm.Expense = response;
            }
        }, function (error) {
            console.log(error);
        });
    }

    var loadWorkContents = function () {

        ajaxService.AjaxPostWithData(vm.EstimationModel, "narration/estimationworkcontentlist/" + vm.EstimationModel.ID, function (res) {
            if (res) {
                for (var x = 0; x < res.length; x++) {
                    var wc = res[x].WorkContent;
                    //var index = vm.workContent.findIndex(x=>x === wc);             
                    var index = FindIndex(res[x], res);
                    var the_variable = 'vm.NarrationEstimation.Contents' + index;
                    var model = $parse(the_variable);
                    model.assign($scope, 1);
                }
            }
        }, errorFunction);
    }

    function FindIndex(key, array) {
        var index = -1;

        array.some(function (item, i) {
            var result = Object.keys(key).some(function (oKey) {
                return (oKey in item && item[oKey] === key[oKey]);
            });
            if (result) index = i;
            return result;
        });

        return index;
    }

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.EstimationModel = $stateParams.Estimation;
            }

            vm.EstimationModel = $localStorage.EstimationModel;
        }
    }

    var mapCulturalItems = function () {
        for (var x = 0; x < vm.culturalProperties.length; x++) {
            var the_variable = 'vm.CulturalItem.' + vm.culturalProperties[x];
            var val = vm.EstimationModel[vm.culturalProperties[x]];
            var model = $parse(the_variable);
            model.assign($scope, val);
        }
    }

    function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    var onGetEstimationDetails = function (response) {
        if (response) {
            angular.forEach(estimationDetailData, function (value, key) {
                estimationDetailData.splice(0, 1);
            });

            //var test = vm.languages.filter(function (obj) { return obj.ID === response[i].SourceLanguageID; });

            for (var i = 0; i < response.length; i++) {
                var sourceIndex = findWithAttr(vm.languages, 'ID', response[i].SourceLanguageID);
                var targetIndex = findWithAttr(vm.languages, 'ID', response[i].TargetLanguageID);

                estimationDetailData = {
                    ID: response[i].ID,
                    SourceLanguage: vm.languages[sourceIndex],
                    TargetLanguage: vm.languages[targetIndex],
                    UnitPrice1: response[i].UnitPrice1,
                    UnitPriceTime: response[i].UnitPriceTime,
                    UnitPriceSubTotal: response[i].UnitPriceSubTotal,
                    DiscountRate: response[i].DiscountRate,
                    DiscountedPrice: response[i].DiscountedPrice,
                    EstimationPrice: response[i].EstimationPrice,
                    StudioPrice: response[i].StudioPrice,
                    StudioPriceTime: response[i].StudioPriceTime,
                    EditPrice: response[i].EditPrice,
                    EditPriceTime: response[i].EditPriceTime,
                    StudioPriceSubTotal: response[i].StudioPriceSubTotal,
                    StudioPriceDiscountRate: response[i].StudioPriceDiscountRate,
                    StudioDiscountedPrice: response[i].StudioDiscountedPrice,
                    TotalAfterDiscount: response[i].TotalAfterDiscount,
                    NumberOfPeople: response[i].NumberOfPeople,
                    Total: response[i].Total,
                    PaymentRate: response[i].PaymentRate,
                    ExpectedPayment: response[i].ExpectedPayment
                };

                vm.gridOptions.data.push(estimationDetailData);
            }
        }
    };

    function loadGridData(language) {
        vm.gridData.UnitPrice = 1;
        vm.gridData.UnitPriceTime = 1;
        vm.gridOptions.data = vm.gridData;
    }

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

        //vm.DTPEstimation.RemarksCoordinatorType = RemarksString.trim();
        //vm.DTPEstimation.TaxEstimation = vm.TaxEstimate;
        //vm.DTPEstimation.GrandTotal = vm.TotalCost;
        //vm.DTPEstimation.QuotationExclTax = vm.ExclTaxQuotation;
        //vm.DTPEstimation.ConsumptionOnTax = vm.ConsumptionTax;

        var itemContents = document.getElementsByClassName('chk-content');
        var itemQuality = document.getElementsByClassName('chk-quality');
        var itemAbility = document.getElementsByClassName('chk-ability');

        var contentItems = [];
        var competencyItem = [];
        var emptyObj = {};

        for (var x = 0; x < itemContents.length; x++) {
            if (itemContents[x].checked) {
                var obj = {
                    WorkContent: itemContents[x].value
                };
                contentItems.push(obj);
            }
        }

        for (var x = 0; x < itemQuality.length; x++) {
            if (itemQuality[x].checked) {
                var obj = {
                    CompetencyType: "Expected Quality",
                    CompetencyDetail: itemQuality[x].value
                };
                competencyItem.push(obj);
            }
        }

        for (var x = 0; x < itemAbility.length; x++) {
            if (itemAbility[x].checked) {
                var obj = {
                    CompetencyType: "Japanese Quality",
                    CompetencyDetail: itemAbility[x].value
                };
                competencyItem.push(obj);
            }
        }


        if (competencyItem.length > 0) vm.NarrationEstimation.CompetencyDetail = [].concat(competencyItem);
        else vm.NarrationEstimation.CompetencyDetail = null;

        if (contentItems.length > 0) vm.NarrationEstimation.Contents.WorkContents = [].concat(contentItems);
        else vm.NarrationEstimation.Contents.WorkContents = null;

        angular.forEach(vm.gridOptions.data, function (value, key) {

            vm.gridOptions.data[key].SourceLanguageID = vm.gridOptions.data[key].SourceLanguage.ID;
            vm.gridOptions.data[key].TargetLanguageID = vm.gridOptions.data[key].TargetLanguage.ID;
        });

        var model = {
            'Estimation': vm.EstimationModel,
            'EstimationNarrationExpense': vm.Expense,
            'EstimationDetails': vm.gridOptions.data,
            'WorkContents': vm.NarrationEstimation.Contents.WorkContents,
            'estimationCompetencies': vm.NarrationEstimation.CompetencyDetail,
            'CulturalItem': vm.CulturalItem,
            'CurrentUserID': currentUser.CurrentUserID,
            'ApplicationID': appSettings.ApplicationId,
            'Culture': currentCulture
        };

        ajaxService.AjaxPostWithData(model, "narrationEstimation/save", function (response) {
            console.log(response);
            alerting.addSuccess($filter('translator')('DATASAVED'));
        }, function (error) {
            console.log(error);
            alerting.addDanger(error);
        });

    };


    vm.saveEstimationAction = function (flag) {
        if (!flag) {
            vm.EstimationAction.CurrentUserID = currentUser.CurrentUserID;
            vm.EstimationAction.CurrentCulture = currentCulture;
            vm.EstimationAction.ApplicationId = appSettings.ApplicationId;
            vm.EstimationAction.EstimationID = vm.NarrationEstimationModel.ID;
            ajaxService.AjaxPostWithData(vm.EstimationAction, "estimationaction/save", function (response) {
                toastr.success($filter('translator')('DATASAVED'));
                loadEstimationActionList();
                vm.EstimationAction.NextActionDate = '';
                vm.EstimationAction.ActionDetails = '';
                vm.EstimationAction.ActionType = '';
            }, errorFunction);
        }
    };

    var loadEstimationActionList = function () {
        ajaxService.AjaxPostWithData(vm.EstimationModel, "estimationaction/list/" + vm.EstimationAction.ID, function (res) {
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
                vm.EstimationAction.ActionItems = [].concat(actionItems);
            }
        }, errorFunction);
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



    //* Mr. roy code*//
}