


using HiWork.Utils.Infrastructure;
using System;

namespace HiWork.BLL.Models
{
    public class EstimationSpecializedFieldModel : BaseDomainModel<EstimationSpecializedFieldModel>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
