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
    public class CompanyBusinessSpecialityController : ApiController
    {
        ICompanyBusinessSpecialityService _companyBusinessSpecialityService;
        public CompanyBusinessSpecialityController(ICompanyBusinessSpecialityService companyBusinessSpecialityService)
        {
            _companyBusinessSpecialityService = companyBusinessSpecialityService;
        }
        [Route("businessspeciality/list")]
        [HttpPost]
        public HttpResponseMessage GetBusinessSpecialityModels(CompanyBusinessSpecialityModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyBusinessSpecialityList = _companyBusinessSpecialityService.GetAllCompanyBusinessSpecialityList(model);
                    if (companyBusinessSpecialityList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyBusinessSpecialityList);
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

        [Route("businessspeciality/save")]
        [HttpPost]
        public HttpResponseMessage Save(CompanyBusinessSpecialityModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyBusinessSpecialityList = _companyBusinessSpecialityService.SaveCompanyBusinessSpeciality(model);
                    if (companyBusinessSpecialityList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyBusinessSpecialityList);
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

        [Route("businessspeciality/delete")]
        [HttpPost]
        public HttpResponseMessage Delete(CompanyBusinessSpecialityModel model)
        {
           try
            {
                if (this.ModelState.IsValid)
                {
                    var companyBusinessSpecialityList = _companyBusinessSpecialityService.DeleteCompanyBusinessSpeciality(model);
                    if (companyBusinessSpecialityList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyBusinessSpecialityList);
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
