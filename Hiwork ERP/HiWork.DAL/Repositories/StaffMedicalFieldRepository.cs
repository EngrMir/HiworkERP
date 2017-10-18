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
      public partial interface IStaffMedicalFieldRepository : IBaseRepository<Master_StaffMedicalField>
    {
        List<Master_StaffMedicalField> GetAllStaffMedicalFieldList();
        Master_StaffMedicalField GetStaffMedicalField(Guid Id);
        Master_StaffMedicalField InsertStaffMedicalField(Master_StaffMedicalField StaffMedicalField);
        Master_StaffMedicalField UpdateStaffMedicalField(Master_StaffMedicalField StaffMedicalField);
        bool DeleteStaffMedicalField(Guid Id);
    }

    public class StaffMedicalFieldRepository : BaseRepository<Master_StaffMedicalField, CentralDBEntities>, IStaffMedicalFieldRepository, IDisposable
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
        public StaffMedicalFieldRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteStaffMedicalField(Guid Id)
        {
            try
            {
                var StaffMedicalField = _dbContext.Master_StaffMedicalField.ToList().Find(d => d.ID == Id);
                if (StaffMedicalField != null)
                {
                    _dbContext.Master_StaffMedicalField.Remove(StaffMedicalField);
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

        public List<Master_StaffMedicalField> GetAllStaffMedicalFieldList()
        {
            try
            {
                return _dbContext.Master_StaffMedicalField.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffMedicalField GetStaffMedicalField(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffMedicalField.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_StaffMedicalField InsertStaffMedicalField(Master_StaffMedicalField StaffMedicalField)
        {
            try
            {
                var result = _dbContext.Master_StaffMedicalField.Add(StaffMedicalField);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffMedicalField UpdateStaffMedicalField(Master_StaffMedicalField StaffMedicalField)
        {
            try
            {
                _dbContext.Entry(StaffMedicalField).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return StaffMedicalField;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
