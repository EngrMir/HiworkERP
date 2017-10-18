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
     
    public partial interface IEducationalHistoryRepository : IBaseRepository<Staff_EducationalHistory>
    {
        List<Staff_EducationalHistory> GetAllEducationalHistoryList();
        Staff_EducationalHistory GetEducationalHistory(Guid Id);
        Staff_EducationalHistory InsertEducationalHistory(Staff_EducationalHistory EducationalHistory);
        Staff_EducationalHistory UpdateEducationalHistory(Staff_EducationalHistory EducationalHistory);
        bool DeleteEducationalHistory(Guid Id);
    }

    public class EducationalHistoryRepository : BaseRepository<Staff_EducationalHistory, CentralDBEntities>, IEducationalHistoryRepository, IDisposable
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
        public EducationalHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteEducationalHistory(Guid Id)
        {
            try
            {
                var EducationalHistory = _dbContext.Staff_EducationalHistory.ToList().Find(d => d.ID == Id);
                if (EducationalHistory != null)
                {
                    _dbContext.Staff_EducationalHistory.Remove(EducationalHistory);
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

        public List<Staff_EducationalHistory> GetAllEducationalHistoryList()
        {
            try
            {
                return _dbContext.Staff_EducationalHistory.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Staff_EducationalHistory GetEducationalHistory(Guid Id)
        {
            try
            {
                return _dbContext.Staff_EducationalHistory.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Staff_EducationalHistory InsertEducationalHistory(Staff_EducationalHistory EducationalHistory)
        {
            try
            {
                var result = _dbContext.Staff_EducationalHistory.Add(EducationalHistory);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Staff_EducationalHistory UpdateEducationalHistory(Staff_EducationalHistory EducationalHistory)
        {
            try
            {
                _dbContext.Entry(EducationalHistory).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return EducationalHistory;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
