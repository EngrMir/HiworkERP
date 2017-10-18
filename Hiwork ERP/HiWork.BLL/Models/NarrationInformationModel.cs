using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class NarrationInformationModel : BaseDomainModel<NarrationInformationModel>
    {
        public Guid ID { get; set; }
        public Guid StaffID { get; set; }
        public  long  NoOfExpYear { get; set; }
        public  long  NoOfExpMonth { get; set; }
        public string OtherExperience_en { get; set; }
        public string OtherExperience_jp { get; set; }
        public string OtherExperience_kr { get; set; }
        public string OtherExperience_cn { get; set; }
        public string OtherExperience_fr { get; set; }
        public string OtherExperience_tl { get; set; }
        public  long  PricePerWork { get; set; }
        public  long  PricePerHour { get; set; }
        public  long  MinPrice { get; set; }
        public  long  PricePerWorkRequest { get; set; }
        public  long  PricePerHourRequest { get; set; }
        public  long  MinPriceRequest { get; set; }
        public  int  VoiceType { get; set; }
        public  int  AgeImpression { get; set; }
        public string SceneOrPurposes { get; set; }
        public string VoiceSampleFile1 { get; set; }
        public string VoiceSampleFile2 { get; set; }
        public string VoiceSampleFile3 { get; set; }
        public string SelfPromotion_en { get; set; }
        public string SelfPromotion_jp { get; set; }
        public string SelfPromotion_kr { get; set; }
        public string SelfPromotion_cn { get; set; }
        public string SelfPromotion_fr { get; set; }
        public string SelfPromotion_tl { get; set; }
        public string Coordinator_en { get; set; }
        public string Coordinator_jp { get; set; }
        public string Coordinator_kr { get; set; }
        public string Coordinator_cn { get; set; }
        public string Coordinator_fr { get; set; }
        public string Coordinator_tl { get; set; }
        public  bool  IsShowWebsite { get; set; }
        public  bool  IsPartnerNarrator { get; set; }
        public  bool  IsPromote { get; set; }
        public  int  PriorityValue { get; set; }
        public  int  CustomerEvaluation { get; set; }
        public  int  NarrationLevel { get; set; }
        public  Guid  ProfessionID { get; set; }

    }
}
