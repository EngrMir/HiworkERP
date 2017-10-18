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
    public class UserTypeController : ApiController
    {
        [Route("userType/get")]
        [HttpPost]
        public HttpResponseMessage GetUserTypes(UserTypeModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IUserTypeRepository repo = new UserTypeRepository(uWork);
            IUserTypeService userTypeService = new UserTypeService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var userTypeList = userTypeService.GetAllUserTypeList(model);
                    if (userTypeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, userTypeList);
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
    }
}
