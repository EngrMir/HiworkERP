using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
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
    public partial interface IEstimationTypeRepository : IBaseRepository<Master_EstimationType>
    {
        List<Master_EstimationType> GetAllEstimationTypeList();

        Master_EstimationType InsertEstimationType(Master_EstimationType mse);

        Master_EstimationType UpdateEstimationType(Master_EstimationType mse);

        bool DeleteEstimationType(int mseId);
    }





    public class EstimationTypeRepository:BaseRepository<Master_EstimationType,CentralDBEntities>,IEstimationTypeRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public EstimationTypeRepository(IUnitOfWork unitofwork):base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_EstimationType> GetAllEstimationTypeList()
        {
            return _dbContext.Master_EstimationType.Where(e=>e.IsDeleted==false).ToList();


        }

        public Master_EstimationType InsertEstimationType(Master_EstimationType mse)
        {

            var result = _dbContext.Master_EstimationType.Add(mse);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_EstimationType UpdateEstimationType(Master_EstimationType mse)
        {

            _dbContext.Entry(mse).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return mse;



        }

        public bool DeleteEstimationType(int mseId)
        {

            var MSE = _dbContext.Master_EstimationType.ToList().Find(m => m.ID == mseId);
            if (MSE != null)
            {
                _dbContext.Master_EstimationType.Remove(MSE);
                _dbContext.SaveChanges();

                return true;
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
