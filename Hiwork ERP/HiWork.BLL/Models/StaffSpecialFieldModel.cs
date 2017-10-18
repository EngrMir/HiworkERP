using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class StaffSpecialFieldModel : BaseDomainModel<StaffSpecialFieldModel>
    {
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
    }
}