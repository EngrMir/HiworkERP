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
     
    //public partial interface IJobTypeRepository : IBaseRepository<Master_StaffJobType>
    //{
    //    List<Master_StaffJobType> GetAllJobTypeList();
    //    Master_StaffJobType GetJobType(Guid Id);
    //    Master_StaffJobType InsertJobType(Master_StaffJobType userJobType);
    //    Master_StaffJobType UpdateJobType(Master_StaffJobType userJobType);
    //    bool DeleteJobType(Guid Id);
    //}

    //public class JobTypeRepository : BaseRepository<Master_StaffJobType, CentralDBEntities>, IJobTypeRepository
    //{
    //    private readonly CentralDBEntities _dbContext;

    //    public JobTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    //    {
    //        _dbContext = new CentralDBEntities();
    //    }

    //    public bool DeleteJobType(Guid Id)
    //    {
    //        try
    //        {
    //            var user = _dbContext.Master_StaffJobType.ToList().Find(f => f.ID == Id);
    //            if (user != null)
    //            {
    //                _dbContext.Master_StaffJobType.Remove(user);
    //                _dbContext.SaveChanges();

    //                return true;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string message;
    //            message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            throw new Exception(message);
    //        }
    //        return false;
    //    }

    //    public List<Master_StaffJobType> GetAllJobTypeList()
    //    {
    //        try
    //        {
    //            return _dbContext.Master_StaffJobType.ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public Master_StaffJobType GetJobType(Guid Id)
    //    {
    //        try
    //        {
    //            return _dbContext.Master_StaffJobType.FirstOrDefault(u => u.ID == Id);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public Master_StaffJobType InsertJobType(Master_StaffJobType JobType)
    //    {
    //        try
    //        {
    //            JobType.ID = Guid.NewGuid();
    //            var result = _dbContext.Master_StaffJobType.Add(JobType);
    //            _dbContext.SaveChanges();

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public Master_StaffJobType UpdateJobType(Master_StaffJobType userJobType)
    //    {
    //        try
    //        {
    //            _dbContext.Entry(userJobType).State = EntityState.Modified;
    //            _dbContext.SaveChanges();
    //            return userJobType;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }
    //}
}
