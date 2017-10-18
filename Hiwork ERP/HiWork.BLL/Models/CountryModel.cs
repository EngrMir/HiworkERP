using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class CountryModel: BaseDomainModel<CountryModel>
    {
        public long ID { get; set; }
        public long CurrencyID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } 
        public bool IsTrading { get; set; }
        public CurrencyModel CurrencyM { get; set; }


        public static implicit operator CountryModel(string v)
        {
            throw new NotImplementedException();
        }
    }

}
