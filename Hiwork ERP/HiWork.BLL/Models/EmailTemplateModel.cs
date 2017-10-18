using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class EmailTemplateModel : BaseDomainModel<EmailTemplateModel>
    {
        public long ID { get; set; }
        public long GroupID { get; set; }
        public string Name { get; set; }
        public long? ParentID { get; set; }
      
        public string Subject { get; set; }
        public string Subject_en { get; set; }
        public string Body { get; set; }
        public string Body_en { get; set; }

    }
}
