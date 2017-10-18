using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class TransproInformationModel : BaseDomainModel <TransproInformationModel>
    {
        public Guid ID { get; set; }
        public Guid StaffID { get; set; }
        public   long  OverallScore { get; set; }
        public   long  InternalScore { get; set; }
        public   int  NumberOfWorks { get; set; }
        public   long  FavoritePoint { get; set; }
        public   long  CustomerEvaluationPoint { get; set; }
        public   long  PenaltyPoint { get; set; }
        public   long  NegativePoint { get; set; }
        public   DateTime  BannedPeriodFrom { get; set; }
        public   DateTime  BannedPeriodTo { get; set; }
        public string CustomerEvaluation_en { get; set; }
        public string CustomerEvaluation_jp { get; set; }
        public string CustomerEvaluation_kr { get; set; }
        public string CustomerEvaluation_cn { get; set; }
        public string CustomerEvaluation_fr { get; set; }
        public string CustomerEvaluation_tl { get; set; }
    }
}
