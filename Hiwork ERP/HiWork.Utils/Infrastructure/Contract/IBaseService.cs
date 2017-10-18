using System.Collections.Generic;

namespace HiWork.Utils.Infrastructure.Contract
{
    public interface IBaseService<T,TU> 
        where T: IBaseDomainModel
        where TU: class 
    {
        IEnumerable<T> GetAll();
        IList<T> GetList();
        T GetById(object id);
        bool Exists(object id);
        long Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void DeleteSoftly(object id);

        BusinessTransaction<T, TU> GetTransaction();
    }
}