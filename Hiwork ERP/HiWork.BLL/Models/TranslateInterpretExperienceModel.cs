using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{   
   public class TranslateInterpretExperienceModel : BaseDomainModel<TranslateInterpretExperienceModel >
    {
        public Guid ID { get; set; }
        public Guid StaffID { get; set; }
        public int PriorityID { get; set; }
        public Guid TransSpecialFieldID1 { get; set; }
        public Guid TransSpecialFieldID2 { get; set; }
        public Guid TransSpecialFieldID3 { get; set; }
        public bool HasResearchExperience { get; set; }
        public bool HasMSOfficeExperience { get; set; }
        public bool HasMacOSExperience { get; set; }
        public bool HasAdobeExperience { get; set; }
        public long AssignedWork { get; set; }
        public long WorkDelay { get; set; }
        public long WorkClaim { get; set; }
        public long PuntualityPercentage { get; set; }
        public long ClaimPercentage { get; set; }
        public  decimal  EvaluationAvg { get; set; }
        public  decimal  EvaluationInterp { get; set; }
        public  decimal  EvaluationTrans { get; set; }
        public  decimal  EvaluationDTP { get; set; }
        public  decimal  TransRatingPunctuality { get; set; }
        public  decimal  TransRatingQuality { get; set; }
        public  decimal  TransRatingComment { get; set; }
        public  decimal  TransRatingFeedback { get; set; }
        public  decimal  TransRatingLayout { get; set; }
        public  decimal  TransRatingResponse { get; set; }
        public  decimal  TransRatingNegotiation { get; set; }
        public  decimal  InterpRatingPunctuality { get; set; }
        public  decimal  InterpRatingQuality { get; set; }
        public  decimal  InterpRatingComment { get; set; }
        public  decimal  InterpRatingFeedback { get; set; }
        public  decimal  InterpRatingReport { get; set; }
        public  decimal  InterpRatingManner { get; set; }
        public  decimal  InerpRatingNegotiation { get; set; }
        public  decimal  TransPricePerPage { get; set; }
        public  decimal  TransPricePerWord { get; set; }
        public  decimal  TransPricePerPageRequested { get; set; }
        public  decimal  TransPricePerWordRequested { get; set; }
        public string TransPriceNote_en { get; set; }
        public string TransPriceNote_jp { get; set; }
        public string TransPriceNote_kr { get; set; }
        public string TransPriceNote_cn { get; set; }
        public string TransPriceNote_fr { get; set; }
        public string TransPriceNote_tl { get; set; }

        public decimal InterpPricePerDay { get; set; }
        public decimal InterpPricePerHalfDay { get; set; }
        public decimal InterpPricePerHour { get; set; }
        public decimal InterpPricePerPageRequested { get; set; }
        public decimal InterpPricePerHalfDayRequested { get; set; }
        public decimal InterpPricePerHourRequested { get; set; }
        public string InterpPriceNote_en { get; set; }
        public string InterpPriceNote_jp { get; set; }
        public string InterpPriceNote_kr { get; set; }
        public string InterpPriceNote_cn { get; set; }
        public string InterpPriceNote_fr { get; set; }
        public string InterpPriceNote_tl { get; set; }
        public string CoordinatorSalesNote_en { get; set; }
        public string CoordinatorSalesNote_jp { get; set; }
        public string CoordinatorSalesNote_kr { get; set; }
        public string CoordinatorSalesNote_cn { get; set; }
        public string CoordinatorSalesNote_fr { get; set; }
        public string CoordinatorSalesNote_tl { get; set; }

    }
}
