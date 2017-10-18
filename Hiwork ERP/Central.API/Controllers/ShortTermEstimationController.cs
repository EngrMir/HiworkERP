using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using HiWork.Utils;
using System.IO;
using System.Net.Http.Headers;

namespace Central.API.Controllers
{
    [Authorize]
    public class ShortTermEstimationController : ApiController
    {
        IShortTermEstimationService _service;
        public ShortTermEstimationController(IShortTermEstimationService service)
        {
            _service = service;
        }
        [Route("quotation/save")]
        [HttpPost]
        public HttpResponseMessage Save(EstimationModel model)
        {
            try
            {
                var shortTermEstimationList = _service.SaveEstimation(model);
                if (shortTermEstimationList)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, shortTermEstimationList);
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
        [Route("quotation/list")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimations(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var shortTermEstimationList = _service.GetAllEstimationList(model);
                    if (shortTermEstimationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, shortTermEstimationList);
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

        [Route("quotation/projectlist")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimationsProject(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var shortTermEstimationList = _service.GetAllEstimationList(model).FindAll(e => e.ProjectID == null || e.ProjectID == Guid.Empty);
                    if (shortTermEstimationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, shortTermEstimationList);
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

        [Route("quotationproject/save")]
        [HttpPost]
        public HttpResponseMessage SaveTranslationEstimationsProject(EstimationProjectModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.SaveEstimationProject(model);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("quotationproject/nextId")]
        [HttpPost]
        public HttpResponseMessage GetEstimationProjectNextId(BaseViewModel model)
        {
            try
            {
                var result = _service.GetEstimationProjectNextNumber(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("shorttermestimation/generatepdf")]
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
        [Route("quotationdetails/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationDetailsList(BaseViewModel model, string EstimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var shortTermEstimationList = _service.GetEstimationDetailsListByID(model, EstimationID);
                    if (shortTermEstimationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, shortTermEstimationList);
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

        [Route("quotationfiles/list/{EstimationID}/{EstimationDetailsID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationFilesList(BaseViewModel model, string EstimationID, string EstimationDetailsID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var shortTermEstimationList = _service.GetEstimationFilesListByID(model, EstimationID, EstimationDetailsID);
                    if (shortTermEstimationList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, shortTermEstimationList);
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

        [Route("quotationregistration/translationcertificatesettingslist")]
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

        [Route("quotationregistration/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTranslationEstimations(EstimationModel translationEstimationModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.DeleteEstimation(translationEstimationModel);
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
