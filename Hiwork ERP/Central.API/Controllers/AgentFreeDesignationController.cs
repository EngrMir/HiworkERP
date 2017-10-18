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
    public class AgentFreeDesignationController : ApiController
    {
        IAgentFreeDesignationService _agentFreeDesignationService;
        public AgentFreeDesignationController(IAgentFreeDesignationService agentFreeDesignationService)
        {
            _agentFreeDesignationService = agentFreeDesignationService;
        }
        [Route("agentfreedesignation/list")]
        [HttpPost]
        public HttpResponseMessage GetAgentFreeDesignationModels(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentFreeDesignationList = _agentFreeDesignationService.GetAllAgentFreeDesignationList(model);
                    if (companyAgentFreeDesignationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentFreeDesignationList);
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

        [Route("agentfreedesignation/save")]
        [HttpPost]
        public HttpResponseMessage Save(AgentFreeDesignationModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentFreeDesignationList = _agentFreeDesignationService.SaveAgentFreeDesignation(model);
                    if (companyAgentFreeDesignationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentFreeDesignationList);
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

        [Route("agentfreedesignation/delete")]
        [HttpPost]
        public HttpResponseMessage Delete(AgentFreeDesignationModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentFreeDesignationList = _agentFreeDesignationService.DeleteAgentFreeDesignation(model);
                    if (companyAgentFreeDesignationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentFreeDesignationList);
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
