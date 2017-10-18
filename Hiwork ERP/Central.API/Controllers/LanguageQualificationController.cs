

/* ******************************************************************************************************************
 * Controller for Master_StaffLanguageQualifications Entity
 * Date             :   15-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;

namespace Central.API.Controllers
{

    [EnableCors("*", "*", "*")]
    public class LanguageQualificationController : ApiController
    {
        private ILanguageQualificationService service;
        public LanguageQualificationController(ILanguageQualificationService ser)
        {
            this.service = ser;
        }

        [Route("langqual/save")]
        [HttpPost]
        public HttpResponseMessage SaveLanguageQualification(LanguageQualificationModel arg)
        {
            HttpResponseMessage result;
            List<LanguageQualificationModel> modList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    modList = service.SaveLanguageQualification(arg);
                    if (modList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, modList);
                    }
                    else
                    {
                        string message = "Error while saving LanguageQualification data";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }


        [Route("langqual/list")]
        [HttpPost]
        public HttpResponseMessage GetLanguageQualificationList(BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<LanguageQualificationModel> modList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    modList = service.GetLanguageQualificationList(arg);
                    if (modList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, modList);
                    }
                    else
                    {
                        string message = "Error while retriving LanguageQualification list";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }


        [Route("langqual/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteLanguageQualification(LanguageQualificationModel arg)
        {
            HttpResponseMessage result;
            List<LanguageQualificationModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteLanguageQualification(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting LanguageQualification data";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }
    }
}