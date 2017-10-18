using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using HiWork.DAL.Repositories;
using System.Globalization;
using System.IO;
using HiWork.BLL.ServiceHelper;

namespace HiWork.BLL.Services
{ 
    public class AdvancedStaffSearchReturnModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int PostalCode { get; set; }
        public string LivingCountry { get; set; }
    }

    public partial interface IAdvancedStaffSearchService
    {
        dynamic GetAdvancedStaffSearch(AdvancedStaffSearchModel model);
        dynamic GetData(string type, string culture);

        List<object> GetSourceOfRegistration(BaseViewModel model);
        List<object> GetLanguage(BaseViewModel model);        
        List<object> GetLanguageLevel(BaseViewModel model);
        List<object> GetAge(BaseViewModel model);
        List<object> GetNationalityGroup(BaseViewModel model);
        List<object> GetNationality(BaseViewModel model);
        List<object> GetVisaType(BaseViewModel model);
        List<object> GetVisaExpire(BaseViewModel model);
        List<object> GetSnsAccount(BaseViewModel model);
        List<object> GetDtp(BaseViewModel model);
        List<object> GetOfficeType(BaseViewModel model);
        List<object> GetWebType(BaseViewModel model);
        List<object> GetTranslationTools(BaseViewModel model);
        List<object> GetToolName(BaseViewModel model);
        List<object> GetDesign(BaseViewModel model);
        List<object> GetSoftwareName(BaseViewModel model);
        List<object> GetTin(BaseViewModel model);
        List<object> GetIin(BaseViewModel model);
        List<object> GetNin(BaseViewModel model);
        List<object> GetNarrationPerformance(BaseViewModel model);
    }

    public class AdvancedStaffSearchService : IAdvancedStaffSearchService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public AdvancedStaffSearchService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        /* EXPERIMENTAL TO BE DONE
         * var culturalItems = new List<string> { "BillingAddress", "ClientAddress", "BillingCompanyName", "DeliveryCompanyName", "DeliveryAddress", "Remarks" };
                var columns = typeof(VW_GetAdvancedSearch).GetProperties().Select(property => property.Name).ToArray();
                foreach (var cols in typeof(AdvancedStaffSearchModel).GetProperties().Select(property => property.Name).ToArray())
                {
                    foreach (var item in columns)
                    {
                        if((item.StartsWith(cols)) && (item.Contains("_" + model.Culture)))
                        {
                            retn.Add(item);
                        }
                    }
                }
         */
        
        public dynamic GetAdvancedStaffSearch(AdvancedStaffSearchModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<AdvancedStaffSearchReturnModel> retList = new List<AdvancedStaffSearchReturnModel>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchData", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CurrentUserID", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationID);
                cmd.Parameters.AddWithValue("@Culture", model.Culture);

                cmd.Parameters.AddWithValue("@RegistrationID", Convert.ToInt64(model.RegistrationID));
                cmd.Parameters.AddWithValue("@MyIdentityNo", model.MyIdentityNo);
                cmd.Parameters.AddWithValue("@StaffEmailID", model.StaffEmailID);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Telephone", model.Telephone);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                cmd.Parameters.AddWithValue("@Fax", model.Fax);
                cmd.Parameters.AddWithValue("@TextSearch", model.TextSearch);
                cmd.Parameters.AddWithValue("@AchievementSearch", model.AchievementSearch);
                cmd.Parameters.AddWithValue("@SourceOfRegistrationID", model.SourceOfRegistrationID);
                cmd.Parameters.AddWithValue("@ForiegnLanguage1ID", model.ForiegnLanguage1ID);
                cmd.Parameters.AddWithValue("@ForeignLang1Level", Convert.ToInt32(model.ForeignLang1Level));
                cmd.Parameters.AddWithValue("@ForiegnLanguage2ID", model.ForiegnLanguage2ID);
                cmd.Parameters.AddWithValue("@ForeignLang2Level", Convert.ToInt32(model.ForeignLang2Level));
                cmd.Parameters.AddWithValue("@ForiegnLanguage3ID", model.ForiegnLanguage3ID);
                cmd.Parameters.AddWithValue("@ForeignLang3Level", Convert.ToInt32(model.ForeignLang3Level));
                cmd.Parameters.AddWithValue("@ForiegnLanguage4ID", model.ForiegnLanguage4ID);
                cmd.Parameters.AddWithValue("@ForeignLang4Level", Convert.ToInt32(model.ForeignLang4Level));
                cmd.Parameters.AddWithValue("@Sex", model.Sex);
                cmd.Parameters.AddWithValue("@AgeFrom", Convert.ToInt32(model.AgeFrom));
                cmd.Parameters.AddWithValue("@AgeTo", Convert.ToInt32(model.AgeTo));
                cmd.Parameters.AddWithValue("@NationalityID", model.NationalityID);
                cmd.Parameters.AddWithValue("@VisaCountryID", model.VisaCountryID);
                cmd.Parameters.AddWithValue("@VisaTypeID", model.VisaTypeID);
                cmd.Parameters.AddWithValue("@VisaExpire", model.VisaExpire.ToString());
                cmd.Parameters.AddWithValue("@RdoResidenceType", model.RdoResidenceType);
                cmd.Parameters.AddWithValue("@ResidenceNationalityID", model.ResidenceNationalityID);
                cmd.Parameters.AddWithValue("@IsIntroVideo", Convert.ToBoolean(model.IsIntroVideo));
                cmd.Parameters.AddWithValue("@ChkActive30Days", Convert.ToBoolean(model.ChkActive30Days));
                cmd.Parameters.AddWithValue("@IsSNS", Convert.ToBoolean(model.IsSNS));
                cmd.Parameters.AddWithValue("@SNSAccount", model.SNSAccount);
                cmd.Parameters.AddWithValue("@IsDtpExperience", Convert.ToBoolean(model.IsDtpExperience));
                cmd.Parameters.AddWithValue("@DtpExp", model.DtpExp);
                cmd.Parameters.AddWithValue("@TechnicalSkillParent", model.TechnicalSkillParent);
                cmd.Parameters.AddWithValue("@TechnicalSkillChild", model.TechnicalSkillChild);
                cmd.Parameters.AddWithValue("@DevelopmentSkillParent", model.DevelopmentSkillParent);
                cmd.Parameters.AddWithValue("@DevelopmentSkillChild", model.DevelopmentSkillChild);
                cmd.Parameters.AddWithValue("@KnowledgeSkillParent", model.KnowledgeSkillParent);
                cmd.Parameters.AddWithValue("@KnowledgeSkillChild", model.KnowledgeSkillChild);
                cmd.Parameters.AddWithValue("@MedicalSkillParent", model.MedicalSkillParent);
                cmd.Parameters.AddWithValue("@MedicalSkillChild", model.MedicalSkillChild);
                cmd.Parameters.AddWithValue("@TranslationFrom", Convert.ToInt32(model.TranslationFrom));
                cmd.Parameters.AddWithValue("@TranslationTo", Convert.ToInt32(model.TranslationTo));
                cmd.Parameters.AddWithValue("@TranslationUnitPrice", Convert.ToDouble(model.TranslationUnitPrice));
                cmd.Parameters.AddWithValue("@InterpretationFrom", Convert.ToInt32(model.InterpretationFrom));
                cmd.Parameters.AddWithValue("@InterpretationTo", Convert.ToInt32(model.InterpretationTo));
                cmd.Parameters.AddWithValue("@InterpretationUnitPriceType", model.InterpretationUnitPriceType);
                cmd.Parameters.AddWithValue("@InterpretationUnitPriceValue", Convert.ToDouble(model.InterpretationUnitPriceValue));
                cmd.Parameters.AddWithValue("@IsSuccessiveInterpretation", Convert.ToBoolean(model.IsSuccessiveInterpretation));
                cmd.Parameters.AddWithValue("@IsWhisperingInterpretation", Convert.ToBoolean(model.IsWhisperingInterpretation));
                cmd.Parameters.AddWithValue("@IsSimultaneousInterpretation", Convert.ToBoolean(model.IsSimultaneousInterpretation));
                cmd.Parameters.AddWithValue("@NarrationFrom", Convert.ToInt32(model.NarrationFrom));
                cmd.Parameters.AddWithValue("@NarrationTo", Convert.ToInt32(model.NarrationTo));
                cmd.Parameters.AddWithValue("@IsSpecialistNarator", Convert.ToBoolean(model.IsSpecialistNarator));
                cmd.Parameters.AddWithValue("@NarrationPerformance", model.NarrationPerformance);

                #region "Commented"
                //cmd.Parameters.AddWithValue("@RegistrationID", model.RegistrationID);
                //cmd.Parameters.AddWithValue("@MyIdentityNo", model.MyIdentityNo);
                //cmd.Parameters.AddWithValue("@StaffEmailID", model.StaffEmailID);
                //cmd.Parameters.AddWithValue("@Name", model.Name);
                //cmd.Parameters.AddWithValue("@Telephone", model.Telephone);
                //cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                //cmd.Parameters.AddWithValue("@Fax", model.Fax);
                //cmd.Parameters.AddWithValue("@TextSearch", model.TextSearch);
                //cmd.Parameters.AddWithValue("@AchievementSearch", model.AchievementSearch);
                //cmd.Parameters.AddWithValue("@SourceOfRegistrationID", model.SourceOfRegistrationID);
                //cmd.Parameters.AddWithValue("@ForiegnLanguage1ID", model.ForiegnLanguage1ID);
                //cmd.Parameters.AddWithValue("@ForeignLang1Level", model.ForeignLang1Level);
                //cmd.Parameters.AddWithValue("@ForiegnLanguage2ID", model.ForiegnLanguage2ID);
                //cmd.Parameters.AddWithValue("@ForeignLang2Level", model.ForeignLang2Level);
                //cmd.Parameters.AddWithValue("@ForiegnLanguage3ID", model.ForiegnLanguage3ID);
                //cmd.Parameters.AddWithValue("@ForeignLang3Level", model.ForeignLang3Level);
                //cmd.Parameters.AddWithValue("@ForiegnLanguage4ID", model.ForiegnLanguage4ID);
                //cmd.Parameters.AddWithValue("@ForeignLang4Level", model.ForeignLang4Level);
                //cmd.Parameters.AddWithValue("@Sex", model.Sex);
                //cmd.Parameters.AddWithValue("@AgeFrom", model.AgeFrom);
                //cmd.Parameters.AddWithValue("@AgeTo", model.AgeTo);
                //cmd.Parameters.AddWithValue("@NationalityID", model.NationalityID);
                //cmd.Parameters.AddWithValue("@VisaCountryID", model.VisaCountryID);
                //cmd.Parameters.AddWithValue("@VisaTypeID", model.VisaTypeID);
                //cmd.Parameters.AddWithValue("@VisaExpire", model.VisaExpire.ToString());
                //cmd.Parameters.AddWithValue("@ResidenceType", model.RdoResidenceType);
                //cmd.Parameters.AddWithValue("@ResidenceNationalityID", model.ResidenceNationalityID);
                //cmd.Parameters.AddWithValue("@IsIntroductionVideo", model.IsIntroVideo);
                //cmd.Parameters.AddWithValue("@IsStaffActive", model.ChkActive30Days);
                //cmd.Parameters.AddWithValue("@IsSNS", model.IsSNS);
                //cmd.Parameters.AddWithValue("@IsDTP", model.IsDtpExperience);
                //cmd.Parameters.AddWithValue("@DTPExperience", model.DtpExp);
                //cmd.Parameters.AddWithValue("@CompletedTranslationFrom", model.TranslationFrom);
                //cmd.Parameters.AddWithValue("@CompletedTranslationTo", model.CompletedTranslationTo);
                //cmd.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
                //cmd.Parameters.AddWithValue("@CompletedInterpretationFrom", model.CompletedInterpretationFrom);
                //cmd.Parameters.AddWithValue("@CompletedInterpretationTo", model.CompletedInterpretationTo);
                //cmd.Parameters.AddWithValue("@InterpretationUnitPriceType", model.InterpretationUnitPriceType);
                //cmd.Parameters.AddWithValue("@InterpretationUnitPrice", model.InterpretationUnitPrice);
                //cmd.Parameters.AddWithValue("@IsSuccessive", model.IsSuccessive);
                //cmd.Parameters.AddWithValue("@IsWhispering", model.IsWhispering);
                //cmd.Parameters.AddWithValue("@IsSimultaneous", model.IsSimultaneous);
                //cmd.Parameters.AddWithValue("@CompletedNarrationFrom", model.CompletedNarrationFrom);
                //cmd.Parameters.AddWithValue("@CompletedNarrationTo", model.CompletedNarrationTo);
                //cmd.Parameters.AddWithValue("@IsSpecialist", model.IsSpecialist);
                //cmd.Parameters.AddWithValue("@Performance", model.Performance);
                #endregion "Commented"

                string query = cmd.CommandText;

                foreach (SqlParameter p in cmd.Parameters)
                {
                    if (p.Value != null)
                    {
                        query += p.ParameterName + " = '" + p.Value.ToString() + "', ";
                    }
                }

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    AdvancedStaffSearchReturnModel retVal = new AdvancedStaffSearchReturnModel();
                    if (!string.IsNullOrEmpty(DataReader["ID"].ToString())) { retVal.ID = Guid.Parse((DataReader["ID"].ToString())); }
                    if (!string.IsNullOrEmpty(DataReader["Name"].ToString())) { retVal.Name = (DataReader["Name"].ToString()); }
                    if (!string.IsNullOrEmpty(DataReader["BirthDate"].ToString())) { retVal.BirthDate = Convert.ToDateTime(DataReader["BirthDate"]); } //Convert.ToDateTime(DataReader["BirthDate"]);
                    if (!string.IsNullOrEmpty(DataReader["Gender"].ToString())) { retVal.Gender = DataReader["Gender"].ToString(); }
                    if (!string.IsNullOrEmpty(DataReader["PostalCode"].ToString())) { retVal.PostalCode = Convert.ToInt32(DataReader["PostalCode"]); }
                    if (!string.IsNullOrEmpty(DataReader["LivingCountry"].ToString())) { retVal.LivingCountry = DataReader["LivingCountry"].ToString(); }
                    retList.Add(retVal);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return retList;
        #region "commented"
        //SqlCommand cmd;
        //SqlDataReader Reader;
        //dynamic ret = null;
        //List<string> retn = new List<string>();
        //_sqlConnService.OpenConnection();
        //try
        //{
        //    /* CONDITIONS ON HOW TO CHECK
        //     * --------------------------
        //     * search query model and compare with vwModel for properties in culture
        //     */



        //    var query = _dbContext.VW_GetAdvancedSearch.Select(f => f);


        //    if (color != null)
        //    {
        //        query = query.Where(f => f.Color == color);
        //    }
        //    if (ripe != null)
        //    {
        //        query = query.Where(f => f.Ripe == ripe);
        //    }
        //    return new FruitListResponse
        //    {
        //        Result = query.Select(f => new Fruit { Id = f.FruitId, Name = f.Name }).ToList()
        //    };



        //    //var val = _dbContext.VW_GetAdvancedSearch.Where(w => w.StaffEmailID == "fsfsd").ToList();
        //    // query model will need to search culture based columns
        //    // results will need to return culture based columns
        //    //ret = _dbContext.VW_GetAdvancedSearch.Where(w => w.StaffEmailID == model.StaffEmailID).ToList();
        //}
        //catch (Exception ex)
        //{
        //    IErrorLogService errorLog = new ErrorLogService();
        //    string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
        //    throw new Exception(message);
        //}
        //finally
        //{
        //    _sqlConnService.CloseConnection();
        //}
        //return ret;
        #endregion "commented"
        }

        public dynamic GetData(string type, string culture)
        {
            SqlCommand cmd;
            SqlDataReader Reader;
            dynamic wc;
            List<object> lstObjRet = new List<object>();
            object o = new
            {
                ID = "",
                Value = ""
            };
            lstObjRet.Add(o);

            _sqlConnService.OpenConnection();
            try
            {
                if(type == "regsource")
                {
                    wc = _dbContext.Master_Language.Select(x => x.GetType().GetProperty("%_en").GetValue(x).ToString());
                }
                else if(type == "")
                {

                }
                wc = _dbContext.VW_GetAdvancedSearch.ToList();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return wc;
        }
        public List<object> Get(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }


        #region "GetInits"
        public List<object> GetSourceOfRegistration(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "registration");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetLanguage(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "language");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        // comes from UTILITIES Static Defination
        public List<object> GetLanguageLevel(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "languagelevel");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetAge(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "age");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        // Removed
        public List<object> GetNationalityGroup(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "nationalitygroup");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetNationality(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "nationality");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetVisaType(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "visatype");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        // How to do?
        public List<object> GetVisaExpire(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "visaexpire");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        // MISSING
        public List<object> GetSnsAccount(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "snsaccount");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        // MISSING
        public List<object> GetDtp(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "dtp");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetOfficeType(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "officetype");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetWebType(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "webtype");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetTranslationTools(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "translationtools");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetToolName(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "toolname");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetDesign(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "design");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetSoftwareName(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "softwarename");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetTin(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "tin");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetIin(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "iin");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetNin(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "nin");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        public List<object> GetNarrationPerformance(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetAdvancedStaffSearchInit", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                cmd.Parameters.AddWithValue("@UserID", model.CurrentUserID.ToString());
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationId);
                cmd.Parameters.AddWithValue("@Type", "narrationperformance");

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["Id"].ToString(),
                        Value = DataReader["Value"].ToString()
                    };
                    lstObjRet.Add(row);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return lstObjRet;
        }
        #endregion "GetInits"


        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
                return;
            if (this._dbContext == null)
                return;
            this._dbContext.Dispose();
            this._dbContext = null;
            return;
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}
