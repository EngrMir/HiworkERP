using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class ErrorReportWebController : ApiController
    {
        IErrorReportWebService service;

        public ErrorReportWebController(ErrorReportWebService _service)
        {
            service = _service;
        }

        [Route("ErrorReportWeb/save")]
        [HttpPost]
        public HttpResponseMessage SaveErrorReportWeb(ErrorReportWebModel errModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var errList = service.SaveErrorReportWeb(errModel);
                    if (errList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, errList);
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
        
    }
}