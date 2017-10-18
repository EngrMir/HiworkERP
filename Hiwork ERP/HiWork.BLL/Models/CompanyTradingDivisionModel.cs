
using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class CompanyTradingDivisionModel : BaseDomainModel<CompanyTradingDivisionModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
