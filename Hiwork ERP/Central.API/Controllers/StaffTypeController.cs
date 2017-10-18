

/* ******************************************************************************************************************
 * Controller for Master_StaffType Entity
 * Date             :   13-Jun-2017
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
    public class StaffTypeController : ApiController
    {
        private IStaffTypeService service;
        public StaffTypeController(IStaffTypeService ser)
        {
            service = ser;
        }

        [Route("stafftype/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffType(StaffTypeModel sModel)
        {
            HttpResponseMessage result;
            List<StaffTypeModel> modelList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    modelList = service.SaveStaffType(sModel);
                    if (modelList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, modelList);
                    }
                    else
                    {
                        string message = "Error while saving StaffType data";
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

        [Route("stafftype/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffTypeList(BaseViewModel sModel)
        {
            HttpResponseMessage result;
            List<StaffTypeModel> modelList;

            try
            {
                if(this.ModelState.IsValid == true)
                {
                    modelList = service.GetStaffTypeList(sModel);
                    if (modelList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, modelList);
                    }
                    else
                    {
                        string message = "Error while retriving StaffType list";
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


        [Route("stafftype/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffType(StaffTypeModel sModel)
        {
            HttpResponseMessage result;
            List<StaffTypeModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteStaffType(sModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting StaffType data";
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