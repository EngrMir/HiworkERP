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
    public class TechnicalSkillLevelController : ApiController
    {
        //ITechnicalSkillLevelService _technicalSkillLevelService;
        //public TechnicalSkillLevelController(ITechnicalSkillLevelService technicalSkillLevelService)
        //{
        //    _technicalSkillLevelService = technicalSkillLevelService;
        //}
        //[Route("technicalskilllevel/list")]
        //[HttpPost]
        //public HttpResponseMessage GetTechnicalSkillLevelModels(TechnicalSkillLevelModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var technicalSkillLevelList = _technicalSkillLevelService.GetAllStaffTechnicalSkillLevelList(model);
        //            if (technicalSkillLevelList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, technicalSkillLevelList);
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

        //[Route("technicalskilllevel/save")]
        //[HttpPost]
        //public HttpResponseMessage Save(TechnicalSkillLevelModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var technicalSkillLevelList = _technicalSkillLevelService.SaveStaffTechnicalSkillLevel(model);
        //            if (technicalSkillLevelList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, technicalSkillLevelList);
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

        //[Route("technicalskilllevel/delete")]
        //[HttpPost]
        //public HttpResponseMessage Delete(TechnicalSkillLevelModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var technicalSkillLevelList = _technicalSkillLevelService.DeleteStaffTechnicalSkillLevel(model);
        //            if (technicalSkillLevelList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, technicalSkillLevelList);
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
