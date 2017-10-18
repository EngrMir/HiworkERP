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
     public partial interface IStaffPatentFieldRepository : IBaseRepository<Master_StaffPatentField>
    {
        List<Master_StaffPatentField> GetAllStaffPatentFieldList();
        Master_StaffPatentField GetStaffPatentField(Guid Id);
        Master_StaffPatentField InsertStaffPatentField(Master_StaffPatentField StaffPatentField);
        Master_StaffPatentField UpdateStaffPatentField(Master_StaffPatentField StaffPatentField);
        bool DeleteStaffPatentField(Guid Id);
    }

    public class StaffPatentFieldRepository : BaseRepository<Master_StaffPatentField, CentralDBEntities>, IStaffPatentFieldRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
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
        public StaffPatentFieldRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteStaffPatentField(Guid Id)
        {
            try
            {
                var StaffPatentField = _dbContext.Master_StaffPatentField.ToList().Find(d => d.ID == Id);
                if (StaffPatentField != null)
                {
                    _dbContext.Master_StaffPatentField.Remove(StaffPatentField);
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

        public List<Master_StaffPatentField> GetAllStaffPatentFieldList()
        {
            try
            {
                return _dbContext.Master_StaffPatentField.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffPatentField GetStaffPatentField(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffPatentField.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_StaffPatentField InsertStaffPatentField(Master_StaffPatentField StaffPatentField)
        {
            try
            {
                var result = _dbContext.Master_StaffPatentField.Add(StaffPatentField);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffPatentField UpdateStaffPatentField(Master_StaffPatentField StaffPatentField)
        {
            try
            {
                _dbContext.Entry(StaffPatentField).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return StaffPatentField;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
