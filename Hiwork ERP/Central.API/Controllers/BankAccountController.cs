using System;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using System.Net;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class BankAccountController : ApiController
    {
        IBankAccountService service;
        public BankAccountController(IBankAccountService _service)
        {
            service = _service;
        }
        [Route("bankaccount/save")]
        [HttpPost]
        public HttpResponseMessage SaveBankAccount(BankAccountModel aBankAccountModel)
        {
        
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.SaveBankAccount(aBankAccountModel);

                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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


        [Route("bankaccount/list")]
        [HttpPost]
        public HttpResponseMessage GetAllBankAccount(BankAccountModel aBankAccountModel)
        {
        
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.GetAllBankAccountList(aBankAccountModel);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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
        [Route("bankaccount/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteBankAccount(BankAccountModel aBankAccountModel)
        {
          
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteBankAccount(aBankAccountModel);
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
        [Route("bankaccount/edit")]
        [HttpPost]
        public HttpResponseMessage EditBankAccount(BankAccountModel aBankAccountModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.UpdateBankAccount(aBankAccountModel);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("bankAccountType/list")]
        [HttpPost]
        public HttpResponseMessage GetAllBankAccountType(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.GetAllBankAccountType(model);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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
