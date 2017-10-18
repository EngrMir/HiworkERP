

/* ******************************************************************************************************************
 * Controller for Master_CompanyTradingDivision Entity
 * Date             :   04-July-2017
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
    public class CompanyTradingDivisionController : ApiController
    {
        private ICompanyTradingDivisionService service;
        public CompanyTradingDivisionController(ICompanyTradingDivisionService arg)
        {
            service = arg;
        }


        [Route("companytradingdivision/save")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyTradingDivision(CompanyTradingDivisionModel arg)
        {
            HttpResponseMessage result;
            List<CompanyTradingDivisionModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveCompanyTradingDivision(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving CompanyTradingDivision data";
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


        [Route("companytradingdivision/list")]
        [HttpPost]
        public HttpResponseMessage GetCompanyTradingDivisionList(BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<CompanyTradingDivisionModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.GetCompanyTradingDivisionList(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving CompanyTradingDivision list";
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


        [Route("companytradingdivision/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCompanyTradingDivision(CompanyTradingDivisionModel arg)
        {
            HttpResponseMessage result;
            List<CompanyTradingDivisionModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteCompanyTradingDivision(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting CompanyTradingDivision data";
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
    }
}