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
    public class StaffMajorSubController : ApiController
    {

        [Route("staffmajorsub/save")]
        [HttpPost]
        public HttpResponseMessage Save(MajorSubjectModel aMajorSubjectModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IMajorSubjectRepository repo = new MajorSubjectRepository(uWork);
            IMajorSubjectService staffmajorsubService = new MajorSubjectService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffmejorsubList = staffmajorsubService.SaveMajorSubject(aMajorSubjectModel);
                    if (staffmejorsubList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffmejorsubList);
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


        //GetAllMajorSubjectList

        [Route("staffmajorsub/list")]
        [HttpPost]
        public HttpResponseMessage GetAllMajorSubjectList(MajorSubjectModel model)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IMajorSubjectRepository repo = new MajorSubjectRepository(uWork);
            IMajorSubjectService staffmajorsubService = new MajorSubjectService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var staffmejorsubList = staffmajorsubService.GetAllMajorSubjectList(model);
                    if (staffmejorsubList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, staffmejorsubList);
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


        [Route("staffmajorsub/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteMajorSubject(MajorSubjectModel aMajorSubjectModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IMajorSubjectRepository repo = new MajorSubjectRepository(uWork);
            IMajorSubjectService staffmajorsubService = new MajorSubjectService(repo);

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = staffmajorsubService.DeleteMajorSubject(aMajorSubjectModel);
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


        [Route("staffmajorsub/edit")]
        [HttpPost]
        public HttpResponseMessage EditMajorSubject(MajorSubjectModel aMajorSubjectModel)
        {
            IUnitOfWork uWork = new UnitOfWork();
            IMajorSubjectRepository repo = new MajorSubjectRepository(uWork);
            IMajorSubjectService staffmajorsubService = new MajorSubjectService(repo);
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = staffmajorsubService.EditMajorSubject(aMajorSubjectModel);
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