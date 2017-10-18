using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Models
{
    public class EmployeeModel : BaseDomainModel<EmployeeModel>
    {
        public Guid ID { get; set; }
        public long RegistrationID { get; set; }
        public string EmployeeID { get; set; }
        //public Guid DivisionID { get; set; }
        //public DivisionModel division { get; set; }
        //public string DivisionName { get; set; }
        public long CountryID { get; set; }
        public CountryModel country { get; set; }
        public string CountryName { get; set; }
        public Guid DepartmentID { get; set; }
        public DepartmentModel department { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.Guid> BranchOfficeID { get; set; }
        public string BranchName { get; set; }
        //public Guid TeamID { get; set; }
        //public TeamModel team { get; set; }
        //public string TeamName { get; set; }
        public long EmployeeTypeID { get; set; }
        public RoleModel EmployeeType { get; set; } 
        public string EmployeeTypeName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public int AttendanceDay { get; set; }
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }

        public string SkypeID { get; set; }
        public string SkypePassword { get; set; }
      //  [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public bool IsResponsiblePerson { get; set; }
        public string HomeAddress { get; set; }
        public string HomeAddress_en { get; set; }
        public string HomeAddress_jp { get; set; }
        public string HomeAddress_kr { get; set; }
        public string HomeAddress_cn { get; set; }
        public string HomeAddress_fr { get; set; }
        public string HomeAddress_tl { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string PCEmail { get; set; }
        public string MobileEmail { get; set; }
        public string Sns_one { get; set; }
        public string Sns_two { get; set; }
        public Guid BankID { get; set; }
        public BankModel bank { get; set; }
        public string BankName { get; set; }
        public Guid BankBranchID { get; set; }
        public BankBranchModel bankbranch { get; set; }
        public string BankBranchName { get; set; }
        public long BankAccountTypeID { get; set; }
        public BankAccountTypeModel bankAccountType { get; set; }
        public string BankAccountTypeName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string  Photo { get; set; }
        public string Signature { get; set; }
        public string Institute { get; set; }
        public string Institute_en { get; set; }
        public string Institute_jp { get; set; }
        public string Institute_kr { get; set; }
        public string Institute_cn { get; set; }
        public string Institute_fr { get; set; }
        public string Institute_tl { get; set; }
        public string AcademicQualification { get; set; }
        public LanguageModel language { get; set; }
        public Guid Language_one { get; set; }
        public Guid Language_two { get; set; }
        public Guid Language_three { get; set; }
        public string LanguageName1 { get; set; }
        public string LanguageName2 { get; set; }
        public string LanguageName3 { get; set; }
        public string SelfIntroduction { get; set; }
        public string SelfIntroduction_en { get; set; }
        public string SelfIntroduction_jp { get; set; }
        public string SelfIntroduction_kr { get; set; }
        public string SelfIntroduction_fr { get; set; }
        public string SelfIntroduction_tl { get; set; }
        public string SelfIntroduction_cn { get; set; }
        public string Note { get; set; }
        public string Note_en { get; set; }
        public string Note_jp { get; set; }
        public string Note_kr { get; set; }
        public string Note_fr { get; set; }
        public string Note_tl { get; set; }
        public string Note_cn { get; set; }
       

    }

    public class EmployeeFormModel
    {
        public List<CountryModel> countries { get; set; }
       // public List<DivisionModel> divisions { get; set; }
        public List<DepartmentModel> departments { get; set; }
        public List<BranchModel> branches { get; set; }
        public List<RoleModel> roles { get; set; }
        public List<BankModel> banks { get; set; }
        public List<BankBranchModel> bankbranches { get; set; }
        public List<BankAccountTypeModel> bankAccountTypes { get; set; }
        public List<LanguageModel> languages { get; set; }
        public string EmployeeNo { get; set; }
    }

}
