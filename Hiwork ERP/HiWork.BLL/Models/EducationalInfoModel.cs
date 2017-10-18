using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class EducationHistoryModel 
    {
        public string Culture { get; set; }
        public long CurrentUserID { get; set; }
        public long ApplicationID { get; set; }
        public List<Staff_EducationalHistory> EducationalInformation { get; set; }

    }
}
