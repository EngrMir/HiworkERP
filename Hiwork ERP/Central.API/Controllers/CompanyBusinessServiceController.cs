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
    public class CompanyBusinessServiceController : ApiController
    {
        ICompanyBusinessService _companyBusinessService;
        public CompanyBusinessServiceController(ICompanyBusinessService companyBusinessService)
        {
            _companyBusinessService = companyBusinessService;
        }
        [Route("companybusiness/list")]
        [HttpPost]
        public HttpResponseMessage GetBusinessServiceModels(CompanyBusinessServiceModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyBusinessList = _companyBusinessService.GetAllCompanyBusinessList(model);
                    if (companyBusinessList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyBusinessList);
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

        [Route("companybusiness/save")]
        [HttpPost]
        public HttpResponseMessage Save(CompanyBusinessServiceModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyBusinessList = _companyBusinessService.SaveCompanyBusiness(model);
                    if (companyBusinessList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyBusinessList);
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

        [Route("companybusiness/delete")]
        [HttpPost]
        public HttpResponseMessage Delete(CompanyBusinessServiceModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyBusinessList = _companyBusinessService.DeleteCompanyBusiness(model);
                    if (companyBusinessList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyBusinessList);
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
