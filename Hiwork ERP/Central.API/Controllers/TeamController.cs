

/* ******************************************************************************************************************
 * Service for Company_Team Entity
 * Date             :   08-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{

    [Authorize]
    public class TeamController : ApiController
    {
        private ITeamService service;

        public TeamController(ITeamService ser)
        {
            service = ser;
        }

        [Route("team/save")]
        [HttpPost]
        public HttpResponseMessage Save(TeamModel aTeamModel)
        {
            HttpResponseMessage result;
            List<TeamModel> retModel;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    retModel = service.SaveTeam(aTeamModel);
                    if (retModel != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, retModel);
                    }
                    else
                    {
                        string message = "Error while saving Team data";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }
        

        [Route("team/list")]
        [HttpPost]
        public HttpResponseMessage GetTeamList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<TeamModel> dataList;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetTeamList(model);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving Team list";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }


        [Route("team/formdata")]
        [HttpPost]
        public HttpResponseMessage GetTeamFormData(BaseViewModel arg)
        {
            HttpResponseMessage result;
            TeamFormModel? data;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    data = service.GetTeamFormData(arg);
                    if (data.HasValue == true)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, data.Value);
                    }
                    else
                    {
                        string message = "Error while retriving form data of Team";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }


        [Route("team/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTeam(TeamModel aTeamModel)
        {
            HttpResponseMessage result;
            List<TeamModel> retModel;
            
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    retModel = service.DeleteTeam(aTeamModel);
                    if (retModel != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, retModel);
                    }
                    else
                    {
                        string message = "Error while deleting Team data";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }
    }
}
