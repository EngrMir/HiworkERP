using System.Collections.Generic;
using AutoMapper;
using HiWork.BLL.Models;
using HiWork.BLL.Resources;
using HiWork.BLL.Responses;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using HiWork.BLL.ViewModels;
using HiWork.Utils;

namespace HiWork.BLL.Services
{
    public partial interface IUserService : IBaseService<UserModel, UserInformation>
    {
        //IList<UserModel> GetAllUserList();
        UserModel GetUser(UserViewModel model);
        UserModel GetUserBySession(UserModel model);
        //BaseResponse InsertUser(UserModel user);
        //BaseResponse UpdateUser(UserModel user);
        //BaseResponse DeleteUser((UserModel user);
    }
    public class UserService : BaseService<UserModel, UserInformation>, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
        }
       
        public UserModel GetUser(UserViewModel model) //string username,string password,long AppId
        {
            UserModel _user = null;
            IUnitOfWork unitWork = new UnitOfWork();
            IApplicationRepository appRep = new ApplicationRepository(unitWork);


            var result = _userRepository.GetUserByUsername(model.UserName, model.Password);

            var appIsActive = appRep.IsApplicationActive(model.ApplicationId);

            if (result != null && appIsActive)
            {
                BaseViewModel baseModel = new BaseViewModel();
               
                baseModel.ApplicationId = model.ApplicationId;
                baseModel.CurrentCulture = model.CurrentCulture;
                baseModel.ID = (Guid)result.EmployeeID;
               

                result.Session = Guid.NewGuid();
                _userRepository.UpdateUser(result);
                var mappedUser = Mapper.Map<UserInformation, UserModel>(result);
                _user = new UserModel();
                _user.CurrentUserID = mappedUser.Id;
                _user.SessionId = result.Session;
                _user.IsSuperAdmin = mappedUser.IsSuperAdmin;
                _user.IsActive = mappedUser.IsActive;
                _user.FullName = result.FirstName + " " + result.LastName;
                _user.Role = mappedUser.Role;
                _user.UserType = mappedUser.UserType;
                _user.Username = mappedUser.Username;
                setUserBranchTeam(ref _user, baseModel);

                return _user;
            }

            return null;
          
        }

  
        private void setUserBranchTeam(ref UserModel _user, BaseViewModel baseModel)
        {
            IUnitOfWork unitWork = new UnitOfWork();
            IEmployeeService employee_service = new EmployeeService(new EmployeeRepository(unitWork));

            _user.Role.Name = Utility.GetPropertyValue(_user.Role, "Name", baseModel.CurrentCulture) == null ? string.Empty :
                                                        Utility.GetPropertyValue(_user.Role, "Name", baseModel.CurrentCulture).ToString();

            var employee = baseModel.ID == Guid.Empty ? null : employee_service.GetEmployeeByID(baseModel);
            _user.Branch = employee != null ? employee.BranchName : _user.Role.Name;
            _user.Team = employee != null ? employee.DepartmentName : "System User";
        }
        public UserModel GetUserBySession(UserModel model)
        {
            UserModel _user = null;
            var result =  _userRepository.GetUserBySession(model.SessionId.ToString());
            var mappedUser = Mapper.Map<UserInformation, UserModel>(result);
            _user = new UserModel();
            _user.CurrentUserID = mappedUser.Id;
            _user.SessionId = result.Session;
            _user.IsSuperAdmin = mappedUser.IsSuperAdmin;
            _user.IsActive = mappedUser.IsActive;
            _user.Role = mappedUser.Role;
            _user.UserType = mappedUser.UserType;
            return _user;
        }    
       
    }
}
