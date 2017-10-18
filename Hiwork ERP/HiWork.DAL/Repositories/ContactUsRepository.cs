using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.DAL.Repositories
{
    public partial interface IContactUsRepository: IBaseRepository<ContactU>
    {
        ContactU InsertContactus(ContactU contact);
        ContactU UpdateContact(ContactU contact);
        List<ContactU> GetContactus();

    }
   public class ContactUsRepository: BaseRepository<ContactU,CentralDBEntities>, IContactUsRepository, IDisposable
    {

        private CentralDBEntities _dbContext;
        public ContactUsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }
        public ContactU InsertContactus(ContactU contact)
        {
            try
            {
                _dbContext.ContactUs.Add(contact);
                _dbContext.SaveChanges();
               return contact;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public ContactU UpdateContact(ContactU contact)
        {
            var entry = _dbContext.Entry(contact);
            entry.State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();
            return contact;
        }
       public List<ContactU> GetContactus()
        {
            try
            {
                return _dbContext.ContactUs.Where(c=> c.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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
