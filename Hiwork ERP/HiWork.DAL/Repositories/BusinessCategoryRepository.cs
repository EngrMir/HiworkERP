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
    public partial interface IBusinessCategoryRepository : IBaseRepository<Master_StaffBusinessCategory>
    {
        List<Master_StaffBusinessCategory> GetAllBusinessCategoryList();
        Master_StaffBusinessCategory GetBusinessCategory(Guid Id);
        Master_StaffBusinessCategory InsertBusinessCategory(Master_StaffBusinessCategory userBusinessCategory);
        Master_StaffBusinessCategory UpdateBusinessCategory(Master_StaffBusinessCategory userBusinessCategory);
        bool DeleteBusinessCategory(Guid Id);
    }
    public class BusinessCategoryRepository : BaseRepository<Master_StaffBusinessCategory, CentralDBEntities>, IBusinessCategoryRepository
    {
        private readonly CentralDBEntities _dbContext;

        public BusinessCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteBusinessCategory(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffBusinessCategory.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Master_StaffBusinessCategory.Remove(user);
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

        public List<Master_StaffBusinessCategory> GetAllBusinessCategoryList()
        {
            try
            {
                return _dbContext.Master_StaffBusinessCategory.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffBusinessCategory GetBusinessCategory(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffBusinessCategory.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffBusinessCategory InsertBusinessCategory(Master_StaffBusinessCategory BusinessCategory)
        {
            try
            {
                BusinessCategory.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffBusinessCategory.Add(BusinessCategory);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffBusinessCategory UpdateBusinessCategory(Master_StaffBusinessCategory userBusinessCategory)
        {
            try
            {
                _dbContext.Entry(userBusinessCategory).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userBusinessCategory;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
