

using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{
    public partial interface IRoleService : IBaseService<RoleModel, Role>
    {
        List<RoleModel> SaveRole(RoleModel aRoleModel);
        List<RoleModel> GetAllRoleList(BaseViewModel aRoleModel);
        List<RoleModel> DeleteRole(RoleModel aRoleModel);
    }

    public class RoleService : BaseService<RoleModel, Role>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository) : base(roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public List<RoleModel> SaveRole(RoleModel aRoleModel)
        {
            List<RoleModel> roles = null;
            try
            {
                Utility.SetDynamicPropertyValue(aRoleModel, aRoleModel.CurrentCulture);

                var role = Mapper.Map<RoleModel, Role>(aRoleModel);
                if (role.Id > 0)
                {
                    role.UpdatedBy = aRoleModel.CurrentUserID;
                    role.UpdatedDate = DateTime.Now;
                    _roleRepository.UpdateUserRole(role);
                }
                else
                {
                    role.CreatedBy = aRoleModel.CurrentUserID;
                    role.CreatedDate = DateTime.Now;
                    _roleRepository.InsertUserRole(role);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aRoleModel.CurrentUserID, "Role", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel basemodel = new BaseViewModel();
            basemodel.CurrentCulture = aRoleModel.CurrentCulture;
            basemodel.CurrentUserID = aRoleModel.CurrentUserID;
            roles = GetAllRoleList(basemodel);
            return roles;
        }

        public List<RoleModel> GetAllRoleList(BaseViewModel model)
        {
            List<RoleModel> roleList = new List<RoleModel>();
            RoleModel roleModel = new RoleModel();
            object pvalue;
            try
            {
                List<Role> rollList = _roleRepository.GetAllUserRoleList();
                if (rollList != null)
                {
                    foreach(Role masterRole in rollList)
                    {
                        if (masterRole.IsDeleted == true)
                            continue;

                        roleModel = Mapper.Map<Role, RoleModel>(masterRole);

                        pvalue = Utility.GetPropertyValue(roleModel, "Name", model.CurrentCulture);
                        roleModel.Name = pvalue == null ? string.Empty : pvalue.ToString();
                        pvalue = Utility.GetPropertyValue(roleModel, "Description", model.CurrentCulture);
                        roleModel.Description = pvalue == null ? string.Empty : pvalue.ToString();

                        roleModel.CurrentUserID = model.CurrentUserID;
                        roleModel.CurrentCulture = model.CurrentCulture;
                        roleList.Add(roleModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Role", message);
                throw new Exception(ex.Message);
            }

             roleList.Sort(CompareRoleByName);
            return roleList;
        }
        public List<RoleModel> DeleteRole(RoleModel aRoleModel)
        {
            List<RoleModel> roles = null;
            try
            {
                //_roleRepository.DeleteUserRole(aRoleModel.Id);
                //roles = GetAllRoleList(aRoleModel);
                aRoleModel.IsDeleted = true;
                roles = this.SaveRole(aRoleModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aRoleModel.CurrentUserID, "Role", message);
                throw new Exception(ex.Message);
            }
            return roles;
        }


        public int CompareRoleByName(RoleModel model1, RoleModel model2)
        {
            int cmpresult;
            cmpresult = string.Compare(model1.Name, model2.Name, true);
            return cmpresult;
        }
    }
}
