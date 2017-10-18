/* ******************************************************************************************************************
 * Service for Estimation Interpretation Controller
 * Programmed by    :   Md. Al-Amin Hossain (b-Bd_14 Hossain)
 * Date             :   22-Sep-2017
 * *****************************************************************************************************************/

using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{
    public class EstimationInterpretationController : ApiController
    {
        IEstimationInterpretationService _service;
        public EstimationInterpretationController(IEstimationInterpretationService service)
        {
            _service = service;
        }



        [Route("estimationinterpretation/save")]
        [HttpPost]
        public HttpResponseMessage Save(EstimationModel model)
        {
            try
            {
                // if (this.ModelState.IsValid)
                //{
                var estimationList = _service.SaveEstimation(model);
                if (estimationList)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, estimationList);
                }
                else
                {
                    string message = "Error Saving Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
               //}
                //else
                //{
                //   return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimationinterpretation/detailslist/{ID}")]
        [HttpPost]
        public HttpResponseMessage GetSpecificEstimation(EstimationDetailsModel model, Guid ID)
        {
            try
            {
                var estimation = _service.GetEstimationDetailsByID(model, ID);
                if (estimation != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, estimation);
                }
                else
                {
                    string message = "Error in getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimationinterpretation/filelist/{ID}")]
        [HttpPost]
        public HttpResponseMessage GetFileList(EstimationFileModel model, Guid ID)
        {
            try
            {
                var estimation = _service.GetFileListByID(model, ID);
                if (estimation != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, estimation);
                }
                else
                {
                    string message = "Error in getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimationinterpretation/getorderidbyestimationid/{ID}")]
        [HttpPost]
        public HttpResponseMessage GetOrderID(EstimationModel model, Guid id)
        {
            try
            {
                var getorder = _service.GetOrderIDByID(model, id);
                if(getorder !=null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getorder);
                }
                else
                {
                    string message = "Error in Getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimationinterpretation/getstaffallowancebyid/{ID}")]
        [HttpPost]
        public HttpResponseMessage GetStaffAllowanceList( Guid ID)
        {
            try
            {
                var staffallowancelist = _service.GetStaffAllowanceListByID( ID);
                if (staffallowancelist != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, staffallowancelist);
                }
                else
                {
                    string message = "Error in Getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
