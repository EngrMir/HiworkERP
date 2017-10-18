using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public interface IPartnerServiceTypeRepository : IBaseRepository<Master_PartnerServiceType>
    {
        List<Master_PartnerServiceType> GetAllList();
    }
    public class PartnerServiceTypeRepository : BaseRepository<Master_PartnerServiceType, CentralDBEntities>, IPartnerServiceTypeRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public PartnerServiceTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_PartnerServiceType> GetAllList()
        {
            return _dbContext.Master_PartnerServiceType.ToList();
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
