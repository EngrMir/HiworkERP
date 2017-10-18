
using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class StaffBankAccountInfoModel : BaseDomainModel<StaffBankAccountInfoModel>
    {
        public Guid ID { get; set; }
        public Guid StaffID { get; set; }
        public Guid BankID { get; set; }
        public Guid BankBranchID { get; set; }
        public Int16 AccountTypeID { get; set; }
        public string ChangeLog { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderName_en { get; set; }
        public string AccountHolderName_jp { get; set; }
        public string AccountHolderName_kr { get; set; }
        public string AccountHolderName_cn { get; set; }
        public string AccountHolderName_fr { get; set; }
        public string AccountHolderName_tl { get; set; }
        public string AccountNo { get; set; }
        public string AccountHolderAddress { get; set; }
        public string AccountHolderAddress_en { get; set; }
        public string AccountHolderAddress_jp { get; set; }
        public string AccountHolderAddress_kr { get; set; }
        public string AccountHolderAddress_cn { get; set; }
        public string AccountHolderAddress_fr { get; set; }
        public string AccountHolderAddress_tl { get; set; }
        public string AccountHolderContactNo { get; set; }
        public string AccountNote { get; set; }
        public string AccountNote_en { get; set; }
        public string AccountNote_jp { get; set; }
        public string AccountNote_kr { get; set; }
        public string AccountNote_cn { get; set; }
        public string AccountNote_fr { get; set; }
        public string AccountNote_tl { get; set; }
       




    }
}
