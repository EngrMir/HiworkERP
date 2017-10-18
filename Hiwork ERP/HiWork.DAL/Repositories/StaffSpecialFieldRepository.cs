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
       public partial interface IStaffSpecialFieldRepository : IBaseRepository<Master_StaffSpecialField>
    {
        List<Master_StaffSpecialField> GetAllStaffSpecialFieldList();
        Master_StaffSpecialField GetStaffSpecialField(Guid Id);
        Master_StaffSpecialField InsertStaffSpecialField(Master_StaffSpecialField userStaffSpecialField);
        Master_StaffSpecialField UpdateStaffSpecialField(Master_StaffSpecialField userStaffSpecialField);
        bool DeleteStaffSpecialField(Guid Id);
    }

    public class StaffSpecialFieldRepository : BaseRepository<Master_StaffSpecialField, CentralDBEntities>, IStaffSpecialFieldRepository
    {
        private readonly CentralDBEntities _dbContext;

        public StaffSpecialFieldRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteStaffSpecialField(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffSpecialField.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Master_StaffSpecialField.Remove(user);
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

        public List<Master_StaffSpecialField> GetAllStaffSpecialFieldList()
        {
            try
            {
                return _dbContext.Master_StaffSpecialField.Where(s=>s.IsDeleted==false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffSpecialField GetStaffSpecialField(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffSpecialField.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffSpecialField InsertStaffSpecialField(Master_StaffSpecialField StaffSpecialField)
        {
            try
            {
                StaffSpecialField.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffSpecialField.Add(StaffSpecialField);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffSpecialField UpdateStaffSpecialField(Master_StaffSpecialField userStaffSpecialField)
        {
            try
            {
                _dbContext.Entry(userStaffSpecialField).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userStaffSpecialField;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
