using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
   // [Authorize]
    public class BankController : ApiController
    {
        IBankService service;
        public BankController(IBankService _service)
        {
            service = _service;
        }
        [Route("bank/save")]
        [HttpPost]
        public HttpResponseMessage Save(BankModel aBankModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var bankList = service.SaveBank(aBankModel);
                    if (bankList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, bankList);
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


        //GetAllUserBankList

        [Route("bank/list")]
        [HttpPost]
        public HttpResponseMessage GetBanks(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var bankList = service.GetAllBankList(model);
                    if (bankList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, bankList);
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

        [Route("bank/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteBanks(BankModel aBankModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteBank(aBankModel);
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
