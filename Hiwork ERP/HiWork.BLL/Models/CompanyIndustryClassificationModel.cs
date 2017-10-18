
using System;
using HiWork.Utils.Infrastructure;
using System.Collections.Generic;

namespace HiWork.BLL.Models
{
    public class CompanyIndustryClassificationModel : BaseDomainModel<CompanyIndustryClassificationModel>
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOther { get; set; }
        public List<CompanyIndustryClassificationItemModel> itemList { get; set; }
       
    }
}
