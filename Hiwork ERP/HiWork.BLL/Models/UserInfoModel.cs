using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class UserInfoModel : BaseDomainModel<UserInfoModel>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long UserTypeId { get; set; }
        public long RoleId { get; set; }
        public Guid EmployeeID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<int> PasswordAgeLimit { get; set; }
        public bool IsPasswordChanged { get; set; }
        public Nullable<bool> HasMultipleRole { get; set; }
        public bool IsLocked { get; set; }
        public Nullable<System.DateTime> LastPasswordChangedDate { get; set; }
        public Nullable<System.DateTime> LastLockedDate { get; set; }
        public Nullable<int> WrongPasswordTryLimit { get; set; }
        public bool IsSuperAdmin { get; set; }      
        public Nullable<System.Guid> Session { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
