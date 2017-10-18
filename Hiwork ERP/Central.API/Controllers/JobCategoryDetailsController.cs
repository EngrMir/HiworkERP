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
    public class JobCategoryDetailsController : ApiController
    {
        IJobCategoryDetailsService _jobCategoryDetailsService;
        public JobCategoryDetailsController(IJobCategoryDetailsService jobCategoryDetailsService)
        {
            _jobCategoryDetailsService = jobCategoryDetailsService;
        }

        [Route("jobcategorydetails/save")]
        [HttpPost]
        public HttpResponseMessage Save(JobCategoryDetailsModel aJobCategoryDetailsModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var JobCategoryDetailsModelList = _jobCategoryDetailsService.SaveJobCategoryDetails(aJobCategoryDetailsModel);
                    if (JobCategoryDetailsModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobCategoryDetailsModelList);
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

        [Route("jobcategorydetails/list")]
        [HttpPost]
        public HttpResponseMessage GetJobCategoryDetailsModels(JobCategoryDetailsModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var JobCategoryDetailsList = _jobCategoryDetailsService.GetAllJobCategoryDetailsList(model);
                    if (JobCategoryDetailsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobCategoryDetailsList);
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

        [Route("jobcategorydetails/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteJobCategoryDetails(JobCategoryDetailsModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _jobCategoryDetailsService.DeleteJobCategoryDetails(model);
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
