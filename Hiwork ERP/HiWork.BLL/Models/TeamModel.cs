
using System;
using HiWork.Utils.Infrastructure;
using System.Collections.Generic;

namespace HiWork.BLL.Models
{
    public class TeamModel : BaseDomainModel<TeamModel>
    {
        public Guid Id { get; set; }                // Primary key
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long CountryId { get; set; }
        public Guid BranchId { get; set; }
        public Guid DivisionId { get; set; }
        public Guid DepartmentId { get; set; }
        public CountryModel Country { get; set; }
        public BranchModel Branch { get; set; }
        public DivisionModel Division { get; set; }
        public DepartmentModel Department { get; set; }
   
    }

    public struct TeamFormModel
    {
        public List<DivisionModel> divisionList;
        public List<CountryModel> countryList;
        public List<BranchModel> branchList;
        public List<DepartmentModel> departmentList;
    }
}
