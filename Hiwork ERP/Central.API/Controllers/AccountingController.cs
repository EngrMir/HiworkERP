using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{
    public class AccountingController : ApiController
    {

        IAccountingService accountingService;
        public AccountingController()
        {
            accountingService = new AccountingService();
        }



        [Route("staffpayment/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffPayment(BaseViewModel model)
        {
            HttpResponseMessage result;
       
            try
            {
                if (this.ModelState.IsValid)
                {
                    var Data = accountingService.GetStaffPaymentList(model);
                    result = Request.CreateResponse(HttpStatusCode.OK, Data);
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
    }
}


