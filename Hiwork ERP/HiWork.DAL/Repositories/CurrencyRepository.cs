using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Database;
using System.Data.Entity;

namespace HiWork.DAL.Repositories
{
    public partial interface ICurrencyRepository : IBaseRepository<Master_Currency>
    {
        List<Master_Currency> GetAllCurrencyList();
        Master_Currency GetCurrency(long CurrencyId);
        Master_Currency InsertCurrency(Master_Currency Currency);
        Master_Currency UpdateCurrency(Master_Currency Currency);
        bool DeleteCurrency(long CurrencyId);
    }
    public class CurrencyRepository : BaseRepository<Master_Currency, CentralDBEntities>, ICurrencyRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public CurrencyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<Master_Currency> GetAllCurrencyList()
        {
            try
            {
                return _dbContext.Master_Currency.Where(c=>c.IsDeleted==false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_Currency GetCurrency(long CurrencyId)
        {

            try
            {
                return _dbContext.Master_Currency.FirstOrDefault(C => C.ID == CurrencyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_Currency InsertCurrency(Master_Currency Currency)
        {
            try
            {
                _dbContext.Master_Currency.Add(Currency);
                _dbContext.SaveChanges();

                return Currency;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_Currency UpdateCurrency(Master_Currency Currency)
        {
            try
            {
                _dbContext.Entry(Currency).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Currency;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteCurrency(long CurrencyId)
        {
            try
            {
                var Currency = _dbContext.Master_Currency.ToList().Find(C => C.ID == CurrencyId);
                if (Currency != null)
                {
                    _dbContext.Master_Currency.Remove(Currency);
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
