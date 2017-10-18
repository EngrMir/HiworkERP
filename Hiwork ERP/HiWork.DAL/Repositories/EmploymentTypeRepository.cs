using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace HiWork.DAL.Repositories
{
    public partial interface IEmploymentTypeRepository : IBaseRepository<Master_StaffEmploymentType>
    {
        List<Master_StaffEmploymentType> GetAllEmploymentTypeList();
        Master_StaffEmploymentType GetEmploymentType(Guid Id);
        Master_StaffEmploymentType InsertEmploymentType(Master_StaffEmploymentType userEmploymentType);
        Master_StaffEmploymentType UpdateEmploymentType(Master_StaffEmploymentType userEmploymentType);
        bool DeleteEmploymentType(Guid Id);
    }

    public class EmploymentTypeRepository : BaseRepository<Master_StaffEmploymentType, CentralDBEntities>, IEmploymentTypeRepository
    {
        private readonly CentralDBEntities _dbContext;

        public EmploymentTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteEmploymentType(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffEmploymentType.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Master_StaffEmploymentType.Remove(user);
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

        public List<Master_StaffEmploymentType> GetAllEmploymentTypeList()
        {
            try
            {
                return _dbContext.Master_StaffEmploymentType.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffEmploymentType GetEmploymentType(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffEmploymentType.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffEmploymentType InsertEmploymentType(Master_StaffEmploymentType EmploymentType)
        {
            try
            {
                EmploymentType.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffEmploymentType.Add(EmploymentType);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffEmploymentType UpdateEmploymentType(Master_StaffEmploymentType userEmploymentType)
        {
            try
            {
                _dbContext.Entry(userEmploymentType).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userEmploymentType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
