using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Central.API.Controllers
{
    //[Authorize]
    public class EmailDeliverySettingsController : ApiController
    {
        IEmailDeliverySettingsService service;
        public EmailDeliverySettingsController(IEmailDeliverySettingsService _service)
        {
            service = _service;
        }

        [Route("emaildeliverysettings/save")]
        [HttpPost]
        public HttpResponseMessage Save(EmailDeliverySettingsModel vmodel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
 
                        var result = service.SaveUpdateEntity(vmodel);
                     return Request.CreateResponse(HttpStatusCode.OK, result);
 
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


        [Route("emaildeliverysettings/getByID")]
        [HttpPost]
        public HttpResponseMessage GetByID(EmailDeliverySettingsModel vmodel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {

                    var result = service.GetByID(vmodel);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

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