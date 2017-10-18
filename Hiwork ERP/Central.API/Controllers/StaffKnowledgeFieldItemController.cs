using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class StaffKnowledgeFieldItemController : ApiController
    {
        IStaffKnowledgeFieldItemService service;
        public StaffKnowledgeFieldItemController(IStaffKnowledgeFieldItemService _service)
        {
            service = _service;
        }
        [Route("staffknowledgefielditem/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffKnowledgeFieldItemModel aStaffKnowledgeFieldItemModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffKnowledgeFieldItemList = service.SaveItem(aStaffKnowledgeFieldItemModel);
                    if (staffKnowledgeFieldItemList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffKnowledgeFieldItemList);
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


        //GetAllUserStaffKnowledgeFieldItemList

        [Route("staffknowledgefielditem/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffKnowledgeFieldItems(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffKnowledgeFieldItemList = service.GetItemList(model);
                    if (staffKnowledgeFieldItemList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffKnowledgeFieldItemList);
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

        [Route("staffknowledgefielditem/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffKnowledgeFieldItems(StaffKnowledgeFieldItemModel aStaffKnowledgeFieldItemModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteItem(aStaffKnowledgeFieldItemModel);
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
