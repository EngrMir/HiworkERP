using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.DAL.Repositories
{ 
     public interface IStaffKnowledgeFieldItemRepository : IBaseRepository<Master_StaffKnowledgeFieldItem>
      {
        List<Master_StaffKnowledgeFieldItem> GetItemList();
        Master_StaffKnowledgeFieldItem InsertItem(Master_StaffKnowledgeFieldItem anItem);
        Master_StaffKnowledgeFieldItem UpdateItem(Master_StaffKnowledgeFieldItem anItem);
        bool DeleteItem(Guid id);
      }

    public class StaffKnowledgeFieldItemRepository : BaseRepository<Master_StaffKnowledgeFieldItem, CentralDBEntities>,
                                                    IStaffKnowledgeFieldItemRepository, IDisposable
    {
        private CentralDBEntities _dbContext;

        public StaffKnowledgeFieldItemRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_StaffKnowledgeFieldItem> GetItemList()
        {
            return _dbContext.Master_StaffKnowledgeFieldItem.Where(s=>s.IsDeleted==false).ToList();
        }

        public Master_StaffKnowledgeFieldItem InsertItem(Master_StaffKnowledgeFieldItem anItem)
        {
            Master_StaffKnowledgeFieldItem result;
            result = _dbContext.Master_StaffKnowledgeFieldItem.Add(anItem);
            _dbContext.SaveChanges();
            this._dbContext.Entry(anItem).State = EntityState.Detached;
            this._dbContext.Master_CompanyIndustryClassificationItem.Find(anItem.ID);
            return result;
        }

        public Master_StaffKnowledgeFieldItem UpdateItem(Master_StaffKnowledgeFieldItem anItem)
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
            Master_StaffKnowledgeFieldItem targetItem;
            List<Master_StaffKnowledgeFieldItem> itemList;

            itemList = _dbContext.Master_StaffKnowledgeFieldItem.ToList();
            targetItem = itemList.Find(item => item.ID == id);

            if (targetItem != null)
            {
                _dbContext.Master_StaffKnowledgeFieldItem.Remove(targetItem);
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
