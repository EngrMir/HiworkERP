using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class StaffTranslationFieldsModel : BaseDomainModel<StaffTranslationFieldsModel>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsNew()
        {
            bool isNew;

            // Check whether the associated GUID has '00000' inside of it
            if (this.ID.ToString().Contains("00000"))
                isNew = true;
            else
                isNew = false;
            return isNew;
        }
    }
}


