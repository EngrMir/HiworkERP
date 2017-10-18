using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public partial interface IUserInRoleRepository : IBaseRepository<UserInRole>
    {
        //List<UserInRole> GetAllUserInRoleList();

        //UserInRole GetUserInRole(Guid userinroleId);

        //UserInRole InsertUserInRole(UserInRole userinroleId);

        //UserInRole UpdateUserInRole(UserInRole userinroleId);

        //bool DeleteUserInRole(Guid userinroleId);
    }


    public class UserInRoleRepository : BaseRepository<UserInRole, CentralDBEntities>, IUserInRoleRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public UserInRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        //public List<UserInRole> GetAllUserInRoleList()
        //{
        //    return _dbContext.UserInRole.ToList();


        //}

        //public UserInRole GetUserInRole(Guid userinroleId)
        //{

        //    return _dbContext.UserInRole.FirstOrDefault(u => u.ID == userinroleId);

        //}

        //public UserInRole InsertUserInRole(UserInRole userinroleId)
        //{

        //    var result = _dbContext.UserInRole.Add(userinrole);
        //    _dbContext.SaveChanges();

        //    return result;
        //}

        //public UserInRole UpdateUserInRole(UserInRole userinroleId)
        //{

        //    _dbContext.Entry(userinroleId).State = EntityState.Modified;
        //    _dbContext.SaveChanges();
        //    return userinroleId;



        //}

        //public bool DeleteUserInRole(Guid userinroleId)
        //{

        //    var UserInRole = _dbContext.UserInRole.ToList().Find(u => u.ID == userinroleId);
        //    if (UserInRole != null)
        //    {
        //        _dbContext.UserInRole.Remove(UserInRole);
        //        _dbContext.SaveChanges();

        //        return true;
        //    }

        //    return false;
        //}

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //Garbase collector
        }
    }
}
