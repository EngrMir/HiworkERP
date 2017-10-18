using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.DAL.Repositories;
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
    public class DesignationController: ApiController
    {
        IDesignationService service;
        public DesignationController (IDesignationService _service)
        {
            service = _service;
        }
        [Route("designation/save")]
        [HttpPost]
        public HttpResponseMessage SaveDesignation(DesignationModel aDesignationModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.SaveDesignation(aDesignationModel);

                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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


        [Route("designation/list")]
        [HttpPost]
        public HttpResponseMessage GetAllDesignation(BaseViewModel model)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.GetAllDesignationList(model);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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
        [Route("designation/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteDesignation(DesignationModel aDesignationModel)
        {
           

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteDesignation(aDesignationModel);
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
        [Route("designation/edit")]
        [HttpPost]
        public HttpResponseMessage EditDesignation(DesignationModel aDesignationModel)
        {
           try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.UpdateDesignation(aDesignationModel);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        string message = "Not updated successfully";
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


        [Route("designation/formdata")]
        [HttpPost]
        public HttpResponseMessage GetDesignationFormData(BaseViewModel arg)
        {
            HttpResponseMessage result;
            DesignationFormModel data;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    data = service.GetDesignationFormData(arg);
                    if (data != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else
                    {
                        string message = "Error while retriving form data of Designation";
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

    }
}