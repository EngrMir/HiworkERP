//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HiWork.DAL.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Master_BankAccount
    {
        public System.Guid ID { get; set; }
        public System.Guid BankID { get; set; }
        public System.Guid BankBranchID { get; set; }
        public long AccountTypeID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName_en { get; set; }
        public string AccountName_jp { get; set; }
        public string AccountName_kr { get; set; }
        public string AccountName_cn { get; set; }
        public string AccountName_fr { get; set; }
        public string AccountName_tl { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Master_Bank Master_Bank { get; set; }
        public virtual Master_BankAccountType Master_BankAccountType { get; set; }
        public virtual Master_BankBranch Master_BankBranch { get; set; }
    }
}