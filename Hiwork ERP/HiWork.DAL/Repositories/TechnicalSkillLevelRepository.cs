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
   //public partial interface ITechnicalSkillLevelRepository : IBaseRepository<Master_StaffTechnicalSkillLevel>
   // {
   //     List<Master_StaffTechnicalSkillLevel> GetAllStaffTechnicalSkillLevelList();
   //     Master_StaffTechnicalSkillLevel GetStaffTechnicalSkillLevel(Guid Id);
   //     Master_StaffTechnicalSkillLevel InsertStaffTechnicalSkillLevel(Master_StaffTechnicalSkillLevel StaffTechnicalSkillLevel);
   //     Master_StaffTechnicalSkillLevel UpdateStaffTechnicalSkillLevel(Master_StaffTechnicalSkillLevel StaffTechnicalSkillLevel);
   //     bool DeleteStaffTechnicalSkillLevel(Guid Id);
   // }

   // public class TechnicalSkillLevelRepository : BaseRepository<Master_StaffTechnicalSkillLevel, CentralDBEntities>, ITechnicalSkillLevelRepository, IDisposable
   // {
   //     private CentralDBEntities _dbContext;
   //     protected void Dispose(bool disposing)
   //     {
   //         if (disposing)
   //         {
   //             if (_dbContext != null)
   //             {
   //                 _dbContext.Dispose();
   //                 _dbContext = null;
   //             }
   //         }
   //     }

   //     public void Dispose()
   //     {
   //         Dispose(true);
   //         GC.SuppressFinalize(this); //Garbase collector
   //     }
   //     public TechnicalSkillLevelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
   //     {
   //         _dbContext = new CentralDBEntities();
   //     }

   //     public bool DeleteStaffTechnicalSkillLevel(Guid Id)
   //     {
   //         try
   //         {
   //             var StaffTechnicalSkillLevel = _dbContext.Master_StaffTechnicalSkillLevel.ToList().Find(d => d.ID == Id);
   //             if (StaffTechnicalSkillLevel != null)
   //             {
   //                 _dbContext.Master_StaffTechnicalSkillLevel.Remove(StaffTechnicalSkillLevel);
   //                 _dbContext.SaveChanges();

   //                 return true;
   //             }
   //         }
   //         catch (Exception ex)
   //         {
   //             string message;
   //             message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
   //             throw new Exception(message);
   //         }
   //         return false;
   //     }

   //     public List<Master_StaffTechnicalSkillLevel> GetAllStaffTechnicalSkillLevelList()
   //     {
   //         try
   //         {
   //             return _dbContext.Master_StaffTechnicalSkillLevel.ToList();
   //         }
   //         catch (Exception ex) { }

   //         return null;
   //     }

   //     public Master_StaffTechnicalSkillLevel GetStaffTechnicalSkillLevel(Guid Id)
   //     {
   //         try
   //         {
   //             return _dbContext.Master_StaffTechnicalSkillLevel.FirstOrDefault(u => u.ID == Id);
   //         }
   //         catch (Exception ex) { }
   //         return null;
   //     }

   //     public Master_StaffTechnicalSkillLevel InsertStaffTechnicalSkillLevel(Master_StaffTechnicalSkillLevel StaffTechnicalSkillLevel)
   //     {
   //         try
   //         {
   //             var result = _dbContext.Master_StaffTechnicalSkillLevel.Add(StaffTechnicalSkillLevel);
   //             _dbContext.SaveChanges();

   //             return result;
   //         }
   //         catch (Exception ex) { }

   //         return null;
   //     }

   //     public Master_StaffTechnicalSkillLevel UpdateStaffTechnicalSkillLevel(Master_StaffTechnicalSkillLevel StaffTechnicalSkillLevel)
   //     {
   //         try
   //         {
   //             _dbContext.Entry(StaffTechnicalSkillLevel).State = EntityState.Modified;
   //             _dbContext.SaveChanges();
   //             return StaffTechnicalSkillLevel;
   //         }
   //         catch (Exception ex) { }

   //         return null;
   //     }
   // }
}
