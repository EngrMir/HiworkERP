

/* ******************************************************************************************************************
 * Controller for Master_CompanyIndustryClassificationItem Entity
 * Date             :   29-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{


    public class CompanyIndustryClassificationItemController : ApiController
    {
        private ICompanyIndustryClassificationItemService service;
        public CompanyIndustryClassificationItemController(ICompanyIndustryClassificationItemService arg)
        {
            service = arg;
        }


        [Route("companyindustryclassificationitem/save")]
        [HttpPost]
        public HttpResponseMessage
                    SaveCompanyIndustryClassificationItem(CompanyIndustryClassificationItemModel arg)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationItemModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveCompanyIndustryClassificationItem(arg);
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


        [Route("companyindustryclassificationitem/list")]
        [HttpPost]
        public HttpResponseMessage
                    GetCompanyIndustryClassificationItemList (BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationItemModel> dataset;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataset = service.GetCompanyIndustryClassificationItemList(arg);
                    if (dataset != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataset);
                    }
                    else
                    {
                        string message = "Error while retriving CompanyIndustryClassificationItem list";
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


        [Route("companyindustryclassificationitem/delete")]
        [HttpPost]
        public HttpResponseMessage
                    DeleteCompanyIndustryClassificationItem(CompanyIndustryClassificationItemModel arg)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationItemModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteCompanyIndustryClassificationItem(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting CompanyIndustryClassificationItem data";
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