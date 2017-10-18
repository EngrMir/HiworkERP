using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    [Authorize]
    public class NarrationVoiceFilesController : ApiController
    {
        [Route("narrationvoicefiles/save")]
        [HttpPost]
        public HttpResponseMessage Save(NarrationVoiceFilesModel narrationVoiceFilesModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            INarrationVoiceFilesRepository narrationVoiceFilesRepository = new NarrationVoiceFilesRepository(uWork);
            INarrationVoiceFilesService narrationVoiceFilesService = new NarrationVoiceFilesService(narrationVoiceFilesRepository);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var narrationVoiceFilesModelList = narrationVoiceFilesService.SaveNarrationVoiceFiles(narrationVoiceFilesModel);
                    if (narrationVoiceFilesModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, narrationVoiceFilesModelList);
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

        [Route("narrationvoicefiles/list")]
        [HttpPost]
        public HttpResponseMessage GetNarrationVoiceFileModels(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            INarrationVoiceFilesRepository narrationVoiceFilesRepository = new NarrationVoiceFilesRepository(uWork);
            INarrationVoiceFilesService narrationVoiceFilesService = new NarrationVoiceFilesService(narrationVoiceFilesRepository);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var narrationVoiceFileList = narrationVoiceFilesService.GetAllNarrationVoiceFileList(model);
                    if (narrationVoiceFileList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, narrationVoiceFileList);
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

        [Route("narrationvoicefiles/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteNarrationVoiceFile(NarrationVoiceFilesModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            INarrationVoiceFilesRepository narrationVoiceFilesRepository = new NarrationVoiceFilesRepository(uWork);
            INarrationVoiceFilesService narrationVoiceFilesService = new NarrationVoiceFilesService(narrationVoiceFilesRepository);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = narrationVoiceFilesService.DeleteNarrationVoiceFile(model);
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
