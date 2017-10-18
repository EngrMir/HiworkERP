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
    public class CurrencyController : ApiController
    {
        [Route("currency/list")]
        [HttpPost]
        public HttpResponseMessage GetAllCurrency(BaseViewModel aCurrencyModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ICurrencyRepository repo = new CurrencyRepository(uWork);
            ICurrencyService Service = new CurrencyService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var currencyList = Service.GetAllCurrencyList(aCurrencyModel);
                    if (currencyList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, currencyList);
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
    }
}
