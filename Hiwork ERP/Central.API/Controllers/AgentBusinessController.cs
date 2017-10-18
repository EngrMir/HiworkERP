using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    public class AgentBusinessController : ApiController
    {
        IAgentBusinessService _agentBusinessService;
        public AgentBusinessController(IAgentBusinessService agentBusinessService)
        {
            _agentBusinessService = agentBusinessService;
        }
        [Route("agentbusiness/list")]
        [HttpPost]
        public HttpResponseMessage GetAgentBusinessModels(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentBusinessList = _agentBusinessService.GetAllAgentBusinessList(model);
                    if (companyAgentBusinessList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentBusinessList);
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

        [Route("agentbusiness/save")]
        [HttpPost]
        public HttpResponseMessage Save(AgentBusinessModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentBusinessList = _agentBusinessService.SaveAgentBusiness(model);
                    if (companyAgentBusinessList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentBusinessList);
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

        [Route("agentbusiness/delete")]
        [HttpPost]
        public HttpResponseMessage Delete(AgentBusinessModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentBusinessList = _agentBusinessService.DeleteAgentBusiness(model);
                    if (companyAgentBusinessList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentBusinessList);
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
