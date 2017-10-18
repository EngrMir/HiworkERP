using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure.Contract;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;
using System;
using HiWork.DAL.Repositories;
using AutoMapper;
using HiWork.Utils;
using HiWork.BLL.ViewModels;
using System.Data.SqlClient;
using System.Data;

namespace HiWork.BLL.Services
{
    public partial interface ITransproService
    {
        bool RegisterCustomer(CompanyModel model);
        CustomerUserModel CustomerLogin(string email, string password, string culture);
        StaffModel RegisterTranslator(StaffModel model);
        CompanyModel GetCustomerById(string customerId,string cultureId);
        TranslatorUserModel TranlatorLogin(string email, string password, string culture);
        CustomerUserModel PartnerLogin(string email, string password, string culture);
        StaffModel GetTranslatorById(string translatorId, string cultureId);
        bool CheckExistingCustomer(string emailId,string culture);
        List<StaffViewModel> SearchTranslators(BaseViewModel model, Guid? srcLanguageID, Guid? targetLanguageID, Guid? specialFieldID);
		bool CheckExistingTranslator(string emailId, string culture);
        bool ResetPassword(string email, string type, string culturId);
        bool SetNewPassword(ResetPassword model);
        bool ChangePhoto(Guid staffId, string Photo);
        StaffModel GetImageById(string translatorId, string cultureId);
        bool CheckTranslatorPassword(string email, string password);
        bool ChangePassword(Guid staffId, string password);
        StaffViewModel GetTranslatorProfile(TranslatorFilterModel model);
        bool WithDrawTranslatorMembership(StaffModel model);
        CheckBoxModel GetMasterData(BaseViewModel model);
        List<MessageModel> getReceiveMsgById(Guid receiverID);
        List<MessageModel> getSendMsgById(Guid senderID);
        List<MessageModel> getDetailsbymsgId(long msgId);

    }
    public class TransproService : ITransproService
    {

        private readonly ISqlConnectionService _sqlConnService;
        public TransproService()
        {
            _sqlConnService = new SqlConnectionService();
        }

