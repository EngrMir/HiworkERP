using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HiWork.Utils;
using System.Collections.Generic;
using System.Configuration;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using Newtonsoft.Json;
using System.Web.Configuration;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
     [Authorize]
    public class EmailController : ApiController
    {

        IEmailTemplateService service;

        public EmailController(IEmailTemplateService _service)
        {
            service = _service;

        }

        [Route("email/send")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendEmail()
        {
            string fileName = "";
            var root = HttpContext.Current.Server.MapPath("~/App_Data/Uploadfiles/");
            try
            {                            
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                List<string> files = new List<string>();
                var model = result.FormData["model"];
                if (model == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                if (this.ModelState.IsValid)
                {
                    //
                    var emailModel = JsonConvert.DeserializeObject<EmailModel>(model);

                    foreach (var fileData in result.FileData)
                    {
                        //TODO: Do something with uploaded file.  
                        fileName = fileData.Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        //string f = Path.GetFileNameWithoutExtension(fileName);
                        //string file = Path.GetExtension(fileName);
                        //string n = f.Replace(f, resultData.ID.ToString());
                        // string file = n + e;
                        string sPath = root;
                        File.Copy(fileData.LocalFileName, Path.Combine(sPath, fileName));
                        string filePath = sPath + fileName;
                        files.Add(filePath);
                        //new EmailService().SendEmail(emailModel.EmailTo, emailModel.EmailSubject, emailModel.EmailBody, filePath, false);
                    }
                    new EmailService().SendEmail(emailModel.EmailTo,emailModel.EmailCc,emailModel.EmailBcc, emailModel.EmailSubject, emailModel.EmailBody, files, false);
                    Utility.ClearFolder(root);
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
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

        [Route("email/save")]
        [HttpPost]
        public HttpResponseMessage saveemailTemplate(EmailTemplateModel aEmailTemplateModel)
        {
            List<EmailTemplateModel> result;

            try
            {
                if (this.ModelState.IsValid)
                {
                     result = service.SaveEmailTemplate(aEmailTemplateModel);

                    if (result !=null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("email/list")]
        [HttpPost]
        public HttpResponseMessage GetEmailList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<EmailTemplateModel> dataList;
            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetEmailTemplateList(model);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving Division list";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }

        [Route("email/update")]
        [HttpPost]
        public HttpResponseMessage updateemailTemplate(EmailTemplateModel aEmailTemplateModel)
        {
            

            try
            {
                if (this.ModelState.IsValid)
                {
                    bool  result = service.UpdateEmailTemplate(aEmailTemplateModel);

                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("email/grouplist")]
        [HttpPost]
        public HttpResponseMessage getEmailGroupList(BaseViewModel aBaseViewModel)
        {
            HttpResponseMessage result;
            List<MasterEmailGroupSettingsModel> dataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetEmailGroupList(aBaseViewModel);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving Division list";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }

        [Route("email/categorylist")]
        [HttpPost]
        public HttpResponseMessage getEmailCategoryList(BaseViewModel aBaseViewModel)
        {
            HttpResponseMessage result;
            List<MasterEmailCategorySettingsModel> dataList;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    dataList = service.GetEmailCategoryList(aBaseViewModel);
                    if (dataList != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, dataList);
                    }
                    else
                    {
                        string message = "Error while retriving Division list";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
            return result;
        }

    }
}
