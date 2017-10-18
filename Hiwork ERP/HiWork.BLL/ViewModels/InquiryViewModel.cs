using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.ViewModels
{
    public class InquiryViewModel
    {
        public string CompanyName { get; set; }
        public string ContatName { get; set; }
        public string Deploy { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        public string MailAddress { get; set; }
        public bool IsKnownByGoogle { get; set; }
        public bool IsKnownByYahoo { get; set; }
        public bool IsKnownByEmail { get; set; }
        public string IntroductionText { get; set; }
        public string OtherText { get; set; }
        public string IsLight { get; set; }
        public string IsBusiness { get; set; }
        public string IsExpert { get; set; }
    }
}
