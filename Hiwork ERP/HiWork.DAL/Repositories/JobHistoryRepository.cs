using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace HiWork.DAL.Repositories
{
  public partial interface IJobHistoryRepository : IBaseRepository<Staff_JobHistory>
    {
        List<Staff_JobHistory> GetAllJobHistoryList();
        Staff_JobHistory GetJobHistory(Guid Id);
        Staff_JobHistory InsertJobHistory(Staff_JobHistory JobHistory);
        Staff_JobHistory UpdateJobHistory(Staff_JobHistory JobHistory);
        bool DeleteJobHistory(Guid Id);
    }

    public class JobHistoryRepository : BaseRepository<Staff_JobHistory, CentralDBEntities>, IJobHistoryRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
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
        public JobHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteJobHistory(Guid Id)
        {
            try
            {
                var JobHistory = _dbContext.Staff_JobHistory.ToList().Find(d => d.ID == Id);
                if (JobHistory != null)
                {
                    _dbContext.Staff_JobHistory.Remove(JobHistory);
                    _dbContext.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            return false;
        }

        public List<Staff_JobHistory> GetAllJobHistoryList()
        {
            try
            {
                return _dbContext.Staff_JobHistory.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Staff_JobHistory GetJobHistory(Guid Id)
        {
            try
            {
                return _dbContext.Staff_JobHistory.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Staff_JobHistory InsertJobHistory(Staff_JobHistory JobHistory)
        {
            try
            {
                var result = _dbContext.Staff_JobHistory.Add(JobHistory);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Staff_JobHistory UpdateJobHistory(Staff_JobHistory JobHistory)
        {
            try
            {
                _dbContext.Entry(JobHistory).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return JobHistory;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
