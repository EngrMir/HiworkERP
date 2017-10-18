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
    public class TranslateInterpretExperienceController : ApiController
    {
        [Route("translateinterpretexperience/save")]
        [HttpPost]
        public HttpResponseMessage Save(TranslateInterpretExperienceModel aTranslateInterpretExperienceModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ITranslateInterpretExperienceRepository translateInterpretExperience = new TranslateInterpretExperienceRepository(uWork);
            ITranslateInterpretExperienceService translateInterpretExperienceService = new TranslateInterpretExperienceService(translateInterpretExperience);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var TranslateInterpretExperienceModelList = translateInterpretExperienceService.SaveTranslateInterpretExperience(aTranslateInterpretExperienceModel);
                    if (TranslateInterpretExperienceModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, TranslateInterpretExperienceModelList);
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

        [Route("translateinterpretexperience/list")]
        [HttpPost]
        public HttpResponseMessage GetTranslateInterpretExperienceModels(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ITranslateInterpretExperienceRepository translateInterpretExperience = new TranslateInterpretExperienceRepository(uWork);
            ITranslateInterpretExperienceService translateInterpretExperienceService = new TranslateInterpretExperienceService(translateInterpretExperience);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var TranslateInterpretExperienceList = translateInterpretExperienceService.GetAllTranslateInterpretExperienceList(model);
                    if (TranslateInterpretExperienceList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, TranslateInterpretExperienceList);
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

        [Route("translateinterpretexperience/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTranslateInterpretExperience(TranslateInterpretExperienceModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            ITranslateInterpretExperienceRepository translateInterpretExperience = new TranslateInterpretExperienceRepository(uWork);
            ITranslateInterpretExperienceService translateInterpretExperienceService = new TranslateInterpretExperienceService(translateInterpretExperience);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = translateInterpretExperienceService.DeleteTranslateInterpretExperience(model);
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
