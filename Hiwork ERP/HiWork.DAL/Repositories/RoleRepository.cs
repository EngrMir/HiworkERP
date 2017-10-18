using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace HiWork.DAL.Repositories
{
    public partial interface IRoleRepository : IBaseRepository<Role>
    {
        List<Role> GetAllUserRoleList();
        Role GetUserRole(Int64 roleId);
        Role InsertUserRole(Role userRole);
        Role UpdateUserRole(Role userRole);
        bool DeleteUserRole(long id);
    }
    public class RoleRepository : BaseRepository<Role, CentralDBEntities>, IRoleRepository,IDisposable
    {
        private  CentralDBEntities _dbContext;
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

     
        public List<Role> GetAllUserRoleList()
        {
            return _dbContext.Roles.ToList();         
        }

        public Role GetUserRole(long roleId)
        {
            return _dbContext.Roles.FirstOrDefault(u => u.Id == roleId);          
        }

        public Role InsertUserRole(Role userRole)
        {
            var result = _dbContext.Roles.Add(userRole);
            _dbContext.SaveChanges();

            return result;
          
        }

        public Role UpdateUserRole(Role userRole)
        {
            _dbContext.Entry(userRole).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return userRole;
          
        }
        public bool DeleteUserRole(long roleId)
        {
            var user = _dbContext.Roles.ToList().Find(f => f.Id == roleId);
            if (user != null)
            {
                _dbContext.Roles.Remove(user);
                _dbContext.SaveChanges();

                return true;
            }
           
            return false;
        }

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
