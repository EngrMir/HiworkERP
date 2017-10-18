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
   // [Authorize]
    public class CurrentStateController : ApiController
    {
        [Route("currentstate/save")]
        [HttpPost]
        public HttpResponseMessage Save(CurrentStateModel aCurrentStateModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ICurrentStateRepository repo = new CurrentStateRepository(uWork);
            ICurrentStateService currentStateService = new CurrentStateService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var CurrentStateList = currentStateService.SaveCurrentState(aCurrentStateModel);
                    if (CurrentStateList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, CurrentStateList);
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

        //GetAllUserCurrentStateList

        [Route("currentstate/list")]
        [HttpPost]
        public HttpResponseMessage GetCurrentStates(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ICurrentStateRepository repo = new CurrentStateRepository(uWork);
            ICurrentStateService currentStateService = new CurrentStateService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var CurrentStateList = currentStateService.GetAllCurrentStateList(model);
                    if (CurrentStateList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, CurrentStateList);
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
        [Route("currentstate/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCurrentStates(CurrentStateModel aCurrentStateModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ICurrentStateRepository repo = new CurrentStateRepository(uWork);
            ICurrentStateService currentStateService = new CurrentStateService(repo);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = currentStateService.DeleteCurrentState(aCurrentStateModel);
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
