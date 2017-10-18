angular.module("hiworkApp").component('advancedStaffSearch', {
    templateUrl: 'app/Components/AdvancedStaffSearch/Template/AdvancedStaffSearch.html',
    controllerAs: "vm",
    bindings: {
        routes: "=",
        services: "=",
        sourceofregistration: "=",
        language: "=",
        languagelevel: "=",
        age: "=",
        nationalitygroup: "=",
        nationality: "=",
        visatype: "=",
        visaexpire: "=",
        snsaccount: "=",
        dtp: "=",
        officetype: "=",
        webtype: "=",
        translationtools: "=",
        toolname: "=",
        design: "=",
        softwarename: "=",
        tin: "=",
        iin: "=",
        nin: "=",
        narrationperformance: "="
    },
    controller: advancedStaffSearchController
})
advancedStaffSearchController.$inject = ['$scope', 'appSettings', 'AppStorage', 'sessionFactory', 'ajaxService', '$state'];

function advancedStaffSearchController($scope, appSettings, AppStorage, sessionFactory, ajaxService, $state) {
    var vm = this;
    var BaseModel = {};
    var currentCulture = sessionFactory.GetData(AppStorage.currentLanguage);
    var currentUser = sessionFactory.GetObject(AppStorage.userData);
    BaseModel.CurrentUserID = currentUser.CurrentUserID;
    BaseModel.CurrentCulture = currentCulture;
    BaseModel.ApplicationId = appSettings.ApplicationId;

    vm.AdvancedStaffSearchModel = {};
    vm.AdvancedStaffSearchModel.PriorityList = [];
    vm.AdvancedStaffSearchModel.CertifiedExpert = [];
    vm.AdvancedStaffSearchModel.Exclusion = [];
    vm.AdvancedStaffSearchModel.PCSkill = [];


    vm.$onInit = function () {
        vm.AdvancedStaffSearchModel.SourceOfRegistrationID = null;
        vm.AdvancedStaffSearchModel.ForiegnLanguage1ID = null;
        vm.AdvancedStaffSearchModel.ForeignLang1Level = null;
        vm.AdvancedStaffSearchModel.ForiegnLanguage2ID = null;
        vm.AdvancedStaffSearchModel.ForeignLang2Level = null;
        vm.AdvancedStaffSearchModel.ForiegnLanguage3ID = null;
        vm.AdvancedStaffSearchModel.ForeignLang3Level = null;
        vm.AdvancedStaffSearchModel.ForiegnLanguage4ID = null;
        vm.AdvancedStaffSearchModel.ForeignLang4Level = null;
        vm.AdvancedStaffSearchModel.Sex = "all";
        vm.AdvancedStaffSearchModel.AgeFrom = null;
        vm.AdvancedStaffSearchModel.AgeTo = null;
        vm.AdvancedStaffSearchModel.NationalityID = null;
        vm.AdvancedStaffSearchModel.VisaCountryID = null;
        vm.AdvancedStaffSearchModel.VisaTypeID = null;
        vm.AdvancedStaffSearchModel.VisaExpire = null;
        vm.AdvancedStaffSearchModel.RdoResidence = "all";
        vm.AdvancedStaffSearchModel.ResidenceNationalityID = null;
        vm.AdvancedStaffSearchModel.SNSAccount = null;
        vm.AdvancedStaffSearchModel.DtpExp = null;
        vm.AdvancedStaffSearchModel.OfficeType = null;
        vm.AdvancedStaffSearchModel.WebType = null;
        vm.AdvancedStaffSearchModel.TranslationTools = null;
        vm.AdvancedStaffSearchModel.ToolName = null;
        vm.AdvancedStaffSearchModel.Design = null;
        vm.AdvancedStaffSearchModel.SoftwareName = null;
        vm.AdvancedStaffSearchModel.TranslationFrom = null;
        vm.AdvancedStaffSearchModel.TranslationTo = null;
        vm.AdvancedStaffSearchModel.InterpretationFrom = null;
        vm.AdvancedStaffSearchModel.InterpretationTo = null;
        vm.AdvancedStaffSearchModel.NarrationFrom = null;
        vm.AdvancedStaffSearchModel.NarrationTo = null;
        vm.AdvancedStaffSearchModel.NarrationPerformance = null;
    }

    vm.search = function () {
        vm.AdvancedStaffSearchModel.CurrentUserID = currentUser.CurrentUserID,
        vm.AdvancedStaffSearchModel.ApplicationID = appSettings.ApplicationId,
        vm.AdvancedStaffSearchModel.Culture = currentCulture        
        $state.go('AdvancedStaffSearchList', { "MODEL": vm.AdvancedStaffSearchModel });
    }
}