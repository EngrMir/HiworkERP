using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{     
    public class EducationalHistoryModel : BaseDomainModel<EducationalHistoryModel>
    {
        public Guid Id { get; set; }
        public Guid StaffID { get; set; }
        public StaffModel Staff { get; set; }
        public Guid DegreeID { get; set; }
        public EducationModel Education { get; set; }
        public Guid MajorSubjectID { get; set; }
        public MajorSubjectModel MajorSubject { get; set; }
        public long CountryId { get; set; }
        public CountryModel Country { get; set; }
        public string InstituteName { get; set; }
        public string Comment { get; set; }
        public System.DateTime EntryYear { get; set; }
        public System.DateTime GraduationYear { get; set; }
    }
}
