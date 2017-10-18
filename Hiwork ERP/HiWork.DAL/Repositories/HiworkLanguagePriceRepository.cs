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
   public partial interface IHiworkLanguagePriceRepository : IBaseRepository<HiworkLanguagePrice>
    {
        List<HiworkLanguagePrice> GetAllHiworkLanguagePriceList();
        HiworkLanguagePrice GetHiworkLanguagePrice(Guid Id);
        HiworkLanguagePrice InsertHiworkLanguagePrice(HiworkLanguagePrice HiworkLanguagePrice);
        HiworkLanguagePrice UpdateHiworkLanguagePrice(HiworkLanguagePrice HiworkLanguagePrice);
        bool DeleteHiworkLanguagePrice(Guid Id);
    }

    public class HiworkLanguagePriceRepository : BaseRepository<HiworkLanguagePrice, CentralDBEntities>, IHiworkLanguagePriceRepository, IDisposable
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
        public HiworkLanguagePriceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteHiworkLanguagePrice(Guid Id)
        {
            try
            {
                var HiworkLanguagePrice = _dbContext.HiworkLanguagePrices.ToList().Find(d => d.ID == Id);
                if (HiworkLanguagePrice != null)
                {
                    _dbContext.HiworkLanguagePrices.Remove(HiworkLanguagePrice);
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

        public List<HiworkLanguagePrice> GetAllHiworkLanguagePriceList()
        {
            try
            {
                return _dbContext.HiworkLanguagePrices.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public HiworkLanguagePrice GetHiworkLanguagePrice(Guid Id)
        {
            try
            {
                return _dbContext.HiworkLanguagePrices.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public HiworkLanguagePrice InsertHiworkLanguagePrice(HiworkLanguagePrice HiworkLanguagePrice)
        {
            try
            {
                var result = _dbContext.HiworkLanguagePrices.Add(HiworkLanguagePrice);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public HiworkLanguagePrice UpdateHiworkLanguagePrice(HiworkLanguagePrice HiworkLanguagePrice)
        {
            try
            {
                _dbContext.Entry(HiworkLanguagePrice).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return HiworkLanguagePrice;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
