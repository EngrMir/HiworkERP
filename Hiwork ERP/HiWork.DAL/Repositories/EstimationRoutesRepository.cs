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

    public partial interface IEstimationRoutesRepository : IBaseRepository<Master_EstimationRoutes>
    {
        List<Master_EstimationRoutes> GetAllEstimationRoutesList();

        Master_EstimationRoutes InsertEstimationRoutes(Master_EstimationRoutes mer);

        Master_EstimationRoutes UpdateEstimationRoutes(Master_EstimationRoutes mer);

        bool DeleteEstimationRoutes(Guid merId);
    }

    public class EstimationRoutesRepository :BaseRepository<Master_EstimationRoutes,CentralDBEntities>, IEstimationRoutesRepository,IDisposable
    {
        private CentralDBEntities _dbContext;

        public EstimationRoutesRepository(IUnitOfWork unitofwork):base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_EstimationRoutes> GetAllEstimationRoutesList()
        {
            return _dbContext.Master_EstimationRoutes.Where(e=>e.IsDeleted==false).ToList();


        }

        public Master_EstimationRoutes InsertEstimationRoutes(Master_EstimationRoutes mer)
        {

            var result = _dbContext.Master_EstimationRoutes.Add(mer);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_EstimationRoutes UpdateEstimationRoutes(Master_EstimationRoutes mer)
        {

            _dbContext.Entry(mer).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return mer;



        }

        public bool DeleteEstimationRoutes(Guid merId)
        {

            var MSE = _dbContext.Master_EstimationRoutes.ToList().Find(m => m.ID == merId);
            if (MSE != null)
            {
                _dbContext.Master_EstimationRoutes.Remove(MSE);
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
