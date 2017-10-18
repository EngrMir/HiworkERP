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
    public partial interface ICompanyBusinessService
    {
        List<CompanyBusinessServiceModel> SaveCompanyBusiness(CompanyBusinessServiceModel model);
        List<CompanyBusinessServiceModel> GetAllCompanyBusinessList(CompanyBusinessServiceModel model);
        List<CompanyBusinessServiceModel> DeleteCompanyBusiness(CompanyBusinessServiceModel model);
    }
    public class CompanyBusinessService : ICompanyBusinessService
    {
        private readonly ISqlConnectionService _sqlConnService;
        public CompanyBusinessService()
        {
            _sqlConnService = new SqlConnectionService();
        }

        public List<CompanyBusinessServiceModel> GetAllCompanyBusinessList(CompanyBusinessServiceModel model)
        {
            List<CompanyBusinessServiceModel> companyBusinessServiceList = new List<CompanyBusinessServiceModel>();
            CompanyBusinessServiceModel companyBusinessServiceModel;
            try
            {
                 _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllCompanyBusinessService", _sqlConnService.CreateConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    companyBusinessServiceModel = new CompanyBusinessServiceModel();
                    companyBusinessServiceModel.ID = Guid.Parse(reader["ID"].ToString());
                    companyBusinessServiceModel.Code = reader["Code"].ToString();
                    companyBusinessServiceModel.Name = reader["Name"].ToString();
                    companyBusinessServiceModel.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    companyBusinessServiceModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    companyBusinessServiceModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    companyBusinessServiceModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    companyBusinessServiceModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    companyBusinessServiceModel.CurrentUserID = model.CurrentUserID;
                    companyBusinessServiceModel.CurrentCulture = model.CurrentCulture;
                    companyBusinessServiceList.Add(companyBusinessServiceModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyBusinessService", message);
                throw new Exception(ex.Message);
            }
            return companyBusinessServiceList;
        }     

        public List<CompanyBusinessServiceModel> SaveCompanyBusiness(CompanyBusinessServiceModel model)
        {
            List<CompanyBusinessServiceModel> companyBusinessServiceList = new List<CompanyBusinessServiceModel>();
            try
            {
                 _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_SaveCompanyBusinessService", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@Code", model.Code);
                cmd.Parameters.AddWithValue("@Name_"+model.CurrentCulture, model.Name);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
                if (a > 0)
                  {
                     companyBusinessServiceList = GetAllCompanyBusinessList(model);
                  }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyBusinessService", message);
                throw new Exception(ex.Message);
            }
            return companyBusinessServiceList;
        }
        public List<CompanyBusinessServiceModel> DeleteCompanyBusiness(CompanyBusinessServiceModel model)
        {
            List<CompanyBusinessServiceModel> companyBusinessServiceList = new List<CompanyBusinessServiceModel>();
            try
            {
                 _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_DeleteCompanyBusinessService", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
                if (a > 0)
                {
                    companyBusinessServiceList = GetAllCompanyBusinessList(model);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyBusinessService", message);
                throw new Exception(ex.Message);
            }
            return companyBusinessServiceList;
        }
    }
}
