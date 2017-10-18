using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class EstimationTypeModel :BaseDomainModel<EstimationTypeModel>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }       
        public bool IsNew()
        {
            bool isNew;

            // Check whether the associated GUID has '00000' inside of it
            if (this.ID <= 0)
                isNew = true;
            else
                isNew = false;
            return isNew;
        }
    }
}
