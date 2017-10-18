

/* ******************************************************************************************************************
 * Repository for Master_StaffType Entity
 * Programmed by    :   Ashis Kr. Das (b-Bd_12 Ashis)
 * Date             :   13-Jun-2017
 * *****************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Database;

namespace HiWork.DAL.Repositories
{
    public interface IStaffTypeRepository : IBaseRepository<Master_StaffType>
    {
        List<Master_StaffType>                          GetStaffTypeList();
        Master_StaffType                                InsertStaffType(Master_StaffType stype);
        Master_StaffType                                UpdateStaffType(Master_StaffType stype);
        bool                                            DeleteStaffType(int ID);
    }

    public class StaffTypeRepository : BaseRepository<Master_StaffType, CentralDBEntities>,
                                                IStaffTypeRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffTypeRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_StaffType> GetStaffTypeList()
        {
            return _dbContext.Master_StaffType.Where(s=>s.IsDeleted==false).ToList();
        }

        public Master_StaffType InsertStaffType(Master_StaffType stype)
        {
            Master_StaffType result;
            result = _dbContext.Master_StaffType.Add(stype);
            _dbContext.SaveChanges();
            return result;
        }

        public Master_StaffType UpdateStaffType(Master_StaffType stype)
        {
            var entry = _dbContext.Entry(stype);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            return stype;
        }

        public bool DeleteStaffType(int ID)
        {
            bool result;
            Master_StaffType targetType;
            List<Master_StaffType> listTypes;

            listTypes = _dbContext.Master_StaffType.ToList();
            targetType = listTypes.Find(type => type.ID == ID);

            if (targetType != null)
            {
                _dbContext.Master_StaffType.Remove(targetType);
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
