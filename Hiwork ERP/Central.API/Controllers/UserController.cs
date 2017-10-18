using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.ViewModels;
using HiWork.BLL.Services;
using System.Net;
using System;
using HiWork.BLL.Models;

namespace Central.API.Controllers
{

    public class UserController : ApiController
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #region User Login
        [Route("user/login")]
        [HttpPost]
        public HttpResponseMessage Login(UserViewModel objUser) 
        {            
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = _userService.GetUser(objUser);
                    if (user != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }
                    else
                    {
                        string message = "Please check your username and password.";
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

        #endregion
    }
}
