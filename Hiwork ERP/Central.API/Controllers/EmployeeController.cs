


using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;

namespace Central.API.Controllers
{
    public class EmployeeController : ApiController
    {
        IEmployeeService service;  
        public EmployeeController (IEmployeeService _service)
        {
            service = _service;
        }


        [Route("employee/save")]
        [HttpPost]
        public HttpResponseMessage SaveEmployee(EmployeeModel model)
        {
            HttpResponseMessage result;

            try
            {
                if (this.ModelState.IsValid)
                {
                 var   employeedata = service.SaveEmployee(model);
                    result = Request.CreateResponse(HttpStatusCode.OK, employeedata);     
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
        [Route("employee/conlist/{con}")]
        [HttpPost]
        public HttpResponseMessage ConList(BaseViewModel model, string con)
        {
            HttpResponseMessage result;

            try
            {
                if (this.ModelState.IsValid)
                {
                    var datalist = service.GetSearchEmployeeList(model,con);
                    result = Request.CreateResponse(HttpStatusCode.OK, datalist);
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
        [Route("employee/rolelist")]
        [HttpPost]
        public HttpResponseMessage RoleList(BaseViewModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var countryList = service.GetAllRoleList(model);
                    if (countryList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, countryList);
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
        [Route("employee/list")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeList(BaseViewModel model)
        {
            HttpResponseMessage result;

            try
            {
                if (this.ModelState.IsValid)
                {
                  var  datalist = service.GetEmployeeList(model);
                    result = Request.CreateResponse(HttpStatusCode.OK, datalist);
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
      

        [Route("employee/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEmployee(EmployeeModel model)
        {
            HttpResponseMessage result;


            try
            {
                if (this.ModelState.IsValid)
                {
                  var  datalist = service.DeleteEmployee(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting Employee data";
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


        [Route("employee/edit")]
        [HttpPost]
        public HttpResponseMessage EditEmployee(EmployeeModel model)
        {
            HttpResponseMessage result;
            EmployeeModel employeedata;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    employeedata = service.UpdateEmployee(model);
                    if (employeedata != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, employeedata);
                    }
                    else
                    {
                        string message = "Error while updating Employee data";
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


        [Route("employee/formData")]
        public HttpResponseMessage EmployeeFormData(BaseViewModel model)
        {
            HttpResponseMessage result;

            try
            {
               var formData = service.GetEmployeeFormData(model);
                result = Request.CreateResponse(HttpStatusCode.OK, formData);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
            return result;
        }

        [HttpPost]
        [Route("employee/IsExisting")]
        public HttpResponseMessage CheckEmployeeID(EmployeeModel model)
        {
            try
            {

             var Status = service.CheckEmployeeByEmployeeID(model);
           
             return Request.CreateResponse(HttpStatusCode.OK, Status);
            
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }
    }
}