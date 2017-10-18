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
    [Authorize]
    public class BusinessCategoryDetailsController : ApiController
    {
        [Route("businesscategorydetails/save")]
        [HttpPost]
        public HttpResponseMessage Save(BusinessCategoryDetailsModel aBusinessCategoryDetailsModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IBusinessCategoryDetailsRepository businessCategoryDetails = new BusinessCategoryDetailsRepository(uWork);
            IBusinessCategoryDetailsService businessCategoryDetailsService = new BusinessCategoryDetailsService(businessCategoryDetails);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var businessCategoryDetailsModelList = businessCategoryDetailsService.SaveBusinessCategoryDetails(aBusinessCategoryDetailsModel);
                    if (businessCategoryDetailsModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, businessCategoryDetailsModelList);
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

        [Route("businesscategorydetails/list")]
        [HttpPost]
        public HttpResponseMessage GetBusinessCategoryDetailsModels(BusinessCategoryDetailsModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IBusinessCategoryDetailsRepository businessCategoryDetails = new BusinessCategoryDetailsRepository(uWork);
            IBusinessCategoryDetailsService businessCategoryDetailsService = new BusinessCategoryDetailsService(businessCategoryDetails);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var businessCategoryDetailsList = businessCategoryDetailsService.GetAllBusinessCategoryDetailsList(model);
                    if (businessCategoryDetailsList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, businessCategoryDetailsList);
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

        [Route("businesscategorydetails/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteBusinessCategoryDetails(BusinessCategoryDetailsModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IBusinessCategoryDetailsRepository businessCategoryDetails = new BusinessCategoryDetailsRepository(uWork);
            IBusinessCategoryDetailsService businessCategoryDetailsService = new BusinessCategoryDetailsService(businessCategoryDetails);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = businessCategoryDetailsService.DeleteBusinessCategoryDetails(model);
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
