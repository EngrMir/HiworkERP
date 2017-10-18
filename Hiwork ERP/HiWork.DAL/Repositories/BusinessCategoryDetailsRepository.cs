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
    
    public partial interface IBusinessCategoryDetailsRepository : IBaseRepository<Master_StaffBusinessCategoryDetails>
    {
        List<Master_StaffBusinessCategoryDetails> GetAllBusinessCategoryDetailsList();
        Master_StaffBusinessCategoryDetails GetBusinessCategoryDetails(Guid Id);
        Master_StaffBusinessCategoryDetails InsertBusinessCategoryDetails(Master_StaffBusinessCategoryDetails BusinessCategoryDetails);
        Master_StaffBusinessCategoryDetails UpdateBusinessCategoryDetails(Master_StaffBusinessCategoryDetails BusinessCategoryDetails);
        bool DeleteBusinessCategoryDetails(Guid Id);
    }

    public class BusinessCategoryDetailsRepository : BaseRepository<Master_StaffBusinessCategoryDetails, CentralDBEntities>, IBusinessCategoryDetailsRepository, IDisposable
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
        public BusinessCategoryDetailsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteBusinessCategoryDetails(Guid Id)
        {
            try
            {
                var BusinessCategoryDetails = _dbContext.Master_StaffBusinessCategoryDetails.ToList().Find(d => d.ID == Id);
                if (BusinessCategoryDetails != null)
                {
                    _dbContext.Master_StaffBusinessCategoryDetails.Remove(BusinessCategoryDetails);
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

        public List<Master_StaffBusinessCategoryDetails> GetAllBusinessCategoryDetailsList()
        {
            try
            {
                return _dbContext.Master_StaffBusinessCategoryDetails.ToList();
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffBusinessCategoryDetails GetBusinessCategoryDetails(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffBusinessCategoryDetails.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) { }
            return null;
        }

        public Master_StaffBusinessCategoryDetails InsertBusinessCategoryDetails(Master_StaffBusinessCategoryDetails BusinessCategoryDetails)
        {
            try
            {
                var result = _dbContext.Master_StaffBusinessCategoryDetails.Add(BusinessCategoryDetails);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex) { }

            return null;
        }

        public Master_StaffBusinessCategoryDetails UpdateBusinessCategoryDetails(Master_StaffBusinessCategoryDetails BusinessCategoryDetails)
        {
            try
            {
                _dbContext.Entry(BusinessCategoryDetails).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return BusinessCategoryDetails;
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
