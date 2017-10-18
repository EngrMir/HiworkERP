

/* ******************************************************************************************************************
 * Repository for Master_Team Entity
 * Programmed by    :   Ashis Kr. Das (b-Bd_12 Ashis)
 * Date             :   08-Jun-2017
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
    public interface ITeamRepository : IBaseRepository<Master_Team>
    {
        List<Master_Team>  GetTeamList();
        Master_Team  InsertTeam(Master_Team recordData);
        Master_Team   UpdateTeam(Master_Team recordData);
        bool   DeleteTeam(Guid ID);
    }

    public class TeamRepository : BaseRepository<Master_Team, CentralDBEntities>, ITeamRepository, IDisposable
    {

        private CentralDBEntities _dbContext;
        public TeamRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_Team> GetTeamList()
        {
            return _dbContext.Master_Team.Where(t=>t.IsDeleted==false).ToList();
        }

        public Master_Team InsertTeam(Master_Team recordData)
        {
            Master_Team result;
            result = _dbContext.Master_Team.Add(recordData);
            _dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return result;
        }

        public Master_Team UpdateTeam(Master_Team recordData)
        {
            var entry = _dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            this._dbContext.Entry(recordData).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(recordData.ID);
            return recordData;
        }

        public bool DeleteTeam(Guid ID)
        {
            bool result;
            Master_Team targetType;
            List<Master_Team> dataList;

            dataList = _dbContext.Master_Team.ToList();
            targetType = dataList.Find(item => item.ID == ID);

            if (targetType != null)
            {
                _dbContext.Master_Team.Remove(targetType);
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
