using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;
using HiWork.Utils;

namespace Central.API.Controllers
{
    [Authorize]
    public class NotificationController : ApiController
    {
        private INotificationService _service;
        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [Route("notification/list")]
        [HttpPost]
        public HttpResponseMessage GetList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<NotificationModel> list;
            try
            {
                list = _service.GetUnapprovedList(model);
                if (list != null)
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    string message = "Error while retriving data";
                    result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }

        [Route("notification/unreadcount")]
        [HttpPost]
        public HttpResponseMessage GetUnreadCount(BaseViewModel model)
        {
            HttpResponseMessage result;
            int count = 0;
            try
            {
                var status = (int)EstimationApprovalStatus.Unread;
                count = _service.CountNotification(model, status);
                result = Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }

        [Route("notification/unapprovedcount")]
        [HttpPost]
        public HttpResponseMessage GetUnapprovedCount(BaseViewModel model)
        {
            HttpResponseMessage result;
            int count = 0;
            try
            {
                var status = (int)EstimationApprovalStatus.Approved;
                count = _service.CountNotification(model, status);
                result = Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }

        [Route("notification/updateasread")]
        [HttpPost]
        public HttpResponseMessage UpdateNotificationsAsRead(BaseViewModel model)
        {
            HttpResponseMessage result;
            try
            {
                var status = (int)EstimationApprovalStatus.Read;
                var res = _service.UpdateNotificationsAsRead(model);
                result = Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }
    }
}