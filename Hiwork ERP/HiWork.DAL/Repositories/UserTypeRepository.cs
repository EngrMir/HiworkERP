using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;

namespace HiWork.DAL.Repositories
{
    public partial interface IUserTypeRepository : IBaseRepository<UserType>
    {
       // string GetNextUserTypeId();
        List<UserType> GetAllUserTypeList();
        UserType GetUserType(Int64 userTypeId);
        UserType InsertUserType(UserType userType);
        UserType UpdateUserType(UserType userType);
        int DeleteUserType(long userTypeId);
    }

    public class UserTypeRepository : BaseRepository<UserType, CentralDBEntities>, IUserTypeRepository, IDisposable
    {
        private  CentralDBEntities _dbContext;

        public UserTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        //public string GetNextUserTypeId()
        //{
        //    var query = new StringBuilder();
        //    query.Append("EXEC [dbo].[USP_GetNextId] 'UserTypeId', 'UserTypeIdLength'");

        //    var finalQuery = query.ToString().Contains("''")
        //        ? query.ToString().Replace("''", "NULL")
        //        : query.ToString();
        //    var result = _dbContext.Database.SqlQuery<USP_GetNextId_Result>(finalQuery).FirstOrDefault();
        //    return result.Id;
        //}

        public List<UserType> GetAllUserTypeList()
        {
            return _dbContext.UserTypes.ToList();
        }

        public UserType GetUserType(Int64 userTypeId)
        {
            return _dbContext.UserTypes.FirstOrDefault(u => u.Id == userTypeId);
        }

        public UserType InsertUserType(UserType userType)
        {
            var result = _dbContext.UserTypes.Add(userType);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            return result;
        }

        public UserType UpdateUserType(UserType userType)
        {
            _dbContext.Entry(userType).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return userType;
        }

        public int DeleteUserType(long userTypeId)
        {
            string query = "EXEC [dbo].[USP_DeleteUserType] " + userTypeId + "";
            return _dbContext.Database.ExecuteSqlCommand(query);
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
