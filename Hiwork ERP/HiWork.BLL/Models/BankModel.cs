using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{     
  public class BankModel : BaseDomainModel<BankModel>
    {
        public Guid Id { get; set; }                // Primary key
        public string Name { get; set; }
        public string Code { get; set; }
        public long CountryId { get; set; }
        public long CurrencyId { get; set; }
        public CountryModel Country { get; set; }
        public CurrencyModel Currency { get; set; }
       
    }


    public class BankAccountModel : BaseDomainModel<BankAccountModel>
    {
        public Guid ID { get; set; }
        public Guid BankID { get; set; }
        public Guid BankBranchID { get; set; }
        public long AccountTypeID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountName_en { get; set; }
        public string AccountName_jp { get; set; }
        public string AccountName_kr { get; set; }
        public string AccountName_cn { get; set; }
        public string AccountName_fr { get; set; }
        public string AccountName_tl { get; set; }

        public BankModel bankModel { get; set; }
        public BankBranchModel branchModel { get; set; }

        public BankAccountTypeModel AccountTypeModel { get; set; }


    }

    public class BankAccountTypeModel : BaseDomainModel<BankAccountTypeModel>
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }


    public class BankBranchModel : BaseDomainModel<BankBranchModel>
    {
        public Guid ID { get; set; }
        public Guid BankID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SwiftCode { get; set; }
        public string Address_en { get; set; }
        public string Address_jp { get; set; }
        public string Address_kr { get; set; }
        public string Address_cn { get; set; }
        public string Address_fr { get; set; }
        public string Address_tl { get; set; }
        public string Address { get; set; }
        public virtual BankModel bankModel { get; set; }
    }
}
