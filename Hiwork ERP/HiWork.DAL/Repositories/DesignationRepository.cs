using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    // Developed by Mr. Islam
    // Date: 05/24/2017
    public partial interface IDesignationRepository: IBaseRepository<Master_Designation>
    {
        List<Master_Designation> GetAllDesignations();
        Master_Designation GetDesignation(Guid Id);
        Master_Designation InsertDesignation(Master_Designation designation);
        Master_Designation UpdateDesignation(Master_Designation designation);
        bool DeleteDesignation(Guid Id);
    }
    public class DesignationRepository : BaseRepository<Master_Designation, CentralDBEntities>, IDesignationRepository,IDisposable
    {
        private  CentralDBEntities _dbContext;
        public DesignationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_Designation> GetAllDesignations()
        {
           try
            {
                return _dbContext.Master_Designation.Where(d=>d.IsDeleted==false).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_Designation GetDesignation(Guid Id)
        {

            try
            {
                return _dbContext.Master_Designation.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Designation InsertDesignation(Master_Designation designation)
        {
            try
            {
                var result = _dbContext.Master_Designation.Add(designation);
                _dbContext.SaveChanges();
                this._dbContext.Entry(designation).State = EntityState.Detached;
                this._dbContext.Master_Designation.Find(designation.ID);

                return result;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Designation UpdateDesignation(Master_Designation designation)
        {
            try
            {
                _dbContext.Entry(designation).State = EntityState.Modified;
                _dbContext.SaveChanges();
                this._dbContext.Entry(designation).State = EntityState.Detached;
                this._dbContext.Master_Designation.Find(designation.ID);
                return designation;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteDesignation(Guid Id)
        {
            try
            {
                var division = _dbContext.Master_Designation.ToList().Find(d => d.ID == Id);
                if (division != null)
                {
                    _dbContext.Master_Designation.Remove(division);
                    _dbContext.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex) {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            return false;
        }
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

    }
}
