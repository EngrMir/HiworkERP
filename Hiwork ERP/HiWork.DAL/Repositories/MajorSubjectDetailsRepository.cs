using HiWork.DAL.Database;
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
   public partial interface IMajorSubjectDetailsRepository:IBaseRepository<Master_StaffMajorSubjectDetails>
    {
        List<Master_StaffMajorSubjectDetails> GetAllMajorSubjectDetailsList();

        //Master_StaffMajorSubject GetMajorSubjectDetails(Guid msmsdId);
        Master_StaffMajorSubjectDetails InsertMajorSubjectDetails(Master_StaffMajorSubjectDetails msmsd);


        Master_StaffMajorSubjectDetails UpdateMajorSubjectDetails(Master_StaffMajorSubjectDetails msmsd);

        bool DeleteMajorSubjectDetails(Guid msmsdId);

    }

    public class MajorSubjectDetailsRepository : BaseRepository<Master_StaffMajorSubjectDetails, CentralDBEntities>, IMajorSubjectDetailsRepository, IDisposable
    {

        private CentralDBEntities _dbContext;

        public MajorSubjectDetailsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<Master_StaffMajorSubjectDetails> GetAllMajorSubjectDetailsList()
        {
            return _dbContext.Master_StaffMajorSubjectDetails.ToList();

        }
        public Master_StaffMajorSubjectDetails InsertMajorSubjectDetails(Master_StaffMajorSubjectDetails msmsd)
        {

            var result = _dbContext.Master_StaffMajorSubjectDetails.Add(msmsd);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_StaffMajorSubjectDetails UpdateMajorSubjectDetails(Master_StaffMajorSubjectDetails msmsd)
        {

            _dbContext.Entry(msmsd).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return msmsd;



        }

        public bool DeleteMajorSubjectDetails(Guid msmsdId)
        {

            var Msm = _dbContext.Master_StaffMajorSubjectDetails.ToList().Find(m => m.ID == msmsdId);
            if (Msm != null)
            {
                _dbContext.Master_StaffMajorSubjectDetails.Remove(Msm);
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
