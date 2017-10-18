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
       public partial interface IStaffNarrationTypeRepository : IBaseRepository<Master_StaffNarrationType>
    {
        List<Master_StaffNarrationType> GetAllStaffNarrationTypeList();
        Master_StaffNarrationType GetStaffNarrationType(Guid Id);
        Master_StaffNarrationType InsertStaffNarrationType(Master_StaffNarrationType userStaffNarrationType);
        Master_StaffNarrationType UpdateStaffNarrationType(Master_StaffNarrationType userStaffNarrationType);
        bool DeleteStaffNarrationType(Guid Id);
    }

    public class StaffNarrationTypeRepository : BaseRepository<Master_StaffNarrationType, CentralDBEntities>, IStaffNarrationTypeRepository
    {
        private readonly CentralDBEntities _dbContext;
        public StaffNarrationTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteStaffNarrationType(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffNarrationType.Find(Id);
                if (user != null)
                {
                    _dbContext.Master_StaffNarrationType.Remove(user);
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

        public List<Master_StaffNarrationType> GetAllStaffNarrationTypeList()
        {
            try
            {
                return _dbContext.Master_StaffNarrationType.Where(s =>s.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffNarrationType GetStaffNarrationType(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffNarrationType.Find(Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffNarrationType InsertStaffNarrationType(Master_StaffNarrationType StaffNarrationType)
        {
            try
            {
                StaffNarrationType.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffNarrationType.Add(StaffNarrationType);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffNarrationType UpdateStaffNarrationType(Master_StaffNarrationType userStaffNarrationType)
        {
            try
            {
                _dbContext.Entry(userStaffNarrationType).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userStaffNarrationType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
