using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.BLL;

namespace HiWork.BLL.Models
{
    public class AdvancedStaffSearchModel
    {
        //FROM BASE MODEL START
        public string CurrentUserID { get; set; }
        public string ApplicationID { get; set; }
        public string Culture { get; set; }

        //FROM BASE MODEL END

        public long RegistrationID { get; set; }
        public string MyIdentityNo { get; set; }
        public string StaffEmailID { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string TextSearch { get; set; }
        public string AchievementSearch { get; set; }
        public Guid? SourceOfRegistrationID { get; set; }
        public Guid? ForiegnLanguage1ID { get; set; }
        public int ForeignLang1Level { get; set; }
        public Guid? ForiegnLanguage2ID { get; set; }
        public int ForeignLang2Level { get; set; }
        public Guid? ForiegnLanguage3ID { get; set; }
        public int ForeignLang3Level { get; set; }
        public Guid? ForiegnLanguage4ID { get; set; }
        public int ForeignLang4Level { get; set; }
        public string Sex { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public Guid? NationalityID { get; set; }
        public Guid? VisaCountryID { get; set; }
        public Guid? VisaTypeID { get; set; }
        public DateTime VisaExpire { get; set; }
        public string RdoResidenceType { get; set; }
        public Guid? ResidenceNationalityID { get; set; }
        public bool IsIntroVideo { get; set; }
        public bool ChkActive30Days { get; set; }
        public bool IsSNS { get; set; }
        public Guid? SNSAccount { get; set; }
        public bool IsDtpExperience { get; set; }
        public Guid? DtpExp { get; set; }
        public Guid? TechnicalSkillParent { get; set; }
        public Guid? TechnicalSkillChild { get; set; }
        public Guid? DevelopmentSkillParent { get; set; }
        public Guid? DevelopmentSkillChild { get; set; }
        public Guid? KnowledgeSkillParent { get; set; }
        public Guid? KnowledgeSkillChild { get; set; }
        public Guid? MedicalSkillParent { get; set; }
        public Guid? MedicalSkillChild { get; set; }
        public int TranslationFrom { get; set; }
        public int TranslationTo { get; set; }
        public double TranslationUnitPrice { get; set; }
        public int InterpretationFrom { get; set; }
        public int InterpretationTo { get; set; }
        public string InterpretationUnitPriceType { get; set; }
        public double InterpretationUnitPriceValue { get; set; }
        public bool IsSuccessiveInterpretation { get; set; }
        public bool IsWhisperingInterpretation { get; set; }
        public bool IsSimultaneousInterpretation { get; set; }
        public int NarrationFrom { get; set; }
        public int NarrationTo { get; set; }
        public bool IsSpecialistNarator { get; set; }
        public Guid NarrationPerformance { get; set; }
    }

    //public List<string> PriorityList { get; set; }
    //public List<string> CertifiedExpert { get; set; }
    //public List<string> Exclusion { get; set; }
    //public List<Guid?> PCSkill { get; set; }
}
