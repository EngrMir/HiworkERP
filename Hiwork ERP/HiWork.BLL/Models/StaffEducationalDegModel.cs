using System;
using HiWork.Utils.Infrastructure;
using System.Collections.Generic;

namespace HiWork.BLL.Models
{
  public class StaffEducationalDegModel : BaseDomainModel<StaffEducationalDegModel>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
