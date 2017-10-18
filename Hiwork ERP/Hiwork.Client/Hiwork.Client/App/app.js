(function () {
    'use strict'

    var hiworkApp = angular.module("hiworkApp",
        [
            "ui.router",
            "ngResource",
            "ngAnimate",
            "ngSanitize",
            "ngTouch",
            "ngCookies",
            "blockUI",
            "ui.bootstrap",
            "ui.calendar",
            "smart-table",
            "ui.select",
            "ngAnimate",
            "ui.grid",
            'ui.grid.resizeColumns',
            'ui.grid.moveColumns',
            'ui.grid.selection',
            'ui.grid.edit',
            'ui.grid.exporter',
            'ui.grid.pagination',
            'ui.grid.autoResize',
            'ngRoute',
            'ngStorage'
        ]);

    hiworkApp.config(function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/login');
        $stateProvider.state("login",
                {
                    url: "/login",
                    templateUrl: 'Views/UserManagement/Login.html',
                    controller: "loginController"
                })
            .state("home",
                {
                    url: "/home",
                    templateUrl: 'Views/home.html',
                    controller: "homeController"
                })
            .state("dashboard",
            {
                parent: "home",
                url: "/dashboard",
                template: '<dashboard currency-list="$resolve.currencyList"></dashboard>',
                resolve: {
                    currencyList: ["estimationService", function (estimationService) {
                        return estimationService.currencyList();
                    }]
                }
            })
            .state("hrdashboard",
            {
                parent: "home",
                url: "/hrdashboard",
                template: "<hrdashboard></hrdashboard>",
                resolve: {
                }
            })
            .state("accountsdashboard",
            {
                parent: "home",
                url: "/accountsdashboard",
                template: "<accountsdashboard></accountsdashboard>",
                resolve: {
                }
            })
            .state("translatordashboard",
            {
                parent: "home",
                url: "/translatordashboard",
                template: "<translatordashboard></translatordashboard>",
                resolve: {
                }
            })
            .state("SystemSettings",
                {
                    parent: "home",
                    url: "/SystemSettings",
                    template: "<application></application>",
                    resolve: {
                    }
                })
            .state("Country",
                {
                    parent: "home",
                    url: "/Country",
                    template: "<country></country>",
                    resolve: {

                    }
                })
            .state("UserInfo",
                {
                    parent: "home",
                    url: "/UserInfo",
                    template: '<user-Information data="$resolve.data" roles="$resolve.roles" user-Types="$resolve.userTypes"></user-Information>',
                    resolve: {
                        data: ['UserInfoService', function (UserInfoService) {
                            return UserInfoService.getUser();
                        }],
                        roles: ['UserInfoService', function (UserInfoService) {
                            return UserInfoService.getRoles();
                        }],
                        userTypes: ['UserInfoService', function (UserInfoService) {
                            return UserInfoService.getUserType();
                        }]
                    }
                })
                .state("AdvancedStaffSearch",
                {
                    parent: "home",
                    url: "/AdvancedStaffSearch",
                    params: { MODEL: null },
                    template: '<advanced-staff-search routes="$resolve.routes" services="$resolve.services" sourceofregistration="$resolve.sourceofregistration" language="$resolve.language" languagelevel="$resolve.languagelevel" age="$resolve.age" nationalitygroup="$resolve.nationalitygroup" nationality="$resolve.nationality" visatype="$resolve.visatype" visaexpire="$resolve.visaexpire" snsaccount="$resolve.snsaccount" dtp="$resolve.dtp" officetype="$resolve.officetype" webtype="$resolve.webtype" translationtools="$resolve.translationtools" toolname="$resolve.toolname" design="$resolve.design" softwarename="$resolve.softwarename" tin="$resolve.tin" iin="$resolve.iin" nin="$resolve.nin" narrationperformance="$resolve.narrationperformance"></advanced-staff-search>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        sourceofregistration: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.sourceofregistration();
                        }],
                        language: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.language();
                        }],
                        languagelevel: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.languagelevel();
                        }],
                        age: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.age();
                        }],
                        nationalitygroup: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.nationalitygroup();
                        }],
                        nationality: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.nationality();
                        }],
                        visatype: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.visatype();
                        }],
                        visaexpire: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.visaexpire();
                        }],
                        snsaccount: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.snsaccount();
                        }],
                        dtp: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.dtp();
                        }],
                        officetype: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.officetype();
                        }],
                        webtype: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.webtype();
                        }],
                        translationtools: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.translationtools();
                        }],
                        toolname: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.toolname();
                        }],
                        design: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.design();
                        }],
                        softwarename: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.softwarename();
                        }],
                        tin: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.tin();
                        }],
                        iin: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.iin();
                        }],
                        nin: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.nin();
                        }],
                        narrationperformance: ["advancedStaffSearchService", function (advancedStaffSearchService) {
                            return advancedStaffSearchService.narrationperformance();
                        }]
                    }
                })
                .state("StaffList",
                {
                    parent: "home",
                    url: "/StaffList",
                    template: "<staff-list></staff-list>",
                    resolve: {
                    }
                })
                .state("AdvancedStaffSearchList",
                    {
                        parent: "home",
                        url: "/AdvancedStaffSearchList",
                        params: { MODEL: null },
                        template: '<advanced-staff-search-list routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></advanced-staff-search-list>',
                        resolve: {
                            routes: ["estimationService", function (estimationService) {
                                return estimationService.estimationRoutes();
                            }],
                            services: ["estimationService", function (estimationService) {
                                return estimationService.estimationServices();
                            }],
                            languages: ["estimationService", function (estimationService) {
                                return estimationService.estimationLanguages();
                            }],
                            businessCategories: ["estimationService", function (estimationService) {
                                return estimationService.businessCategories();
                            }],
                            specializedFields: ["estimationService", function (estimationService) {
                                return estimationService.specializedFields();
                            }],
                            subSpecializedFields: ["estimationService", function (estimationService) {
                                return estimationService.subSpecializedFields();
                            }],
                            currencyList: ["estimationService", function (estimationService) {
                                return estimationService.currencyList();
                            }]
                        }
                    })
                .state("TranscriptionOrderDetails",
                {
                    parent: "home",
                    url: "/TranscriptionOrderDetails/:id",
                    params: { Estimation: null },
                    template: '<transcription-order routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></transcription-order>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
               .state("NarrationEstimation",
                {
                    parent: "home",
                    url: "/NarrationEstimation/:id",
                    params: { Estimation: null },
                    template: '<narration-Estimation routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></narration-Estimation>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }],
                        translationCertificateSettingsList: ["estimationService", function (estimationService) {
                            return estimationService.translationCertificateSettingsList();
                        }]
                    }
                })
             .state("NarrationOrderDetails",
                {
                    parent: "home",
                    url: "/NarrationOrderDetails/:id",
                    params: { Estimation: null },
                    template: '<narration-Order routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></narration-Order>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
                .state("DTPEstimation",
                {
                    parent: "home",
                    url: "/DTPEstimation/:id",
                    params: { Estimation: null },
                    template: '<dtp-estimation routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></dtp-estimation>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
                .state("TranscriptionEstimation",
                {
                    parent: "home",
                    url: "/TranscriptionEstimation/:id",
                    params: { Estimation: null },
                    template: '<transcription-estimation routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></transcription-estimation>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }],
                        translationCertificateSettingsList: ["estimationService", function (estimationService) {
                            return estimationService.translationCertificateSettingsList();
                        }]
                    }
                })
               .state("StaffRegistration",
                {
                    parent: "home",
                    url: "/StaffRegistration/:staffNo",
                    template: '<staff-Registration narration-Type="$resolve.narrationType" staff-Subject-Data="$resolve.staffSubjectData" staff-Degree-Data="$resolve.staffDegreeData" country-Data-Bank="$resolve.countryDataBank" staff-Languagefl3="$resolve.staffLanguagefl3"  staff-Languagefl2="$resolve.staffLanguagefl2"  staff-Languagefl1="$resolve.staffLanguagefl1" staff-Profession ="$resolve.staffProfession"  staff-Special-Field="$resolve.staffSpecialField" staff-Translation="$resolve.staffTranslation" staff-Tech-Skill-Item-Data="$resolve.staffTechSkillItemData" staff-Education-Degree-Data= "$resolve.staffEducationDegreeData" staff-Bank-Branch-Data= "$resolve.staffBankBranchData" staff-State-Data ="$resolve.staffStateData" staff-Language-Data="$resolve.staffLanguageData" staff-Employment-Type-Data="$resolve.staffEmploymentTypeData" staff-Job-Sub-Category-Data="$resolve.staffJobSubCategoryData"  staff-Job-Category-Data="$resolve.staffJobCategoryData"  staff-Business-Category-Detail-Data="$resolve.staffBusinessCategoryDetailData"  staff-Business-Category-Data="$resolve.staffBusinessCategoryData" staff-Visa-Type-Data="$resolve.staffVisaTypeData" bank-Data="$resolve.bankData" bank-Account-Type-Data="$resolve.bankAccountTypeData"  country-Data="$resolve.countryData" language-Skill-Level-Data="$resolve.languageSkillLevelData"></staff-Registration>',
                    resolve: {
                        countryData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getCountry();
                        }],
                        countryDataBank: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getCountry();
                        }],
                        bankData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getBank();
                        }],
                        bankAccountTypeData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getBankAccountType();
                        }],
                        staffVisaTypeData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffVisaType();
                        }],
                        staffBusinessCategoryData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffBusinessCategory();
                        }],
                        staffBusinessCategoryDetailData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffBusinessCategoryDetail();
                        }],
                        staffJobCategoryData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffJobCategory();
                        }],
                        staffJobSubCategoryData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffJobCategoryDetail();
                        }],
                        staffEmploymentTypeData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffEmploymentType();
                        }],
                        staffLanguageData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getLanguage();
                        }],
                        staffLanguagefl1: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getLanguagefl1();
                        }],
                        staffLanguagefl2: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getLanguagefl1();
                        }],
                        staffLanguagefl3: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getLanguagefl1();
                        }],
                        staffStateData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getCurrentState();
                        }],
                        staffBankBranchData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getBankBranches();
                        }],
                        staffEducationDegreeData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getEducationDegree();
                        }],
                        staffTechSkillItemData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffTechSkillItem();
                        }],
                        staffTranslation: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffTranslation();
                        }],
                        staffSpecialField: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffSpecialField();
                        }],
                        staffProfession: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffProfession();
                        }],
                        staffDegreeData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getEducationaldegree();
                        }],
                        staffSubjectData: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getMejorSubject();
                        }],
                        narrationType: ['StaffRegistrationService', function (StaffRegistrationService) {
                            return StaffRegistrationService.getStaffNarrationType();
                        }]
                        
                    }
                })
                .state("StaffRegistration.details", {
                    parent: "StaffRegistration",
                    url: "/jobDetails",
                    templateUrl: "App/Components/StaffRegistration/Templates/JobDetail.html"
                })
                .state("StaffRegistration.jobHistory", {
                    parent: "StaffRegistration",
                    url: "/jobHistory",
                    templateUrl: "App/Components/StaffRegistration/Templates/JobHistory.html"
                })
                .state("StaffRegistration.narration", {
                    parent: "StaffRegistration",
                    url: "/narration",
                    templateUrl: "App/Components/StaffRegistration/Templates/Narration.html"
                })
               .state("StaffRegistration.educationalInfo", {
                   parent: "StaffRegistration",
                   url: "/EducationalInfo",
                   templateUrl: "App/Components/StaffRegistration/Templates/EducationalInfo.html",
                   //activetab: 'educationalInfo'
               })
               .state("StaffRegistration.skillCertificate", {
                   parent: "StaffRegistration",
                   url: "/SkillAndCertificate",
                   templateUrl: "App/Components/StaffRegistration/Templates/SkillAndCertificate.html"
               })
               .state("StaffRegistration.trExperience", {
                   parent: "StaffRegistration",
                   url: "/TRExperience",
                   templateUrl: "App/Components/StaffRegistration/Templates/TRExperience.html"
               })
               .state("StaffRegistration.bankPayment", {
                   parent: "StaffRegistration",
                   url: "/BankPayment",
                   templateUrl: "App/Components/StaffRegistration/Templates/BankAndPayment.html"
               })
               .state("StaffRegistration.basicHRInfo", {
                   parent: "StaffRegistration",
                   url: "/BasicHRInfo",
                   templateUrl: "App/Components/StaffRegistration/Templates/BasicHRInfo.html"
               })
                .state("StaffRegistration.hrDetail", {
                    parent: "StaffRegistration",
                    url: "/HRDetail",
                    templateUrl: "App/Components/StaffRegistration/Templates/HRDetails.html"
                })
               .state("StaffRegistration.transPro", {
                   parent: "StaffRegistration",
                   url: "/TransPro",
                   templateUrl: "App/Components/StaffRegistration/Templates/TransPro.html"
               })
            .state("Role",
                {
                    parent: "home",
                    url: "/Role",
                    template: "<roles></roles>",
                    resolve:
                    {
                    }
                })
             .state("Unit",
                {
                    parent: "home",
                    url: "/Unit",
                    template: "<units></units>",
                    resolve:
                    {
                    }
                })
            .state("UnitPrice",
                {
                    parent: "home",
                    url: "/UnitPrice",
                    template: "<unitprices></unitprices>",
                    resolve:
                    {
                    }
                })
            .state("VisaType",
                {
                    parent: "home",
                    url: "/VisaType",
                    template: "<visatypes></visatypes>",
                    resolve: {
                    }
                })
            .state("JobType",
                {
                    parent: "home",
                    url: "/JobType",
                    template: "<jobtypes></jobtypes>",
                    resolve: {
                    }
                })
            .state("Division",
                {
                    parent: "home",
                    url: "/Division",
                    template: "<divisions></divisions>",
                    resolve: {

                    }
                })
           .state("Culture",
                {
                    parent: "home",
                    url: "/Culture",
                    template: "<culture></culture>",
                    resolve: {

                    }
                })
            .state("Team",
                {
                    parent: "home",
                    url: "/Team",
                    template: "<teams></teams>",
                    resolve: {
                    }
                })
            .state("TechnicalSkillCategory",
                {
                    parent: "home",
                    url: "/TechnicalSkillCategory",
                    template: "<techskillcategory></techskillcategory>",
                    resolve: {
                    }
                })
            .state("TechnicalSkillItems",
                {
                    parent: "home",
                    url: "/TechnicalSkillItems",
                    template: "<techskillitems></techskillitems>",
                    resolve: {
                    }
                })
            .state("StaffType",
                {
                    parent: "home",
                    url: "/StaffType",
                    template: "<stafftype></stafftype>",
                    resolve: {
                    }
                })
            .state("OtherExperiences",
                {
                    parent: "home",
                    url: "/OtherExperiences",
                    template: "<otherexperiences></otherexperiences>",
                    resolve: {
                    }
                })
            .state("StaffTranslationFields",
                {
                    parent: "home",
                    url: "/StaffTranslationFields",
                    template: "<stafftranslationfields></stafftranslationfields>",
                    resolve: {
                    }
                })
                .state("StaffTechnicalFields",
                {
                    parent: "home",
                    url: "/StaffTechnicalFields",
                    template: "<masterstfields></masterstfields>",
                    resolve: {
                    }
                })
            .state("StaffMajorSubject",
                {
                    parent: "home",
                    url: "/StaffMajorSubject",
                    template: "<staffmajorsubjects></staffmajorsubjects>",
                    resolve: {
                    }
                })
             .state("StaffMajorSubjectDetails",
                {
                    parent: "home",
                    url: "/StaffMajorSubjectDetails",
                    template: "<staffmajorsubjecstdetails></staffmajorsubjecstdetails>",
                    resolve: {
                    }
                })
            .state("StaffDevelopmentSkill",
                {
                    parent: "home",
                    url: "/StaffDevelopmentSkill",
                    template: "<staffdevelopmentskill></staffdevelopmentskill>",
                    resolve: {
                    }
                })
            .state("StaffDevelopmentSkillItem",
                {
                    parent: "home",
                    url: "/StaffDevelopmentSkillItem",
                    template: "<staffdevskillitem></staffdevskillitem>",
                    resolve: {
                    }
                })
            .state("Profession",
                {
                    parent: "home",
                    url: "/Profession",
                    template: "<professions></professions>",
                    resolve: {
                    }
                })
             .state("Branch",
                {
                    parent: "home",
                    url: "/Branch",
                    template: "<branches></branches>",
                    resolve: {
                    }
                })
            .state("Bank",
                {
                    parent: "home",
                    url: "/Bank",
                    template: "<banks></banks>",
                    resolve: {
                    }
                })
              .state("BankBranch",
                {
                    parent: "home",
                    url: "/BankBranch",
                    template: "<brankbranch></brankbranch>",
                    resolve: {
                    }
                })
               .state("BankAccount",
                {
                    parent: "home",
                    url: "/BankAccount",
                    template: "<brankaccount></brankaccount>",
                    resolve: {
                    }
                })
              .state("Category",
                {
                    parent: "home",
                    url: "/Category",
                    template: "<categories></categories>",
                    resolve: {
                    }
                })
               .state("CurrentState",
                 {
                     parent: "home",
                     url: "/CurrentState",
                     template: "<currentstates></currentstates>",
                     resolve: {
                     }
                 })
                .state("Department",
                {
                    parent: "home",
                    url: "/Department",
                    template: "<departments></departments>",
                    resolve: {
                    }
                })
               .state("Education",
                    {
                        parent: "home",
                        url: "/Education",
                        template: "<educations></educations>",
                        resolve: {
                        }
                    })
              .state("BusinessCategory",
                    {
                        parent: "home",
                        url: "/BusinessCategory",
                        template: "<bcategories></bcategories>",
                        resolve: {
                        }
                    })
             .state("BusinessCategoryDetails",
                    {
                        parent: "home",
                        url: "/BusinessCategoryDetails",
                        template: "<bcategorydetails></bcategorydetails>",
                        resolve: {
                        }
                    })
             .state("Designation",
                {
                    parent: "home",
                    url: "/Designation",
                    template: "<designation></designation>",
                    resolve: {
                    }
                })
                .state("EmployeRegistration",
                {
                    parent: "home",
                    url: "/EmployeRegistration",
                    params: { ID: null, },
                    template: "<employeregistration></employeregistration>",
                    resolve: {
                    }
                }).state("EmployeeList", {
                    parent: "home",
                    url: "/EmployeeList",
                    template: "<employeelist><employeelist>",
                    resolve:
                        {

                        }
                })
            .state("LanguageSkillLevel",
                {
                    parent: "home",
                    url: "/LanguageSkillLevel",
                    template: "<langskill></langskill>",
                    resolve: {
                    }
                })
            .state("LanguageQualification",
                {
                    parent: "home",
                    url: "/LanguageQualification",
                    template: "<langqual></langqual>",
                    resolve: {
                    }
                })
               .state("Language",
                {
                    parent: "home",
                    url: "/Language",
                    template: "<languages></languages>",
                    resolve: {
                    }
                })
               .state("JobCategory",
                {
                    parent: "home",
                    url: "/JobCategory",
                    template: "<jobcategories></jobcategories>",
                    resolve: {
                    }
                })
              .state("JobCategoryDetails",
                {
                    parent: "home",
                    url: "/JobCategoryDetails",
                    template: "<jobcategorydetails></jobcategorydetails>",
                    resolve: {
                    }
                })
             .state("ProfessionalCerificate",
                {
                    parent: "home",
                    url: "/ProfessionalCertificate",
                    template: "<professionalcerificates></professionalcerificates>",
                    resolve: {
                    }
                })
            .state("ProfessionalSpeciality",
                {
                    parent: "home",
                    url: "/ProfessionalSpeciality",
                    template: "<profspeciality></profspeciality>",
                    resolve: {
                    }
                })
                 .state("EstimationRoutes",
                {
                    parent: "home",
                    url: "/EstimationRoutes",
                    template: "<meroutes></meroutes>",
                    resolve: {
                    }
                })
              .state("EstimationServiceType",
                {
                    parent: "home",
                    url: "/EstimationServiceType",
                    template: "<meservicetypes></meservicetypes>",
                    resolve: {
                    }
                })

             .state("TransproLanguagePrice",
                {
                    parent: "home",
                    url: "/TransproLanguagePrice",
                    template: "<translanprice></translanprice>",
                    resolve: {
                    }
                })

             .state("EstimationSpecializedField",
                {
                    parent: "home",
                    url: "/EstimationSpecializedField",
                    template: "<mesfields></mesfields>",
                    resolve: {
                    }
                })
             .state("EstimationSubSpecializedField",
                {
                    parent: "home",
                    url: "/EstimationSubSpecializedField",
                    template: "<estisubspecializedfield></estisubspecializedfield>",
                    resolve: {
                    }
                })
            .state("TranslationEstimation",
            {
                parent: "home",
                url: "/TranslationEstimation/:id",
                params: { Estimation: null },
                template: '<translation-estimation routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList" translation-certificate-settings-list="$resolve.translationCertificateSettingsList"></translation-estimation>', //manipulate-user-access="$resolve.manipulateUserAccess"
                resolve: {
                    routes: ["estimationService", function (estimationService) {
                        return estimationService.estimationRoutes();
                    }],
                    services: ["estimationService", function (estimationService) {
                        return estimationService.estimationServices("TR");
                    }],
                    languages: ["estimationService", function (estimationService) {
                        return estimationService.estimationLanguages();
                    }],
                    businessCategories: ["estimationService", function (estimationService) {
                        return estimationService.businessCategories();
                    }],
                    specializedFields: ["estimationService", function (estimationService) {
                        return estimationService.specializedFields();
                    }],
                    subSpecializedFields: ["estimationService", function (estimationService) {
                        return estimationService.subSpecializedFields();
                    }],
                    currencyList: ["estimationService", function (estimationService) {
                        return estimationService.currencyList();
                    }],
                    translationCertificateSettingsList: ["estimationService", function (estimationService) {
                        return estimationService.translationCertificateSettingsList();
                    }]
                    //,manipulateUserAccess: ["estimationService", function (estimationService) {
                    //    return estimationService.manipulateUserAccess();
                    //}]
                }
            })
            .state("EstimationList", {
                parent: "home",
                url: "/EstimationList",
                template: '<estimation-list></estimation-list>',
                resolve: {}
            })
             .state("TaskQuotationInput",
                {
                    parent: "home",
                    url: "/TaskQuotationInput/:id",
                    params: { Estimation: null },
                    template: '<task-quotation routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></task-quotation>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
            .state("ApprovalEstimationList", {
                parent: "home",
                url: "/ApprovalEstimationList",
                template: '<approvalestimation-list></approvalestimation-list>',
                resolve: {}
            })
            .state("OverheadCostQuotation",
                {
                    parent: "home",
                    url: "/OverheadCostQuotation/:id",
                    params: { Estimation: null },
                    template: '<overhead-cost routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></overhead-cost>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
            .state("TranslationOrderDetails",
                {
                    parent: "home",
                    url: "/TranslationOrderDetails/:id",
                    params: { Estimation: null },
                    template: '<trans-order routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></trans-order>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
            .state("DTPOrderDetails",
                {
                    parent: "home",
                    url: "/DTPOrderDetails/:id",
                    params: { Estimation: null },
                    template: '<dtpdetails routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></dtpdetails>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
            .state("TaskOrderDetails",
                {
                    parent: "home",
                    url: "/TaskOrderDetails/:id",
                    params: { Estimation: null },
                    template: '<taskdetails routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></taskdetails>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })
            .state("OrderDetails",
                {
                    parent: "home",
                    url: "/OrderDetails/:id",
                    params: { Estimation: null },
                    template: '<orderdetails routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></orderdetails>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices();
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }]
                    }
                })


            .state("InterpretationOrderDetails",
            {
                parent: "home",
                url: "/InterpretationOrderDetails/:id",
                params: { Estimation: null },
                template: '<interpretationorderdetails routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList"></interpretationorderdetails>',
                resolve: {
                    routes: ["estimationService", function (estimationService) {
                        return estimationService.estimationRoutes();
                    }],
                    services: ["estimationService", function (estimationService) {
                        return estimationService.estimationServices();
                    }],
                    languages: ["estimationService", function (estimationService) {
                        return estimationService.estimationLanguages();
                    }],
                    businessCategories: ["estimationService", function (estimationService) {
                        return estimationService.businessCategories();
                    }],
                    specializedFields: ["estimationService", function (estimationService) {
                        return estimationService.specializedFields();
                    }],
                    subSpecializedFields: ["estimationService", function (estimationService) {
                        return estimationService.subSpecializedFields();
                    }],
                    currencyList: ["estimationService", function (estimationService) {
                        return estimationService.currencyList();
                    }]
                }
            })


               .state("EstimationInterpretationQuotation",
                {
                    parent: "home",
                    url: "/EstimationInterpretationQuotation/:id",
                    params: { Estimation: null },
                    template: '<estiinterpretationquotation routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList" translation-certificate-settings-list="$resolve.translationCertificateSettingsList"></estiinterpretationquotation>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices("IN");
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }],
                        translationCertificateSettingsList: ["estimationService", function (estimationService) {
                            return estimationService.translationCertificateSettingsList();
                        }]
                    }
                })
               .state("CompanyTradingCategory",
                {
                    parent: "home",
                    url: "/CompanyTradingCategory",
                    template: "<mctradingcategory></mctradingcategory>",
                    resolve: {
                    }
                })
               .state("CompanyTradingCategoryItem",
                {
                    parent: "home",
                    url: "/CompanyTradingCategoryItem",
                    template: "<mctradingcategoryitem></mctradingcategoryitem>",
                    resolve: {
                    }
                })

             .state("InterpretationFields",
                {
                    parent: "home",
                    url: "/InterpretationFields",
                    template: "<interpretationfields></interpretationfields>",
                    resolve: {
                    }
                })
              .state("CompanyIndustryClassification",
                {
                    parent: "home",
                    url: "/CompanyIndustryClassification",
                    template: "<companyindustryclassification></companyindustryclassification>",
                    resolve: {
                    }
                })
            .state("CompanyIndustryClassificationItem",
                {
                    parent: "home",
                    url: "/CompanyIndustryClassificationItem",
                    template: "<companyindustryclassificationitem></companyindustryclassificationitem>",
                    resolve: {
                    }
                })
            .state("CompanyTradingDivision",
                {
                    parent: "home",
                    url: "/CompanyTradingDivision",
                    template: "<companytradingdivision></companytradingdivision>",
                    resolve: {
                    }
                })
                .state("CompanyRegistration",
                {
                    parent: "home",
                    url: "/CompanyRegistration",
                    params: { ID: null, },
                    template: '<companyregistration config-Data="$resolve.Config" bank-Data="$resolve.bankData" bank-Account-Type-Data="$resolve.bankAccountTypeData" bank-Branch-Data="$resolve.BankBranchData" designation-Data="$resolve.Designation"></companyregistration>',
                    resolve: {
                        Config: ['companyService', function (companyService) {
                            return companyService.getCompanyConfigData();
                        }],
                        bankData: ['companyService', function (companyService) {
                            return companyService.getBank();
                        }],
                        bankAccountTypeData: ['companyService', function (companyService) {
                            return companyService.getBankAccountType();
                        }],
                        BankBranchData: ['companyService', function (companyService) {
                            return companyService.getBankBranches();
                        }],
                        Designation: ['companyService', function (companyService) {
                            return companyService.getDesignation();
                        }],
                    }
                })
                .state("CompanyRegistration.companyInfo", {
                    url: "/CompanyInfo",
                    templateUrl: "App/Components/Company/Template/Tab_BasicInfo.html"
                })
                .state("CompanyRegistration.detailInfo", {
                    url: "/DetailInfo",
                    templateUrl: "App/Components/Company/Template/Tab_DetailInfo.html"
                })
                .state("CompanyRegistration.salesRecord", {
                    url: "/SalesRecord",
                    templateUrl: "App/Components/Company/Template/Tab_SalesRecord.html"
                })
                .state("CompanyRegistration.subcontact", {
                    url: "/Subcontact",
                    templateUrl: "App/Components/Company/Template/Tab_Subcontact.html"
                })
                .state("CompanyRegistration.bank", {
                    url: "/BankPayment",
                    templateUrl: "App/Components/Company/Template/Tab_BankPayment.html"
                })
                .state("CompanyRegistration.transpro", {
                    url: "/trans-pro",
                    templateUrl: "App/Components/Company/Template/Tab_Transpro.html",
                    
                })
                 .state("CompanyRegistration.editingpro", {
                     url: "/editing-pro",
                     templateUrl: "App/Components/Company/Template/Tab_Editingpro.html"
                 })
                    .state("CompanyRegistration.hrpartner", {
                        url: "/HR-Partner",
                        templateUrl: "App/Components/Company/Template/Tab_Hrpartner.html"
                    })
                      .state("CompanyRegistration.dispatch", {
                          url: "/Dispatch",
                          templateUrl: "App/Components/Company/Template/Tab_Dispatch.html"
                      })
                    .state("CompanyList", {
                        parent: "home",
                        url: "/CompanyList",
                        template: '<companylist company-Data="$resolve.CompanyList"><companylist>',
                        resolve: {
                            CompanyList: ['companyService', function (companyService) {
                                return companyService.getCompany();
                            }]
                        }
                    })
                .state("CompanyBusiness", {
                    parent: "home",
                    url: "/CompanyBusiness",
                    template: "<companybusiness></companybusiness>",
                    resolve: {
                    }
                })
               .state("CompanyBusinessSpeciality",
                {
                    parent: "home",
                    url: "/CompanyBusinessSpeciality",
                    template: "<companybusinessspeciality></companybusinessspeciality>",
                    resolve: {
                    }
                })
              .state("AgentBusiness",
                {
                    parent: "home",
                    url: "/AgentBusiness",
                    template: "<agentbusiness></agentbusiness>",
                    resolve: {
                    }
                })
        .state("AgentCharacterstics",
                {
                    parent: "home",
                    url: "/AgentCharacterstics",
                    template: "<agentcharacterstics></agentcharacterstics>",
                    resolve: {
                    }
                })
        .state("AgentFreeDesignation",
                {
                    parent: "home",
                    url: "/AgentFreeDesignation",
                    template: "<agentfreedesignation></agentfreedesignation>",
                    resolve: {
                    }
                })
         .state("TechnicalSkillLevel",
                {
                    parent: "home",
                    url: "/TechnicalSkillLevel",
                    template: "<technicalskilllevel></technicalskilllevel>",
                    resolve: {
                    }
                })
         .state("StaffKnowledgeField",
                {
                    parent: "home",
                    url: "/StaffKnowledgeField",
                    template: "<staffknowledgefields></staffknowledgefields>",
                    resolve: {
                    }
                })
         .state("StaffKnowledgeFieldItem",
                {
                    parent: "home",
                    url: "/StaffKnowledgeFieldItem",
                    template: "<staffknowledgefielditems></staffknowledgefielditems>",
                    resolve: {
                    }
                })
        .state("StaffMedicalField",
                {
                    parent: "home",
                    url: "/StaffMedicalField",
                    template: "<staffmedicalfield></staffmedicalfield>",
                    resolve: {
                    }
                })
          .state("StaffPatentField",
                {
                    parent: "home",
                    url: "/StaffPatentField",
                    template: "<staffpatentfield></staffpatentfield>",
                    resolve: {
                    }
                })
         .state("HiworkLanguagePrice",
                {
                    parent: "home",
                    url: "/HiworkLanguagePrice",
                    template: "<hiworklanguageprices></hiworklanguageprices>",
                    resolve: {
                    }
                })
         .state("ShortTermEstimation",
                {
                    parent: "home",
                    url: "/ShortTermEstimation/:id",
                    params: { Estimation: null },
                    template: '<shortterm routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList" translation-certificate-settings-list="$resolve.translationCertificateSettingsList"></shortterm>',
                    resolve: {
                        routes: ["estimationService", function (estimationService) {
                            return estimationService.estimationRoutes();
                        }],
                        services: ["estimationService", function (estimationService) {
                            return estimationService.estimationServices("ST");
                        }],
                        languages: ["estimationService", function (estimationService) {
                            return estimationService.estimationLanguages();
                        }],
                        businessCategories: ["estimationService", function (estimationService) {
                            return estimationService.businessCategories();
                        }],
                        specializedFields: ["estimationService", function (estimationService) {
                            return estimationService.specializedFields();
                        }],
                        subSpecializedFields: ["estimationService", function (estimationService) {
                            return estimationService.subSpecializedFields();
                        }],
                        currencyList: ["estimationService", function (estimationService) {
                            return estimationService.currencyList();
                        }],
                        translationCertificateSettingsList: ["estimationService", function (estimationService) {
                            return estimationService.translationCertificateSettingsList();
                        }]
                    }
                })




            .state("WebOrderModule", {
                parent: "home",
                url: "/WebOrderModule/:id",
                params: { OrderInformation: null },
                template: '<webordermodule routes="$resolve.routes" services="$resolve.services" languages="$resolve.languages" business-categories="$resolve.businessCategories" specialized-fields="$resolve.specializedFields" sub-specialized-fields="$resolve.subSpecializedFields" currency-list="$resolve.currencyList" translation-certificate-settings-list="$resolve.translationCertificateSettingsList"></webordermodule>',
                resolve: {
                    routes: ["estimationService", function (estimationService) {
                        return estimationService.estimationRoutes();
                    }],
                    services: ["estimationService", function (estimationService) {
                        return estimationService.estimationServices("ST");
                    }],
                    languages: ["estimationService", function (estimationService) {
                        return estimationService.estimationLanguages();
                    }],
                    businessCategories: ["estimationService", function (estimationService) {
                        return estimationService.businessCategories();
                    }],
                    specializedFields: ["estimationService", function (estimationService) {
                        return estimationService.specializedFields();
                    }],
                    subSpecializedFields: ["estimationService", function (estimationService) {
                        return estimationService.subSpecializedFields();
                    }],
                    currencyList: ["estimationService", function (estimationService) {
                        return estimationService.currencyList();
                    }],
                    translationCertificateSettingsList: ["estimationService", function (estimationService) {
                        return estimationService.translationCertificateSettingsList();
                    }]
                }
            })




        .state("StaffPayment", {
            parent: "home",
            url: "/StaffPayment",
            template: "<staffpayment></staffpayment>",
            resolve: {
            }

        })
          .state("StaffTaxReport", {
              parent: "home",
              url: "/StaffTaxReport",
              template: "<stafftaxreport></stafftaxreport>",
              resolve: {
              }

          })
        .state("Notice", {
            parent: "home",
            url: "/Notice",
            template: "<notice></notice>",
            resolve: {
            }

        })
         .state("StaffSoftwareskill", {
             parent: "home",
             url: "/StaffSoftwareskill",
             template: "<staffsoftwareskill></staffsoftwareskill>",
             resolve: {
             }

         })
         .state("InquiryList", {
             parent: "home",
             url: "/InquiryList",
             template: "<inquirylist></inquirylist>",
             resolve: {
             }

         })
        .state("AdvertizementSettings", {
            parent: "home",
            url: "/AdvertizementSettings",
            template: "<advertizementsettings></advertizementsettings>",
            resolve: {
            }

        })

        .state("WebOrderList", {
            parent: "home",
            url: "/WebOrderList",
            template: "<weborderlist></weborderlist>",
            resolve: {

            }
        }).state("Penalty", {
            parent: "home",
            url: "/Penalty",
            template: "<penlty></penlty>",
            resolve: {

            }
        })

    });


    //hiworkApp.config(['$qProvider', function ($qProvider) {
    //    $qProvider.errorOnUnhandledRejections(false);
    //}]);

    hiworkApp.config(function ($provide) {
        $provide.decorator("$exceptionHandler", function ($delegate, $injector) {
            return function (exception, cause) {
                $delegate(exception, cause);
                var alerting = $injector.get("alerting");
                alerting.addDanger(exception.message);
            };
        });
    });

    hiworkApp.run(function ($rootScope) {
        $rootScope.$on("$locationChangeStart", function (event, next, current) {
            var appDocument = angular.element(document);
            appDocument.find('.nav-home-menu').on('click', function () {
                appDocument.find('#navMenu').removeClass('in');
            });
        });
    });


    /* Application Constants
        ======================================================*/
    hiworkApp.constant("appSettings", {
        API_BASE_URL: 'http://localhost:58580/',
        //API_BASE_URL: 'http://163.47.35.165:8081/',
        API_PHOTO_URL: 'http://localhost:58580/Upload/AllPhotos/',
        APPLICATION_VERSION: '1.0.0',
        ApplicationId: 1 //HiWork ID
    });
    /* Notification Constants
    ======================================================*/
    hiworkApp.constant("Notify", {
        DATA_READY: "dataFactory::dataReady",
        LOGIN_SUCCESSFUL: "accountFactory::loginSuccessful",
        LOGIN_UNSUCCESSFUL: "accountFactory::loginUnSuccessful",
        LOGOUT_SUCCESSFUL: "accountFactory::logoutSuccessful"

    });
    hiworkApp.constant("appMenu", {
        Dashboard: 1,
        Role: 2,
        Country: 3,
        Division: 4,
        Branch: 5,
        Department: 6
    });
    hiworkApp.constant("EstimationType", {
        Translation: 1,
        Interpreter: 2,
        SchoolExcursion: 3,
        ShortTermDispatch: 4,
        Project: 5,
        DTP: 6,
        Narration: 7,
        TapeOver: 8,
        WebCreation: 9,
        Transcription: 10,
        OverheadCost: 11
    });
    hiworkApp.constant("EstimationStatus", {
        Created: 1,
        RequestedForApproval: 2,
        Approved: 3,
        Requested: 4,
        Confirmed: 5,
        Ordered: 6,
        OrderLost: 7
    });
    hiworkApp.constant("UserType", {
        SuperAdmin: 1,
        Employee: 2,
        Maintenance: 3,
        Guest: 4
    });
    hiworkApp.constant("AppStorage", {
        currentLanguage: "currentLanguage",
        userData: "userData",
        cultureData: "cultureData",
        appData: "appData"

    });
    hiworkApp.constant("EstimationDefaultStatus", {
        init: {
            BtnOrderDetails: false,
            BtnTemporaryRegistration: true,
            BtnProjectInitiation: true,
            BtnApprovalRequest: false,
            BtnApproval: false,
            BtnQuotationEmail: false,
            BtnQuotationRequest: false,
            BtnConfirmationEmail: false,
            BtnDelete: false
        }
    });
}());
