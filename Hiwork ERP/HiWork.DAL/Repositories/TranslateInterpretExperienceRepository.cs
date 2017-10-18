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
   public partial interface ITranslateInterpretExperienceRepository : IBaseRepository<Staff_TranslateInterpretExperience>
    {
        List<Staff_TranslateInterpretExperience> GetAllTranslateInterpretExperienceList();
        Staff_TranslateInterpretExperience GetTranslateInterpretExperience(Guid Id);
        Staff_TranslateInterpretExperience InsertTranslateInterpretExperience(Staff_TranslateInterpretExperience userTranslateInterpretExperience);
        Staff_TranslateInterpretExperience UpdateTranslateInterpretExperience(Staff_TranslateInterpretExperience userTranslateInterpretExperience);
        bool DeleteTranslateInterpretExperience(Guid Id);
    }

    public class TranslateInterpretExperienceRepository : BaseRepository<Staff_TranslateInterpretExperience, CentralDBEntities>, ITranslateInterpretExperienceRepository
    {
        private readonly CentralDBEntities _dbContext;
        public TranslateInterpretExperienceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteTranslateInterpretExperience(Guid Id)
        {
            try
            {
                var user = _dbContext.Staff_TranslateInterpretExperience.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Staff_TranslateInterpretExperience.Remove(user);
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

        public List<Staff_TranslateInterpretExperience> GetAllTranslateInterpretExperienceList()
        {
            try
            {
                return _dbContext.Staff_TranslateInterpretExperience.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TranslateInterpretExperience GetTranslateInterpretExperience(Guid Id)
        {
            try
            {
                return _dbContext.Staff_TranslateInterpretExperience.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TranslateInterpretExperience InsertTranslateInterpretExperience(Staff_TranslateInterpretExperience TranslateInterpretExperience)
        {
            try
            {
                TranslateInterpretExperience.ID = Guid.NewGuid();
                var result = _dbContext.Staff_TranslateInterpretExperience.Add(TranslateInterpretExperience);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TranslateInterpretExperience UpdateTranslateInterpretExperience(Staff_TranslateInterpretExperience userTranslateInterpretExperience)
        {
            try
            {
                _dbContext.Entry(userTranslateInterpretExperience).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userTranslateInterpretExperience;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
