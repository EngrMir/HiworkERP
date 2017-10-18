

/* ******************************************************************************************************************
 * Repository for Master_Unit Entity
 * Date             :   02-July-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HiWork.DAL.Repositories
{
    public partial interface IUnitRepository : IBaseRepository<Master_Unit>
    {
        List<Master_Unit> GetUnitList();
        Master_Unit InsertUnit(Master_Unit tlp);
        Master_Unit UpdateUnit(Master_Unit tlp);
        bool DeleteUnit(long tlpId);
    }
    public class UnitRepository : BaseRepository<Master_Unit, CentralDBEntities>, IUnitRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public UnitRepository(IUnitOfWork uwork) : base(uwork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_Unit> GetUnitList()
        {
            return this._dbContext.Master_Unit.Where(u=>u.IsDeleted==false).ToList();
        }

        public Master_Unit InsertUnit(Master_Unit tlp)
        {
            var result = _dbContext.Master_Unit.Add(tlp);
            _dbContext.SaveChanges();
            return result;
        }

        public Master_Unit UpdateUnit(Master_Unit mu)
        {
            _dbContext.Entry(mu).State =EntityState.Modified;
            _dbContext.SaveChanges();
            return mu;

        }

        public bool DeleteUnit(long tlpId)
        {

            var Msm = _dbContext.Master_Unit.ToList().Find(m => m.ID == tlpId);
            if (Msm != null)
            {
                _dbContext.Master_Unit.Remove(Msm);
                _dbContext.SaveChanges();

                return true;
            }

            return false;
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
