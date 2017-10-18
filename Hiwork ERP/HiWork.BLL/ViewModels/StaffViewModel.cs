using HiWork.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.ViewModels
{
    public class StaffViewModel
    {
        public System.Guid ID { get; set; }
        public long TranslatorNo { get; set; }
        public string StaffEmailID { get; set; }
        public int StaffTypeID { get; set; }
        public Nullable<long> ApplicationID { get; set; }
        public System.DateTime? RegistrationDate { get; set; }
        public string RegisteredFrom { get; set; }     
        public string NickName { get; set; }    
        public string Surname { get; set; }
        public string FirstName { get; set; }  
        public string MiddleName { get; set; }     
        public string LastName { get; set; }     
        public string FullName { get; set; }
        public bool Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public CountryModel CountryOfCitizenship { get; set; }
        public Nullable<long> NationalityID { get; set; }
        public string NationalityName { get; set; }
        public string PostalCode { get; set; }
        public bool IsJapanese { get; set; }
        public Nullable<long> LivingCountryID { get; set; }
        public string CityOfOverseas { get; set; }     
        public System.Guid NativeLanguageID { get; set; }   
        public string NativeLanguageName { get; set; }
        public System.Guid ForiegnLanguage1ID { get; set; }
        public string ForeignLanguage1Name { get; set; }
        public Nullable<System.Guid> ForiegnLanguage2ID { get; set; }
        public string ForeignLanguage2Name { get; set; }
        public Nullable<System.Guid> ForiegnLanguage3ID { get; set; }
        public string ForeignLanguage3Name { get; set; }
        public Nullable<System.Guid> ForiegnLanguage4ID { get; set; }
        public string ForeignLanguage4Name { get; set; }
        public string ForeignLang1Level { get; set; }
        public string ForeignLang2Level { get; set; }
        public string ForeignLang3Level { get; set; }
        public string ForeignLang4Level { get; set; }

        public string OtherLanguage { get; set; }
        public string OtherLangQualification { get; set; }
        public System.Guid? VisaTypeID { get; set; }
        public string  VistaTypeName { get; set; }
        public Nullable<System.DateTime> VisaDeadLine { get; set; }
        public string District { get; set; }
        public string TownName { get; set; }      
        public string Address { get; set; }
     
        public string ApartmentName { get; set; }
    
        public string ApartmentNo { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string HomeCountryPhone { get; set; }
        public string Fax { get; set; }
        public string HomeCountryAddress { get; set; }
      
        public Nullable<bool> HasSocialExperienceJapan { get; set; }
        public Nullable<bool> HasSocialExperienceOtherCountry { get; set; }
        public string Image { get; set; }
        public string Street { get; set; }
   
        public Nullable<System.Guid> EducationalDegree1 { get; set; }
        public string EducationDegree1Name { get; set; }
        public Nullable<System.Guid> EducationalDegree2 { get; set; }
        public string EducationDegree2Name { get; set; }
        public Nullable<System.Guid> EducationalDegree3 { get; set; }
        public string EducationDegree3Name { get; set; }
        public string MainCareer { get; set; }    
        public string SelfPR { get; set; }
   
        //public string BankName { get; set; }
        //public string BankBranchName { get; set; }
        //public string BankAccountNumber { get; set; }
        //public string BankAccountName { get; set; }
        //public string BankAccountType { get; set; }
        public List<ProfessionalSpecialityViewModel> ProfessionSpecialities { get; set; }
        public List<CurrentStateViewModel> CurrentStates { get; set; }
        public List<SoftwareUseViewModel> SoftwareUses { get; set; }
        public long OnlineTranslation { get; set; }
        public long AppointedTranslation { get; set; }
        public long NativeTranslation { get; set; }
        public long AllTranslation { get; set; }
    }

    public class ProfessionalSpecialityViewModel
    {
        public Guid StaffID { get; set; }
        public long ID { get; set; }
        public string Name { get; set; }
    }

    public class SoftwareUseViewModel
    {
        public Guid StaffID { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
    public class CurrentStateViewModel
    {
        public Guid? StaffID { get; set; }
        public Guid? ID { get; set; }
        public string Name { get; set; }
    }
    public class TranslatorFilterModel
    {
        public Guid? ID { get; set; }
        public long? TranslatorNo { get; set; } 
        public string  CultureCode { get; set; } 
    }
}
