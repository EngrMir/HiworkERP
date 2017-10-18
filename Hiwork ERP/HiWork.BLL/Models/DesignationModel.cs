using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
   public class DesignationModel : BaseDomainModel<DesignationModel>
    {
      public Guid ID { get; set; }
      public long CountryID { get; set; }
      public Guid BranchID { get; set; }
      public Guid DivisionID { get; set; }
      public Guid DepartmentID { get; set; }
      public Guid  TeamID { get; set; }
      public string Code { get; set; }
      public string Name { get; set; }
      public CountryModel countrymodel { get; set; }
      public BranchModel branchmodel { get; set; }
      public DivisionModel divisionmodel { get; set; }
      public DepartmentModel departmentmodel { get; set; }
      public TeamModel teammodel { get; set; }

    }

    public class DesignationFormModel
    {
        public List<CountryModel> countryList { get; set; }
        public List<BranchModel> branchList { get; set; }
        public List<DivisionModel> divisionList { get; set; }
        public List<DepartmentModel> departmentList { get; set; }
        public List<TeamModel> teamList { get; set; }
    }
}
