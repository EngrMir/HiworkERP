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
    
    public class EducationController : ApiController
    {
        [Route("education/save")]
        [HttpPost]
        public HttpResponseMessage Save(EducationModel aEducationModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IEducationRepository Education = new EducationRepository(uWork);
            IEducationService educationService = new EducationService(Education);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var educationModelList = educationService.SaveEducation(aEducationModel);
                    if (educationModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, educationModelList);
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

        [Route("education/list")]
        [HttpPost]
        public HttpResponseMessage GetEducationModels(EducationModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IEducationRepository education = new EducationRepository(uWork);
            IEducationService educationService = new EducationService(education);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var educationList = educationService.GetAllEducationList(model);
                    if (educationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, educationList);
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

        [Route("education/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEducation(EducationModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IEducationRepository education = new EducationRepository(uWork);
            IEducationService educationService = new EducationService(education);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = educationService.DeleteEducation(model);
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
