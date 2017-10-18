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
    public class DepartmentController : ApiController
    {
        IDepartmentService service;
        public DepartmentController(IDepartmentService _service)
        {
            service = _service;
        }

        #region Department
        [Route("department/save")]
        [HttpPost]
        public HttpResponseMessage Save(DepartmentModel aDepartmentModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var departmentList = service.SaveDepartment(aDepartmentModel);
                    if (departmentList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, departmentList);
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

        [Route("department/list")]
        [HttpPost]
        public HttpResponseMessage GetDepartments(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var departmentList = service.GetAllDepartmentList(model);
                    if (departmentList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, departmentList);
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


        [Route("department/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteDepartments(DepartmentModel aDepartmentModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteDepartment(aDepartmentModel);
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

        [Route("department/formdata")]
        [HttpPost]
        public HttpResponseMessage GetDepartmentFormData(BaseViewModel arg)
        {
            HttpResponseMessage result;
            DepartmentFormModel data;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    data = service.GetDepartmentFormData(arg);
                    if (data != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else
                    {
                        string message = "Error while retriving form data of Department";
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
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }


        //[Route("department/edit")]
        //[HttpPost]
        //public HttpResponseMessage EditDepartment(DepartmentModel aDepartmentModel)
        //{
           
        //    try
        //    {
        //        if (this.ModelState.IsValid)
        //        {
        //            var user = service.EditDepartment(aDepartmentModel);
        //            if (user != null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, user);
        //            }
        //            else
        //            {
        //                string message = "Not updated successfully";
        //                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
        //    }
        //}
        #endregion

    }
}