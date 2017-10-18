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
    public partial interface IStaffCurrentStateRepository : IBaseRepository<Staff_CurrentStates>
    {

        Staff_CurrentStates InsertStaffCurrentState(Staff_CurrentStates staffcurrentstate);
        Staff_CurrentStates UpdateStaffCurrentState(Staff_CurrentStates staffcurrentstate);
        List<Staff_CurrentStates> GetAllStaffCurrentStateList();
    }
    
    public class StaffCurrentStateRepository : BaseRepository<Staff_CurrentStates, CentralDBEntities>, IStaffCurrentStateRepository
    {
        private readonly CentralDBEntities _dbContext;

        public StaffCurrentStateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public Staff_CurrentStates InsertStaffCurrentState(Staff_CurrentStates staffcurrentstate)
        {
            try
            {
                var result = _dbContext.Staff_CurrentStates.Add(staffcurrentstate);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Staff_CurrentStates UpdateStaffCurrentState(Staff_CurrentStates staffcurrentstate)
        {
            try
            {
                _dbContext.Entry(staffcurrentstate).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return staffcurrentstate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Staff_CurrentStates> GetAllStaffCurrentStateList()
        {
            try
            {
                return _dbContext.Staff_CurrentStates.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
