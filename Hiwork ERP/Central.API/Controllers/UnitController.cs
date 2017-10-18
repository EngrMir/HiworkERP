

/* ******************************************************************************************************************
 * Controller for Master_Unit Entity
 * Date             :   02-July-2017
 * By               :   Ashis
 * Updated by       :   Tamal
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
    [EnableCors("*", "*", "*")]
    public class UnitController : ApiController
    {
        private IUnitService service;
        public UnitController(IUnitService arg)
        {
            service = arg;
        }

        [Route("unit/list")]
        [HttpPost]
        public HttpResponseMessage GetUnitList(BaseViewModel arg)
        {
            HttpResponseMessage result;
            List<UnitModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.GetUnitList(arg);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving Unit list";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }


        [Route("unit/save")]
        [HttpPost]
        public HttpResponseMessage SaveUnit(UnitModel aUnit)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var unitlist = service.SaveUnit(aUnit);
                    if (unitlist != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, unitlist);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("unit/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteUnit(UnitModel model)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteUnit(model);
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