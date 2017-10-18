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
    [Authorize]
    public class NarrationEstimationController : ApiController
    {
        INarrationEstimationService _service;
        public NarrationEstimationController(INarrationEstimationService service)
        {
            _service = service;
        }

        [Route("narrationEstimation/save")]
        [HttpPost]
        public HttpResponseMessage Save(CommonModelHelper model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var estimationList = _service.Save(model);
                    if (estimationList)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, estimationList);
                    }
                    else
                    {
                        string message = "Error Saving Data";
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("narration/estimationworkcontentlist/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetWorkContentList(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                var items = _service.GetWorkContentList(EstimationID).Select(x => new
                {
                    ID = x.ID,
                    EstimationID = x.Estimation.ID,
                    WorkContent = x.WorkContent
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("narrationestimationdetails/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationDetailsList(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var itemList = new List<EstimationDetailsModel>();
                    var estimationList = _service.GetEstimationDetailsListByID(model, EstimationID);
                    estimationList.ForEach(item =>
                    {
                        var detailsModel = new EstimationDetailsModel
                        {
                            ID = item.ID,
                            SourceLanguageID = item.SourceLanguageID,
                            TargetLanguageID = item.TargetLanguageID,
                            UnitPriceTime = Convert.ToInt16(item.UnitPriceTime),
                            UnitPriceSubTotal = Convert.ToDecimal(item.UnitPriceSubTotal),
                            DiscountRate = Convert.ToDecimal(item.DiscountRate),
                            DiscountedPrice = Convert.ToDecimal(item.DiscountedPrice),
                            EstimationPrice = Convert.ToDecimal(item.EstimationPrice),
                            StudioPrice = Convert.ToDecimal(item.StudioPrice),
                            StudioPriceTime = Convert.ToDecimal(item.StudioPriceTime),
                            EditPrice = Convert.ToDecimal(item.EditPrice),
                            EditPriceTime = Convert.ToInt16(item.EditPriceTime),
                            StudioPriceSubTotal = Convert.ToDecimal(item.StudioPriceSubTotal),
                            StudioPriceDiscountRate = Convert.ToDecimal(item.StudioPriceDiscountRate),
                            StudioDiscountedPrice = Convert.ToDecimal(item.StudioDiscountedPrice),
                            TotalAfterDiscount = Convert.ToDecimal(item.TotalAfterDiscount),
                            NumberOfPeople = Convert.ToInt16(item.NumberOfPeople),
                            Total = Convert.ToDecimal(item.Total),
                            PaymentRate = Convert.ToDecimal(item.PaymentRate),
                            ExpectedPayment = Convert.ToDecimal(item.ExpectedPayment),
                           

                        };

                        itemList.Add(detailsModel);
                    });
                    if (estimationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, itemList);
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


        [Route("narrationestimationexpenses/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationNarrationExpenseList(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var expenseList = _service.GetEstimationNarrationExpenseListByID(model, EstimationID);
                    if (expenseList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, expenseList);
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

        [Route("narrationEstimation/list")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimations(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var estimationList = _service.GetAllNarrationEstimationList(model);
                    if (estimationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, estimationList);
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
    }
}
