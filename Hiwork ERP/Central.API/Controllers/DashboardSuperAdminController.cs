using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.IO;
using System.Net.Http.Headers;

namespace Central.API.Controllers
{
    //[Authorize]
    public class DashboardSuperAdminController : ApiController
    {
        IDashboardSuperAdminService _service;
        public DashboardSuperAdminController(IDashboardSuperAdminService service)
        {
            _service = service;
        }

        [Route("dashboardsuperadmin/getdata")]
        [HttpPost]
        public HttpResponseMessage GetData(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var data = _service.GetSAdminDashboardData(model);
                    return Request.CreateResponse(HttpStatusCode.OK, data);
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
    }
}