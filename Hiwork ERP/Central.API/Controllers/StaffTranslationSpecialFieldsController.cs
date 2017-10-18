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
    public class StaffTranslationSpecialFieldsController : ApiController
    {
        [Route("stafftranslationspecialfields/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffTranslationSpecialFieldsModel aStaffTranslationSpecialFieldsModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffTranslationSpecialFieldsRepository staffTranslationSpecialFields = new StaffTranslationSpecialFieldsRepository(uWork);
            IStaffTranslationSpecialFieldsService staffTranslationSpecialFieldsService = new StaffTranslationSpecialFieldsService(staffTranslationSpecialFields);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffTranslationSpecialFieldsList = staffTranslationSpecialFieldsService.SaveStaffTranslationSpecialFields(aStaffTranslationSpecialFieldsModel);
                    if (staffTranslationSpecialFieldsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffTranslationSpecialFieldsList);
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

        [Route("stafftranslationspecialfields/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffTranslationSpecialFieldsModels(BaseViewModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffTranslationSpecialFieldsRepository staffTranslationSpecialFields = new StaffTranslationSpecialFieldsRepository(uWork);
            IStaffTranslationSpecialFieldsService staffTranslationSpecialFieldsService = new StaffTranslationSpecialFieldsService(staffTranslationSpecialFields);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffTranslationSpecialFieldsList = staffTranslationSpecialFieldsService.GetAllStaffTranslationSpecialFieldsList(model);
                    if (staffTranslationSpecialFieldsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffTranslationSpecialFieldsList);
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

        [Route("stafftranslationspecialfields/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffTranslationSpecialFields(StaffTranslationSpecialFieldsModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IStaffTranslationSpecialFieldsRepository staffTranslationSpecialFields = new StaffTranslationSpecialFieldsRepository(uWork);
            IStaffTranslationSpecialFieldsService staffTranslationSpecialFieldsService = new StaffTranslationSpecialFieldsService(staffTranslationSpecialFields);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = staffTranslationSpecialFieldsService.DeleteStaffTranslationSpecialFields(model);
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
