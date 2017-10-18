using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class CountryToBranchModel: BaseDomainModel<CountryToBranchModel>
    {
        public Guid ID { get; set; }
        public long CountryID { get; set; }
        public Guid BranchID { get; set; }

    }
}
