using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Central.API.Controllers
{
    //[Authorize]
    public class LanguageController : ApiController
    {
        ILanguageService service;
        public LanguageController(ILanguageService _service)
        {
            service = _service;
        }
        [Route("language/save")]
        [HttpPost]
        public HttpResponseMessage Save(LanguageModel model)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    var branchList = service.SaveLanguage(model);
                    if (branchList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, branchList);
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

        [Route("language/list")]
        [HttpPost]
        public HttpResponseMessage GetLanguages(BaseViewModel model)
        {
            
            try
            {
                if (this.ModelState.IsValid)
                {
                    var branchList = service.GetAllLanguageList(model);
                    if (branchList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, branchList);
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


        [Route("languagelvlist/list")]
        [HttpPost]
        public HttpResponseMessage GetLanguageslvl(BaseViewModel model)
        {

            try
            {

                IUnitOfWork uwork = new UnitOfWork();
                CompanyConfigData lanData = new CompanyConfigData();

                lanData.LanguageLevelList = Utility.getItemCultureList(Utility.LanguageLevelList, model);
                return Request.CreateResponse(HttpStatusCode.OK, lanData);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }

        }




        [Route("language/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteLanguage(LanguageModel model)
        {
            

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteLanguage(model);
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


        [Route("language/edit")]
        [HttpPost]
        public HttpResponseMessage EditLanguage(LanguageModel model)
        {
            
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = service.EditLanguage(model);
                    if (user != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }
                    else
                    {
                        string message = "Not updated successfully";
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

        [Route("language/getById")]
        [HttpGet]
        public HttpResponseMessage GetByLanguageId(LanguageModel model)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    BaseViewModel baseViewModel = new BaseViewModel();
                    baseViewModel.CurrentCulture = model.CurrentCulture;
                    baseViewModel.CurrentUserID = model.CurrentUserID;
                    var result = service.GetAllLanguageList(baseViewModel);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        string message = "Not updated successfully";
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
