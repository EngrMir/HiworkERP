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
     
      public partial interface IInterpretationFieldsRepository : IBaseRepository<Master_StaffInterpretationFields>
    {
        List<Master_StaffInterpretationFields> GetAllInterpretationFieldsList();
        Master_StaffInterpretationFields GetInterpretationFields(Guid Id);
        Master_StaffInterpretationFields InsertInterpretationFields(Master_StaffInterpretationFields userInterpretationFields);
        Master_StaffInterpretationFields UpdateInterpretationFields(Master_StaffInterpretationFields userInterpretationFields);
        bool DeleteInterpretationFields(Guid Id);
    }

    public class InterpretationFieldsRepository : BaseRepository<Master_StaffInterpretationFields, CentralDBEntities>, IInterpretationFieldsRepository
    {
        private readonly CentralDBEntities _dbContext;

        public InterpretationFieldsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public bool DeleteInterpretationFields(Guid Id)
        {
            try
            {
                var user = _dbContext.Master_StaffInterpretationFields.ToList().Find(f => f.ID == Id);
                if (user != null)
                {
                    _dbContext.Master_StaffInterpretationFields.Remove(user);
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

        public List<Master_StaffInterpretationFields> GetAllInterpretationFieldsList()
        {
            try
            {
                return _dbContext.Master_StaffInterpretationFields.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffInterpretationFields GetInterpretationFields(Guid Id)
        {
            try
            {
                return _dbContext.Master_StaffInterpretationFields.FirstOrDefault(u => u.ID == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffInterpretationFields InsertInterpretationFields(Master_StaffInterpretationFields InterpretationFields)
        {
            try
            {
                InterpretationFields.ID = Guid.NewGuid();
                var result = _dbContext.Master_StaffInterpretationFields.Add(InterpretationFields);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Master_StaffInterpretationFields UpdateInterpretationFields(Master_StaffInterpretationFields userInterpretationFields)
        {
            try
            {
                _dbContext.Entry(userInterpretationFields).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return userInterpretationFields;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
