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
    public class EstimationRoleAccessController : ApiController
    {
        IEstimationRoleAccessService _service;
        public EstimationRoleAccessController(IEstimationRoleAccessService service)
        {
            _service = service;
        }

        [Route("estimationroleaccess/getitem")]
        [HttpPost]
        public HttpResponseMessage GetRoles(EstimationRoleAccessModel model)
        {
            try
            {
                var items = _service.GetEstimationRoleAccess(model);
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }

    }
}
