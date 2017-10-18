

/* ******************************************************************************************************************
 * Controller for Master_StaffTechnicalSkillItems Entity
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
    public class TechnicalSkillItemsController : ApiController
    {
        private ITechnicalSkillItemsService service;
        public TechnicalSkillItemsController(ITechnicalSkillItemsService arg)
        {
            service = arg;
        }

        [Route("tsi/save")]
        [HttpPost]
        public HttpResponseMessage SaveItem(TechnicalSkillItemsModel aModel)
        {
            HttpResponseMessage result;
            List<TechnicalSkillItemsModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveItem(aModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving TechnicalSkillItems data";
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


        [Route("tsi/list")]
        [HttpPost]
        public HttpResponseMessage GetItemsList(BaseViewModel aModel)
        {
            HttpResponseMessage result;
            List<TechnicalSkillItemsModel> itemList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    itemList = service.GetItemList(aModel);
                    if (itemList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, itemList);
                    }
                    else
                    {
                        string message = "Error while retriving TechnicalSkillItems list";
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


        [Route("tsi/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteItem(TechnicalSkillItemsModel aModel)
        {
            HttpResponseMessage result;
            List<TechnicalSkillItemsModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteItem(aModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting TechnicalSkillItems data";
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