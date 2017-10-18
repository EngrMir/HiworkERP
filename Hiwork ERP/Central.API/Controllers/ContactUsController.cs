using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{

    
    public class ContactUsController : ApiController
    {
        IContactUsService service;
        public ContactUsController(IContactUsService _service)
        {
            service = _service;

        }
        [Route("Contactus/save")]
        [HttpPost]
        public HttpResponseMessage SaveContactUs(ContactUsModel contactus )
        {
            try
            {
                if (this.ModelState.IsValid)
                {  
                    var contact = service.SaveContactUs(contactus);
                    if (contact != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, contact);
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
        [Route("Contactus/list")]
        [HttpPost]
        public HttpResponseMessage GetContactus(BaseViewModel contactus)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var ContactList = service.GetContactus(contactus);
                    if (ContactList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ContactList);
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
        [Route("contactus/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteContactUs(ContactUsModel contactus)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteContactUs(contactus);
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
