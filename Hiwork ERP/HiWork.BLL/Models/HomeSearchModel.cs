using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.BLL;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class HomeSearchModel : BaseViewModel
    {
        public HomeSearchValueModel Search { get; set; }
    }
    public class HomeSearchValueModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
