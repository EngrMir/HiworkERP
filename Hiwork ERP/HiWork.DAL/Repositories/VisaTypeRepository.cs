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
    public partial interface IVisaTypeRepository : IBaseRepository<Master_StaffVisaType>
    {
        List<Master_StaffVisaType> GetAllVisaTypeList();
        Master_StaffVisaType GetVisaType(Guid Id);
        Master_StaffVisaType InsertVisaType(Master_StaffVisaType userVisaType);
        Master_StaffVisaType UpdateVisaType(Master_StaffVisaType userVisaType);
        bool DeleteVisaType(Guid Id);
    }

    public class VisaTypeRepository : BaseRepository<Master_StaffVisaType, CentralDBEntities>, IVisaTypeRepository
    {
        private readonly CentralDBEntities _dbContext;

        public VisaTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteVisaType(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffVisaType.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Master_StaffVisaType.Remove(user);
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

        public List<Master_StaffVisaType> GetAllVisaTypeList()
        {
            try
            {
                return _dbContext.Master_StaffVisaType.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffVisaType GetVisaType(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffVisaType.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffVisaType InsertVisaType(Master_StaffVisaType visaType)
        {
            try
            {
                visaType.ID= Guid.NewGuid();
                var result = _dbContext.Master_StaffVisaType.Add(visaType);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffVisaType UpdateVisaType(Master_StaffVisaType userVisaType)
        {
            try
            {
                _dbContext.Entry(userVisaType).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userVisaType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
