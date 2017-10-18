

/* ******************************************************************************************************************
 * Controller for Staff_BankAccountInfo Entity
 * Date             :   14-July-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    public class StaffBankAccountInfoController : ApiController
    {
        private IStaffBankAccountInfoService service;

        public StaffBankAccountInfoController(IStaffBankAccountInfoService service)
        {
            this.service = service;
        }

        [Route("staffBankAccInfo/save")]
        [HttpPost]
        public HttpResponseMessage SaveStaffBankAccountInfo(StaffBankAccountInfoModel model)
        {
            HttpResponseMessage result;
            List<StaffBankAccountInfoModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveStaffBankAccountInfo(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving StaffBankAccountInfo data";
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

        [Route("staffbankaccountinfo/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffBankAccountInfoList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<StaffBankAccountInfoModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.GetStaffBankAccountInfoList(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving StaffBankAccountInfo list";
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

        [Route("staffbankaccountinfo/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffBankAccountInfo(StaffBankAccountInfoModel model)
        {
            HttpResponseMessage result;
            List<StaffBankAccountInfoModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteStaffBankAccountInfo(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting StaffBankAccountInfo data";
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