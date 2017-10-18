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
    public class ProfessionalCerificateController : ApiController
    {
        //IProfessionalCerificateService _professionalCerificateService;
        //public ProfessionalCerificateController(IProfessionalCerificateService professionalCerificateService)
        //{
        //    _professionalCerificateService = professionalCerificateService;
        //}
        //[Route("professionalCerificate/save")]
        //[HttpPost]
        //public HttpResponseMessage Save(ProfessionalCerificateModel aProfessionalCerificateModel)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var ProfessionalCerificateModelList = _professionalCerificateService.SaveProfessionalCerificate(aProfessionalCerificateModel);
        //            if (ProfessionalCerificateModelList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, ProfessionalCerificateModelList);
        //            }
        //            else
        //            {
        //                string message = "Error Saving Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }

        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }
        //}

        //[Route("professionalCerificate/list")]
        //[HttpPost]
        //public HttpResponseMessage GetProfessionalCerificateModels(ProfessionalCerificateModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var ProfessionalCerificateList = _professionalCerificateService.GetAllProfessionalCerificateList(model);
        //            if (ProfessionalCerificateList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, ProfessionalCerificateList);
        //            }
        //            else
        //            {
        //                string message = "Error in getting Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }

        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }

        //}

        //[Route("professionalCerificate/delete")]
        //[HttpPost]
        //public HttpResponseMessage DeleteProfessionalCerificate(ProfessionalCerificateModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var result = _professionalCerificateService.DeleteProfessionalCerificate(model);
        //            if (result != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, result);
        //            }
        //            else
        //            {
        //                string message = "Not deleted successfully";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
        //    }

        //}
    }
}
