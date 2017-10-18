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
    public class StaffSpecialFieldController : ApiController
    {
        [Route("staffspecialfield/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffSpecialFieldModel aStaffSpecialFieldModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffSpecialFieldRepository staffSpecialField = new StaffSpecialFieldRepository(uWork);
            IStaffSpecialFieldService staffSpecialFieldService = new StaffSpecialFieldService(staffSpecialField);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffSpecialFieldModelList = staffSpecialFieldService.SaveStaffSpecialField(aStaffSpecialFieldModel);
                    if (staffSpecialFieldModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffSpecialFieldModelList);
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

        [Route("staffspecialfield/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffSpecialFieldModels(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffSpecialFieldRepository staffSpecialField = new StaffSpecialFieldRepository(uWork);
            IStaffSpecialFieldService staffSpecialFieldService = new StaffSpecialFieldService(staffSpecialField);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffSpecialFieldModelList = staffSpecialFieldService.GetAllStaffSpecialFieldList(model);
                    if (staffSpecialFieldModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffSpecialFieldModelList);
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

        [Route("staffspecialfield/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffSpecialField(StaffSpecialFieldModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffSpecialFieldRepository staffSpecialField = new StaffSpecialFieldRepository(uWork);
            IStaffSpecialFieldService staffSpecialFieldService = new StaffSpecialFieldService(staffSpecialField);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = staffSpecialFieldService.DeleteStaffSpecialField(model);
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
