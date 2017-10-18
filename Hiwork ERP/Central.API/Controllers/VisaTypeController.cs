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
    public class VisaTypeController : ApiController
    {
        [Route("visa/save")]
        [HttpPost]
        public HttpResponseMessage Save(VisaTypeModel aVisaTypeModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IVisaTypeRepository visaType = new VisaTypeRepository(uWork);
            IVisaTypeService visaTypeService = new VisaTypeService(visaType);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var visaTypeModelList = visaTypeService.SaveVisaType(aVisaTypeModel);
                    if (visaTypeModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, visaTypeModelList);
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

        [Route("visa/list")]
        [HttpPost]
        public HttpResponseMessage GetVisaTypeModels(VisaTypeModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IVisaTypeRepository visaType = new VisaTypeRepository(uWork);
            IVisaTypeService visaTypeService = new VisaTypeService(visaType);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var visaTypeList = visaTypeService.GetAllVisaTypeList(model);
                    if (visaTypeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, visaTypeList);
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

        [Route("visaType/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteVisaType(VisaTypeModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IVisaTypeRepository visaType = new VisaTypeRepository(uWork);
            IVisaTypeService visaTypeService = new VisaTypeService(visaType);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = visaTypeService.DeleteVisaType(model);
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
