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
    public class NarrationInformationController : ApiController
    {
        [Route("narrationinformation/save")]
        [HttpPost]
        public HttpResponseMessage Save(NarrationInformationModel aNarrationInformationModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            INarrationInformationRepository narrationInformation = new NarrationInformationRepository(uWork);
            INarrationInformationService narrationInformationService = new NarrationInformationService(narrationInformation);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var narrationInformationModelList = narrationInformationService.SaveNarrationInformation(aNarrationInformationModel);
                    if (narrationInformationModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, narrationInformationModelList);
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

        [Route("narrationinformation/list")]
        [HttpPost]
        public HttpResponseMessage GetNarrationInformationModels(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            INarrationInformationRepository narrationInformation = new NarrationInformationRepository(uWork);
            INarrationInformationService narrationInformationService = new NarrationInformationService(narrationInformation);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var narrationInformationList = narrationInformationService.GetAllNarrationInformationList(model);
                    if (narrationInformationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, narrationInformationList);
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

        [Route("narrationinformation/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteNarrationInformation(NarrationInformationModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            INarrationInformationRepository narrationInformation = new NarrationInformationRepository(uWork);
            INarrationInformationService narrationInformationService = new NarrationInformationService(narrationInformation);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = narrationInformationService.DeleteNarrationInformation(model);
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
