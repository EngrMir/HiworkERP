using HiWork.DAL.Database;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public interface ICompanyRepository: IBaseRepository<Company>
    {
        List<CompanyIndustryClassification> GetCompanyIndustryClassificationList();
        CompanyIndustryClassification InsertCompanyIndustryClassification(CompanyIndustryClassification recordData);
        CompanyIndustryClassification UpdateCompanyIndustryClassification(CompanyIndustryClassification recordData);
        bool DeleteCompanyIndustryClassification(Guid cdID);
        Company SaveCompany(Company model);
        Company UpdateCompany(Company model);
        List<Company> GetAllCompany(BaseViewModel model);
        List<Company_Department> GetDepartmentListByCompanyID(Guid cdID);
        Company_Department InsertCompanyDepartment(Company_Department Companydept);
        Company_Department UpdateCompanyDepartment(Company_Department Companydept);
        Company_AgencyPrice SaveAgenciesPrice(Company_AgencyPrice model);
         Company_AgencyPrice UpdateAgenciesPrice(Company_AgencyPrice model);
        long GetCompanyNextRegistrationID(BaseViewModel model);
        Company GetCustomerByUser(string email, string password);
        Company_TransproPartner InsertCompanyTransproPartner(Company_TransproPartner model);
        Company GetCustomerByRegisterID(long id);
        Company_TransproPartner SaveUpdateCompanyTranspro(Company_TransproPartner model);

        List<Master_DeliveryMethod> GetDeliveryMethod();
    }

    public class CompanyRepository : BaseRepository<Company, CentralDBEntities>, ICompanyRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public CompanyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public Company SaveCompany(Company model)
        {
            try
            {
                _dbContext.Companies.Add(model);
                _dbContext.SaveChanges();
                return model;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Company_TransproPartner SaveUpdateCompanyTranspro(Company_TransproPartner model)
        {


                if (model.ID == Guid.Empty)
                {
                     model.ID = Guid.NewGuid();
                    _dbContext.Company_TransproPartner.Add(model);
                    _dbContext.SaveChanges();
                }
                else
                {
                    _dbContext.Entry(model).State = EntityState.Modified;
                    _dbContext.SaveChanges();


                }

            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Trace.TraceInformation(
            //                  "Class: {0}, Property: {1}, Error: {2}",
            //                  validationErrors.Entry.Entity.GetType().FullName,
            //                  validationError.PropertyName,
            //                  validationError.ErrorMessage);
            //        }
            //    }
            //}

            
           

            return model;
        }


       public Company UpdateCompany(Company model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return model;
           
        }
        public List<Company> GetAllCompany(BaseViewModel model)
        {
              return _dbContext.Companies.ToList();            
       }
        public Company GetCustomerByUser(string email,string password)
        {
            var data = from d in _dbContext.Companies
                       where (d.ClientID != null) 
                       select d;
            return data.ToList().Find(f=> (f.ClientID.Trim() == email.Trim() && f.Password == Utility.MD5(password)));
        }
        public Company GetCustomerByRegisterID(long id)
        {
            var data = (from d in _dbContext.Companies
                       where (d.RegistrationID == id)
                       select d).FirstOrDefault();
            return data;
        }
        public long GetCompanyNextRegistrationID(BaseViewModel model)
        {    
          return _dbContext.Companies.ToList().Where(c => c.ApplicationId == model.ApplicationId).LastOrDefault().RegistrationID;
        }

        public List<CompanyIndustryClassification> GetCompanyIndustryClassificationList()
        {
            return _dbContext.CompanyIndustryClassifications.ToList();
        }

        public CompanyIndustryClassification InsertCompanyIndustryClassification(CompanyIndustryClassification recordData)
        {
            CompanyIndustryClassification result;
            result = this._dbContext.CompanyIndustryClassifications.Add(recordData);
            this._dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return result;
        }

        public CompanyIndustryClassification UpdateCompanyIndustryClassification(CompanyIndustryClassification recordData)
        {
            var entry = this._dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return recordData;
        }

        public CompanyIndustryClassification GetCompanyIndustryClassification(Guid ID)
        {
            return this._dbContext.CompanyIndustryClassifications.FirstOrDefault(C => C.ID == ID);
        }

        public bool DeleteCompanyIndustryClassification(Guid ID)
        {
            bool result;
            CompanyIndustryClassification data;
            List<CompanyIndustryClassification> dataList;

            dataList = this._dbContext.CompanyIndustryClassifications.ToList();
            data = dataList.Find(item => item.ID == ID);

            if (data != null)
            {
                this._dbContext.CompanyIndustryClassifications.Remove(data);
                this._dbContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public Company_Department InsertCompanyDepartment(Company_Department Companydept)
        {
            try
            {
                Companydept.ID = Guid.NewGuid();
                var result = _dbContext.Company_Department.Add(Companydept);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Company_Department UpdateCompanyDepartment(Company_Department Companydept)
        {
            try
            {
                _dbContext.Entry(Companydept).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Companydept;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public Company_Department GetDepartmentByCompanyID(Guid cdID)
        {
            return _dbContext.Company_Department.FirstOrDefault(d => d.CompanyID ==cdID );

        }

        public List<Company_Department> GetDepartmentListByCompanyID(Guid cdID)
        {
            return _dbContext.Company_Department.Where(d => d.CompanyID == cdID).ToList(); ;

        }

        public Company_AgencyPrice SaveAgenciesPrice(Company_AgencyPrice model)
        {
            model.ID = Guid.NewGuid();
            var result = _dbContext.Company_AgencyPrice.Add(model);
            _dbContext.SaveChanges();

            return result;

        }

        public Company_AgencyPrice UpdateAgenciesPrice(Company_AgencyPrice model)
        {
             _dbContext.Entry(model).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return model;
        }

        public Company_TransproPartner InsertCompanyTransproPartner(Company_TransproPartner model)
        {
            if(model.ID==Guid.Empty)
            {
                 model.ID = Guid.NewGuid();
                _dbContext.Company_TransproPartner.Add(model);
                _dbContext.SaveChanges();
            }
            else
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        
            return model;
        }

       public List<Master_DeliveryMethod> GetDeliveryMethod()
        {
            return _dbContext.Master_DeliveryMethod.Where(d => d.IsDeleted == false).ToList();
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //Garbase collector
        }

      
    }
}
