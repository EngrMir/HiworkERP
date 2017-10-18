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
    public partial interface IProfessionRepository : IBaseRepository<Master_StaffProfession>
    {
        // string GetNextUserTypeId();
        List<Master_StaffProfession> GetAllProfessionList();
        Master_StaffProfession GetProfession(Guid Id);
        Master_StaffProfession InsertProfession(Master_StaffProfession profession);
        Master_StaffProfession UpdateProfession(Master_StaffProfession profession);
        bool DeleteProfession(Guid Id);
    }
    public class ProfessionRepository : BaseRepository<Master_StaffProfession, CentralDBEntities>, IProfessionRepository
    {
        private readonly CentralDBEntities _dbContext;

        public ProfessionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteProfession(Guid Id)
        {
            try
            {
                var profession = _dbContext.Master_StaffProfession.ToList().Find(f => f.ID == Id);
                if (profession != null)
                {
                    _dbContext.Master_StaffProfession.Remove(profession);
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

        public List<Master_StaffProfession> GetAllProfessionList()
        {
            try
            {
                return _dbContext.Master_StaffProfession.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffProfession GetProfession(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffProfession.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffProfession InsertProfession(Master_StaffProfession profession)
        {
            try
            {
                profession.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffProfession.Add(profession);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffProfession UpdateProfession(Master_StaffProfession profession)
        {
            try
            {
                _dbContext.Entry(profession).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return profession;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}