using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{
    public class AdvertizementSettingsController : ApiController
    {
        IAdvertizementSettingsService service;
        public AdvertizementSettingsController(IAdvertizementSettingsService _service)
        {
            service = _service;
        }

        [Route("advertizementsettings/save")]
        [HttpPost]
        public HttpResponseMessage SaveAdvertizementSettings(AdvertizementSettingsModel advertizement)
        {
            try
            {

                if (this.ModelState.IsValid)
                {
                    var advertizementsetting = service.SaveAdvertizementSettings(advertizement);
                    if (advertizementsetting != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, advertizementsetting);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }


        }
        [Route("advertizementsettings/list")]
        [HttpPost]
        public HttpResponseMessage GetAdvertizementsettings(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var advertizementsettingsList = service.GetAdvertizementlist(model);
                    if (advertizementsettingsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, advertizementsettingsList);
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
        [Route("AdvertizementSettings/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteAdvertizementSettings(AdvertizementSettingsModel advertizement)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteAdvertizement(advertizement);
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
