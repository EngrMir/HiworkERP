using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class StaffTranslationSpecialFieldsModel : BaseDomainModel<StaffTranslationSpecialFieldsModel>
    {
        public System.Guid ID { get; set; }
        public System.Guid StaffID { get; set; }
        public System.Guid TranslationSpecialFieldID { get; set; }
        public bool IsSelected { get; set; }
        public virtual StaffTranslationFieldsModel StaffTranslationFields { get; set; }
        public virtual StaffModel Staff { get; set; }
    }
}
