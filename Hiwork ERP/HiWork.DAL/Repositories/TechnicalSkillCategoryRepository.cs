

/* ******************************************************************************************************************
 * Repository for Master_StaffTechnicalSkillCategory Entity
 * Date             :   08-Jun-2017
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

    public interface ITechnicalSkillCategoryRepository : IBaseRepository<Master_StaffTechnicalSkillCategory>
    {
        List<Master_StaffTechnicalSkillCategory>        GetCategoryList();
        Master_StaffTechnicalSkillCategory              InsertCategory(Master_StaffTechnicalSkillCategory category);
        Master_StaffTechnicalSkillCategory              UpdateCategory(Master_StaffTechnicalSkillCategory category);
        bool                                            DeleteCategory(Guid ID);
    }

    public class TechnicalSkillCategoryRepository : BaseRepository<Master_StaffTechnicalSkillCategory, CentralDBEntities>,
                                                ITechnicalSkillCategoryRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public TechnicalSkillCategoryRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_StaffTechnicalSkillCategory> GetCategoryList()
        {
            return _dbContext.Master_StaffTechnicalSkillCategory.ToList();
        }

        public Master_StaffTechnicalSkillCategory InsertCategory(Master_StaffTechnicalSkillCategory category)
        {
            Master_StaffTechnicalSkillCategory result;
            result = _dbContext.Master_StaffTechnicalSkillCategory.Add(category);
            _dbContext.SaveChanges();
            return result;
        }

        public Master_StaffTechnicalSkillCategory UpdateCategory(Master_StaffTechnicalSkillCategory category)
        {
            var entry = _dbContext.Entry(category);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            return category;
        }

        public bool DeleteCategory(Guid ID)
        {
            bool result;
            Master_StaffTechnicalSkillCategory targetCategory;
            List<Master_StaffTechnicalSkillCategory> listCategories;

            listCategories = _dbContext.Master_StaffTechnicalSkillCategory.ToList();
            targetCategory = listCategories.Find(d => d.ID == ID);

            if (targetCategory != null)
            {
                _dbContext.Master_StaffTechnicalSkillCategory.Remove(targetCategory);
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
