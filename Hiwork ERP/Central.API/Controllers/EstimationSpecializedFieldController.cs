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

    public class EstimationSpecializedFieldController : ApiController
    {

        IEstimationSpecializedFieldService _mesfService;

        public EstimationSpecializedFieldController(IEstimationSpecializedFieldService mesfService)
        {

            _mesfService = mesfService;

        }

        [Route("estSpecializedFields/save")]
        [HttpPost]
        public HttpResponseMessage SaveEstimationSpecializedField(EstimationSpecializedFieldModel aMERM)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mesfService.SaveEstimationSpecializedField(aMERM);
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

        [Route("estSpecializedFields/list")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationSpecializedFieldList(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mesfService.GetAllEstimationSpecializedFieldList(model);
                    if (mecList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mecList);
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


        [Route("estSpecializedFields/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEstimationSpecializedField(EstimationSpecializedFieldModel merModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _mesfService.DeleteEstimationSpecializedField(merModel);
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