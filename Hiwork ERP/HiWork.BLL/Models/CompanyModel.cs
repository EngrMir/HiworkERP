using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HiWork.Utils.Infrastructure;
using HiWork.Utils;

namespace HiWork.BLL.Models
{

    public class CompanyModel: BaseDomainModel<CompanyModel>
    {
        public Guid ID { get; set; }
        public long RegistrationID { get; set; }
        public int CompanyTypeID { get; set; }
        public long CompanyLocationID { get; set; }
        public int CompanyRegPurposeID { get; set; }
        public Guid? CompanyTradingOfficeID { get; set; }
        public string RegistrationNo { get; set; }
        public bool IsPartner { get; set; }
        public long? PartnerServiceTypeID { get; set; }
        public bool IsSubcontactual { get; set; }
        public string ClientNo { get; set; }
        public string ClientID { get; set; }
        public string Name_Local { get; set; }
        public string Name_Global { get; set; }
        public string WebSiteURL { get; set; }
        public string RepresentativeDirector { get; set; }
        public long Capital { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string BusinesClassification { get; set; }
        public string TransactionCategory { get; set; }
        public string TransactionNotes { get; set; }
        public string TransactionRecords { get; set; }
        public string FilesWithInvoice { get; set; }
        public string IntroducedBy { get; set; }
        public long DepartmentLocationID { get; set; }
        public long DepartmentTradingOfficeID { get; set; }
        public string DepartmentName_Local { get; set; }
        public string DepartmentName_Global { get; set; }
        public string DepartmentPhoneNo { get; set; }
        public string DepartmentFax { get; set; }
        public string DepartmentPostalCode { get; set; }
        public string DepartmentAddress { get; set; }
        public bool IsStopTrading { get; set; }
        public bool IsSharedRecord { get; set; }
        public string BusinessSummary { get; set; }
        public string CompanyIntroduction { get; set; }
        public string CompanyImpression { get; set; }
        public DateTime? SalesRecordRegDate { get; set; }
        public int ActivityTypeID { get; set; }
        public int ActivityResultID { get; set; }
        public string Comment { get; set; }
        public string Comment_en { get; set; }
        public string Comment_jp { get; set; }
        public string Comment_kr { get; set; }
        public string Comment_fr { get; set; }
        public string Comment_tl { get; set; }
        public string Comment_cn { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ClientMemberName { get; set; }
        public Guid EmployeeMemberID { get; set; }
        public string EmployeeMemberName { get; set; }
        public int PaymentWayID { get; set; }
        public long? BankTradingOfficeID { get; set; }
        public string BillingPostCode { get; set; }
        public string BillingAddress { get; set; }
        public string BilligPersonDesignation { get; set; }
        public string BillingPersonName { get; set; }
        public bool AllowPaymentAfterDelivery { get; set; }
        public string AffiliateCode { get; set; }
        public int AffiliateTypeID { get; set; }
        public string AffiliateCompanyName { get; set; }
        public DateTime? AffiliateUpdateDate { get; set; }
        public string Note { get; set; }
        public string Note_en { get; set; }
        public string Note_jp { get; set; }
        public string Note_kr { get; set; }
        public string Note_fr { get; set; }
        public string Note_cn { get; set; }
        public string Note_tl { get; set; }
        public bool IsCampaignEmail { get; set; }
        public bool IsOrderShowEnable { get; set; }
        public int TabId { get; set; }
        public CompanyDepartmentModel Dept { get; set; }
        public List<CompanyAgencyPriceModel> AgencyPrice { get; set; }
        //  public EmployeeModel Employee { get; set; }

        //Transpro Model
        public string Name { get; set; }
        public int? RegistrationType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fax { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string PostalCode { get; set; }
        public string Address1 { get; set; }
        public string Address1_en { get; set; }
        public string Address1_jp { get; set; }
        public string Address1_kr { get; set; }
        public string Address1_cn { get; set; }
        public string Address1_fr { get; set; }
        public string Address1_tl { get; set; }
        public string Address2 { get; set; }
        public string Address2_en { get; set; }
        public string Address2_jp { get; set; }
        public string Address2_kr { get; set; }
        public string Address2_cn { get; set; }
        public string Address2_fr { get; set; }
        public string Address2_tl { get; set; }
        public bool IsSelected { get; set; }
        public Nullable<bool> IsRecieveEmail { get; set; }
        public Nullable<bool> IsPostTransactionAllowed { get; set; }
        public List<CompanyIndustryClassificationViewModel> IndustryClassifications { get; set; }

        public CompanyTransproPartner transpro { get; set; }
    }

    public class CompanyViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public long RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public string ClientNo { get; set; }
        public string ClientID { get; set; }
        public string WebSiteURL { get; set; }
        public long Capital { get; set; }
        public string ClientLocation { get; set; }
        public DateTime? EstablishedDate { get; set; }
    }

    public class CompanyConfigData
    {
        public List<SelectedItem> ClientLocationType { get; set; }
        public List<SelectedItem> CompanyType { get; set; }
        public List<SelectedItem> RegPurposeType { get; set; }
        public List<BranchModel> BranchOfficeList { get; set; }
        public List<SelectedItem> ActivityType { get; set; }
        public List<SelectedItem> ResultofActivity { get; set; }
        public List<SelectedItem> PartnerType { get; set; }
        public List<SelectedItem> AffiliateType { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }
        public List<LanguageModel> LanguageList { get; set; }
        public List<EstimationTypeModel> EstimationTypeList { get; set; }
        public List<EstimationSpecializedFieldModel> SpecializationList { get; set; }
        public List<SelectedItem> PriceCalculateTypeList { get; set; }
        public List<SelectedItem> PamentWayList { get; set; }
        public List<SelectedItem> LanguageLevelList { get; set; }

        public List<PartnerServiceTypeModel> PartnerServiceTypeList { get; set; }
        public List<SelectedItem> PartnerTypeList { get; set; }

        public List<DeliveryMethodModel> DeliveryMethodList { get; set; }
    }
    public class CompanyIndustryClassificationViewModel : BaseDomainModel<CompanyIndustryClassificationViewModel>
    {
        public Guid ID { get; set; }
        public Guid CompanyID { get; set; }
        public Guid IndustryClassificationID { get; set; }
        public Guid IndustryClassificationItemID { get; set; }
        public bool IsSelected { get; set; }

        public string CompanyName { get; set; }
        public string ClassificationName { get; set; }
        public string ClassificationItemName { get; set; }
    }

    public class CompanyDepartmentModel: BaseDomainModel<CompanyDepartmentModel>
    {
        public Guid ID { get; set; }
        public Guid CompanyID { get; set; }
        public Guid DepartmentID { get; set; }
        public long LocationID { get; set; }
        public int TradingOfficeID { get; set; }
        public string Name_Local { get; set; }
        public string Name_Global { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string BillingClientName { get; set; }
        public string BillingTo { get; set; }
        public string BillingEmail { get; set; }
        public string BillingAddress { get; set; }
        public string BillingContactNo { get; set; }
        public string BillingFax { get; set; }
        public string BillingPaymentTerms { get; set; }
        public int? InchargeClientTypeID { get; set; }
        public long? InchargeTradingOfficeID { get; set; }
        public string InchargeName_Local { get; set; }
        public string InchargeName_pronounciation { get; set; }
        public string InchargeName_Global { get; set; }
        public string InchargePhoneNo { get; set; }
        public string InchargeCellPhoneNo { get; set; }
        public string InchargeEmail { get; set; }
        public string InchargePromotionEmail { get; set; }
        public string InchargeEmail_CC { get; set; }
        public string InchargePassword { get; set; }
        public string InchargeSalesPlanDate { get; set; }
        public string InchargeInternalName { get; set; }

    }
    public class CompanyAgencyPriceModel
    {
        public Guid ID { get; set; }
        public Guid CompanyID { get; set; }
        public int EstimationTypeID { get; set; }

        public string EstimationTypeName { get; set; }
        public Guid SourceLanguageID { get; set; }
        public string SourceLanguageName { get; set; }
        public Guid DestinationLanguageID { get; set; }
        public string DestinationLanguageName { get; set; }
        public Guid SpecializedFieldID { get; set; }
        public string SpecializedFieldName { get; set; }
        public long Unit { get; set; }
        public int PriceCalculationOnID { get; set; }
        public string PriceCalculationOnName { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsDelete { get; set; }
    }

    public class CompanyTransproPartner: BaseDomainModel<CompanyTransproPartner>
    {
        public Guid ID { get; set; }
        public Guid CompanyID { get; set; }
        public string AffiliateCode { get; set; }
        public string AffiliateCodeUpperID { get; set; }
        public long PartnerTypeID { get; set; }
        public string Name { get; set; }
        public string CompanyNumber { get; set; }
        public string TransPro_URL { get; set; }
        public string EstimationSending_URL { get; set; }
        public decimal MaintenanceFee { get; set; }
        public int MagnificationPrice { get; set; }
        public int TranslatorPrice { get; set; }
        public int PartnerCompanyPrice { get; set; }
        public int AgentPrice { get; set; }
        public int BcausePrice { get; set; }
        public string CompanyName { get; set; }
        public string CompanyName_en { get; set; }
        public string CompanyName_jp { get; set; }
        public string CompanyName_kr { get; set; }
        public string CompanyName_cn { get; set; }
        public string CompanyName_fr { get; set; }
        public string CompanyName_tl { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string Address_en { get; set; }
        public string Address_jp { get; set; }
        public string Address_kr { get; set; }
        public string Address_cn { get; set; }
        public string Address_fr { get; set; }
        public string Address_tl { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string CEOName { get; set; }
        public string CEOName_en { get; set; }
        public string CEOName_jp { get; set; }
        public string CEOName_kr { get; set; }
        public string CEOName_cn { get; set; }
        public string CEOName_fr { get; set; }
        public string CEOName_tl { get; set; }
        public string URL { get; set; }
        public string Logo { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public string ButtonImage { get; set; }
        public string InqueryImageFile { get; set; }
        public string Inquery_URL { get; set; }
        public string BannarImage1 { get; set; }
        public string BannarImage1_URL { get; set; }
        public string BannarImage2 { get; set; }
        public string BannarImage2_URL { get; set; }
        public string Title { get; set; }
        public string Title_en { get; set; }
        public string Title_jp { get; set; }
        public string Title_kr { get; set; }
        public string Title_cn { get; set; }
        public string Title_fr { get; set; }
        public string Title_tl { get; set; }
        public string Description { get; set; }

        public string ServiceName { get; set; }
        public string ServiceName_en { get; set; }
        public string ServiceName_jp { get; set; }
        public string ServiceName_kr { get; set; }
        public string ServiceName_cn { get; set; }
        public string ServiceName_fr { get; set; }
        public string ServiceName_tl { get; set; }
        public bool IsInvoicePartner { get; set; }
        public bool IsEmailTemplatepartner { get; set; }
        public bool IsAllowInqueryEmail { get; set; }
        public Guid? BankID { get; set; }
        public string BankName { get; set; }
        public Guid? BankBranchID { get; set; }
        public string BankBranchName { get; set; }
        public long? BankAccountTypeID { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string InvoiceCompanyName { get; set; }
        public string InvoiceCompanyName_en { get; set; }
        public string InvoiceCompanyName_jp { get; set; }
        public string InvoiceCompanyName_kr { get; set; }
        public string InvoiceCompanyName_cn { get; set; }
        public string InvoiceCompanyName_fr { get; set; }
        public string InvoiceCompanyName_tl { get; set; }
        public string InvoicePostCode { get; set; }
        public string InvoiceAddress1 { get; set; }
        public string InvoiceAddress1_en { get; set; }
        public string InvoiceAddress1_jp { get; set; }
        public string InvoiceAddress1_kr { get; set; }
        public string InvoiceAddress1_cn { get; set; }
        public string InvoiceAddress1_fr { get; set; }
        public string InvoiceAddress1_tl { get; set; }
        public string InvoiceAddress2 { get; set; }
        public string InvoiceAddress2_en { get; set; }
        public string InvoiceAddress2_jp { get; set; }
        public string InvoiceAddress2_kr { get; set; }
        public string InvoiceAddress2_cn { get; set; }
        public string InvoiceAddress2_fr { get; set; }
        public string InvoiceAddress2_tl { get; set; }
        public string InvoiveEmail { get; set; }
        public string InvoiceContactNumber { get; set; }
        public string InchagreName { get; set; }
        public string InchagreName_en { get; set; }
        public string InchagreName_jp { get; set; }
        public string InchagreName_kr { get; set; }
        public string InchagreName_cn { get; set; }
        public string InchagreName_fr { get; set; }
        public string InchagreName_tl { get; set; }
        public Guid? InchargeDesignationID { get; set; }
        public string InchargeDesignation { get; set; }
        public string ComapanySealImage { get; set; }
        public string AddressedPersonName { get; set; }
        public string AddressedPersonName_en { get; set; }
        public string AddressedPersonName_jp { get; set; }
        public string AddressedPersonName_kr { get; set; }
        public string AddressedPersonName_cn { get; set; }
        public string AddressedPersonName_fr { get; set; }
        public string AddressedPersonName_tl { get; set; }

    }

}
