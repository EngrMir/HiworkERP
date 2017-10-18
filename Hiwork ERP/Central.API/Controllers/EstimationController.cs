using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System.IO;
using System.Net.Http.Headers;

namespace Central.API.Controllers
{
   // [Authorize]
    public class EstimationController : ApiController
    {
        IEstimationService _service;
        public EstimationController(IEstimationService service)
        {
            _service = service;
        }


        [Route("estimation/save")]
        [HttpPost]
        public HttpResponseMessage Save(EstimationModel model)
        {
            try
            {
                //if (this.ModelState.IsValid)
                //{
                    var estimationList = _service.SaveEstimation(model);
                    if (estimationList)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, estimationList);
                    }
                    else
                    {
                        string message = "Error Saving Data";
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                    }
                //}
                //else
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("estimation/list")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimations(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var estimationList = _service.GetAllEstimationList(model);
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

        [Route("estimation/approvallist")]
        [HttpPost]
        public HttpResponseMessage GetApprovalEstimations(BaseViewModel model)
        {
            try
            {
                var estimationList = _service.GetApprovalEstimationList(model);
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimation/getspecificestimation/{ID}")]
        [HttpPost]
        public HttpResponseMessage GetSpecificEstimation(BaseViewModel model, Guid ID)
        {
            try
            {
                var estimation = _service.GetEstimationByID(model, ID);
                if (estimation != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, estimation);
                }
                else
                {
                    string message = "Error in getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimation/projectlist")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimationsProject(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var estimationList = _service.GetAllEstimationList(model).FindAll(e => e.ProjectID == null || e.ProjectID == Guid.Empty);
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

        [Route("estimationproject/save")]
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

        [Route("estimationproject/nextId")]
        [HttpPost]
        public HttpResponseMessage GetEstimationProjectNextId(BaseViewModel model)
        {
            try
            {
                var result = _service.GetEstimationProjectNextNumber(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("estimationdetails/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationDetailsList(BaseViewModel model, string EstimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var estimationList = _service.GetEstimationDetailsListByID(model, EstimationID);
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

        [Route("estimationfiles/list/{EstimationID}/{EstimationDetailsID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationFilesList(BaseViewModel model, string EstimationID, string EstimationDetailsID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var estimationList = _service.GetEstimationFilesListByID(model, EstimationID, EstimationDetailsID);
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

        [Route("estimation/translationcertificatesettingslist")]
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

        [Route("estimation/delete")]
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

        [Route("estimation/approvalrequest")]
        [HttpPost]
        public HttpResponseMessage RequestForApproval(EstimationModel model)
        {
            try
            {
                var result = _service.ApprovalRequest(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("estimation/approverequest")]
        [HttpPost]
        public HttpResponseMessage ApprovePendingRequest(EstimationModel model)
        {
            try
            {
                var result = _service.ApprovePendingRequest(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [Route("estimation/orderstatus")]
        [HttpPost]
        public HttpResponseMessage OrderStatus(EstimationModel model)
        {
            try
            {
                var result = _service.OrderEstimationStatus(model.ID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("estimation/orderloss")]
        [HttpPost]
        public HttpResponseMessage OrderLoss(EstimationModel model)
        {
            try
            {
                var result = _service.OrderEstimationOrderLoss(model.ID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [Route("estimation/emailconfirmation")]
        [HttpPost]
        public HttpResponseMessage EmailConfirmation(EstimationModel model)
        {
            try
            {
                var result = _service.EmailConfirmation(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("estimation/generatepdf")]
        [HttpPost]
        public HttpResponseMessage GenerateQuotationPdf(EstimationModel model)
        {
            try
            {
                var viewData = new System.Web.Mvc.ViewDataDictionary { { "Estimation", model } };
                var html = RenderViewToString("Estimation", "~/Views/Template/TranslationEstimation.cshtml", viewData);
                var buffer = HiWork.Utils.PdfHelper.HtmlToPdf(html, "");
                var result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(buffer));
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentLength = buffer.Length;
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentDisposition.FileName = "TranslationEstimation_" + DateTime.Now.ToShortDateString() + ".pdf";
                result.Content.Headers.Add("FileName", "TranslationEstimation_" + DateTime.Now.ToShortDateString() + ".pdf");
                result.Content.Headers.Add("Access-Control-Expose-Headers", "FileName");
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
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
