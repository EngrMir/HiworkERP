

/* ******************************************************************************************************************
 * Data Model for Master_StaffTechnicalSkillItems Entity
 * Date             :   09-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class TechnicalSkillItemsModel : BaseDomainModel<TechnicalSkillItemsModel>
    {
        public Guid Id { get; set; }
        public Guid TechnicalSkillCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TechnicalSkillCategoryModel TSCategory { get; set; }
    }
}
