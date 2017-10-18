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
    public partial interface IJobCategoryRepository : IBaseRepository<Master_StaffJobCategory>
    {
        List<Master_StaffJobCategory> GetAllJobCategoryList();
        Master_StaffJobCategory GetJobCategory(Guid JobCategoryId);
        Master_StaffJobCategory InsertJobCategory(Master_StaffJobCategory jobCategory);
        Master_StaffJobCategory UpdateJobCategory(Master_StaffJobCategory JobCategory);
        bool DeleteJobCategory(Guid id);
    }
    public class JobCategoryRepository : BaseRepository<Master_StaffJobCategory, CentralDBEntities>, IJobCategoryRepository
    {
        private readonly CentralDBEntities _dbContext;

        public JobCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteJobCategory(Guid id)
        {
            var user = _dbContext.Master_StaffJobCategory.ToList().Find(f => f.ID == id);
            if (user != null)
            {
                _dbContext.Master_StaffJobCategory.Remove(user);
                _dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public List<Master_StaffJobCategory> GetAllJobCategoryList()
        {
            return _dbContext.Master_StaffJobCategory.ToList();
        }

        public Master_StaffJobCategory GetJobCategory(Guid JobCategoryId)
        {
            return _dbContext.Master_StaffJobCategory.FirstOrDefault(u => u.ID == JobCategoryId);
        }

        public Master_StaffJobCategory InsertJobCategory(Master_StaffJobCategory jobCategory)
        {
            var result = _dbContext.Master_StaffJobCategory.Add(jobCategory);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_StaffJobCategory UpdateJobCategory(Master_StaffJobCategory JobCategory)
        {
            _dbContext.Entry(JobCategory).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return JobCategory;
        }
    }
    }
