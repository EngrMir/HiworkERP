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
    [Authorize]
    public class EmploymentTypeController : ApiController
    {
        IEmploymentTypeService _employmentTypeService;
        public EmploymentTypeController(IEmploymentTypeService employmentTypeService)
        {
            _employmentTypeService = employmentTypeService;
        }
        [Route("employmentType/save")]
        [HttpPost]
        public HttpResponseMessage Save(EmploymentTypeModel aEmploymentTypeModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var EmploymentTypeModelList = _employmentTypeService.SaveEmploymentType(aEmploymentTypeModel);
                    if (EmploymentTypeModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, EmploymentTypeModelList);
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

        [Route("employmentType/list")]
        [HttpPost]
        public HttpResponseMessage GetEmploymentTypeModels(EmploymentTypeModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var EmploymentTypeList = _employmentTypeService.GetAllEmploymentTypeList(model);
                    if (EmploymentTypeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, EmploymentTypeList);
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

        [Route("employmentType/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEmploymentType(EmploymentTypeModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _employmentTypeService.DeleteEmploymentType(model);
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
