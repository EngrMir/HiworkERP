using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class CurrencyModel : BaseDomainModel<CurrencyModel>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public bool Active { get; set; }

        public static implicit operator CurrencyModel(string v)
        {
            throw new NotImplementedException();
        }
    }
}
