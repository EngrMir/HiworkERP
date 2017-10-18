using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string CultureCode { get; set; }
        public long UserID { get; set; }
        public int UserType { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; } 
    }
}
