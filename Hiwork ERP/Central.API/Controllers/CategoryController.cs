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
    public class CategoryController : ApiController
    {
        ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Route("category/save")]
        [HttpPost]
        public HttpResponseMessage Save(CategoryModel aCategoryModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var categoryModelList = _categoryService.SaveCategory(aCategoryModel);
                    if (categoryModelList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, categoryModelList);
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

        [Route("category/list")]
        [HttpPost]
        public HttpResponseMessage GetCategoryModels(CategoryModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var CategoryList = _categoryService.GetAllCategoryList(model);
                    if (CategoryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, CategoryList);
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

        [Route("Category/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCategory(CategoryModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = _categoryService.DeleteCategory(model);
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
