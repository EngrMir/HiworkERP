

/* ******************************************************************************************************************
 * API Controller for TransproLanguagePriceCategory & TransproLanguagePriceDetails Entity
 * Date             :   14-September-2017
 * By               :   Ashis Kr. Das
 * *****************************************************************************************************************/


using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.Net;
using System;
using System.Collections.Generic;
using HiWork.Utils;

namespace Central.API.Controllers
{

    //[Authorize]
    public class TransproLanguagePriceController : ApiController
    {

        ITransproLanguagePriceService _tlpService;

        public TransproLanguagePriceController(ITransproLanguagePriceService tlpService)
        {

            _tlpService = tlpService;

        }


        /*******************************************************************************************************************************
         * *****************************************************************************************************************************
         * *****************************************************************************************************************************
         * *****************************************************************************************************************************/


        [Route("transprolpc/viewlist")]
        [HttpPost]
        public HttpResponseMessage GetTransproLanguagePriceViewList(BaseViewModel model)
        {
            HttpResponseMessage response;
            TransproLanguagePriceViewListModel? ViewModel;

            try
            {
                ViewModel = _tlpService.GetTransproLanguagePriceViewList(model);
                if (ViewModel != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, ViewModel);
                }
                else
                {
                    string message = "Error while retriving TransproLanguagePriceCategory list";
                    response = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [Route("transprolpc/list")]
        [HttpPost]
        public HttpResponseMessage GetTransproLanguagePriceCategoryList(TransproLanguagePriceCategoryQueryModel model)
        {
            HttpResponseMessage response;
            List<TransproLanguagePriceCategoryModel> DataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    DataList = _tlpService.GetTransproLanguagePriceCategoryList(model);
                    if (DataList != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, DataList);
                    }
                    else
                    {
                        string message = "Error while retriving TransproLanguagePriceCategory list";
                        response = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [Route("transprolpd/list/{PriceCategoryID}")]
        [HttpPost]
        public HttpResponseMessage GetTransproLanguagePriceDetailsList(BaseViewModel model, string PriceCategoryID)
        {
            HttpResponseMessage response;
            List<TransproLanguagePriceDetailsModel> DataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    DataList = _tlpService.GetTransproLanguagePriceDetailsList(model, PriceCategoryID);
                    if (DataList != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, DataList);
                    }
                    else
                    {
                        string message = "Error while retriving TransproLanguagePriceCategory list";
                        response = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [Route("transprolpc/save")]
        [HttpPost]
        public HttpResponseMessage SaveTransproLanguagePriceCategory(TransproLanguagePriceCategoryModel model)
        {
            HttpResponseMessage response;
            List<TransproLanguagePriceCategoryModel> DataList;

            try
            {
                //if (this.ModelState.IsValid == true)
                //{
                    DataList = _tlpService.SaveTransproLanguagePriceCategory(model);
                    if (DataList != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, DataList);
                    }
                    else
                    {
                        string message = "Error while saving TransproLanguagePriceCategory data";
                        response = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                //}
                //else
                //{
                //    response = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                //}
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [Route("transprolpc/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTransproLanguagePriceCategory(TransproLanguagePriceCategoryModel model)
        {
            HttpResponseMessage response;
            List<TransproLanguagePriceCategoryModel> DataList;

            try
            {
                //if (this.ModelState.IsValid == true)
                //{
                DataList = _tlpService.DeleteTransproLanguagePriceCategory(model);
                if (DataList != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, DataList);
                }
                else
                {
                    string message = "Error while deleting TransproLanguagePriceCategory data";
                    response = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                }
                //}
                //else
                //{
                //    response = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                //}
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }
    }
}
 
 
 