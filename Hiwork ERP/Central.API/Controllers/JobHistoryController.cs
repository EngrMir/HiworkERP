using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class JobHistoryController : ApiController
    {
        IJobHistoryService service;
        public JobHistoryController(IJobHistoryService _service)
        {
            service = _service;
        }
        [Route("jobhistory/save")]
        [HttpPost]
        public HttpResponseMessage Save(JobHistoryModel aJobHistoryModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var jobHistoryList = service.SaveJobHistory(aJobHistoryModel);
                    if (jobHistoryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, jobHistoryList);
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


        //GetAllUserjobHistoryList

        [Route("jobhistory/list")]
        [HttpPost]
        public HttpResponseMessage GetJobHistorys(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var jobHistoryList = service.GetAllJobHistoryList(model);
                    if (jobHistoryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, jobHistoryList);
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

        [Route("jobhistory/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteJobHistorys(JobHistoryModel aJobHistoryModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteJobHistory(aJobHistoryModel);
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
