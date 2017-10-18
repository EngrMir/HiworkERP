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
    public class MasterStaffTechnicalFieldsController : ApiController
    {

        //IMasterStaffTechnicalFieldsService _mstfieldsService;

        //public MasterStaffTechnicalFieldsController(IMasterStaffTechnicalFieldsService mstfieldsService)
        //{

        //    _mstfieldsService = mstfieldsService;

        //}

        //[Route("mstechfields/save")]
        //[HttpPost]
        //public HttpResponseMessage SaveMasterStaffTechnicalFields(MasterStaffTechnicalFieldsModel aMERM)
        //{

        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var mctcList = _mstfieldsService.SaveMasterStaffTechnicalFields(aMERM);
        //            if (mctcList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, mctcList);
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


    

        //[Route("mstechfields/list")]
        //[HttpPost]
        //public HttpResponseMessage GetAllMasterStaffTechnicalFieldsList(MasterStaffTechnicalFieldsModel model)
        //{

        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var mctcList = _mstfieldsService.GetAllMasterStaffTechnicalFieldsList(model);
        //            if (mctcList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, mctcList);
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


        //[Route("mstechfields/delete")]
        //[HttpPost]
        //public HttpResponseMessage DeleteMasterStaffTechnicalFields(MasterStaffTechnicalFieldsModel merModel)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var result = _mstfieldsService.DeleteMasterStaffTechnicalFields(merModel);
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