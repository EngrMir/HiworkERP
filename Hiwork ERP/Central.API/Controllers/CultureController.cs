using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using System.Net;
using System;
using HiWork.Utils.Infrastructure;

namespace Central.API.Controllers
{
    
    public class CultureController : ApiController
    {
        ICultureService service;
        public CultureController(ICultureService _service)
        {
            service = _service;
        }

        [Route("culture/save")]
        [HttpPost]
        public HttpResponseMessage Save(CultureModel aCultureModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var culList = service.SaveCulture(aCultureModel);
                    if (culList != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, culList);
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


        [Route("culture/list")]
        [HttpPost]
        public HttpResponseMessage GetCultures(CultureModel aCultureModel)
        {
            //IUnitOfWork uWork = new UnitOfWork();
            //ICountryRepository country_repo = new CountryRepository(uWork);
            //ICountryService country_Service = new CountryService(country_repo);
            try
            {
                //if (this.ModelState.IsValid)
                //{
                //    CountryModel aCountryModel = new CountryModel();
                //    aCountryModel.CurrentCulture = aCultureModel.CurrentCulture;
                //    aCountryModel.CurrentUserID = aCultureModel.CurrentUserID;

                //    var culList = service.GetAllCultureList(aCultureModel);
                //    var countryList = country_Service.GetAllCountryList(aCountryModel);

                //    if (culList != null && countryList !=null)
                //    {
                //        var result = (from c in countryList
                //                      join cl in culList on c.Id equals cl.CountryID
                //                      select new
                //                      {
                //                          Id = cl.ID,
                //                          Country = c.Name,
                //                          Code = cl.Code,
                //                          IsActive = cl.IsActive

                //                      }).ToList(); 
                //        return Request.CreateResponse(HttpStatusCode.OK, result);
                //    }
                //    else
                //    {
                //        string message = "Error in getting Data";
                //        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                //    }

                //}
                //else
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                //}
                var result = service.GetAllCultureList(aCultureModel);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }

        }
        [Route("culture/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCulture(CultureModel aCultureModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.DeleteCulture(aCultureModel);
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
        [Route("culture/edit")]
        [HttpPost]
        public HttpResponseMessage EditCulture(CultureModel aCultureModel)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var culture = service.EditCulture(aCultureModel);
                    if (culture != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, culture);
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


        [Route("culture/list")]
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                if (this.ModelState.IsValid)
                {

                    var culList = service.GetAll();

                    List<CultureModel> cultures = new List<CultureModel>();

                    if (culList != null)
                    {

                        foreach (var culture in culList)
                        {

                            var c = new CultureModel();
                            c.ID = culture.ID;
                            c.CountryID = culture.CountryID;
                            c.Code = culture.Code;
                            c.Name = culture.Description;
                            c.SortBy = culture.SortBy;
                            cultures.Add(c);
                        }


                        cultures = cultures.OrderBy(o => o.SortBy).ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, cultures);
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
    }
}
