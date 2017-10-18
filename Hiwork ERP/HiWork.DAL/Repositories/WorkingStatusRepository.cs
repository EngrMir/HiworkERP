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
       public partial interface IWorkingStatusRepository : IBaseRepository<Master_WorkingStatus>
    {
        bool DeleteItem(int Id);
        List<Master_WorkingStatus> GetAllList();
        Master_WorkingStatus Get(int Id);
        Master_WorkingStatus InsertItem(Master_WorkingStatus item);
        Master_WorkingStatus UpdateItem(Master_WorkingStatus item);
    }

    public class WorkingStatusRepository : BaseRepository<Master_WorkingStatus, CentralDBEntities>, IWorkingStatusRepository
    {
        private readonly CentralDBEntities _dbContext;
        public WorkingStatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteItem(int Id)
        {
            try
            {
                var user = _dbContext.Master_WorkingStatus.Find(Id);
                if (user != null)
                {
                    _dbContext.Master_WorkingStatus.Remove(user);
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

        public List<Master_WorkingStatus> GetAllList()
        {
            try
            {
                return _dbContext.Master_WorkingStatus.Where(s => s.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_WorkingStatus Get(int Id)
        {
            try
            {
                return _dbContext.Master_WorkingStatus.Find(Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_WorkingStatus InsertItem(Master_WorkingStatus item)
        {
            try
            {
                var result = _dbContext.Master_WorkingStatus.Add(item);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_WorkingStatus UpdateItem(Master_WorkingStatus item)
        {
            try
            {
                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
