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
    public partial interface IEducationRepository : IBaseRepository<Master_StaffEducationalDegree>
    {
        List<Master_StaffEducationalDegree> GetAllUserEducationList();
        Master_StaffEducationalDegree GetUserEducation(Guid EducationId);
        Master_StaffEducationalDegree InsertUserEducation(Master_StaffEducationalDegree userEducation);
        Master_StaffEducationalDegree UpdateUserEducation(Master_StaffEducationalDegree userEducation);
        bool DeleteUserEducation(Guid id);
        Master_StaffEducationalDegree GeEducationById(Guid? EducationId);
    }

    public class EducationRepository : BaseRepository<Master_StaffEducationalDegree, CentralDBEntities>, IEducationRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public EducationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<Master_StaffEducationalDegree> GetAllUserEducationList()
        {
            return _dbContext.Master_StaffEducationalDegree.ToList();
        }

        public Master_StaffEducationalDegree GetUserEducation(Guid EducationId)
        {
            return _dbContext.Master_StaffEducationalDegree.FirstOrDefault(u => u.ID == EducationId);
        }
        public Master_StaffEducationalDegree GeEducationById(Guid? EducationId)
        {
            return _dbContext.Master_StaffEducationalDegree.FirstOrDefault(u => u.ID == EducationId);
        }
        public Master_StaffEducationalDegree InsertUserEducation(Master_StaffEducationalDegree userEducation)
        {
            var result = _dbContext.Master_StaffEducationalDegree.Add(userEducation);
            _dbContext.SaveChanges();

            return result;

        }

        public Master_StaffEducationalDegree UpdateUserEducation(Master_StaffEducationalDegree userEducation)
        {
            _dbContext.Entry(userEducation).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return userEducation;

        }
        public bool DeleteUserEducation(Guid EducationId)
        {
            var user = _dbContext.Master_StaffEducationalDegree.ToList().Find(f => f.ID == EducationId);
            if (user != null)
            {
                _dbContext.Master_StaffEducationalDegree.Remove(user);
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
