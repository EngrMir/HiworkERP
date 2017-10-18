using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; }
       
        public string Password { get; set; }

        public long ApplicationId { get; set; }

        public string CurrentCulture { get; set; }
    }
}
