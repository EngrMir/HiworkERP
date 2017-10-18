using System;
using System.Data.Entity;

namespace HiWork.Utils.Infrastructure.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; set; }
        void Save();
        void StartTransaction();
        void Commit();
    }
}
