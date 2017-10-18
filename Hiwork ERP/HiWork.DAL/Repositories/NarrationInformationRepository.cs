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
       public partial interface INarrationInformationRepository : IBaseRepository<Staff_NarrationInformation>
    {
        List<Staff_NarrationInformation> GetAllNarrationInformationList();
        Staff_NarrationInformation GetNarrationInformation(Guid Id);
        Staff_NarrationInformation InsertNarrationInformation(Staff_NarrationInformation userNarrationInformation);
        Staff_NarrationInformation UpdateNarrationInformation(Staff_NarrationInformation userNarrationInformation);
        bool DeleteNarrationInformation(Guid Id);
    }

    public class NarrationInformationRepository : BaseRepository<Staff_NarrationInformation, CentralDBEntities>, INarrationInformationRepository
    {
        private readonly CentralDBEntities _dbContext;

        public NarrationInformationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteNarrationInformation(Guid Id)
        {
            try
            {
                var user = _dbContext.Staff_NarrationInformation.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Staff_NarrationInformation.Remove(user);
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

        public List<Staff_NarrationInformation> GetAllNarrationInformationList()
        {
            try
            {
                return _dbContext.Staff_NarrationInformation.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_NarrationInformation GetNarrationInformation(Guid Id)
        {
            try
            {
                return _dbContext.Staff_NarrationInformation.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_NarrationInformation InsertNarrationInformation(Staff_NarrationInformation NarrationInformation)
        {
            try
            {
                NarrationInformation.ID = Guid.NewGuid();
                var result = _dbContext.Staff_NarrationInformation.Add(NarrationInformation);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_NarrationInformation UpdateNarrationInformation(Staff_NarrationInformation userNarrationInformation)
        {
            try
            {
                _dbContext.Entry(userNarrationInformation).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userNarrationInformation;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
