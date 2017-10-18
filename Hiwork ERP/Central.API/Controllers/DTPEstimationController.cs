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
using HiWork.DAL.Database;
using System.Collections.Generic;
using System.Linq;

namespace Central.API.Controllers
{
    [Authorize]
    public class DTPEstimationController : ApiController
    {
        IDTPEstimationService _service;
        public DTPEstimationController(IDTPEstimationService service)
        {
            _service = service;
        }

        [Route("dtpestimation/save")]
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

        [Route("dtpestimation/estimationfiletypelist/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetFileTypeList(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                var items = _service.GetFileTypeList(EstimationID).Select(x => new
                {
                    ID = x.ID,
                    EstimationID = x.Estimation.ID,
                    FileType = x.FileType,
                    Version = x.Version
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("dtpestimation/estimationworkcontentlist/{EstimationID}")]
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

        [Route("dtpestimation/list")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimations(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var estimationList = _service.GetAllDTPEstimationList(model);
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

        [Route("dtpestimation/translationcertificatesettingslist")]
        [HttpPost]
        public HttpResponseMessage GetTranslationCertificateSettingsList(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var settingsList = _service.GetTranslationCertificateSettingsList(model);
                    if (settingsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, settingsList);
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

        [Route("dtpestimation/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTranslationEstimations(EstimationModel translationEstimationModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.DeleteDTPEstimation(translationEstimationModel);
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

        [Route("dtpestimation/generatepdf")]
        [HttpPost]
        public HttpResponseMessage GenerateQuotationPdf(EstimationModel model)
        {
            try
            {
                var viewData = new System.Web.Mvc.ViewDataDictionary { { "Estimation", model } };
                var html = RenderViewToString("TaskQuotation", "~/Views/Template/TaskQuotation.cshtml", viewData);
                var buffer = PdfHelper.HtmlToPdf(html, "");
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