using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;

namespace Central.API.Controllers
{
    public class OrderStaffAllowanceController : ApiController
    {
        IOrderStaffAllowanceService _service;
        public OrderStaffAllowanceController( IOrderStaffAllowanceService service)
        {
            _service = service;
        }

        [Route("orderstaffallowance/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffAllowance ( List<OrderStaffAllowanceModel>  model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var countryList = _service.SaveStaffAllowance(model);

                    if (countryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, countryList);
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

        [Route("orderstaffallowance/getstaffallowancelist/{ID}")]
        [HttpPost]
        public HttpResponseMessage GetAllowanceListByID(OrderStaffAllowanceModel model, Guid ID)
        {
            try
            {
                var estimation = _service.GetAllStaffAllowance(model, ID);
                if (estimation != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, estimation);
                }
                else
                {
                    string message = "Error in getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