        public bool RegisterCustomer(CompanyModel model)
        {
            bool isRegistered = false;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                ICompanyRepository rep = new CompanyRepository(ouw);
                ICompanyService service = new CompanyService(rep);
                if (model.ID == Guid.Empty)
                {
                    model.ID = Guid.NewGuid();
                    model.RegistrationNo = service.GetNextCompanyNumber(new BaseViewModel() { ApplicationId = model.ApplicationId });
                    model.Password = Utility.MD5(model.Password);
                    model.CreatedDate = DateTime.Now;
                    if (model.RegistrationType == (int)CompanyRegistrationType.Individual)
                    {
                        model.Name_Local = model.Name;
                        model.Name_Global = model.Name;
                    }
                    Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                    var map = Mapper.Map<CompanyModel, Company>(model);
                    rep.SaveCompany(map);
                }
                else
                {
                    Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                    var map = Mapper.Map<CompanyModel, Company>(model);
                    rep.UpdateCompany(map);
                }

                if (model.IndustryClassifications!=null && model.IndustryClassifications.Count > 0)
                {
                    model.IndustryClassifications.ForEach(f =>
                    {
                        f.CompanyID = model.ID;
                    });
                    service.InsertIndustryClassifications(model.IndustryClassifications);
                }
                              
            }
            catch(Exception ex)
            {
                isRegistered = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
               // errorLog.SetErrorLog(model.CurrentUserID, "CustomerRegistrationFromTranspro", message);
                throw new Exception(ex.Message);
            }
            return isRegistered;
        }
        public CustomerUserModel CustomerLogin(string email,string password,string culture)
        {
            CustomerUserModel model = null;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                ICompanyRepository rep = new CompanyRepository(ouw);
                ICompanyService service = new CompanyService(rep);
                var data = service.GetCustomerByUser(email, password,culture,false);

                if (data != null)
                {
                    model = new CustomerUserModel();
                    model.ID = data.ID;
                    model.RegistrationID = data.RegistrationID;
                    model.RegistrationNo = data.RegistrationNo;
                    model.Email = data.ClientID;
                    model.Name = data.Name;
                    model.UserType = (int)TransproUserType.Customer;
                    model.IsAuthenticated = true;
                    return model;
                }

            }
            catch(Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
               // errorLog.SetErrorLog(model.CurrentUserID, "Customer Login Error for transprologin", message);
                throw new Exception(ex.Message);
            }
            return model;
        }
        public CompanyModel GetCustomerById(string customerId,string culturId)
        {
            CompanyModel _model = null;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                ICompanyRepository rep = new CompanyRepository(ouw);
                ICompanyService service = new CompanyService(rep);
                var data = service.GetCustomerByID(new BaseViewModel() { ID = Guid.Parse(customerId),CurrentCulture = culturId});
                if (data != null)
                    _model = data;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _model;
        }
        public bool ResetPassword(string email,string type,string culturId)
        {
            IUnitOfWork ouw = new UnitOfWork();
            try
            {
                string templateName = string.Format("passwordTemplate_{0}.html", culturId);
                if (Convert.ToInt32(type) == (int)TransproUserType.Customer || Convert.ToInt32(type) == (int)TransproUserType.Partner)
                {
                    ICompanyRepository rep = new CompanyRepository(ouw);
                    ICompanyService service = new CompanyService(rep);                    
                    var data = service.GetCustomerByEmail(email);
                    if (data != null)
                    {
                        EmailService emailSvc = new EmailService();
                        
                        emailSvc.SendResetPasswordMail(new EmailModel() { UserID = data.RegistrationID, EmailTo = email, Name = data.Name }, type, culturId, templateName);
                    }
                }
                else if (Convert.ToInt32(type) == (int)TransproUserType.Translator)
                {
                    IStaffRepository rep = new StaffRepository(ouw);
                    IStaffService service = new StaffService(rep);
                    var data = service.GetStaffByEmail(email);
                    if (data != null)
                    {
                        EmailService emailSvc = new EmailService();
                        emailSvc.SendResetPasswordMail(new EmailModel() { UserID = data.RegistrationID, EmailTo = email }, type, culturId, templateName);
                    }
                }
                else
                {
                    throw new Exception("User not found with this Email.");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally{
                ouw.Dispose();
            }
            return true;
        }

        public CheckBoxModel GetMasterData(BaseViewModel model)
        {
            CheckBoxModel DataModel;
            IUnitOfWork uwork = new UnitOfWork();
            IEstimationSpecializedFieldService SpecializedService = new EstimationSpecializedFieldService(new EstimationSpecializedFieldRepository(uwork));
            ICurrentStateService CurrentStateService = new CurrentStateService(new CurrentStateRepository(uwork));
            IMasterStaffSoftwareSkillService SoftwareService = new MasterStaffSoftwareSkillService(new MasterStaffSoftwareSkillRepository(uwork));

            DataModel = new CheckBoxModel();
            DataModel.SpecializedFieldList = SpecializedService.GetAllEstimationSpecializedFieldList(model);
            DataModel.CurrentStateList = CurrentStateService.GetAllCurrentStateList(model);
            DataModel.SoftwareSkillList = SoftwareService.GetStaffSoftwareSkill(model);
            return DataModel;
        }

        public StaffModel RegisterTranslator(StaffModel model)
        {
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                IStaffBankAccountInfoRepository bankrep = new StaffBankAccountInfoRepository(ouw);
                IStaffBankAccountInfoService bankservice = new StaffBankAccountInfoService(bankrep);
                IStaffSoftwareRepository _staffsoftwareRepository = new StaffSoftwareSkillRepository(ouw);
                IStaffProfessioanlSpecialityRepository _spRepository = new StaffProfessioanlSpecialityRepository(ouw);
                IStaffCurrentStateRepository _cstateRepository = new StaffCurrentStateRepository(ouw);

                if (model.ID == Guid.Empty)
                {
                    model.ID = Guid.NewGuid();
                    model.Password = Utility.MD5(model.Password);
                    model.CreatedDate = DateTime.Now;
                    model.RegistrationDate = DateTime.Now;
                    Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                    var map = Mapper.Map<StaffModel, Staff>(model);
                    rep.InsertStaff(map);

                    //model.Staffbank.ID = model.ID;
                    if(model.Staffbank.ID==Guid.Empty)
                    {
                        model.Staffbank.ID = Guid.NewGuid();
                        model.Staffbank.StaffID = model.ID;
                        //
                        model.Staffbank.CurrentCulture = model.CurrentCulture;
                       
                        Utility.SetDynamicPropertyValue(model.Staffbank, model.CurrentCulture);
                        var bnk = Mapper.Map<StaffBankAccountInfoModel, Staff_BankAccountInfo>(model.Staffbank);
                        bankrep.InsertStaffBankAccountInfo(bnk);
                    }
                }
                else
                {
                    Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                    var map = Mapper.Map<StaffModel, Staff>(model);
                    var bnk = Mapper.Map<StaffBankAccountInfoModel, Staff_BankAccountInfo>(model.Staffbank);
                    rep.UpdateStaff(map);
                    bankrep.UpdateStaffBankAccountInfo(bnk);
                }

                foreach (StaffSoftwareSkillModel staff in model.staffsoft)
                {
                    staff.StaffID = model.ID;
                    var software = Mapper.Map<StaffSoftwareSkillModel, Staff_SoftwareSkill>(staff);
                    if (staff.ID == Guid.Empty)
                    {
                        software.CreatedDate = DateTime.Now;
                        software.ID = Guid.NewGuid();

                        _staffsoftwareRepository.InsertStaffSkill(software);
                    }
                    else
                    {
                        software.UpdatedDate = DateTime.Now;
                        software.UpdatedBy = staff.UpdatedBy;
                        _staffsoftwareRepository.UpdateStaffSkill(software);
                    }
                }
                foreach (StaffProfesionalSpecialityModel professional in model.StaffProfessional)
                {
                    professional.StaffID = model.ID;
                    var staffspecial = Mapper.Map<StaffProfesionalSpecialityModel, Staff_ProfessionalSpeciality>(professional);

                    if (professional.ID > 0)
                    {
                        _spRepository.UpdateStaffProfessinalSpeciality(staffspecial);
                    }
                    else
                    {
                        _spRepository.InsertStaffProfessinalSpeciality(staffspecial);
                    }
                }
                foreach (StaffCurrentStateModel state in model.StaffCurrentState)
                {
                    state.StaffID = model.ID;
                    var staffcurrentstate = Mapper.Map<StaffCurrentStateModel, Staff_CurrentStates>(state);

                    if (state.ID > 0)
                    {
                        _cstateRepository.UpdateStaffCurrentState(staffcurrentstate);
                    }
                    else
                    {
                        _cstateRepository.InsertStaffCurrentState(staffcurrentstate);
                    }
                }

            }
            catch (Exception ex)
            {
                model = null;
                //isRegistered = false;
                //IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                //errorLog.SetErrorLog(model.CurrentUserID, "TranslatorRegistrationFromTranspro", message);
                throw new Exception(ex.Message);
            }
            return model;

        }

        


        public bool CheckExistingCustomer(string emailId,string culture)
        {
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                ICompanyRepository rep = new CompanyRepository(ouw);
                ICompanyService service = new CompanyService(rep);
                var data = service.GetAllCompanyList(new BaseViewModel(){ CurrentCulture = culture }).Find(f=>f.ClientID.Trim() == emailId.Trim());
                if (data == null)
                    return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public List<StaffViewModel> SearchTranslators(BaseViewModel model, Guid? srcLanguageID, Guid? targetLanguageID, Guid? specialFieldID)
        {
            List<StaffViewModel> _translators = null;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                _translators = service.GetStaffSearchList(model, srcLanguageID, targetLanguageID, specialFieldID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _translators;
        }
        public StaffViewModel GetTranslatorProfile(TranslatorFilterModel model)
        {
            StaffViewModel stfModel = null;
            SqlCommand cmd, cmd1, cmd2,cmd3;
            SqlDataReader dataReader, dataReader1, dataReader2,dr3;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetTranslatorDetailsByID", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CultureId", model.CultureCode);
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@RegistrationID",model.TranslatorNo);

                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    stfModel = new StaffViewModel();
                    stfModel.ID = Guid.Parse(dataReader["ID"].ToString());
                    stfModel.TranslatorNo = Convert.ToInt64(dataReader["TranslatorNo"].ToString());
                    stfModel.Address = dataReader["Address"].ToString();
                    stfModel.FirstName = dataReader["FirstName"].ToString();
                    stfModel.LastName = dataReader["LastName"].ToString();
                    stfModel.MiddleName = dataReader["MiddleName"].ToString();
                    stfModel.HomeCountryAddress = dataReader["HomeCountryAddress"].ToString();
                    stfModel.ApartmentName = dataReader["ApartmentName"].ToString();
                    stfModel.SelfPR = dataReader["SelfPR"].ToString();
                    stfModel.MainCareer = dataReader["MainCareer"].ToString();
                    stfModel.Street = dataReader["Street"].ToString();
                    stfModel.TownName = dataReader["TownName"].ToString();
                    stfModel.NativeLanguageID = string.IsNullOrEmpty(dataReader["NativeLanguageID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["NativeLanguageID"].ToString());
                    stfModel.NativeLanguageName = dataReader["NativeLanguageName"].ToString();
                    stfModel.NationalityID = dataReader["NationalityID"] == DBNull.Value ? (long?)null : Convert.ToInt64(dataReader["NationalityID"].ToString());
                    stfModel.NationalityName = dataReader["NationalityName"].ToString();
                    stfModel.ForiegnLanguage1ID = string.IsNullOrEmpty(dataReader["ForiegnLanguage1ID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["ForiegnLanguage1ID"].ToString());
                    stfModel.ForiegnLanguage2ID = string.IsNullOrEmpty(dataReader["ForiegnLanguage2ID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["ForiegnLanguage2ID"].ToString());
                    stfModel.ForiegnLanguage3ID = string.IsNullOrEmpty(dataReader["ForiegnLanguage3ID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["ForiegnLanguage3ID"].ToString());
                    stfModel.ForiegnLanguage4ID = string.IsNullOrEmpty(dataReader["ForiegnLanguage4ID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["ForiegnLanguage4ID"].ToString());
                    stfModel.ForeignLanguage1Name = dataReader["ForeignLanguage1Name"].ToString();
                    stfModel.ForeignLanguage2Name = dataReader["ForeignLanguage2Name"].ToString();
                    stfModel.ForeignLanguage3Name = dataReader["ForeignLanguage3Name"].ToString();
                    stfModel.ForeignLanguage4Name = dataReader["ForeignLanguage4Name"].ToString();
                    stfModel.EducationDegree1Name = dataReader["EducationDegree1Name"].ToString();
                    stfModel.EducationDegree2Name = dataReader["EducationDegree2Name"].ToString();
                    stfModel.EducationDegree3Name = dataReader["EducationDegree3Name"].ToString();
                    stfModel.Image = dataReader["Image"].ToString();
                    stfModel.VisaTypeID = string.IsNullOrEmpty(dataReader["VisaTypeID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["VisaTypeID"].ToString());
                    stfModel.VisaDeadLine = dataReader["VisaDeadLine"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["VisaDeadLine"]);
                    stfModel.PostalCode = dataReader["PostalCode"].ToString();

                    stfModel.ProfessionSpecialities = new List<ProfessionalSpecialityViewModel>();
                    cmd1 = new SqlCommand("SP_GetProfessionalSpeciality", _sqlConnService.CreateConnection());
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@CultureId", model.CultureCode);
                    cmd1.Parameters.AddWithValue("@StaffID", stfModel.ID);

                    cmd1.CommandType = CommandType.StoredProcedure;
                    dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        ProfessionalSpecialityViewModel spc = new ProfessionalSpecialityViewModel();
                        spc.ID = Convert.ToInt64(dataReader1["ID"].ToString());
                        spc.Name = dataReader1["Name"].ToString();
                        stfModel.ProfessionSpecialities.Add(spc);
                    }

                    dataReader1.Close();
                    stfModel.SoftwareUses = new List<SoftwareUseViewModel>();
                    cmd2 = new SqlCommand("SP_GetSoftwareSkills", _sqlConnService.CreateConnection());
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@CultureId", model.CultureCode);
                    cmd2.Parameters.AddWithValue("@StaffID", stfModel.ID);

                    cmd2.CommandType = CommandType.StoredProcedure;
                    dataReader2 = cmd2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        SoftwareUseViewModel spc = new SoftwareUseViewModel();
                        spc.ID = Guid.Parse(dataReader2["ID"].ToString());
                        spc.Name = dataReader2["Name"].ToString();
                        stfModel.SoftwareUses.Add(spc);
                    }
                    dataReader2.Close();

                    cmd3 = new SqlCommand("SP_AllTranslation_History", _sqlConnService.CreateConnection());
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@ID", stfModel.ID);

                    cmd3.CommandType = CommandType.StoredProcedure;
                    dr3 = cmd3.ExecuteReader();
                    while (dr3.Read())
                    {
                        stfModel.OnlineTranslation = Convert.ToInt64(dr3["OnlineTranslation"].ToString());
                        stfModel.AppointedTranslation = Convert.ToInt64(dr3["AppointedTranslation"].ToString());
                        stfModel.NativeTranslation = Convert.ToInt64(dr3["NativeTranslation"].ToString());
                        stfModel.AllTranslation = stfModel.OnlineTranslation + stfModel.AppointedTranslation + stfModel.NativeTranslation;
                    }
                    dr3.Close();
                }
                dataReader.Close();

            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                //errorLog.SetErrorLog(model.CurrentUserID, "Staff", message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }

            return stfModel;
        }

        public TranslatorUserModel TranlatorLogin(string email, string password, string culture)
        {
            TranslatorUserModel model = null;
            SqlCommand cmd;
            SqlDataReader dataReader;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetTranslatorByEmail", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CultureId", culture);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", Utility.MD5(password));

                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    model = new TranslatorUserModel();
                    model.ID = Guid.Parse(dataReader["ID"].ToString());
                    model.RegistrationID = Convert.ToInt64(dataReader["TranslatorNo"].ToString());
                    model.Email = dataReader["StaffEmailId"].ToString();
                    model.FirstName = dataReader["FirstName"].ToString();
                    model.LastName = dataReader["LastName"].ToString();
                    model.Name = model.FirstName + " " + model.LastName;
                    model.ApartmentName = dataReader["ApartmentName"].ToString();
                    model.SelfPR = dataReader["SelfPR"].ToString();
                    model.UserType = (int)TransproUserType.Translator;
                    model.NativeLanguageID = string.IsNullOrEmpty(dataReader["NativeLanguageID"].ToString()) ? Guid.Empty : Guid.Parse(dataReader["NativeLanguageID"].ToString());
                    model.IsAuthenticated = true;
                }
                    //IUnitOfWork ouw = new UnitOfWork();
                    //IStaffRepository rep = new StaffRepository(ouw);
                    //IStaffService service = new StaffService(rep);
                    //var data = service.GetTranslatorByUser(email, password, culture);

                    //if (data != null)
                    //{
                    //    model = new TranslatorUserModel();
                    //    model.ID = data.ID;
                    //    model.RegistrationID = data.RegistrationID;
                    //    //model.RegistrationNo = data.RegistrationNo;
                    //    model.Email = data.StaffEmailID;
                    //    //model.Name = data.FirstName;
                    //    model.Name = data.FirstName;
                    //    model.UserType = (int)TransproUserType.Translator;
                    //    model.NativeLanguageID = data.NativeLanguageID;
                    //    model.IsAuthenticated = true;
                    //    return model;
                    //}

                }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(ex.Message);
            }
            return model;
        }

        public CustomerUserModel PartnerLogin(string email, string password, string culture)
        {
            CustomerUserModel model = null;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                ICompanyRepository rep = new CompanyRepository(ouw);
                ICompanyService service = new CompanyService(rep);
                var data = service.GetCustomerByUser(email, password, culture,true);

                if (data != null)
                {
                    model = new CustomerUserModel();
                    model.ID = data.ID;
                    model.RegistrationID = data.RegistrationID;
                    model.RegistrationNo = data.RegistrationNo;
                    model.Email = data.ClientID;
                    model.Name = data.Name;
                    model.UserType = (int)TransproUserType.Customer;
                    model.IsAuthenticated = true;
                    return model;
                }

            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                // errorLog.SetErrorLog(model.CurrentUserID, "Customer Login Error for transprologin", message);
                throw new Exception(ex.Message);
            }
            return model;
        }

        public bool CheckTranslatorPassword(string email, string password)
        {
           
            
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                
        var data = rep.GetTranslatorByUser(email, password);

                if (data == null)
                {
                    return false;
           
                }

            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(ex.Message);
            }
            return true;
        }



        public bool CheckExistingTranslator(string emailId, string culture)
        {
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                var data = rep.GetStaffList().Find(f => f.StaffEmailID.Trim() == emailId.Trim());
                    //service.GetStaffList(new BaseViewModel() { CurrentCulture = culture }).Find(f => f.StaffEmailID.Trim() == emailId.Trim());
                if (data == null)
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }


        public StaffModel GetTranslatorById(string translatorId, string cultureId)
        {
            StaffModel _model = null;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                var data = service.GetTranslatorByID(new BaseViewModel() { ID = Guid.Parse(translatorId), CurrentCulture = cultureId });

                if (data != null)
                    _model = data;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _model;
        }


        public StaffModel GetImageById(string translatorId, string cultureId)
        {
            StaffModel _model = null;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                var data = service.GetTranslatorByID(new BaseViewModel() { ID = Guid.Parse(translatorId), CurrentCulture = cultureId });
                if (data != null)
                    _model = data;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _model;
        }

        public bool SetNewPassword(ResetPassword model)
        {           
            try
            {
                ISqlConnectionService _sqlConnService = new SqlConnectionService();
                SqlCommand cmd;
                try
                {
                    _sqlConnService.OpenConnection();
                    cmd = new SqlCommand("SP_SetNewPassword", _sqlConnService.CreateConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.UserID);
                    cmd.Parameters.AddWithValue("@Password",Utility.MD5(model.Password));
                    cmd.Parameters.AddWithValue("@StatementType", model.UserType);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {                  
                    string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                    throw new Exception(message);
                }
                finally
                {
                    _sqlConnService.CloseConnection();
                }
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool  ChangePhoto(Guid staffId, string Photo)
        {
            bool changephoto = false;
            try
            {
            IUnitOfWork ouw = new UnitOfWork();
            IStaffRepository rep = new StaffRepository(ouw);
            IStaffService service = new StaffService(rep);

            
                var result = rep.GetStaffById(staffId);

                if (result != null)
                {
                    result.Image = Photo;
                    rep.UpdateStaff(result);
                }
                  

            }
            catch (Exception ex)
            {
                changephoto = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(ex.Message);
            }

            return changephoto;

        }

        public bool ChangePassword(Guid staffId, string password)
        {
            bool changepass = false;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);


                var result = rep.GetStaffById(staffId);

                if (result != null)
                {
                    result.Password = Utility.MD5(password);
                    rep.UpdateStaff(result);
                }


            }
            catch (Exception ex)
            {
                changepass = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(ex.Message);
            }

            return changepass;

        }      

        public bool WithDrawTranslatorMembership(StaffModel model)
        {
            bool status = false;
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IStaffRepository rep = new StaffRepository(ouw);
                IStaffService service = new StaffService(rep);
                var mapdata = Mapper.Map<StaffModel, Staff>(model);
                status = rep.WithdrawMembeship(mapdata);
            }
            catch (Exception ex)
            {
                status = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(ex.Message);
            }

            return status;

        }


        public List<MessageModel> getReceiveMsgById(Guid receiverID)
        {
            List<MessageModel> msgList = new List<MessageModel>();
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IMessageRepository rep = new MessageRepository(ouw);
                IMessageService service = new MessageService(rep);

                var list = rep.GetAllByReceiverID(receiverID);
                msgList = Mapper.Map<List<Message>, List<MessageModel>>(list);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return msgList;
        }

        public List<MessageModel> getSendMsgById(Guid senderID)
        {
            List<MessageModel> msgList = new List<MessageModel>();
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IMessageRepository rep = new MessageRepository(ouw);
            
                var list = rep.GetAllBySenderID(senderID);
                msgList = Mapper.Map<List<Message>, List<MessageModel>>(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return msgList;
        }

        public List<MessageModel> getDetailsbymsgId(long msgId)
        {
            List<MessageModel> msgdetailsList = new List<MessageModel>();
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                IMessageRepository rep = new MessageRepository(ouw);

                var list = rep.GetDetailsBymsgid(msgId);
                msgdetailsList = Mapper.Map<List<Message>, List<MessageModel>>(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return msgdetailsList;
        }

    }
}
