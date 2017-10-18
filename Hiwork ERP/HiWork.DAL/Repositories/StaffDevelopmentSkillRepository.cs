

/* ******************************************************************************************************************
 * Repository for Master_StaffDevelopmentSkill Entity
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

    public interface IStaffDevelopmentSkillRepository : IBaseRepository<Master_StaffDevelopmentSkill>
    {
        List<Master_StaffDevelopmentSkill>          GetStaffDevelopmentSkillList();
        Master_StaffDevelopmentSkill                InsertStaffDevelopmentSkill(Master_StaffDevelopmentSkill data);
        Master_StaffDevelopmentSkill                UpdateStaffDevelopmentSkill(Master_StaffDevelopmentSkill data);
        bool                                        DeleteStaffDevelopmentSkill(Guid id);
    }

    public class StaffDevelopmentSkillRepository : BaseRepository<Master_StaffDevelopmentSkill, CentralDBEntities>,
                                                    IStaffDevelopmentSkillRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffDevelopmentSkillRepository(IUnitOfWork uwork) : base(uwork)
        {
            this._dbContext = new CentralDBEntities();
        }

        public List<Master_StaffDevelopmentSkill> GetStaffDevelopmentSkillList()
        {
            return this._dbContext.Master_StaffDevelopmentSkill.Where(s=>s.IsDeleted==false).ToList();
        }

        public Master_StaffDevelopmentSkill InsertStaffDevelopmentSkill(Master_StaffDevelopmentSkill data)
        {
            Master_StaffDevelopmentSkill result;
            result = this._dbContext.Master_StaffDevelopmentSkill.Add(data);
            this._dbContext.SaveChanges();
            return result;
        }

        public Master_StaffDevelopmentSkill UpdateStaffDevelopmentSkill(Master_StaffDevelopmentSkill data)
        {
            Master_StaffDevelopmentSkill localData;
            localData = this._dbContext.Master_StaffDevelopmentSkill.Local.FirstOrDefault(item => item.ID == data.ID);
            if (localData != null)
            {
                this._dbContext.Entry(localData).State = EntityState.Detached;
            }
            var entry = this._dbContext.Entry(data);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            return data;
        }

        public bool DeleteStaffDevelopmentSkill(Guid ID)
        {
            bool result;
            Master_StaffDevelopmentSkill data;
            List<Master_StaffDevelopmentSkill> datalist;

            datalist = this._dbContext.Master_StaffDevelopmentSkill.ToList();
            data = datalist.Find(item => item.ID == ID);

            if (data != null)
            {
                this._dbContext.Master_StaffDevelopmentSkill.Remove(data);
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
