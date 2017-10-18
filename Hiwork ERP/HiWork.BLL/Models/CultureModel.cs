using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class CultureModel: BaseDomainModel<CultureModel>
    {
       public Guid  ID {get;set;}
        public long  CountryID {get;set;}
        public string CountryName { get; set; }
        public string Code{get;set;}
        public string SystemCode{get;set;}
        public string Description{get;set;}
        public string Name { get; set; }
        public CountryModel country { get; set; } 
    }


    public class CultureViewModel
    {
        public Guid ID { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string SystemCode { get; set; }
        public string Description { get; set; }
    }
}
