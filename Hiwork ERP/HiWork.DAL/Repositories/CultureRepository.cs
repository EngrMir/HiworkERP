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
    public partial interface ICultureRepository : IBaseRepository<Master_Culture>
    {
        List<Master_Culture> GetAllCultureList();
        Master_Culture GetCulture(Guid Id);
        Master_Culture InsertCulture(Master_Culture culture);
        Master_Culture UpdateCulture(Master_Culture culture);
        bool DeletCulture(Guid Id);
    }
    public class CultureRepository : BaseRepository<Master_Culture, CentralDBEntities>, ICultureRepository, IDisposable
    {
        private  CentralDBEntities _dbContext;
        public CultureRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

     
        public List<Master_Culture> GetAllCultureList()
        {
            try
            {
                return _dbContext.Master_Culture.Where(c=>c.IsDeleted==false).ToList();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Culture GetCulture(Guid Id)
        {
            try
            {
                return _dbContext.Master_Culture.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Culture InsertCulture(Master_Culture culture)
        {
         
            try
            {
                var result = _dbContext.Master_Culture.Add(culture);
                _dbContext.SaveChanges();

                return result;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Culture UpdateCulture(Master_Culture culture)
        {
            try
            {
                _dbContext.Entry(culture).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return culture;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public bool DeletCulture(Guid Id)
        {
            try
            {
                var culture = _dbContext.Master_Culture.ToList().Find(f => f.ID == Id);
                if (culture != null)
                {
                    _dbContext.Master_Culture.Remove(culture);
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
