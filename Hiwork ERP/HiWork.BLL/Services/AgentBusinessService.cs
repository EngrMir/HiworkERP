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
    public partial interface IAgentBusinessService
    {
        List<AgentBusinessModel> SaveAgentBusiness(AgentBusinessModel model);
        List<AgentBusinessModel> GetAllAgentBusinessList(BaseViewModel model);
        List<AgentBusinessModel> DeleteAgentBusiness(AgentBusinessModel model);
    }
    public class AgentBusinessService : IAgentBusinessService
    {
        private readonly ISqlConnectionService _sqlConnService;
        public AgentBusinessService()
        {
            _sqlConnService = new SqlConnectionService();
        }

        public List<AgentBusinessModel> GetAllAgentBusinessList(BaseViewModel model)
        {
            List<AgentBusinessModel> agentBusinessList = new List<AgentBusinessModel>();
            AgentBusinessModel agentBusinessModel;
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllAgentBusiness", _sqlConnService.CreateConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    agentBusinessModel = new AgentBusinessModel();
                    agentBusinessModel.ID = Guid.Parse(reader["ID"].ToString());
                    agentBusinessModel.Code = reader["Code"].ToString();
                    agentBusinessModel.Name = reader["Name"].ToString();
                    agentBusinessModel.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    agentBusinessModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    agentBusinessModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    agentBusinessModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    agentBusinessModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    agentBusinessModel.CurrentUserID = model.CurrentUserID;
                    agentBusinessModel.CurrentCulture = model.CurrentCulture;
                    agentBusinessList.Add(agentBusinessModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            return agentBusinessList;
        }

        public List<AgentBusinessModel> SaveAgentBusiness(AgentBusinessModel model)
        {
            List<AgentBusinessModel> agentBusinessList = new List<AgentBusinessModel>();
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_SaveAgentBusiness", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@Code", model.Code);
                cmd.Parameters.AddWithValue("@Name_" + model.CurrentCulture, model.Name);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();                
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllAgentBusinessList(md);
        }
        public List<AgentBusinessModel> DeleteAgentBusiness(AgentBusinessModel model)
        {
            List<AgentBusinessModel> agentBusinessList = new List<AgentBusinessModel>();
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_DeleteAgentBusiness", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllAgentBusinessList(md);
        }
    }
}
