

/* ******************************************************************************************************************
 * Controller for Master_StaffLanguageSkillLevel Entity
 * Date             :   15-Jun-2017
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

    [Authorize]
    public class LanguageSkillLevelController : ApiController
    {
        //private ILanguageSkillLevelService service;
        //public LanguageSkillLevelController(ILanguageSkillLevelService ser)
        //{
        //    this.service = ser;
        //}

        //[Route("langskill/save")]
        //[HttpPost]
        //public HttpResponseMessage SaveLanguageSkillLevel(LanguageSkillLevelModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<LanguageSkillLevelModel> modList;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            modList = service.SaveLanguageSkillLevel(arg);
        //            if (modList != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, modList);
        //            }
        //            else
        //            {
        //                string message = "Error while saving LanguageSkillLevel data";
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


        //[Route("langskill/list")]
        //[HttpPost]
        //public HttpResponseMessage GetLanguageSkillLevelList(BaseViewModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<LanguageSkillLevelModel> modList;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            modList = service.GetLanguageSkillLevelList(arg);
        //            if (modList != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, modList);
        //            }
        //            else
        //            {
        //                string message = "Error while retriving LanguageSkillLevel list";
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


        //[Route("langskill/delete")]
        //[HttpPost]
        //public HttpResponseMessage DeleteLanguageSkillLevel(LanguageSkillLevelModel arg)
        //{
        //    HttpResponseMessage result;
        //    List<LanguageSkillLevelModel> datalist;
            
        //    try
        //    {
        //        if (this.ModelState.IsValid == true)
        //        {
        //            datalist = service.DeleteLanguageSkillLevel(arg);
        //            if (datalist != null)
        //            {
        //                result = Request.CreateResponse(HttpStatusCode.OK, datalist);
        //            }
        //            else
        //            {
        //                string message = "Error while deleting LanguageSkillLevel data";
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