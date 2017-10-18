using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class StaffKnowledgeFieldController : ApiController
    {
        IStaffKnowledgeFieldService service;
        public StaffKnowledgeFieldController(IStaffKnowledgeFieldService _service)
        {
            service = _service;
        }
        [Route("staffknowledgefield/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffKnowledgeFieldModel aStaffKnowledgeFieldModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffKnowledgeFieldList = service.SaveStaffKnowledgeField(aStaffKnowledgeFieldModel);
                    if (staffKnowledgeFieldList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffKnowledgeFieldList);
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
        [Route("staffknowledgefield/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffKnowledgeField(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffKnowledgeFieldList = service.GetAllStaffKnowledgeFieldList(model);
                    if (staffKnowledgeFieldList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffKnowledgeFieldList);
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

        [Route("staffknowledgefield/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffKnowledgeFields(StaffKnowledgeFieldModel aStaffKnowledgeFieldModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteStaffKnowledgeField(aStaffKnowledgeFieldModel);
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