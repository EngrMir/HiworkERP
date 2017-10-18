

/* ******************************************************************************************************************
 * Controller for Master_StaffType Entity
 * Date             :   13-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    [Authorize]
    public class WorkingStatusController : ApiController
    {
        private IWorkingStatusService service;
        public WorkingStatusController(IWorkingStatusService ser)
        {
            service = ser;
        }

        [Route("workingstatus/list")]
        [HttpPost]
        public HttpResponseMessage GetList(BaseViewModel sModel)
        {
            HttpResponseMessage result;
            List<WorkingStatusModel> modelList;
            try
            {
                modelList = service.GetAllList(sModel);
                if (modelList != null)
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, modelList);
                }
                else
                {
                    string message = "Error while retriving WorkingStatus list";
                    result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }
    }
}