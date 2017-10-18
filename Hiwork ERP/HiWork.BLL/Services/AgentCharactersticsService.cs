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
  public partial interface IAgentCharactersticsService
    {
        List<AgentCharactersticsModel> SaveAgentCharacterstics(AgentCharactersticsModel model);
        List<AgentCharactersticsModel> GetAllAgentCharactersticsList(BaseViewModel model);
        List<AgentCharactersticsModel> DeleteAgentCharacterstics(AgentCharactersticsModel model);
    }
    public class AgentCharactersticsService : IAgentCharactersticsService
    {
        private readonly ISqlConnectionService _sqlConnService;
        public AgentCharactersticsService()
        {
            _sqlConnService = new SqlConnectionService();
        }

        public List<AgentCharactersticsModel> GetAllAgentCharactersticsList(BaseViewModel model)
        {
            List<AgentCharactersticsModel> agentCharactersticsList = new List<AgentCharactersticsModel>();
            AgentCharactersticsModel agentCharactersticsModel;
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand command = new SqlCommand("SP_GetAllAgentCharacterstics", _sqlConnService.CreateConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CultureId", model.CurrentCulture);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    agentCharactersticsModel = new AgentCharactersticsModel();
                    agentCharactersticsModel.ID = Guid.Parse(reader["ID"].ToString());
                    agentCharactersticsModel.Code = reader["Code"].ToString();
                    agentCharactersticsModel.Name = reader["Name"].ToString();
                    agentCharactersticsModel.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                    agentCharactersticsModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    agentCharactersticsModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    agentCharactersticsModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    agentCharactersticsModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    agentCharactersticsModel.CurrentUserID = model.CurrentUserID;
                    agentCharactersticsModel.CurrentCulture = model.CurrentCulture;
                    agentCharactersticsList.Add(agentCharactersticsModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentCharacterstics", message);
                throw new Exception(ex.Message);
            }
            return agentCharactersticsList;
        }

        public List<AgentCharactersticsModel> SaveAgentCharacterstics(AgentCharactersticsModel model)
        {
            List<AgentCharactersticsModel> agentCharactersticsList = new List<AgentCharactersticsModel>();
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_SaveAgentCharacterstics", _sqlConnService.CreateConnection());
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
                errorLog.SetErrorLog(model.CurrentUserID, "AgentCharacterstics", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllAgentCharactersticsList(md);
        }
        public List<AgentCharactersticsModel> DeleteAgentCharacterstics(AgentCharactersticsModel model)
        {
            List<AgentCharactersticsModel> agentCharactersticsList = new List<AgentCharactersticsModel>();
            try
            {
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_DeleteAgentCharacterstics", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.ID);
                int a = cmd.ExecuteNonQuery();
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentCharacterstics", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllAgentCharactersticsList(md);
        }
    }
}
