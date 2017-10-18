using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class DeliveryMethodModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name_en { get; set; }
        public string Name_jp { get; set; }
        public string Name_kr { get; set; }
        public string Name_cn { get; set; }
        public string Name_fr { get; set; }
        public string Name_tl { get; set; }
        public int? Type { get; set; }
        public bool IsDeleted { get;  set; }
        public bool IsActive { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
