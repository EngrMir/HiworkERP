
using HiWork.DAL.Database;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Models
{
    public class EstimationModel : BaseDomainModel<EstimationModel>
    {
        public EstimationModel()
        {
            InquiryDate = DateTime.Now;
            RegistrationDate = DateTime.Now;
            FinalDeliveryDate = DateTime.Now;
            FirstDeliveryDate = DateTime.Now;
        }

        public Guid ID { get; set; }
        public long RegistrationID { get; set; }
        public string EstimationNo { get; set; }
        public DateTime? InquiryDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid? EstimateRouteID { get; set; }
        public string EstimateRouteName { get; set; }
        public Guid? OutwardSalesID { get; set; }
        public string OutwardSalesName { get; set; }
        public Guid? LargeSalesID { get; set; }
        public string LargeSalesName { get; set; }
        public Guid? SalesPersonID { get; set; }
        public string SalesPersonName { get; set; }
        public Guid? AssistantID { get; set; }
        public string AssistantName { get; set; }
        public Guid? CoordinatorID { get; set; }
        public string CoordinatorName { get; set; }
        public Guid? ApprovalID { get; set; }
        public string ApprovalName { get; set; }
        public Guid ClientID { get; set; }
        public string ClientName { get; set; }
        public Guid TradingID { get; set; }
        public string TradingName { get; set; }
        public Guid? ProjectID { get; set; }
        public Guid? TeamID { get; set; }
        public string TeamName { get; set; }
        public bool? IsCompanyPrivate { get; set; }
        public int? ClientStatus { get; set; }
        public Guid SpecializedFieldID { get; set; }
        public Guid? SubSpecializedFieldID { get; set; }
        public Guid? ClientDepartmentID { get; set; }
        public Guid? BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
        public string ClientPersonInCharge { get; set; }
        public string ClientEmailCC { get; set; }
        public string ClientAddress { get; set; }
        public string ClientContactNo { get; set; }
        public string ClientFax { get; set; }
        public string CurrencyName { get; set; }
        public string BillingCompanyName { get; set; }
        public string BillingTo { get; set; }
        public string BillingEmailCC { get; set; }

        public string BillingAddress { get; set; }
        public string BillingContactNo { get; set; }
        public string BillingFax { get; set; }
        public bool IsPostingBill { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryCompanyName { get; set; }
        public string DeliveryTo { get; set; }
        public string DeliveryEmailCC { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryContactNo { get; set; }
        public string DeliveryFax { get; set; }
        public string DeliveryInstruction { get; set; }
        public string RemarksCoordinatorType { get; set; }
        public long? CurrencyID { get; set; }
        public bool? IsProspect { get; set; }
        public bool? IsUndisclosed { get; set; }
        public EstimationType EstimationType { get; set; }
        public string EstimationTypeName { get; set; }
        public int? EstimationStatusID { get; set; }
        public EstimationStatus EstimationStatus { get; set; }
        public string EstimationStatusName { get; set; }
        public bool? IsInternalPurpose { get; set; }
        public bool? IsExternalPurpose { get; set; }
        public bool? IsPrintPurpose { get; set; }
        public bool? IsWebPurpose { get; set; }
        public bool? IsOtherPurpose { get; set; }
        public string OtherPurposeText { get; set; }
        public bool? PriorityQuality { get; set; }
        public bool? PriorityPrice { get; set; }
        public bool? PriorityDelivery { get; set; }
        public bool? PriorityTender { get; set; }
        public bool? KnownByGoogle { get; set; }
        public bool? KnownByYahoo { get; set; }
        public bool? KnownByEmail { get; set; }
        public bool? KnownByBing { get; set; }
        public bool? KnownByOthers { get; set; }
        public string KnownOtherText { get; set; }
        public DateTime? FinalDeliveryDate { get; set; }
        public DateTime? FirstDeliveryDate { get; set; }
        public string CoordinatorNotes { get; set; }
        public string Remarks { get; set; }
        public bool? IsRemarksHideInPDF { get; set; }
        public bool? IsOrderReceived { get; set; }
        public decimal? DiscountTotal { get; set; }
        public decimal? AverageUnitPrice { get; set; }
        public decimal? ActualUnitPrice { get; set; }
        public string PurposeDetails { get; set; }
        public string OrderTitle { get; set; }
        public long IssuedByTranslator { get; set; }
        public long IssuedByCompany { get; set; }
        public decimal? PriceCertification { get; set; }
        public string OtherItemName { get; set; }
        public decimal? OtherItemUnitPrice { get; set; }
        public int? OtherItemNumber { get; set; }
        public decimal? OtherAmount { get; set; }
        public decimal? TaxEstimation { get; set; }
        public decimal? QuotationInclTax { get; set; }
        public decimal? QuotationExclTax { get; set; }
        public decimal? ConsumptionOnTax { get; set; }
        public decimal? ConsumptionTax { get; set; }
        public decimal? ExcludedTaxCost { get; set; }
        public bool? IsCampaign { get; set; }
        public bool? IsSpecialPrice { get; set; }
        public bool? IsSpecialDeal { get; set; }
        public bool? IsPerformance { get; set; }
        public bool? IsPrice { get; set; }
        public bool? IsAccuracy { get; set; }
        public bool? IsLocal { get; set; }
        public decimal? SubtotalAfterDiscount { get; set; }
        public decimal? QuotationTotalInclTax { get; set; }
        public decimal? QuotationTotalExclTax { get; set; }

        public string TypeCode { get; set; }
        public string TypeName { get; set; }

        public string PageUrl { get; set; }

        public List<EstimationDetailsModel> EstimationItems { get; set; }
        public List<EstimationDetailsModel> TaskQuotationItems { get; set; }
        public List<EstimationDetailsModel> TranscriptionEstimationItems { get; set; }
        public List<OrderStaffAllowanceModel> CollectionFee { get; set; }

        public bool? IsSenderIncharge { get; set; }
        public bool? IsSenderRepresentative { get; set; }
        public bool? IsAttnIncharge { get; set; }
        public bool? IsCompanyName { get; set; }
        public bool? IsSealNone { get; set; }
        public bool? IsSealPresent { get; set; }
        public bool? IsInclTax { get; set; }
        public bool? IsBeforeTax { get; set; }
        public bool? IsContentAll { get; set; }
        public bool? IsContentDetailView { get; set; }
        public bool? IsRevisionNone { get; set; }
        public bool? IsRevisionPresent { get; set; }
        public decimal? GrandTotal { get; set; }
        public string TargetRoute { get; set; }
        public decimal? Profit { get; set; }
        public decimal? TotalWithoutTax { get; set; }
        public decimal? TotalWithTax { get; set; } 
        public Guid? StaffID { get; set; }
        public string StaffName { get; set; }
        public List<EstimationDeliveryFileType> DeliveryFileTypes { get; set; }
        public List<EstimationWorkContent> WorkContents { get; set; }
        public PageAttributes PageButtonAttribute { get; set; }
        public string QuotationNotes { get; set; }
        public string AttachedMaterialFileName { get; set; }
        public string AttachedMaterialDownloadURL { get; set; }
        public bool? ReturnDocument { get; set; }
        public bool? AddTimestampInSubtitles { get; set; }
        public bool? IsTemporaryRegistration { get; set; }
        public bool IsPromotion { get; set; }
        public bool IsSpecialPayment { get; set; }
        public bool IsPastComplaint { get; set; }
        public bool IsExpertise { get; set; }
        public bool IsOnGoingTask { get; set; }
        public bool IsOverseas { get; set; }
        public bool IsJapan { get; set; }
        public long CountryID { get; set; }
        public string KnownIntroductionText { get; set; }
        public bool KnownByIntroduction { get; set; }
        public long UnitID { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class EstimationProjectModel: BaseDomainModel<EstimationProjectModel>
    {
        public Guid ID { get; set; }
        public long RegistrationID { get; set; }
        public string ProjectNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Guid> EstimationList { get; set; }
    }

}
