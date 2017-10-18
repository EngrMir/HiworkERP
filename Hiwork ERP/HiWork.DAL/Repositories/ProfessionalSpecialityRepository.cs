

/* ******************************************************************************************************************
 * Repository for Master_StaffProfessionalSpeciality Entity
 * Date             :   16-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.DAL.Repositories
{
    //public interface IProfessionalSpecialityRepository : IBaseRepository<Master_StaffProfessionalSpeciality>
    //{
    //    List<Master_StaffProfessionalSpeciality>    GetProfessionalSpecialityList();
    //    Master_StaffProfessionalSpeciality          InsertProfessionalSpeciality(Master_StaffProfessionalSpeciality recordData);
    //    Master_StaffProfessionalSpeciality          UpdateProfessionalSpeciality(Master_StaffProfessionalSpeciality recordData);
    //    bool                                        DeleteProfessionalSpeciality(Guid ID);
    //}
    //public class ProfessionalSpecialityRepository :
    //                                    BaseRepository<Master_StaffProfessionalSpeciality, CentralDBEntities>,
    //                                    IProfessionalSpecialityRepository, IDisposable
    //{
    //    private CentralDBEntities _dbContext;
    //    public ProfessionalSpecialityRepository(IUnitOfWork uWork) : base(uWork)
    //    {
    //        _dbContext = new CentralDBEntities();
    //    }

    //    public List<Master_StaffProfessionalSpeciality> GetProfessionalSpecialityList()
    //    {
    //        return this._dbContext.Master_StaffProfessionalSpeciality.ToList();
    //    }

    //    public Master_StaffProfessionalSpeciality InsertProfessionalSpeciality(Master_StaffProfessionalSpeciality recordData)
    //    {
    //        Master_StaffProfessionalSpeciality result;
    //        result = this._dbContext.Master_StaffProfessionalSpeciality.Add(recordData);
    //        this._dbContext.SaveChanges();
    //        return result;
    //    }

    //    public Master_StaffProfessionalSpeciality UpdateProfessionalSpeciality(Master_StaffProfessionalSpeciality recordData)
    //    {
    //        var entry = this._dbContext.Entry(recordData);
    //        entry.State = EntityState.Modified;
    //        this._dbContext.SaveChanges();
    //        return recordData;
    //    }

    //    public bool DeleteProfessionalSpeciality(Guid ID)
    //    {
    //        bool result;
    //        Master_StaffProfessionalSpeciality delItem;
    //        List<Master_StaffProfessionalSpeciality> dataList;

    //        dataList = this._dbContext.Master_StaffProfessionalSpeciality.ToList();
    //        delItem = dataList.Find(item => item.ID == ID);

    //        if (delItem != null)
    //        {
    //            this._dbContext.Master_StaffProfessionalSpeciality.Remove(delItem);
    //            this._dbContext.SaveChanges();
    //            result = true;
    //        }
    //        else
    //        {
    //            result = false;
    //        }
    //        return result;
    //    }

    //    protected void Dispose(bool disposing)
    //    {
    //        if (disposing == false)
    //            return;
    //        if (this._dbContext == null)
    //            return;
    //        this._dbContext.Dispose();
    //        this._dbContext = null;
    //        return;
    //    }

    //    public void Dispose()
    //    {
    //        this.Dispose(true);
    //        GC.SuppressFinalize(this);
    //        return;
    //    }
    //}
}
