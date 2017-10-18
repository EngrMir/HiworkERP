

/* ******************************************************************************************************************
 * Controller for Master_CompanyIndustryClassification Entity
 * Date             :   24-Jun-2017
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
    public class CompanyIndustryClassificationController : ApiController
    {
        private ICompanyIndustryClassificationService service;
        public CompanyIndustryClassificationController(ICompanyIndustryClassificationService arg)
        {
            service = arg;
        }

        [Route("companyindustryclassification/save")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyIndustryClassification(CompanyIndustryClassificationModel arg)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveCompanyIndustryClassification(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving CompanyIndustryClassification data";
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

        [Route("companyindustryclassification/list")]
        [HttpPost]
        public HttpResponseMessage GetCompanyIndustryClassificationList(BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.GetCompanyIndustryClassificationList(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving CompanyIndustryClassification list";
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

        [Route("companyindustryclassification/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCompanyIndustryClassification(CompanyIndustryClassificationModel arg)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteCompanyIndustryClassification(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting CompanyIndustryClassification data";
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