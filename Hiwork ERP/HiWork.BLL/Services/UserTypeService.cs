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

namespace HiWork.BLL.Services
{
    public partial interface IUserTypeService : IBaseService<UserTypeModel, UserType>
    {
        List<UserTypeModel> SaveUserType(UserTypeModel model);
        List<UserTypeModel> GetAllUserTypeList(UserTypeModel model);
        List<UserTypeModel> DeleteUserType(UserTypeModel model);
    }

    public class UserTypeService : BaseService<UserTypeModel, UserType>, IUserTypeService
    {
        private readonly IUserTypeRepository _userTypeRepository;
        public UserTypeService(IUserTypeRepository userTypeRepository)
            : base(userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }       
        public List<UserTypeModel> SaveUserType(UserTypeModel model)
        {
            List<UserTypeModel> usertypeList = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var usertype = Mapper.Map<UserTypeModel, UserType>(model);
                if (usertype.Id != 0)
                {
                    usertype.UpdatedBy = model.CurrentUserID;
                    usertype.UpdatedDate = DateTime.Now;
                    _userTypeRepository.UpdateUserType(usertype);
                }
                else
                {
                    usertype.CreatedBy = model.CurrentUserID;
                    usertype.CreatedDate = DateTime.Now;
                    _userTypeRepository.InsertUserType(usertype);
                }
                usertypeList = GetAllUserTypeList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "UserType", message);
                throw new Exception(ex.Message);
            }
            return usertypeList;
        }
        public List<UserTypeModel> GetAllUserTypeList(UserTypeModel model)
        {
            List<UserTypeModel> userTypeList = new List<UserTypeModel>();
            UserTypeModel userTypeModel = new UserTypeModel();
            try
            {
                List<UserType> userTypevList = _userTypeRepository.GetAllUserTypeList();
                if (userTypevList != null)
                {
                    userTypevList.ForEach(a =>
                    {
                        userTypeModel = Mapper.Map<UserType, UserTypeModel>(a);
                        userTypeModel.Name = Utility.GetPropertyValue(userTypeModel, "Name", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(userTypeModel, "Name", model.CurrentCulture).ToString();
                        userTypeModel.Description = Utility.GetPropertyValue(userTypeModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(userTypeModel, "Description", model.CurrentCulture).ToString();
                        userTypeModel.CurrentUserID = model.CurrentUserID;
                        userTypeModel.CurrentCulture = model.CurrentCulture;
                        userTypeList.Add(userTypeModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "UserType", message);
                throw new Exception(ex.Message);
            }
            return userTypeList;
        }



        public List<UserTypeModel> DeleteUserType(UserTypeModel model)
        {
            List<UserTypeModel> userTypeList = null;
            try
            {
                _userTypeRepository.DeleteUserType(model.ID);
                userTypeList = GetAllUserTypeList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "UserType", message);
                throw new Exception(ex.Message);
            }
            return userTypeList;
        }
    }
}
