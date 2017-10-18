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
        public partial interface ITransproInformationRepository : IBaseRepository<Staff_TransproInformation>
    {
        List<Staff_TransproInformation> GetAllTransproInformationList();
        Staff_TransproInformation GetTransproInformation(Guid Id);
        Staff_TransproInformation InsertTransproInformation(Staff_TransproInformation userTransproInformation);
        Staff_TransproInformation UpdateTransproInformation(Staff_TransproInformation userTransproInformation);
        bool DeleteTransproInformation(Guid Id);
    }

    public class TransproInformationRepository : BaseRepository<Staff_TransproInformation, CentralDBEntities>, ITransproInformationRepository
    {
        private readonly CentralDBEntities _dbContext;
        public TransproInformationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteTransproInformation(Guid Id)
        {
            try
            {
                var user = _dbContext.Staff_TransproInformation.Find(Id);
                if (user != null)
                {
                    _dbContext.Staff_TransproInformation.Remove(user);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                string message;
                message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                throw new Exception(message);
            }
            return false;
        }

        public List<Staff_TransproInformation> GetAllTransproInformationList()
        {
            try
            {
                return _dbContext.Staff_TransproInformation.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TransproInformation GetTransproInformation(Guid Id)
        {
            try
            {
                return _dbContext.Staff_TransproInformation.Find(Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TransproInformation InsertTransproInformation(Staff_TransproInformation TransproInformation)
        {
            try
            {
                TransproInformation.ID = Guid.NewGuid();
                var result = _dbContext.Staff_TransproInformation.Add(TransproInformation);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TransproInformation UpdateTransproInformation(Staff_TransproInformation userTransproInformation)
        {
            try
            {
                _dbContext.Entry(userTransproInformation).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userTransproInformation;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
