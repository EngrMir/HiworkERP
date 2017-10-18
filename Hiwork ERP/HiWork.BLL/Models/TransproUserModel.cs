using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class BaseTransproUser
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int UserType { get; set; }
        public bool IsAuthenticated { get; set; }
    }
    public class CustomerUserModel: BaseTransproUser
    {
        public long RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string WebsiteURL { get; set; }
    }
    public class TranslatorUserModel : BaseTransproUser
    {
        public long RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public string MainCareer { get; set; }
        public string SelfPR { get; set; }
        public string Street { get; set; }
        public string ApartmentName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankAccountName { get; set; }     
        public Guid NativeLanguageID { get; set; }


    }
    public class PartnerUserModel : BaseTransproUser
    {
        public long RegistrationID { get; set; }
        public string CompanyName { get; set; }
        public string AffiliateName { get; set; }

    }
    public enum TransproUserType
    {
        Customer = 1,
        Translator = 2,
        Partner = 3
    } 
}
