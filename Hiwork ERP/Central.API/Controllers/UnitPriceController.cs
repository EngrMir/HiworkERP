//Developed by Tamal Roy 


using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    [Authorize]
    public class UnitPriceController : ApiController
    {
        IUnitPriceService service;
        public UnitPriceController(IUnitPriceService _service)
        {
            service = _service;
        }
        [Route("UnitPrice/save")]
        [HttpPost]
        public HttpResponseMessage SaveUnitPrice(UnitPriceModel aUnitPriceModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var UnitPriceList = service.SaveUnitPrice(aUnitPriceModel);
                    if (UnitPriceList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, UnitPriceList);
                    }
                    else
                    {
                        string message = "Source language or target language already exist in the database";
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


        //GetUnitPriceList

        [Route("UnitPrice/list")]
        [HttpPost]
        public HttpResponseMessage GetUnitPrices(BaseViewModel aUnitPriceModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var UnitPriceList = service.GetAllUnitPrice(aUnitPriceModel);
                    if (UnitPriceList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, UnitPriceList);
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

        [Route("UnitPrice/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteUnitPrice(UnitPriceModel aUnitPriceModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteUnitPrice(aUnitPriceModel);
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
        //[Route("UnitPrice/matchin")]
        //[HttpPost]
        //public HttpResponseMessage GetGeneralUnitPrice(UnitPriceModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var GeneralUnitPrice = service.GetGeneralUnitPriceByID(model);
        //            if (GeneralUnitPrice != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, GeneralUnitPrice);
        //            }
        //            else
        //            {
        //                string message = "Error in getting Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }





        //}
    }
}
