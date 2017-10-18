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
    public class InterpretationFieldsController : ApiController
    {
        IInterpretationFieldsService _interpretationFieldsService;
        public InterpretationFieldsController(IInterpretationFieldsService interpretationFieldsService)
        {
            _interpretationFieldsService = interpretationFieldsService;
        }
        [Route("interpretationfields/save")]
        [HttpPost]
        public HttpResponseMessage Save(InterpretationFieldsModel aInterpretationFieldsModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var InterpretationFieldsModelList = _interpretationFieldsService.SaveInterpretationFields(aInterpretationFieldsModel);
                    if (InterpretationFieldsModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, InterpretationFieldsModelList);
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

        [Route("interpretationfields/list")]
        [HttpPost]
        public HttpResponseMessage GetInterpretationFieldsModels(InterpretationFieldsModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var InterpretationFieldsList = _interpretationFieldsService.GetAllInterpretationFieldsList(model);
                    if (InterpretationFieldsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, InterpretationFieldsList);
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

        [Route("interpretationfields/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteInterpretationFields(InterpretationFieldsModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _interpretationFieldsService.DeleteInterpretationFields(model);
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
