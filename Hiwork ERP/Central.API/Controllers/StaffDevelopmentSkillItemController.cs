

/* ******************************************************************************************************************
 * Controller for Master_StaffDevelopmentSkillItem Entity
 * Date             :   19-July-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;

namespace Central.API.Controllers
{

    public class StaffDevelopmentSkillItemController : ApiController
    {
        private IStaffDevelopmentSkillItemService service;
        
        public StaffDevelopmentSkillItemController(IStaffDevelopmentSkillItemService ser)
        {
            service = ser;
        }


        [Route("staffdevelopmentskillitem/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffDevelopmentSkillItem(StaffDevelopmentSkillItemModel arg)
        {
            HttpResponseMessage result;
            List<StaffDevelopmentSkillItemModel> dataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.SaveStaffDevelopmentSkillItem(arg);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while saving StaffDevelopmentSkillItem data";
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


        [Route("staffdevelopmentskillitem/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffDevelopmentSkillItemList(BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<StaffDevelopmentSkillItemModel> dataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetStaffDevelopmentSkillItemList(arg);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving StaffDevelopmentSkillItem list";
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


        [Route("staffdevelopmentskillitem/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffDevelopmentSkillItem(StaffDevelopmentSkillItemModel arg)
        {
            HttpResponseMessage result;
            List<StaffDevelopmentSkillItemModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteStaffDevelopmentSkillItem(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting StaffDevelopmentSkillItem data";
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