using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;

namespace HiWork.BLL.Models
{
    public class NotificationModel : BaseDomainModel<WorkingStatusModel>
    {
        public Guid ID { get; set; }
        public long ApplicationID { get; set; }
        public Guid? ApproverID { get; set; }
        public Guid? EstimationID { get; set; }
        public Guid? OrderID { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public int? EstimationType { get; set; }
    }
}