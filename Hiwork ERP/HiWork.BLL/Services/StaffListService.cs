

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
using HiWork.Utils.Infrastructure.Contract;
using AutoMapper;

namespace HiWork.BLL.Services
{
    public partial interface IStaffListService
    {
        List<StaffListData> GetStaffList(BaseViewModel model);
        dynamic GetStaffID(string staffID);
        List<StaffListData> GetSearchStaffList(BaseViewModel model, string con);
    }
    
    public class StaffListData
    {
        public string StaffNo { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PostalCode { get; set; }
        public string LivingCountry { get; set; }
    }

    public class StaffListService : IStaffListService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public StaffListService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        public List<StaffListData> GetStaffList(BaseViewModel model)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            TranslationCertificateSettingsModel SettingsModel;
            List<StaffListData> retList = new List<StaffListData>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetStaffList", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    StaffListData retVal = new StaffListData();
                    if (!string.IsNullOrEmpty(DataReader["StaffNo"].ToString())) { retVal.StaffNo = (DataReader["StaffNo"].ToString()); }
                    if (!string.IsNullOrEmpty(DataReader["Name"].ToString())) { retVal.Name = (DataReader["Name"].ToString()); }
                    if (!string.IsNullOrEmpty(DataReader["BirthDate"].ToString())) { retVal.BirthDate = Convert.ToDateTime(DataReader["BirthDate"]); }
                    if (!string.IsNullOrEmpty(DataReader["Gender"].ToString())) { retVal.Gender = DataReader["Gender"].ToString(); }
                    if (!string.IsNullOrEmpty(DataReader["PostalCode"].ToString())) { retVal.PostalCode = DataReader["PostalCode"].ToString(); }
                    if (!string.IsNullOrEmpty(DataReader["LivingCountry"].ToString())) { retVal.LivingCountry = DataReader["LivingCountry"].ToString(); }
                    retList.Add(retVal);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffListService", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return retList;
        }
        public List<StaffListData> GetSearchStaffList(BaseViewModel model, string con)
        {
            List<StaffListData> retList = new List<StaffListData>();
            SqlDataReader DataReader;
            try
            {
                string branch = con.Split(',')[0];
                string type = con.Split(',')[1];
                string eid = con.Split(',')[2];
                string resign = con.Split(',')[3];
                string role = con.Split(',')[4];
                string author = con.Split(',')[5];
                string ename = con.Split(',')[6];
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetSearchAllUser", _sqlConnService.CreateConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                if (model.ID == Guid.Empty)
                {
                    command.Parameters.AddWithValue("@ID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@ID", model.ID);
                }
                command.Parameters.AddWithValue("@EmployeeId", DBNull.Value);
                command.Parameters.AddWithValue("@UserTypeId", DBNull.Value);
                if (branch == "")
                    command.Parameters.AddWithValue("@branch", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@branch", branch);
                if (type == "")
                    command.Parameters.AddWithValue("@type", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@type", type);
                if (eid == "")
                    command.Parameters.AddWithValue("@eid", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@eid", eid);
                if (resign == "")
                    command.Parameters.AddWithValue("@resign", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@resign", resign);
                if (role == "")
                    command.Parameters.AddWithValue("@role", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@role", role);
                if (author == "")
                    command.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@author", author);
                if (ename == "")
                    command.Parameters.AddWithValue("@ename", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@ename", ename);

                DataReader = command.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    StaffListData retVal = new StaffListData();
                    if (!string.IsNullOrEmpty(DataReader["StaffNo"].ToString())) { retVal.StaffNo = (DataReader["StaffNo"].ToString()); }
                    if (!string.IsNullOrEmpty(DataReader["Name"].ToString())) { retVal.Name = (DataReader["Name"].ToString()); }
                    if (!string.IsNullOrEmpty(DataReader["BirthDate"].ToString())) { retVal.BirthDate = Convert.ToDateTime(DataReader["BirthDate"]); }
                    if (!string.IsNullOrEmpty(DataReader["Gender"].ToString())) { retVal.Gender = DataReader["Gender"].ToString(); }
                    if (!string.IsNullOrEmpty(DataReader["PostalCode"].ToString())) { retVal.PostalCode = DataReader["PostalCode"].ToString(); }
                    if (!string.IsNullOrEmpty(DataReader["LivingCountry"].ToString())) { retVal.LivingCountry = DataReader["LivingCountry"].ToString(); }
                    retList.Add(retVal);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffListService", message);
                throw new Exception(message);
            }
            finally
            {
                _sqlConnService.CloseConnection();
            }
            return retList;
        }
        public dynamic GetStaffID(string staffID)
        {
            SqlCommand cmd;
            SqlDataReader DataReader;
            List<object> lstObjRet = new List<object>();

            try
            {
                _sqlConnService.OpenConnection();
                cmd = new SqlCommand("SP_GetStaffID", _sqlConnService.CreateConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@staffID", staffID);

                DataReader = cmd.ExecuteReader();
                while (DataReader.Read() == true)
                {
                    object row = new
                    {
                        Id = DataReader["ID"].ToString()
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
