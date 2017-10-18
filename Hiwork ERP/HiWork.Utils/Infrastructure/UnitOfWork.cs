using System;
using System.Data.Entity;
using System.Transactions;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.Utils.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext _dbContext;
        private bool _disposed;
        private TransactionScope _transaction;

        public UnitOfWork()
        {
        }

        public DbContext DbContext
        {
            get { return _dbContext; }
            set
            {
                _dbContext = value;
            }
        }

        public void Save()
        {
            DbContext.SaveChanges();

        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            Save();
            _transaction.Complete();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
