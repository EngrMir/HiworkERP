using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class OrderStaffAllowanceModel : BaseDomainModel<OrderStaffAllowanceModel>
    {
        public Guid ID { get; set; }
        public Guid EstimationID { get; set; }
        public int AllowanceType { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int NoOfPersons { get; set; }
        public int NoOfDays { get; set; }
        public bool IsCompleteSet { get; set; }
        public bool IsExclTax { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsMarkedForDelete { get; set; }


    }
}
