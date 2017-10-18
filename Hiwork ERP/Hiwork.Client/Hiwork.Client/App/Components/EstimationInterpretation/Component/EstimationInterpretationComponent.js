angular.module("hiworkApp").component('estiinterpretationquotation', {
    templateUrl: 'app/Components/EstimationInterpretation/Template/EstimationInterpretation.html',
    controllerAs: "vm",
    bindings: {
        routes: "=",
        services: "=",
        languages: "=",
        businessCategories: "=",
        specializedFields: "=",
        subSpecializedFields: "=",
        currencyList: "=",
        manipulateUserAccess: "="
    },
    controller: interpretationEstimationController
})

function interpretationEstimationController($scope, $uibModal, $filter, estimationService, AppStorage, appSettings, sessionFactory, ajaxService, EstimationType, EstimationStatus, $stateParams, $localStorage, $state, EstimationDefaultStatus) {

    var vm = this;
    vm.rowCollection = [];
    vm.EstimationModel = {};
    vm.StaffAllowanceModel = {};
    vm.StaffAllowanceModel.Translation = {};
    vm.StaffAllowanceModel.Accommodation = {};
    vm.StaffAllowanceModel.Meal = {};
    vm.StaffAllowanceModel.Allowance = {};
    vm.StaffAllowanceModel.CollectionFee = [];
    vm.Translation = {};
    vm.RemarksState = new Array(9);
    vm.EstimationModel.ActionItems = [];
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
    vm.ButtonTitles = ['Registration / Update', 'Order Details', 'Temporary Registration', 'Project initiation', 'Approval Request', 'Approval', 'Quotation Email', 'Quotation Request', 'Confirmation Email', 'Delete'];
    var NewDocument = {
        EstimateID: "",
        EstimateDetailsID: "",
        FileName: "",
        WordCount: 0,
        WordCountStr: "0",
        TranslationText: "",
        DownloadURL: ""
    };
    var DummyEstimationDetailsModel = {
        GeneralUnitPrice: "", Contents: "", BasicAmount: 0, BasicTime: 0, AdditionalTime: 0, TotalDayHour: "", LateAtNightAmount: 0, SubTotal: 0, StartDate:"", CompletionDate:"",
        AdditionalBasicAmount: 0, ExtensionAmount: 0, ExtensionTime: 0, LateNightTime: 0, TransferTime: 0, TransferAmount: 0, NumberOfDays: 0, DiscountRate: 0, StartTime:"", FinishTime:"", NewStartTime:"", NewFinishTime:"",
        SourceLanguageID: "", TargetLanguageID: "", ServiceType: "", ExcludeTax: false, NumberOfPeople: 0, IsMarkedForDelete: false, TotalAfterDiscount: 0, Total: 0, PaymentRate: 0,
        PaymentRate:0,ExpectedPayment:0,UnitPriceSubTotal:0, DiscountedPrice:0,IsOverseas:"",
        Document: _.clone(NewDocument)
    };

    vm.EstimationAccessModel = {
        UserID: sessionFactory.GetObject(AppStorage.userData).CurrentUserID,
        EstimationTypeID: 1,
        EstimationStatusID: 0
    }

    vm.$onInit = function () {
        estimationService.manipulateUserAccess(vm.EstimationAccessModel, vm.EstimationModel.CreatedBy);
        EstimationOrder = null;
        vm.GetAllServiceType();
        vm.GetAllUnitPrice();
        vm.GetAllCountryList();
        vm.GetAllUnit();
        vm.EstimationModel.TotalNoOfDay = null;
        vm.EstimationModel.TotalTime = null;

        vm.EstimationModel.TotalTime = null;

        vm.EstimationModel.TotalDayHour = null;
        vm.EstimationModel.TotalTimePeriod = null;
     
        vm.EstimationModel.StartDate = null;
        vm.EstimationModel.FinishDate = null;
        vm.EstimationModel.FileName = "";
        vm.EstimationModel.DownloadURL = "";
        vm.StartTime = null;
        vm.FinishTime = null;
        vm.StaffAllowanceModel.Translation.ItemName = null;
        vm.StaffAllowanceModel.Translation.UnitPrice =0;
        vm.StaffAllowanceModel.Translation.NoOfPersons = 0;
        vm.StaffAllowanceModel.Translation.NoOfDays = 0;
        vm.StaffAllowanceModel.Translation.SubTotal = 0;
        vm.StaffAllowanceModel.Accommodation.ItemName =null;
        vm.StaffAllowanceModel.Accommodation.UnitPrice = 0;
        vm.StaffAllowanceModel.Accommodation.NoOfPersons = 0;
        vm.StaffAllowanceModel.Accommodation.NoOfDays = 0;
        vm.StaffAllowanceModel.Accommodation.SubTotal = 0;
        vm.StaffAllowanceModel.Meal.ItemName = null;
        vm.StaffAllowanceModel.Meal.UnitPrice = 0;
        vm.StaffAllowanceModel.Meal.NoOfPersons = 0;
        vm.StaffAllowanceModel.Meal.NoOfDays = 0;
        vm.StaffAllowanceModel.Meal.SubTotal = 0;
        vm.StaffAllowanceModel.Allowance.ItemName = null;
        vm.StaffAllowanceModel.Allowance.UnitPrice = 0;
        vm.StaffAllowanceModel.Allowance.NoOfPersons = 0;
        vm.StaffAllowanceModel.Allowance.NoOfDays = 0;
        vm.StaffAllowanceModel.Allowance.SubTotal = 0;
        vm.StaffAllowanceModel.Total = null;

        for (i = 0; i < 3; i++) {
            var item = angular.copy(DummyEstimationDetailsModel);
            vm.rowCollection.push(item);
        }
        for (i = 0; i < vm.RemarksState.length; i++) {
            vm.RemarksState[i] = false;
        }
        // vm.EstimationModel.EstimationItems = [].concat(vm.rowCollection);
        vm.EstimationModel.EstimationItems.push(angular.copy(DummyEstimationDetailsModel));


        for (i = 0; i < vm.currencyList.length; i++) {
            if (vm.currencyList[i].Code == "USD") {
                vm.Currency = vm.currencyList[i];
                vm.changeCurrency();
                break;
            }
        }

    
        if ($stateParams.id != "" && $stateParams.id != null) {
            var i, j;
            manageState();
            vm.EstimationModel.InquiryDate = new Date(vm.EstimationModel.InquiryDate);
            vm.EstimationModel.FirstDeliveryDate = new Date(vm.EstimationModel.FirstDeliveryDate);
            vm.EstimationModel.FinalDeliveryDate = new Date(vm.EstimationModel.FinalDeliveryDate);
            vm.EstimationModel.StartDate = new Date(vm.EstimationModel.StartDate);
            vm.EstimationModel.EndDate = new Date(vm.EstimationModel.EndDate);
            j = 0;
            for (i = 0; i < vm.currencyList.length; i++) {
                if (vm.currencyList[i].Id == vm.EstimationModel.CurrencyID) {
                    vm.Currency = vm.currencyList[i];
                    vm.changeCurrency();
                    break;
                }
            }
           
            loadEstimationActionList();
            vm.LoadStaffAllowance();
            var BaseModel = new Object();
            BaseModel.CurrentUserID = currentUser.CurrentUserID;
            BaseModel.CurrentCulture = currentCulture;
            BaseModel.ApplicationId = appSettings.ApplicationId;
            ajaxService.AjaxPostWithData(BaseModel, "estimationinterpretation/detailslist/" + $stateParams.id, onGetEstimationDetails, errorFunction);

            vm.EstimationAccessModel.EstimationStatusID = vm.EstimationModel.EstimationStatus ? vm.EstimationModel.EstimationStatus : 0;
            estimationService.manipulateUserAccess(vm.EstimationAccessModel, vm.EstimationModel.CreatedBy);
        }

    };


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
        Item.BasicAmount = 0;
        var i;
        var UnitPrice;
        for (i = 0; i < vm.UnitPriceList.length; i += 1) {
            UnitPrice = vm.UnitPriceList[i];
            if (UnitPrice.SourceLanguageID == Item.SourceLanguageID
                && UnitPrice.TargetLanguageID == Item.TargetLanguageID
                && UnitPrice.EstimationTypeID == EstimationType.Interpreter
                && UnitPrice.UnitID == vm.EstimationModel.UnitID) {
                Item.BasicAmount = UnitPrice.GeneralUnitPrice;
                break;
            }
        }
        vm.PriceCalculation(Item);
    };

    vm.UpdateUnitPriceByUnit=function()
    {
        var i = 0;
        for(i=0; i< vm.EstimationModel.EstimationItems.length; i++)
        {
            vm.EstimationModel.EstimationItems[i].BasicAmount = 0;
            for (j = 0; j < vm.UnitPriceList.length; j += 1) {
                UnitPrice = vm.UnitPriceList[j];
                if (UnitPrice.SourceLanguageID == vm.EstimationModel.EstimationItems[i].SourceLanguageID
                    && UnitPrice.TargetLanguageID == vm.EstimationModel.EstimationItems[i].TargetLanguageID
                    && UnitPrice.EstimationTypeID == EstimationType.Interpreter
                    && UnitPrice.UnitID == vm.EstimationModel.UnitID) {
                    vm.EstimationModel.EstimationItems[i].BasicAmount = UnitPrice.GeneralUnitPrice;
                    break;
                }
            }
            vm.PriceCalculation(vm.EstimationModel.EstimationItems[i]);
        }
    }

    vm.UpdateCostByBasicPrice=function(item)
    {
        vm.PriceCalculation(item);
    }

    vm.changeCurrency = function () {
        vm.EstimationModel.CurrencyID = vm.Currency.Id;
        vm.EstimationModel.CurrencySymbol = vm.Currency.Symbol;
    };

    vm.orderDetails=function()
    {
        $state.go('InterpretationOrderDetails', { 'id': vm.EstimationModel.ID, 'Estimation': vm.EstimationModel });
    }

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
        vm.EstimationModel.ClientID = response[0].ID;
        vm.EstimationModel.ClientName = response[0].Name;
    });
    $scope.$on("selectedTrade", function (event, response) {
        vm.EstimationModel.TradingID = response[0].ID;
        vm.EstimationModel.TradingName = response[0].Name;
    });
    $scope.$on("selectedTeam", function (event, response) {
        vm.EstimationModel.TeamID = response[0].ID;
        vm.EstimationModel.AffiliateTeamName = response[0].Name;
    });


    vm.addItem = function () {
        var item = angular.copy(DummyEstimationDetailsModel);
        vm.EstimationModel.EstimationItems.push(item);
    };

    vm.registerQuotaion = function () {
      
        vm.EstimationModel.EstimationType = EstimationType.Interpreter;
        vm.EstimationModel.EstimationStatus = EstimationStatus.Created;
        vm.EstimationModel.CurrentUserID = currentUser.CurrentUserID;
        vm.EstimationModel.CurrentCulture = currentCulture;
        vm.EstimationModel.ApplicationId = appSettings.ApplicationId;
       // ajaxService.AjaxPostWithData(vm.EstimationModel, "estimationinterpretation/save", successFunction, errorFunction);
        vm.EstimationModel.CollectionFee = [];
        var obj = new Object();
        obj.ItemName = "";
        obj.UnitPrice = 0;
        obj.NoOfPersons = 0;
        obj.NoOfDays = 0;
        obj.SubTotal = 0;
        obj.IsCompleteSet = "";
        obj.IsExclTax = "";
        obj.IsMarkedForDelete = false;
        obj.EstimationID = "";
        var i = 0;
        if (vm.StaffAllowanceModel.Translation.SubTotal >= 0) {
            vm.EstimationModel.CollectionFee.push(angular.copy(obj));
            vm.EstimationModel.CollectionFee[i].ID = vm.StaffAllowanceModel.Translation.ID;
            vm.EstimationModel.CollectionFee[i].ItemName = vm.StaffAllowanceModel.Translation.ItemName;
            vm.EstimationModel.CollectionFee[i].UnitPrice = vm.StaffAllowanceModel.Translation.UnitPrice;
            vm.EstimationModel.CollectionFee[i].NoOfPersons = vm.StaffAllowanceModel.Translation.NoOfPersons;
            vm.EstimationModel.CollectionFee[i].NoOfDays = vm.StaffAllowanceModel.Translation.NoOfDays;
            vm.EstimationModel.CollectionFee[i].SubTotal = vm.StaffAllowanceModel.Translation.SubTotal;
            vm.EstimationModel.CollectionFee[i].IsCompleteSet = vm.StaffAllowanceModel.Translation.IsCompleteSet;
            vm.EstimationModel.CollectionFee[i].IsExclTax = vm.StaffAllowanceModel.Translation.IsExclTax;
            vm.EstimationModel.CollectionFee[i].AllowanceType = 1;
            vm.EstimationModel.CollectionFee[i].EstimationID = vm.StaffAllowanceModel.Translation.EstimationID;
            i++;
        }
        if (vm.StaffAllowanceModel.Accommodation.SubTotal >= 0) {
            vm.EstimationModel.CollectionFee.push(angular.copy(obj));
            vm.EstimationModel.CollectionFee[i].ID = vm.StaffAllowanceModel.Accommodation.ID;
            vm.EstimationModel.CollectionFee[i].ItemName = vm.StaffAllowanceModel.Accommodation.ItemName;
            vm.EstimationModel.CollectionFee[i].UnitPrice = vm.StaffAllowanceModel.Accommodation.UnitPrice;
            vm.EstimationModel.CollectionFee[i].NoOfPersons = vm.StaffAllowanceModel.Accommodation.NoOfPersons;
            vm.EstimationModel.CollectionFee[i].NoOfDays = vm.StaffAllowanceModel.Accommodation.NoOfDays;
            vm.EstimationModel.CollectionFee[i].SubTotal = vm.StaffAllowanceModel.Accommodation.SubTotal;
            vm.EstimationModel.CollectionFee[i].IsCompleteSet = vm.StaffAllowanceModel.Accommodation.IsCompleteSet;
            vm.EstimationModel.CollectionFee[i].IsExclTax = vm.StaffAllowanceModel.Accommodation.IsExclTax;
            vm.EstimationModel.CollectionFee[i].AllowanceType = 2;
            vm.EstimationModel.CollectionFee[i].EstimationID = vm.StaffAllowanceModel.Accommodation.EstimationID;
            i++;
        }
        if (vm.StaffAllowanceModel.Meal.SubTotal >= 0) {
            vm.EstimationModel.CollectionFee.push(angular.copy(obj));
            vm.EstimationModel.CollectionFee[i].ID = vm.StaffAllowanceModel.Meal.ID;
            vm.EstimationModel.CollectionFee[i].ItemName = vm.StaffAllowanceModel.Meal.ItemName;
            vm.EstimationModel.CollectionFee[i].UnitPrice = vm.StaffAllowanceModel.Meal.UnitPrice;
            vm.EstimationModel.CollectionFee[i].NoOfPersons = vm.StaffAllowanceModel.Meal.NoOfPersons;
            vm.EstimationModel.CollectionFee[i].NoOfDays = vm.StaffAllowanceModel.Meal.NoOfDays;
            vm.EstimationModel.CollectionFee[i].SubTotal = vm.StaffAllowanceModel.Meal.SubTotal;
            vm.EstimationModel.CollectionFee[i].IsCompleteSet = vm.StaffAllowanceModel.Meal.IsCompleteSet;
            vm.EstimationModel.CollectionFee[i].IsExclTax = vm.StaffAllowanceModel.Meal.IsExclTax;
            vm.EstimationModel.CollectionFee[i].AllowanceType = 3;
            vm.EstimationModel.CollectionFee[i].EstimationID = vm.StaffAllowanceModel.Meal.EstimationID;
            i++;

        }
        if (vm.StaffAllowanceModel.Allowance.SubTotal >= 0) {
            vm.EstimationModel.CollectionFee.push(angular.copy(obj));
            vm.EstimationModel.CollectionFee[i].ID = vm.StaffAllowanceModel.Allowance.ID;
            vm.EstimationModel.CollectionFee[i].ItemName = vm.StaffAllowanceModel.Allowance.ItemName;
            vm.EstimationModel.CollectionFee[i].UnitPrice = vm.StaffAllowanceModel.Allowance.UnitPrice;
            vm.EstimationModel.CollectionFee[i].NoOfPersons = vm.StaffAllowanceModel.Allowance.NoOfPersons;
            vm.EstimationModel.CollectionFee[i].NoOfDays = vm.StaffAllowanceModel.Allowance.NoOfDays;
            vm.EstimationModel.CollectionFee[i].SubTotal = vm.StaffAllowanceModel.Allowance.SubTotal;
            vm.EstimationModel.CollectionFee[i].IsCompleteSet = vm.StaffAllowanceModel.Allowance.IsCompleteSet;
            vm.EstimationModel.CollectionFee[i].IsExclTax = vm.StaffAllowanceModel.Allowance.IsExclTax;
            vm.EstimationModel.CollectionFee[i].AllowanceType = 4;
            vm.EstimationModel.CollectionFee[i].EstimationID = vm.StaffAllowanceModel.Allowance.EstimationID;
            i++;
        }

        if (vm.StaffAllowanceModel.Translation.ID != null && vm.StaffAllowanceModel.Translation.SubTotal <= 0)
        {
            vm.EstimationModel.CollectionFee[0].IsMarkedForDelete = true;
        }
        if (vm.StaffAllowanceModel.Accommodation.ID != null && vm.StaffAllowanceModel.Accommodation.SubTotal <= 0) {
            vm.EstimationModel.CollectionFee[1].IsMarkedForDelete = true;
        }
        if (vm.StaffAllowanceModel.Meal.ID != null && vm.StaffAllowanceModel.Meal.SubTotal <= 0) {
            vm.EstimationModel.CollectionFee[2].IsMarkedForDelete = true;
        }
        if (vm.StaffAllowanceModel.Allowance.ID != null && vm.StaffAllowanceModel.Allowance.SubTotal <= 0) {
            vm.EstimationModel.CollectionFee[3].IsMarkedForDelete = true;
        }

        ajaxService.AjaxPostWithData(vm.EstimationModel, "estimationinterpretation/save", successFunction, errorFunction);

    };

    var successFunction = function (response) {
        toastr.success($filter('translator')('DATASAVED'));
    };
    var errorFunction = function (response) {
        toastr.error("An error has occured, operation aborted");
    };

    vm.UploadFile = function (title,pos) {
        var binding = {};
        binding.component = "estimationFileSelection";
        binding.resolve = {};
        binding.resolve.modalData = {};
        binding.resolve.modalData.Document = NewDocument;
        binding.resolve.modalData.docIndex = pos;
        binding.resolve.modalData.listento = "receiveDocumentData";
        binding.resolve.title = function () { return title; };
        $uibModal.open(binding);
    };

    $scope.$on("receiveDocumentData", function (event, response) {
        var item = vm.EstimationModel.EstimationItems[response.Position];
        item.Document = response.Document;
        item.Document.WordCountStr = item.Document.WordCount.toLocaleString();
        vm.EstimationModel.AttachedMaterialFileName = response.Document.FileName;
        vm.EstimationModel.AttachedMaterialDownloadURL = response.Document.DownloadURL;
    });
   
    var onGetError= function (message)
    {
        toastr.error($filter('translator')('ERRORDBOPERATION'));
    }
    //ServiceType
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
    vm.ResetTime= function(item)
    {
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

        if (LateNightStartMeridian == 'PM')
        {
            LateNightInverseStartMeridian = 'AM';
        }
        else
        {
            LateNightInverseStartMeridian = 'PM';
        }

        if (LateNightFinishMeridian == 'PM') {
            LateNightInverseFinishMeridian = 'AM';
        }
        else {
            LateNightInverseFinishMeridian = 'PM';
        }


        if (item.StartTime != null && item.FinishTime != null)
        {
           
            var milisecondsDiff = item.FinishTime - item.StartTime;
                // vm.EstimationModel.TotalTimePeriod = Math.floor(milisecondsDiff / (1000 * 60 * 60)).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / (1000 * 60)) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / 1000) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 });
                var seconddif = milisecondsDiff / 1000;
                var minutedif = seconddif / 60;
                var hourdif = milisecondsDiff / (1000*60*60);
               
         
        }
        else {
            item.BasicTime = null;
        }

        // Split 24 Hour Time Format to 12 Hour Time Format

        if (item.StartTime != null && item.FinishTime != null)
        {
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
            else if(StartHour==0)
            {
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

            if (NewStartHour == NewFinishHour && NewStartMinute == NewFinishMinute && StartMeridian == FinishMeridian)
            {
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

            if (NewStartHour == NewFinishHour && NewStartMinute != NewFinishMinute && StartMeridian == FinishMeridian)
            {
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
        
            for (i =0 ;i<=totalhour+4 ; i++)
            {
                
                if((clockhour==13 || clockhour==12)&& isFirsCheck !=0)
                {
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
                    if(clockhour==NewFinishHour&& NewStartMeridian==FinishMeridian)
                    {
                        break;
                    }

                    if (clockhour >= 11 && NewStartMeridian == LateNightStartMeridian && clockhour < 12 || ((clockhour <= 6 && NewStartMeridian == LateNightFinishMeridian) || (clockhour==12 && NewStartMeridian==LateNightFinishMeridian)))
                    {
                        count++;
                    }
                    clockhour++;
                    isFirsCheck++;
            }
            //if (count >= 8)
            //{
            //    count--;
            //}

            vm.EstimationModel.BaseCount = count;
            if ((NewStartMinute != NewFinishMinute) || (StartMeridian !=FinishMeridian) )
            {
                if((NewStartHour>=LateNightStartHour&& StartMeridian==LateNightStartMeridian &&NewStartHour<12)|| (NewStartHour<=LateNightFinishHour&&StartMeridian==LateNightFinishMeridian)||(NewStartHour==12 && StartMeridian==LateNightFinishMeridian) && Flag)
                {
                    if((NewFinishHour>LateNightFinishHour && FinishMeridian==LateNightFinishMeridian && NewFinishHour<12) || (NewFinishHour<11 && FinishMeridian==LateNightStartMeridian)||(NewFinishHour==12 && FinishMeridian==LateNightStartMeridian))
                    {
                        count = count - 1;
                        var minutedif = NewStartMinute;
                        var totalminute = (count * 60) - minutedif;
                        count = totalminute / 60;
                    }
                }
                 if((NewFinishHour>=LateNightStartHour && FinishMeridian==LateNightStartMeridian && NewFinishHour<12)|| (NewFinishHour<=LateNightFinishHour && FinishMeridian==LateNightFinishMeridian)||(NewFinishHour==12 && FinishMeridian==LateNightFinishMeridian) && Flag)
                {
                    if((NewStartHour>LateNightFinishHour&& StartMeridian==LateNightFinishMeridian && NewStartHour<12) || (NewStartHour<11 && StartMeridian==LateNightStartMeridian)||(NewStartHour==12 && StartMeridian==LateNightStartMeridian))
                    {
                        var minutedif = NewFinishMinute;
                        var totalminute = (count * 60) + minutedif;
                        count = totalminute / 60;
                    }
                }
                 if((NewStartHour>=LateNightStartHour && StartMeridian==LateNightStartMeridian && NewStartHour<12)|| (NewStartHour<=LateNightFinishHour && StartMeridian==LateNightFinishMeridian)||(NewStartHour==12 && StartMeridian==LateNightFinishMeridian) && Flag)
                {
                    if((NewFinishHour>=LateNightStartHour && FinishMeridian==LateNightStartMeridian && NewFinishHour<12)|| (NewFinishHour<=LateNightFinishHour && FinishMeridian==LateNightFinishMeridian)||(NewFinishHour==12 && FinishMeridian==LateNightFinishMeridian))
                    {
                        if (NewStartMinute < NewFinishMinute)
                        {
                            var minutedif = NewFinishMinute - NewStartMinute;
                            var totalminute = (count * 60) + minutedif;
                            count = totalminute / 60;
                        }
                        else
                        {
                            var minutedif = NewStartMinute - NewFinishMinute;
                            var totalminute = (count * 60) - minutedif;
                            if (totalminute < 0)
                            {
                                totalminute = totalminute+(7 * 60);
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

    vm.PriceCalculation=function( item)
    {
        if (item.StartTime != null && item.FinishTime != null ) {
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
            if (item.BasicAmount != "" && item.BasicAmount != null && item.BasicAmount != 0) {
                item.TransferAmount = item.TransferTime * TransferFeePerHour;
            }
            else
            {
                item.TransferAmount = 0;
            }
            //Late Night Fee Calculation
            var LateAtNightAmount = 0;
            LateAtNightAmount = BasicAmount * (150 / 100);
            item.LateAtNightAmount = LateAtNightAmount * item.LateNightTime;

            item.UnitPriceSubTotal = item.AdditionalBasicAmount + item.ExtensionAmount + item.LateAtNightAmount + item.TransferAmount;

            vm.DiscountCalculation(item);
        }

    }

    vm.DiscountCalculation=function(item)
    {
        if (item.StartTime != null && item.FinishTime != null)
        {
            item.DiscountedPrice = item.UnitPriceSubTotal * (item.DiscountRate / 100);
            item.TotalAfterDiscount = item.UnitPriceSubTotal - item.DiscountedPrice;
        }
        else {
            return;
        }
        vm.TotalCostCalcultion(item);
    }

    vm.TotalCostCalcultion= function (item)
    {
        if (item.StartDate != null && item.CompletionDate != null  && item.StartTime != null && item.FinishTime != null)
        {
            if (item.NumberOfDays == 0)
            {
                item.NumberOfDays = 1;
            }
            item.Total = item.TotalAfterDiscount * item.NumberOfDays;
        }

        vm.FinalCostCalculation();
        
    }

    vm.TransFeeCalculation=function()
    {
        vm.StaffAllowanceModel.Translation.SubTotal = vm.StaffAllowanceModel.Translation.UnitPrice * vm.StaffAllowanceModel.Translation.NoOfPersons * vm.StaffAllowanceModel.Translation.NoOfDays;
        vm.StaffAllowanceModel.Accommodation.SubTotal = vm.StaffAllowanceModel.Accommodation.UnitPrice * vm.StaffAllowanceModel.Accommodation.NoOfPersons * vm.StaffAllowanceModel.Accommodation.NoOfDays;
        vm.StaffAllowanceModel.Meal.SubTotal = vm.StaffAllowanceModel.Meal.UnitPrice * vm.StaffAllowanceModel.Meal.NoOfPersons * vm.StaffAllowanceModel.Meal.NoOfDays;
        vm.StaffAllowanceModel.Allowance.SubTotal = vm.StaffAllowanceModel.Allowance.UnitPrice * vm.StaffAllowanceModel.Allowance.NoOfPersons * vm.StaffAllowanceModel.Allowance.NoOfDays;
        vm.StaffAllowanceModel.Total = vm.StaffAllowanceModel.Translation.SubTotal + vm.StaffAllowanceModel.Accommodation.SubTotal + vm.StaffAllowanceModel.Meal.SubTotal + vm.StaffAllowanceModel.Allowance.SubTotal;
        // Total Fee Calculation
        vm.EstimationModel.TotalCollectionCost = vm.StaffAllowanceModel.Translation.SubTotal + vm.StaffAllowanceModel.Accommodation.SubTotal + vm.StaffAllowanceModel.Meal.SubTotal + vm.StaffAllowanceModel.Allowance.SubTotal;
    }


    vm.GetAllCountryList=function()
    {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "country/list", onGetCountryList, onGetServiceError);
    }

    var onGetCountryList=function(response)
    {
        vm.CountryList = response;
    }


    vm.GetAllUnit=function()
    {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "unit/list", onGetUnitList, onGetServiceError);
    }

    var onGetUnitList=function(response)
    {
        vm.UnitList = response;
    }

    var onGetEstimationDetails = function (response) {
        vm.EstimationModel.EstimationItems = response;
        var length = response.length;
        for (i = 0; i < length; i++) {
            vm.EstimationModel.EstimationItems[i].StartDate = new Date(vm.EstimationModel.EstimationItems[i].StartDate);
            vm.EstimationModel.EstimationItems[i].CompletionDate = new Date(vm.EstimationModel.EstimationItems[i].CompletionDate);

            var starttimetoken = vm.EstimationModel.EstimationItems[i].StartTime.split(':');
            var startspacetoken = starttimetoken[1].split(' ');

            if (starttimetoken[0] == 12 && startspacetoken[1] == "AM") {
                starttimetoken[0] = parseInt(starttimetoken[0]) - 12;
            }
            else if (starttimetoken[0] < 12 && startspacetoken[1] == "PM") {
                starttimetoken[0] = parseInt(starttimetoken[0]) + 12;
            }
            else {

            }

            vm.EstimationModel.EstimationItems[i].StartTime = new Date(1970, 0, 1, starttimetoken[0], startspacetoken[0]);

            var finishtimetoken = response[i].FinishTime.split(':');
            var finishspacetoken = finishtimetoken[1].split(' ');

            if (finishtimetoken[0] == 12 && finishspacetoken[1] == "AM") {
                finishtimetoken[0] = parseInt(finishtimetoken[0]) - 12;
            }
            else if (finishtimetoken[0] < 12 && finishspacetoken[1] == "PM") {
                finishtimetoken[0] = parseInt(finishtimetoken[0]) + 12;
            }
            else {

            }

            vm.EstimationModel.EstimationItems[i].FinishTime = new Date(1970, 0, 1, finishtimetoken[0], finishspacetoken[0]);

            vm.EstimationModel.EstimationItems[i].NewStartTime = starttimetoken[0] + ":" + startspacetoken[0] + " " + startspacetoken[1];
            vm.EstimationModel.EstimationItems[i].NewFinishTime = finishtimetoken[0] + ":" + finishspacetoken[0] + " " + finishspacetoken[1];

        }
        vm.FinalCostCalculation();
    };

    var manageState = function () {
        if ($stateParams.id != "" && $stateParams.id != null) {
            if ($stateParams.Estimation != null) {
                $localStorage.TranslationEstimation = $stateParams.Estimation;
            }
            vm.EstimationModel = $localStorage.TranslationEstimation;
        }
    }

    var loadEstimationActionList = function () {
        var BaseModel = new Object();
        BaseModel.CurrentUserID = currentUser.CurrentUserID;
        BaseModel.CurrentCulture = currentCulture;
        BaseModel.ApplicationId = appSettings.ApplicationId;
        ajaxService.AjaxPostWithData(BaseModel, "estimationaction/list/" + vm.EstimationModel.ID, OnGetActionList, errorFunction)
    };

    var OnGetActionList=function(response)
    {
        var actionItems = [];
        for(var x=0;x<response.length;x++)
        {
            var actionItem = {
                ID: response[x].ID,
                EstimationID: response[x].EstimationID,
                Date: response[x].NextActionDate,
                Updatedby: response[x].OperandName,
                Detail:response[x].ActionDetails
            }
            actionItems.push(actionItem);
        }
        vm.EstimationModel.ActionItems = [].concat(actionItems);
    }

    vm.LoadStaffAllowance=function()
    {
        if (vm.EstimationModel.ID != null && vm.EstimationModel.ID != "") {
            var BaseModel = new Object();
            BaseModel.CurrentUserID = currentUser.CurrentUserID;
            BaseModel.CurrentCulture = currentCulture;
            BaseModel.ApplicationId = appSettings.ApplicationId;
            ajaxService.AjaxPostWithData(BaseModel, "estimationinterpretation/getstaffallowancebyid/" + vm.EstimationModel.ID, OnSuccessallowance, onGetError);
        }
    }

    var OnSuccessallowance=function(response)
    {
        vm.EstimationModel.TotalCollectionCost = 0;
        for(i=0;i<response.length;)
        {
            if(response[i].AllowanceType==1)
            {
                // Data load to Translation Fee
                vm.StaffAllowanceModel.Translation.ID = response[i].ID;
                vm.StaffAllowanceModel.Translation.ItemName = response[i].ItemName
                vm.StaffAllowanceModel.Translation.UnitPrice = response[i].UnitPrice;
                vm.StaffAllowanceModel.Translation.NoOfPersons = response[i].NoOfPersons;
                vm.StaffAllowanceModel.Translation.NoOfDays = response[i].NoOfDays;
                vm.StaffAllowanceModel.Translation.SubTotal = response[i].SubTotal;
                vm.StaffAllowanceModel.Translation.IsCompleteSet = response[i].IsCompleteSet;
                vm.StaffAllowanceModel.Translation.IsExclTax = response[i].IsExclTax;
                vm.StaffAllowanceModel.Translation.EstimationID = response[i].EstimationID;

            }
            if(response[i].AllowanceType==2)
            {
                //Data load to Accommodation Fee
                vm.StaffAllowanceModel.Accommodation.ID = response[i].ID;
                vm.StaffAllowanceModel.Accommodation.ItemName = response[i].ItemName;
                vm.StaffAllowanceModel.Accommodation.UnitPrice = response[i].UnitPrice;
                vm.StaffAllowanceModel.Accommodation.NoOfPersons = response[i].NoOfPersons;
                vm.StaffAllowanceModel.Accommodation.NoOfDays = response[i].NoOfDays;
                vm.StaffAllowanceModel.Accommodation.SubTotal = response[i].SubTotal;
                vm.StaffAllowanceModel.Accommodation.IsCompleteSet = response[i].IsCompleteSet;
                vm.StaffAllowanceModel.Accommodation.IsExclTax = response[i].IsExclTax;
                vm.StaffAllowanceModel.Accommodation.EstimationID = response[i].EstimationID;


            }
            if(response[i].AllowanceType==3)
            {
                //Data Load to Meal Fee
                vm.StaffAllowanceModel.Meal.ID = response[i].ID;
                vm.StaffAllowanceModel.Meal.ItemName = response[i].ItemName;
                vm.StaffAllowanceModel.Meal.UnitPrice = response[i].UnitPrice;
                vm.StaffAllowanceModel.Meal.NoOfPersons = response[i].NoOfPersons;
                vm.StaffAllowanceModel.Meal.NoOfDays = response[i].NoOfDays;
                vm.StaffAllowanceModel.Meal.SubTotal = response[i].SubTotal;
                vm.StaffAllowanceModel.Meal.IsCompleteSet = response[i].IsCompleteSet;
                vm.StaffAllowanceModel.Meal.IsExclTax = response[i].IsExclTax;
                vm.StaffAllowanceModel.Meal.EstimationID = response[i].EstimationID;

            }
            if(response[i].AllowanceType==4)
            {
                //Data Load to Daily Allowance
                vm.StaffAllowanceModel.Allowance.ID = response[i].ID;
                vm.StaffAllowanceModel.Allowance.ItemName = response[i].ItemName;
                vm.StaffAllowanceModel.Allowance.UnitPrice = response[i].UnitPrice;
                vm.StaffAllowanceModel.Allowance.NoOfPersons = response[i].NoOfPersons;
                vm.StaffAllowanceModel.Allowance.NoOfDays = response[i].NoOfDays;
                vm.StaffAllowanceModel.Allowance.SubTotal = response[i].SubTotal;
                vm.StaffAllowanceModel.Allowance.IsCompleteSet = response[i].IsCompleteSet;
                vm.StaffAllowanceModel.Allowance.IsExclTax = response[i].IsExclTax;
                vm.StaffAllowanceModel.Allowance.EstimationID = response[i].EstimationID;

            }
            vm.EstimationModel.TotalCollectionCost = vm.EstimationModel.TotalCollectionCost + response[i].SubTotal;
            i++;
        }
        vm.TransFeeCalculation();
    }
    vm.FinalCostCalculation=function()
    {
        var TotalCost = 0;
        var TotalDiscount = 0;
        var i=0;
        for(i=0;i< vm.EstimationModel.EstimationItems.length;i++)
        {
            TotalCost = TotalCost + vm.EstimationModel.EstimationItems[i].Total;
            TotalDiscount = TotalDiscount + vm.EstimationModel.EstimationItems[i].DiscountedPrice;
        }
        vm.EstimationModel.TotalCost = TotalCost;
        vm.EstimationModel.TotalDiscount = TotalDiscount;
        //Total Estimation Price(Including Tax) Calculation
        var TotalCostIncludingTax = 0;
        for(i=0;i<vm.EstimationModel.EstimationItems.length;i++)
        {
            if(vm.EstimationModel.EstimationItems[i].IsOverseas==true)
            {
                TotalCostIncludingTax = TotalCostIncludingTax + vm.EstimationModel.EstimationItems[i].Total;
            }
        }
        vm.EstimationModel.QuotationExclTax = TotalCostIncludingTax;
        //Total Estimation Price(Excluding Tax) Calculation
        var TotalCostExcludingTax = 0;
        for(i=0;i<vm.EstimationModel.EstimationItems.length;i++)
        {
            if(vm.EstimationModel.EstimationItems[i].IsOverseas==false)
            {
                TotalCostExcludingTax=TotalCostExcludingTax+vm.EstimationModel.EstimationItems[i].Total
            }
        }
        vm.EstimationModel.TaxEstimate = TotalCostExcludingTax;
        // Consumption Tax 8% Calculation
        vm.EstimationModel.ConsumptionOnTax = vm.EstimationModel.TaxEstimate * (8 / 100);
        vm.EstimationModel.ExcludedTaxCost = parseInt(vm.EstimationModel.ExcludedTaxCost);
        vm.EstimationModel.QuotationInclTax = vm.EstimationModel.TaxEstimate + vm.EstimationModel.QuotationExclTax + vm.EstimationModel.ConsumptionOnTax + vm.EstimationModel.ExcludedTaxCost;
    }
}