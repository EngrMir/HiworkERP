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
    public class AgentCharactersticsController : ApiController
    {
        IAgentCharactersticsService _agentCharactersticsService;
        public AgentCharactersticsController(IAgentCharactersticsService agentCharactersticsService)
        {
            _agentCharactersticsService = agentCharactersticsService;
        }
        [Route("agentcharacterstics/list")]
        [HttpPost]
        public HttpResponseMessage GetAgentCharactersticsModels(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentCharactersticsList = _agentCharactersticsService.GetAllAgentCharactersticsList(model);
                    if (companyAgentCharactersticsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentCharactersticsList);
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

        [Route("agentcharacterstics/save")]
        [HttpPost]
        public HttpResponseMessage Save(AgentCharactersticsModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentCharactersticsList = _agentCharactersticsService.SaveAgentCharacterstics(model);
                    if (companyAgentCharactersticsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentCharactersticsList);
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

        [Route("agentcharacterstics/delete")]
        [HttpPost]
        public HttpResponseMessage Delete(AgentCharactersticsModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var companyAgentCharactersticsList = _agentCharactersticsService.DeleteAgentCharacterstics(model);
                    if (companyAgentCharactersticsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, companyAgentCharactersticsList);
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
