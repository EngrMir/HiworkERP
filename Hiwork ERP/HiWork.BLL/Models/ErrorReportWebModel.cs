using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class ErrorReportWebModel:BaseDomainModel<ErrorReportWebModel>
    {
        public long ID { get; set; }
        public  long ApplicationID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ErrorPageUrl { get; set; }
        public string ErrorMessage { get; set; }
        public string OsName { get; set; }
        public string BrowserVersion { get; set; }
        public string ErrorDescription { get; set; }
        public string Reproducibility { get; set; }
       
    }
}
