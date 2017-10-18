using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
  public class StaffSoftwareSkillModel: BaseDomainModel<StaffSoftwareSkillModel>
    {
        public Guid ID { get; set; }
        public Guid StaffID { get; set; }
        public StaffModel Staff { get; set; }
        public long StaffSoftwareSkillID { get; set; }
        public MasterStaffSoftwareSkillModel MasterStaffSoftwareSkill { get; set; }
        public bool IsSelected { get; set; }
        public int ApplicationID { get; set; }

    }
}
