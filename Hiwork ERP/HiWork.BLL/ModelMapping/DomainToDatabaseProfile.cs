using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;

namespace HiWork.BLL.ModelMapping
{
    public partial class DomainToDatabaseProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<UserTypeModel, UserType>();
            CreateMap<UserModel, UserInformation>();
            CreateMap<RoleModel, Role>();
            CreateMap<VisaTypeModel, Master_StaffVisaType>();
            CreateMap<CountryModel, Master_Country>();
            CreateMap<ProfessionModel, Master_StaffProfession>();
            CreateMap<CultureModel, Master_Culture>();
            CreateMap<BranchModel, Master_BranchOffice>();
            CreateMap<DesignationModel, Master_Designation>();
            CreateMap<DivisionModel, Master_Division>();
            CreateMap<DepartmentModel, Master_Department>();
            CreateMap<TeamModel, Master_Team>();
            CreateMap<CategoryModel, Master_StaffCategory>();
            CreateMap<BankModel, Master_Bank>();
            CreateMap<CurrentStateModel, Master_StaffCurrentState>();
            CreateMap<CurrencyModel, Master_Currency>();
            CreateMap<BankAccountModel, Master_BankAccount>();
            CreateMap<BankBranchModel, Master_BankBranch>();
            CreateMap<BankAccountTypeModel, Master_BankAccountType>();
            CreateMap<EducationModel, Master_StaffEducationalDegree>();
            CreateMap<UserInfoModel, UserInformation>();
            CreateMap<TechnicalSkillCategoryModel, Master_StaffTechnicalSkillCategory>();
            CreateMap<TechnicalSkillItemsModel, Master_StaffTechnicalSkillItems>();
            CreateMap<BusinessCategoryModel, Master_StaffBusinessCategory>();
            CreateMap<BusinessCategoryDetailsModel, Master_StaffBusinessCategoryDetails>();
            CreateMap<UserInRoleModel, UserInRole>();
            CreateMap<UserTypeModel, UserType>();
            CreateMap<LanguageModel, Master_Language>();
            CreateMap<EmployeeModel, Employee>();
            CreateMap<MajorSubjectModel, Master_StaffMajorSubject>();
            CreateMap<StaffTranslationFieldsModel, Master_StaffTranslationFields>();
            CreateMap<MajorSubjectDetailsModel, Master_StaffMajorSubjectDetails>();
            CreateMap<JobCategoryModel, Master_StaffJobCategory>();
            CreateMap<JobCategoryDetailsModel, Master_StaffJobCategoryDetails>();
            CreateMap<StaffTypeModel, Master_StaffType>();
            CreateMap<StaffModel, Staff>();
            CreateMap<LanguageQualificationModel, Master_StaffLanguageQualifications>();
            CreateMap<EstimationTypeModel, Master_EstimationType>();
            CreateMap<InterpretationFieldsModel, Master_StaffInterpretationFields>();
            CreateMap<EmploymentTypeModel, Master_StaffEmploymentType>();
            CreateMap<EstimationRoutesModel, Master_EstimationRoutes>();
            CreateMap<EstimationServiceTypeModel, Master_EstimationServiceType>();
            CreateMap<CompanyModel, Company>();
            CreateMap<EstimationSpecializedFieldModel, Master_EstimationSpecializedField>();
            CreateMap<CompanyTradingCategoryModel, Master_CompanyTradingCategory>();
            CreateMap<CompanyTradingCategoryItemModel, Master_CompanyTradingCategoryItem>();
            CreateMap<CompanyIndustryClassificationModel, Master_CompanyIndustryClassification>();
            CreateMap<CompanyIndustryClassificationItemModel, Master_CompanyIndustryClassificationItem>();
            CreateMap<CompanyTradingDivisionModel, Master_CompanyTradingDivision>();
            CreateMap<UnitModel, Master_Unit>();
            CreateMap<ApplicationModel, Application>();
            CreateMap<CompanyIndustryClassificationViewModel, CompanyIndustryClassification>();
            CreateMap<StaffBankAccountInfoModel, Staff_BankAccountInfo>();
            CreateMap<JobHistoryModel, Staff_JobHistory>();
            CreateMap<StaffDevelopmentSkillModel, Master_StaffDevelopmentSkill>();
            CreateMap<StaffDevelopmentSkillItemModel, Master_StaffDevelopmentSkillItem>();
            CreateMap<StaffKnowledgeFieldModel, Master_StaffKnowledgeField>();
            CreateMap<StaffKnowledgeFieldItemModel, Master_StaffKnowledgeFieldItem>();
            CreateMap<StaffMedicalFieldModel, Master_StaffMedicalField>();
            CreateMap<StaffPatentFieldModel, Master_StaffPatentField>();
            CreateMap<EstimationSubSpecializedFieldModel, Master_EstimationSubSpecializedField>();
            CreateMap<NarrationInformationModel, Staff_NarrationInformation>();
            CreateMap<StaffSpecialFieldModel, Master_StaffSpecialField>();
            CreateMap<StaffTranslationSpecialFieldsModel, Staff_TranslationSpecialFields>();
            CreateMap<TranslateInterpretExperienceModel, Staff_TranslateInterpretExperience>();
            CreateMap<TransproInformationModel, Staff_TransproInformation>();
            CreateMap<StaffNarrationTypeModel, Master_StaffNarrationType>();
            CreateMap<EstimationModel, Estimation>();
            CreateMap<CompanyDepartmentModel, Company_Department>();
            CreateMap<EstimationProjectModel, EstimationProject>();
            CreateMap<HiworkLanguagePriceModel, HiworkLanguagePrice>();
            CreateMap<CompanyAgencyPriceModel,Company_AgencyPrice>();
            CreateMap<UnitPriceModel, Master_UnitPriceSetting>();
            CreateMap<TransproDeliveryPlanSettingModel, TransproDeliveryPlanSetting>();
            CreateMap<OrderDetailsModel, OrderDetail>();
            CreateMap<OrderModel, Order>();
            CreateMap<StaffProfesionalSpecialityModel, Staff_ProfessionalSpeciality>();
            CreateMap<StaffCurrentStateModel, Staff_CurrentStates>();
            CreateMap<EmailTemplateModel, EmailTemplate>();
            CreateMap<NoticeModel, Notice>();
            CreateMap<MasterEmailCategorySettingsModel, Master_EmailCategorySettings>();
            CreateMap<MasterEmailGroupSettingsModel, Master_EmailGroupSettings>();
            CreateMap<CompanyTransproPartner, Company_TransproPartner>();
            CreateMap<StaffEducationalDegModel, Master_StaffEducationalDegree>();
            CreateMap<PartnerServiceTypeModel, Master_PartnerServiceType>();
            CreateMap<ErrorReportWebModel, ErrorReportWeb>();
            CreateMap<MasterStaffSoftwareSkillModel, Master_StaffSoftwareSkill>();
            CreateMap<EstimationDetailsModel, EstimationDetail>();
            CreateMap<EmailDeliverySettingsModel, EmailDeliverySetting>();
            CreateMap<MessageModel, Message>();
            CreateMap<ContactUsModel, ContactU>();
            CreateMap<StaffSoftwareSkillModel,Staff_SoftwareSkill>();
            CreateMap<TranslateInterpretExperienceModel,Staff_TranslateInterpretExperience>();
            CreateMap<EstimationFileModel, EstimationFile>();
            CreateMap<AdvertizementSettingsModel,AdvertizementSetting>();
            CreateMap<OrderStaffAllowanceModel, Order_StaffAllowance>();
            CreateMap<NarrationVoiceFilesModel, Staff_NarrationVoiceFiles>();
            CreateMap<NarrationInformationModel, Staff_NarrationInformation>();
            CreateMap<DeliveryMethodModel, Master_DeliveryMethod>();
            CreateMap<PenaltyModel, Master_Penalty>();
        }
    }
}


