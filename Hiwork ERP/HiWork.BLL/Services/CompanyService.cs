using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
//using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace HiWork.BLL.Services
{
    public interface ICompanyService : IBaseService<CompanyModel, Company>
    {
        CompanyModel SaveCompany(CompanyModel model);
        List<CompanyModel> GetAllCompanyList(BaseViewModel model);
        List<CompanyViewModel> GetCompanyViewList(BaseViewModel model);
        CompanyModel GetCompanyByID(BaseViewModel model);
        string GetNextCompanyNumber(BaseViewModel model);
        List<CompanyIndustryClassificationViewModel> GetCompanyIndustryClassificationViewList(BaseViewModel model);
        List<CompanyIndustryClassificationViewModel> SaveCompanyIndustryClassificationView(CompanyIndustryClassificationViewModel model);
        bool SaveCompanyIndustryClassificationViewList(List<CompanyIndustryClassificationViewModel> dataList);
        List<CompanyIndustryClassificationViewModel> DeleteCompanyIndustryClassificationView(CompanyIndustryClassificationViewModel model);
        CompanyDepartmentModel SaveCompanyDepartment(CompanyDepartmentModel model);
        List<CompanyDepartmentModel> GetDepartmentListByCompanyID(Guid ID);
        bool InsertIndustryClassifications(List<CompanyIndustryClassificationViewModel> dataList);
        CompanyViewModel GetCustomerByUser(string email, string password, string culture,bool IsPartner);
        CompanyModel GetCustomerByID(BaseViewModel model);
        CompanyModel GetCustomerByEmail(string email);
        CompanyModel GetCustomerByRegisterID(long id);
        CompanyTransproPartner SaveCompanyTranspro(CompanyTransproPartner model);
        List<CompanyModel> GetEstimationRequestedCompany(BaseViewModel model, Guid estimationID);
        bool SendRequest(CommonModelHelper model);

        List<DeliveryMethodModel> GetDeliveryMethod(BaseViewModel model);
    }


    public class CompanyService : BaseService<CompanyModel, Company>, ICompanyService
    {
        public ICompanyRepository companyRepository;
        private readonly ISqlConnectionService _sqlConnService;
        private CentralDBEntities _dbContext;
        public CompanyService(ICompanyRepository _companyRepository) : base(_companyRepository)
        {
            companyRepository = _companyRepository;
            _sqlConnService = new SqlConnectionService();
            _dbContext = new CentralDBEntities();
        }

        public List<CompanyModel> GetAllCompanyList(BaseViewModel model)
        {
            List<CompanyModel> companyList = new List<CompanyModel>();
            List<Company> dbCompanyList = new List<Company>();

            try
            {
                dbCompanyList= companyRepository.GetAllCompany(model);
                companyList = Mapper.Map<List<Company>, List<CompanyModel>>(dbCompanyList);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Company", message);
                throw new Exception(ex.Message);
            }

            return companyList;
        }

        public string ProcessTemplate(CommonModelHelper model, Company comapny, Estimation estimation)
        {
            var html = System.IO.File.ReadAllText(HostingEnvironment.MapPath($"~/EmailTemplate/EstimationRequest_{model.Culture}.html"));
            var match = Regex.Matches(html, @"\[([A-Za-z0-9\-._]+)]", RegexOptions.IgnoreCase);
            foreach (var v in match)
            {
                var result = string.Empty;
                var originalString = v.ToString();
                var x = v.ToString();
                x = x.Replace("[", "");
                x = x.Replace("]", "");
                if (x.Contains("Company"))
                {
                    var prop = x.Split('.')[1];
                    result = comapny.GetType().GetProperty(prop).GetValue(comapny, null)?.ToString();
                }
                else
                {
                    x = x.Contains("_") ? x + model.Culture : x;
                    result = estimation.GetType().GetProperty(x).GetValue(estimation, null)?.ToString() ?? "";
                }
                html = html.Replace(originalString, result.ToString());
            }
            return html;
        }

        public bool SendRequest(CommonModelHelper model)
        {
            var flag = true;
            var items = new List<Company>();
            var emailService = new EmailService();
            try
            {
                var estimation = _dbContext.Estimations.Find(model.Estimation.ID);
                var histories = _dbContext.EstimationRequestHistories.Where(h => h.EstimationID == model.Estimation.ID).ToList();
                var hids = histories.Select(h => h.Company.ID).ToList();
                var checkedIds = model.CompanyModels.Select(h => h.ID).ToList();
                histories.ForEach(h =>
                {
                    if (!checkedIds.Contains(h.Company.ID))
                    {
                        _dbContext.EstimationRequestHistories.Remove(h);
                    }
                });
                model.CompanyModels.ForEach(cm =>
                {
                    var isExist = _dbContext.EstimationRequestHistories.Any(h => h.Company.ID == cm.ID);
                    var company = _dbContext.Companies.Find(cm.ID);
                    if (!isExist)
                    {
                        var obj = new EstimationRequestHistory
                        {
                            ID = Guid.NewGuid(),
                            EstimationID = model.Estimation.ID,
                            CompanyID = cm.ID,
                            Company = company,
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.CurrentUserID
                        };
                        _dbContext.EstimationRequestHistories.Add(obj);
                    }
                    if (!string.IsNullOrEmpty(company.Email))
                    {
                        items.Add(company);
                    }
                });
                _dbContext.SaveChanges();
                items.ForEach(i =>
                {
                    var subject = $"Estimation Request";
                    emailService.SendEmail(i.Email, null,null, subject, ProcessTemplate(model, i, estimation), null, true);
                });
            }
            catch (Exception ex)
            {
                flag = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Company", message);
                throw new Exception(ex.Message);
            }
            return flag;
        }

        public List<CompanyModel> GetEstimationRequestedCompany(BaseViewModel model, Guid estimationID)
        {
            var list = new List<CompanyModel>();
            try
            {
                var ids = _dbContext.EstimationRequestHistories.Where(e => e.EstimationID == estimationID).Select(e => e.CompanyID);
                list = (from c in _dbContext.Companies
                        where c.IsActive == true && c.IsSubcontactual == true
                        select new CompanyModel
                        {
                            ID = c.ID,
                            Name = c.ClientID,
                            IsSelected = ids.Contains(c.ID) ? true : false
                        })
                        .OrderBy(x => x.Name)
                        //.Skip((model.PageIndex - 1) * model.PageSize)
                        //.Take(model.PageSize)
                        .ToList();
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Company", message);
                throw new Exception(ex.Message);
            }
            return list;
        }

        public CompanyViewModel GetCustomerByUser(string email, string password,string culture, bool IsPartner)
        {
            CompanyViewModel company = null;
            try
            {
                var data = companyRepository.GetCustomerByUser(email, password);
                if (IsPartner)
                    data = data.IsPartner == true ? data : null;
                var mapData = Mapper.Map<Company, CompanyModel>(data);
                if (mapData != null)
                {
                    mapData.Name = Utility.GetPropertyValue(mapData, "Name", culture) == null ? string.Empty :
                                                      Utility.GetPropertyValue(mapData, "Name", culture).ToString();
                    mapData.Address1 = Utility.GetPropertyValue(mapData, "Address1", culture) == null ? string.Empty :
                                        Utility.GetPropertyValue(mapData, "Address1", culture).ToString();
                    mapData.Address2 = Utility.GetPropertyValue(mapData, "Address2", culture) == null ? string.Empty :
                                                      Utility.GetPropertyValue(mapData, "Address2", culture).ToString();

                    company = new CompanyViewModel();
                    company.ID = mapData.ID;
                    company.Name = mapData.Name;
                    company.RegistrationID = mapData.RegistrationID;
                    company.RegistrationNo = mapData.RegistrationNo;
                    company.ClientID = mapData.ClientID;
                    company.WebSiteURL = mapData.WebSiteURL;
                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                //errorLog.SetErrorLog(, "Company", message);
                throw new Exception(ex.Message);
            }
            return company;
       }

        public List<CompanyViewModel> GetCompanyViewList(BaseViewModel model)
        {
 
            List<CompanyViewModel> companyViewList = new List<CompanyViewModel>();
            List<CompanyModel> tempdata = new List<CompanyModel>();
            try
            {
               
                tempdata = GetAllCompanyList(model);  
                var Locations = Utility.ClientLocationType;


                companyViewList = (from c in tempdata join 
                                   l in Locations on c.CompanyLocationID equals l.Id
                                   select new CompanyViewModel
                                   {
                                       ID=c.ID,
                                       Name=c.Name_Local,
                                       ClientID=c.ClientID,
                                       Capital=c.Capital,
                                       ClientNo=c.ClientNo,
                                       EstablishedDate=c.EstablishedDate,
                                       RegistrationID=c.RegistrationID,
                                       RegistrationNo=c.RegistrationNo,
                                       WebSiteURL=c.WebSiteURL,
                                       ClientLocation=l.Name
                                   }).ToList();

            }
       
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Company", message);
                throw new Exception(ex.Message);
            }
            return companyViewList;
        }

        public CompanyModel GetCompanyByID(BaseViewModel model)
        {
            CompanyModel company = new CompanyModel();

            CompanyAgencyPriceModel agency = new CompanyAgencyPriceModel();
            List<CompanyAgencyPriceModel> agencyList = new List<CompanyAgencyPriceModel>();
          //  CompanyTransproPartner transpro = new CompanyTransproPartner();
            try
            {
                var dbCompany = companyRepository.GetById(model.ID);
                 company = Mapper.Map<Company, CompanyModel>(dbCompany);

                 company.transpro = Mapper.Map<Company_TransproPartner, CompanyTransproPartner>(dbCompany.Company_TransproPartner.FirstOrDefault());

                company.transpro.Name= Utility.GetPropertyValue(company.transpro, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                              Utility.GetPropertyValue(company.transpro, "Name", model.CurrentCulture).ToString();

                company.transpro.Address = Utility.GetPropertyValue(company.transpro, "Address", model.CurrentCulture) == null ? string.Empty :
                                                                              Utility.GetPropertyValue(company.transpro, "Address", model.CurrentCulture).ToString();

                company.transpro.CompanyName = Utility.GetPropertyValue(company.transpro, "CompanyName", model.CurrentCulture) == null ? string.Empty :
                                                                             Utility.GetPropertyValue(company.transpro, "CompanyName", model.CurrentCulture).ToString();

                company.transpro.CEOName = Utility.GetPropertyValue(company.transpro, "CEOName", model.CurrentCulture) == null ? string.Empty :
                                                                              Utility.GetPropertyValue(company.transpro, "CEOName", model.CurrentCulture).ToString();

                company.transpro.InvoiceCompanyName = Utility.GetPropertyValue(company.transpro, "InvoiceCompanyName", model.CurrentCulture) == null ? string.Empty :
                                                                          Utility.GetPropertyValue(company.transpro, "InvoiceCompanyName", model.CurrentCulture).ToString();
                company.transpro.InvoiceAddress1 = Utility.GetPropertyValue(company.transpro, "InvoiceAddress1", model.CurrentCulture) == null ? string.Empty :
                                                                         Utility.GetPropertyValue(company.transpro, "InvoiceAddress1", model.CurrentCulture).ToString();

                company.transpro.InvoiceAddress2 = Utility.GetPropertyValue(company.transpro, "InvoiceAddress2", model.CurrentCulture) == null ? string.Empty :
                                                                         Utility.GetPropertyValue(company.transpro, "InvoiceAddress2", model.CurrentCulture).ToString();

                company.transpro.InchagreName = Utility.GetPropertyValue(company.transpro, "InchagreName", model.CurrentCulture) == null ? string.Empty :
                                                                        Utility.GetPropertyValue(company.transpro, "InchagreName", model.CurrentCulture).ToString();

                company.Name = Utility.GetPropertyValue(dbCompany, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                              Utility.GetPropertyValue(dbCompany, "Name", model.CurrentCulture).ToString();
                    company.Address1 = Utility.GetPropertyValue(dbCompany, "Address1", model.CurrentCulture) == null ? string.Empty :
                                                                             Utility.GetPropertyValue(dbCompany, "Address1", model.CurrentCulture).ToString();
                    company.Address2 = Utility.GetPropertyValue(dbCompany, "Address2", model.CurrentCulture) == null ? string.Empty :
                                                                          Utility.GetPropertyValue(dbCompany, "Address2", model.CurrentCulture).ToString();
                    company.Comment = Utility.GetPropertyValue(dbCompany, "Comment", model.CurrentCulture) == null ? string.Empty :
                                                                   Utility.GetPropertyValue(dbCompany, "Comment", model.CurrentCulture).ToString();
                    company.Note = Utility.GetPropertyValue(dbCompany, "Note", model.CurrentCulture) == null ? string.Empty :
                                                                               Utility.GetPropertyValue(dbCompany, "Note", model.CurrentCulture).ToString();
           
                    var AgencyPriceList = dbCompany.Company_AgencyPrice.ToList();


                if (AgencyPriceList.Count() > 0 )
                {

                    AgencyPriceList.ForEach(a =>
                    {
                        agency = Mapper.Map<Company_AgencyPrice, CompanyAgencyPriceModel>(a);

                        agency.DestinationLanguageName = Utility.GetPropertyValue(a.Master_Language, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(a.Master_Language, "Name", model.CurrentCulture).ToString();

                        agency.SourceLanguageName = Utility.GetPropertyValue(a.Master_Language1, "Name", model.CurrentCulture) == null ? string.Empty :
                                                            Utility.GetPropertyValue(a.Master_Language1, "Name", model.CurrentCulture).ToString();

                        agency.EstimationTypeName = Utility.GetPropertyValue(a.Master_EstimationType, "Name", model.CurrentCulture) == null ? string.Empty :
                                                            Utility.GetPropertyValue(a.Master_EstimationType, "Name", model.CurrentCulture).ToString();

                        agency.SpecializedFieldName= Utility.GetPropertyValue(a.Master_EstimationSpecializedField, "Name", model.CurrentCulture) == null ? string.Empty :
                                                            Utility.GetPropertyValue(a.Master_EstimationSpecializedField, "Name", model.CurrentCulture).ToString();

                        agency.PriceCalculationOnName = Utility.GetPropertyValue(Utility.PriceCalculateTypeList.Where(e => e.Id == a.PriceCalculationOnID).SingleOrDefault(), "Name", model.CurrentCulture) == null ? string.Empty :
                                                            Utility.GetPropertyValue(Utility.PriceCalculateTypeList.Where(e => e.Id == a.PriceCalculationOnID).SingleOrDefault(), "Name", model.CurrentCulture).ToString();
                                                      
                        agencyList.Add(agency);
                    });
                }


                company.AgencyPrice = agencyList;
                
            }
            catch(Exception ex)
            {

            }
            return company;
        }
        public CompanyModel GetCustomerByID(BaseViewModel model)
        {
            CompanyModel company = new CompanyModel();

            CompanyAgencyPriceModel agency = new CompanyAgencyPriceModel();
            List<CompanyAgencyPriceModel> agencyList = new List<CompanyAgencyPriceModel>();
            try
            {
                var dbCompany = companyRepository.GetById(model.ID);
                company = Mapper.Map<Company, CompanyModel>(dbCompany);
                //company.EmployeeMemberName= Utility.GetPropertyValue(dbCompany.Employee, "Name", model.CurrentCulture) == null ? string.Empty :
                //                                             Utility.GetPropertyValue(dbCompany.Employee, "Name", model.CurrentCulture).ToString();
                company.Name = Utility.GetPropertyValue(dbCompany, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                          Utility.GetPropertyValue(dbCompany, "Name", model.CurrentCulture).ToString();
                company.Address1 = Utility.GetPropertyValue(dbCompany, "Address1", model.CurrentCulture) == null ? string.Empty :
                                                                         Utility.GetPropertyValue(dbCompany, "Address1", model.CurrentCulture).ToString();
                company.Address2 = Utility.GetPropertyValue(dbCompany, "Address2", model.CurrentCulture) == null ? string.Empty :
                                                                      Utility.GetPropertyValue(dbCompany, "Address2", model.CurrentCulture).ToString();
                company.Comment = Utility.GetPropertyValue(dbCompany, "Comment", model.CurrentCulture) == null ? string.Empty :
                                                               Utility.GetPropertyValue(dbCompany, "Comment", model.CurrentCulture).ToString();
                company.Note = Utility.GetPropertyValue(dbCompany, "Note", model.CurrentCulture) == null ? string.Empty :
                                                                            Utility.GetPropertyValue(dbCompany, "Note", model.CurrentCulture).ToString();
                if (dbCompany.CompanyIndustryClassifications != null && dbCompany.CompanyIndustryClassifications.Count > 0)
                {
                    company.IndustryClassifications = Mapper.Map<List<CompanyIndustryClassification>, List<CompanyIndustryClassificationViewModel>>(dbCompany.CompanyIndustryClassifications.ToList());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return company;
        }
        public CompanyModel GetCustomerByEmail(string email)
        {
            CompanyModel company = new CompanyModel();
            try
            {
                var dbCompany = companyRepository.GetList().Where(f => f.ClientID.Trim() == email.Trim()).FirstOrDefault();
                company = Mapper.Map<Company, CompanyModel>(dbCompany);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return company;
        }

        public List<CompanyDepartmentModel> GetDepartmentListByCompanyID(Guid ID)
        {
            List<CompanyDepartmentModel> companydept = new List<CompanyDepartmentModel>();
            try
            {
                var dbdept = companyRepository.GetDepartmentListByCompanyID(ID);
                companydept = Mapper.Map<List<Company_Department>, List<CompanyDepartmentModel>>(dbdept);
            }
            catch(Exception ex)
            {

            }
            return companydept;
        }

        public string GetNextCompanyNumber(BaseViewModel model)
        {
            string CompanyNo = string.Empty;
            try
            {
                IUnitOfWork unitWork = new UnitOfWork();
                IApplicationService appService = new ApplicationService(new ApplicationRepository(unitWork));

                long? NextID = companyRepository.GetCompanyNextRegistrationID(model);
                NextID = NextID==null ? 0 : NextID;
                NextID = NextID + 1;
                CompanyNo = Helper.GenerateUniqueID(appService.GetApplicationCode(model.ApplicationId), NextID.ToString());
            }
            catch(Exception ex)
            {

            }
     
            return CompanyNo;
        }

       
        public CompanyModel SaveCompany(CompanyModel model)
        {
            CompanyModel company = new CompanyModel();
            Company dbCompany = new Company();
            try
            {

                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var map = Mapper.Map<CompanyModel, Company>(model);

                if (model.ID==Guid.Empty)
                {
                    map.ID = Guid.NewGuid();
                    dbCompany = companyRepository.SaveCompany(map);
                    model.ID = map.ID;
                    CompanyTabSave(model);
                }
                else
                {
                    dbCompany = companyRepository.UpdateCompany(map);
                    CompanyTabSave(model);
                }
                company = Mapper.Map<Company, CompanyModel>(dbCompany);
              
                /*
                _sqlConnService.OpenConnection();
                SqlCommand cmd = new SqlCommand("SP_SaveDeleteCompany", _sqlConnService.CreateConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DivisionID", model.DivisionID);
                cmd.Parameters.AddWithValue("@CorporateDivisionID", model.TradingDivisionID); 
                cmd.Parameters.AddWithValue("@OriginCountryID", model.OriginCountryID);
                cmd.Parameters.AddWithValue("@ApplicationID", model.ApplicationID);
                cmd.Parameters.AddWithValue("@IsCompanyAgent", model.IsCompanyAgent);
                cmd.Parameters.AddWithValue("@CompanyNo", model.CompanyNo);
                cmd.Parameters.AddWithValue("@Name_" + model.CurrentCulture, model.Name);
                cmd.Parameters.AddWithValue("@CorporateNo", model.CorporateNo);
                cmd.Parameters.AddWithValue("@WebSiteURL", model.WebSiteURL);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Email_CC", model.Email_CC);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@RepresentativeDirector_" + model.CurrentCulture, model.RepresentativeDirector);
                cmd.Parameters.AddWithValue("@Phone", model.Phone);
                cmd.Parameters.AddWithValue("@Fax", model.Fax);
                cmd.Parameters.AddWithValue("@PostalCode", model.PostalCode);
                cmd.Parameters.AddWithValue("@Address" + model.CurrentCulture, model.Address);
                cmd.Parameters.AddWithValue("@Captital", model.Captital);
                cmd.Parameters.AddWithValue("@EstablishmentDate", model.EstablishmentDate);
                cmd.Parameters.AddWithValue("@MainPhoto", model.MainPhoto);
                cmd.Parameters.AddWithValue("@Logo", model.Logo);
                cmd.Parameters.AddWithValue("@IsBillingMonthly", model.IsBillingMonthly);
                cmd.Parameters.AddWithValue("@EmployeeName", model.EmployeeName);
                cmd.Parameters.AddWithValue("@BranchName_" + model.CurrentCulture, model.BranchName);
                cmd.Parameters.AddWithValue("@TradingNote" + model.CurrentCulture, model.TradingNote);
                cmd.Parameters.AddWithValue("@TradingRecord" + model.CurrentCulture, model.TradingRecord);
                cmd.Parameters.AddWithValue("@BankID" + model.CurrentCulture, model.BankID);
                cmd.Parameters.AddWithValue("@BankBranchID", model.BankBranchID);
                cmd.Parameters.AddWithValue("@BankAccountTypeID", model.BankAccountTypeID);
                cmd.Parameters.AddWithValue("@PrivateCompanyName", model.PrivateCompanyName);
                cmd.Parameters.AddWithValue("@PersonInCharge" + model.CurrentCulture, model.PersonInCharge);
                cmd.Parameters.AddWithValue("@IsJapneseOffice", model.IsJapneseOffice);
                cmd.Parameters.AddWithValue("@HomeOfficeAddressID", model.HomeOfficeAddressID);
                cmd.Parameters.AddWithValue("@ForeignCountryID", model.ForeignCountryID);
                cmd.Parameters.AddWithValue("@IsAddressPrivate", model.IsAddressPrivate);
                cmd.Parameters.AddWithValue("@PrivateName", model.PrivateName);
                cmd.Parameters.AddWithValue("@ProvinceOfOverseas" + model.CurrentCulture, model.PrivateName);
                cmd.Parameters.AddWithValue("@ClosingDate", model.ClosingDate);
                cmd.Parameters.AddWithValue("@BuildingName", model.BuildingName);
                cmd.Parameters.AddWithValue("@BillingCompanyName_" + model.CurrentCulture, model.BillingCompanyName);
                cmd.Parameters.AddWithValue("@BillingDivisionName", model.BillingDivisionName);
                cmd.Parameters.AddWithValue("@BillingPostalCode", model.BillingPostalCode);
                cmd.Parameters.AddWithValue("@BillingAddress_" + model.CurrentCulture, model.BillingAddress);
                cmd.Parameters.AddWithValue("@BillingPersonInCharge_" + model.CurrentCulture, model.BillingPersonInCharge);
                cmd.Parameters.AddWithValue("@BillingBankID", model.BillingBankID);
                cmd.Parameters.AddWithValue("@BillingBankBranchID", model.BillingBankBranchID);
                cmd.Parameters.AddWithValue("@BillingBankAccountTypeID", model.BillingBankAccountTypeID);
                cmd.Parameters.AddWithValue("@BillingBankAccountNo", model.BillingBankAccountNo);
                cmd.Parameters.AddWithValue("@BillingBankAccountHolder_" + model.CurrentCulture, model.BillingBankAccountHolder);
                cmd.Parameters.AddWithValue("@IsWebsitePrivate", model.IsWebsitePrivate);
                cmd.Parameters.AddWithValue("@SalesMan", model.SalesMan);
                cmd.Parameters.AddWithValue("@DateOfPayment", model.DateOfPayment);
                cmd.Parameters.AddWithValue("@Coordinate", model.Coordinate);
                cmd.Parameters.AddWithValue("@InHousePersonID", model.InHousePersonID);
                cmd.Parameters.AddWithValue("@SalesScale", model.SalesScale);
                cmd.Parameters.AddWithValue("@IndustryClassification", model.IndustryClassification);
                cmd.Parameters.AddWithValue("@TradingDivision", model.TradingDivision);
                cmd.Parameters.AddWithValue("@SalesVisitDate", model.SalesVisitDate);
                cmd.Parameters.AddWithValue("@IsInteractByVisit", model.IsInteractByVisit);
                cmd.Parameters.AddWithValue("@IsInteractByPhone", model.IsInteractByPhone);
                cmd.Parameters.AddWithValue("@HasThePersonInCharge", model.HasThePersonInCharge);
                cmd.Parameters.AddWithValue("@InteractWithCompanyDiscription", model.InteractWithCompanyDiscription);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedBy", model.CurrentUserID);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CultureId", model.CurrentCulture);

                if (model.ID==Guid.Empty)
                {              
                    cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@StatementType", "Insert");
                   
                }
                else
                {              
                    cmd.Parameters.AddWithValue("@ID", model.ID);                    
                    cmd.Parameters.AddWithValue("@StatementType", "Update");                
                }
                cmd.ExecuteNonQuery();
                */


            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            return company;
        }


        public CompanyTransproPartner SaveCompanyTranspro(CompanyTransproPartner model)
        {
            CompanyTransproPartner companytrnaspro = new CompanyTransproPartner();
            Company_TransproPartner dbCompanyTranspro = new Company_TransproPartner();
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var map = Mapper.Map<CompanyTransproPartner, Company_TransproPartner>(model);
                dbCompanyTranspro = companyRepository.SaveUpdateCompanyTranspro(map);

                companytrnaspro = Mapper.Map<Company_TransproPartner, CompanyTransproPartner>(dbCompanyTranspro);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyTranspro", message);
                throw new Exception(ex.Message);
            }

            return companytrnaspro;
        }

        private void CompanyTabSave(CompanyModel model)
        {
          
            if (model.TabId == 1)
            {
                CompanyDepartmentModel companyDepartment = new CompanyDepartmentModel();
                 companyDepartment = model.Dept;
                Utility.SetDynamicPropertyValue(companyDepartment, model.CurrentCulture);
                var dept = Mapper.Map<CompanyDepartmentModel, Company_Department>(companyDepartment);
                dept.CompanyID = model.ID;
                var comdept = companyRepository.InsertCompanyDepartment(dept);
            }
            else if (model.TabId == 4)
            {
                var AgencyPriceList = model.AgencyPrice;
                var AgencypriceMapList = Mapper.Map<List<CompanyAgencyPriceModel>, List<Company_AgencyPrice>>(AgencyPriceList);
                foreach (var item in AgencypriceMapList)
                {
                    item.CompanyID = model.ID;

                    if (item.ID == Guid.Empty)
                    {

                        companyRepository.SaveAgenciesPrice(item);
                    }
                    else
                    {
                        companyRepository.UpdateAgenciesPrice(item);
                    }
                }
            }
            else if(model.TabId==6)
            {
                Utility.SetDynamicPropertyValue(model.transpro, model.CurrentCulture);
                var TransproPartner = Mapper.Map<CompanyTransproPartner, Company_TransproPartner>(model.transpro);
                companyRepository.InsertCompanyTransproPartner(TransproPartner);

            }
        }

        public List<CompanyIndustryClassificationViewModel> GetCompanyIndustryClassificationViewList(BaseViewModel dataModel)
        {
            CompanyIndustryClassificationViewModel model;
            List<CompanyIndustryClassification> datalist;
            List<CompanyIndustryClassificationViewModel> modlist = new List<CompanyIndustryClassificationViewModel>();
            CompanyIndustryClassificationModel IndustryClassificationModel;
            CompanyIndustryClassificationItemModel IndustryClassificationItemModel;
            object pvalue;

            try
            {
                datalist = companyRepository.GetCompanyIndustryClassificationList();
                if (datalist != null)
                {
                    foreach (CompanyIndustryClassification data in datalist)
                    {
                        if (dataModel.ID != Guid.Empty && data.CompanyID != dataModel.ID)
                        {
                            continue;
                        }

                        model = Mapper.Map<CompanyIndustryClassification, CompanyIndustryClassificationViewModel>(data);
                        IndustryClassificationModel = Mapper.Map<Master_CompanyIndustryClassification, CompanyIndustryClassificationModel>(data.Master_CompanyIndustryClassification);
                        IndustryClassificationItemModel = Mapper.Map<Master_CompanyIndustryClassificationItem, CompanyIndustryClassificationItemModel>(data.Master_CompanyIndustryClassificationItem);
                        
                        pvalue = Utility.GetPropertyValue(IndustryClassificationModel, "Name", dataModel.CurrentCulture);
                        model.ClassificationName = pvalue == null ? string.Empty : pvalue.ToString();
                        pvalue = Utility.GetPropertyValue(IndustryClassificationItemModel, "Name", dataModel.CurrentCulture);
                        model.ClassificationItemName = pvalue == null ? string.Empty : pvalue.ToString();
                        model.CompanyName = data.Company.Name_Local;

                        model.CurrentCulture = dataModel.CurrentCulture;
                        model.CurrentUserID = dataModel.CurrentUserID;
                        model.ApplicationId = dataModel.ApplicationId;
                        modlist.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(dataModel.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            return modlist;
        }

        public List<CompanyIndustryClassificationViewModel> SaveCompanyIndustryClassificationView(CompanyIndustryClassificationViewModel dataModel)
        {
            CompanyIndustryClassification data;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                data = Mapper.Map<CompanyIndustryClassificationViewModel, CompanyIndustryClassification>(dataModel);

                if (dataModel.ID == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = dataModel.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    companyRepository.InsertCompanyIndustryClassification(data);
                }
                else
                {
                    data.UpdatedBy = dataModel.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    companyRepository.UpdateCompanyIndustryClassification(data);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(dataModel.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = dataModel.CurrentCulture;
            md.CurrentUserID = dataModel.CurrentUserID;
            return GetCompanyIndustryClassificationViewList(md);
        }

        public bool SaveCompanyIndustryClassificationViewList(List<CompanyIndustryClassificationViewModel> dataList)
        {
            CompanyIndustryClassification data;

            try
            {
                foreach (CompanyIndustryClassificationViewModel model in dataList)
                {
                    Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                    data = Mapper.Map<CompanyIndustryClassificationViewModel, CompanyIndustryClassification>(model);

                    if (model.ID == Guid.Empty)
                    {
                        data.ID = Guid.NewGuid();
                        data.CreatedBy = model.CurrentUserID;
                        data.CreatedDate = DateTime.Now;
                        companyRepository.InsertCompanyIndustryClassification(data);
                    }
                    else
                    {
                        data.UpdatedBy = model.CurrentUserID;
                        data.UpdatedDate = DateTime.Now;
                        companyRepository.UpdateCompanyIndustryClassification(data);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(dataList[0].CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            return true;
        }
        public bool InsertIndustryClassifications(List<CompanyIndustryClassificationViewModel> dataList)
        {
            CompanyIndustryClassification data;

            try
            {
                var indData = companyRepository.GetCompanyIndustryClassificationList().FindAll(f => f.CompanyID == dataList[0].CompanyID).ToList();
                if(indData != null && indData.ToList().Count > 0)
                {
                    indData.ForEach(model =>
                    {
                        companyRepository.DeleteCompanyIndustryClassification(model.ID);
                    });
                }
                dataList.ForEach(model =>
                {
                    data = Mapper.Map<CompanyIndustryClassificationViewModel, CompanyIndustryClassification>(model);
                    data.ID = Guid.NewGuid();
                    companyRepository.InsertCompanyIndustryClassification(data);
                });
              
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(dataList[0].CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }
            return true;
        }
        public List<CompanyIndustryClassificationViewModel> DeleteCompanyIndustryClassificationView(CompanyIndustryClassificationViewModel dataModel)
        {
            try
            {
                companyRepository.DeleteCompanyIndustryClassification(dataModel.ID);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(dataModel.CurrentUserID, "AgentBusiness", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = dataModel.CurrentCulture;
            md.CurrentUserID = dataModel.CurrentUserID;
            return GetCompanyIndustryClassificationViewList(md);
        }

        public CompanyDepartmentModel SaveCompanyDepartment(CompanyDepartmentModel model)
        {
            Company_Department data;

            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                data = Mapper.Map<CompanyDepartmentModel, Company_Department>(model);

                if (model.ID == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = model.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    companyRepository.InsertCompanyDepartment(data);
                }
                else
                {
                    data.UpdatedBy = model.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    companyRepository.UpdateCompanyDepartment(data);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyDept", message);
                throw new Exception(ex.Message);
            }
            return model;
        }
        public CompanyModel GetCustomerByRegisterID(long id)
        {
            CompanyModel _result = null;
            try
            {
                var data = companyRepository.GetCustomerByRegisterID(id);
                _result = Mapper.Map<Company,CompanyModel>(data);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _result;
        }

        public List<DeliveryMethodModel> GetDeliveryMethod(BaseViewModel model)
        {
            List<DeliveryMethodModel> result = new List<DeliveryMethodModel>();
            try
            {
                var data = companyRepository.GetDeliveryMethod();
                result = Mapper.Map<List<Master_DeliveryMethod>, List<DeliveryMethodModel>>(data);

                result.ForEach(a => {

                    a.Name = Utility.GetPropertyValue(a, "Name", model.CurrentCulture) == null ? string.Empty :
                                                      Utility.GetPropertyValue(a, "Name", model.CurrentCulture).ToString();

                });

             
            }
            catch(Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "delivertMethod in Company service", message);
                throw new Exception(ex.Message);

            }
            return result;
        }

    }
}
