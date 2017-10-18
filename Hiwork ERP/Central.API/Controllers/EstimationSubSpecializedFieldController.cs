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
    //[Authorize]
    public class EstimationSubSpecializedFieldController : ApiController
    {

        IEstimationSubSpecializedFieldService _essfService;

        public EstimationSubSpecializedFieldController(IEstimationSubSpecializedFieldService essfService)
        {

            _essfService = essfService;

        }

        [Route("estimationssf/save")]
        [HttpPost]
        public HttpResponseMessage SaveEstimationSubSpecializedField(EstimationSubSpecializedFieldModel aESSFM)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _essfService.SaveEstimationSubSpecializedField(aESSFM);
                    if (mecList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mecList);
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


        //Get All Estimation Specialized Field List

        [Route("estimationssf/list")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationSubSpecializedFieldList(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mescList = _essfService.GetAllEstimationSubSpecializedFieldList(model);
                    if (mescList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mescList);
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


        [Route("estimationssf/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEstimationSubSpecializedField(EstimationSubSpecializedFieldModel messModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _essfService.DeleteEstimationSubSpecializedField(messModel);
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