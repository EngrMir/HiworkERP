
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Database;
using System.Data.Entity;

namespace HiWork.DAL.Repositories
{
    // Developed by Mr. Islam
    // Date: 05/20/2017
    public partial interface ICountryRepository : IBaseRepository<Master_Country>
    {
        List<Master_Country> GetAllCountryList();
        Master_Country GetCountry(long countryId);
        Master_Country InsertCountry(Master_Country country);
        Master_Country UpdateCountry(Master_Country country);
        bool DeleteCountry(long countryId);

        List<Master_Country> GetTradinCountry();
    }
    public class CountryRepository : BaseRepository<Master_Country, CentralDBEntities>, ICountryRepository, IDisposable
    {
        private  CentralDBEntities _dbContext;
        public CountryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

       
        public List<Master_Country> GetAllCountryList()
        {
           try
            {
                return _dbContext.Master_Country.Where(c=>c.IsDeleted==false).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       public  List<Master_Country> GetTradinCountry()
        {
            try
            {
                return _dbContext.Master_Country.Where(c => c.IsTrading == true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_Country GetCountry(long countryId)
        {
           
           try
            {
                return _dbContext.Master_Country.FirstOrDefault(C => C.ID == countryId);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Country InsertCountry(Master_Country country)
        {
           try
            {
                _dbContext.Master_Country.Add(country);
                _dbContext.SaveChanges();

                return country;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Master_Country UpdateCountry(Master_Country country)
        {
           try
            {
                _dbContext.Entry(country).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return country;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteCountry(long countryId)
        {
            try
            {
                var Country = _dbContext.Master_Country.ToList().Find(C => C.ID == countryId);
                if(Country!=null)
                {
                    _dbContext.Master_Country.Remove(Country);
                    _dbContext.SaveChanges();

                    return true;
                }
               
            }
            catch(Exception ex) {
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
