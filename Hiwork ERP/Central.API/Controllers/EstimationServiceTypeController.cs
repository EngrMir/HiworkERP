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
    public class EstimationServiceTypeController : ApiController
    {

        IEstimationServiceTypeService _mesService;

        public EstimationServiceTypeController(IEstimationServiceTypeService mesService)
        {

            _mesService = mesService;

        }

        [Route("estimationServiceType/save")]
        [HttpPost]
        public HttpResponseMessage SaveMasterEstimationServiceType(EstimationServiceTypeModel aMEestiSR)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mesService.SaveEstimationServiceType(aMEestiSR);
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


        //Get All Master Edition Service Type

        [Route("estimationServiceType/list/{type}")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationServiceTypeList(EstimationServiceTypeModel model, string type)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mesService.GetAllEstimationServiceTypeList(model, type);
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

        [Route("estimationServiceType/list")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationService(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mesService.GetAllEstimationService(model);
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




        [Route("estimationServiceType/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEstimationServiceType(EstimationServiceTypeModel merModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _mesService.DeleteEstimationServiceType(merModel);
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