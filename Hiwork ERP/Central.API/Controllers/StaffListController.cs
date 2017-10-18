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
    public class GetValue
    {
        public string Value { get; set; }
    }

    [Authorize]
    public class StaffListController : ApiController
    {
        IStaffListService _service;
        public StaffListController(IStaffListService service)
        {
            _service = service;
        }

        [Route("Stafflist/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffList(BaseViewModel model)
        {
            try
            {
                var staffList = _service.GetStaffList(model);
                if (staffList.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, staffList);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("Stafflist/conlist/{con}")]
        [HttpPost]
        public HttpResponseMessage ConList(BaseViewModel model, string con)
        {
            HttpResponseMessage result;
            try
            {
                if (this.ModelState.IsValid)
                {
                    var datalist = _service.GetSearchStaffList(model, con);
                    result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }
        [Route("Stafflist/getid")]
        [HttpPost]
        public HttpResponseMessage GetStaffList(GetValue staffNo)
        {
            try
            {
                var staffList = _service.GetStaffID(staffNo.Value);
                if (staffList.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, (object)staffList);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}