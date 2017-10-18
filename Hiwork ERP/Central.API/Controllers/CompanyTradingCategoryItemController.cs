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
    [Authorize]
    public class CompanyTradingCategoryItemController : ApiController
    {

        ICompanyTradingCategoryItemService _mctciService;

        public CompanyTradingCategoryItemController(ICompanyTradingCategoryItemService mctcService)
        {

            _mctciService = mctcService;

        }

        [Route("mcompanytradcategoryitem/save")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyTradingCategoryItem(CompanyTradingCategoryItemModel aMERM)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mctcList = _mctciService.SaveCompanyTradingCategoryItem(aMERM);
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


        //GetAllMasterCompanyTradingItemList

        [Route("mcompanytradcategoryitem/list")]
        [HttpPost]
        public HttpResponseMessage GetAllCompanyTradingCategoryItemList(CompanyTradingCategoryItemModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var mctciList = _mctciService.GetAllCompanyTradingCategoryItemList(model);
                    if (mctciList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, mctciList);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        [Route("mcompanytradcategoryitem/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCompanyTradingCategoryItem(CompanyTradingCategoryItemModel mciModel)
        {


            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _mctciService.DeleteCompanyTradingCategoryItem(mciModel);
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