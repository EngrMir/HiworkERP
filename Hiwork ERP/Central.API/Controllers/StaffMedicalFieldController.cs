using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class StaffMedicalFieldController : ApiController
    {
        IStaffMedicalFieldService service;
        public StaffMedicalFieldController(IStaffMedicalFieldService _service)
        {
            service = _service;
        }
        [Route("staffmedicalfield/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffMedicalFieldModel aStaffMedicalFieldModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var StaffMedicalFieldList = service.SaveStaffMedicalField(aStaffMedicalFieldModel);
                    if (StaffMedicalFieldList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StaffMedicalFieldList);
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

        //Get All UserStaff KnowledgeField List   
        [Route("staffmedicalfield/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffMedicalField(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var StaffMedicalFieldList = service.GetAllStaffMedicalFieldList(model);
                    if (StaffMedicalFieldList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StaffMedicalFieldList);
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

        [Route("staffmedicalfield/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffMedicalFields(StaffMedicalFieldModel aStaffMedicalFieldModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteStaffMedicalField(aStaffMedicalFieldModel);
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
