using System;
using HiWork.Utils.Infrastructure;
using System.Collections.Generic;


namespace HiWork.BLL.Models
{
    public class StaffMejorSubModel : BaseDomainModel<StaffMejorSubModel>
    {
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
