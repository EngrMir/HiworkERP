

/* ******************************************************************************************************************
 * Repository for Master_StaffTechnicalSkillItems Entity
 * Date             :   09-Jun-2017
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
    public interface ITechnicalSkillItemsRepository : IBaseRepository<Master_StaffTechnicalSkillItems>
    {
        List<Master_StaffTechnicalSkillItems>           GetItemList();
        Master_StaffTechnicalSkillItems                 InsertItem(Master_StaffTechnicalSkillItems anItem);
        Master_StaffTechnicalSkillItems                 UpdateItem(Master_StaffTechnicalSkillItems anItem);
        bool                                            DeleteItem(Guid id);
    }

    public class TechnicalSkillItemsRepository : BaseRepository<Master_StaffTechnicalSkillItems, CentralDBEntities>,
                                                    ITechnicalSkillItemsRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public TechnicalSkillItemsRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_StaffTechnicalSkillItems> GetItemList()
        {
            return _dbContext.Master_StaffTechnicalSkillItems.ToList();
        }

        public Master_StaffTechnicalSkillItems InsertItem(Master_StaffTechnicalSkillItems anItem)
        {
            Master_StaffTechnicalSkillItems result;
            result = _dbContext.Master_StaffTechnicalSkillItems.Add(anItem);
            _dbContext.SaveChanges();
            this._dbContext.Entry(anItem).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(anItem.ID);
            return result;
        }

        public Master_StaffTechnicalSkillItems UpdateItem(Master_StaffTechnicalSkillItems anItem)
        {
            var entity = _dbContext.Entry(anItem);
            entity.State = EntityState.Modified;
            _dbContext.SaveChanges();
            this._dbContext.Entry(anItem).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(anItem.ID);
            return anItem;
        }

        public bool DeleteItem(Guid id)
        {
            bool result;
            Master_StaffTechnicalSkillItems targetItem;
            List<Master_StaffTechnicalSkillItems> itemList;

            itemList = _dbContext.Master_StaffTechnicalSkillItems.ToList();
            targetItem = itemList.Find(item => item.ID == id);

            if (targetItem != null)
            {
                _dbContext.Master_StaffTechnicalSkillItems.Remove(targetItem);
                _dbContext.SaveChanges();
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
