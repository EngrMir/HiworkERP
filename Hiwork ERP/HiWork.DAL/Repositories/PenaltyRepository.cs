using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public interface IpenaltyRepository : IBaseRepository<Master_Penalty>
    {
         Master_Penalty InsertPenalty(Master_Penalty model);
         Master_Penalty UpdatePenalty(Master_Penalty model);
         List<Master_Penalty> GetAllPenaltyList();
         bool DeletePenalty(BaseViewModel model);
    }
    public class PenaltyRepository : BaseRepository<Master_Penalty, CentralDBEntities>, IpenaltyRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public PenaltyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<Master_Penalty> GetAllPenaltyList()
        {
            var result = _dbContext.Master_Penalty.ToList();

            return result;
        }

        public Master_Penalty InsertPenalty(Master_Penalty model)
        {
            try
            {
                var result = _dbContext.Master_Penalty.Add(model);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_Penalty UpdatePenalty(Master_Penalty model)
        {
            try
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return model;
            }
            catch (Exception ex) { }

            return null;
        }
        public bool DeletePenalty(BaseViewModel model)
        {
            try
            {
                var penalty = _dbContext.Master_Penalty.ToList().Find(d => d.ID == model.ID);
                if (penalty != null)
                {
                    _dbContext.Master_Penalty.Remove(penalty);
                    _dbContext.SaveChanges();

                    return true;
                }
            }
            catch(Exception ex)
            {

            }
            return false;
        }

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
    }
}
