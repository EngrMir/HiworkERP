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
   // [Authorize]
    public class MasterStaffSoftwareSkillController : ApiController
    {
        IMasterStaffSoftwareSkillService service;
        public MasterStaffSoftwareSkillController(IMasterStaffSoftwareSkillService _service)
        {
            service = _service;

        }
        [Route("staffsoftwareskill/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffSoftwareSkill(MasterStaffSoftwareSkillModel staff)
        {
            try
            {

                if (this.ModelState.IsValid)
                {
                    var softwareskill = service.SaveStaffSoftwareSkill(staff);
                    if (softwareskill != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, softwareskill);
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
        [Route("StaffSoftwareSkill/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffSoftwareSkill(BaseViewModel staff)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffsoftwareskilllist = service.GetStaffSoftwareSkill(staff);
                    if (staffsoftwareskilllist != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffsoftwareskilllist);
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
        [Route("StaffSoftwareSkill/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffSoftwareSkill(MasterStaffSoftwareSkillModel staff)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteStaffSoftwareSkill(staff);
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
