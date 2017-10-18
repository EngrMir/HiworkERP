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
    public partial interface ITransproDeliveryPlanRepository : IBaseRepository<TransproDeliveryPlanSetting>
    {
        List<TransproDeliveryPlanSetting> GetAllTransproDeliveryType();
        //TransproDeliveryTypeSetting InsertTransproDeliveryType(TransproDeliveryTypeSetting tlp);
        //TransproDeliveryTypeSetting UpdateTransproDeliveryType(TransproDeliveryTypeSetting tlp);
        //bool DeleteTransproDeliveryType(long tlpId);

    }

    public class TransproDeliveryPlanRepository : BaseRepository<TransproDeliveryPlanSetting, CentralDBEntities>, ITransproDeliveryPlanRepository, IDisposable
    {

        private CentralDBEntities _dbContext;

        public TransproDeliveryPlanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<TransproDeliveryPlanSetting> GetAllTransproDeliveryType()
        {
            return _dbContext.TransproDeliveryPlanSettings.ToList();

        }
        //public TransproDeliveryTypeSetting InsertTransproDeliveryType(TransproDeliveryTypeSetting  tlp)
        //{

        //    var result = _dbContext.TransproDeliveryTypeSettings .Add(tlp);
        //    _dbContext.SaveChanges();

        //    return result;
        //}

        //public TransproDeliveryTypeSetting UpdateTransproDeliveryType(TransproDeliveryTypeSetting tlp)
        //{

        //    _dbContext.Entry(tlp).State = EntityState.Modified;
        //    _dbContext.SaveChanges();
        //    return tlp;



        //}

        //public bool DeleteTransproDeliveryType(long tlpId)
        //{

        //    var Msm = _dbContext.TransproDeliveryTypeSettings.ToList().Find(m => m.ID == tlpId);
        //    if (Msm != null)
        //    {
        //        _dbContext.TransproDeliveryTypeSettings.Remove(Msm);
        //        _dbContext.SaveChanges();

        //        return true;
        //    }

        //    return false;
        //}

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
