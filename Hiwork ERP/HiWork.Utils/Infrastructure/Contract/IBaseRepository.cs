using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HiWork.Utils.Infrastructure.Contract
{
    public interface IBaseRepository<T>
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        IEnumerable<T> GetAll();
        IList<T> GetList();
        T GetById(object id);
        bool Exists(object id);
        long Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void DeleteSoftly(object id);

        IUnitOfWork UnitOfWork { get; }
        Dictionary<string, string> GetAuditNames(dynamic dynamicObject);
    }
}