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
    public partial interface IStaffTranslationFieldsRepository : IBaseRepository<Master_StaffTranslationFields>
    {
        List<Master_StaffTranslationFields> GetAllStaffTranslationFieldsList();

        Master_StaffTranslationFields GetStaffTranslationFields(Guid stafftranslatioId);

        Master_StaffTranslationFields InserStaffTranslationFields(Master_StaffTranslationFields stafftranslation);

        Master_StaffTranslationFields UpdateStaffTranslationFields(Master_StaffTranslationFields stafftranslation);

        bool DeleteStaffTranslationFields(Guid stafftranslatioId);
    }


    public class StaffTranslationFieldsRepository : BaseRepository<Master_StaffTranslationFields, CentralDBEntities>, IStaffTranslationFieldsRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public StaffTranslationFieldsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public List<Master_StaffTranslationFields> GetAllStaffTranslationFieldsList()
        {
            return _dbContext.Master_StaffTranslationFields.Where(s=>s.IsDeleted==false).ToList();


        }

        public Master_StaffTranslationFields GetStaffTranslationFields(Guid stafftranslationId)
        {

            return _dbContext.Master_StaffTranslationFields.FirstOrDefault(d => d.ID == stafftranslationId);

        }

        public Master_StaffTranslationFields InserStaffTranslationFields(Master_StaffTranslationFields stafftranslation)
        {

            var result = _dbContext.Master_StaffTranslationFields.Add(stafftranslation);
            _dbContext.SaveChanges();

            return result;
        }

        public Master_StaffTranslationFields UpdateStaffTranslationFields(Master_StaffTranslationFields stafftranslation)
        {

            _dbContext.Entry(stafftranslation).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return stafftranslation;



        }

        public bool DeleteStaffTranslationFields(Guid stafftranslationId)
        {

            var StaffTranslationField = _dbContext.Master_StaffTranslationFields.ToList().Find(s => s.ID == stafftranslationId);
            if (StaffTranslationField != null)
            {
                _dbContext.Master_StaffTranslationFields.Remove(StaffTranslationField);
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
