using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;


namespace HiWork.DAL.Repositories
{
     
        public partial interface IJobCategoryDetailsRepository : IBaseRepository<Master_StaffJobCategoryDetails>
    {
        List<Master_StaffJobCategoryDetails> GetAllJobCategoryDetailsList();
        Master_StaffJobCategoryDetails GetJobCategoryDetails(Guid JobCategoryDetailsId);
        Master_StaffJobCategoryDetails InsertJobCategoryDetails(Master_StaffJobCategoryDetails JobCategoryDetails);
        Master_StaffJobCategoryDetails UpdateJobCategoryDetails(Master_StaffJobCategoryDetails JobCategoryDetails);
        bool DeleteJobCategoryDetails(Guid id);
    }
    public class JobCategoryDetailsRepository : BaseRepository<Master_StaffJobCategoryDetails, CentralDBEntities>, IJobCategoryDetailsRepository
    {
        private readonly CentralDBEntities _dbContext;

        public JobCategoryDetailsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteJobCategoryDetails(Guid id)
        {
            var user = _dbContext.Master_StaffJobCategoryDetails.ToList().Find(f => f.ID == id);
            if (user != null)
            {
                _dbContext.Master_StaffJobCategoryDetails.Remove(user);
                _dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public List<Master_StaffJobCategoryDetails> GetAllJobCategoryDetailsList()
        {
            return _dbContext.Master_StaffJobCategoryDetails.ToList();
        }

        public Master_StaffJobCategoryDetails GetJobCategoryDetails(Guid JobCategoryDetailsId)
        {
            return _dbContext.Master_StaffJobCategoryDetails.FirstOrDefault(u => u.ID == JobCategoryDetailsId);
        }

        public Master_StaffJobCategoryDetails InsertJobCategoryDetails(Master_StaffJobCategoryDetails JobCategoryDetails)
        {
            var result = _dbContext.Master_StaffJobCategoryDetails.Add(JobCategoryDetails);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_StaffJobCategoryDetails UpdateJobCategoryDetails(Master_StaffJobCategoryDetails JobCategoryDetails)
        {
            _dbContext.Entry(JobCategoryDetails).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return JobCategoryDetails;
        }
    }
}
