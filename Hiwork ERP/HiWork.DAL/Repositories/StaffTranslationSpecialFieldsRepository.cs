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
       public partial interface IStaffTranslationSpecialFieldsRepository : IBaseRepository<Staff_TranslationSpecialFields>
    {
        List<Staff_TranslationSpecialFields> GetAllStaffTranslationSpecialFieldsList();
        Staff_TranslationSpecialFields GetStaffTranslationSpecialFields(Guid Id);
        Staff_TranslationSpecialFields InsertStaffTranslationSpecialFields(Staff_TranslationSpecialFields userStaffTranslationSpecialFields);
        Staff_TranslationSpecialFields UpdateStaffTranslationSpecialFields(Staff_TranslationSpecialFields userStaffTranslationSpecialFields);
        bool DeleteStaffTranslationSpecialFields(Guid Id);
    }

    public class StaffTranslationSpecialFieldsRepository : BaseRepository<Staff_TranslationSpecialFields, CentralDBEntities>, IStaffTranslationSpecialFieldsRepository
    {
        private readonly CentralDBEntities _dbContext;
        public StaffTranslationSpecialFieldsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteStaffTranslationSpecialFields(Guid Id)
        {
            try
            {
                var user = _dbContext.Staff_TranslationSpecialFields.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Staff_TranslationSpecialFields.Remove(user);
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

        public List<Staff_TranslationSpecialFields> GetAllStaffTranslationSpecialFieldsList()
        {
            try
            {
                return _dbContext.Staff_TranslationSpecialFields.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TranslationSpecialFields GetStaffTranslationSpecialFields(Guid Id)
        {
            try
            {
                return _dbContext.Staff_TranslationSpecialFields.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TranslationSpecialFields InsertStaffTranslationSpecialFields(Staff_TranslationSpecialFields staffTranslationSpecialFields)
        {
            try
            {
                staffTranslationSpecialFields.ID = Guid.NewGuid();
                var result = _dbContext.Staff_TranslationSpecialFields.Add(staffTranslationSpecialFields);
                _dbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_TranslationSpecialFields UpdateStaffTranslationSpecialFields(Staff_TranslationSpecialFields userStaffTranslationSpecialFields)
        {
            try
            {
                _dbContext.Entry(userStaffTranslationSpecialFields).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userStaffTranslationSpecialFields;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
