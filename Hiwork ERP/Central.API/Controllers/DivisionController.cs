
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{

  //  [Authorize]
    public class DivisionController : ApiController
    {
        private IDivisionService service;

        public DivisionController(IDivisionService ser)
        {
            service = ser;
        }

        [Route("division/save")]
        [HttpPost]
        public HttpResponseMessage SaveDivision(DivisionModel aDivisionModel)
        {
            HttpResponseMessage result;
            List<DivisionModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveDivision(aDivisionModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving Division data";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }
        

        [Route("division/list")]
        [HttpPost]
        public HttpResponseMessage GetDivisionList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<DivisionModel> dataList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetDivisionList(model);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving Division list";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }


        [Route("division/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteDivision(DivisionModel aDivisionModel)
        {
            HttpResponseMessage result;
            List<DivisionModel> datalist;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteDivision(aDivisionModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting Division data";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
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
