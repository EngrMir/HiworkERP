


using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;


namespace Central.API.Controllers
{

    public class UserInfoController : ApiController
    {

        private IUserInfoService service;
        public UserInfoController(IUserInfoService service)
        {
            this.service = service;
        }


        [Route("userInfo/save")]
        [HttpPost]
        public HttpResponseMessage Save(UserInfoModel aUserInfoModel)
        {
            HttpResponseMessage result;
            List<UserInfoModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveUserInfo(aUserInfoModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving UserInformation data";
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
        

        [Route("userInfo/list")]
        [HttpPost]
        public HttpResponseMessage GetUserInfoList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<UserInfoModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.GetUserInfoList(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving UserInformation list";
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


        [Route("userInfo/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteUserInfos(UserInfoModel aUserInfoModel)
        {
            HttpResponseMessage result;
            List<UserInfoModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteUserInfo(aUserInfoModel);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting UserInformation data";
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
        [Route("userInfo/conlist/{con}")]
        [HttpPost]
        public HttpResponseMessage ConList(BaseViewModel model, string con)
        {
            HttpResponseMessage result;

            try
            {
                if (this.ModelState.IsValid)
                {
                    var datalist = service.GetSearchUserList(model, con);
                    result = Request.CreateResponse(HttpStatusCode.OK, datalist);
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

        [Route("userInfo/checkUser")]
        [HttpPost]
        public HttpResponseMessage CheckIfExistUser(UserInfoModel aUserInfoModel)
        {
            BaseViewModel baseModel = new BaseViewModel();
            baseModel.CurrentCulture = aUserInfoModel.CurrentCulture;
            baseModel.CurrentUserID = aUserInfoModel.CurrentUserID;
            List<UserInfoModel> datalist;
            UserInfoModel aModel;
            HttpResponseMessage result;

            try
            {
                //if (this.ModelState.IsValid)
                //{
                datalist = service.GetUserInfoList(baseModel);
                if (datalist != null)
                {
                    aModel = datalist.Where(u => u.Username.ToUpper() == aUserInfoModel.Username.ToUpper()).FirstOrDefault();
                    if (aModel != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, aModel.Username);
                    }
                    else
                    {
                        string message = "No user was found on the given user information";
                        result = Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                }
                else
                {
                    string message = "Error while retriving UserInformation data";
                    result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                }
                //}
                //else
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                //}
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }
    }
}
