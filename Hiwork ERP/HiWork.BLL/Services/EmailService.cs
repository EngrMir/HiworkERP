using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.ServiceModel;
using HiWork.BLL.Models;

namespace HiWork.BLL.Services
{
    public class EmailService
    {
        public bool SendEmail(string recepientEmail, string cc, string Bcc, string subject, string body, List<string> files, bool isHtmlBody)
        {
            bool emailSent = false;
            using (MailMessage mailMessage = new MailMessage())
            {
                string[] toEmails = null;
                string[] toCCs = null;
                string[] toBCCs = null;
                try
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SMTP_Server"]);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = isHtmlBody;
                    toEmails = recepientEmail.Split(',');
                    if (cc != null)
                        toCCs = cc.Split(',');

                    if (toEmails != null && toEmails.ToList().Count > 0)
                    {
                        foreach (string t in toEmails)
                        {
                            if (t != string.Empty)
                            {
                                mailMessage.To.Add(new MailAddress(t));
                            }
                        }
                    }
                    else
                    {
                        if (recepientEmail != null && recepientEmail != string.Empty)
                            mailMessage.To.Add(new MailAddress(recepientEmail));
                    }

                    if (toCCs != null && toCCs.ToList().Count > 0)
                    {
                        foreach (string c in toCCs)
                        {
                            if (c != string.Empty)
                            {
                                mailMessage.CC.Add(new MailAddress(c));
                            }
                        }
                    }
                    else
                    {
                        if (cc != null && cc != string.Empty)
                            mailMessage.CC.Add(new MailAddress(cc));
                    }
                    if (toBCCs != null && toBCCs.ToList().Count > 0)
                    {
                        foreach (string c in toBCCs)
                        {
                            if (c != string.Empty)
                            {
                                mailMessage.Bcc.Add(new MailAddress(c));
                            }
                        }
                    }
                    else
                    {
                        if (Bcc != null && Bcc != string.Empty)
                            mailMessage.Bcc.Add(new MailAddress(Bcc));
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SMTP_Mail"];
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PortNo"]);
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["SMTP_Server"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["SMTP_Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    if (files != null && files.Count > 0)
                    {
                        foreach (string file in files)
                        {
                            Attachment attachment;
                            attachment = new Attachment(file);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                    smtp.Send(mailMessage);
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    emailSent = false;
                    throw new FaultException(ex.Message);
                }
            }
            return emailSent;
        }

        public bool SendContactusEmail(ContactUsModel model)
        {
            string Greetings = "Thank you very much for contacting us Our staff will contact back to you later.";
            string Subject = "Request to tie-up with trans-Pro.";
            string Title = "Inquiry about Trans-pro. Partnership";
            SendContactUsEmailToClient(Title, Subject, model, Greetings);
            SendContactUsEmailToAdmin(Title, Subject, model, "");

            return true;

        }

        public bool SendContactUsEmailToAdmin(string Title, string Subject, ContactUsModel model, string Greetings)
        {
            bool emailSent = false;
            string templateName = string.Format("contacUs_{0}.html", model.CurrentCulture);

            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    mailMessage.From = new MailAddress(model.Email);
                    mailMessage.Subject = Subject;
                    mailMessage.Body = CreateContactUsEmailBody(model, Title, templateName, Greetings);
                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["SMTP_Server"]));
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SMTP_Mail"];
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PortNo"]);
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["SMTP_Server"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["SMTP_Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    emailSent = false;
                    throw new FaultException(ex.Message);
                }

            }
            return emailSent;
        }



        public bool SendContactUsEmailToClient(string Title, string Subject, ContactUsModel model, string Greetings)
        {
            bool emailSent = false;
            string templateName = string.Format("contacUs_{0}.html", model.CurrentCulture);
            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SMTP_Server"]);
                    mailMessage.Subject = Subject;
                    mailMessage.Body = CreateContactUsEmailBody(model, Title, templateName, Greetings);
                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(new MailAddress(model.Email));
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SMTP_Mail"];
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PortNo"]);
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["SMTP_Server"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["SMTP_Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    emailSent = false;
                    throw new FaultException(ex.Message);
                }

            }
            return emailSent;
        }

        private string PopulateBody(string userName, string url, string culture, string templateName)
        {
            string body = string.Empty;
            try
            {
                string fileName = string.Empty;
                fileName = GetTemplateURL(templateName);
                using (StreamReader reader = new StreamReader(fileName))
                {
                    body = reader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                throw new FaultException("Unable to populate email body." + ex.Message);
            }

            return body;
        }
        public string GetTemplateURL(string template)
        {
            return HostingEnvironment.MapPath(string.Format("~/EmailTemplate/{0}", template));
        }

        public void SendHtmlFormattedMail(EmailModel model, string type, string culture, string templateName)
        {
            try
            {
                //log.Debug("Url traking");

                // log.Debug("Url==========" +  System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/");
                //log.Debug("Url==========" + System.Web.HttpContext.Current.Request.FilePath);

                //var url1 = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri + "SetPassword.aspx?UserId = " + oUserData.Id;
                //var baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                //var url = baseUrl + "Email/SetPassword.aspx?UserId=" + oUserData.Id;

                // var url = "http://localhost:63487/#/" + string.Format("setPassword-{0}",culture)+"?UserID=" + model.UserID+"&UserType="+type;
                var url = "http://163.47.35.165:8086/#/" + string.Format("setPassword-{0}", culture) + "?UserID=" + model.UserID + "&UserType=" + type;
                string body = this.PopulateBody(model.Name, url, culture, templateName);

                this.SendEmail(model.EmailTo, model.EmailCc, model.EmailBcc, "Password reset", body, new List<string>(), true);
            }
            catch (Exception ex)
            {
                throw new FaultException("Unable to email." + ex.Message);
            }
        }
        public void SendResetPasswordMail(EmailModel model, string type, string culture, string templateName)
        {
            try
            {
                //log.Debug("Url traking");

                // log.Debug("Url==========" +  System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/");
                //log.Debug("Url==========" + System.Web.HttpContext.Current.Request.FilePath);

                //var url1 = OperationContext.Current.Channel.LocalAddress.Uri.AbsoluteUri + "SetPassword.aspx?UserId = " + oUserData.Id;
                //var baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                //var url = baseUrl + "Email/SetPassword.aspx?UserId=" + oUserData.Id;

                // var url = "http://localhost:63487/#/" + string.Format("setPassword-{0}",culture)+"?UserID=" + model.UserID+"&UserType="+type;               
                var url = "http://163.47.35.165:8086/#/" + string.Format("setPassword-{0}", culture) + "?UserID=" + model.UserID + "&UserType=" + type;
                string body = this.PopulateBody(model.Name, url, culture, templateName);
                body = body.Replace("{name}", model.Name);
                body = body.Replace("{action_url}", url);
                this.SendEmail(model.EmailTo, model.EmailCc, model.EmailBcc, "Password reset", body, new List<string>(), true);
            }
            catch (Exception ex)
            {
                throw new FaultException("Unable to email." + ex.Message);
            }
        }

        private string CreateContactUsEmailBody(ContactUsModel model, string Title, string templateName, string Greetings)
        {

            string body = string.Empty;
            string fileName = string.Empty;
            fileName = GetTemplateURL(templateName);

            using (StreamReader reader = new StreamReader(fileName))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Title}", Title);
            body = body.Replace("{Message}", Greetings);
            body = body.Replace("{ContactDate}", DateTime.Now.ToString("yyyy/MM/dd"));
            body = body.Replace("{CompanyName}", model.CompanyName ?? "");
            body = body.Replace("{Name}", model.Name ?? "");
            body = body.Replace("{Department}", model.DivisionName ?? "");
            body = body.Replace("{PhoneNumber}", model.TelNumber ?? "");
            body = body.Replace("{Email}", model.Email);
            body = body.Replace("{Comments}", model.Comment ?? "");
            body = body.Replace("{WebURL1}", model.CompanyURLOne ?? "");
            body = body.Replace("{WebURL2}", model.CompanyURLTwo ?? "");
            return body;

        }


        public bool SendEstimationRequestEmailToAdmin(EstimationModel model)
        {
            bool emailSent = false;
            string fileName = string.Format("EstimationRequest_{0}.html", model.CurrentCulture);

            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    string body = string.Empty;


                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{ContactDate}", DateTime.Now.ToString("yyyy/MM/dd"));
                    body = body.Replace("{EstimationNo}", model.EstimationNo);
                    body = body.Replace("{CompanyName}", model.BillingCompanyName ?? "");
                    body = body.Replace("{Name}", model.ClientPersonInCharge ?? "");
                    body = body.Replace("{Adress}", model.BillingAddress ?? "");
                    body = body.Replace("{PhoneNumber}", model.BillingContactNo ?? "");
                    body = body.Replace("{Email}", model.BillingEmailCC ?? "");

                    mailMessage.From = new MailAddress(model.BillingEmailCC);
                    mailMessage.Subject = "Estimation Request";
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["SMTP_Server"]));
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SMTP_Mail"];
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PortNo"]);
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["SMTP_Server"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["SMTP_Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;

                    smtp.Send(mailMessage);
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    emailSent = false;
                    throw new FaultException(ex.Message);
                }

            }
            return emailSent;



        }
    }
}
