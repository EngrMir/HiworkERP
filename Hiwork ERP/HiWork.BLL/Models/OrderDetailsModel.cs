using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class OrderDetailsModel : BaseDomainModel<OrderDetailsModel>
    {
        public string ID { get; set; }
        // public Guid ID { get; set; }
        // public Guid OrderID { get; set; }
         public string EstimationDetailsID { get; set; }
         public string StaffID { get; set; }
        // public string StaffName { get; set; }
        // public int? WorkingStatus { get; set; }
         public DateTime? DeliveryDate { get; set; }
         public decimal? EvaluationAmount { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? AgencyCommission { get; set; }
         public string ComplainDetails { get; set; }
        // public DateTime? RemitanceDate { get; set; }
        public decimal? DepositAmount { get; set; }
        //// public virtual StaffModel Staff { get; set; }
        // public EstimationDetailsModel EstimationDetail { get; set; }
        // public OrderModel Order { get; set; }
        // public StaffModel staffModel { get; set; }
        // public string FirstName_en { get; set; }
        public decimal? CostIncTax { get; set; }
        public decimal? CostExclTax { get; set; }
        public decimal? ClientBillingCost { get; set; }
        public decimal? PaymentAmountIncludingTax { get; set; }
        public decimal? StaffPaymentCostIncludingTax { get; set; }
        public decimal? StaffPaymentCostExcludingTax { get; set; }
        public decimal? TotalStaffPaymentCost { get; set; }
        public decimal? Penalty { get; set; }
        public bool IsComplaint { get; set; }

    }

    public class DetailsViewModel
    {
        public Guid ID { get; set; }
        public Guid EstimationDetailsID { get; set; }
        public Guid EstimationID { get; set; }
        public Guid OrderDetailID { get; set; }
        public Guid OrderID { get; set; }
        public Guid? StaffID { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public decimal? UnitPrice1 { get; set; }
        public int? PageCount1 { get; set; }
        public decimal? Discount1 { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? FinalDeliveryDate { get; set; }
        public string StaffName { get; set; }
        public int? WorkingStatus { get; set; }
        public string WorkingStatusName { get; set; }
        public decimal? EvaluationAmount { get; set; }

        public decimal? PaymentAmountIncludingTax { get; set; }
        public decimal? PaymentAmountExcludingTax { get; set; }
        public decimal? StaffPaymentCostIncludingTax { get; set; }
        public decimal? StaffPaymentCostExcludingTax { get; set; }
        public decimal? TotalStaffPaymentCost { get; set; }

        public decimal? AgencyCommission { get; set; }
        public DateTime? RemitanceDate { get; set; }
        public decimal? DepositAmount { get; set; }
        public string EmailAddress { get; set; }
        public string ComplainDetails { get; set; }
        public int? Condition { get; set; }
        public string StaffNo { get; set; }
        public decimal? Evaluation { get; set; }
        public decimal? Penalty { get; set; }
        public string Residence { get; set; }
        public Master_Language Source { get; set; }
        public Master_Language Target { get; set; }
        public Master_EstimationServiceType ServiceType { get; set; }
        public Master_WorkingStatus WorkingStatusObj { get; set; }
        public Staff Staff { get; set; }
        public bool? WithTranslation { get; set; }
        public decimal? LengthMinute { get; set; }
    }
}
