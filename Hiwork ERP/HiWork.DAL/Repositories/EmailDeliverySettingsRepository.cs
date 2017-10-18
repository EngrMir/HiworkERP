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
    public interface IEmailDeliverySettingsRepository : IBaseRepository<EmailDeliverySetting>
    {
        EmailDeliverySetting SaveEntity(EmailDeliverySetting model);
        EmailDeliverySetting UpdateEntity(EmailDeliverySetting model);
        EmailDeliverySetting GetEntityByID(EmailDeliverySetting model);

    }
    public class EmailDeliverySettingsRepository : BaseRepository<EmailDeliverySetting, CentralDBEntities>, IEmailDeliverySettingsRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
        public EmailDeliverySettingsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public EmailDeliverySetting SaveEntity(EmailDeliverySetting model)
        {
            _dbContext.EmailDeliverySettings.Add(model);
            _dbContext.SaveChanges();

            return model;
        }

        public EmailDeliverySetting UpdateEntity(EmailDeliverySetting model)
        {
            _dbContext.Entry(model).State= EntityState.Modified;
            _dbContext.SaveChanges();

            return model;
        }

        public EmailDeliverySetting GetEntityByID(EmailDeliverySetting model)
        {

            var result = _dbContext.EmailDeliverySettings.Where(E => model.StaffID !=null?(E.StaffID ==model.StaffID):(E.CustomerID==model.CustomerID)).SingleOrDefault();

            return result;
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
