/**
 * Created by Mahfuz on 07/24/2017.
 */
angular.module("hiworkApp").factory('StaffRegistrationService', ['$q', '$timeout', '$http', 'ajaxService', 'appSettings', 'AppStorage', 'sessionFactory', 'exceptionHandler', function ($q, $timeout, $http, ajaxService, appSettings, AppStorage, sessionFactory, exceptionHandler) {

    var dataObject = {};
    var requestConfig = {
        headers: {
            'X-ZUMO-APPLICATION': 'GSECUHNQOOrCwgRHFFYLXWiViGnXNV88'
        }
    };

    var service = {
        getCountry: getCountry,
        getLanguageSkillLevel: getLanguageSkillLevel,
        getBank: getBank,
        getBankAccountType: getBankAccountType,
        getStaffVisaType: getStaffVisaType,
        getStaffBusinessCategory: getStaffBusinessCategory,
        getStaffBusinessCategoryDetail: getStaffBusinessCategoryDetail,
        getStaffJobCategory: getStaffJobCategory,
        getStaffJobCategoryDetail: getStaffJobCategoryDetail,
        getStaffEmploymentType: getStaffEmploymentType,
        saveStaff: saveStaff,
        getLanguage: getLanguage,
        getCurrentState: getCurrentState,
        getBankBranches: getBankBranches,
        saveJobHistory: saveJobHistory,
        saveBankInfo: saveBankInfo,
        getEducationDegree: getEducationDegree,
        getStaffTechSkillItem: getStaffTechSkillItem,
        getStaffTranslation: getStaffTranslation,
        getStaffSpecialField: getStaffSpecialField,
        getStaffProfession: getStaffProfession,
        getLanguagefl1: getLanguagefl1,
        getEducationaldegree: getEducationaldegree,
        getMejorSubject: getMejorSubject,
        saveStaffEducation: saveStaffEducation,
        saveSkillTech: saveSkillTech,
        saveTRExperience: saveTRExperience,
        saveTransPro: saveTransPro,
        getStaffNarrationType: getStaffNarrationType,
        saveNarration: saveNarration
    };

    dataObject.currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    dataObject.currentUser = sessionFactory.GetObject(AppStorage.userData);

    return service;

    function getMejorSubject() {
        var url = "mejorSubject/list";
        return httpPost(url, dataObject);
    }

    function getEducationaldegree() {
        var url = "educationalDegree/list";
        return httpPost(url, dataObject);
    }

    function getStaffProfession() {
        var url = "profession/list";
        return httpPost(url, dataObject);
    }

    function getStaffSpecialField() {
        var url = "staffspecialfield/list";
        return httpPost(url, dataObject);
    }

    function getStaffTranslation() {
        var url = "stafftranslationfields/list";
        return httpPost(url, dataObject);
    }

    function getStaffTechSkillItem() {
        var url = "tsi/list";
        return httpPost(url, dataObject);
    }

    function getEducationDegree() {
        var url = "education/list";
        return httpPost(url, dataObject);
    }

    function saveStaff(dataModel) {
        var url = "staff/save";

        dataModel.CurrentUserID = dataObject.currentUser.CurrentUserID;
        dataModel.currentCulture = dataObject.currentCulture;

        return httpPost(url, dataModel);
    }

    function getStaffEmploymentType() {
        var url = "employmentType/list";
        return httpPost(url, dataObject);
    }

    function getStaffJobCategory() {
        var url = "jobcategory/list";

        return httpPost(url, dataObject);
    }

    function getStaffJobCategoryDetail() {
        var url = "jobcategorydetails/list";

        return httpPost(url, dataObject);
    }

    function getStaffBusinessCategoryDetail() {
        var url = "businesscategorydetails/list";
        return httpPost(url, dataObject);
    }

    function getStaffBusinessCategory() {
        var url = "businesscategory/list";
        return httpPost(url, dataObject);
    }

    function getCountry(data) {
        var url = "country/list";
        return httpPost(url, dataObject);
    };

    function getLanguageSkillLevel(data) {
        var url = "langskill/list";
        return httpPost(url, dataObject);

    };

    function getBank(data) {
        var url = "bank/list";
        return httpPost(url, dataObject);
    };

    function getBankAccountType(data) {
        var url = "bankAccountType/list";
        return httpPost(url, dataObject);
    };

    function getStaffVisaType(data) {
        var url = "visa/list";
        return httpPost(url, dataObject);
    };

    function getLanguagefl1(data) {
        var url = "language/list";
        return httpPost(url, dataObject);

    };

    function getLanguage(data) {
        var url = "language/list";
        return httpPost(url, dataObject);
    };

    function getCurrentState(data) {
        var url = "currentstate/list";
        return httpPost(url, dataObject);
    };

    function getStaffNarrationType(data) {
        var url = "staffnarrationtype/list";
        return httpPost(url, dataObject);
    };

    function getBankBranches(data) {
        var url = "bankbranch/list";
        return httpPost(url, dataObject);
    };

    function saveNarration(dataModel) {
        var url = "staffNarration/save";
        return httpPost(url, dataModel);
    }

    function saveTransPro(dataModel) {
        var url = "staffTransPro/save";
        return httpPost(url, dataModel);
    }

    function saveSkillTech(dataModel) {
        var url = "staffSkillTech/save";
        return httpPost(url, dataModel);
    }

    function saveStaffEducation(dataModel) {
        var url = "staffEducation/save";
        return httpPost(url, dataModel);
    }

    function saveJobHistory(dataModel) {
        var url = "jobhistory/save";

        dataModel.CurrentUserID = dataObject.currentUser.CurrentUserID;
        dataModel.currentCulture = dataObject.currentCulture;

        return httpPost(url, dataModel);
    }

    function saveBankInfo(dataModel) {
        var url = "staffBankAccInfo/save";

        dataModel.CurrentUserID = dataObject.currentUser.CurrentUserID;
        dataModel.currentCulture = dataObject.currentCulture;

        return httpPost(url, dataModel);
    }

    function saveTRExperience(dataModel) {
        var url = "staffTRExperience/save";
        dataModel.CurrentUserID = dataObject.currentUser.CurrentUserID;
        dataModel.currentCulture = dataObject.currentCulture;
        return httpPost(url, dataModel);
    }

    function httpExecute(requestUrl, method, data) {
        console.log("httpExecute::" + requestUrl);
        console.log("method::" + method);
        console.log("method::" + data);

        return $http({
            url: appSettings.API_BASE_URL + requestUrl,
            method: method,
            data: data
           // headers: requestConfig.headers
        }).then(function (response) {

            console.log('**response from EXECUTE', response);
            return response.data;
        }, function (error) {
            console.log('**response from error', error);
            //throw new Error(error);
            exceptionHandler.handleException(error);
        });
    }


    function httpDelete(url) {
        return httpExecute(url, 'DELETE');
    }

    function httpGet(url) {
        console.log("URL" + url);
        return httpExecute(url, 'GET');
    }

    function httpPatch(url, data) {
        return httpExecute(url, 'PATCH', data);
    }

    function httpPost(url, data) {
        return httpExecute(url, 'POST', data);
    }

}]);