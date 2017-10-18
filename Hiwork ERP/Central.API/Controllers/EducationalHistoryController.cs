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
    public class EducationalHistoryController : ApiController
    {
        IEducationalHistoryService _educationalHistoryService;
        public EducationalHistoryController(IEducationalHistoryService educationalHistoryService)
        {
            _educationalHistoryService = educationalHistoryService;
        }
        #region Educational History
        [Route("educationalHistory/save")]
        [HttpPost]
        public HttpResponseMessage Save(EducationalHistoryModel aEducationalHistoryModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var EducationalHistoryModelList = _educationalHistoryService.SaveEducationalHistory(aEducationalHistoryModel);
                    if (EducationalHistoryModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, EducationalHistoryModelList);
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

        [Route("educationalHistory/list")]
        [HttpPost]
        public HttpResponseMessage GetEducationalHistoryModels(EducationalHistoryModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var EducationalHistoryList = _educationalHistoryService.GetAllEducationalHistoryList(model);
                    if (EducationalHistoryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, EducationalHistoryList);
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

        [Route("educationalHistory/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEducationalHistory(EducationalHistoryModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _educationalHistoryService.DeleteEducationalHistory(model);
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

        #endregion
    }
}
