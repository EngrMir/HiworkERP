using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
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
    public partial interface IMajorSubjectRepository : IBaseRepository<Master_StaffMajorSubject>
    {
        List<Master_StaffMajorSubject> GetAllMajorSubjectList();
        Master_StaffMajorSubject GetMajorSubject(Guid majorsubjectId);

        Master_StaffMajorSubject InsertMajorSubject(Master_StaffMajorSubject majorsubject);

        Master_StaffMajorSubject UpdateMajorSubject(Master_StaffMajorSubject majorsubject);

        bool DeleteMajorSubject(Guid majorsubjectId);
    }


    public class MajorSubjectRepository : BaseRepository<Master_StaffMajorSubject, CentralDBEntities>, IMajorSubjectRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public MajorSubjectRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_StaffMajorSubject> GetAllMajorSubjectList()
        {
            return _dbContext.Master_StaffMajorSubject.ToList();


        }

        public Master_StaffMajorSubject GetMajorSubject(Guid majorsubjectId)
        {

            return _dbContext.Master_StaffMajorSubject.FirstOrDefault(m => m.ID == majorsubjectId);

        }

        public Master_StaffMajorSubject InsertMajorSubject(Master_StaffMajorSubject majorsubject)
        {

            var result = _dbContext.Master_StaffMajorSubject.Add(majorsubject);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_StaffMajorSubject UpdateMajorSubject(Master_StaffMajorSubject majorsubject)
        {

            _dbContext.Entry(majorsubject).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return majorsubject;



        }

        public bool DeleteMajorSubject(Guid majorsubjectId)
        {

            var MajorSubject = _dbContext.Master_StaffMajorSubject.ToList().Find(m => m.ID == majorsubjectId);
            if (MajorSubject != null)
            {
                _dbContext.Master_StaffMajorSubject.Remove(MajorSubject);
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