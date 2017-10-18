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
    public partial interface IStaffProfessioanlSpecialityRepository : IBaseRepository<Staff_ProfessionalSpeciality>
    {

        Staff_ProfessionalSpeciality InsertStaffProfessinalSpeciality(Staff_ProfessionalSpeciality staffprofesionalspecial);

        Staff_ProfessionalSpeciality UpdateStaffProfessinalSpeciality(Staff_ProfessionalSpeciality staffprofesionalspecial);

        List<Staff_ProfessionalSpeciality> GetAllStaffProfessinalSpecialityList();
    }

    public class StaffProfessioanlSpecialityRepository:BaseRepository<Staff_ProfessionalSpeciality, CentralDBEntities>,IStaffProfessioanlSpecialityRepository
    {
        private readonly CentralDBEntities _dbContext;

        public StaffProfessioanlSpecialityRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public Staff_ProfessionalSpeciality InsertStaffProfessinalSpeciality(Staff_ProfessionalSpeciality staffprofesionalspecial)
        {
            try
            {
                var result = _dbContext.Staff_ProfessionalSpeciality.Add(staffprofesionalspecial);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Staff_ProfessionalSpeciality UpdateStaffProfessinalSpeciality(Staff_ProfessionalSpeciality staffprofesionalspecial)
        {
            try
            {
                _dbContext.Entry(staffprofesionalspecial).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return staffprofesionalspecial;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Staff_ProfessionalSpeciality> GetAllStaffProfessinalSpecialityList()
        {
            try
            {
                return _dbContext.Staff_ProfessionalSpeciality.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
