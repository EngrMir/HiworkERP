
using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class LanguageQualificationModel : BaseDomainModel<LanguageQualificationModel>
    {
        public Guid Id {get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
