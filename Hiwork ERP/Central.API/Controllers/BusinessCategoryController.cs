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
    public class BusinessCategoryController : ApiController
    {
        [Route("businesscategory/save")]
        [HttpPost]
        public HttpResponseMessage Save(BusinessCategoryModel aBusinessCategoryModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IBusinessCategoryRepository businessCategory = new BusinessCategoryRepository(uWork);
            IBusinessCategoryService businessCategoryService = new BusinessCategoryService(businessCategory);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var BusinessCategoryModelList = businessCategoryService.SaveBusinessCategory(aBusinessCategoryModel);
                    if (BusinessCategoryModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, BusinessCategoryModelList);
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

        [Route("businesscategory/list")]
        [HttpPost]
        public HttpResponseMessage GetBusinessCategoryModels(BusinessCategoryModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IBusinessCategoryRepository businessCategory = new BusinessCategoryRepository(uWork);
            IBusinessCategoryService businessCategoryService = new BusinessCategoryService(businessCategory);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var BusinessCategoryList = businessCategoryService.GetAllBusinessCategoryList(model);
                    if (BusinessCategoryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, BusinessCategoryList);
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

        [Route("businesscategory/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteBusinessCategory(BusinessCategoryModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IBusinessCategoryRepository businessCategory = new BusinessCategoryRepository(uWork);
            IBusinessCategoryService businessCategoryService = new BusinessCategoryService(businessCategory);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = businessCategoryService.DeleteBusinessCategory(model);
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
