using System;
using System.Collections.Generic;
using AutoMapper;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.Utils.Infrastructure
{
    public class BaseService<T, TU> : IBaseService<T, TU>
        where T : IBaseDomainModel
        where TU: class 
    {
        private readonly IBaseRepository<TU> _dbRepository;

        public BaseService(IBaseRepository<TU> dbRepository)
        {
            if (dbRepository == null) throw new ArgumentNullException("dbRepository");
            _dbRepository = dbRepository;
        }

        public IEnumerable<T> GetAll()
        {
            return Mapper.Map<IEnumerable<T>>(_dbRepository.GetAll());
        }

        public IList<T> GetList()
        {
            return Mapper.Map<IList<T>>(_dbRepository.GetList());
        }

        public T GetById(object id)
        {
            return Mapper.Map<T>(_dbRepository.GetById(id));
        }

        public bool Exists(object id)
        {
            return _dbRepository.Exists(id);
        }

        public long Insert(T entity)
        {
            return _dbRepository.Insert(Mapper.Map<TU>(entity));
        }

        public void Update(T entity)
        {
            _dbRepository.Update(Mapper.Map<TU>(entity));
        }

        public void Delete(object id)
        {
            _dbRepository.Delete(id);
        }

        public void Delete(T entity)
        {
            _dbRepository.Delete(Mapper.Map<TU>(entity));
        }

        public void DeleteSoftly(object id)
        {
            _dbRepository.DeleteSoftly(id);
        }

        private static BusinessTransaction<T, TU> _transaction;
        public BusinessTransaction<T, TU> GetTransaction()
        {
            if (_transaction == null)
            {
                _transaction = new BusinessTransaction<T, TU>(_dbRepository);
            }
            else
            {
                if (!_transaction.IsInTransaction)
                    _transaction.StartTransaction();
            }
            return _transaction;
        }

        //public Pagination<T> GetDataWithPaging(int page, long pagesize, string dbProcedureName, string filter, string sort, Dictionary<string, string> param)
        //{
        //    Pagination<U> list = _dbRepository.GetDataWithPaging(page, pagesize, dbProcedureName, filter, sort, param);
        //    Pagination<T> pagingList = new Pagination<T>();
        //    pagingList.total = list.total;
        //    pagingList.rows = Mapper.Map<List<T>>(list.rows);
        //    return pagingList;
        //}

        //public Pagination<T> GetDataWithPaging(NameValueCollection parametersValues, string dbProcedureName, Dictionary<string, string> parameters = null)
        //{
        //    Pagination<U> list = _dbRepository.GetDataWithPaging(parametersValues, dbProcedureName, parameters);
        //    Pagination<T> pagingList = new Pagination<T>();
        //    pagingList.total = list.total;
        //    pagingList.rows = Mapper.Map<List<T>>(list.rows);
        //    return pagingList;
        //}

        //public Pagination<T> GetAllByPage(int page, long itemsPerPage, Dictionary<string, object> paramList)
        //{
        //    Pagination<U> list = _dbRepository.GetAllWithPaging(page, itemsPerPage, paramList);
        //    Pagination<T> pagingList = new Pagination<T>();
        //    pagingList.total = list.total;
        //    pagingList.rows = Mapper.Map<List<T>>(list.rows);
        //    return pagingList;
        //}
    }
}
