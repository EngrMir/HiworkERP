using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.Utils;

namespace HiWork.DAL.Repositories
{
    public partial interface IUserRepository : IBaseRepository<UserInformation>
    {
        // string GetNextUserTypeId();
        List<UserInformation> GetAllUserList();
        UserInformation GetUser(Int64 userId);
        UserInformation GetUserByUsername(string username,string password);
        UserInformation InsertUser(UserInformation user);
        UserInformation UpdateUser(UserInformation user);
        long DeleteUser(long userId);
        UserInformation GetUserBySession(string sessionId);
    }
    public class UserRepository : BaseRepository<UserInformation, CentralDBEntities>, IUserRepository
    {
        private readonly CentralDBEntities _dbContext;

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }
        public List<UserInformation> GetAllUserList()
        {
            try
            {
                return _dbContext.UserInformations.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserInformation GetUser(Int64 userId)
        {
            try
            {
                return _dbContext.UserInformations.FirstOrDefault(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserInformation GetUserBySession(string sessionId)
        {
            try
            {
                return _dbContext.UserInformations.FirstOrDefault(u => u.Session.ToString() == sessionId);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public UserInformation GetUserByUsername(string username, string password)
        {
            string encryptedPass = Utility.MD5(password);
            var user = _dbContext.UserInformations.Where(f => f.Username.Trim() == username.Trim() && f.Password.Trim() == encryptedPass.Trim()).FirstOrDefault();
            return user;
        }
        public UserInformation InsertUser(UserInformation user)
        {
            var result = _dbContext.UserInformations.Add(user);
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

        public UserInformation UpdateUser(UserInformation user)
        {
            try
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public long DeleteUser(long userTypeId)
        {
            //string query = "EXEC [dbo].[USP_DeleteUserType] " + userTypeId + "";
            //return _dbContext.Database.ExecuteSqlCommand(query);
            
            try
            {
                var user = _dbContext.UserInformations.ToList().Find(f => f.Id == userTypeId);
                if (user != null)
                {
                    _dbContext.UserInformations.Remove(user);
                    _dbContext.SaveChanges();
                }
                return user.Id;
            }
            catch (Exception ex)
            {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
        }
    }
}
