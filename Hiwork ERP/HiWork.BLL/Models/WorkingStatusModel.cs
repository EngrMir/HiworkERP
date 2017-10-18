using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{     
    public class WorkingStatusModel : BaseDomainModel<WorkingStatusModel>
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
