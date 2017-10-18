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
    public class StaffCurrentStatesController : ApiController
    {
        IStaffCurrentStateService service;
        public StaffCurrentStatesController(IStaffCurrentStateService _service)
        {
            service = _service;

        }


        [Route("staffcurrentstate/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffCurrentState(StaffCurrentStateModel amodel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffstatelist = service.SaveStaffCurrentState(amodel);

                    if (staffstatelist != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffstatelist);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }


        [Route("staffcurrentstate/savelist")]
        [HttpPost]
        public HttpResponseMessage SaveStaffCurrentState(List<StaffCurrentStateModel> amodel)
        {


            try
            {
                    bool IsSuccessful = service.SaveStaffCurrentStateList(amodel);

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