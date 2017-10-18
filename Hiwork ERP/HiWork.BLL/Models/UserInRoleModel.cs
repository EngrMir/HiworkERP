using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    class UserInRoleModel: BaseDomainModel<UserInRoleModel>
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }
    }
}
