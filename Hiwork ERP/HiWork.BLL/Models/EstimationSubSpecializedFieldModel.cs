using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class EstimationSubSpecializedFieldModel :BaseDomainModel<EstimationSubSpecializedFieldModel>
    {
        public Guid ID { get; set; }
        public Guid SpecializedField { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        
        public EstimationSpecializedFieldModel EstimationSpecializedField { get; set; }
        public bool IsNew()
        {
            bool isNew;
            if (this.ID.ToString().Contains("0000"))
                isNew = true;
            else
                isNew = false;
            return isNew;
        }

    }
}
