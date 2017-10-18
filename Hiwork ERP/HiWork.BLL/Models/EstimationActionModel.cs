
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HiWork.BLL.Models
{
    public class EstimationActionModel : BaseDomainModel<EstimationActionModel>
    {
        public Guid ID { get; set; }
        public Guid EstimationID { get; set; }
        public DateTime? NextActionDate { get; set; }
        public string ActionDetails { get; set; }
        public string ActionDetails_en { get; set; }
        public string ActionDetails_jp { get; set; }
        public string ActionDetails_kr { get; set; }
        public string ActionDetails_cn { get; set; }
        public string ActionDetails_fr { get; set; }
        public string ActionDetails_tl { get; set; }
        public int? ActionType { get; set; }
        public long? OperationBy { get; set; }
        public string OperandName { get; set; }
        public DateTime? OperationDate { get; set; }
    }
}
