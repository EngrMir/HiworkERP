using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{     
      public class StaffKnowledgeFieldItemModel : BaseDomainModel<StaffKnowledgeFieldItemModel>
    {
        public Guid ID { get; set; }
        public Guid KnowledgeFieldID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StaffKnowledgeFieldModel SKField { get; set; }
    }
}
