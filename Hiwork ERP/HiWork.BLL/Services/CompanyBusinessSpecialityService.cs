using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HiWork.BLL.Services
{
     
   public partial interface ICompanyBusinessSpecialityService
    {
        List<CompanyBusinessSpecialityModel> SaveCompanyBusinessSpeciality(CompanyBusinessSpecialityModel model);
        List<CompanyBusinessSpecialityModel> GetAllCompanyBusinessSpecialityList(CompanyBusinessSpecialityModel model);
        List<CompanyBusinessSpecialityModel> DeleteCompanyBusinessSpeciality(CompanyBusinessSpecialityModel model);
    }
    public class CompanyBusinessSpecialityService : ICompanyBusinessSpecialityService
    {
        private readonly ISqlConnectionService _sqlConnService;
        public CompanyBusinessSpecialityService()
        {
            _sqlConnService = new SqlConnectionService();
        }

        public List<CompanyBusinessSpecialityModel> GetAllCompanyBusinessSpecialityList(CompanyBusinessSpecialityModel model)
        {
            List<CompanyBusinessSpecialityModel> companyBusinessSpecialityList = new List<CompanyBusinessSpecialityModel>();
            CompanyBusinessSpecialityModel companyBusinessSpecialityModel;
            try
            {
                 _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllCompanyBusinessSpeciality", _sqlConnService.CreateConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    companyBusinessSpecialityModel = new CompanyBusinessSpecialityModel();
                    companyBusinessSpecialityModel.ID = Guid.Parse(reader["ID"].ToString());
                    companyBusinessSpecialityModel.Code = reader["Code"].ToString();
                    companyBusinessSpecialityModel.Name = reader["Name"].ToString();
                    companyBusinessSpecialityModel.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    companyBusinessSpecialityModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    companyBusinessSpecialityModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    companyBusinessSpecialityModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    companyBusinessSpecialityModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    companyBusinessSpecialityModel.CurrentUserID = model.CurrentUserID;
                    companyBusinessSpecialityModel.CurrentCulture = model.CurrentCulture;
                    companyBusinessSpecialityList.Add(companyBusinessSpecialityModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyBusinessSpeciality", message);
                throw new Exception(ex.Message);
            }
            return companyBusinessSpecialityList;
        }

        public List<CompanyBusinessSpecialityModel> SaveCompanyBusinessSpeciality(CompanyBusinessSpecialityModel model)
        {
            List<CompanyBusinessSpecialityModel> companyBusinessSpecialityList = new List<CompanyBusinessSpecialityModel>();
            try
            {
                 _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_SaveCompanyBusinessSpeciality", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@Code", model.Code);
                cmd.Parameters.AddWithValue("@Name_" + model.CurrentCulture, model.Name);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
                if (a > 0)
                {
                    companyBusinessSpecialityList = GetAllCompanyBusinessSpecialityList(model);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyBusinessSpeciality", message);
                throw new Exception(ex.Message);
            }
            return companyBusinessSpecialityList;
        }
        public List<CompanyBusinessSpecialityModel> DeleteCompanyBusinessSpeciality(CompanyBusinessSpecialityModel model)
        {
            List<CompanyBusinessSpecialityModel> companyBusinessSpecialityList = new List<CompanyBusinessSpecialityModel>();
            try
            {             
                 _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_DeleteCompanyBusinessSpeciality", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
                if (a > 0)
                {
                    companyBusinessSpecialityList = GetAllCompanyBusinessSpecialityList(model);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyBusinessSpeciality", message);
                throw new Exception(ex.Message);
            }
            return companyBusinessSpecialityList;
        }
    }
}
