using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using HiWork.DAL.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http.Headers;
using HiWork.Utils;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    [Authorize]
    public class AdvancedStaffSearchController : ApiController
    {
        IAdvancedStaffSearchService _service;
        public AdvancedStaffSearchController(IAdvancedStaffSearchService service)
        {
            _service = service;
        }

        [Route("advancedstaffsearchlist/search")]
        [HttpPost]
        public HttpResponseMessage Search(AdvancedStaffSearchModel model)
        {
            try
            {
                var ret = _service.GetAdvancedStaffSearch(model);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("advancedstaffsearch/getdata")]
        [HttpPost]
        public HttpResponseMessage GetData(temp t)
        {
            try
            {
                var ret = _service.GetData(t.type, t.culture);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #region "Init"
        [Route("advancedStaffSearch/getsourceofregistration")]
        [HttpPost]
        public HttpResponseMessage getsourceofregistration(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetSourceOfRegistration(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getlanguage")]
        [HttpPost]
        public HttpResponseMessage getlanguage(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetLanguage(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getlanguagelevel")]
        [HttpPost]
        public HttpResponseMessage getlanguagelevel(BaseViewModel data)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Utility.getItemCultureList(Utility.LanguageLevelList, data));
                //var ret = _service.GetLanguageLevel(data);
                //return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getage")]
        [HttpPost]
        public HttpResponseMessage getage(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetAge(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getnationalitygroup")]
        [HttpPost]
        public HttpResponseMessage getnationalitygroup(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetNationalityGroup(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getnationality")]
        [HttpPost]
        public HttpResponseMessage getnationality(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetNationality(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getvisaType")]
        [HttpPost]
        public HttpResponseMessage getvisaType(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetVisaType(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getvisaexpire")]
        [HttpPost]
        public HttpResponseMessage getvisaexpire(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetVisaExpire(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getsnsaccount")]
        [HttpPost]
        public HttpResponseMessage getsnsaccount(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetSnsAccount(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getdtp")]
        [HttpPost]
        public HttpResponseMessage getdtp(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetDtp(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getofficetype")]
        [HttpPost]
        public HttpResponseMessage getofficetype(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetOfficeType(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getwebtype")]
        [HttpPost]
        public HttpResponseMessage getwebtype(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetWebType(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/gettranslationtools")]
        [HttpPost]
        public HttpResponseMessage gettranslationtools(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetTranslationTools(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/gettoolname")]
        [HttpPost]
        public HttpResponseMessage gettoolname(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetToolName(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getdesign")]
        [HttpPost]
        public HttpResponseMessage getdesign(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetDesign(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getsoftwarename")]
        [HttpPost]
        public HttpResponseMessage getsoftwarename(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetSoftwareName(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/gettin")]
        [HttpPost]
        public HttpResponseMessage gettin(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetTin(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getiin")]
        [HttpPost]
        public HttpResponseMessage getiin(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetIin(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getnin")]
        [HttpPost]
        public HttpResponseMessage getnin(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetNin(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("advancedStaffSearch/getnarrationperformance")]
        [HttpPost]
        public HttpResponseMessage getnarrationperformance(BaseViewModel data)
        {
            try
            {
                var ret = _service.GetNarrationPerformance(data);
                return Request.CreateResponse(HttpStatusCode.OK, (object)ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion "Init"
    }
    public class temp
    {
        public string type { get; set; }
        public string culture { get; set; }
    }
}