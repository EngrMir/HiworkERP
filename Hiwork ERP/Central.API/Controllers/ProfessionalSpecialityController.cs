

/* ******************************************************************************************************************
 * Controller for Master_StaffProfessionalSpeciality Entity
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
    public class ProfessionalSpecialityController : ApiController
    {
        //private IProfessionalSpecialityService service;

        //public ProfessionalSpecialityController(IProfessionalSpecialityService ser)
        //{
        //    service = ser;
        //}

        //[Route("profspeciality/save")]
        //[HttpPost]
        //public HttpResponseMessage SaveProfessionalSpeciality(ProfessionalSpecialityModel arg)
        //{
        //    HttpResponseMessage result;
        //    List< ProfessionalSpecialityModel > dataList;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            dataList = service.SaveProfessionalSpeciality(arg);
        //            if (dataList != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, dataList);
        //            }
        //            else
        //            {
        //                string message = "Error while saving ProfessionalSpeciality data";
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


        //[Route("profspeciality/list")]
        //[HttpPost]
        //public HttpResponseMessage GetProfessionalSpecialityList(BaseViewModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<ProfessionalSpecialityModel> dataList;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            dataList = service.GetProfessionalSpecialityList(arg);
        //            if (dataList != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, dataList);
        //            }
        //            else
        //            {
        //                string message = "Error while retriving ProfessionalSpeciality list";
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


        //[Route("profspeciality/delete")]
        //[HttpPost]
        //public HttpResponseMessage DeleteProfessionalSpeciality(ProfessionalSpecialityModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<ProfessionalSpecialityModel> datalist;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            datalist = service.DeleteProfessionalSpeciality(arg);
        //            if (datalist != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, datalist);
        //            }
        //            else
        //            {
        //                string message = "Error while deleting ProfessionalSpeciality data";
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