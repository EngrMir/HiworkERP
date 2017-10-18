using HiWork.Utils.Infrastructure;
using System;

namespace HiWork.BLL.Models
{
    public class TransproDeliveryPlanSettingModel : BaseDomainModel<TransproDeliveryPlanSettingModel>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DeliveryType { get; set; }
        public Nullable<int> DeliveryTime { get; set; }
    }
}
