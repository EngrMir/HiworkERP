

/* ******************************************************************************************************************
 * Repository for Master_StaffDevelopmentSkillItem Entity
 * Date             :   18-July-2017
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

    public interface IStaffDevelopmentSkillItemRepository : IBaseRepository<Master_StaffDevelopmentSkillItem>
    {
        List<Master_StaffDevelopmentSkillItem>      GetStaffDevelopmentSkillItemList();
        Master_StaffDevelopmentSkillItem            InsertStaffDevelopmentSkillItem(Master_StaffDevelopmentSkillItem data);
        Master_StaffDevelopmentSkillItem            UpdateStaffDevelopmentSkillItem(Master_StaffDevelopmentSkillItem data);
        bool                                        DeleteStaffDevelopmentSkillItem(Guid id);
    }

    public class StaffDevelopmentSkillItemRepository : BaseRepository<Master_StaffDevelopmentSkillItem, CentralDBEntities>,
                                                    IStaffDevelopmentSkillItemRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffDevelopmentSkillItemRepository(IUnitOfWork uwork) : base(uwork)
        {
            this._dbContext = new CentralDBEntities();
        }

        public List<Master_StaffDevelopmentSkillItem> GetStaffDevelopmentSkillItemList()
        {
            return this._dbContext.Master_StaffDevelopmentSkillItem.Where(s=>s.IsDeleted==false).ToList();
        }

        public Master_StaffDevelopmentSkillItem InsertStaffDevelopmentSkillItem(Master_StaffDevelopmentSkillItem data)
        {
            Master_StaffDevelopmentSkillItem result;
            result = this._dbContext.Master_StaffDevelopmentSkillItem.Add(data);
            this._dbContext.SaveChanges();
            this._dbContext.Entry(data).State = EntityState.Detached;
            this._dbContext.Master_StaffDevelopmentSkillItem.Find(data.ID);
            return result;
        }

        public Master_StaffDevelopmentSkillItem UpdateStaffDevelopmentSkillItem(Master_StaffDevelopmentSkillItem data)
        {
            Master_StaffDevelopmentSkillItem localData;
            localData = this._dbContext.Master_StaffDevelopmentSkillItem.Local.FirstOrDefault(item => item.ID == data.ID);
            if (localData != null)
            {
                this._dbContext.Entry(localData).State = EntityState.Detached;
            }
            var entry = this._dbContext.Entry(data);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            this._dbContext.Entry(data).State = EntityState.Detached;
            this._dbContext.Master_StaffDevelopmentSkillItem.Find(data.ID);
            return data;
        }

        public bool DeleteStaffDevelopmentSkillItem(Guid ID)
        {
            bool result;
            Master_StaffDevelopmentSkillItem data;
            List<Master_StaffDevelopmentSkillItem> datalist;

            datalist = this._dbContext.Master_StaffDevelopmentSkillItem.ToList();
            data = datalist.Find(item => item.ID == ID);

            if (data != null)
            {
                this._dbContext.Master_StaffDevelopmentSkillItem.Remove(data);
                this._dbContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }


        protected void Dispose(bool disposing)
        {
            if (disposing == false)
                return;
            if (this._dbContext == null)
                return;
            this._dbContext.Dispose();
            this._dbContext = null;
            return;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}
