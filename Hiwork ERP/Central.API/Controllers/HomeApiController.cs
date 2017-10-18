using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using HiWork.DAL.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http.Headers;
using HiWork.Utils;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    


    [Authorize]
    public class HomeApiController : ApiController
    {
        IHomeApiService _service;
        public HomeApiController(IHomeApiService service) {
            _service = service;
        }

        [Route("homeapi/searchorder")]
        [HttpPost]
        public HttpResponseMessage SearchOrder(HomeSearchModel model)
        {
            try
            {
                var ret = _service.Get_OrderDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("homeapi/searchstaff")]
        [HttpPost]
        public HttpResponseMessage SearchStaff(HomeSearchModel model)
        {
            try
            {
                var ret = _service.Get_StaffDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("homeapi/searchclient")]
        [HttpPost]
        public HttpResponseMessage SearchClient(HomeSearchModel model)
        {
            try
            {
                var ret = _service.Get_ClientDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}