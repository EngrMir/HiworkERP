

/* ******************************************************************************************************************
 * Controller for Master_StaffDevelopmentSkill Entity
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
    
    public class StaffDevelopmentSkillController : ApiController
    {
        private IStaffDevelopmentSkillService service;

        public StaffDevelopmentSkillController(IStaffDevelopmentSkillService ser)
        {
            service = ser;
        }


        [Route("staffdevelopmentskill/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffDevelopmentSkill(StaffDevelopmentSkillModel arg)
        {
            HttpResponseMessage result;
            List<StaffDevelopmentSkillModel> dataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.SaveStaffDevelopmentSkill(arg);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while saving StaffDevelopmentSkill data";
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


        [Route("staffdevelopmentskill/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffDevelopmentSkillList(BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<StaffDevelopmentSkillModel> dataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetStaffDevelopmentSkillList(arg);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving StaffDevelopmentSkill list";
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


        [Route("staffdevelopmentskill/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffDevelopmentSkill(StaffDevelopmentSkillModel arg)
        {
            HttpResponseMessage result;
            List<StaffDevelopmentSkillModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteStaffDevelopmentSkill(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting StaffDevelopmentSkill data";
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