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
      [Authorize]
    public class CompanyController : ApiController
    {
         private ICompanyService service;
        public CompanyController(ICompanyService _service)
        {
            service = _service;
        }
        [Route("comapny/update")]
        [Route("comapny/save")]
        [HttpPost]
        public HttpResponseMessage SaveCompany(CompanyModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.SaveCompany(model);
              
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                }
            }
            catch (Exception ex) {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }

        }


        [Route("comapnyTranspro/save")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyTranspro(CompanyTransproPartner model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.SaveCompanyTranspro(model);

                    return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("company/estimationrequestedcompanylist/{EstimationID}")]
        [HttpPost]
        public HttpResponseMessage GetEstimationRequestedCompany(BaseViewModel model, Guid EstimationID)
        {
            try
            {
                var result = service.GetEstimationRequestedCompany(model, EstimationID);
                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    string message = "Error in getting Data";
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }

        [Route("company/sendsequest")]
        [HttpPost]
        public HttpResponseMessage SendRequest(CommonModelHelper model)
        {
            try
            {
                var result = service.SendRequest(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
            }
        }

        [Route("company/list")]
        [HttpPost]
        public HttpResponseMessage GetAllCompany(BaseViewModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                    var result = service.GetAllCompanyList(model);
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);
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
        [HttpPost]
        [Route("company/viewlist")]
        public HttpResponseMessage GetCompanyViewData(BaseViewModel model)
        {
            try
            {
                var result = service.GetCompanyViewList(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("company/getbyid")]
        public HttpResponseMessage GetCompanyByID(BaseViewModel model)
        {
            try
            {
                var result = service.GetCompanyByID(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("company/departmentlist")]
        public HttpResponseMessage GetDepartmentByCompanyID(BaseViewModel model)
        {
            try
            {
                var result = service.GetDepartmentListByCompanyID(model.ID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("company/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteCompany(CompanyModel model)
        {

            try
            {
                if (this.ModelState.IsValid)
                {
                     service.DeleteSoftly(model.ID);

                    return Request.CreateResponse(HttpStatusCode.OK);
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
        [Route("company/edit")]
        [HttpPost]
        public HttpResponseMessage EditCompany(CompanyModel model)
        {

            try
            {
                service.Update(model);  
                return Request.CreateResponse(HttpStatusCode.OK);  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("company/config")]
        [HttpPost]
        public HttpResponseMessage CompanyConfigData(BaseViewModel model)
        {
            try
            {

                IUnitOfWork uwork = new UnitOfWork();
                ICountryService countrySerivce = new CountryService(new CountryRepository(uwork));
                IBranchService branchService = new BranchService(new BranchRepository(uwork));
                IDepartmentService deptService = new DepartmentService(new DepartmentRepository(uwork));
                ILanguageService langService = new LanguageService(new LanguageRepository(uwork));
                IEstimationTypeService estimationTypeService = new EstimationTypeService(new EstimationTypeRepository(uwork));
                IEstimationSpecializedFieldService spetializeService = new EstimationSpecializedFieldService(new EstimationSpecializedFieldRepository(uwork));
                IPartnerServiceTypeService partnerServiceType = new PartnerServiceTypeService(new PartnerServiceTypeRepository(uwork));
                CompanyConfigData configData = new CompanyConfigData();
                
                configData.ClientLocationType= Utility.getItemCultureList(Utility.ClientLocationType,model);
                configData.CompanyType = Utility.getItemCultureList(Utility.CompanyType, model);
                configData.RegPurposeType = Utility.getItemCultureList( Utility.RegPurposeType,model);
                configData.BranchOfficeList = branchService.GetAllBranchList(model);
                configData.AffiliateType = Utility.getItemCultureList(Utility.AffiliateType,model);
                configData.ActivityType = Utility.getItemCultureList( Utility.ActivityType,model);
                configData.ResultofActivity = Utility.getItemCultureList( Utility.ResultofActivity,model);
                configData.DepartmentList = deptService.GetAllDepartmentList(model);
                configData.LanguageList = langService.GetAllLanguageList(model);
                configData.EstimationTypeList = estimationTypeService.GetAllEstimationTypeList(model);
                configData.SpecializationList = spetializeService.GetAllEstimationSpecializedFieldList(model);
                 configData.PriceCalculateTypeList = Utility.getItemCultureList(Utility.PriceCalculateTypeList,model);
                configData.PamentWayList = Utility.getItemCultureList(Utility.PaymentWayList,model);
                configData.PartnerServiceTypeList = partnerServiceType.GetAll(model);
                configData.PartnerTypeList = Utility.getItemCultureList(Utility.CompanyTransproTypeList, model);
                configData.DeliveryMethodList = service.GetDeliveryMethod(model);
                return Request.CreateResponse(HttpStatusCode.OK, configData);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [Route("company/nextcompanyno")]
        [HttpPost]
        public HttpResponseMessage GetNextCompanyNumber(BaseViewModel model)
        {
            try
            {


                var result = service.GetNextCompanyNumber(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("company/industryclassificationviewlist")]  
        [HttpPost]
        public HttpResponseMessage GetCompanyIndustryClassificationViewList(BaseViewModel model)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationViewModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.GetCompanyIndustryClassificationViewList(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while retriving CompanyIndustryClassificationView list";
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

        [Route("company/industryclassificationviewsave")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyIndustryClassificationView(CompanyIndustryClassificationViewModel model)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationViewModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.SaveCompanyIndustryClassificationView(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while saving CompanyIndustryClassificationView data";
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


        [Route("company/industryclassificationviewsavelist")]
        [HttpPost]
        public HttpResponseMessage SaveCompanyIndustryClassificationViewList(List<CompanyIndustryClassificationViewModel> model)
        {
            HttpResponseMessage result;
            bool res;

            try
            {
                //if (this.ModelState.IsValid == true)
                //{
                    res = service.SaveCompanyIndustryClassificationViewList(model);

                    if (res == true)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, res);
                    }
                    else
                    {
                        string message = "Error while saving CompanyIndustryClassificationView list";
                        result = Request.CreateResponse(HttpStatusCode.Forbidden, message);
                    }
                //}
                //else
                //{
                //    result = Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
                //}
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
        }


        [Route("company/industryclassificationviewdelete")]
        [HttpPost]
        public HttpResponseMessage DeleteCompanyIndustryClassificationView(CompanyIndustryClassificationViewModel model)
        {
            HttpResponseMessage result;
            List<CompanyIndustryClassificationViewModel> datalist;

            try
            {
                if (this.ModelState.IsValid == true)
                {
                    datalist = service.DeleteCompanyIndustryClassificationView(model);
                    if (datalist != null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.OK, datalist);
                    }
                    else
                    {
                        string message = "Error while deleting CompanyIndustryClassificationView data";
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


        [Route("deliveryMethod/getall")]
        [HttpPost]
        public HttpResponseMessage GetDeliveryMethod(BaseViewModel model)
        {
            try
            {
                var data = service.GetDeliveryMethod(model);
                return Request.CreateResponse(HttpStatusCode.OK, data); ;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


    }
}