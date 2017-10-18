

/* ******************************************************************************************************************
 * Repository for Master_CompanyIndustryClassification Entity
 * Date             :   24-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.DAL.Repositories
{
    public interface ICompanyIndustryClassificationRepository : IBaseRepository<Master_CompanyIndustryClassification>
    {
        List<Master_CompanyIndustryClassification> GetCompanyIndustryClassificationList();
        Master_CompanyIndustryClassification InsertCompanyIndustryClassification(Master_CompanyIndustryClassification recordData);
        Master_CompanyIndustryClassification UpdateCompanyIndustryClassification(Master_CompanyIndustryClassification recordData);
        Master_CompanyIndustryClassification GetCompanyIndustryClassification(Guid ID);
        bool DeleteCompanyIndustryClassification(Guid ID);
    }

    public class CompanyIndustryClassificationRepository :
            BaseRepository<Master_CompanyIndustryClassification, CentralDBEntities>,
            ICompanyIndustryClassificationRepository,
            IDisposable
    {
        private CentralDBEntities _dbContext;
        public CompanyIndustryClassificationRepository(IUnitOfWork uWork) : base(uWork)
        {
            this._dbContext = new CentralDBEntities();
        }

        public List<Master_CompanyIndustryClassification>
                GetCompanyIndustryClassificationList()
        {
            return this._dbContext.Master_CompanyIndustryClassification.Where(c => c.IsDeleted == false).ToList();
        }

        public Master_CompanyIndustryClassification InsertCompanyIndustryClassification(Master_CompanyIndustryClassification recordData)
        {
            Master_CompanyIndustryClassification result;
            result = this._dbContext.Master_CompanyIndustryClassification.Add(recordData);
            this._dbContext.SaveChanges();
            return result;
        }

        public Master_CompanyIndustryClassification UpdateCompanyIndustryClassification(Master_CompanyIndustryClassification recordData)
        {
            var entry = this._dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            return recordData;
        }

        public Master_CompanyIndustryClassification GetCompanyIndustryClassification(Guid ID)
        {
            return this._dbContext.Master_CompanyIndustryClassification.FirstOrDefault(C => C.ID == ID);
        }

        public bool DeleteCompanyIndustryClassification(Guid ID)
        {
            bool result;
            Master_CompanyIndustryClassification data;
            List<Master_CompanyIndustryClassification> dataList;

            dataList = this._dbContext.Master_CompanyIndustryClassification.ToList();
            data = dataList.Find(item => item.ID == ID);

            if (data != null)
            {
                this._dbContext.Master_CompanyIndustryClassification.Remove(data);
                this._dbContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing == false)
                return;
            if (this._dbContext == null)
                return;
            this._dbContext.Dispose();
            this._dbContext = null;
            return;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}
