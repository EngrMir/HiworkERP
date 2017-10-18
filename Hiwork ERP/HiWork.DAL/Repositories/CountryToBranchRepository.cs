using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    //public partial interface ICountryToBranchRepository: IBaseRepository<Master_CountryToBranch>
    //{
    //    List<Master_CountryToBranch> GetAllCountryToBranch();
    //    Master_CountryToBranch GetCountryToBranch(Guid Id);
    //    Master_CountryToBranch InsertCountryToBranch(Master_CountryToBranch countrytobranch);
    //    Master_CountryToBranch UpdateCountryToBranch(Master_CountryToBranch countrytobranch);
    //    bool DeleteCountryToBranch(Guid Id);
    //}

    //public class CountryToBranchRepository : BaseRepository<Master_CountryToBranch, CentralDBEntities>, ICountryToBranchRepository ,IDisposable
    //{
    //    private  CentralDBEntities _dbContext;
    //    public CountryToBranchRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    //    {
    //        _dbContext = new CentralDBEntities();
    //    }

    //    public List<Master_CountryToBranch> GetAllCountryToBranch()
    //    {
    //        try
    //        {
    //            return _dbContext.Master_CountryToBranch.ToList();
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //        return null;
    //    }

    //    public Master_CountryToBranch GetCountryToBranch(Guid Id)
    //    {
    //        try
    //        {
    //            return _dbContext.Master_CountryToBranch.FirstOrDefault(c => c.ID == Id);
    //        }
    //        catch (Exception ex) { }
    //        return null;
    //    }

    //    public Master_CountryToBranch InsertCountryToBranch(Master_CountryToBranch countrytobranch)
    //    {
    //        try
    //        {
    //            var result = _dbContext.Master_CountryToBranch.Add(countrytobranch);
    //            _dbContext.SaveChanges();

    //            return result;
    //        }
    //        catch (Exception ex) { }

    //        return null;
    //    }

    //    public Master_CountryToBranch UpdateCountryToBranch(Master_CountryToBranch countrytobranch)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeleteCountryToBranch(Guid Id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (_dbContext != null)
    //            {
    //                _dbContext.Dispose();
    //                _dbContext = null;
    //            }
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this); //Garbase collector
    //    }

    //}
}
