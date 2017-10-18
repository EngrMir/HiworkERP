using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class  NoticeModel: BaseDomainModel<NoticeModel>
    {
        public Guid ID { get; set; }
        public long NoticeNo { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public string NoticeURL { get; set; }
        public int? ClientDisplayStatus { get; set; }
        public string ClientDisplayStatusName { get; set; }
        public int? StaffDisplayStatus { get; set; }
        public string StaffDisplayStatusName { get; set; }
        public int? PartnerDisplayStatus { get; set; }
        public string PartnerDisplayStatusName { get; set; }
        public string Title { get; set; }
        public string Title_en { get; set; }
        public string Title_jp { get; set; }
        public string Title_kr { get; set; }
        public string Title_cn { get; set; }
        public string Title_fr { get; set; }
        public string Title_tl { get; set; }
        public string Description { get; set; }
     
      
    



    }
}
