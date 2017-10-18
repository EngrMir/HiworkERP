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

    //public partial interface IMasterStaffTechnicalFieldsRepository : IBaseRepository<Master_StaffTechnicalFields>
    //{
    //    List<Master_StaffTechnicalFields> GetAllMasterStaffTechnicalFieldsList();

    //    Master_StaffTechnicalFields InsertMasterStaffTechnicalFields(Master_StaffTechnicalFields mstf);

    //    Master_StaffTechnicalFields UpdateMasterStaffTechnicalFields(Master_StaffTechnicalFields mstf);

    //    bool DeleteMasterStaffTechnicalFields(Guid mstfId);
    //}

    //public class MasterStaffTechnicalFieldsRepository : BaseRepository<Master_StaffTechnicalFields, CentralDBEntities>, IMasterStaffTechnicalFieldsRepository, IDisposable
    //{
    //    private CentralDBEntities _dbContext;

    //    public MasterStaffTechnicalFieldsRepository(IUnitOfWork unitofwork) : base(unitofwork)
    //    {
    //        _dbContext = new CentralDBEntities();

    //    }

    //    public List<Master_StaffTechnicalFields> GetAllMasterStaffTechnicalFieldsList()
    //    {
    //        return _dbContext.Master_StaffTechnicalFields.ToList();


    //    }

    //    public Master_StaffTechnicalFields InsertMasterStaffTechnicalFields(Master_StaffTechnicalFields mstf)
    //    {

    //        var result = _dbContext.Master_StaffTechnicalFields.Add(mstf);
    //        _dbContext.SaveChanges();

    //        return result;
    //    }

    //    public Master_StaffTechnicalFields UpdateMasterStaffTechnicalFields(Master_StaffTechnicalFields mstf)
    //    {

    //        _dbContext.Entry(mstf).State = EntityState.Modified;
    //        _dbContext.SaveChanges();
    //        return mstf;



    //    }

    //    public bool DeleteMasterStaffTechnicalFields(Guid mstfId)
    //    {

    //        var MSE = _dbContext.Master_StaffTechnicalFields.ToList().Find(m => m.ID == mstfId);
    //        if (MSE != null)
    //        {
    //            _dbContext.Master_StaffTechnicalFields.Remove(MSE);
    //            _dbContext.SaveChanges();

    //            return true;
    //        }

    //        return false;
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
