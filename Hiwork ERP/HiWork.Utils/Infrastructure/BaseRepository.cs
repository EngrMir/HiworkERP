using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.Utils.Infrastructure
{
    public class BaseRepository<T, TT> : IBaseRepository<T>
        where TT : DbContext, new()
        where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TT _dbContext = new TT();
        internal DbSet<T> DbSet;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = unitOfWork;
            _unitOfWork.DbContext = _dbContext;
            DbSet = _dbContext.Set<T>();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual IEnumerable<T> GetAll()
        {
            //return DbSet.AsNoTracking().AsEnumerable().ToList();
            return _dbContext.Set<T>().AsNoTracking().AsEnumerable();
        }

        public IList<T> GetList()
        {
            return GetAll().ToList();
        }

        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public bool Exists(object id)
        {
            return id != null && DbSet.Find(id) != null;
        }

        public virtual long Insert(T entity)
        {
            dynamic obj = DbSet.Add(entity);
            _unitOfWork.Save();
            return obj.Id;
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            //_dbContext.Entry(entity).Property("CreatedOn").IsModified = false;
            //_dbContext.Entry(entity).Property("CreatedBy").IsModified = false;
            _unitOfWork.Save();
        }

        public virtual void Delete(object id)
        {
            T entity = DbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            _unitOfWork.Save();
        }

        public void DeleteSoftly(object id)
        {
            T entity = DbSet.Find(id);
            if (entity != null)
            {
                _dbContext.Entry(entity).Property("IsDeleted").CurrentValue = true;
                DbSet.Attach(entity);
                _dbContext.Entry(entity).Property("IsDeleted").IsModified = true;
                _unitOfWork.Save();
            }
        }

        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        public Dictionary<string, string> GetAuditNames(dynamic dynamicObject)
        {
            throw new NotImplementedException();
        }
    }
}
