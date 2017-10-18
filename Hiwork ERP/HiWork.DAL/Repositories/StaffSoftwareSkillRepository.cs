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
    public partial interface IStaffSoftwareRepository: IBaseRepository<Staff_SoftwareSkill>
    {
        Staff_SoftwareSkill InsertStaffSkill(Staff_SoftwareSkill software);
        Staff_SoftwareSkill UpdateStaffSkill(Staff_SoftwareSkill recordData);
        List<Staff_SoftwareSkill> GetStaffSoftware();


    }
  public  class StaffSoftwareSkillRepository: BaseRepository<Staff_SoftwareSkill,CentralDBEntities>, IStaffSoftwareRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffSoftwareSkillRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

      public Staff_SoftwareSkill InsertStaffSkill(Staff_SoftwareSkill software)
        {
            try
            {
                _dbContext.Staff_SoftwareSkill.Add(software);

              
                   _dbContext.SaveChanges();
                   return software;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
      public  Staff_SoftwareSkill UpdateStaffSkill(Staff_SoftwareSkill recordData)
        {

            var entry = _dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
            return recordData;


        }
       public List<Staff_SoftwareSkill> GetStaffSoftware()
        {
            return _dbContext.Staff_SoftwareSkill.ToList();
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
