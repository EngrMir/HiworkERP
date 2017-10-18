using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    public class StaffProfessionalSpecialityController : ApiController
    {
        IStaffProfessionalSpecialityService service;

        public StaffProfessionalSpecialityController(IStaffProfessionalSpecialityService _service)
        {
            service = _service;

        }

        
        [Route("staffprofessionalspecial/savelist")]
        [HttpPost]
        public HttpResponseMessage SaveProfessionalSpecial(List<StaffProfesionalSpecialityModel> amodel)
        {
            try
            {
                bool IsSuccessful = service.SaveStaffProfessionalList(amodel);

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

    }


}