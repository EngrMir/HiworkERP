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
    public class EstimationTypeController : ApiController
    {

        IEstimationTypeService _mecService;

        public EstimationTypeController(IEstimationTypeService mecService)
        {

            _mecService = mecService;

        }

        [Route("estimationType/save")]
        [HttpPost]
        public HttpResponseMessage SaveEstimationType(EstimationTypeModel aMECM)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mecService.SaveEstimationType(aMECM);
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


        //GetAllEstimationTypeList

        [Route("estimationType/list")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationTypeList(BaseViewModel model)
        {
          
            try
            {
                if (this.ModelState.IsValid)
                {
                    var mecList = _mecService.GetAllEstimationTypeList(model);
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


        [Route("estimationType/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEstimationType(EstimationTypeModel mecModel)
        {
            

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _mecService.DeleteEstimationType(mecModel);
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