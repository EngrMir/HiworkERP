using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Responses;
using HiWork.BLL.ViewModels;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;


namespace Central.API.Controllers
{

    public class RoleController : ApiController
    {
        IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [Route("role/save")]
        [HttpPost]
        public HttpResponseMessage Save(RoleModel aRoleModel)
        {
            
            try
            {
                if (this.ModelState.IsValid)
                {
                    var roleList = _roleService.SaveRole(aRoleModel);
                    if (roleList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, roleList);
                    }
                    else
                    {
                        string message = "Error Saving Data";
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                    }

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }


        [Route("role/list")]
        [HttpPost]
        public HttpResponseMessage GetRoles(BaseViewModel model)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    var roleList = _roleService.GetAllRoleList(model);
                    if (roleList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, roleList);
                    }
                    else
                    {
                        string message = "Error in getting Data";
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                    }

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }

        }
        [Route("role/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteRoles(RoleModel aRoleModel) 
        {
            
           try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _roleService.DeleteRole(aRoleModel);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        string message = "Not deleted successfully";
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }
       
    }
}
