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
    public class StaffNarrationTypeController : ApiController
    {
        [Route("staffnarrationtype/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffNarrationTypeModel aStaffNarrationTypeModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffNarrationTypeRepository staffNarrationType = new StaffNarrationTypeRepository(uWork);
            IStaffNarrationTypeService staffNarrationTypeService = new StaffNarrationTypeService(staffNarrationType);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffNarrationTypeList = staffNarrationTypeService.SaveStaffNarrationType(aStaffNarrationTypeModel);
                    if (staffNarrationTypeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffNarrationTypeList);
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

        [Route("staffnarrationtype/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffNarrationTypeModels(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffNarrationTypeRepository staffNarrationType = new StaffNarrationTypeRepository(uWork);
            IStaffNarrationTypeService staffNarrationTypeService = new StaffNarrationTypeService(staffNarrationType);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffNarrationTypeList = staffNarrationTypeService.GetAllStaffNarrationTypeList(model);
                    if (staffNarrationTypeList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffNarrationTypeList);
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

        [Route("staffnarrationtype/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffNarrationType(StaffNarrationTypeModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffNarrationTypeRepository staffNarrationType = new StaffNarrationTypeRepository(uWork);
            IStaffNarrationTypeService staffNarrationTypeService = new StaffNarrationTypeService(staffNarrationType);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = staffNarrationTypeService.DeleteStaffNarrationType(model);
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


