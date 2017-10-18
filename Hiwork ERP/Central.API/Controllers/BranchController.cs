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
    [Authorize]
    public class BranchController : ApiController
    {
        IBranchService service;
        public BranchController(IBranchService _service)
        {
            service = _service;
        }

        #region Branch

        [Route("branch/save")]
        [HttpPost]
        public HttpResponseMessage Save(BranchModel aBranchModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var branchList = service.SaveBranch(aBranchModel);
                    if (branchList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, branchList);
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

       
        //GetAllUserBranchList

        [Route("branch/list")]
        [HttpPost]
        public HttpResponseMessage GetBranches(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var branchList = service.GetAllBranchList(model);
                    if (branchList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, branchList);
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


        [Route("branch/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteBranches(BranchModel aBranchModel)
        {
   

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteBranch(aBranchModel);
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


        [Route("branch/edit")]
        [HttpPost]
        public HttpResponseMessage EditBranches(BranchModel aBranchModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = service.EditBranch(aBranchModel);
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

        [Route("branch/getByBranchId/{countryId}/{culture}")]
        [HttpGet]
        public HttpResponseMessage GetByBranchId(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var branchList = service.GetAllBranchList(model);
                    if (branchList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, branchList);
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

        #endregion
    }
}