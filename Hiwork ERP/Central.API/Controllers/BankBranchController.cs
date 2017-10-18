using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{

    public class BankBranchController : ApiController
    {
        IBankBranchService service;
        public BankBranchController(IBankBranchService _service)
        {
            service = _service;
        }
        [Route("bankbranch/save")]
        [HttpPost]
        public HttpResponseMessage SaveBankBranch(BankBranchModel aBankBranchModel)
        {
      
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.SaveBankBranch(aBankBranchModel);

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

        [Route("bankbranch/list")]
        [HttpPost]
        public HttpResponseMessage GetAllBankBranch(BaseViewModel model)
        {
     
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.GetAllBankBranchList(model);
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

        [Route("bankbranch/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteBankBranch(BankBranchModel aBankBranchModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteBankBranch(aBankBranchModel);
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

        [Route("bankbranch/edit")]
        [HttpPost]
        public HttpResponseMessage EditBankBranch(BankBranchModel aBankBranchModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.UpdateBankBranch(aBankBranchModel);
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
    }
}