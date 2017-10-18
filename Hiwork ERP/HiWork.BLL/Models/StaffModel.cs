
using System;
using HiWork.Utils.Infrastructure;
using System.Collections.Generic;

namespace HiWork.BLL.Models
{
    public class StaffModel : BaseDomainModel<StaffModel>
    {
        public System.Guid ID { get; set; }
        public long RegistrationID { get; set; }
        public string StaffEmailID { get; set; }
        public Nullable<long> ApplicationID { get; set; }
        public int StaffTypeID { get; set; }
        public System.DateTime? RegistrationDate { get; set; }
        public string RegisteredFrom { get; set; }
        public string Password { get; set; }
       
        public string Name { get; set; }
        public string NickName { get; set; }
        public string NickName_en { get; set; }
        public string NickName_jp { get; set; }
        public string NickName_kr { get; set; }
        public string NickName_cn { get; set; }
        public string NickName_fr { get; set; }
        public string NickName_tl { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string FirstName_en { get; set; }
        public string FirstName_jp { get; set; }
        public string FirstName_kr { get; set; }
        public string FirstName_cn { get; set; }
        public string FirstName_fr { get; set; }
        public string FirstName_tl { get; set; }
        public string MiddleName { get; set; }
        public string MiddleName_en { get; set; }
        public string MiddleName_jp { get; set; }
        public string MiddleName_kr { get; set; }
        public string MiddleName_cn { get; set; }
        public string MiddleName_fr { get; set; }
        public string MiddleName_tl { get; set; }
        public string LastName { get; set; }
        public string LastName_en { get; set; }
        public string LastName_jp { get; set; }
        public string LastName_kr { get; set; }
        public string LastName_cn { get; set; }
        public string LastName_fr { get; set; }
        public string LastName_tl { get; set; }
       // public string Name { get; set; }
       public string StaffNo { get; set; }
        public bool Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public long CountryOfCitizenship { get; set; }
        public string CountryOfCitizenshipName { get; set; }
        public string PostalCode { get; set; }
        public bool IsJapanese { get; set; }
        public Nullable<long> LivingCountryID { get; set; }
        public string CityOfOverseas { get; set; }
        public string CityOfOverseas_en { get; set; }
        public string CityOfOverseas_jp { get; set; }
        public string CityOfOverseas_kr { get; set; }
        public string CityOfOverseas_cn { get; set; }
        public string CityOfOverseas_fr { get; set; }
        public string CityOfOverseas_tl { get; set; }
        public System.Guid NativeLanguageID { get; set; }
        public string NativeLanguageName { get; set; }
        public System.Guid ForiegnLanguage1ID { get; set; }
        public Nullable<System.Guid> ForiegnLanguage2ID { get; set; }
        public Nullable<System.Guid> ForiegnLanguage3ID { get; set; }
        public Nullable<System.Guid> ForiegnLanguage4ID { get; set; }
        public Nullable<int> ForeignLang1Level { get; set; }
        public Nullable<int> ForeignLang2Level { get; set; }
        public Nullable<int> ForeignLang3Level { get; set; }
        public Nullable<int> ForeignLang4Level { get; set; }
        public Nullable<System.Guid> TranslationExpID { get; set; }
        public string OtherLanguage { get; set; }
        public string OtherLangQualification { get; set; }
        public System.Guid? VisaTypeID { get; set; }
        public Nullable<System.DateTime> VisaDeadLine { get; set; }
        public string District { get; set; }
        public string District_en { get; set; }
        public string District_jp { get; set; }
        public string District_kr { get; set; }
        public string District_cn { get; set; }
        public string District_fr { get; set; }
        public string District_tl { get; set; }
        public string TownName{ get; set; }
        public string TownName_en { get; set; }
        public string TownName_jp { get; set; }
        public string TownName_kr { get; set; }
        public string TownName_cn { get; set; }
        public string TownName_fr { get; set; }
        public string TownName_tl { get; set; }
        public string Address { get; set; }
        public string Address_en { get; set; }
        public string Address_jp { get; set; }
        public string Address_kr { get; set; }
        public string Address_cn { get; set; }
        public string Address_fr { get; set; }
        public string Address_tl { get; set; }
        public string ApartmentName{ get; set; }
        public string ApartmentName_en { get; set; }
        public string ApartmentName_jp { get; set; }
        public string ApartmentName_kr { get; set; }
        public string ApartmentName_cn { get; set; }
        public string ApartmentName_fr { get; set; }
        public string ApartmentName_tl { get; set; }
        public string ApartmentNo { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string HomeCountryPhone { get; set; }
        public string Fax { get; set; }
        public string HomeCountryAddress { get; set; }
        public string HomeCountryAddress_en { get; set; }
        public string HomeCountryAddress_jp { get; set; }
        public string HomeCountryAddress_kr { get; set; }
        public string HomeCountryAddress_cn { get; set; }
        public string HomeCountryAddress_fr { get; set; }
        public string HomeCountryAddress_tl { get; set; }
        public Nullable<bool> HasSocialExperienceJapan { get; set; }
        public Nullable<bool> HasSocialExperienceOtherCountry { get; set; }
        public string Image { get; set; }
        public string Street { get; set; }
        public string Street_en { get; set; }
        public string Street_jp { get; set; }
        public string Street_kr { get; set; }
        public string Street_cn { get; set; }
        public string Street_fr { get; set; }
        public string Street_tl { get; set; }
        public Nullable<System.Guid> EducationalDegree1 { get; set; }
        public Nullable<System.Guid> EducationalDegree2 { get; set; }
        public Nullable<System.Guid> EducationalDegree3 { get; set; }
        public string MainCareer{ get; set; }
        public string MainCareer_en { get; set; }
        public string MainCareer_jp { get; set; }
        public string MainCareer_kr { get; set; }
        public string MainCareer_cn { get; set; }
        public string MainCareer_fr { get; set; }
        public string MainCareer_tl { get; set; }
        public string SelfPR { get; set; }
        public string SelfPR_en { get; set; }
        public string SelfPR_jp { get; set; }
        public string SelfPR_kr { get; set; }
        public string SelfPR_cn { get; set; }
        public string SelfPR_fr { get; set; }
        public string SelfPR_tl { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountType { get; set; }
        public string SelfVideoURL { get; set; }
        public  StaffBankAccountInfoModel Staffbank{get; set;}
        public List<StaffSoftwareSkillModel> staffsoft { get; set; }
        public List<StaffProfesionalSpecialityModel> StaffProfessional { get; set; }
        public List<StaffCurrentStateModel> StaffCurrentState { get; set; }


    }

}
