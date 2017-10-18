
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Services;
using System.Net;
using System;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;
using HiWork.BLL.Models;

namespace Central.API.Controllers
{
    public class ApplicationController : ApiController
    {

        IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }


        [Route("app/list")]
        [HttpPost]
        public HttpResponseMessage GetApplications(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<ApplicationModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = _applicationService.GetApplicationList(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving Application list";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch(Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }
        [Route("app/getApplicationInfo")]
        [HttpGet]
        public HttpResponseMessage GetApplicationInfo(int applicationId)
        {
            HttpResponseMessage result;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                   var data = _applicationService.GetApplicationinfo(applicationId);
                    if (data != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else
                    {
                        string message = "Error while retriving Application list";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }

        [Route("app/save")]
        [HttpPost]
        public HttpResponseMessage SaveApplications(ApplicationModel model)
        {
            HttpResponseMessage result;
            List<ApplicationModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = _applicationService.SaveApplication(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving Application data";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch(Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }
    }
}