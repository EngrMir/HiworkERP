/**
 * Created by Mahfuz on 06/20/2014.
 */

var app = angular.module("hiworkApp");


app.component('staffRegistration', {

    templateUrl: 'App/Components/StaffRegistration/Templates/staffRegistration.html',
    bindings: {
        countryData: "=",
        //languageSkillLevelData: "=",
        bankData: "=",
        bankAccountTypeData: "=",
        staffVisaTypeData: "=",
        staffBusinessCategoryData: "=",
        staffBusinessCategoryDetailData: "=",
        staffJobCategoryData: "=",
        staffJobSubCategoryData: "=",
        staffEmploymentTypeData: "=",
        staffLanguageData: "=",
        staffStateData: "=",
        staffBankBranchData: "=",
        staffEducationDegreeData: "=",
        staffTechSkillItemData: "=",
        staffTranslation: "=",
        staffSpecialField: "=",
        staffProfession: "=",
        staffLanguagefl1: "=",
        staffLanguagefl2: "=",
        staffLanguagefl3: "=",
        countryDataBank: "=",
        staffSubjectData: "=",
        staffDegreeData: "=",
        narrationType: "="

    },
    controllerAs: "vm",
    controller: staffRegistrationController

});


staffRegistrationController.$inject = ['$scope', '$filter', 'translatorService', 'StaffRegistrationService', 'sessionFactory', 'AppStorage', 'appSettings', '$state', 'alerting', 'localStorage', 'ajaxService', '$stateParams'];


