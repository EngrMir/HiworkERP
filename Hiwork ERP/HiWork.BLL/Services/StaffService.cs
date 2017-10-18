using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using HiWork.BLL.ViewModels;
using System.Data.Entity;
using HiWork.BLL.ServiceHelper;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace HiWork.BLL.Services
{
    public interface IStaffService : IBaseService<StaffModel, Staff>
    {
        List<StaffModel> SaveStaff(StaffModel aStaffModel);
        List<StaffModel> GetStaffList(BaseViewModel aStaffModel);
        List<StaffModel> DeleteStaff(StaffModel aStaffModel);

        StaffModel GetTranslatorByUser(string email, string password, string culture);
        StaffModel GetTranslatorByID(BaseViewModel model);
        List<StaffViewModel> GetStaffSearchList(BaseViewModel model, Guid? srcLanguageID, Guid? targetLanguageID, Guid? specialFieldID);
        List<StaffMejorSubModel> GetMejorSubjectList(BaseViewModel model);
        List<StaffEducationalDegModel> GetEducationalDegreeList(BaseViewModel model);
        bool SaveStaffEducation(EducationHistoryModel educationalInfoModel);
        StaffModel GetStaffByEmail(string email);
        bool SaveStaffSkillTech(StaffSkillTechModel staffSkillTechModel);
        bool SaveStaffTRExperience(TranslateInterpretExperienceModel staffTRExperienceModel);
        bool SaveBankAccInfo(StaffBankAccountInfoModel staffBankAccountInfoModel);
        bool SaveTransPro(TransproInformationModel transproInformationModel);
        bool SaveNarration(NarrationCommon narrationCommon);
        StaffNestedModel GetAllStaffAndChildByID(string staffID);
    }
    public class StaffService : BaseService<StaffModel, Staff>, IStaffService
    {
        private CentralDBEntities _dbContext;
        private IStaffRepository _StaffRepository;
        private readonly ISqlConnectionService _sqlConnService;
        public StaffService(IStaffRepository StaffRepository) : base(StaffRepository)
        {
            _dbContext = new CentralDBEntities();
            _StaffRepository = StaffRepository;
            _sqlConnService = new SqlConnectionService();
        }

        public bool SaveNarration(NarrationCommon narrationCommon)
        {
            var isSuccessful = true;
            Staff_NarrationInformation staffNarrationInformation = null;
            Staff_NarrationVoiceFiles staffNarrationVoiceFiles = null;
            try
            {
                if (narrationCommon.NarrationInformationModel != null)
                {
                    staffNarrationInformation = Mapper.Map<NarrationInformationModel, Staff_NarrationInformation>(narrationCommon.NarrationInformationModel);
                    if (staffNarrationInformation.ID == Guid.Empty)
                    {
                        staffNarrationInformation.ID = Guid.NewGuid();
                        staffNarrationInformation.CreatedBy = narrationCommon.NarrationInformationModel.CurrentUserID;
                        staffNarrationInformation.CreatedDate = DateTime.Now;
                        _dbContext.Staff_NarrationInformation.Add(staffNarrationInformation);
                    }
                    else
                    {
                        staffNarrationInformation.UpdatedBy = narrationCommon.NarrationInformationModel.CurrentUserID;
                        staffNarrationInformation.UpdatedDate = DateTime.Now;
                        _dbContext.Entry(staffNarrationInformation).State = EntityState.Modified;
                    }
                }

                if (narrationCommon.NarrationVoiceFilesModel != null)
                {
                    staffNarrationVoiceFiles = Mapper.Map<NarrationVoiceFilesModel, Staff_NarrationVoiceFiles>(narrationCommon.NarrationVoiceFilesModel);
                    if (staffNarrationVoiceFiles.ID == Guid.Empty)
                    {
                        staffNarrationVoiceFiles.ID = Guid.NewGuid();
                        staffNarrationVoiceFiles.CreatedBy = narrationCommon.NarrationVoiceFilesModel.CurrentUserID;
                        staffNarrationVoiceFiles.CreatedDate = DateTime.Now;
                        _dbContext.Staff_NarrationVoiceFiles.Add(staffNarrationVoiceFiles);
                    }
                    else
                    {
                        staffNarrationVoiceFiles.UpdatedBy = narrationCommon.NarrationVoiceFilesModel.CurrentUserID;
                        staffNarrationVoiceFiles.UpdatedDate = DateTime.Now;
                        _dbContext.Entry(staffNarrationVoiceFiles).State = EntityState.Modified;
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(narrationCommon.NarrationInformationModel.CreatedBy.Value, "staffTRExperience", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        public bool SaveTransPro(TransproInformationModel transproInformationModel)
        {
            var isSuccessful = true;
            Staff_TransproInformation staffTransproInformation = null;
            try
            {
                if (transproInformationModel != null)
                {
                    staffTransproInformation = Mapper.Map<TransproInformationModel, Staff_TransproInformation>(transproInformationModel);
                    if (staffTransproInformation.ID == Guid.Empty)
                    {
                        staffTransproInformation.ID = Guid.NewGuid();
                        staffTransproInformation.CreatedBy = transproInformationModel.CurrentUserID;
                        staffTransproInformation.CreatedDate = DateTime.Now;
                        _dbContext.Staff_TransproInformation.Add(staffTransproInformation);
                    }
                    else
                    {
                        staffTransproInformation.UpdatedBy = transproInformationModel.CurrentUserID;
                        staffTransproInformation.UpdatedDate = DateTime.Now;
                        _dbContext.Entry(staffTransproInformation).State = EntityState.Modified;
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(transproInformationModel.CreatedBy.Value, "staffTRExperience", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }
        public bool SaveBankAccInfo(StaffBankAccountInfoModel staffBankAccountInfoModel)
        {
            var isSuccessful = true;
            Staff_BankAccountInfo staffBankAccountInfo = null;
            try
            {
                if (staffBankAccountInfoModel != null)
                {
                    staffBankAccountInfo = Mapper.Map<StaffBankAccountInfoModel, Staff_BankAccountInfo>(staffBankAccountInfoModel);
                    if (staffBankAccountInfo.ID == Guid.Empty)
                    {
                        staffBankAccountInfo.ID = Guid.NewGuid();
                        staffBankAccountInfo.CreatedBy = staffBankAccountInfoModel.CurrentUserID;
                        staffBankAccountInfo.CreatedDate = DateTime.Now;
                        _dbContext.Staff_BankAccountInfo.Add(staffBankAccountInfo);
                    }
                    else
                    {
                        staffBankAccountInfo.UpdatedBy = staffBankAccountInfoModel.CurrentUserID;
                        staffBankAccountInfo.UpdatedDate = DateTime.Now;
                        _dbContext.Entry(staffBankAccountInfo).State = EntityState.Modified;
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(staffBankAccountInfo.UpdatedBy.Value, "staffTRExperience", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        public bool SaveStaffTRExperience(TranslateInterpretExperienceModel staffTRExperienceModel)
        {
            var isSuccessful = true;
            Staff_TranslateInterpretExperience staffTRExperience = null;
            try
            {
                if (staffTRExperienceModel != null)
                {
                    staffTRExperience = Mapper.Map<TranslateInterpretExperienceModel, Staff_TranslateInterpretExperience>(staffTRExperienceModel);
                    if (staffTRExperience.ID == Guid.Empty)
                    {
                        staffTRExperience.ID = Guid.NewGuid();
                        staffTRExperience.CreatedBy = staffTRExperienceModel.CurrentUserID;
                        staffTRExperience.CreatedDate = DateTime.Now;
                        _dbContext.Staff_TranslateInterpretExperience.Add(staffTRExperience);
                    }
                    else
                    {
                        staffTRExperience.UpdatedBy = staffTRExperienceModel.CurrentUserID;
                        staffTRExperience.UpdatedDate = DateTime.Now;
                        _dbContext.Entry(staffTRExperienceModel).State = EntityState.Modified;
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(staffTRExperience.UpdatedBy.Value, "staffTRExperience", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        public bool SaveStaffSkillTech(StaffSkillTechModel staffSkillTechModel)
        {
            var isSuccessful = true;
            try
            {
                if (staffSkillTechModel.SkillCertificateModel != null)
                {
                    if (staffSkillTechModel.SkillCertificateModel.ID == Guid.Empty)
                    {
                        staffSkillTechModel.SkillCertificateModel.ID = Guid.NewGuid();
                        staffSkillTechModel.SkillCertificateModel.CreatedBy = staffSkillTechModel.CurrentUserID;
                        staffSkillTechModel.SkillCertificateModel.CreatedDate = DateTime.Now;
                        _dbContext.Staff_SkillCertificate.Add(staffSkillTechModel.SkillCertificateModel);
                    }
                    else
                    {
                        staffSkillTechModel.SkillCertificateModel.UpdatedBy = staffSkillTechModel.CurrentUserID;
                        staffSkillTechModel.SkillCertificateModel.UpdatedDate = DateTime.Now;
                        _dbContext.Entry(staffSkillTechModel.SkillCertificateModel).State = EntityState.Modified;
                    }
                }

                if (staffSkillTechModel.TechnicalCertificateModel != null && staffSkillTechModel.TechnicalCertificateModel.Count() > 0)
                {
                    foreach (var item in staffSkillTechModel.TechnicalCertificateModel)
                    {
                        if (item.ID == Guid.Empty)
                        {
                            item.ID = Guid.NewGuid();
                            item.CreatedBy = staffSkillTechModel.CurrentUserID;
                            item.CreatedDate = DateTime.Now;
                            _dbContext.Staff_TechnicalCertificate.Add(item);
                        }
                        else
                        {
                            item.UpdatedBy = staffSkillTechModel.CurrentUserID;
                            item.UpdatedDate = DateTime.Now;
                            _dbContext.Entry(item).State = EntityState.Modified;
                        }
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(staffSkillTechModel.CurrentUserID, "staffSkillTech", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        public bool SaveStaffEducation(EducationHistoryModel educationalInfoModel)
        {
            var isSuccessful = true;
            try
            {
                foreach (var item in educationalInfoModel.EducationalInformation)
                {
                    if (item.ID == Guid.Empty)
                    {
                        item.ID = Guid.NewGuid();
                        _dbContext.Staff_EducationalHistory.Add(item);
                    }
                    else
                    {
                        _dbContext.Entry(item).State = EntityState.Modified;
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(educationalInfoModel.CurrentUserID, "StaffEducation", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        public List<StaffModel> SaveStaff(StaffModel aStaffModel)
        {
            List<StaffModel> Staffs = null;

            BaseViewModel bvModel = new BaseViewModel();
            bvModel.ApplicationId = aStaffModel.ApplicationId;

            try
            {
                Utility.SetDynamicPropertyValue(aStaffModel, aStaffModel.CurrentCulture);
                var Staff = Mapper.Map<StaffModel, Staff>(aStaffModel);

                if (Staff.ID == Guid.Empty)
                {
                    Staff.StaffNo = GetNextStaffNumber(bvModel);
                    Staff.CreatedBy = aStaffModel.CurrentUserID;
                    Staff.CreatedDate = DateTime.Now;
                    _StaffRepository.InsertStaff(Staff);
                }
                else
                {
                    Staff.UpdatedBy = aStaffModel.CurrentUserID;
                    Staff.UpdatedDate = DateTime.Now;
                    _StaffRepository.UpdateStaff(Staff);               
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aStaffModel.CurrentCulture;
                baseViewModel.CurrentUserID = aStaffModel.CurrentUserID;
                Staffs = GetStaffList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffModel.CurrentUserID, "Staff", message);
                throw new Exception(ex.Message);
            }
            return Staffs;
        }

        public string GetNextStaffNumber(BaseViewModel model)
        {
            string StaffNo = string.Empty;
            try
            {
                IUnitOfWork unitWork = new UnitOfWork();
                IApplicationService appService = new ApplicationService(new ApplicationRepository(unitWork));

                long? NextID = _StaffRepository.GetStaffNextRegistrationID(model);
                NextID = NextID == null ? 0 : NextID;
                NextID = NextID + 1;
                StaffNo = Helper.GenerateUniqueID(appService.GetApplicationCode(model.ApplicationId), NextID.ToString());
            }
            catch (Exception ex)
            {

            }

            return StaffNo;
        }

        public List<StaffModel> GetStaffList(BaseViewModel model)
        {
            List<StaffModel> usersInfoModelList = new List<StaffModel>();
            StaffModel StaffModel = new StaffModel();
            try
            {
                List<Staff> StaffList = _StaffRepository.GetStaffList();

                if (StaffList != null)
                {
                    StaffList.ForEach(a =>
                    {
                        StaffModel = Mapper.Map<Staff, StaffModel>(a);
                        StaffModel.FirstName = Utility.GetPropertyValue(StaffModel, "FirstName", model.CurrentCulture) == null ? string.Empty :
                                                     Utility.GetPropertyValue(StaffModel, "FirstName", model.CurrentCulture).ToString();
                        StaffModel.MiddleName = Utility.GetPropertyValue(StaffModel, "MiddleName", model.CurrentCulture) == null ? string.Empty :
                                                    Utility.GetPropertyValue(StaffModel, "MiddleName", model.CurrentCulture).ToString();
                        StaffModel.LastName = Utility.GetPropertyValue(StaffModel, "LastName", model.CurrentCulture) == null ? string.Empty :
                                                    Utility.GetPropertyValue(StaffModel, "LastName", model.CurrentCulture).ToString();
                        StaffModel.Address = Utility.GetPropertyValue(StaffModel, "Address", model.CurrentCulture) == null ? string.Empty :
                                            Utility.GetPropertyValue(StaffModel, "Address", model.CurrentCulture).ToString();
                        StaffModel.HomeCountryAddress = Utility.GetPropertyValue(StaffModel, "HomeCountryAddress", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(StaffModel, "HomeCountryAddress", model.CurrentCulture).ToString();
                        StaffModel.SelfPR = Utility.GetPropertyValue(StaffModel, "SelfPR", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(StaffModel, "SelfPR", model.CurrentCulture).ToString();
                        StaffModel.Street = Utility.GetPropertyValue(StaffModel, "Street", model.CurrentCulture) == null ? string.Empty :
                                                         Utility.GetPropertyValue(StaffModel, "Street", model.CurrentCulture).ToString();
                        StaffModel.TownName = Utility.GetPropertyValue(StaffModel, "TownName", model.CurrentCulture) == null ? string.Empty :
                                                        Utility.GetPropertyValue(StaffModel, "TownName", model.CurrentCulture).ToString();

                        StaffModel.Name = $"{StaffModel.FirstName} {StaffModel.LastName}";
                        StaffModel.StaffNo = a.StaffNo;
                        StaffModel.CurrentUserID = model.CurrentUserID;
                        StaffModel.CurrentCulture = model.CurrentCulture;
                        usersInfoModelList.Add(StaffModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Staff", message);
                throw new Exception(ex.Message);
            }

            return usersInfoModelList;
        }

        public List<StaffViewModel> GetStaffSearchList(BaseViewModel model, Guid? srcLanguageID, Guid? targetLanguageID, Guid? specialFieldID)
        {
            List<StaffViewModel> _list = new List<StaffViewModel>();
            SqlCommand cmd;
            SqlDataReader dataReader;
            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAllTranslators_By_Search", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture == null ? null : model.CurrentCulture);
                cmd.Parameters.AddWithValue("@SourceLangID", srcLanguageID == null ? null : srcLanguageID);
                cmd.Parameters.AddWithValue("@TargetLangID", targetLanguageID == null ? null : targetLanguageID);
                cmd.Parameters.AddWithValue("@SpecialFieldID", specialFieldID == null ? null : specialFieldID);
                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    StaffViewModel stfModel = new StaffViewModel();
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
                    _list.Add(stfModel);
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


            return _list;
        }

        public List<StaffModel> DeleteStaff(StaffModel aStaffModel)
        {
            List<StaffModel> StaffModels = null;
            try
            {
                _StaffRepository.DeleteStaff(aStaffModel.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aStaffModel.CurrentCulture;
                baseViewModel.CurrentUserID = aStaffModel.CurrentUserID;
                StaffModels = GetStaffList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffModel.CurrentUserID, "Staff", message);
                throw new Exception(ex.Message);
            }
            return StaffModels;
        }

        public StaffModel GetTranslatorByUser(string email, string password, string culture)
        {
            StaffModel translator = null;
            try
            {
                var data = _StaffRepository.GetTranslatorByUser(email, password);
                var mapData = Mapper.Map<Staff, StaffModel>(data);
                if (mapData != null)
                {
                    mapData.ApartmentName = Utility.GetPropertyValue(mapData, "ApartmentName", culture) == null ? string.Empty :
                                            Utility.GetPropertyValue(mapData, "ApartmentName", culture).ToString();

                    mapData.FirstName = Utility.GetPropertyValue(mapData, "FirstName", culture) == null ? string.Empty :
                                        Utility.GetPropertyValue(mapData, "FirstName", culture).ToString();

                    mapData.LastName = Utility.GetPropertyValue(mapData, "LastName", culture) == null ? string.Empty :
                                       Utility.GetPropertyValue(mapData, "LastName", culture).ToString();

                    mapData.Street = Utility.GetPropertyValue(mapData, "Street", culture) == null ? string.Empty :
                                         Utility.GetPropertyValue(mapData, "Street", culture).ToString();

                    mapData.MainCareer = Utility.GetPropertyValue(mapData, "MainCareer", culture) == null ? string.Empty :
                                       Utility.GetPropertyValue(mapData, "MainCareer", culture).ToString();

                    mapData.SelfPR = Utility.GetPropertyValue(mapData, "SelfPR", culture) == null ? string.Empty :
                                      Utility.GetPropertyValue(mapData, "SelfPR", culture).ToString();

                    translator = new StaffModel();
                    translator.ID = mapData.ID;
                    translator.StaffEmailID = mapData.StaffEmailID;
                    translator.RegistrationID = mapData.RegistrationID;
                    translator.FirstName = string.Concat(mapData.FirstName, " ", mapData.LastName);
                    translator.NativeLanguageID = mapData.NativeLanguageID;

                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                //errorLog.SetErrorLog(, "Company", message);
                throw new Exception(ex.Message);
            }

            return translator;

        }

        public StaffModel GetTranslatorByID(BaseViewModel model)
        {

            StaffModel translator = new StaffModel();
            StaffBankAccountInfoModel bankaccountInfo = new StaffBankAccountInfoModel();
            List<StaffBankAccountInfoModel> bankaccountInfoList = new List<StaffBankAccountInfoModel>();
            StaffSoftwareSkillModel skillmodel = new StaffSoftwareSkillModel();
            List<StaffSoftwareSkillModel> skillList = new List<StaffSoftwareSkillModel>();
            List<StaffProfesionalSpecialityModel> spModel = new List<StaffProfesionalSpecialityModel>();
            StaffProfesionalSpecialityModel sModel = new StaffProfesionalSpecialityModel();
            List<StaffCurrentStateModel> statelist = new List<StaffCurrentStateModel>();
            StaffCurrentStateModel usModel = new StaffCurrentStateModel();

            try
            {
                var trans = _StaffRepository.GetById(model.ID);
                translator = Mapper.Map<Staff, StaffModel>(trans);
                translator.Staffbank = Mapper.Map<Staff_BankAccountInfo, StaffBankAccountInfoModel>(trans.Staff_BankAccountInfo.FirstOrDefault());


                if (trans != null)
                {
                    translator.ApartmentName = Utility.GetPropertyValue(trans, "ApartmentName", model.CurrentCulture) == null ? string.Empty :
                                                Utility.GetPropertyValue(trans, "ApartmentName", model.CurrentCulture).ToString();

                    translator.FirstName = Utility.GetPropertyValue(trans, "FirstName", model.CurrentCulture) == null ? string.Empty :
                                            Utility.GetPropertyValue(trans, "FirstName", model.CurrentCulture).ToString();

                    translator.LastName = Utility.GetPropertyValue(trans, "LastName", model.CurrentCulture) == null ? string.Empty :
                                            Utility.GetPropertyValue(trans, "LastName", model.CurrentCulture).ToString();

                    translator.Street = Utility.GetPropertyValue(trans, "Street", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(trans, "Street", model.CurrentCulture).ToString();

                    translator.MainCareer = Utility.GetPropertyValue(trans, "MainCareer", model.CurrentCulture) == null ? string.Empty :
                                            Utility.GetPropertyValue(trans, "MainCareer", model.CurrentCulture).ToString();

                    translator.SelfPR = Utility.GetPropertyValue(trans, "SelfPR", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(trans, "SelfPR", model.CurrentCulture).ToString();

                    translator.CityOfOverseas = Utility.GetPropertyValue(trans, "CityOfOverseas", model.CurrentCulture) == null ? string.Empty :
                                                Utility.GetPropertyValue(trans, "CityOfOverseas", model.CurrentCulture).ToString();

                    translator.CountryOfCitizenshipName = Utility.GetPropertyValue(trans.Master_Country, "Name", model.CurrentCulture) == null ? string.Empty :
                                                Utility.GetPropertyValue(trans.Master_Country, "Name", model.CurrentCulture).ToString();

                    translator.Image = trans.Image;
                    translator.Staffbank.AccountHolderName = Utility.GetPropertyValue(trans.Staff_BankAccountInfo.FirstOrDefault(), "AccountHolderName", model.CurrentCulture) == null ? string.Empty :
                                                Utility.GetPropertyValue(trans.Staff_BankAccountInfo.FirstOrDefault(), "AccountHolderName", model.CurrentCulture).ToString();


                    var StaffSoftwareSkillList = trans.Staff_SoftwareSkill.ToList();
                    translator.staffsoft = skillList;

                    if (StaffSoftwareSkillList.Count() > 0)
                    {
                        StaffSoftwareSkillList.ForEach(a =>
                        {
                            //if (a.StaffID == trans.ID)
                            //{
                            skillmodel = Mapper.Map<Staff_SoftwareSkill, StaffSoftwareSkillModel>(a);
                            skillmodel.CurrentUserID = model.CurrentUserID;
                            skillmodel.CurrentCulture = model.CurrentCulture;
                            skillList.Add(skillmodel);
                            //}
                        });
                    }

                    var StaffProfessionalSpecialityList = trans.Staff_ProfessionalSpeciality.ToList();
                    translator.StaffProfessional = spModel;
                    if (StaffProfessionalSpecialityList.Count() > 0)
                    {
                        StaffProfessionalSpecialityList.ForEach(a =>
                        {

                            sModel = Mapper.Map<Staff_ProfessionalSpeciality, StaffProfesionalSpecialityModel>(a);
                            sModel.CurrentUserID = model.CurrentUserID;
                            sModel.CurrentCulture = model.CurrentCulture;
                            spModel.Add(sModel);
                        });
                    }

                    var StaffCurrentStateList = trans.Staff_CurrentStates.ToList();
                    translator.StaffCurrentState = statelist;
                    if (StaffCurrentStateList.Count() > 0)
                    {
                        StaffCurrentStateList.ForEach(a =>
                        {
                            usModel = Mapper.Map<Staff_CurrentStates, StaffCurrentStateModel>(a);
                            usModel.CurrentUserID = model.CurrentUserID;
                            usModel.CurrentCulture = model.CurrentCulture;
                            statelist.Add(usModel);
                        });
                    }
                }
            }

            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
               // errorLog.SetErrorLog(model.CurrentUserID, "Translator By ID", message);
                throw new Exception(message);

            }

            return translator;
        }

        public List<StaffMejorSubModel> GetMejorSubjectList(BaseViewModel model)
        {
            StaffMejorSubModel staffMejorSubModel;
            var staffMejorSubModelList = new List<StaffMejorSubModel>();

            try
            {
                var masterDataList = _dbContext.Master_StaffMajorSubject;
                if (masterDataList != null)
                {
                    foreach (Master_StaffMajorSubject MasterData in masterDataList)
                    {
                        staffMejorSubModel = Mapper.Map<Master_StaffMajorSubject, StaffMejorSubModel>(MasterData);
                        staffMejorSubModel.CurrentUserID = model.CurrentUserID;
                        staffMejorSubModel.CurrentCulture = model.CurrentCulture;
                        staffMejorSubModelList.Add(staffMejorSubModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Master_StaffMajorSubject", message);
                throw new Exception(message);
            }
            return staffMejorSubModelList;
        }
        public List<StaffEducationalDegModel> GetEducationalDegreeList(BaseViewModel model)
        {
            StaffEducationalDegModel staffEducationalDegModel;
            var StaffEducationalDegModelList = new List<StaffEducationalDegModel>();

            try
            {
                var masterDataList = _dbContext.Master_StaffEducationalDegree;
                if (masterDataList != null)
                {
                    foreach (Master_StaffEducationalDegree MasterData in masterDataList)
                    {
                        staffEducationalDegModel = Mapper.Map<Master_StaffEducationalDegree, StaffEducationalDegModel>(MasterData);
                        staffEducationalDegModel.CurrentUserID = model.CurrentUserID;
                        staffEducationalDegModel.CurrentCulture = model.CurrentCulture;
                        StaffEducationalDegModelList.Add(staffEducationalDegModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Master_StaffEducationalDegree", message);
                throw new Exception(message);
            }
            return StaffEducationalDegModelList;

        }
        public StaffModel GetStaffByEmail(string email)
        {
            StaffModel staff = new StaffModel();
            try
            {
                var dbCompany = _StaffRepository.GetList().Where(f => f.StaffEmailID.Trim() == email.Trim()).FirstOrDefault();
                staff = Mapper.Map<Staff, StaffModel>(dbCompany);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return staff;
        }

        public StaffNestedModel GetAllStaffAndChildByID(string staffIDparam)
        {
            StaffNestedModel staffWholeEntity = new StaffNestedModel();
            Guid staffID = new Guid(staffIDparam);
            try
            {
                Staff staff = _dbContext.Staffs.Where(i => i.ID == staffID).FirstOrDefault();
                List<Staff_JobHistory> staffJob = _dbContext.Staff_JobHistory.Where(i => i.StaffID == staffID).ToList();
                var staffEducationHistory = _dbContext.Staff_EducationalHistory.Where(i => i.StaffID == staffID).ToList();
                Staff_SkillCertificate staffSkillCertificate = _dbContext.Staff_SkillCertificate.Where(i => i.StaffID == staffID).FirstOrDefault();
                var staffTechCertificate = _dbContext.Staff_TechnicalCertificate.Where(i => i.StaffID == staffID).ToList();
                Staff_TranslateInterpretExperience staffTRExperience = _dbContext.Staff_TranslateInterpretExperience.Where(i => i.StaffID == staffID).FirstOrDefault();
                Staff_BankAccountInfo staffBankPayment = _dbContext.Staff_BankAccountInfo.Where(i => i.StaffID == staffID).FirstOrDefault();
                Staff_TransproInformation staffTransPro = _dbContext.Staff_TransproInformation.Where(i => i.StaffID == staffID).FirstOrDefault();
                Staff_NarrationInformation staffNarration = _dbContext.Staff_NarrationInformation.Where(i => i.StaffID == staffID).FirstOrDefault();

                staffWholeEntity.staff = Mapper.Map<Staff, StaffModel>(staff);
                foreach (Staff_JobHistory job in staffJob)
                {
                    staffWholeEntity.staffJobHistory.Add(Mapper.Map<Staff_JobHistory, JobHistoryModel>(job));
                }

                foreach (Staff_EducationalHistory eh in staffEducationHistory)
                {
                    staffWholeEntity.staffEducationalHistory.EducationalInformation.Add(eh);
                }

                if(staffSkillCertificate != null) staffWholeEntity.staffSkillCertificate.SkillCertificateModel = staffSkillCertificate ;

                foreach (Staff_TechnicalCertificate tc in staffTechCertificate)
                {
                    staffWholeEntity.staffSkillCertificate.TechnicalCertificateModel.Add(tc);
                }
                staffWholeEntity.staffTRExperience = Mapper.Map<Staff_TranslateInterpretExperience, TranslateInterpretExperienceModel>(staffTRExperience);
                staffWholeEntity.staffBankAccountInfoModel = Mapper.Map<Staff_BankAccountInfo, StaffBankAccountInfoModel>(staffBankPayment);
                staffWholeEntity.transproInformationModel = Mapper.Map<Staff_TransproInformation, TransproInformationModel>(staffTransPro);
                staffWholeEntity.narrationInformationModel = Mapper.Map<Staff_NarrationInformation, NarrationInformationModel>(staffNarration);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return staffWholeEntity;
        }

    }
}
