
using HiWork.Utils.Infrastructure;
using System;

namespace HiWork.BLL.Models
{
    public class DivisionModel : BaseDomainModel<DivisionModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long CountryId { get; set; }
        public CountryModel Country { get; set; }
        public int Type { get; set; }

    }
}
