using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    //[Authorize]
    public class CountryController : ApiController
    {
        ICountryService service;
        public  CountryController(ICountryService _service)
        {
            service = _service;

        }


        [Route("country/save")]
        [HttpPost]
        public HttpResponseMessage SaveCountry(CountryModel aCountryModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var countryList = service.SaveCountry(aCountryModel);
                 
                    if (countryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, countryList);
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

        //GetAllUserRoleList

        [Route("country/list")]
        [HttpPost]
        public HttpResponseMessage GetAllCountry(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var countryList = service.GetAllCountryList(model);
                    if (countryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, countryList);
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
        [Route("country/tradinglist")]
        [HttpPost]
        public HttpResponseMessage GetTradingCountry(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var countryList = service.GetTradinCountry(model);
                    if (countryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, countryList);
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
        [Route("country/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCountry(CountryModel aCountryModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteCountry(aCountryModel);
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
        [Route("country/edit")]
        [HttpPost]
        public HttpResponseMessage EditCountry(CountryModel aCountryModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = service.EditCountry(aCountryModel);
                    if (user != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }
                    else
                    {
                        string message = "Not updated successfully";
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