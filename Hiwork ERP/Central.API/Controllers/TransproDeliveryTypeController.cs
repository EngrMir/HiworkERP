//Developed by Tamal Roy 


using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    //[Authorize]
    public class TransproDeliveryTypeController : ApiController
    {
        ITransproDeliveryPlanService service;

        public TransproDeliveryTypeController(ITransproDeliveryPlanService _service)
        {
            service = _service;
        }

        [Route("transproDeliveryPlan/list")]
        [HttpPost]
        public HttpResponseMessage Get(BaseViewModel atdtModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var UnitPriceList = service.GetAllTransproDeliveryType(atdtModel);
                    if (UnitPriceList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, UnitPriceList);
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