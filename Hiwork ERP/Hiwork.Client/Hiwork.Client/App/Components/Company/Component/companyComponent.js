angular.module("hiworkApp").component('companyregistration', {
    templateUrl: 'App/Components/Company/Template/company.html',
    bindings: {
        configData: "=",
        bankData: "=",
        bankAccountTypeData: "=",
        bankBranchData: "=",
        designationData:"="
    },
    controllerAs: "vm",
    controller: companyController
})

companyController.$inject = ['$scope', '$uibModal', 'appSettings', 'AppStorage', 'sessionFactory', 'loginFactory', '$filter', 'ajaxService', '$state', '$stateParams', 'fileUploadService'];

function companyController($scope, $uibModal, appSettings, AppStorage, sessionFactory, loginFactory, $filter, ajaxService, $state, $stateParams, fileUploadService) {
    
    var vm = this;
    $scope.vm = {};
    $scope.vm.company = {};
    $scope.vm.companyDepartment = {};
    $scope.vm.ClassificationList = null;

    $scope.IsBlocked = false;

    var ID = $stateParams.ID;
    $scope.imageUploaded = false;
    $scope.vm.CompanyNo = '';
    var tempBankBranchList = [];
    var tempBankList = [];
    $scope.vm.BusinesClassification = '';
    var BusinesClassificationList = [];
    $scope.vm.IsCompanyRegistered = false;
    $scope.vm.ClientLocationType = [];
    $scope.vm.CompanyType = [];
    $scope.vm.RegPurposeType = [];
    $scope.vm.TradingOfficeList = [];
    $scope.vm.divisionList = [];
    var CompanyClassification = [];
    var uid = 1;

    $scope.isDisabled = true;

    $scope.IsHideClientNo = true;

    $scope.vm.PriceList =[];

    var DummyClassification = { ID: "", CompanyID: "", IndustryClassificationID: "", IndustryClassificationItemID: "", IsSelected: false };



    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

       var AffiliateCodeUpper = [{ ID: '001', Name: 'trans-pro.net' },
                         {ID:'011', Name:'trans-pro.Kr'},
                         {ID:'020', Name:'trans-pro.au.com'}];

    var BaseModel = {}
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    this.$onInit = function () {
        
        getNextCompanyNo();
        //getTradingDivision();
        $scope.vm.ClientLocationType = vm.configData.ClientLocationType,
        $scope.vm.CompanyType = vm.configData.CompanyType;
        $scope.vm.RegPurposeType = vm.configData.RegPurposeType;
        $scope.vm.BranchOfficeList = vm.configData.BranchOfficeList;
        $scope.vm.CompanyDeptList = vm.configData.DepartmentList;
        $scope.vm.ActivityTypeList = vm.configData.ActivityType;
        $scope.vm.ResultofActivity = vm.configData.ResultofActivity;
        // $scope.vm.divisionList = vm.configData.DepartmentList;
        $scope.vm.LanguageList = vm.configData.LanguageList;
        $scope.vm.EstimationTypeList = vm.configData.EstimationTypeList;
        $scope.vm.SpecializationList = vm.configData.SpecializationList;
        $scope.vm.PriceCalculateTypeList = vm.configData.PriceCalculateTypeList;
        $scope.vm.PamentWayList = vm.configData.PamentWayList;
        $scope.vm.AffiliateType = vm.configData.AffiliateType;
        $scope.vm.PartnerServiceTypeList = vm.configData.PartnerServiceTypeList;
        $scope.vm.DeliveryMethodList = vm.configData.DeliveryMethodList;

        $scope.checkboxSelection = '1';
     //   getBankData();
      //  getDivision();
        if ($stateParams.ID !== null) {
            $scope.IsBlocked = false;
            BaseModel.ID = $stateParams.ID;
            $scope.IsHideClientNo = false;
            GetCompanyByID();
            getCompanyIndustryClassifications();
        }
        else {
            $scope.IsBlocked = true;
            getCompanyIndustryClassifications();
        }
    };
    
    $scope.RouteForExistingCompany = function ($event) {
        if ($scope.IsBlocked == true) {
            $event.preventDefault();
        }
    };

    function getBankData() {
       
        ajaxService.AjaxPostWithData(BaseModel, "employee/formData", onGetDataForm, onGetError);
    }
    function getTradingDivision() {
        
        ajaxService.AjaxPostWithData(BaseModel, "companytradingdivision/list", onGetTradingDivision, onGetError);
    }
    function getDivision()
    {
        ajaxService.AjaxPostWithData(BaseModel, "division/list", onGetDivision, onGetError);
    }
    function getNextCompanyNo()
    {
        ajaxService.AjaxPostWithData(BaseModel,"company/nextcompanyno",onGetCompanyNo,onGetError)
    }
    function getCompanyIndustryClassifications() {
        ajaxService.AjaxPostWithData(BaseModel, "companyindustryclassification/list", onGetCompanyIndustryClassification, onGetError);
    }

    function GetCompanyByID()
    {
        ajaxService.AjaxPostWithData(BaseModel, "company/getbyid", onGetCompanyByID, onGetError);
    }

    $scope.$on("selectedEmployee", function (event, response) {
        $scope.vm.EmployeeMemberID = response[0].ID;
        $scope.vm.EmployeeMemberName = response[0].Name;
    });

    var onGetCompanyByID = function (response) {
        $scope.vm.PriceList = {};
        $scope.vm = angular.copy(response);
        $scope.vm.EstablishedDate = new Date(response.EstablishedDate);
        $scope.vm.PicSalesPlanDate = new Date(response.PicSalesPlanDate);
        $scope.vm.ClientLocationType = vm.configData.ClientLocationType,
        $scope.vm.CompanyType = vm.configData.CompanyType;
        $scope.vm.RegPurposeType = vm.configData.RegPurposeType;
        $scope.vm.BranchOfficeList = vm.configData.BranchOfficeList;
        $scope.vm.ActivityType = vm.configData.ActivityType;
        $scope.vm.ResultofActivity = vm.configData.ResultofActivity;
        $scope.vm.CompanyDeptList = vm.configData.DepartmentList;
        $scope.vm.LanguageList = vm.configData.LanguageList;
        $scope.vm.EstimationTypeList = vm.configData.EstimationTypeList;
        $scope.vm.SpecializationList = vm.configData.SpecializationList;
        $scope.vm.PriceCalculateTypeList = vm.configData.PriceCalculateTypeList;
        $scope.vm.SalesRecordRegDate = new Date(response.SalesRecordRegDate);
        $scope.vm.StartTime = new Date(response.StartTime);
        $scope.vm.EndTime = new Date(response.EndTime);
        $scope.vm.PriceList = response.AgencyPrice;
        $scope.vm.PamentWayList = vm.configData.PamentWayList;
        $scope.vm.AffiliateType = vm.configData.AffiliateType;
        $scope.vm.AffiliateUpdateDate = new Date(response.AffiliateUpdateDate)
        $scope.vm.PartnerServiceTypeList = vm.configData.PartnerServiceTypeList;
        $scope.vm.BankList = vm.bankData;
        $scope.vm.BankBranchList = vm.bankBranchData;
        $scope.vm.BankAccTypeList = vm.bankAccountTypeData;
        $scope.vm.DesignationList = vm.designationData;
        $scope.vm.PartnerType = vm.configData.PartnerTypeList;
        $scope.vm.AffiliateCodeUpper = AffiliateCodeUpper;
        $scope.vm.DeliveryMethodList = vm.configData.DeliveryMethodList;
        $scope.checkboxSelection = '1';
    };

    var onGetTradingDivision = function (response) {
        $scope.TradingList = [];
        $scope.TradingList = response;
    };

    var onGetDivision = function (response) {
        $scope.divisionList = [];
        $scope.divisionList = response;
    };

    var onGetDataForm = function (response) {
        $scope.BankList = [];
        $scope.BankList = response.banks;
        $scope.BankBranchList = [];
        $scope.BankBranchList = response.bankbranches;
        $scope.BankAccTypeList = [];
        $scope.BankAccTypeList = response.bankAccountTypes;
    };

    var onGetCompanyNo = function (response) {
        $scope.vm.ClientNo = response;
    };

    var onGetCompanyIndustryClassification = function (response) {
        var SteteObj;
        CompanyClassification = response;
        $scope.industryClassificationChunk = [];
        $scope.industryClassificationChunk = _.chunk(response, 7);
        _.forEach($scope.industryClassificationChunk, function (companyIndustryClassificationList) {
            _.forEach(companyIndustryClassificationList, function (companyIndustryClassification) {
                Object.defineProperty(companyIndustryClassification, 'IsSelected', { value: false, writable: true, enumerable: true, configurable: true });
                Object.defineProperty(companyIndustryClassification, 'ChildSelectedCount', { value: 0, writable: true, enumerable: true, configurable: true });
                companyIndustryClassification.IsSelected = false;
                companyIndustryClassification.ChildSelectedCount = 0;
                _.forEach(companyIndustryClassification.itemList, function (companyIndustryClassificationItem) {
                    StateObj = new Object();
                    StateObj.ID = null;
                    StateObj.CompanyID = null;
                    StateObj.IndustryClassificationID = null;
                    StateObj.IndustryClassificationItemID = null;
                    StateObj.IsSelected = false;
                    Object.defineProperty(companyIndustryClassificationItem, 'State', { value: StateObj, writable: true, enumerable: true, configurable: true });
                });
            });
        });

        if ($scope.IsBlocked == false) {
            ajaxService.AjaxPostWithData(BaseModel, "company/industryclassificationviewlist", onGetCompanyIndustryClassificationView, onGetError);
        }
    };

    var onGetCompanyIndustryClassificationView = function (response) {

        var index = 0;
        $scope.vm.ClassificationList = response;
        var i, j, k;
        var ClassificationObj;
        
        for (i = 0; i < $scope.industryClassificationChunk.length; i++) {
            for (j = 0; j < $scope.industryClassificationChunk[i].length; j++) {
                for (k = 0; k < $scope.industryClassificationChunk[i][j].itemList.length; k++) {

                    $scope.industryClassificationChunk[i][j].itemList[k].State.IndustryClassificationID = $scope.industryClassificationChunk[i][j].Id;
                    $scope.industryClassificationChunk[i][j].itemList[k].State.IndustryClassificationItemID = $scope.industryClassificationChunk[i][j].itemList[k].Id;

                    for (index = 0; index < $scope.vm.ClassificationList.length; index += 1) {
                        if ($scope.vm.ClassificationList[index].IndustryClassificationItemID == $scope.industryClassificationChunk[i][j].itemList[k].Id) {
                            $scope.industryClassificationChunk[i][j].itemList[k].State.ID = $scope.vm.ClassificationList[index].ID;
                            $scope.industryClassificationChunk[i][j].itemList[k].State.CompanyID = $scope.vm.ClassificationList[index].CompanyID;
                            $scope.industryClassificationChunk[i][j].itemList[k].State.IndustryClassificationID = $scope.vm.ClassificationList[index].IndustryClassificationID;
                            $scope.industryClassificationChunk[i][j].itemList[k].State.IndustryClassificationItemID = $scope.vm.ClassificationList[index].IndustryClassificationItemID;
                            $scope.industryClassificationChunk[i][j].itemList[k].State.IsSelected = $scope.vm.ClassificationList[index].IsSelected;
                            if ($scope.industryClassificationChunk[i][j].itemList[k].State.IsSelected == true) {
                                $scope.industryClassificationChunk[i][j].ChildSelectedCount += 1;
                            }
                            $scope.industryClassificationChunk[i][j].IsSelected = $scope.industryClassificationChunk[i][j].ChildSelectedCount == $scope.industryClassificationChunk[i][j].itemList.length;
                            break;
                        }
                    }
                }
                
                //ClassificationObj = $scope.industryClassificationChunk[i][j];
                //console.log("Classification: " + ClassificationObj.Name + " ChildSelected: " + ClassificationObj.ChildSelectedCount + "/" + ClassificationObj.itemList.length);
            }
        };
    };

    var onGetError = function (message) {
        toastr.error('Error in getting records');
    };

    $scope.companyIndustryClassificationItemSelectionChanged = function (companyIndustryClassification, companyIndustryClassificationItem) {
        if (_.isUndefined(companyIndustryClassification.ChildSelectedCount) == true) {
            companyIndustryClassification.ChildSelectedCount = 0;
        }
        if (companyIndustryClassificationItem.State.IsSelected == false) {
            companyIndustryClassification.ChildSelectedCount -= 1;
        }
        else if (companyIndustryClassificationItem.State.IsSelected == true) {
            companyIndustryClassification.ChildSelectedCount += 1;
        }
        companyIndustryClassification.IsSelected = companyIndustryClassification.ChildSelectedCount == companyIndustryClassification.itemList.length;
    };

    $scope.companyIndustryClassificationSelectionChanged = function (companyIndustryClassification) {
        _.forEach(companyIndustryClassification.itemList, function(companyIndustryClassificationItem) {
            companyIndustryClassificationItem.State.IsSelected = companyIndustryClassification.IsSelected;
        });
        $scope.vm.BusinesClassification = '';
        companyIndustryClassification.ChildSelectedCount = companyIndustryClassification.IsSelected == true ? companyIndustryClassification.itemList.length : 0;

        if (companyIndustryClassification.IsSelected)
        {
            BusinesClassificationList.push(companyIndustryClassification.Name);
        }
        else
        {
            if (BusinesClassificationList.length > 0)
            {
                _.pull(BusinesClassificationList, companyIndustryClassification.Name);
            }
        }
        _.forEach(BusinesClassificationList, function (Name) {

            $scope.vm.BusinesClassification += Name + "\n";
        })
       
    };

    //function changeStatus(event) {

    //    var currentId = event.target.id;

    //    if (currentId === 'salesRecord' || currentId === 'salesUrl') {

    //        $state.go('StaffRegistration.educationalInfo');
    //        var eduEl = angular.element(document.querySelector('#eduLnk'));
    //        eduEl.removeClass('active');
    //        eduEl.addClass('active');
    //        return;
    //    }
    //}


    //$scope.uploadLogo = function () {
    //    var file = event.target.files[0];
    //    var reader = new FileReader();
    //    reader.onload = function (evt) {
    //        $scope.$apply(function ($scope) {
    //            $scope.vm.Logo = evt.target.result;
    //        });
    //    };
    //    reader.readAsDataURL(file);
    //};

    //$scope.uploadPhoto = function () {
    //    var file = event.target.files[0];
    //    var reader = new FileReader();
    //    reader.onload = function (evt) {
    //        $scope.$apply(function ($scope) {
    //            $scope.vm.MainPhoto = evt.target.result;
    //        });
    //    };
    //    reader.readAsDataURL(file);
    //};


    $scope.saveCompany = function (param) {

        $scope.vm.CurrentUserID = currentUser.CurrentUserID;
        $scope.vm.CurrentCulture = currentCulture;
        $scope.vm.ApplicationId = appSettings.ApplicationId;
        $scope.vm.TabId = param;
        $scope.vm.AgencyPrice = $scope.vm.PriceList;

        ajaxService.AjaxPostWithData($scope.vm, "comapny/save", onSuccess, onError);
        var index;
        var ClassificationList = [];

        if (param === 2) {
            var i, j, k;
            for (i = 0; i < $scope.industryClassificationChunk.length; i++) {
                for (j = 0; j < $scope.industryClassificationChunk[i].length; j++) {
                    for (k = 0; k < $scope.industryClassificationChunk[i][j].itemList.length; k++) {
                        $scope.industryClassificationChunk[i][j].itemList[k].State.CompanyID = $scope.vm.ID;
                        ClassificationList.push($scope.industryClassificationChunk[i][j].itemList[k].State);
                    }
                }
            }
            ajaxService.AjaxPostWithData(ClassificationList, "company/industryclassificationviewsavelist", onSuccess, onError);
        }
    };

    $scope.SaveTranspro=function(param)
    {
        if ($scope.vm.ID == '')
            return;
        $scope.vm.transpro.CurrentUserID = currentUser.CurrentUserID;
        $scope.vm.transpro.CurrentCulture = currentCulture;
        $scope.vm.transpro.ApplicationId = appSettings.ApplicationId;
        $scope.vm.transpro.CompanyID = $scope.vm.ID;
        ajaxService.AjaxPostWithData($scope.vm.transpro, "comapnyTranspro/save", onSuccess, onError);

    }

    var onError = function (response) {
        toastr.error('Error in saving records');
    };

    var onSuccess = function (response) {
        if (response === null) {
            $scope.IsBlocked = true;
            $scope.vm.IsCompanyRegistered = false;
            toastr.error('Error in saving records');
        }
        else {
            $scope.IsBlocked = false;
            $scope.vm.IsCompanyRegistered = true;
            toastr.success($filter('translator')('DATASAVED'));
        }
    };

    $scope.openEmailWindow = function () {
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


    $scope.AddPrice = function (price) {
        $scope.isTriedSave = true;
        if (!price.EstimationTypeID || !price.SourceLanguageID || !price.DestinationLanguageID || !price.SpecializedFieldID || !price.PriceCalculationOnID  || !price.Unit || !price.UnitPrice 
            || price.EstimationTypeID=="" || price.SourceLanguageID=="" || price.DestinationLanguageID=="" || price.SpecializedFieldID=="" || price.PriceCalculationOnID==""  || price.Unit=="" || price.UnitPrice=="" )
            return;

        if (price.ID == null) {
            price.ID = uid++;
            price.EstimationTypeName = _.find($scope.vm.EstimationTypeList, { ID: price.EstimationTypeID }).Name;
            price.SourceLanguageName = _.find($scope.vm.LanguageList, { ID: price.SourceLanguageID }).Name;
            price.DestinationLanguageName = _.find($scope.vm.LanguageList, { ID: price.DestinationLanguageID }).Name;
            price.SpecializedFieldName = _.find($scope.vm.SpecializationList, { ID: price.SpecializedFieldID }).Name;
            price.PriceCalculationOnName = _.find($scope.vm.PriceCalculateTypeList, { Id: price.PriceCalculationOnID }).Name;
            price.Unit = price.Unit;
            price.UnitPrice = price.UnitPrice;
            $scope.vm.PriceList.push(price);
        }
        else {
            for (i in $scope.vm.PriceList) {
                if ($scope.vm.PriceList[i].ID == price.ID) {
                    price.EstimationTypeName = _.find($scope.vm.EstimationTypeList, { ID: price.EstimationTypeID }).Name;
                    price.SourceLanguageName = _.find($scope.vm.LanguageList, { ID: price.SourceLanguageID }).Name;
                    price.DestinationLanguageName = _.find($scope.vm.LanguageList, { ID: price.DestinationLanguageID }).Name;
                    price.SpecializedFieldName = _.find($scope.vm.SpecializationList, { ID: price.SpecializedFieldID }).Name;
                    price.PriceCalculationOnName = _.find($scope.vm.PriceCalculateTypeList, { Id: price.PriceCalculationOnID }).Name;
                    price.Unit = price.Unit;
                    price.UnitPrice = price.UnitPrice;
                    $scope.vm.PriceList[i] = price;
                }
            }
        }
        
        price = {};
        $scope.AgencyPrice = " ";
     
    };

    $scope.edit = function (price) {
        $scope.AgencyPrice = angular.copy(price);
    };

    $scope.isCheckboxSelected = function (index) {

        if (index === 1)
        {
            $scope.isDisabled = true;
        }
        else
        {
            $scope.isDisabled = false;
        }
       
    };

    $scope.handleChange = function (input) {
        if (input.value < 0) input.value = 0;
        if (input.value > 100) input.value = 100;
    }



}

