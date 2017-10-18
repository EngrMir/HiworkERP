using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class OrderModel : BaseDomainModel<OrderModel>
    {
        public System.Guid ID { get; set; }
        public long RegistrationID { get; set; }
        public Nullable<long> ApplicationID { get; set; }
        public string OrderNo { get; set; }
        public string InvoiceNo { get; set; }
        public System.Guid EstimationID { get; set; }
        public System.Guid CompanyID { get; set; }
        public string DeliveryDescription { get; set; }
        public Nullable<decimal> QuoatedPrice { get; set; }
        public Nullable<decimal> ConsumptionTax { get; set; }
        public Nullable<decimal> CostExclTax { get; set; }
        public Nullable<decimal> BillingAmount { get; set; }
        public Nullable<decimal> OriginalCost { get; set; }
        public Nullable<decimal> WithdrawlAmount { get; set; }
        public Nullable<decimal> Profit { get; set; }
        public Nullable<int> OrderStatus { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
        public Nullable<decimal> GrossInterestProfit { get; set; }
        public Nullable<bool> IsDeposited { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<System.DateTime> EstimatedDateOfDeposit { get; set; }
        public Nullable<int> ClientComplain { get; set; }
        public Nullable<int> StaffComplain { get; set; }
        public Nullable<int> NonStaffComplain { get; set; }
        public Nullable<System.DateTime> ComplainDate { get; set; }
        public Nullable<System.DateTime> ResponseComplainDate { get; set; }
        public string ComplainDetails { get; set; }
        public string AccountingRelatedMemo { get; set; }
        public Nullable<decimal> CostInclTax { get; set; }
        public Nullable<decimal> PaymentinInstallment { get; set; }
        public Nullable<decimal> InternalPayment { get; set; }
        public Nullable<decimal> ExternalPayment { get; set; }
        public Nullable<decimal> Shand { get; set; }
        public Nullable<decimal> Bhand { get; set; }
        public Nullable<decimal> Chand { get; set; }
        public Nullable<decimal> Partner { get; set; }
        public Nullable<decimal> NetProfit { get; set; }
        public Nullable<decimal> NetMarginRate { get; set; }
        public Nullable<decimal> GrossProfit { get; set; }
        public Nullable<decimal> GrossMarginRate { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public string CoordinatorMemo { get; set; }
        public string CoordinatorPrecautions { get; set; }
        public string CoordinatorNotes { get; set; }
        public string NotesToStaff { get; set; }
        public Nullable<bool> IsInternalComplain { get; set; }
        public Nullable<bool> IsClientComplain { get; set; }
        public string DirectManuscript { get; set; }
        public Nullable<System.Guid> Coordinator2ID { get; set; }
        public string EmailCCFullString { get; set; }
        public Nullable<int> PaymentDateByMonth { get; set; }
        public bool? IsInternalResponse { get; set; }
        public decimal? OutwardsSalesPersonShare { get; set; }
        public decimal? Sales { get; set; }
        public decimal? PersonCharge1 { get; set; }
        public decimal? PersonCharge2 { get; set; }
    }
}
