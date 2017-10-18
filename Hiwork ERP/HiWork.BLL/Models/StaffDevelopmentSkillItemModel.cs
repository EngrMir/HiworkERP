
using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class StaffDevelopmentSkillItemModel : BaseDomainModel<StaffDevelopmentSkillItemModel>
    {
        public Guid ID { get; set; }
        public Guid DevelopmentSkillID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StaffDevelopmentSkillModel DevSkillModel { get; set; }
    }
}
