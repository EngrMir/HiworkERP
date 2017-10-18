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
    public class HiworkLanguagePriceController : ApiController
    {
        IHiworkLanguagePriceService service;
        public HiworkLanguagePriceController(IHiworkLanguagePriceService _service)
        {
            service = _service;
        }
        [Route("hiworklanguageprice/save")]
        [HttpPost]
        public HttpResponseMessage Save(HiworkLanguagePriceModel aHiworkLanguagePriceModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var HiworkLanguagePriceList = service.SaveHiworkLanguagePrice(aHiworkLanguagePriceModel);
                    if (HiworkLanguagePriceList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, HiworkLanguagePriceList);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }


        //GetAllUserHiworkLanguagePriceList

        [Route("hiworklanguageprice/list")]
        [HttpPost]
        public HttpResponseMessage GetHiworkLanguagePrices(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var HiworkLanguagePriceList = service.GetAllHiworkLanguagePriceList(model);
                    if (HiworkLanguagePriceList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, HiworkLanguagePriceList);
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

        [Route("hiworklanguageprice/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteHiworkLanguagePrices(HiworkLanguagePriceModel aHiworkLanguagePriceModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteHiworkLanguagePrice(aHiworkLanguagePriceModel);
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
