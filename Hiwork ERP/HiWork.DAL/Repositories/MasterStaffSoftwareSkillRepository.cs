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
    public partial interface IMasterStaffSoftwareSkillRepository :IBaseRepository<Master_StaffSoftwareSkill>
    {
        Master_StaffSoftwareSkill InsertStaffSoftwareSkill(Master_StaffSoftwareSkill staffsoftware);
        Master_StaffSoftwareSkill UpdateStaffSoftwareSkill(Master_StaffSoftwareSkill recordData);
        List<Master_StaffSoftwareSkill> GetStaffSoftwareSkill();

    }            



 public class MasterStaffSoftwareSkillRepository:BaseRepository<Master_StaffSoftwareSkill, CentralDBEntities>, IMasterStaffSoftwareSkillRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public MasterStaffSoftwareSkillRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }
        public Master_StaffSoftwareSkill InsertStaffSoftwareSkill(Master_StaffSoftwareSkill staffsoftware)
        {
            try
            {
                _dbContext.Master_StaffSoftwareSkill.Add(staffsoftware);
                _dbContext.SaveChanges();
                return staffsoftware;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       public Master_StaffSoftwareSkill UpdateStaffSoftwareSkill(Master_StaffSoftwareSkill recordData)
        {
            var entry = _dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            return recordData;
        }
      public  List<Master_StaffSoftwareSkill> GetStaffSoftwareSkill()
        {
            return _dbContext.Master_StaffSoftwareSkill.Where(s => s.IsDeleted == false).ToList();



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
