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
     
   public partial interface IAgentFreeDesignationService
    {
        List<AgentFreeDesignationModel> SaveAgentFreeDesignation(AgentFreeDesignationModel model);
        List<AgentFreeDesignationModel> GetAllAgentFreeDesignationList(BaseViewModel model);
        List<AgentFreeDesignationModel> DeleteAgentFreeDesignation(AgentFreeDesignationModel model);
    }
    public class AgentFreeDesignationService : IAgentFreeDesignationService
    {
        private readonly ISqlConnectionService _sqlConnService;
        public AgentFreeDesignationService()
        {
            _sqlConnService = new SqlConnectionService();
        }

        public List<AgentFreeDesignationModel> GetAllAgentFreeDesignationList(BaseViewModel model)
        {
            List<AgentFreeDesignationModel> agentFreeDesignationList = new List<AgentFreeDesignationModel>();
            AgentFreeDesignationModel agentFreeDesignationModel;
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllAgentFreeDesignation", _sqlConnService.CreateConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    agentFreeDesignationModel = new AgentFreeDesignationModel();
                    agentFreeDesignationModel.ID = Guid.Parse(reader["ID"].ToString());
                    agentFreeDesignationModel.Code = reader["Code"].ToString();
                    agentFreeDesignationModel.Name = reader["Name"].ToString();
                    agentFreeDesignationModel.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    agentFreeDesignationModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    agentFreeDesignationModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    agentFreeDesignationModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    agentFreeDesignationModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    agentFreeDesignationModel.CurrentUserID = model.CurrentUserID;
                    agentFreeDesignationModel.CurrentCulture = model.CurrentCulture;
                    agentFreeDesignationList.Add(agentFreeDesignationModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentFreeDesignation", message);
                throw new Exception(ex.Message);
            }
            return agentFreeDesignationList;
        }

        public List<AgentFreeDesignationModel> SaveAgentFreeDesignation(AgentFreeDesignationModel model)
        {
            List<AgentFreeDesignationModel> agentFreeDesignationList = new List<AgentFreeDesignationModel>();
            try
            {
                _sqlConnService.CloseConnection();
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_SaveAgentFreeDesignation", _sqlConnService.CreateConnection());
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
                errorLog.SetErrorLog(model.CurrentUserID, "AgentFreeDesignation", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllAgentFreeDesignationList(md);
        }
        public List<AgentFreeDesignationModel> DeleteAgentFreeDesignation(AgentFreeDesignationModel model)
        {
            List<AgentFreeDesignationModel> agentFreeDesignationList = new List<AgentFreeDesignationModel>();
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_DeleteAgentFreeDesignation", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentFreeDesignation", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllAgentFreeDesignationList(md);
        }
    }
}
