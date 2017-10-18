

/* ******************************************************************************************************************
 * Repository for Master_StaffOtherExperiences Entity
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
    //public interface IOtherExperiencesRepository : IBaseRepository<Master_StaffOtherExperiences>
    //{
    //    List<Master_StaffOtherExperiences>      GetOtherExperiencesList();
    //    Master_StaffOtherExperiences            InsertOtherExperiences(Master_StaffOtherExperiences recordData);
    //    Master_StaffOtherExperiences            UpdateOtherExperiences(Master_StaffOtherExperiences recordData);
    //    bool                                    DeleteOtherExperiences(Guid ID);
    //}
    //public class OtherExperiencesRepository : BaseRepository<Master_StaffOtherExperiences, CentralDBEntities>,
    //                                            IOtherExperiencesRepository, IDisposable
    //{
    //    private CentralDBEntities _dbContext;
    //    public OtherExperiencesRepository (IUnitOfWork uWork) : base(uWork)
    //    {
    //        this._dbContext = new CentralDBEntities();
    //    }

    //    public List<Master_StaffOtherExperiences> GetOtherExperiencesList()
    //    {
    //        return this._dbContext.Master_StaffOtherExperiences.ToList();
    //    }

    //    public Master_StaffOtherExperiences InsertOtherExperiences(Master_StaffOtherExperiences recordData)
    //    {
    //        Master_StaffOtherExperiences result;
    //        result = this._dbContext.Master_StaffOtherExperiences.Add(recordData);
    //        this._dbContext.SaveChanges();
    //        return result;
    //    }

    //    public Master_StaffOtherExperiences UpdateOtherExperiences(Master_StaffOtherExperiences recordData)
    //    {
    //        var entry = this._dbContext.Entry(recordData);
    //        entry.State = EntityState.Modified;
    //        this._dbContext.SaveChanges();
    //        return recordData;
    //    }

    //    public bool DeleteOtherExperiences(Guid ID)
    //    {
    //        bool result;
    //        Master_StaffOtherExperiences data;
    //        List<Master_StaffOtherExperiences> dataList;

    //        dataList = this._dbContext.Master_StaffOtherExperiences.ToList();
    //        data = dataList.Find(item => item.ID == ID);

    //        if (data != null)
    //        {
    //            this._dbContext.Master_StaffOtherExperiences.Remove(data);
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
