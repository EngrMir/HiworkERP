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

    public partial interface ICompanyTradingCategoryRepository : IBaseRepository<Master_CompanyTradingCategory>
    {
        List<Master_CompanyTradingCategory> GetAllCompanyTradingCategoryList();

        Master_CompanyTradingCategory InsertCompanyTradingCategory(Master_CompanyTradingCategory mctc);

        Master_CompanyTradingCategory UpdateCompanyTradingCategory(Master_CompanyTradingCategory mctc);

        bool DeleteCompanyTradingCategory(Guid mctcId);
    }

    public class CompanyTradingCategoryRepository : BaseRepository<Master_CompanyTradingCategory, CentralDBEntities>, ICompanyTradingCategoryRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public CompanyTradingCategoryRepository(IUnitOfWork unitofwork) : base(unitofwork)
        {
            _dbContext = new CentralDBEntities();

        }

        public List<Master_CompanyTradingCategory> GetAllCompanyTradingCategoryList()
        {
            return _dbContext.Master_CompanyTradingCategory.Where(c=>c.IsActive==true&&c.IsDeleted==false).ToList();


        }

        public Master_CompanyTradingCategory InsertCompanyTradingCategory(Master_CompanyTradingCategory mctc)
        {

            var result = _dbContext.Master_CompanyTradingCategory.Add(mctc);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_CompanyTradingCategory UpdateCompanyTradingCategory(Master_CompanyTradingCategory mctc)
        {

            _dbContext.Entry(mctc).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return mctc;



        }

        public bool DeleteCompanyTradingCategory(Guid mctcId)
        {

            var MSE = _dbContext.Master_CompanyTradingCategory.ToList().Find(m => m.ID == mctcId);
            if (MSE != null)
            {
                _dbContext.Master_CompanyTradingCategory.Remove(MSE);
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
