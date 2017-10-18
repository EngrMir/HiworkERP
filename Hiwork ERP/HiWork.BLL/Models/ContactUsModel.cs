using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
public class ContactUsModel : BaseDomainModel<ContactUsModel>
    {
        public Guid ID { get; set;}
        public long RegistrationID { get; set; }
        public string Name { get; set; }
        public string DivisionName { get; set;}
        //public Guid DivisionID { get; set; }
        //public DivisionModel Division{ get; set; }
        public string TelNumber { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Comment_en { get; set; }
        public string Comment_jp { get; set; }
        public string Comment_kr { get; set; }
        public string Comment_cn { get; set; }
        public string Comment_fr { get; set; }
        public string Comment_tl { get; set; }
        public DateTime? ReplyDate { get; set; }
        public long? RepliedBy { get; set; }
        public UserInfoModel UserInformation { get; set; }
        public bool  IsReplied { get; set; }
        public string CompanyName { get; set; }
        public string CompanyURLOne { get; set;}
        public string CompanyURLTwo { get; set; }
        public string PrefferedWebSystem { get; set;} 
        public bool IsApplication { get; set; }
    }
}
