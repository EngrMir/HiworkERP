using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class StaffProfesionalSpecialityModel: BaseDomainModel<StaffProfesionalSpecialityModel>
    {
        public long ID { get; set; }
        public Guid? StaffID { get; set; }
        public Guid? ProfessionalID { get; set; }
        public bool IsSelected { get; set; }
    }
}
