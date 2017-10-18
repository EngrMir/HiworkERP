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
     public partial interface IStaffKnowledgeFieldRepository : IBaseRepository<Master_StaffKnowledgeField>
    {
        List<Master_StaffKnowledgeField> GetAllStaffKnowledgeFieldList();
        Master_StaffKnowledgeField GetStaffKnowledgeField(Guid Id);
        Master_StaffKnowledgeField InsertStaffKnowledgeField(Master_StaffKnowledgeField StaffKnowledgeField);
        Master_StaffKnowledgeField UpdateStaffKnowledgeField(Master_StaffKnowledgeField StaffKnowledgeField);
        bool DeleteStaffKnowledgeField(Guid Id); 
    }

    public class StaffKnowledgeFieldRepository : BaseRepository<Master_StaffKnowledgeField, CentralDBEntities>, IStaffKnowledgeFieldRepository, IDisposable
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
        public StaffKnowledgeFieldRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteStaffKnowledgeField(Guid Id)
        {
            try
            {
                var StaffKnowledgeField = _dbContext.Master_StaffKnowledgeField.ToList().Find(d => d.ID == Id);
                if (StaffKnowledgeField != null)
                {
                    _dbContext.Master_StaffKnowledgeField.Remove(StaffKnowledgeField);
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

        public List<Master_StaffKnowledgeField> GetAllStaffKnowledgeFieldList()
        {
            try
            {
                return _dbContext.Master_StaffKnowledgeField.Where(s=>s.IsDeleted==false).ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffKnowledgeField GetStaffKnowledgeField(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffKnowledgeField.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_StaffKnowledgeField InsertStaffKnowledgeField(Master_StaffKnowledgeField StaffKnowledgeField)
        {
            try
            {
                var result = _dbContext.Master_StaffKnowledgeField.Add(StaffKnowledgeField);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffKnowledgeField UpdateStaffKnowledgeField(Master_StaffKnowledgeField StaffKnowledgeField)
        {
            try
            {
                _dbContext.Entry(StaffKnowledgeField).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return StaffKnowledgeField;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
