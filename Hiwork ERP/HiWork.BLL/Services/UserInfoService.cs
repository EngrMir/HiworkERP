


using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace HiWork.BLL.Services
{
    public partial interface IUserInfoService : IBaseService<UserInfoModel, UserInformation>
    {
        List<UserInfoModel> SaveUserInfo(UserInfoModel aUserInfoModel);
        List<UserInfoModel> GetUserInfoList(BaseViewModel baseModel);
        List<UserInfoModel> DeleteUserInfo(UserInfoModel aUserInfoModel);
        UserInfoModel GetUserByEmployeeId(BaseViewModel model);
        List<UserInfoModel> GetSearchUserList(BaseViewModel model, string con);
    }
    public class UserInfoService : BaseService<UserInfoModel, UserInformation>, IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel basemodel;
        public UserInfoService(IUserInfoRepository userInfoRepository) : base(userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
            _sqlConnService = new SqlConnectionService();
            basemodel = new BaseViewModel();
        }

        public List<UserInfoModel> SaveUserInfo(UserInfoModel aUserInfoModel)
        {
            UserInformation userdata;
            try
            {
                aUserInfoModel.Password = Utility.MD5(aUserInfoModel.Password);
                Utility.SetDynamicPropertyValue(aUserInfoModel, aUserInfoModel.CurrentCulture);
                userdata = Mapper.Map<UserInfoModel, UserInformation>(aUserInfoModel);

                if (userdata.Id > 0)
                {
                    userdata.UpdatedBy = aUserInfoModel.CurrentUserID;
                    userdata.UpdateDate = DateTime.Now;
                    _userInfoRepository.UpdateUserInformation(userdata);
                }
                else
                {
                    userdata.CreatedBy = aUserInfoModel.CurrentUserID;
                    userdata.CreatedDate = DateTime.Now;
                    _userInfoRepository.InsertUserInformation(userdata);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, aUserInfoModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel baseModel = new BaseViewModel();
            baseModel.CurrentCulture = aUserInfoModel.CurrentCulture;
            baseModel.CurrentUserID = aUserInfoModel.CurrentUserID;
            return GetUserInfoList(baseModel);
        }
        public List<UserInfoModel> GetSearchUserList(BaseViewModel model, string con)
        {
            List<UserInfoModel> userInfoList = new List<UserInfoModel>();
            UserInfoModel userInfoModel;
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
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userInfoModel = new UserInfoModel();
                    userInfoModel.Id = Convert.ToInt32(reader["ID"].ToString());
                    userInfoModel.FirstName = reader["FirstName"].ToString();
                    userInfoModel.LastName = reader["LastName"].ToString();
                    userInfoModel.EmployeeID = Guid.Parse(reader["EmployeeID"].ToString());
                    userInfoModel.MobileNo = reader["MobileNo"].ToString();
                    userInfoModel.Email = reader["Email"].ToString();
                    userInfoModel.Address = reader["Address"].ToString();
                    userInfoModel.UserTypeId = Convert.ToInt32(reader["UserTypeId"].ToString());
                    userInfoModel.RoleId = Convert.ToInt32(reader["RoleId"].ToString());
                    userInfoModel.Username = reader["Username"].ToString();
                    userInfoModel.Password = reader["Password"].ToString();          
                    userInfoModel.IsActive = Convert.ToBoolean(reader["Active"].ToString());
                    userInfoModel.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    userInfoModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                    userInfoModel.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                    userInfoModel.UpdatedBy = Convert.ToInt32(reader["UpdatedBy"].ToString());
                    userInfoList.Add(userInfoModel);
                }
                _sqlConnService.CloseConnection();
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }
            return userInfoList;
        }
        public List<UserInfoModel> GetUserInfoList(BaseViewModel model)
        {
            
            UserInfoModel userInfoModel;
            List<UserInformation> datalist;
            List<UserInfoModel> usersInfoList;

            try
            {
                usersInfoList = new List<UserInfoModel>();
                datalist = _userInfoRepository.GetUserInformationList();
                if (datalist != null)
                {
                    foreach (UserInformation data in datalist)
                    {
                        userInfoModel = Mapper.Map<UserInformation, UserInfoModel>(data);

                        userInfoModel.CurrentUserID = model.CurrentUserID;
                        userInfoModel.CurrentCulture = model.CurrentCulture;
                        usersInfoList.Add(userInfoModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            return usersInfoList;
        }

        //public UserInfoModel EditUserInfo(UserInfoModel aUserInfoModel)
        //{
        //    var userInfo = Mapper.Map<UserInfoModel, UserInformation>(aUserInfoModel);
        //    var aUserInfo = _userInfoRepository.UpdateUserInfo(userInfo);
        //    var userInfoModel = Mapper.Map<UserInformation, UserInfoModel>(aUserInfo);
        //    return userInfoModel;
        //}

        public List<UserInfoModel> DeleteUserInfo(UserInfoModel aUserInfoModel)
        {
            try
            {
                _userInfoRepository.DeleteUserInformation(aUserInfoModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, aUserInfoModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel baseModel = new BaseViewModel();
            baseModel.CurrentCulture = aUserInfoModel.CurrentCulture;
            baseModel.CurrentUserID = aUserInfoModel.CurrentUserID;
            return GetUserInfoList(baseModel);
        }

        private string LogException (Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "UserInformation", message);
            return message;
        }

        public UserInfoModel GetUserByEmployeeId(BaseViewModel model)
        {
            var data = _userInfoRepository.GetUserByEmployeeId(model.ID);
             var result = Mapper.Map<UserInformation, UserInfoModel>(data);
            return result;
        }
    }
}
