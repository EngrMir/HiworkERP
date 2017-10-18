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
    public partial interface IBranchRepository : IBaseRepository<Master_BranchOffice>
    {
        List<Master_BranchOffice> GetAllBranchList();

        Master_BranchOffice GetBranch(Guid branchId);

        Master_BranchOffice InsertBranch(Master_BranchOffice branch);

        Master_BranchOffice UpdateBranch(Master_BranchOffice branch);

        bool DeleteBranch(Guid branchId);
    }


    public class BranchRepository : BaseRepository<Master_BranchOffice, CentralDBEntities>, IBranchRepository ,IDisposable
    {
        private  CentralDBEntities _dbContext;
        public BranchRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<Master_BranchOffice> GetAllBranchList()
        {
            return _dbContext.Master_BranchOffice.Where(b => b.IsDeleted == false).ToList();
           
           
        }

        public Master_BranchOffice GetBranch(Guid branchId)
        {

             return _dbContext.Master_BranchOffice.FirstOrDefault(b => b.ID ==branchId);
         
        }

        public Master_BranchOffice InsertBranch(Master_BranchOffice branch)
             {
            
                var result = _dbContext.Master_BranchOffice.Add(branch);
                _dbContext.SaveChanges();

                return result;
            }
          
            
        

        public Master_BranchOffice UpdateBranch(Master_BranchOffice branch)
        {
           
                _dbContext.Entry(branch).State = EntityState.Modified;
                _dbContext.SaveChanges();
                 return branch;
            
          
            
        }
        public bool DeleteBranch(Guid branchId)
        {
            
                var Branch = _dbContext.Master_BranchOffice.ToList().Find(b => b.ID == branchId);
                if (Branch != null)
                {
                    _dbContext.Master_BranchOffice.Remove(Branch);
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
