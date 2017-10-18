

/* ******************************************************************************************************************
 * Controller for Master_StaffOtherExperiences Entity
 * Date             :   16-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;

namespace Central.API.Controllers
{

    [EnableCors("*", "*", "*")]
    public class OtherExperiencesController : ApiController
    {
        //private IOtherExperiencesService service;
        //public OtherExperiencesController(IOtherExperiencesService ser)
        //{
        //    service = ser;
        //}

        //[Route("otherexperiences/save")]
        //[HttpPost]
        //public HttpResponseMessage SaveOtherExperiences(OtherExperiencesModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<OtherExperiencesModel> dataList;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            dataList = service.SaveOtherExperiences(arg);
        //            if (dataList != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, dataList);
        //            }
        //            else
        //            {
        //                string message = "Error while saving OtherExperiences data";
        //                result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }
        //    return result;
        //}


        //[Route("otherexperiences/list")]
        //[HttpPost]
        //public HttpResponseMessage GetOtherExperiencesList(BaseViewModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<OtherExperiencesModel> dataList;
            
        //    try
        //    {
        //         if (this.ModelState.IsValid == true)
        //        {
        //            dataList = service.GetOtherExperiencesList(arg);
        //            if (dataList != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, dataList);
        //            }
        //            else
        //            {
        //                string message = "Error while retriving OtherExperiences list";
        //                result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }
        //    return result;
        //}


        //[Route("otherexperiences/delete")]
        //[HttpPost]
        //public HttpResponseMessage DeleteOtherExperiences(OtherExperiencesModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<OtherExperiencesModel> datalist;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            datalist = service.DeleteOtherExperiences(arg);
        //            if (datalist != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, datalist);
        //            }
        //            else
        //            {
        //                string message = "Error while deleting OtherExperiences data";
        //                result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        //    }
        //    return result;
        //}
    }
}