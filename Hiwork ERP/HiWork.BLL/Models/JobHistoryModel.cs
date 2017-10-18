using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class JobHistoryModel : BaseDomainModel<JobHistoryModel>
    {

        public System.Guid ID { get; set; }
        public Nullable<System.Guid> EmploymentTypeID { get; set; }
        public string CompanyName_en { get; set; }
        public Guid BusinessTypeID { get; set; }
        public string OfficeLocation_en { get; set; }
        public Guid JobType1ID { get; set; }
        public Guid BusinessTypeItemID { get; set; }
        public Guid JobType2ItemID { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<System.DateTime> ResignDate { get; set; }
        public string CompanyPrivacyName_en { get; set; }
        public string Position_en { get; set; }
        public System.Guid StaffID { get; set; }
        public System.Guid JobType1ItemID { get; set; }
        public System.Guid JobType2ID { get; set; }




        //public string CompanyName_jp { get; set; }
        //public string CompanyName_kr { get; set; }
        //public string CompanyName_cn { get; set; }
        //public string CompanyName_fr { get; set; }
        //public string CompanyName_tl { get; set; }
        //public string CompanyPrivacyName_jp { get; set; }
        //public string CompanyPrivacyName_kr { get; set; }
        //public string CompanyPrivacyName_cn { get; set; }
        //public string CompanyPrivacyName_fr { get; set; }
        //public string CompanyPrivacyName_tl { get; set; }
        //public string CompanyDetails_en { get; set; }
        //public string CompanyDetails_jp { get; set; }
        //public string CompanyDetails_kr { get; set; }
        //public string CompanyDetails_cn { get; set; }
        //public string CompanyDetails_fr { get; set; }
        //public string CompanyDetails_tl { get; set; }
        //public System.Guid JobType1ItemID { get; set; }
        //public System.Guid JobType2ID { get; set; }
        //public System.Guid JobType3ID { get; set; }
        //public System.Guid JobType3ItemID { get; set; }
        //public string OfficeLocation_jp { get; set; }
        //public string OfficeLocation_kr { get; set; }
        //public string OfficeLocation_cn { get; set; }
        //public string OfficeLocation_fr { get; set; }
        //public string OfficeLocation_tl { get; set; }
        //public string Position_jp { get; set; }
        //public string Position_kr { get; set; }
        //public string Position_cn { get; set; }
        //public string Position_fr { get; set; }
        //public string Position_tl { get; set; }
        //public string JobResponsibility_en { get; set; }
        //public string JobResponsibility_jp { get; set; }
        //public string JobResponsibility_kr { get; set; }
        //public string JobResponsibility_cn { get; set; }
        //public string JobResponsibility_fr { get; set; }
        //public string JobResponsibility_tl { get; set; }
        //public string Achivements_en { get; set; }
        //public string Achivements_jp { get; set; }
        //public string Achivements_kr { get; set; }
        //public string Achivements_cn { get; set; }
        //public string Achivements_fr { get; set; }
        //public string Achivements_tl { get; set; }
        //public string InterviewNote_en { get; set; }
        //public string InterviewNote_jp { get; set; }
        //public string InterviewNote_kr { get; set; }
        //public string InterviewNote_cn { get; set; }
        //public string InterviewNote_fr { get; set; }
        //public string InterviewNote_tl { get; set; }
        //public Nullable<long> SalaryAmount { get; set; }
        //public string ReasonOfResignation_en { get; set; }
        //public string ReasonOfResignation_jp { get; set; }
        //public string ReasonOfResignation_kr { get; set; }
        //public string ReasonOfResignation_cn { get; set; }
        //public string ReasonOfResignation_fr { get; set; }
        //public string ReasonOfResignation_tl { get; set; }

    }
}
