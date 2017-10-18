

/* ******************************************************************************************************************
 * Repository for Staff_BankAccoountInfo Entity
 * Programmed by    :   Ashis Kr. Das (b-Bd_12 Ashis)
 * Date             :   14-July-2017
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
    public interface IStaffBankAccountInfoRepository : IBaseRepository<Staff_BankAccountInfo>
    {
        List<Staff_BankAccountInfo> GetStaffBankAccountInfoList();
        Staff_BankAccountInfo InsertStaffBankAccountInfo(Staff_BankAccountInfo stype);
        Staff_BankAccountInfo UpdateStaffBankAccountInfo(Staff_BankAccountInfo stype);
        bool DeleteStaffBankAccountInfo(Guid ID);
    }

    public class StaffBankAccountInfoRepository : BaseRepository<Staff_BankAccountInfo, CentralDBEntities>,
                                                IStaffBankAccountInfoRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffBankAccountInfoRepository(IUnitOfWork uWork) : base(uWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Staff_BankAccountInfo> GetStaffBankAccountInfoList()
        {
            return _dbContext.Staff_BankAccountInfo.ToList();
        }

        public Staff_BankAccountInfo InsertStaffBankAccountInfo(Staff_BankAccountInfo stype)
        {
            Staff_BankAccountInfo result;
            result = _dbContext.Staff_BankAccountInfo.Add(stype);
            _dbContext.SaveChanges();
            return result;
        }

        public Staff_BankAccountInfo UpdateStaffBankAccountInfo(Staff_BankAccountInfo stype)
        {
            var entry = _dbContext.Entry(stype);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            return stype;
        }

        public bool DeleteStaffBankAccountInfo(Guid ID)
        {
            bool result;
            Staff_BankAccountInfo targetType;
            List<Staff_BankAccountInfo> listTypes;

            listTypes = _dbContext.Staff_BankAccountInfo.ToList();
            targetType = listTypes.Find(type => type.ID == ID);

            if (targetType != null)
            {
                _dbContext.Staff_BankAccountInfo.Remove(targetType);
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
