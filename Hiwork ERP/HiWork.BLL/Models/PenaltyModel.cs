using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class PenaltyModel: BaseDomainModel<PenaltyModel>
    {
       public Guid  ID { get; set; }
        public int CategoryNo { get; set; }
       public string CategoryName { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public string Contents_en { get; set; }
        public string Contents_jp { get; set; }
        public string Contents_kr { get; set; }
        public string Contents_cn { get; set; }
        public string Contents_tl { get; set; }
        public string Contents_fr { get; set; }
        public string Response { get; set; }
        public string Response_en { get; set; }
        public string Response_jp { get; set; }
        public string Response_kr { get; set; }
        public string Response_cn { get; set; }
        public string Response_tl { get; set; }
        public string Response_fr { get; set; }
        public string ApplicationName { get; set; }
        public ApplicationModel Application { get; set; }

    }

    public class penaltyConfigData
    {
        public List<ApplicationModel> ApplicationList { get; set; }
        public List<SelectedItem> PenaltyCategoryList { get; set; }
    }
}
