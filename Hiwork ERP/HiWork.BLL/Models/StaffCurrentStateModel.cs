using System;
using HiWork.Utils.Infrastructure;
namespace HiWork.BLL.Models
{
    public class StaffCurrentStateModel :BaseDomainModel<StaffCurrentStateModel>
    {
        public long ID { get; set; }
        public Guid StaffID { get; set; }
        public Guid CurrentStateID { get; set; }
        public bool IsSelected { get; set; }
    }
}
