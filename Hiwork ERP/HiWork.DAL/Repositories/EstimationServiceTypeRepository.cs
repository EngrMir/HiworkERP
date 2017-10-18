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

    public partial interface IEstimationServiceTypeRepository : IBaseRepository<Master_EstimationServiceType>
    {
        List<Master_EstimationServiceType> GetAllEstimationServiceTypeList();

        Master_EstimationServiceType InsertEstimationServiceType(Master_EstimationServiceType mest);

        Master_EstimationServiceType UpdateEstimationServiceType(Master_EstimationServiceType mest);

        bool DeleteEstimationServiceType(Guid mestId);
    }

    public class EstimationServiceTypeRepository : BaseRepository<Master_EstimationServiceType, CentralDBEntities>, IEstimationServiceTypeRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public EstimationServiceTypeRepository(IUnitOfWork unitofwork) : base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_EstimationServiceType> GetAllEstimationServiceTypeList()
        {
            return _dbContext.Master_EstimationServiceType.Where(e=>e.IsDeleted==false).ToList();


        }

        public Master_EstimationServiceType InsertEstimationServiceType(Master_EstimationServiceType mest)
        {

            var result = _dbContext.Master_EstimationServiceType.Add(mest);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_EstimationServiceType UpdateEstimationServiceType(Master_EstimationServiceType mest)
        {

            _dbContext.Entry(mest).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return mest;



        }

        public bool DeleteEstimationServiceType(Guid mestId)
        {

            var MSE = _dbContext.Master_EstimationServiceType.ToList().Find(m => m.ID == mestId);
            if (MSE != null)
            {
                _dbContext.Master_EstimationServiceType.Remove(MSE);
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
