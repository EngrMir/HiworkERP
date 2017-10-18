using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HiWork.BLL.Models;
using HiWork.BLL.Responses;
using HiWork.BLL.ViewModels;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;


namespace Central.API.Controllers
{


    public class StaffTranslationFieldsController : ApiController
    {
        IStaffTranslationFieldsService _stafftransfieldsService;

        public StaffTranslationFieldsController(IStaffTranslationFieldsService stafftransfieldsService)
        {
            _stafftransfieldsService = stafftransfieldsService;
        }

        [Route("stafftranslationfields/save")]
        [HttpPost]
        public HttpResponseMessage Save(StaffTranslationFieldsModel aStaffTranFieldsModel)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    var stafftransfieldsList = _stafftransfieldsService.SaveStaffTranslationFields(aStaffTranFieldsModel);
                    if (stafftransfieldsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, stafftransfieldsList);
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


        //GetAllDepartmentList

        [Route("stafftranslationfields/list")]
        [HttpPost]
        public HttpResponseMessage GetStaffTranslationFields(BaseViewModel model)
        {
          
            try
            {
                if (this.ModelState.IsValid)
                {
                    var stafftransfieldsList = _stafftransfieldsService.GetAllStaffTranslationFieldsList(model);
                    if (stafftransfieldsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, stafftransfieldsList);
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


        [Route("stafftranslationfields/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStaffTranslationFields(StaffTranslationFieldsModel aStaffTranFieldsModel)
        {
           

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _stafftransfieldsService.DeleteStaffTranslationFields(aStaffTranFieldsModel);
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


        [Route("stafftranslationfields/edit")]
        [HttpPost]
        public HttpResponseMessage EditStaffTranslationFields(StaffTranslationFieldsModel aStaffTranFieldsModel)
        {
           
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = _stafftransfieldsService.EditStaffTranslationFields(aStaffTranFieldsModel);
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
    }
}
