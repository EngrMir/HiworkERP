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

    public partial interface IEstimationSubSpecializedFieldRepository : IBaseRepository<Master_EstimationSubSpecializedField>
    {
        List<Master_EstimationSubSpecializedField> GetAllEstimationSubSpecializedFieldList();

        Master_EstimationSubSpecializedField InsertEstimationSubSpecializedField(Master_EstimationSubSpecializedField messf);

        Master_EstimationSubSpecializedField UpdateEstimationSubSpecializedField(Master_EstimationSubSpecializedField messf);

        bool DeleteEstimationSubSpecializedField(Guid messfId);
    }

    public class EstimationSubSpecializedFieldRepository : BaseRepository<Master_EstimationSubSpecializedField, CentralDBEntities>, IEstimationSubSpecializedFieldRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public EstimationSubSpecializedFieldRepository(IUnitOfWork unitofwork) : base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_EstimationSubSpecializedField> GetAllEstimationSubSpecializedFieldList()
        {
            return _dbContext.Master_EstimationSubSpecializedField.Where(e=>e.IsDeleted==false).ToList();


        }

        public Master_EstimationSubSpecializedField InsertEstimationSubSpecializedField(Master_EstimationSubSpecializedField messf)
        {

            var result = _dbContext.Master_EstimationSubSpecializedField.Add(messf);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_EstimationSubSpecializedField UpdateEstimationSubSpecializedField(Master_EstimationSubSpecializedField messf)
        {

            _dbContext.Entry(messf).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return messf;



        }

        public bool DeleteEstimationSubSpecializedField(Guid messfId)
        {

            var MSSE = _dbContext.Master_EstimationSubSpecializedField.ToList().Find(m => m.ID == messfId);
            if (MSSE != null)
            {
                _dbContext.Master_EstimationSubSpecializedField.Remove(MSSE);
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
