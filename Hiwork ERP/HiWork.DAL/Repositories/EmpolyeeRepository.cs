using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public interface IEmpolyeeRepository : IBaseRepository<Employee> 
    {
        List<Employee> GetAllEmployeeList();
        Employee GetEmployeeById(Guid Id);
        Employee InsertEmployee(Employee model);
        Employee UpdateEmployee(Employee model);
        bool DeleteEmployee(Guid Id);
    }
    public class EmpolyeeRepository : BaseRepository<Employee, CentralDBEntities>, IEmpolyeeRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public EmpolyeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

 

        public List<Employee> GetAllEmployeeList()
        {
            try
            {
                return _dbContext.Employees.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Employee GetEmployeeById(Guid Id)
        {
            try
            {
                return _dbContext.Employees.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Employee InsertEmployee(Employee model)
        {
            try
            {
                var result = _dbContext.Employees.Add(model);
                _dbContext.SaveChanges();

                return result;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        string Property = validationError.PropertyName;
                        string Error = validationError.ErrorMessage;
                       
                    }
                }
            }
            catch(Exception ex)
            {
                string mgs = ex.Message;
            }

            return null;
        }

        public Employee UpdateEmployee(Employee model)
        {
            try
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return model;
            }
            catch (Exception ex) { }

            return null;
        }

        public bool DeleteEmployee(Guid Id)
        {
            try
            {
                var employee = _dbContext.Employees.ToList().Find(d => d.ID == Id);
                if (employee != null)
                {
                    _dbContext.Employees.Remove(employee);
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
