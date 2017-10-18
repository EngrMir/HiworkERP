

/* ******************************************************************************************************************
 * Repository for Master_StaffLanguageQualification Entity
 * Date             :   14-Jun-2017
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

    public interface ILanguageQualificationRepository : IBaseRepository<Master_StaffLanguageQualifications>
    {
        List<Master_StaffLanguageQualifications> GetLanguageQualificationList();
        Master_StaffLanguageQualifications InsertLanguageQualification(Master_StaffLanguageQualifications recordData);
        Master_StaffLanguageQualifications UpdateLanguageQualification(Master_StaffLanguageQualifications recordData);
        bool DeleteLanguageQualification(Guid ID);
    }
    public class LanguageQualificationRepository : BaseRepository<Master_StaffLanguageQualifications, CentralDBEntities>,
                                                        ILanguageQualificationRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public LanguageQualificationRepository(IUnitOfWork uWork) : base(uWork)
        {
            this._dbContext = new CentralDBEntities();
        }

        public List<Master_StaffLanguageQualifications> GetLanguageQualificationList()
        {
            return this._dbContext.Master_StaffLanguageQualifications.ToList();
        }

        public Master_StaffLanguageQualifications InsertLanguageQualification(Master_StaffLanguageQualifications recordData)
        {
            Master_StaffLanguageQualifications result;
            result = this._dbContext.Master_StaffLanguageQualifications.Add(recordData);
            this._dbContext.SaveChanges();
            return result;
        }

        public Master_StaffLanguageQualifications UpdateLanguageQualification(Master_StaffLanguageQualifications recordData)
        {
            var entry = this._dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            return recordData;
        }

        public bool DeleteLanguageQualification(Guid ID)
        {
            bool result;
            Master_StaffLanguageQualifications trecordDataetModel;
            List<Master_StaffLanguageQualifications> dataList;

            dataList = this._dbContext.Master_StaffLanguageQualifications.ToList();
            trecordDataetModel = dataList.Find(item => item.ID == ID);

            if (trecordDataetModel != null)
            {
                this._dbContext.Master_StaffLanguageQualifications.Remove(trecordDataetModel);
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
