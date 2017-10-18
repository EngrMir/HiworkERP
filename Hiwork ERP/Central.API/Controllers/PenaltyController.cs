using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Central.API.Controllers
{
    public class PenaltyController : ApiController
    {
        IPenaltyService service;
        public PenaltyController(IPenaltyService _service)
        {
            service = _service;
        }

        [Route("penalty/save")]
        [HttpPost]
        public HttpResponseMessage Save(PenaltyModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var PenaltyList = service.SavePenalty(model);
                    if (PenaltyList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PenaltyList);
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

        [Route("penalty/list")]
        [HttpPost]
        public HttpResponseMessage GetPenaltyList(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var PenaltyList = service.GetAllPenaltyList(model);
                    if (PenaltyList != null)
                    {
                       // PenaltyList.OrderBy(d => d.ApplicationId);

                        return Request.CreateResponse(HttpStatusCode.OK, PenaltyList);
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


        [Route("penalty/config")]
        [HttpPost]
        public HttpResponseMessage GetPenaltyCategoryList(BaseViewModel model)
        {
            penaltyConfigData configdata = new penaltyConfigData();
            IUnitOfWork ouw = new UnitOfWork();
            IApplicationRepository rep = new ApplicationRepository(ouw);
            IApplicationService service = new ApplicationService(rep);
            try
            {
                if (this.ModelState.IsValid)
                {
                    configdata.PenaltyCategoryList = Utility.getItemCultureList(Utility.PenaltyCategoryList, model);
                    configdata.ApplicationList = service.GetApplicationList(model);
             
                     return Request.CreateResponse(HttpStatusCode.OK, configdata);    
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

        [Route("penalty/delete")]
        [HttpPost]
        public HttpResponseMessage DeletePenalty(BaseViewModel model)
        {
            try
            {
                var result =service.DeletePenalty(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);

            }

        }


        }
}