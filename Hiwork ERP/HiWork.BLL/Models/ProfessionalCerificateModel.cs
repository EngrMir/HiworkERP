using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{     
    public class ProfessionalCerificateModel : BaseDomainModel<ProfessionalCerificateModel>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
