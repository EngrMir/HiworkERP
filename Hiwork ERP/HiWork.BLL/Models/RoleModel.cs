using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class RoleModel : BaseDomainModel<RoleModel>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public int Type { get; set; }
    }
    //public enum RoleType
    //{
    //    CEO=1,
    //    Manager=2,
    //    Public=3,
    //    Intern=4,
    //    PartTime=5,
    //    Contractual=6,
    //    Ordinary=7,
    //    Partner = 8 
    //}
}
