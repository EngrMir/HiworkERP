using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.BLL.ViewModels;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{
    public class MessageController : ApiController
    {

        IMessageService _service;
        public MessageController(IMessageService service)
        {
            _service = service;
        }

        [Route("SendMessage/save")]
        [HttpPost]
        public HttpResponseMessage SendMessage(MessageModel mModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.SaveMessage(mModel);

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                   
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


        


    }

}