using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class DepartmentModel: BaseDomainModel<DepartmentModel>
    {
        public Guid  ID { get; set; }
       // public long CountryID { get; set; }
        //public Guid BranchID { get; set; }
        //public Guid DivisionID { get; set; }

        public CountryModel Country { get; set; }
        public BranchModel Branch { get; set; }
        //public DivisionModel Division { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

    }

    public class DepartmentFormModel
    {
        public List<CountryModel> countryList { get; set; }
        public List<BranchModel> branchList { get; set; }
        public List<DivisionModel> divisionList { get; set; }
    }
}
