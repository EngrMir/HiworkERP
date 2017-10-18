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

    public partial interface IEstimationSpecializedFieldRepository : IBaseRepository<Master_EstimationSpecializedField>
    {
        List<Master_EstimationSpecializedField> GetAllEstimationSpecializedFieldList();

        Master_EstimationSpecializedField InsertEstimationSpecializedField(Master_EstimationSpecializedField mer);

        Master_EstimationSpecializedField UpdateEstimationSpecializedField(Master_EstimationSpecializedField mer);

        bool DeleteEstimationSpecializedField(Guid merId);
    }

    public class EstimationSpecializedFieldRepository : BaseRepository<Master_EstimationSpecializedField, CentralDBEntities>, IEstimationSpecializedFieldRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public EstimationSpecializedFieldRepository(IUnitOfWork unitofwork) : base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_EstimationSpecializedField> GetAllEstimationSpecializedFieldList()
        {
            return _dbContext.Master_EstimationSpecializedField.Where(e=>e.IsDeleted==false).ToList();
        }

        public Master_EstimationSpecializedField InsertEstimationSpecializedField(Master_EstimationSpecializedField mer)
        {

            var result = _dbContext.Master_EstimationSpecializedField.Add(mer);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_EstimationSpecializedField UpdateEstimationSpecializedField(Master_EstimationSpecializedField mer)
        {

            _dbContext.Entry(mer).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return mer;



        }

        public bool DeleteEstimationSpecializedField(Guid merId)
        {

            var MSE = _dbContext.Master_EstimationSpecializedField.ToList().Find(m => m.ID == merId);
            if (MSE != null)
            {
                _dbContext.Master_EstimationSpecializedField.Remove(MSE);
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
