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
    public class JobTypeController : ApiController
    {
        //IJobTypeService _jobTypeService;
        //public JobTypeController(IJobTypeService jobTypeService)
        //{
        //    _jobTypeService = jobTypeService;
        //}
        //[Route("job/save")]
        //[HttpPost]
        //public HttpResponseMessage Save(JobTypeModel aJobTypeModel)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var JobTypeModelList = _jobTypeService.SaveJobType(aJobTypeModel);
        //            if (JobTypeModelList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, JobTypeModelList);
        //            }
        //            else
        //            {
        //                string message = "Error Saving Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }

        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }
        //}

        //[Route("job/list")]
        //[HttpPost]
        //public HttpResponseMessage GetJobTypeModels(JobTypeModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var JobTypeList = _jobTypeService.GetAllJobTypeList(model);
        //            if (JobTypeList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, JobTypeList);
        //            }
        //            else
        //            {
        //                string message = "Error in getting Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }

        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }

        //}

        //[Route("JobType/delete")]
        //[HttpPost]
        //public HttpResponseMessage DeleteJobType(JobTypeModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var result = _jobTypeService.DeleteJobType(model);
        //            if (result != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, result);
        //            }
        //            else
        //            {
        //                string message = "Not deleted successfully";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
        //    }

        //}
    }
}
