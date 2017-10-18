using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using HiWork.DAL.Database;

namespace Central.API.Controllers
{
    [Authorize]
    public class EstimationActionController : ApiController
    {
        IEstimationActionService _service;
        public EstimationActionController(IEstimationActionService service)
        {
            _service = service;
        }

        [Route("estimationaction/save")]
        [HttpPost]
        public HttpResponseMessage Save(CommonModelHelper model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var estimationList = _service.Save(model);
                    if (estimationList)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, estimationList);
                    }
                    else
                    {
                        string message = "Error Saving Data";
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        
        [Route("estimationaction/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetAllEstimationActions(BaseViewModel model, Guid estimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var items = _service.GetAllActionListByEstimation(model, estimationID);
                    if (items != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, items);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimationaction/delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                var isSuccess = _service.Delete(id);
                if (isSuccess)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data deleted successfully");
                }
                else
                {
                    string message = "Error in deleting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}