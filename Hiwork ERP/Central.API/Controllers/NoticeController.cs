using HiWork.BLL.Services;
using HiWork.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using HiWork.Utils.Infrastructure;
using HiWork.Utils;

namespace Central.API.Controllers
{
  //  [Authorize]
    public class NoticeController:ApiController
    {
        INoticeService service;
        public NoticeController(INoticeService _service)
        {
            service = _service;

        }
        [Route("notice/save")]
        [HttpPost]
        public HttpResponseMessage SaveNotice (NoticeModel aNoticeModel)
        {
            try
            {

                if (this.ModelState.IsValid)
                {
                    var notice = service.SaveNotice(aNoticeModel);
                    if (notice != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, notice);
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
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }


        }
        [Route("notice/list")]
        [HttpPost]
        public HttpResponseMessage GetNotice(BaseViewModel aNoticeModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var noticeList = service.GetNoticelist(aNoticeModel);
                    if (noticeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, noticeList);
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
        [Route("notice/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteNotice(NoticeModel aNoticeModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteNotice(aNoticeModel);
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

        [Route("notice/priority")]
        [HttpPost]
        public HttpResponseMessage GetNoticePriority(BaseViewModel model)
       {
            var priorityList = Utility.getItemCultureList(Utility.NoticePriorityList, model);

            return Request.CreateResponse(HttpStatusCode.OK, priorityList);
        }


    }
}