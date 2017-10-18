


using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HiWork.DAL.Repositories
{
    public partial interface IUserInfoRepository : IBaseRepository<UserInformation>
    {
        List<UserInformation> GetUserInformationList();
        UserInformation GetUserInformationById(long userId);
        UserInformation GetUserByEmployeeId(Guid ID);
        UserInformation InsertUserInformation(UserInformation user);
        UserInformation UpdateUserInformation(UserInformation user);
        bool DeleteUserInformation(long id);
    }

    public class UserInfoRepository : BaseRepository<UserInformation, CentralDBEntities>, IUserInfoRepository, IDisposable
    {

        private CentralDBEntities _dbContext;
        public UserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<UserInformation> GetUserInformationList()
        {
            return _dbContext.UserInformations.Where(u=>u.IsDeleted != true).ToList();
        }

        public UserInformation GetUserInformationById(long userId)
        {
            return _dbContext.UserInformations.FirstOrDefault(u => u.Id == userId);
        }

        public UserInformation InsertUserInformation(UserInformation user)
        {
            UserInformation userdata;
            userdata = _dbContext.UserInformations.Add(user);
            _dbContext.SaveChanges();
            return userdata;
        }

        public UserInformation UpdateUserInformation(UserInformation user)
        {
            UserInformation localData;
            localData = _dbContext.UserInformations.Local.FirstOrDefault(item => item.Id == user.Id);
            if (localData != null)
            {
                _dbContext.Entry(localData).State = EntityState.Detached;
            }
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return user;
        }

        public UserInformation GetUserByEmployeeId(Guid ID)
        {
            return _dbContext.UserInformations.ToList().Where(u => u.EmployeeID == ID).SingleOrDefault(); ;
        }

        public bool DeleteUserInformation(long id)
        {
            bool result;
            UserInformation userdata;
            List<UserInformation> datalist;

            datalist = _dbContext.UserInformations.ToList();
            userdata = datalist.Find(item => item.Id == id);

            if (userdata != null)
            {
                userdata.IsDeleted = true;
                _dbContext.Entry(userdata).State = EntityState.Modified;
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
