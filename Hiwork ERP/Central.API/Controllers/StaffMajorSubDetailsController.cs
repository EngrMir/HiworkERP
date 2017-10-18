using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Responses;
using HiWork.BLL.ViewModels;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{

    [Authorize]
    public class StaffMajorSubDetailsController : ApiController
    {

        IMajorSubjectDetailsService _smsdService;

        public StaffMajorSubDetailsController(IMajorSubjectDetailsService smsdService)
        {

            _smsdService = smsdService;

        }

        [Route("smsd/save")]
        [HttpPost]

        public HttpResponseMessage SaveMajorSubjectDetails(MajorSubjectDetailsModel aMajorSubjectDetailsModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var smsdList = _smsdService.SaveMajorSubjectDetails(aMajorSubjectDetailsModel);
                    if (smsdList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, smsdList);
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

        //Get all list

        [Route("smsd/list")]
        [HttpPost]
        public HttpResponseMessage GetAllMajorSubjectDetailsList(MajorSubjectDetailsModel model)
        {
          
            try
            {
                if (this.ModelState.IsValid)
                {
                    var smsdList = _smsdService.GetAllMajorSubjectDetailsList(model);
                    if (smsdList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, smsdList);
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

        [Route("smsd/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteMajorSubjectDetails(MajorSubjectDetailsModel aMajorSubjectDetailsModel)
        {
         

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _smsdService.DeleteMajorSubjectDetails(aMajorSubjectDetailsModel);
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