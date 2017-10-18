

/* ******************************************************************************************************************
 * Repository for Master_CompanyTradingDivision Entity
 * Date             :   04-July-2017
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
    public interface ICompanyTradingDivisionRepository : IBaseRepository<Master_CompanyTradingDivision>
    {
        List<Master_CompanyTradingDivision> GetCompanyTradingDivisionList();
        Master_CompanyTradingDivision InsertCompanyTradingDivision(Master_CompanyTradingDivision recordData);
        Master_CompanyTradingDivision UpdateCompanyTradingDivision(Master_CompanyTradingDivision recordData);
        bool DeleteCompanyTradingDivision(Guid ID);
    }
    public class CompanyTradingDivisionRepository : BaseRepository<Master_CompanyTradingDivision, CentralDBEntities>,
                                                        ICompanyTradingDivisionRepository, IDisposable
    {
        private CentralDBEntities _dbcontext;
        public CompanyTradingDivisionRepository(IUnitOfWork uwork) : base(uwork)
        {
            this._dbcontext = new CentralDBEntities();
        }

        public List<Master_CompanyTradingDivision> GetCompanyTradingDivisionList()
        {
            return this._dbcontext.Master_CompanyTradingDivision.Where(c=>c.IsDeleted==false).ToList();
        }

        public Master_CompanyTradingDivision InsertCompanyTradingDivision(Master_CompanyTradingDivision recordData)
        {
            Master_CompanyTradingDivision result;
            result = this._dbcontext.Master_CompanyTradingDivision.Add(recordData);
            this._dbcontext.SaveChanges();
            return result;
        }

        public Master_CompanyTradingDivision UpdateCompanyTradingDivision(Master_CompanyTradingDivision recordData)
        {
            var entry = this._dbcontext.Entry(recordData);
            entry.State = EntityState.Modified;
            this._dbcontext.SaveChanges();
            return recordData;
        }

        public bool DeleteCompanyTradingDivision(Guid ID)
        {
            bool result;
            Master_CompanyTradingDivision data;
            List<Master_CompanyTradingDivision> datalist;

            datalist = this._dbcontext.Master_CompanyTradingDivision.ToList();
            data = datalist.Find(item => item.ID == ID);

            if (data != null)
            {
                this._dbcontext.Master_CompanyTradingDivision.Remove(data);
                this._dbcontext.SaveChanges();
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
            if (this._dbcontext == null)
                return;
            this._dbcontext.Dispose();
            this._dbcontext = null;
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
