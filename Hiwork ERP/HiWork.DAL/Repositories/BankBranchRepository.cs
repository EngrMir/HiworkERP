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
    public interface IBankBranchRepository : IBaseRepository<Master_BankBranch>
    {

        List<Master_BankBranch> GetAllBankBranchList();
        Master_BankBranch GetBankBranch(Guid Id);
        Master_BankBranch InsertBankBranch(Master_BankBranch bank);
        Master_BankBranch UpdateBankBranch(Master_BankBranch bank);
        bool DeleteBankBranch(Guid Id);
    }

    public class BankBranchRepository : BaseRepository<Master_BankBranch, CentralDBEntities>, IBankBranchRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public BankBranchRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new Database.CentralDBEntities();
        }

        public bool DeleteBankBranch(Guid Id)
        {
            try
            {
                var bankBranch = _dbContext.Master_BankBranch.ToList().Find(d => d.ID == Id);
                if (bankBranch != null)
                {
                    _dbContext.Master_BankBranch.Remove(bankBranch);
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


        public List<Master_BankBranch> GetAllBankBranchList()
        {
            try
            {
                return _dbContext.Master_BankBranch.Where(b=>b.IsDeleted==false).ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_BankBranch GetBankBranch(Guid Id)
        {
            try
            {
                return _dbContext.Master_BankBranch.FirstOrDefault(b => b.ID == Id);
            }
              catch (Exception ex) {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
    
        }

        public Master_BankBranch InsertBankBranch(Master_BankBranch bankBranch)
        {
            try
            {
                var result = _dbContext.Master_BankBranch.Add(bankBranch);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }

        }

        public Master_BankBranch UpdateBankBranch(Master_BankBranch bankBranch)
        {
            try
            {
                _dbContext.Entry(bankBranch).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return bankBranch;
            }
            catch (Exception ex)
            {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }

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
