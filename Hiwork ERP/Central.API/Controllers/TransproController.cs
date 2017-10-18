using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.BLL.ViewModels;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Central.API.Controllers
{

    public class TransproController : ApiController
    {
        ITransproService _service;
        public TransproController(ITransproService service)
        {
            _service = service;
        }
        [Route("customer/register")]
        [HttpPost]
        public HttpResponseMessage SaveCompany(CompanyModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.RegisterCustomer(model);

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
        [Route("customer/login/{email}/{password}/{culture}")]
        [HttpGet]
        public HttpResponseMessage CustomerLogin(string email,string password,string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.CustomerLogin(email, password, culture);
                    if(result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Email or Password.");
                    }
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

        [Route("partner/login/{email}/{password}/{culture}")]
        [HttpGet]
        public HttpResponseMessage PartnerLogin(string email, string password, string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.PartnerLogin(email, password, culture);
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Email or Password.");
                    }
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

        [Route("translator/registration")]
        [HttpPost]
        public HttpResponseMessage Savetranslator(StaffModel model)
        {
            try
            {
                //if (this.ModelState.IsValid)
                //{
                    var result = _service.RegisterTranslator(model);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                    
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
       

        [Route("customer/getCustomerById/{customerId}/{cultureId}")]
        [HttpGet]
        public HttpResponseMessage GetCustomerById(string customerId, string cultureId)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.GetCustomerById(customerId, cultureId);
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error in getting records.");
                    }
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
        [Route("customer/checkExistingCustomer/{emailId}/{culture}")]
        [HttpGet]
        public HttpResponseMessage CheckExistingCustomer(string emailId, string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.CheckExistingCustomer(emailId, culture);
                    if (result)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Email ID already exist,Please use another one");
                    }
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



        [Route("translator/login/{email}/{password}/{culture}")]
        [HttpGet]
        public HttpResponseMessage TranlatorLogin(string email, string password, string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.TranlatorLogin(email, password, culture);
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Email or Password.");
                    }
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

        [Route("translator/checkpassword/{email}/{password}")]
        [HttpGet]
        public HttpResponseMessage CheckTranslatorPassword(string email, string password)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.CheckTranslatorPassword(email, password);
                    if(result ==false)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error in getting records.");
                    }
                    else { 
                    return Request.CreateResponse(HttpStatusCode.OK, result);
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


        [Route("ChangePhoto/save")]
        [HttpPost]
        public HttpResponseMessage ChangePhoto(StaffModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.ChangePhoto( model.ID,model.Image);

                    if (!result)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid photo.");
                    }

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

        [Route("ChangePassword/save")]
        [HttpPost]
        public HttpResponseMessage ChangePassword(StaffModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.ChangePassword(model.ID, model.Password);

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

        


        [Route("translator/getTranslatorById/{translatorId}/{cultureId}")]
        [HttpGet]
        public HttpResponseMessage getTranslatorById(string translatorId, string cultureId)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    ITransproService _service = new TransproService();
                    var result = _service.GetTranslatorById(translatorId, cultureId);
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error in getting records.");
                    }
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

        [Route("traslator/searchTranslator")]
        [HttpPost]
        public HttpResponseMessage SearchTranslator(BaseViewModel model,string srcLanguageID, string targetLanguageID,string specialFieldID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.SearchTranslators(model,srcLanguageID == "0"? (Guid?)null : Guid.Parse(srcLanguageID), targetLanguageID == "0" ? (Guid?)null : Guid.Parse(targetLanguageID), specialFieldID == "0" ? (Guid?)null : Guid.Parse(specialFieldID));
                    
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
        /// <summary>
        /// An email will be sent to user's email address with password reset instruction.
        /// </summary>
        /// <param name="resetModel"></param>
        [Route("customer/resetPassword/{email}/{type}/{culture}")]
        [HttpGet]
        public HttpResponseMessage ResetPassword(string email,string type,string culture) 
        {
            try
            {
                var user = _service.ResetPassword(email,type, culture);
                return Request.CreateResponse(HttpStatusCode.OK, true); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }



        [Route("translator/checkExistingTranstalor/{emailId}/{culture}")]
        [HttpGet]
        public HttpResponseMessage CheckExistingTranlator(string emailId, string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    ITransproService _service = new TransproService();
                    var result = _service.CheckExistingTranslator(emailId, culture);
                    if (!result)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error in getting records.");
                    }
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

        [Route("setPassword/setNewPassword")]
        [HttpPost]
        public HttpResponseMessage SetNewPassword(ResetPassword model)
        {
            try
            {
                var data = _service.SetNewPassword(model);
                return Request.CreateResponse(HttpStatusCode.OK, true); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("order/getWebOrders")]
        [HttpPost]
        public HttpResponseMessage GetWebWorders(OrderFilter filter)
        {
            try
            {
                IOrderWebService service = new OrderWebService();
                var data = service.GetAllWebOrderList(filter);
                return Request.CreateResponse(HttpStatusCode.OK, data); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("order/getAppointedOrders")]
        [HttpPost]
        public HttpResponseMessage GetAppointedOrders(OrderFilter filter)
        {
            try
            {
                IOrderWebService service = new OrderWebService();
                var data = service.GetAppointedOrderList(filter);
                return Request.CreateResponse(HttpStatusCode.OK, data); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("order/saveWebOrder")]
        [HttpPost]
        public HttpResponseMessage SaveWebOrder(OrderWebModel model)
        {
            try
            {
                IOrderWebService service = new OrderWebService();
                string OrderNo = service.SaveWebOrder(model);
                return Request.CreateResponse(HttpStatusCode.OK, OrderNo);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("translator/getImageById/{translatorId}/{cultureId}")]
        [HttpGet]
        public HttpResponseMessage getImageById(string translatorId, string cultureId)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    ITransproService _service = new TransproService();
                    var result = _service.GetImageById(translatorId, cultureId);
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error in getting Images.");
                    }
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
        [Route("order/getCurrentMonthOrders")]
        [HttpPost]
        public HttpResponseMessage GetCurrentMonthOrders(OrderFilter filter)
        {
            try
            {
                IOrderWebService service = new OrderWebService();
                filter.startDate = DateTime.SpecifyKind(filter.firstDateMonth, DateTimeKind.Utc);
                filter.endDate = DateTime.SpecifyKind(filter.lastDateMonth, DateTimeKind.Utc);
                var data = service.GetAllWebOrderList(filter);
                return Request.CreateResponse(HttpStatusCode.OK, data); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("transpro/deliverymethod")]
        [HttpPost]
        public HttpResponseMessage GetDeliveryMethod(BaseViewModel model)
        {
            try
            {
                IUnitOfWork ouw = new UnitOfWork();
                ICompanyRepository rep = new CompanyRepository(ouw);
                ICompanyService service = new CompanyService(rep);
                var data = service.GetDeliveryMethod(model);
                return Request.CreateResponse(HttpStatusCode.OK, data); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("translator/getTranslatorProfile")]
        [HttpPost]
        public HttpResponseMessage GetTranslatorProfile(TranslatorFilterModel filter)
        {
            try
            {
                ITransproService _service = new TransproService();
                var result = _service.GetTranslatorProfile(filter);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        
        [Route("order/orderOperation")]
        [HttpPost]
        public HttpResponseMessage OrderOperation(OrderWebModel model,string type)
        {
            try
            {
                IOrderWebService service = new OrderWebService();
                OrderWebModel data = service.OrderOperation(model, type);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("translator/withdrawmembership")]
        [HttpPost]
        public HttpResponseMessage WithDrawTranslatorMembership(StaffModel model)
        {
            try
            {
                ITransproService _service = new TransproService();
                var result = _service.WithDrawTranslatorMembership(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("translator/getMasterData")]
        [HttpPost]
        public HttpResponseMessage GetMasterData(BaseViewModel model)
        {
            try
            {
                ITransproService service = new TransproService();
                if (this.ModelState.IsValid)
                {
                    var checkboxvalue = service.GetMasterData(model);
                    if (checkboxvalue != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, checkboxvalue);
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }


        [Route("transpro/estimationsave")]
        [HttpPost]
        public HttpResponseMessage Save(EstimationModel model)
        {
            try
            {

                IEstimationService service = new EstimationService();
                var result = service.SaveEstimation(model);
                if (result)
                {
                    new EmailService().SendEstimationRequestEmailToAdmin(model);

                    return Request.CreateResponse(HttpStatusCode.OK, result);
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


        [Route("message/getReceiveMsgById/{receiverID}")]
        [HttpGet]
        public HttpResponseMessage getReceiveMsgById(Guid receiverID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.getReceiveMsgById(receiverID);
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


        [Route("message/getSendMsgById/{senderID}")]
        [HttpGet]
        public HttpResponseMessage getSendMsgById(Guid senderID)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _service.getSendMsgById(senderID);
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

        [Route("msg/getbyid/{msgId}")]
        [HttpGet]
        public HttpResponseMessage GetMessageDetails(long msgId)
        {
            try
            {
                ITransproService service = new TransproService();
                var data = service.getDetailsbymsgId(msgId);
                return Request.CreateResponse(HttpStatusCode.OK, data); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
