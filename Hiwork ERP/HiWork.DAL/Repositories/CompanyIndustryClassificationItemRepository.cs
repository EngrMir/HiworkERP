

/* ******************************************************************************************************************
 * Repository for Master_CompanyIndustryClassificationItem Entity
 * Programmed by    :   Ashis Kr. Das (b-Bd_12 Ashis)
 * Date             :   29-Jun-2017
 * *****************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.DAL.Repositories
{
    public interface ICompanyIndustryClassificationItemRepository :
                            IBaseRepository<Master_CompanyIndustryClassificationItem>
    {
        List<Master_CompanyIndustryClassificationItem> GetCompanyIndustryClassificationItemList();
        Master_CompanyIndustryClassificationItem
                    InsertCompanyIndustryClassificationItem(Master_CompanyIndustryClassificationItem recordData);
        Master_CompanyIndustryClassificationItem
                    UpdateCompanyIndustryClassificationItem(Master_CompanyIndustryClassificationItem recordData);
        bool DeleteCompanyIndustryClassificationItem(Guid ID);
    }

    public class CompanyIndustryClassificationItemRepository :
                BaseRepository<Master_CompanyIndustryClassificationItem, CentralDBEntities>,
                ICompanyIndustryClassificationItemRepository,
                IDisposable
    {
        private CentralDBEntities _dbContext;
        public CompanyIndustryClassificationItemRepository(IUnitOfWork uwork) : base(uwork)
        {
            this._dbContext = new CentralDBEntities();
        }

        public List<Master_CompanyIndustryClassificationItem>
                        GetCompanyIndustryClassificationItemList()
        {
            return this._dbContext.Master_CompanyIndustryClassificationItem.Where(c=>c.IsDeleted==false).ToList();
        }

        public Master_CompanyIndustryClassificationItem
                    InsertCompanyIndustryClassificationItem(Master_CompanyIndustryClassificationItem recordData)
        {
            Master_CompanyIndustryClassificationItem result;
            result = this._dbContext.Master_CompanyIndustryClassificationItem.Add(recordData);
            this._dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return result;
        }

        public Master_CompanyIndustryClassificationItem
                    UpdateCompanyIndustryClassificationItem(Master_CompanyIndustryClassificationItem recordData)
        {
            var entry = this._dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return recordData;
        }

        public bool DeleteCompanyIndustryClassificationItem(Guid ID)
        {
            bool result;
            Master_CompanyIndustryClassificationItem data;
            List<Master_CompanyIndustryClassificationItem> datalist;

            datalist = this._dbContext.Master_CompanyIndustryClassificationItem.ToList();
            data = datalist.Find(item => item.ID == ID);
            if (data != null)
            {
                this._dbContext.Master_CompanyIndustryClassificationItem.Remove(data);
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
            GC.SuppressFinalize(this);
            this.Dispose(true);
            return;
        }
    }
}
