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
    public partial interface IBankRepository : IBaseRepository<Master_Bank>
    {
        List<Master_Bank> GetAllBankList();
        Master_Bank GetBank(Guid Id);
        Master_Bank InsertBank(Master_Bank bank);
        Master_Bank UpdateBank(Master_Bank bank);
        bool DeleteBank(Guid Id);
    }

    public class BankRepository : BaseRepository<Master_Bank, CentralDBEntities>, IBankRepository, IDisposable
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
        public BankRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteBank(Guid Id)
        {
            try
            {
                var bank = _dbContext.Master_Bank.ToList().Find(d => d.ID == Id);
                if (bank != null)
                {
                    _dbContext.Master_Bank.Remove(bank);
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

        public List<Master_Bank> GetAllBankList()
        {
            try
            {
                return _dbContext.Master_Bank.Where(b=>b.IsDeleted==false).ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_Bank GetBank(Guid Id)
        {
            try
            {
                return _dbContext.Master_Bank.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_Bank InsertBank(Master_Bank bank)
        {
            try
            {
                var result = _dbContext.Master_Bank.Add(bank);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_Bank UpdateBank(Master_Bank bank)
        {
            try
            {
                _dbContext.Entry(bank).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return bank;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
