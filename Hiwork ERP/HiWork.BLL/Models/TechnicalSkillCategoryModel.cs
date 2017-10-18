

/* ******************************************************************************************************************
 * Data Model for Master_StaffTechnicalSkillCategory Entity
 * Date             :   08-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System;
using HiWork.Utils.Infrastructure;

namespace HiWork.BLL.Models
{
    public class TechnicalSkillCategoryModel : BaseDomainModel<TechnicalSkillCategoryModel>
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
    }
}
