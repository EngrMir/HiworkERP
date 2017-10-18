using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class EstimationRoleAccessModel
    {
        public long UserID { get; set; }
        public int EstimationTypeID { get; set; }
        public int EstimationStatusID { get; set; }
        public string Options { get; set; }
    }
}
