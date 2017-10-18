using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.IO;
using System.Net.Http.Headers;
using HiWork.Utils;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    [Authorize]
    public class OverheadCostQuotationController : ApiController
    {
        IOverheadCostQuotationService _service;
        public delegate byte[] HtmlToPdf(string html, string path);
        public OverheadCostQuotationController(IOverheadCostQuotationService service)
        {
            _service = service;
        }

        [Route("overheadcostquotation/save")]
        [HttpPost]
        public HttpResponseMessage Save(CommonModelHelper model)
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

        [Route("overheadcostquotation/getquotationestimation")]
        [HttpPost]
        public HttpResponseMessage GetQuotationEstimation(string id)
        {
            try
            {
                var estimation = _service.GetTaskQuotation(id);
                return Request.CreateResponse(HttpStatusCode.OK, estimation);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("overheadcostquotation/getquotationdetaillist")]
        [HttpPost]
        public HttpResponseMessage GetQuotationDetailList(Guid id)
        {
            try
            {
                var items = _service.GetTaskQuotationDetailList(id);
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("overheadcostquotation/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetList(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var itemList = new List<EstimationDetailsModel>();
                    var estimationList = _service.GetEstimationDetailsListByID(model, EstimationID);
                    estimationList.ForEach(item => {
                        var detailsModel = new EstimationDetailsModel {
                            ID = item.ID,
                            ExcludeTax = item.ExcludeTax ?? false,
                            Contents = item.Contents,
                            UnitPrice1 = item.UnitPrice1 ?? 0,
                            PageCount1 = item.PageCount1 ?? 0,
                            Discount1 = item.Discount1 ?? 0,
                            SourceLanguageID = item.SourceLanguageID,
                            TargetLanguageID = item.TargetLanguageID
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

        [Route("overheadcostquotation/generatepdf")]
        [HttpPost]
        public HttpResponseMessage GenerateQuotationPdf(EstimationModel model)
        {
            try
            {
                var viewData = new System.Web.Mvc.ViewDataDictionary { { "Estimation", model } };
                var html = RenderViewToString("TaskQuotation", "~/Views/Template/TaskQuotation.cshtml", viewData);
                var buffer = HtmlToPdfHelper(PdfHelper.HtmlToPdf, html, ""); //PdfHelper.HtmlToPdf(html, "");
                var result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(buffer));
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentLength = buffer.Length;
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = $"Quotation_{DateTime.Now.ToShortDateString()}.pdf";
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        public static byte[] HtmlToPdfHelper(HtmlToPdf delegateFunc, string html, string path)
        {
            return delegateFunc(html, path);
        }

        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new System.Web.Routing.RouteData();
                routeData.Values.Add("controller", controllerName);
                var httpRequest = new System.Web.HttpRequest(null, "http://google.com", null);
                var contextWrapper = new System.Web.HttpContextWrapper(new System.Web.HttpContext(httpRequest, new System.Web.HttpResponse(null)));
                var fakeControllerContext = new System.Web.Mvc.ControllerContext(contextWrapper, routeData, new FakeController());
                fakeControllerContext.RouteData = routeData;
                var razorViewEngine = new System.Web.Mvc.RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);
                var viewContext = new System.Web.Mvc.ViewContext(fakeControllerContext, razorViewResult.View, new System.Web.Mvc.ViewDataDictionary(viewData), new System.Web.Mvc.TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
    }
}