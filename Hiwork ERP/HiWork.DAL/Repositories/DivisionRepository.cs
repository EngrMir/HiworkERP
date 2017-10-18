

/* ******************************************************************************************************************
 * Repository for Master_Division Entity
 * Programmed by    :   Ashis Kr. Das (b-Bd_12 Ashis)
 * Date             :   04-Jun-2017
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
    public interface IDivisionRepository : IBaseRepository<Master_Division>
    {
        List<Master_Division>                      GetDivisionList();
        Master_Division                            InsertDivision(Master_Division recordData);
        Master_Division                            UpdateDivision(Master_Division recordData);
        bool                                        DeleteDivision(Guid ID);
    }
    public class DivisionRepository : BaseRepository<Master_Division, CentralDBEntities>, IDivisionRepository,IDisposable
    {

        private CentralDBEntities _dbContext;
        public DivisionRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }
        
        public List<Master_Division> GetDivisionList()
        {
            return _dbContext.Master_Division.Where(d=>d.IsDeleted==false).ToList();
        }

        public Master_Division InsertDivision(Master_Division recordData)
        {
            Master_Division result;
            result = _dbContext.Master_Division.Add(recordData);
            _dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return result;
        }

        public Master_Division UpdateDivision(Master_Division recordData)
        {
            var entry = _dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return recordData;
        }

        public bool DeleteDivision(Guid ID)
        {
            bool result;
            Master_Division targetType;
            List<Master_Division> dataList;

            dataList = _dbContext.Master_Division.ToList();
            targetType = dataList.Find(item => item.ID == ID);

            if (targetType != null)
            {
                _dbContext.Master_Division.Remove(targetType);
                _dbContext.SaveChanges();
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
