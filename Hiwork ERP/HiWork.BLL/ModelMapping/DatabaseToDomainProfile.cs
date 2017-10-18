using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;

namespace HiWork.BLL.ModelMapping
{
    public partial class DatabaseToDomainProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<UserType, UserTypeModel>();
            CreateMap<UserInformation, UserModel>();
            CreateMap<Role,RoleModel>();
            CreateMap<Master_StaffVisaType, VisaTypeModel>();
            CreateMap<Master_Country, CountryModel>();
            CreateMap<Master_StaffProfession, ProfessionModel>();
            CreateMap<Master_Culture, CultureModel>();
            CreateMap<Master_BranchOffice, BranchModel>();
            CreateMap<Master_Designation, DesignationModel>();
            CreateMap<Master_Division, DivisionModel>();
            CreateMap<Master_Department, DepartmentModel>();
            CreateMap<Master_Team, TeamModel>();
            CreateMap<Master_StaffCategory, CategoryModel>();
            CreateMap<Master_Bank, BankModel>();
            CreateMap<Master_StaffCurrentState, CurrentStateModel>();
            CreateMap<Master_BankAccount, BankAccountModel>();
            CreateMap<Master_Currency, CurrencyModel>();
            CreateMap<Master_BankBranch, BankBranchModel>();
            CreateMap<Master_BankAccountType, BankAccountTypeModel>();
            CreateMap<Master_StaffEducationalDegree, EducationModel>();
            CreateMap<UserInformation, UserInfoModel>();
            CreateMap<Master_StaffTechnicalSkillCategory, TechnicalSkillCategoryModel>();
            CreateMap<Master_StaffTechnicalSkillItems, TechnicalSkillItemsModel>();
            CreateMap<Master_StaffBusinessCategory, BusinessCategoryModel>();
            CreateMap<Master_StaffBusinessCategoryDetails, BusinessCategoryDetailsModel>();
            CreateMap<UserInRole, UserInRoleModel>();
            CreateMap<UserType, UserTypeModel>();
            CreateMap<Master_Language, LanguageModel>();
            CreateMap<Employee, EmployeeModel>();
            CreateMap<Master_StaffMajorSubject, MajorSubjectModel>();
            CreateMap<Master_StaffTranslationFields, StaffTranslationFieldsModel>();
            CreateMap<Master_StaffMajorSubjectDetails, MajorSubjectDetailsModel>();
            CreateMap<Master_StaffJobCategory, JobCategoryModel>();
            CreateMap<Master_StaffJobCategoryDetails, JobCategoryDetailsModel>();
            CreateMap<Master_StaffType, StaffTypeModel>();
            CreateMap<Staff, StaffModel>();
            CreateMap<Master_StaffLanguageQualifications, LanguageQualificationModel>();
            CreateMap<Master_EstimationType, EstimationTypeModel>();
            CreateMap<Master_StaffInterpretationFields, InterpretationFieldsModel>();
            CreateMap<Master_StaffEmploymentType, EmploymentTypeModel>();
            CreateMap<Master_EstimationRoutes, EstimationRoutesModel>();
            CreateMap<Master_EstimationServiceType, EstimationServiceTypeModel>();
            CreateMap<Company, CompanyModel>();
            CreateMap<Master_EstimationSpecializedField, EstimationSpecializedFieldModel>();
            CreateMap<Master_CompanyTradingCategory, CompanyTradingCategoryModel>();
            CreateMap<Master_CompanyTradingCategoryItem, CompanyTradingCategoryItemModel>();
            CreateMap<Master_CompanyIndustryClassification, CompanyIndustryClassificationModel>();
            CreateMap<Master_CompanyIndustryClassificationItem, CompanyIndustryClassificationItemModel>();
            CreateMap<Master_CompanyTradingDivision, CompanyTradingDivisionModel>();
            CreateMap<Master_Unit, UnitModel>();
            CreateMap<Application, ApplicationModel>();
            CreateMap<CompanyIndustryClassification, CompanyIndustryClassificationViewModel>();
            CreateMap<Staff_BankAccountInfo, StaffBankAccountInfoModel>();
            CreateMap<Staff_JobHistory, JobHistoryModel>();
            CreateMap<Master_StaffDevelopmentSkill, StaffDevelopmentSkillModel>();
            CreateMap<Master_StaffDevelopmentSkillItem, StaffDevelopmentSkillItemModel>();
            CreateMap<Master_StaffKnowledgeField, StaffKnowledgeFieldModel>();
            CreateMap<Master_StaffKnowledgeFieldItem, StaffKnowledgeFieldItemModel>();
            CreateMap<Master_StaffMedicalField, StaffMedicalFieldModel>();
            CreateMap<Master_StaffPatentField, StaffPatentFieldModel>();
            CreateMap<Master_EstimationSubSpecializedField, EstimationSubSpecializedFieldModel>();
            CreateMap<Staff_NarrationInformation, NarrationInformationModel>();
            CreateMap<Master_StaffSpecialField, StaffSpecialFieldModel>();
            CreateMap<Staff_TranslationSpecialFields, StaffTranslationSpecialFieldsModel>();
            CreateMap<Staff_TranslateInterpretExperience, TranslateInterpretExperienceModel>();
            CreateMap<Staff_TransproInformation, TransproInformationModel>();
            CreateMap<Master_StaffNarrationType, StaffNarrationTypeModel>();
            CreateMap<Estimation, EstimationModel>();
            CreateMap<Company_Department, CompanyDepartmentModel>();
            CreateMap<EstimationProject, EstimationProjectModel>();
            CreateMap<HiworkLanguagePrice, HiworkLanguagePriceModel>();
            CreateMap<Company_AgencyPrice, CompanyAgencyPriceModel>();
            CreateMap<Master_UnitPriceSetting, UnitPriceModel>();
            CreateMap<TransproDeliveryPlanSetting, TransproDeliveryPlanSettingModel>();
            CreateMap<OrderDetail, OrderDetailsModel>(); 
            CreateMap<Order, OrderModel>();
            CreateMap<Staff_ProfessionalSpeciality, StaffProfesionalSpecialityModel>();
            CreateMap<Staff_CurrentStates, StaffCurrentStateModel>();
            CreateMap<EmailTemplate, EmailTemplateModel>();
            CreateMap<Notice, NoticeModel>();
            CreateMap<Master_EmailCategorySettings, MasterEmailCategorySettingsModel>();
            CreateMap<Master_EmailGroupSettings, MasterEmailGroupSettingsModel>();
            CreateMap<Company_TransproPartner, CompanyTransproPartner>();
            CreateMap<Master_StaffEducationalDegree, StaffEducationalDegModel>();
            CreateMap<Master_StaffMajorSubject, StaffMejorSubModel>();
            CreateMap<Master_PartnerServiceType, PartnerServiceTypeModel>();
            CreateMap<ErrorReportWeb, ErrorReportWebModel>();
            CreateMap<Master_StaffSoftwareSkill, MasterStaffSoftwareSkillModel>();
            CreateMap<EstimationDetail, EstimationDetailsModel>();
            CreateMap<EmailDeliverySetting, EmailDeliverySettingsModel>();
            CreateMap<Message, MessageModel>();
            CreateMap<ContactU,ContactUsModel>();
            CreateMap<Staff_SoftwareSkill,StaffSoftwareSkillModel>(); 
            CreateMap<Staff_TranslateInterpretExperience, TranslateInterpretExperienceModel>();
            CreateMap<EstimationFile, EstimationFileModel>();
            CreateMap<AdvertizementSetting,AdvertizementSettingsModel>();
            CreateMap<Order_StaffAllowance, OrderStaffAllowanceModel>();
            CreateMap<Staff_NarrationVoiceFiles,NarrationVoiceFilesModel>();
            CreateMap<Staff_NarrationInformation,NarrationInformationModel>();
            CreateMap<Master_DeliveryMethod, DeliveryMethodModel>();
            CreateMap<Master_Penalty, PenaltyModel>();

        }
    }
}

