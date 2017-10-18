using System;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.Utils.Infrastructure
{
    public class BusinessTransaction<T,U> : IDisposable
    {
        private readonly IBaseRepository<U> _dbService;
        public bool IsInTransaction { get; set; }
        public BusinessTransaction(IBaseRepository<U> dbService)
        {
            _dbService = dbService;
            GetUnitOfWork().StartTransaction();
            IsInTransaction = true;
        }

        public void StartTransaction()
        {
            GetUnitOfWork().StartTransaction();
            IsInTransaction = true;
        }

        public void Complete()
        {
            GetUnitOfWork().Commit();
            IsInTransaction = false;
        }

        public void Dispose()
        {
            IsInTransaction = false;
            GetUnitOfWork().Dispose();
        }

        private IUnitOfWork GetUnitOfWork()
        {
            return _dbService.UnitOfWork;
        }
    }
}