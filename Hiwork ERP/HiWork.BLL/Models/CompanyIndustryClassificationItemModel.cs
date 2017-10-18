
using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class CompanyIndustryClassificationItemModel : BaseDomainModel<CompanyIndustryClassificationItemModel>
    {
        public Guid Id { get; set; }
        public Guid IndustryClassificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CompanyIndustryClassificationModel CompanyIndustry { get; set; }
    }
}
