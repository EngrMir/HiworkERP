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
    public partial interface ICategoryRepository : IBaseRepository<Master_StaffCategory>
    {
        List<Master_StaffCategory> GetAllCategoryList();
        Master_StaffCategory GetCategory(Guid Id);
        Master_StaffCategory InsertCategory(Master_StaffCategory userCategory);
        Master_StaffCategory UpdateCategory(Master_StaffCategory userCategory);
        bool DeleteCategory(Guid Id);
    }

    public class CategoryRepository : BaseRepository<Master_StaffCategory, CentralDBEntities>, ICategoryRepository
    {
        private readonly CentralDBEntities _dbContext;

        public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteCategory(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffCategory.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Master_StaffCategory.Remove(user);
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

        public List<Master_StaffCategory> GetAllCategoryList()
        {
            try
            {
                return _dbContext.Master_StaffCategory.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffCategory GetCategory(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffCategory.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffCategory InsertCategory(Master_StaffCategory Category)
        {
            try
            {
                Category.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffCategory.Add(Category);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffCategory UpdateCategory(Master_StaffCategory userCategory)
        {
            try
            {
                _dbContext.Entry(userCategory).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userCategory;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
