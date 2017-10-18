using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class StaffPatentFieldController : ApiController
    {
        IStaffPatentFieldService service;
        public StaffPatentFieldController(IStaffPatentFieldService _service)
        {
            service = _service;
        }
        [Route("staffpatentfield/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffPatentFieldModel aStaffPatentFieldModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var StaffPatentFieldList = service.SaveStaffPatentField(aStaffPatentFieldModel);
                    if (StaffPatentFieldList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StaffPatentFieldList);
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
        [Route("staffpatentfield/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffPatentField(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var StaffPatentFieldList = service.GetAllStaffPatentFieldList(model);
                    if (StaffPatentFieldList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StaffPatentFieldList);
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

        [Route("staffpatentfield/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffPatentFields(StaffPatentFieldModel aStaffPatentFieldModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteStaffPatentField(aStaffPatentFieldModel);
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
