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
    public class JobCategoryController : ApiController
    {
        IJobCategoryService _jobCategoryService;
        public JobCategoryController(IJobCategoryService jobCategoryService)
        {
            _jobCategoryService = jobCategoryService;
        }
        [Route("jobcategory/save")]
        [HttpPost]
        public HttpResponseMessage Save(JobCategoryModel aJobCategoryModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var JobCategoryModelList = _jobCategoryService.SaveJobCategory(aJobCategoryModel);
                    if (JobCategoryModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobCategoryModelList);
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

        [Route("jobcategory/list")]
        [HttpPost]
        public HttpResponseMessage GetJobCategoryModels(JobCategoryModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var JobCategoryList = _jobCategoryService.GetAllJobCategoryList(model);
                    if (JobCategoryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobCategoryList);
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

        [Route("jobcategory/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteJobCategory(JobCategoryModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _jobCategoryService.DeleteJobCategory(model);
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
