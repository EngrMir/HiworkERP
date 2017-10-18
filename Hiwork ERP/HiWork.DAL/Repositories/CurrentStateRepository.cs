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
    public partial interface ICurrentStateRepository : IBaseRepository<Master_StaffCurrentState>
    {
        List<Master_StaffCurrentState> GetAllCurrentStateList();
        Master_StaffCurrentState InsertCurrentState(Master_StaffCurrentState currentState);
        Master_StaffCurrentState GetCurrentState(Guid Id);
        Master_StaffCurrentState UpdateCurrentState(Master_StaffCurrentState currentState);
        bool DeleteCurrentState(Guid Id);
    }

    public class CurrentStateRepository : BaseRepository<Master_StaffCurrentState, CentralDBEntities>, ICurrentStateRepository, IDisposable
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
        public CurrentStateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }
        public bool DeleteCurrentState(Guid Id)
        {
            try
            {
                var currentState = _dbContext.Master_StaffCurrentState.ToList().Find(d => d.ID == Id);
                if (currentState != null)
                {
                    _dbContext.Master_StaffCurrentState.Remove(currentState);
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

        public List<Master_StaffCurrentState> GetAllCurrentStateList()
        {
            try
            {
                return _dbContext.Master_StaffCurrentState.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffCurrentState GetCurrentState(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffCurrentState.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_StaffCurrentState InsertCurrentState(Master_StaffCurrentState currentState)
        {
            try
            {
                var result = _dbContext.Master_StaffCurrentState.Add(currentState);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffCurrentState UpdateCurrentState(Master_StaffCurrentState currentState)
        {
            try
            {
                _dbContext.Entry(currentState).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return currentState;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
