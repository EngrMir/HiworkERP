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
    public interface IBankAccountRepository : IBaseRepository<Master_BankAccount> 
    {
        List<Master_BankAccount> GetAllBankAccountList();
        Master_BankAccount GetBankAccount(Guid Id);
        Master_BankAccount InsertBankAccount(Master_BankAccount bank);
        Master_BankAccount UpdateBankAccount(Master_BankAccount bank);
        bool DeleteBankAccount(Guid Id);
        List<Master_BankAccountType> GetAllBankAccountType();
    }

    public class BankAccountRepository : BaseRepository<Master_BankAccount, CentralDBEntities>, IBankAccountRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public BankAccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteBankAccount(Guid Id)
        {
            try
            {
                var bankAcc = _dbContext.Master_BankAccount.ToList().Find(d => d.ID == Id);
                if (bankAcc != null)
                {
                    _dbContext.Master_BankAccount.Remove(bankAcc);
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

      

        public List<Master_BankAccount> GetAllBankAccountList()
        {
            try
            {
                return _dbContext.Master_BankAccount.Where(b=>b.IsDeleted==false).ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_BankAccount GetBankAccount(Guid Id)
        {
            try
            {
                return _dbContext.Master_BankAccount.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_BankAccount InsertBankAccount(Master_BankAccount bankAcc)
        {
            try
            {
                var result = _dbContext.Master_BankAccount.Add(bankAcc);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_BankAccount UpdateBankAccount(Master_BankAccount bankAcc)
        {
            try
            {
                _dbContext.Entry(bankAcc).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return bankAcc;
            }
            catch (Exception ex) { }

            return null;
        }

        public List<Master_BankAccountType> GetAllBankAccountType()
        {
            try
            {
                return _dbContext.Master_BankAccountType.ToList();
            }
            catch (Exception ex) { }

            return null;
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
