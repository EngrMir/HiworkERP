


using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using PayPal.Api;
using System.Net.Http;
using System.Net;
using HiWork.BLL.Services;
using HiWork.BLL.Models;

namespace Central.API.Controllers
{
    public class PaypalController : ApiController
    {
        private IPayPalTransactionService service;
        public PaypalController()
        {
            service = new PayPalTransactionService();
        }

        [Route("paypal/paymentwithpaypal")]
        [HttpPost]
        public HttpResponseMessage PaymentWithPaypal(PayPalModel model)
        {
            string payerId = model.PayerID;     //Request.Params["PayerID"];
            string guid = model.guid;
            APIContext apiContext = PaypalHelper.Configuration.GetAPIContext();

            try
            {
                if (string.IsNullOrEmpty(payerId))
                {

                    // this section will be executed first because PayerID doesn't exist
                    // it is returned by the create function call of the payment class
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/paypal/paymentwithpaypal";

                    // guid we are generating for storing the paymentID received in session
                    // after calling the create function and it is used in the payment execution
                    guid = Convert.ToString((new Random()).Next(100000));

                    // CreatePayment function gives us the payment approval url
                    // on which payer is redirected for paypal acccount payment
                    Payment createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    // get links returned from paypal in response to Create function call
                    IEnumerator<Links> links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext() == true)
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    // HttpContext.Current.Session["guid"] = createdPayment.id;
                    // return Redirect(paypalRedirectUrl);
                    service.InsertPaypalTransactionData(guid, createdPayment.id, model.ApplicationId);

                    var response = Request.CreateResponse(HttpStatusCode.Moved);
                    response.Headers.Location = new Uri(paypalRedirectUrl);
                    return response;
                }
                else {

                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment
                    // var guid = Request.Params["guid"];

                    string paymentid = service.GetPaymentIDByGuid(guid);
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentid);

                    // transaction with system db

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        // return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                // Logger.Log("Error" + ex.Message);
                // return View("FailureView");
            }
            return null;
        }

        private Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            PaymentExecution paymentExecution = new PaymentExecution();
            paymentExecution.payer_id = payerId;
            this.payment = new Payment();
            payment.id = paymentId;
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            // similar to credit card create itemlist and add item objects to it
            ItemList itemList = new ItemList() { items = new List<Item>() };
            itemList.items.Add(new Item()
            {
                name = "Item Name",
                currency = "USD",
                price = "5",
                quantity = "1",
                sku = Guid.NewGuid().ToString()
            });

            Payer payer = new Payer();
            payer.payment_method = "paypal";

            // Configure Redirect Urls here with RedirectUrls object
            RedirectUrls redirUrls = new RedirectUrls();
            redirUrls.cancel_url = redirectUrl;
            redirUrls.return_url = redirectUrl;

            // similar as we did for credit card, do here and create details object
            Details details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "5"
            };

            // similar as we did for credit card, do here and create amount object
            Amount amount = new Amount()
            {
                currency = "USD",
                total = "7",                    // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            List<Transaction> transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
    }
}