function staffRegistrationController($scope, $filter, translatorService, StaffRegistrationService, sessionFactory, AppStorage, appSettings, $state, alerting, localStorage, ajaxService, $stateParams) {

    var vm = this;
    vm.baseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    vm.baseModel.CurrentUserID = currentUser.CurrentUserID;
    vm.baseModel.CurrentCulture = currentCulture;
    vm.baseModel.ApplicationId = appSettings.ApplicationId;
    vm.jobHistoryData = {};
    vm.EducationalInfoModel = {};
    vm.data = {};
    vm.bankInfoData = {};
    vm.StaffBankModel = {};
    vm.StaffTranProModel = {};
    vm.saveStaff = saveStaff;
    vm.addSkill = addSkill;
    vm.isDisabledStaff = false;
    vm.isDisabledJobDetail = false;
    vm.yearNames = getYears();
    vm.AddEducation = AddEducation;
    vm.SaveEducation = SaveEducation;
    vm.saveStaff = saveStaff;
    vm.resetForm = resetForm;
    vm.data = initialDataPattern;
    vm.changeStatus = changeStatus;
    vm.guid = guid;

    var FormObject = function (formName) {
        return formName;
    }
    var currentForm = "";

    //if (localStorage.get('isDisableFlag')) {
    //    $state.go('StaffRegistration.details');
    //    var eduEl = angular.element(document.querySelector('#dtLnk'));
    //    eduEl.addClass('active');
    //    isDisabledOther(true);
    //    return;
    //}


    if ($stateParams.staffNo != "" && $stateParams.staffNo != null) {
        ajaxService.AjaxPostWithData({ "value": $stateParams.staffNo }, "stafflistGet/getID", onGetDataRedirect, function (error) {
            console.log(error);
        });
    }
    function onGetDataRedirect(response) {
        console.log(response)

        if (response.staff != null) {
            vm.data = response.staff;

            vm.data.CurrentUserID = vm.baseModel.CurrentUserID;
            vm.data.CurrentCulture = vm.baseModel.CurrentCulture;
            vm.data.ApplicationId = vm.baseModel.ApplicationId;
            vm.data.ID = guid();
            if (currentCulture === 'en') {
                vm.data.FirstName = vm.data.FirstName_en;
                vm.data.NickName = vm.data.NickName_en;
                vm.data.LastName = vm.data.LastName_en;
                vm.data.Address = vm.data.Address_en;
                if (vm.data.TownName_en != null) vm.staffStateData.selected = vm.staffStateData[findWithAttr(vm.staffStateData, 'Name_en', response.staff.TownName_en)];
                vm.data.District = vm.data.District_en;
                vm.data.MiddleName = vm.data.MiddleName_en;
            }

            vm.staffVisaTypeData.selected = vm.staffVisaTypeData[findWithAttr(vm.staffVisaTypeData, 'ID', response.staff.VisaTypeID)];
            vm.countryData.selected = vm.countryData[findWithAttr(vm.countryData, 'ID', response.staff.CountryOfCitizenship)];
            //vm.countryData.selected = response.staff.LivingCountryID;
            vm.staffLanguageData.selected = vm.staffLanguageData[findWithAttr(vm.staffLanguageData, 'ID', response.staff.NativeLanguageID)];
            vm.staffLanguagefl1.selected = vm.staffLanguagefl1[findWithAttr(vm.staffLanguagefl1, 'ID', response.staff.ForiegnLanguage1ID)];
            vm.staffLanguagefl2.selected = vm.staffLanguagefl2[findWithAttr(vm.staffLanguagefl2, 'ID', response.staff.ForiegnLanguage2ID)];
            vm.staffLanguagefl3.selected = vm.staffLanguagefl3[findWithAttr(vm.staffLanguagefl3, 'ID', response.staff.ForiegnLanguage3ID)];
            //vm.staffLanguagefl4.selected = response.staff.ForiegnLanguage4ID;
            vm.skilLevels1.selected = vm.skilLevels1[findWithAttr(vm.skilLevels1, 'key', response.staff.ForeignLang1Level)];
            vm.skilLevels2.selected = vm.skilLevels2[findWithAttr(vm.skilLevels2, 'key', response.staff.ForeignLang2Level)];
            vm.skilLevels3.selected = vm.skilLevels3[findWithAttr(vm.skilLevels3, 'key', response.staff.ForeignLang3Level)];
            vm.skilLevels4.selected = vm.skilLevels4[findWithAttr(vm.skilLevels4, 'key', response.staff.ForeignLang4Level)];
            //angular.element($('#file'))[0] = response.staff.SelfVideoURL;

            vm.staffBankBranchData.selected = vm.staffBankBranchData[findWithAttr(vm.staffBankBranchData, 'Name', response.staff.BankBranchName)];
            vm.bankAccountTypeData.selected = vm.bankAccountTypeData[findWithAttr(vm.bankAccountTypeData, 'Name', response.staff.BankBranchName)];;

        }

        vm.jobHistoryData = response.staffJobHistory;
        vm.EducationalInfoModel = response.staffEducationalHistory;
        if (response.staffSkillCertificate !== null) {
            vm.SkillCertificateModel = response.staffSkillCertificate.SkillCertificateModel;
            vm.TechnicalCertificateModel = response.staffSkillCertificate.TechnicalCertificateModel;
        }
        vm.TRExperienceModel = response.staffTRExperience !== null ? response.staffTRExperience : "";
        vm.StaffBankModel = response.staffBankAccountInfoModel !== null ? response.staffBankAccountInfoModel : "";
        vm.StaffTranProModel = response.transproInformationModel !== null ? respone.transproInformationModel : "";
        vm.NarrationModel = response.narrationInformationModel !== null ? respone.narrationInformationModel : "";

        $state.go('StaffRegistration.details');
        var eduEl = angular.element(document.querySelector('#dtLnk'));
        eduEl.addClass('active');
        isDisabledOther(false);
        localStorage.add('isDisableFlag', false);
        return;
    }

    function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    function ClearLocalStorage() {
        localStorage.remove("currentForm");
        localStorage.remove('staffRegForm');
        localStorage.remove('EducationalInfoForm');
        localStorage.remove('staffstaffJobHistoryForm');
        localStorage.remove('skillCertificateForm');
        localStorage.remove('TRExperienceForm');
        localStorage.remove('staffBankPaymentForm');
        localStorage.remove('transProForm');
        localStorage.remove('NarrationForm');
        //localStorage.remove('isDisableFlag');
    }

    if (localStorage.get('staffRegForm') !== null) {
        if ($scope.$$nextSibling == null) {
            //window.localStorage.clear();
            ClearLocalStorage();
            $state.go('StaffRegistration.details');
            var eduEl = angular.element(document.querySelector('#dtLnk'));
            eduEl.addClass('active');
            isDisabledOther(true);
            currentForm = "staffRegForm";
        } else if (localStorage.get('staffRegForm').$pristine) {
            vm.staffRegForm = localStorage.get('staffRegForm');
            isDisabledOther(false);
            localStorage.add('isDisableFlag', false);
            currentForm = localStorage.get('currentForm');
        }
        else {
            isDisabledOther(true);
            localStorage.add('isDisableFlag', true);
            currentForm = "staffRegForm";
        }
    }
    else {
        isDisabledOther(true);
        localStorage.add('isDisableFlag', true);
        currentForm = "staffRegForm";
    }

    vm.monthNames = [{ Name: "Jan" },
                     { Name: "Feb" },
                     { Name: "Mar" },
                     { Name: "Apr" },
                     { Name: "May" },
                     { Name: "Jun" },
                     { Name: "Jul" },
                     { Name: "Aug" },
                     { Name: "Sep" },
                     { Name: "Oct" },
                     { Name: "Nov" },
                     { Name: "Dec" }];

    vm.skilLevels1 = [{ key: 1, value: '1', Name: 'L1' }, { key: 2, value: '2', Name: 'L2' }];
    vm.skilLevels2 = [{ key: 1, value: '1', Name: 'L1' }, { key: 2, value: '2', Name: 'L2' }];
    vm.skilLevels3 = [{ key: 1, value: '1', Name: 'L1' }, { key: 2, value: '2', Name: 'L2' }];
    vm.skilLevels4 = [{ key: 1, value: '1', Name: 'L1' }, { key: 2, value: '2', Name: 'L2' }];

    var initialDataPattern = {
        "ID": guid,
        "StaffEmailID": "",
        "RegistrationID": null,
        "FirstName": "",
        "NickName": "",
        "MiddleName": "",
        "Surname": "",
        "Natinality": "",
        "BirthDate": "",
        "Image": ""
    }

    function getYears() {
        var startYear = new Date().getFullYear() - 50;
        var finishYear = startYear + 100;
        var year = [];
        for (var i = startYear; i < finishYear; i++) {
            year.push({ Value: i });
        }
        console.log(new Date());
        return year;
    };

    function addSkill() {

    }

    function AddEducation() {

        vm.EducationalInfoModel.StaffID = vm.data.ID,
        vm.EducationalInfoModel.Degree = vm.staffDegreeData.selected.Name_en;
        vm.EducationalInfoModel.Issuedcountry = vm.countryData.selected.Name_en;
        vm.EducationalInfoModel.Subject = vm.staffSubjectData.selected.Name_en;
        vm.EducationalInfoModel.DegreeID = vm.staffDegreeData.selected.ID;
        vm.EducationalInfoModel.MajorSubjectID = vm.staffSubjectData.selected.ID;
        vm.EducationalInfoModel.CountryID = vm.countryData.selected.ID;

        vm.gridOptions.data.push(vm.EducationalInfoModel);
    }

    function SaveEducation() {

    }

    vm.getTableHeight = function () {
        var rowHeight = 30; // your row height
        var headerHeight = 30; // your header height
        return {
            height: (vm.gridOptions.data.length * rowHeight + headerHeight) + "px"
        };
    };

    vm.gridOptions = {
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
                field: 'Degree',
                displayName: 'Degree',
                width: '12%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'Status',
                displayName: 'Status',
                width: '12%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'InstituteName_en',
                displayName: 'Name of Institute',
                width: '15%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'Issuedcountry',
                displayName: 'Issued country',
                width: '12%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'EntryYear',
                displayName: 'Entry Date',
                width: '12%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'GraduationYear',
                displayName: 'Graduate date',
                width: '12%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'Majorfield',
                displayName: 'Major field',
                width: '12%',
                headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            },
            {
                field: 'Subject',
                displayName: 'Subject',
                width: '12%', headerCellClass: 'backgroundCell',
                cellClass: 'backgroundCell'
            }
        ]
    };

    //vm.data = initialDataPattern;

    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);

    angular.element(document.querySelector('#photoInput')).on('change', PhotoSelect);

    var PhotoSelect = function (evt) {
        var file = evt.currentTarget.files[0];
        var reader = new FileReader();

        reader.onload = function (evt) {
            $scope.$apply(function ($scope) {
                $scope.vm.data.Image = reader.result;
            })
        };

        reader.readAsDataURL(file);
    };

    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    }

    function isDisabledOther(isDisable) {

        vm.isDisableEducationalInfo = isDisable;
        vm.isDisableJobHistory = isDisable;
        vm.isDisabledSkill = isDisable;
        vm.isDisabledTR = isDisable;
        vm.isDisabledBankPayment = isDisable;
        vm.isDisabledBasicHRInfo = isDisable;
        vm.isDisabledHRDetails = isDisable;
        vm.isDisabledTRPro = isDisable;
        vm.isDisabledNarration = isDisable;

        return isDisable;
    }

    vm.$onInit = function () {
        vm.staffLanguagesFL1 = {};
        vm.countries = vm.countryData;
        //vm.skilLevels = vm.skilLevels;
        vm.banks = vm.bankData;
        vm.accountsType = vm.bankAccountTypeData;
        vm.visaTypes = vm.staffVisaTypeData;
        vm.businessCategories = vm.staffBusinessCategoryData;
        vm.businessSubCategories = vm.staffBusinessCategoryDetailData;
        vm.jobCategories = vm.staffJobCategoryData;
        vm.jobSubCategories = vm.staffJobSubCategoryData;
        vm.employmentTypes = vm.staffEmploymentTypeData;
        vm.staffCurrentStates = vm.staffStateData;
        vm.bankBranches = vm.staffBankBranchData;
        vm.educationDegree = vm.staffEducationDegreeData;
        //vm.techSkillItem = vm.staffTechSkillItemData;
        //vm.staffTranslation = vm.staffTranslation;
        //vm.staffProfession = vm.staffProfession;
        langData1 = vm.staffTechSkillItemData;
    };

    function resetForm() {
        $scope.$broadcast('hide-errors-event');
    }

    function removeClass() {
        var lis = document.getElementById("navbar").getElementsByTagName("li");
        angular.forEach(lis, function (value, key) {
            value.setAttribute("class", "");
        });
    }

    function changeStatus(event) {

        //if (localStorage.get('isDisableFlag')) {
        //    return;
        //}

        //var formEl = angular.element(document.querySelector('#staffRegForm'));

        //formEl.removeClass("active");

        if (localStorage.get('isDisableFlag')) {
            $state.go('StaffRegistration.details');
            var eduEl = angular.element(document.querySelector('#dtLnk'));
            eduEl.addClass('active');
            isDisabledOther(true);
            return;
        }

        removeClass();

        var currentId = event.target.id;

        if (currentId === 'dtLnk' || currentId === 'dtAnc') {

            $state.go('StaffRegistration.details');
            var eduEl = angular.element(document.querySelector('#dtLnk'));
            //eduEl.removeClass('active');
            eduEl.addClass('active');
            currentForm = FormObject("jobRegForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'eduLnk' || currentId === 'eduAnc') {

            $state.go('StaffRegistration.educationalInfo');
            var eduEl = angular.element(document.querySelector('#eduLnk'));
            //eduEl.removeClass('active');
            eduEl.addClass('active');
            currentForm = FormObject("EducationalInfoForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'hstLnk' || currentId === 'hstAnc') {
            $state.go('StaffRegistration.jobHistory');
            var hstEl = angular.element(document.querySelector('#hstLnk'));
            hstEl.addClass('active');
            currentForm = FormObject("staffstaffJobHistoryForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'sklLnk' || currentId === 'sklAnc') {
            $state.go('StaffRegistration.skillCertificate');
            var sklEl = angular.element(document.querySelector('#sklLnk'));
            sklEl.addClass('active');
            currentForm = FormObject("skillCertificateForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'trLnk' || currentId === 'trAnc') {
            $state.go('StaffRegistration.trExperience');
            var trEl = angular.element(document.querySelector('#trLnk'));
            trEl.addClass('active');
            currentForm = FormObject("TRExperienceForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'bnkLnk' || currentId === 'bnkAnc') {
            $state.go('StaffRegistration.bankPayment');
            var bnkEl = angular.element(document.querySelector('#bnkLnk'));
            bnkEl.addClass('active');
            currentForm = FormObject("staffBankPaymentForm");
            localStorage.add("currentForm", currentForm);
            return;
        }


        if (currentId === 'hrBasicLnk' || currentId === 'hrBasicAnc') {
            $state.go('StaffRegistration.basicHRInfo');
            var hrEl = angular.element(document.querySelector('#hrBasicLnk'));
            hrEl.addClass('active');
            currentForm = FormObject("hRBasicInfoForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'hrDetailLnk' || currentId === 'hrDetailAnc') {
            $state.go('StaffRegistration.hrDetail');
            var hrEl = angular.element(document.querySelector('#hrDetailLnk'));
            hrEl.addClass('active');
            currentForm = FormObject("hRDetailForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'trProLnk' || currentId === 'trProAnc') {
            $state.go('StaffRegistration.transPro');
            var trProEl = angular.element(document.querySelector('#trProLnk'));
            trProEl.addClass('active');
            currentForm = FormObject("transProForm");
            localStorage.add("currentForm", currentForm);
            return;
        }

        if (currentId === 'nrrLnk' || currentId === 'nrrAnc') {
            $state.go('StaffRegistration.narration');
            var nrrEl = angular.element(document.querySelector('#nrrLnk'));
            nrrEl.addClass('active');
            currentForm = FormObject("NarrationForm");
            localStorage.add("currentForm", currentForm);
            return;
        }
    }

    vm.getStaffType = function () {
        vm.data.StaffTypeID = Number(vm.data.StaffTypeID);
    }

    vm.VoiceTypeChanged = function () {
        vm.NarrationModel.VoiceType = Number(vm.VoiceType);
    }

    vm.AgeImpressionChanged = function () {
        vm.NarrationModel.AgeImpression = Number(vm.AgeImpression);
    }

    vm.SceneOrPurposesChanged = function () {
        vm.NarrationModel.SceneOrPurposes = Number(vm.SceneOrPurposes);
    }

    function today() {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd
        }

        if (mm < 10) {
            mm = '0' + mm
        }
        return mm + '-' + dd + '-' + yyyy;
    }

    function saveStaff() {
        $scope.$broadcast('show-errors-event');

        if (localStorage.get('isDisableFlag') && $scope.staffRegForm.$invalid) {
            return;
        }

        switch (currentForm) {
            case "staffRegForm":
                if (!$scope.staffRegForm.$invalid) {
                    if (vm.data !== '') {
                        vm.data.CurrentUserID = vm.baseModel.CurrentUserID;
                        vm.data.CurrentCulture = vm.baseModel.CurrentCulture;
                        vm.data.ApplicationId = vm.baseModel.ApplicationId;
                        //vm.data.ID = guid();
                        if (vm.data.Address !== '' && currentCulture === 'en') {
                            vm.data.Address_en = vm.data.Address;
                            vm.data.TownName_en = vm.staffStateData.selected.Name;
                            vm.data.District_en = vm.staffStateData.selected.Name;
                            vm.data.LastName_en = vm.data.LastName;
                            vm.data.FirstName_en = vm.data.FirstName;
                            vm.data.NickName_en = vm.data.NickName;
                            vm.data.MiddleName_en = vm.data.MiddleName;
                        }

                        vm.data.TranslationExpID = guid();
                        vm.data.IsJapanese = true;
                        vm.data.Password = "test";
                        vm.data.RegistrationDate = today();
                        vm.data.RegisteredFrom = today();
                        vm.data.VisaTypeID = vm.staffVisaTypeData.selected.ID;
                        vm.data.CountryOfCitizenship = vm.countryData.selected.ID;
                        vm.data.LivingCountryID = vm.countryData.selected.ID;
                        vm.data.NativeLanguageID = vm.staffLanguageData.selected.ID;
                        vm.data.ForiegnLanguage1ID = vm.staffLanguagefl1.selected.ID;
                        vm.data.ForiegnLanguage2ID = vm.staffLanguagefl2.selected.ID;
                        vm.data.ForiegnLanguage3ID = vm.staffLanguagefl3.selected.ID;
                        vm.data.ForiegnLanguage4ID = vm.staffLanguagefl3.selected.ID;
                        vm.data.ForeignLang1Level = vm.skilLevels1.selected.value;
                        vm.data.ForeignLang2Level = vm.skilLevels2.selected.value;
                        vm.data.ForeignLang3Level = vm.skilLevels3.selected.value;
                        vm.data.ForeignLang4Level = vm.skilLevels4.selected.value;
                        vm.data.ForeignLang1Level = vm.skilLevels4.selected.value;
                        vm.data.SelfVideoURL = angular.element($('#file'))[0].value;
                        vm.data.BankBranchName = vm.staffBankBranchData.selected.ID;
                        vm.data.ankAccountType = vm.bankAccountTypeData.selected.ID;

                        StaffRegistrationService.saveStaff(vm.data)
                            .catch(alerting.errorHandler())
                            .then(function (response) {

                                $scope.staffRegForm.$pristine = true;
                                //vm.isDisabledStaff = true;
                                //vm.isDisabledJobDetail = true;
                                // vm.isDisableEducationalInfo = false;
                                isDisabledOther(false);

                                localStorage.add('staffRegForm', $scope.staffRegForm);
                                localStorage.add('isDisableFlag', false);

                                //vm.isDisableJobHistory = false;
                                //$state.go('StaffRegistration.educationalInfo');
                                //var eduEl = angular.element(document.querySelector('#eduLnk'));
                                //eduEl.addClass('active');
                                removeClass();

                                $state.go('StaffRegistration.educationalInfo');
                                var eduEl = angular.element(document.querySelector('#eduLnk'));
                                eduEl.addClass('active');
                                currentForm = FormObject("EducationalInfoForm");
                                localStorage.add("currentForm", currentForm);
                                alerting.addSuccess($filter('translator')('DATASAVED'));

                                //toastr.success($filter('translator')('DATASAVED'));

                                //console.log("response ::" + response);
                            }, function error(error) {
                                console.log("error ::" + error);
                            });
                    }
                }
                break;
            case "EducationalInfoForm":
                if (!$scope.$$childTail.EducationalInfoForm.$invalid) {
                    if (vm.EducationalInfoModel !== '') {


                        var model = {
                            'EducationalInformation': vm.gridOptions.data,
                            'CurrentUserID': vm.baseModel.CurrentUserID,
                            'ApplicationID': vm.baseModel.ApplicationId,
                            'Culture': vm.baseModel.currentCulture
                        };

                        StaffRegistrationService.saveStaffEducation(model)
                                        .catch(alerting.errorHandler())
                                        .then(function (response) {

                                            localStorage.add('EducationalInfoForm', $scope.EducationalInfoForm);
                                            removeClass();

                                            $state.go('StaffRegistration.jobHistory');
                                            var eduEl = angular.element(document.querySelector('#hstLnk'));
                                            eduEl.addClass('active');
                                            currentForm = FormObject("staffstaffJobHistoryForm");
                                            localStorage.add("currentForm", currentForm);
                                            alerting.addSuccess($filter('translator')('DATASAVED'));

                                            //toastr.success($filter('translator')('DATASAVED'));

                                            //console.log("response ::" + response);
                                        }, function error(error) {
                                            console.log("error ::" + error);

                                            //alerting.errorHandler(error);

                                            //toastr.error($filter('translator')('ERRORDBOPERATION'));
                                        });
                    }
                }
                break;
            case "staffstaffJobHistoryForm":
                if (!$scope.$$childTail.staffstaffJobHistoryForm.$invalid) {
                    vm.jobHistoryData.EmploymentTypeID = vm.employmentTypes.selected.ID;
                    vm.jobHistoryData.CompanyName_en = vm.CompanyName;
                    vm.jobHistoryData.BusinessTypeID = vm.businessCategories.selected.ID;
                    vm.jobHistoryData.OfficeLocation_en = vm.Location;
                    vm.jobHistoryData.JobType1ID = vm.jobCategories.selected.ID;
                    vm.jobHistoryData.JoinDate = vm.JoinDate;
                    vm.jobHistoryData.ResignDate = vm.ResignDate;
                    vm.jobHistoryData.CompanyPrivacyName_en = vm.CompanyNamePrivate;
                    vm.jobHistoryData.BusinessTypeItemID = vm.businessSubCategories.selected.ID;
                    vm.jobHistoryData.Position_en = vm.Position;
                    vm.jobHistoryData.JobType2ItemID = vm.jobSubCategories.selected.ID;
                    vm.jobHistoryData.StaffID = vm.data.ID;
                    vm.jobHistoryData.JobType1ItemID = vm.jobSubCategories.selected.ID;
                    vm.jobHistoryData.JobType2ID = vm.jobCategories.selected.ID;

                    StaffRegistrationService.saveJobHistory(vm.jobHistoryData).then(function (response) {
                        localStorage.add('staffstaffJobHistoryForm', $scope.staffstaffJobHistoryForm);
                        removeClass();

                        $state.go('StaffRegistration.skillCertificate');
                        var sklEl = angular.element(document.querySelector('#sklLnk'));
                        sklEl.addClass('active');
                        currentForm = FormObject("skillCertificateForm");
                        localStorage.add("currentForm", currentForm);
                        toastr.success($filter('translator')('DATASAVED'));

                    }, function (error) {
                        toastr.error($filter('translator')('ERRORDBOPERATION'));
                    })
                }
                break;
            case "skillCertificateForm":
                if (!$scope.$$childTail.skillCertificateForm.$invalid) {
                    if (vm.SkillCertificateModel !== '') {

                        vm.SkillCertificateModel.StaffID = vm.data.ID;
                        vm.SkillCertificateModel.MainLanguageID = vm.staffLanguageDataSkill.selected.ID;
                        vm.SkillCertificateModel.FoeignLanguageID1 = vm.staffLanguagefl1Skill.selected.ID;
                        vm.SkillCertificateModel.FoeignLanguageID2 = vm.staffLanguagefl2Skill.selected.ID;
                        vm.SkillCertificateModel.FoeignLanguageID3 = vm.staffLanguagefl3Skill.selected.ID;
                        vm.SkillCertificateModel.ForeignLanguageLevelID1 = vm.skilLevels1Sk.selected.value;
                        vm.SkillCertificateModel.ForeignLanguageLevelID2 = vm.skilLevels2Sk.selected.value;
                        vm.SkillCertificateModel.ForeignLanguageLevelID3 = vm.skilLevels3Sk.selected.value;
                        vm.SkillCertificateModel.ForeignLanguageLevelID4 = vm.skilLevels4Sk.selected.value;
                        vm.SkillCertificateModel.SpecialField1 = vm.staffTranslation1.selected.ID;
                        vm.SkillCertificateModel.SpecialField2 = vm.staffTranslation2.selected.ID;
                        vm.SkillCertificateModel.SpecialField3 = vm.staffTranslation3.selected.ID;
                        vm.SkillCertificateModel.SpecialField4 = vm.staffTranslation4.selected.ID;
                        vm.SkillCertificateModel.SpecialField5 = vm.staffTranslation5.selected.ID;
                        vm.SkillCertificateModel.Project1StartDate = "01" + "-" + vm.startingmonth.selected.Name + "-" + vm.startingyear.selected.Value;
                        vm.SkillCertificateModel.Project1EndDate = "30" + "-" + vm.finishingmonth.selected.Name + "-" + vm.finishingyear.selected.Value;
                        vm.SkillCertificateModel.Project2StartDate = "01" + "-" + vm.startingmonth2.selected.Name + "-" + vm.startingyear2.selected.Value;
                        vm.SkillCertificateModel.Project2EndDate = "30" + "-" + vm.finishingmonth2.selected.Name + "-" + vm.finishingyear2.selected.Value;
                        vm.SkillCertificateModel.TOEICScore = parseInt(vm.SkillCertificateModel.TOEICScore);
                        vm.SkillCertificateModel.TOFELScore = parseInt(vm.SkillCertificateModel.TOFELScore);
                        vm.SkillCertificateModel.Project1TeamSize = parseInt(vm.SkillCertificateModel.Project1TeamSize);
                        vm.SkillCertificateModel.Project1Budget = parseInt(vm.SkillCertificateModel.Project1Budget);
                        vm.SkillCertificateModel.Project2TeamSize = parseInt(vm.SkillCertificateModel.Project2TeamSize);
                        vm.SkillCertificateModel.Project2Budget = parseInt(vm.SkillCertificateModel.Project2Budget);


                        var model = {
                            'SkillCertificateModel': vm.SkillCertificateModel,
                            'TechnicalCertificateModel': vm.TechnicalCertificateModel,
                            'CurrentUserID': vm.baseModel.CurrentUserID,
                            'ApplicationID': vm.baseModel.ApplicationId,
                            'Culture': vm.baseModel.currentCulture
                        };

                        StaffRegistrationService.saveSkillTech(model)
                                        .catch(alerting.errorHandler())
                                        .then(function (response) {
                                            //$scope.skillCertificateForm.$pristine = true;
                                            localStorage.add('skillCertificateForm', $scope.skillCertificateForm);
                                            removeClass();

                                            $state.go('StaffRegistration.trExperience');
                                            var trEl = angular.element(document.querySelector('#trLnk'));
                                            trEl.addClass('active');
                                            currentForm = FormObject("TRExperienceForm");
                                            localStorage.add("currentForm", currentForm);
                                            alerting.addSuccess($filter('translator')('DATASAVED'));

                                            //toastr.success($filter('translator')('DATASAVED'));

                                            //console.log("response ::" + response);
                                        }, function error(error) {
                                            console.log("error ::" + error);

                                            //alerting.errorHandler(error);

                                            //toastr.error($filter('translator')('ERRORDBOPERATION'));
                                        });

                    }
                }
                break;
            case "TRExperienceForm":
                if (!$scope.$$childTail.TRExperienceForm.$invalid) {
                    if (vm.TRExperienceModel !== '') {
                        vm.TRExperienceModel.StaffID = vm.data.ID;
                        vm.TRExperienceModel.TransPriceNote_en = currentCulture = 'en' ? vm.TRExperienceModel.TransPriceNote : "";
                        vm.TRExperienceModel.InterpPriceNote_en = currentCulture = 'en' ? vm.TRExperienceModel.InterpPriceNote : "";
                        vm.TRExperienceModel.CoordinatorSalesNote_en = currentCulture = 'en' ? vm.TRExperienceModel.CoordinatorSalesNote : "";
                        vm.TRExperienceModel.TransSpecialFieldID1 = vm.staffSpecialField1.selected.ID;
                        vm.TRExperienceModel.TransSpecialFieldID1 = vm.staffSpecialField2.selected.ID;
                        vm.TRExperienceModel.TransSpecialFieldID1 = vm.staffSpecialField3.selected.ID;
                        vm.TRExperienceModel.HasResearchExperience = vm.TRExperienceModel.HasResearchExperience.selected;
                        vm.TRExperienceModel.HasMSOfficeExperience = vm.TRExperienceModel.HasMSOfficeExperience.selected;
                        vm.TRExperienceModel.HasMacOSExperience = vm.TRExperienceModel.HasMacOSExperience.selected;
                        vm.TRExperienceModel.HasAdobeExperience = vm.TRExperienceModel.HasAdobeExperience.selected;

                        StaffRegistrationService.saveTRExperience(vm.TRExperienceModel)
                                        .catch(alerting.errorHandler())
                                        .then(function (response) {
                                            //$scope.skillCertificateForm.$pristine = true;
                                            localStorage.add('TRExperienceForm', $scope.TRExperienceForm);
                                            removeClass();

                                            $state.go('StaffRegistration.bankPayment');
                                            var eduEl = angular.element(document.querySelector('#bnkLnk'));
                                            eduEl.addClass('active');
                                            currentForm = FormObject("staffBankPaymentForm");
                                            localStorage.add("currentForm", currentForm);
                                            alerting.addSuccess($filter('translator')('DATASAVED'));

                                            //toastr.success($filter('translator')('DATASAVED'));

                                            //console.log("response ::" + response);
                                        }, function error(error) {
                                            console.log("error ::" + error);

                                            //alerting.errorHandler(error);

                                            //toastr.error($filter('translator')('ERRORDBOPERATION'));
                                        });
                    }
                }
                break;
            case "staffBankPaymentForm":
                if (!$scope.$$childTail.staffBankPaymentForm.$invalid) {
                    if (vm.StaffBankModel !== '') {
                        vm.StaffBankModel.StaffID = vm.data.ID;
                        vm.StaffBankModel.BankID = vm.banks.selected.Id;
                        vm.StaffBankModel.AccountTypeID = vm.bankAccountsType.selected.ID;
                        vm.StaffBankModel.BankBranchID = vm.bankBranches.selected.ID;
                        vm.StaffBankModel.AccountHolderName = currentCulture = 'en' ? vm.StaffBankModel.AccountHolderName : "";
                        vm.StaffBankModel.AccountHolderAddress = currentCulture = 'en' ? vm.StaffBankModel.AccountHolderAddress : "";
                        vm.StaffBankModel.AccountNote = currentCulture = 'en' ? vm.StaffBankModel.AccountNote : "";

                        StaffRegistrationService.saveBankInfo(vm.StaffBankModel)
                                            .catch(alerting.errorHandler())
                                            .then(function (response) {
                                                //$scope.skillCertificateForm.$pristine = true;
                                                localStorage.add('staffBankPaymentForm', $scope.staffBankPaymentForm);
                                                removeClass();

                                                $state.go('StaffRegistration.basicHRInfo');
                                                var eduEl = angular.element(document.querySelector('#hrBasicLnk'));
                                                eduEl.addClass('active');
                                                currentForm = FormObject("hRBasicInfoForm");
                                                localStorage.add("currentForm", currentForm);
                                                alerting.addSuccess($filter('translator')('DATASAVED'));

                                                //toastr.success($filter('translator')('DATASAVED'));

                                                //console.log("response ::" + response);
                                            }, function error(error) {
                                                console.log("error ::" + error);

                                                //alerting.errorHandler(error);

                                                //toastr.error($filter('translator')('ERRORDBOPERATION'));
                                            });

                    }

                }
                break;
            case "hRBasicInfoForm":

                break;
            case "hRDetailForm":

                break;
            case "transProForm":
                if (!$scope.$$childTail.transProForm.$invalid) {
                    if (vm.StaffTranProModel !== '') {
                        vm.StaffTranProModel.StaffID = vm.data.ID;

                        StaffRegistrationService.saveTransPro(vm.StaffTranProModel)
                                                .catch(alerting.errorHandler())
                                                .then(function (response) {
                                                    //$scope.skillCertificateForm.$pristine = true;
                                                    localStorage.add('transProForm', $scope.transProForm);
                                                    removeClass();

                                                    $state.go('StaffRegistration.narration');
                                                    var eduEl = angular.element(document.querySelector('#nrrLnk'));
                                                    eduEl.addClass('active');
                                                    currentForm = FormObject("NarrationForm");
                                                    localStorage.add("currentForm", currentForm);
                                                    alerting.addSuccess($filter('translator')('DATASAVED'));

                                                    //toastr.success($filter('translator')('DATASAVED'));

                                                    //console.log("response ::" + response);
                                                }, function error(error) {
                                                    console.log("error ::" + error);

                                                    //alerting.errorHandler(error);

                                                    //toastr.error($filter('translator')('ERRORDBOPERATION'));
                                                });


                    }
                }
                break;
            case "NarrationForm":
                if (!$scope.$$childTail.NarrationForm.$invalid) {
                    if (vm.NarrationModel != "") {
                        vm.NarrationModel.StaffID = vm.data.ID;
                        vm.NarrationModel.ProfessionID = vm.staffProfession.selected.ID;

                        vm.NarrationVoiceModel.StaffID = vm.data.ID;
                        vm.NarrationVoiceModel.NarrationTypeID = currentCulture == "en" ? vm.narrationType.selected.ID : "";

                        var model = {
                            'NarrationInformationModel': vm.NarrationModel,
                            'NarrationVoiceFilesModel': vm.NarrationVoiceModel
                        };

                        StaffRegistrationService.saveNarration(model)
                                                    .catch(alerting.errorHandler())
                                                    .then(function (response) {
                                                        //$scope.skillCertificateForm.$pristine = true;
                                                        localStorage.add('NarrationForm', $scope.NarrationForm);
                                                        removeClass();
                                                        localStorage.add("currentForm", NarrationForm);
                                                        alerting.addSuccess($filter('translator')('DATASAVED'));

                                                        //toastr.success($filter('translator')('DATASAVED'));

                                                        //console.log("response ::" + response);
                                                    }, function error(error) {
                                                        console.log("error ::" + error);

                                                        //alerting.errorHandler(error);

                                                        //toastr.error($filter('translator')('ERRORDBOPERATION'));
                                                    });
                    }
                }
                break;
        }
    }
}

