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

    public partial interface IUnitPriceRepository : IBaseRepository<Master_UnitPriceSetting>
    {
        List<Master_UnitPriceSetting> GetAllUnitPrice();
        Master_UnitPriceSetting InsertUnitPrice(Master_UnitPriceSetting UnitPrice);
        Master_UnitPriceSetting UpdateUnitPrice(Master_UnitPriceSetting UnitPrice);
        bool MatchedByID(Guid sourcelanguagid, Guid targetlanguageid,long unitid, int estimationtypeid);
        bool DeleteUnitPrice(long ID);
    }

  public class UnitPriceRepository:BaseRepository<Master_UnitPriceSetting,CentralDBEntities>,IUnitPriceRepository,IDisposable
  {
        private CentralDBEntities _dbContext;
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_dbContext !=null)
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

        public UnitPriceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_UnitPriceSetting> GetAllUnitPrice()
        {
            try
            {
                return _dbContext.Master_UnitPriceSetting.Where(u=>u.IsDeleted==false).ToList();
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
            }
            
        }
        public Master_UnitPriceSetting InsertUnitPrice(Master_UnitPriceSetting UnitPrice)
        {
            try
            {
                var result = _dbContext.Master_UnitPriceSetting.Add(UnitPrice);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
            
        }
      

        public Master_UnitPriceSetting UpdateUnitPrice(Master_UnitPriceSetting UnitPrice)
        {
            try
            {
                _dbContext.Entry(UnitPrice).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return UnitPrice;
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
            }
            
        }
        public bool DeleteUnitPrice(long ID)
        {
            try
            {
                var unitprice = _dbContext.Master_UnitPriceSetting.ToList().Find(d => d.ID == ID);
                if (unitprice != null)
                {
                    _dbContext.Master_UnitPriceSetting.Remove(unitprice);
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
        public bool MatchedByID(Guid sourcelanguagid, Guid targetlanguageid, long unitid, int estimationtypeid)
        {
            try
            {
                var set = _dbContext.Master_UnitPriceSetting.Where(u =>u.SourceLanguageID==sourcelanguagid && u.TargetLanguageID==targetlanguageid&&u.UnitID==unitid &&u.EstimationTypeID== estimationtypeid).ToList();
                if (set != null && set.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }



        }

    }
}
