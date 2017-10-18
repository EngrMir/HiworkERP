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
    public class EstimationRoutesController : ApiController
    {

        IEstimationRoutesService _merService;

        public EstimationRoutesController(IEstimationRoutesService merService)
        {

            _merService = merService;

        }

        [Route("estimationRoute/save")]
        [HttpPost]
        public HttpResponseMessage SaveMasterEstimationRoutes(EstimationRoutesModel aMERM)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _merService.SaveEstimationRoutes(aMERM);
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


        //GetAllEstimationRoutesList

        [Route("estimationRoute/list")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationRoutesList(EstimationRoutesModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _merService.GetAllEstimationRoutesList(model);
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


        [Route("estimationRoute/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteMasterEstimationRoutes(EstimationRoutesModel merModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _merService.DeleteEstimationRoutes(merModel);
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