
using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class StaffDevelopmentSkillModel : BaseDomainModel<StaffDevelopmentSkillModel>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
