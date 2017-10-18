using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
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

    public partial interface ICompanyTradingCategoryItemRepository : IBaseRepository<Master_CompanyTradingCategoryItem>
    {
        List<Master_CompanyTradingCategoryItem> GetAllCompanyTradingCategoryItemList();

        Master_CompanyTradingCategoryItem InsertCompanyTradingCategoryItem(Master_CompanyTradingCategoryItem mctci);

        Master_CompanyTradingCategoryItem UpdateCompanyTradingCategoryItem(Master_CompanyTradingCategoryItem mctci);

        bool DeleteCompanyTradingCategoryItem(Guid mctciId);
    }

    public class CompanyTradingCategoryItemRepository : BaseRepository<Master_CompanyTradingCategoryItem, CentralDBEntities>, ICompanyTradingCategoryItemRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public CompanyTradingCategoryItemRepository(IUnitOfWork unitofwork) : base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_CompanyTradingCategoryItem> GetAllCompanyTradingCategoryItemList()
        {
            return _dbContext.Master_CompanyTradingCategoryItem.Where(c=>c.IsActive==true&&c.IsDeleted==false).ToList();


        }

        public Master_CompanyTradingCategoryItem InsertCompanyTradingCategoryItem(Master_CompanyTradingCategoryItem mctci)
        {

            var result = _dbContext.Master_CompanyTradingCategoryItem.Add(mctci);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_CompanyTradingCategoryItem UpdateCompanyTradingCategoryItem(Master_CompanyTradingCategoryItem mctci)
        {

            _dbContext.Entry(mctci).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return mctci;



        }

        public bool DeleteCompanyTradingCategoryItem(Guid mctciId)
        {

            var MSE = _dbContext.Master_CompanyTradingCategoryItem.ToList().Find(m => m.ID == mctciId);
            if (MSE != null)
            {
                _dbContext.Master_CompanyTradingCategoryItem.Remove(MSE);
                _dbContext.SaveChanges();

                return true;
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
