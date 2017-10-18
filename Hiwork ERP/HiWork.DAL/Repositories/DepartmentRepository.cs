
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HiWork.DAL.Repositories
{
    public partial interface IDepartmentRepository: IBaseRepository<Master_Department>
    {
        List<Master_Department> GetAllDepartmentList();
        Master_Department GetDepartment(Guid departmentId);
        Master_Department InsertDepartment(Master_Department department);
        Master_Department UpdateDepartment(Master_Department department);
        bool DeleteDepartment(Guid departmentId);
    }


    public class DepartmentRepository : BaseRepository<Master_Department, CentralDBEntities>, IDepartmentRepository, IDisposable
    {
        private  CentralDBEntities _dbContext;
        public DepartmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_Department> GetAllDepartmentList()
        {
            return _dbContext.Master_Department.Where(d=>d.IsDeleted==false).ToList();
        }

        public Master_Department GetDepartment(Guid departmentId)
        {
            return _dbContext.Master_Department.FirstOrDefault(d => d.ID == departmentId);
        }

        public Master_Department InsertDepartment(Master_Department department)
        {
            var result = _dbContext.Master_Department.Add(department);
            _dbContext.SaveChanges();
            this._dbContext.Entry(department).State = EntityState.Detached;
            this._dbContext.Master_Department.Find(department.ID);
            return result;
        }

        public Master_Department UpdateDepartment(Master_Department department)
        {
            _dbContext.Entry(department).State = EntityState.Modified;
            _dbContext.SaveChanges();
            this._dbContext.Entry(department).State = EntityState.Detached;
            this._dbContext.Master_Department.Find(department.ID);
            return department;
        }

        public bool DeleteDepartment(Guid departmentId)
        {
            var Department = _dbContext.Master_Department.ToList().Find(d => d.ID == departmentId);
            if (Department != null)
            {
                _dbContext.Master_Department.Remove(Department);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
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
