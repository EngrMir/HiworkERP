using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.DAL.Database;

namespace HiWork.BLL.Models
{
    public class StaffNestedModel
    {
        public StaffModel staff { get; set; }
        public List<JobHistoryModel> staffJobHistory { get; set; }
        public EducationHistoryModel staffEducationalHistory { get; set; }
        public StaffSkillTechModel staffSkillCertificate { get; set; }
        public TransproInformationModel transproInformationModel { get; set; }
        public StaffBankAccountInfoModel staffBankAccountInfoModel { get; set; }
        public TranslateInterpretExperienceModel staffTRExperience { get; set; }
        public NarrationInformationModel narrationInformationModel { get; set; }
    }
}
