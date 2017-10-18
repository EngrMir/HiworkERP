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
    //[Authorize]
    public class StaffSoftwareSkillController : ApiController
    {
        IStaffSoftwareService service;
        public StaffSoftwareSkillController(IStaffSoftwareService _service)
        {
            service = _service;
        }
        //[Route("staffsoftware/save")]
        //[HttpPost]
        //public HttpResponseMessage SaveStaffSoftwareSkill(StaffSoftwareSkillModel staff)
        //{
        //    try
        //    {

        //        if (this.ModelState.IsValid)
        //        {
        //            var software = service.SaveStaffSoftware(staff);
        //            if (software != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, software);
        //            }
        //            else
        //            {
        //                string message = "Error Saving Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

        //    }
        //}



        [Route("staffSoftwareSkill/savelist")]
        [HttpPost]
        public HttpResponseMessage SavestaffSoftwareSkill(List<StaffSoftwareSkillModel> amodel)
        {
            try
            {
                bool IsSuccessful = service.staffSoftwareSkill(amodel);

                if (IsSuccessful == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, IsSuccessful);
                }
                else
                {
                    string message = "Error Saving Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }


        [Route("StaffSoftware/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffSoftware(BaseViewModel staff)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffsoftwarelist = service.GetStaffSoftware(staff);
                    if (staffsoftwarelist != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffsoftwarelist);
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



    }
}
