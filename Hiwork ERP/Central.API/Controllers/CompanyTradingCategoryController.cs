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

    public class CompanyTradingCategoryController : ApiController
    {

        ICompanyTradingCategoryService _mctcService;

        public CompanyTradingCategoryController(ICompanyTradingCategoryService mctcService)
        {

            _mctcService = mctcService;

        }

        [Route("mcompanytradcatagory/save")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyTradingCategory(CompanyTradingCategoryModel aMERM)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mctcList = _mctcService.SaveCompanyTradingCategory(aMERM);
                    if (mctcList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mctcList);
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


        //GetAllMasterCompanyTradingList

        [Route("mcompanytradcatagory/list")]
        [HttpPost]
        public HttpResponseMessage GetAllCompanyTradingCategoryList(CompanyTradingCategoryModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mctcList = _mctcService.GetAllCompanyTradingCategoryList(model);
                    if (mctcList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mctcList);
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


        [Route("mcompanytradcatagory/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteMasterCompanyTradingCategory(CompanyTradingCategoryModel merModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _mctcService.DeleteCompanyTradingCategory(merModel);
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