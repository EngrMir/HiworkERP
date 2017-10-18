using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class UserTypeModel : BaseDomainModel<UserTypeModel>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public enum USERTYPE : long
    {
        SuperAdmin = 1,
        Employee,
        Maintenance,
        Guest
    }
}
