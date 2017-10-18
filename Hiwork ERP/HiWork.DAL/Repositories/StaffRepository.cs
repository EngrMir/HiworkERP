
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.Utils;

namespace HiWork.DAL.Repositories
{
    public interface IStaffRepository : IBaseRepository<Staff>
    {
        List<Staff>   GetStaffList();
        Staff    GetStaffById(Guid staffId);
        Staff InsertStaff(Staff staff);
        Staff UpdateStaff(Staff staff);
        bool   DeleteStaff(Guid ID);

        Staff GetTranslatorByUser(string email, string password);
        List<Staff> GetStaffSearchList(string srcLanguageID, string targetLanguageID, string specialFieldID);
        long GetStaffNextRegistrationID(BaseViewModel model);
        bool WithdrawMembeship(Staff model);
    }
    public class StaffRepository : BaseRepository<Staff, CentralDBEntities>, IStaffRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Staff> GetStaffList()
        {
            return _dbContext.Staffs.ToList();
        }

        public Staff GetStaffById(Guid staffId)
        {
            return _dbContext.Staffs.FirstOrDefault(s => s.ID == staffId);
        }

        public long GetStaffNextRegistrationID(BaseViewModel model)
        {
            return _dbContext.Staffs.ToList().Where(c => c.ApplicationID == model.ApplicationId).LastOrDefault().RegistrationID;
        }

        public Staff GetTranslatorByUser(string email, string password)
        {
            var data = from t in _dbContext.Staffs
                       where (t.StaffEmailID != null)
                       select t;
            return data.ToList().Find(f => (f.StaffEmailID.Trim() == email.Trim() && f.Password == Utility.MD5(password)) && f.IsDeleted == false);

        }

        public Staff InsertStaff(Staff staff)
        {
            Staff result;
            result = _dbContext.Staffs.Add(staff);
            _dbContext.SaveChanges();
            return result;
        }
        public List<Staff> GetStaffSearchList(string srcLanguageID, string targetLanguageID, string specialFieldID)
        {
            if (specialFieldID == "0" && srcLanguageID == "0" && targetLanguageID == "0")
            {
                var all = (from r in _dbContext.Staffs.ToList()
                           where r.StaffTypeID == (int)StaffType.Translator
                           select r).ToList();
                return all;
            }
            else if(specialFieldID == "0" && srcLanguageID == "0" && targetLanguageID != "0")
            {
                var result = (from r in _dbContext.Staffs.ToList()
                              where r.NativeLanguageID == Guid.Parse(targetLanguageID) &&  r.StaffTypeID == (int)StaffType.Translator
                              select r).ToList();
                return result;
            }
            else
            {
                var result = (from r in _dbContext.Staffs.ToList()
                              where r.NativeLanguageID == Guid.Parse(targetLanguageID) && r.ForiegnLanguage1ID == Guid.Parse(srcLanguageID) && r.StaffTypeID == (int)StaffType.Translator
                              select r).ToList();
                return result;
            }
            
        }
        public Staff UpdateStaff(Staff staff)
        {
            var entry = _dbContext.Entry(staff);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            return staff;

        }
        public bool DeleteStaff(Guid ID)
        {
            bool result;
            Staff targetType;
            List<Staff> dataList;

            dataList = _dbContext.Staffs.ToList();
            targetType = dataList.Find(item => item.ID == ID);

            if (targetType != null)
            {
                _dbContext.Staffs.Remove(targetType);
                _dbContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool WithdrawMembeship(Staff model)
        {
            var staff = GetTranslatorByUser(model.StaffEmailID, model.Password);

            if (staff!=null)
            {
                staff.IsDeleted = true;
                _dbContext.Entry(staff).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return true;
            }

            return false;


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
