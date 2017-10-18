using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.Utils.Infrastructure;
namespace HiWork.BLL.Models
{
    public class UserModel:BaseDomainModel<UserModel>
    {

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<int> PasswordAgeLimit { get; set; }
        public bool IsPasswordChanged { get; set; }
        public bool IsLocked { get; set; }
        public Nullable<System.DateTime> LastPasswordChangedDate { get; set; }
        public Nullable<System.DateTime> LastLockedDate { get; set; }
        public Nullable<int> WrongPasswordTryLimit { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string FullName { get; set; }
        public RoleModel Role { get; set; }
        public UserTypeModel UserType { get; set; }
        public string Branch { get; set; }
        public string Team { get; set; }
    }
}
