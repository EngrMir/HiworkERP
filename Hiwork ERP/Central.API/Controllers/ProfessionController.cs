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
    public class ProfessionController : ApiController
    {
        IProfessionService _professionService;
        public ProfessionController(IProfessionService professionService)
        {
            _professionService = professionService;
        }
        [Route("profession/save")]
        [HttpPost]
        public HttpResponseMessage Save(ProfessionModel aProfessionModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var ProfessionModelList = _professionService.SaveProfession(aProfessionModel);
                    if (ProfessionModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ProfessionModelList);
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

        [Route("profession/list")]
        [HttpPost]
        public HttpResponseMessage GetProfessionModels(ProfessionModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var ProfessionList = _professionService.GetAllProfessionList(model);
                    if (ProfessionList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ProfessionList);
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

        [Route("Profession/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteProfession(ProfessionModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _professionService.DeleteProfession(model);
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
