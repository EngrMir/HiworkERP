using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class OrderStaffAllowanceDetailsModel : BaseDomainModel<OrderStaffAllowanceDetailsModel>
    {
        public Guid OrderID { get; set; }
        public int AllowanceType { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int NoOfPersons { get; set; }
        public int NoOfDays { get; set; }
        public bool IsCompleteSet { get; set; }
        public bool IsExclTax { get; set; }
        public decimal SubTotal { get; set; }
    }
}
