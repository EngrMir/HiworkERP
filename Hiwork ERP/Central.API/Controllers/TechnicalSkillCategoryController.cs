

/* ******************************************************************************************************************
 * Controller for Master_StaffTechnicalSkillCategory Entity
 * Date             :   09-Jun-2017
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

    [Authorize]
    public class TechnicalSkillCategoryController : ApiController
    {
        private ITechnicalSkillCategoryService service;
        public TechnicalSkillCategoryController(ITechnicalSkillCategoryService arg)
        {
            service = arg;
        }

        [Route("tsc/save")]
        [HttpPost]
        public HttpResponseMessage SaveTSC(TechnicalSkillCategoryModel aModel)
        {
            HttpResponseMessage result;
            List<TechnicalSkillCategoryModel> categoryList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    categoryList = service.SaveCategory(aModel);
                    if (categoryList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, categoryList);
                    }
                    else
                    {
                        string message = "Error while saving TechnicalSkillCategory data";
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


        [Route("tsc/list")]
        [HttpPost]
        public HttpResponseMessage GetTSCList(BaseViewModel aModel)
        {
            HttpResponseMessage result;
            List<TechnicalSkillCategoryModel> categoryList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    categoryList = service.GetCategoryList(aModel);
                    if (categoryList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, categoryList);
                    }
                    else
                    {
                        string message = "Error while retriving TechnicalSkillCategory list";
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


        [Route("tsc/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTSC(TechnicalSkillCategoryModel aModel)
        {
            HttpResponseMessage result;
            List< TechnicalSkillCategoryModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteCategory(aModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting TechnicalSkillCategory data";
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