
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class UnitModel : BaseDomainModel<UnitModel>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        
    }
}
