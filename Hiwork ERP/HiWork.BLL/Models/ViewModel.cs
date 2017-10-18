using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    class ViewModel
    {
    }
    public class StaffPaymentModel
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public int TradingCorporationID { get; set; }
        public string TradingCorporation { get; set; }
        public string DepositeType { get; set; }
        public long DepositeTypeId { get; set; }
        public string Bank { get; set; }
        public Guid BankId { get; set; }
        public string Branch { get; set; }
        public Guid BranchId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public decimal RemittableAmount { get; set; }
        public decimal Carryover { get; set; }
        public decimal LastAdjustmentAmount { get; set; }
        public decimal ScheduledRemittanceAmount { get; set; }
        public decimal StaffBurdenFee { get; set; }
        public decimal BcauseBurdenFee{ get; set; }
        public decimal StaffTransferAmount { get; set; }
        public decimal BcauseTransferAmount { get; set; }
        public decimal MoneyTransfer { get; set; }
        public DateTime? RemittanceDate { get; set; }
        public decimal RemittedAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }

    }
    }
