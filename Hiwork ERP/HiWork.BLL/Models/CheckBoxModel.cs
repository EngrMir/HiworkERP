using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class CheckBoxModel: BaseDomainModel<CheckBoxModel>
    {
        public List<EstimationSpecializedFieldModel> SpecializedFieldList { get; set; }
        public List<CurrentStateModel> CurrentStateList { get; set; }
        public List<MasterStaffSoftwareSkillModel> SoftwareSkillList { get; set; }
        

    }
}
