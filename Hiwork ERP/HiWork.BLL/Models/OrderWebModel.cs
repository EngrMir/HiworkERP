using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HiWork.BLL.Models
{
    public class OrderWebModel : BaseDomainModel<OrderWebModel>
    {
        public Guid ID { get; set; }
        public string ApplicationName { get; set; }
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public string InvoiceNo { get; set; }
        public Guid SourceLanguageID { get; set; }
        public Guid TargetLanguageID { get; set; }
        public Guid TranslationFieldID { get; set; }
        public Guid ClientID { get; set; }
        public long ClientNo { get; set; }
        public Guid? AssignedTranslatorID { get; set; }
        public long DeliveryPlanID { get; set; }
        public string DeliveryPlan { get; set; }
        public string DeliveryLevelName { get; set; }
        public long? CurrencyID { get; set; }
        public Guid IntroducerID { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public int TranslationType { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? OrderStatus { get; set; }
        public int PaymentStatus { get; set; }
        public string CurrencyCode { get; set; }
        public int PaymentMethod { get; set; }
        public long WordCount { get; set; }
        public int? CountType { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public Nullable<decimal> TranslatorFee { get; set; }
        public decimal EstimatedPrice { get; set; }
        public Nullable<decimal>  UnitPrice{ get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> PriceAfterDiscount { get; set; }
        public Nullable<decimal> ConsumptionTax { get; set; }
        public Nullable<decimal> EvaluationScore { get; set; }
        public string EvaluationComment { get; set; }
        //public Nullable<decimal> StaffCharge { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string CommentToTranslator { get; set; }
        public string DeliveryComment { get; set; }
        public string CompanyNotes { get; set; }
        public string CommentToBcause { get; set; }
        public string ReferenceFileName { get; set; }
        public string ReferenceDownloadURL { get; set; }
        public string MenuScript { get; set; }
        public string TranslatorName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string OrderStatusName { get; set; }
        public string TranslationFieldName { get; set; }
        public long? TranslatorNo { get; set; }
        public int CompanyTypeID { get; set; }
        public string CompanyType { get; set; }
        public string WebSiteURL { get; set; }
        public string MobileNo { get; set; }
        public string TelephoneNo { get; set; }
        public string IntroducedBy { get; set; }
        public List<OrderWebDocumentsModel> WebDocumentList { get; set; }
        public List<OrderStaffAllowanceModel> StaffAllowanceList { get; set; }
        public List<MessageModel> MessageList { get; set; }
    }


    public class OrderWebDocumentsModel
    {
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string DownloadURL { get; set; }
        public DateTime? UploadDate { get; set; }
        public long WordCount { get; set; }
        public string OriginalFileName { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
    }

    public class OrderFilter
    {
        public string cultureId { get; set; }
        public long ApplicationId { get; set; }
        public string orderNo { get; set; } 
        public long? orderId { get; set; }
        public int? translationType { get; set; }
        public Guid? srcLangId { get; set; }
        public Guid? trgLangId { get; set; }
        public Guid? specialFieldId { get; set; }
        public Guid? clientId { get; set; }
        public Guid? translatorId { get; set; }
        public int? orderStatus { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime firstDateMonth { get; set; }
        public DateTime lastDateMonth { get; set; }
        
    }
    
}
