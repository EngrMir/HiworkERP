using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net.Http.Formatting;

namespace Central.API.Controllers
{
    [Authorize]
    public class OrderDetailsController : ApiController
    {
        IOrderDetailsService service;
        public OrderDetailsController(IOrderDetailsService _service)
        {
            service = _service;
        }
        
        [Route("orderdetails/save")]
        [HttpPost]
        public HttpResponseMessage Save(CommonModelHelper model)
        {
            try
            {
                var isSuccess = service.Save(model);
                if (isSuccess)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, isSuccess);
                }
                else
                {
                    string message = "Error Saving Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("orderdetails/list/{OrderId}")]
        [HttpPost]
        public HttpResponseMessage GetOrderDetailsList(BaseViewModel model, Guid OrderId)
        {
            try
            {
                var items = service.GetOrderDetailsList(model, OrderId);
                if (items != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, items);
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

        [Route("order/orderitem/{EstimationId}")]
        [HttpPost]
        public HttpResponseMessage GetOrder(BaseViewModel model, Guid EstimationId)
        {
            try
            {
                var order = service.GetOrder(model, EstimationId) ?? new OrderModel();
                if (order != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, order);
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
        
        [Route("orderdetails/estimationorderdetailslist/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetDetailsList(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                var items = service.GetDetailsList(model, EstimationID);
                if (items != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, items);
                }
                else
                {
                    string message = "Error in getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }
        
        [Route("order/profitsharesetting/")]
        [HttpPost]
        public HttpResponseMessage GetProfitShareSetting(BaseViewModel model)
        {
            try
            {
                var items = service.getProfitShareSetting();
                if (items != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, items);
                }
                else
                {
                    string message = "Error in getting Data";
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
