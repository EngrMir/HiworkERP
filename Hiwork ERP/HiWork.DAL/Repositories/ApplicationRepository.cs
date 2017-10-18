
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HiWork.DAL.Repositories
{
    public interface IApplicationRepository: IBaseRepository<Application>
    {
        List<Application>  GetApplicationList();
        Application  InsertApplication(Application model);
        Application   UpdateApplication(Application model);
        bool  DeleteApplication(long appid);
        string GetApplicationCode(long AppId);
        bool  IsApplicationActive(long Id);
    }

    public class ApplicationRepository : BaseRepository<Application, CentralDBEntities>, IApplicationRepository, IDisposable
    {
        private CentralDBEntities _dbContext;
      
        public ApplicationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public string GetApplicationCode(long AppId)
        {
            Application appEntity;
            appEntity = _dbContext.Applications.ToList().Where(a => a.Id == AppId).SingleOrDefault();
            return appEntity.Code;
        }

        public List<Application> GetApplicationList()
        {
            return _dbContext.Applications.ToList();
        }

        public Application InsertApplication(Application model)
        {
            Application result;
            result =  _dbContext.Applications.Add(model);
            _dbContext.SaveChanges();
            return result;
        }

        public Application UpdateApplication(Application model)
        {
            var entry = this._dbContext.Entry(model);
            entry.State = EntityState.Modified;
            this._dbContext.SaveChanges();
            return model;
        }

        public bool DeleteApplication(long appid)
        {
            bool result;
            Application appEntity;
            List<Application> datalist;

            datalist = this._dbContext.Applications.ToList();
            appEntity = datalist.Find(item => item.Id == appid);

            if (appEntity != null)
            {
                this._dbContext.Applications.Remove(appEntity);
                this._dbContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool IsApplicationActive(long Id)
        {
            bool result;
            Application appEntity;
            appEntity = _dbContext.Applications.ToList().Where(a => a.Id == Id && a.IsActive == true).SingleOrDefault();
            result = appEntity != null ? true : false;
            return result;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing == false)
                return;
            if (this._dbContext == null)
                return;
            this._dbContext.Dispose();
            this._dbContext = null;
            return;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}
