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
    public class StaffController : ApiController
    {
        [Route("staff/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffModel aStaffModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var StaffList = _staffService.SaveStaff(aStaffModel);
                    if (StaffList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StaffList);
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

        [Route("staff/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<StaffModel> datalist;
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = _staffService.GetStaffList(model);
                    result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }

        [Route("stafflistGet/getID")]
        [HttpPost]
        public HttpResponseMessage GetAllStaffByID(GetValue staffID)
        {
            HttpResponseMessage result;      
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (this.ModelState.IsValid == true)
                {
                   var  datalist = _staffService.GetAllStaffAndChildByID(staffID.Value);
                    result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }

        [Route("educationalDegree/list")]
        [HttpPost]
        public HttpResponseMessage GetEducationalDegreeList(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var educationalDegreeList = _staffService.GetEducationalDegreeList(model);
                    if (educationalDegreeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, educationalDegreeList);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("mejorSubject/list")]
        [HttpPost]
        public HttpResponseMessage GetMejorSubjectList(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var mejorsubjectList = _staffService.GetMejorSubjectList(model);
                    if (mejorsubjectList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mejorsubjectList);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        [Route("staffEducation/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffEducation(EducationHistoryModel educationalInfoModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var staffEducationList = _staffService.SaveStaffEducation(educationalInfoModel);
                        if (staffEducationList)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, staffEducationList);
                        }
                        else
                        {
                            string message = "Error Saving Data";
                            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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

        [Route("staffSkillTech/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffSkillTech(StaffSkillTechModel staffSkillTechModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var staffSkillTechList = _staffService.SaveStaffSkillTech(staffSkillTechModel);
                        if (staffSkillTechList)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, staffSkillTechList);
                        }
                        else
                        {
                            string message = "Error Saving Data";
                            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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

        [Route("staffTRExperience/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffTRExperience(TranslateInterpretExperienceModel staffTRExperienceModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var staffSkillTechList = _staffService.SaveStaffTRExperience(staffTRExperienceModel);
                        if (staffSkillTechList)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, staffSkillTechList);
                        }
                        else
                        {
                            string message = "Error Saving Data";
                            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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

        [Route("staffBankAccInfo/save")] 
        [HttpPost]
        public HttpResponseMessage SaveBankAccInfo(StaffBankAccountInfoModel staffBankAccountInfoModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var saveBankAccInfoList = _staffService.SaveBankAccInfo(staffBankAccountInfoModel);
                        if (saveBankAccInfoList)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, saveBankAccInfoList);
                        }
                        else
                        {
                            string message = "Error Saving Data";
                            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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

        [Route("staffTransPro/save")]
        [HttpPost]
        public HttpResponseMessage SaveTransPro(TransproInformationModel transproInformationModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var transProList = _staffService.SaveTransPro(transproInformationModel);
                        if (transProList)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, transProList);
                        }
                        else
                        {
                            string message = "Error Saving Data";
                            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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

        [Route("staffNarration/save")]
        [HttpPost]
        public HttpResponseMessage SaveNarration(NarrationCommon narrationCommon)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffRepository repo = new StaffRepository(uWork);
            IStaffService _staffService = new StaffService(repo);
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var narrationList = _staffService.SaveNarration(narrationCommon);
                        if (narrationList)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, narrationList);
                        }
                        else
                        {
                            string message = "Error Saving Data";
                            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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


    }
}
