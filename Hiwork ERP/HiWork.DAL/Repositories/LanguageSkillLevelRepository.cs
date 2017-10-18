

/* ******************************************************************************************************************
 * Repository for Master_StaffLanguageSkillLevel Entity
 * Date             :   15-Jun-2017
 * By               :   Ashis Kr. Das
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
    //public interface ILanguageSkillLevelRepository : IBaseRepository<Master_StaffLanguageSkillLevel>
    //{
    //    List<Master_StaffLanguageSkillLevel> GetLanguageSkillLevelList();
    //    Master_StaffLanguageSkillLevel InsertLanguageSkillLevel(Master_StaffLanguageSkillLevel recordData);
    //    Master_StaffLanguageSkillLevel UpdateLanguageSkillLevel(Master_StaffLanguageSkillLevel recordData);
    //    bool DeleteLanguageSkillLevel(Guid ID);
    //}
    //public class LanguageSkillLevelRepository :
    //        BaseRepository<Master_StaffLanguageSkillLevel, CentralDBEntities>,
    //        ILanguageSkillLevelRepository,
    //        IDisposable
    //{
    //    private CentralDBEntities _dbContext;
    //    public LanguageSkillLevelRepository(IUnitOfWork uWork) : base(uWork)
    //    {
    //        this._dbContext = new CentralDBEntities();
    //    }

    //    public List<Master_StaffLanguageSkillLevel> GetLanguageSkillLevelList()
    //    {
    //        return this._dbContext.Master_StaffLanguageSkillLevel.ToList();
    //    }

    //    public Master_StaffLanguageSkillLevel InsertLanguageSkillLevel(Master_StaffLanguageSkillLevel recordData)
    //    {
    //        Master_StaffLanguageSkillLevel result;
    //        result = this._dbContext.Master_StaffLanguageSkillLevel.Add(recordData);
    //        this._dbContext.SaveChanges();
    //        return result;
    //    }

    //    public Master_StaffLanguageSkillLevel UpdateLanguageSkillLevel(Master_StaffLanguageSkillLevel recordData)
    //    {
    //        var entry = this._dbContext.Entry(recordData);
    //        entry.State = EntityState.Modified;
    //        this._dbContext.SaveChanges();
    //        return recordData;
    //    }

    //    public bool DeleteLanguageSkillLevel(Guid ID)
    //    {
    //        bool result;
    //        Master_StaffLanguageSkillLevel data;
    //        List<Master_StaffLanguageSkillLevel> dataList;

    //        dataList = this._dbContext.Master_StaffLanguageSkillLevel.ToList();
    //        data = dataList.Find(item => item.ID == ID);

    //        if (data != null)
    //        {
    //            this._dbContext.Master_StaffLanguageSkillLevel.Remove(data);
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
