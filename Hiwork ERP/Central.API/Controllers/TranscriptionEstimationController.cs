using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    [Authorize]
    public class TranscriptionEstimationController : ApiController
    {
        ITranscriptionEstimationService _service;
        public TranscriptionEstimationController(ITranscriptionEstimationService service)
        {
            _service = service;
        }

        [Route("transcriptionestimation/save")]
        [HttpPost]
        public HttpResponseMessage Save(CommonModelHelper_Transcription model)
        {
            try
            {
                var estimationList = _service.SaveTranscriptionEstimation(model);
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

        [Route("transcriptionestimation/list/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetTranslationEstimations(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var itemList = new List<EstimationDetailsModel>();
                    var estimationList = _service.GetAllTranscriptionEstimationDetailsList(model, EstimationID);
                    estimationList.ForEach(item => {
                        var detailsModel = new EstimationDetailsModel {
                            ID = item.ID
                            , SourceLanguageID = item.SourceLanguageID
                            , TargetLanguageID = item.TargetLanguageID
                            , Contents = item.Contents
                            , UnitPrice1 = Convert.ToDecimal(item.UnitPrice1 ?? 0)
                            , LengthMinute = Convert.ToDecimal(item.LengthMinute ?? 0)
                            , Discount1 = Convert.ToDecimal(item.Discount1 ?? 0)
                            , WithTranslation = item.WithTranslation ?? false
                            , ExcludeTax = item.ExcludeTax ?? false
                            //, EstimationPrice = (decimal)(item.UnitPrice1 * item.LengthMinute)
                            //, DiscountRate = ((item.Discount1 / (item.UnitPrice1 * item.LengthMinute) * 100))
                            //, Total = ((item.UnitPrice1 * item.LengthMinute) - item.Discount1)
                            //, AvgUnitPrice = 0 ExcludedTaxCost
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
        #region "Changed"
        //[Route("transcriptionestimation/list")]
        //[HttpPost]
        //public HttpResponseMessage GetTranslationEstimations(BaseViewModel model)
        //{
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var estimationList = _service.GetAllTranscriptionEstimationList(model);
        //            if (estimationList != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, estimationList);
        //            }
        //            else
        //            {
        //                string message = "Error in getting Data";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        #endregion "Changed"

        [Route("transcriptionestimation/translationcertificatesettingslist")]
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

        [Route("transcriptionestimation/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteTranslationEstimations(CommonModelHelper_Transcription translationEstimationModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.DeleteTranscriptionEstimation(translationEstimationModel);
                    if (result)
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

        [Route("transcriptionestimation/generatepdf")]
        [HttpPost]
        public HttpResponseMessage GenerateQuotationPdf(EstimationModel model)
        {
            try
            {
                var viewData = new System.Web.Mvc.ViewDataDictionary { { "Estimation", model } };
                var html = RenderViewToString("TaskQuotation", "~/Views/Template/TaskQuotation.cshtml", viewData);

                var res = PdfSharpConvert(html);
                var result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(res));
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentLength = res.Length;
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = $"Estimate_{DateTime.Now.ToShortDateString()}.pdf";
                return result;


                //var buffer = HtmlToPdf(html, "");
                //var result = Request.CreateResponse(HttpStatusCode.OK);
                //result.Content = new StreamContent(new MemoryStream(buffer));
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                //result.Content.Headers.ContentLength = buffer.Length;
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentDisposition.FileName = $"Estimate_{DateTime.Now.ToShortDateString()}.pdf";
                //return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
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

        public static byte[] HtmlToPdf(string html, string filePath)
        {
            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                        {
                            using (var sr = new StringReader(html))
                            {
                                htmlWorker.Parse(sr);
                            }
                        }
                        //using (var srHtml = new StringReader(html))
                        //{
                        //    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        //}
                        //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        //{
                        //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                        //    {
                        //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                        //    }
                        //}
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            return bytes;
        }

        public struct PdfHelper
        {
            public string PostCode { get; set; }
        }
    }

    public class TempController : System.Web.Mvc.ControllerBase { protected override void ExecuteCore() { } }

    // iTextSharp Extended Library Starts
    //void GeneratePdfFromHtml()
    //{
    //    const string outputFilename = @".\Files\report.pdf";
    //    const string inputFilename = @".\Files\report.html";

    //    using (var input = new FileStream(inputFilename, FileMode.Open))
    //    using (var output = new FileStream(outputFilename, FileMode.Create))
    //    {
    //        CreatePdf(input, output);
    //    }
    //}

    //void CreatePdf(Stream htmlInput, Stream pdfOutput)
    //{
    //    using (var document = new Document(PageSize.A4, 30, 30, 30, 30))
    //    {
    //        var writer = PdfWriter.GetInstance(document, pdfOutput);
    //        var worker = XMLWorkerHelper.GetInstance();

    //        document.Open();
    //        worker.ParseXHtml(writer, document, htmlInput, null, Encoding.UTF8, new UnicodeFontFactory());

    //        document.Close();
    //    }
    //}

    //public class UnicodeFontFactory : FontFactoryImp
    //{
    //    private static readonly string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
    //      "Kozuka Mincho Pro.ttf");

    //    private readonly BaseFont _baseFont;

    //    public UnicodeFontFactory()
    //    {
    //        _baseFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    //    }

    //    public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
    //      bool cached)
    //    {
    //        return new Font(_baseFont, size, style, color);
    //    }
    //}
    // iTextSharp Extended Library Ends
}