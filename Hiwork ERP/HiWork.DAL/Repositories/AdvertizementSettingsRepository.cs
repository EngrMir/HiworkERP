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
    public partial interface IAdvertizementSettingsRepository : IBaseRepository<AdvertizementSetting>
    {
        AdvertizementSetting Insertadvertizement(AdvertizementSetting Settings);
        AdvertizementSetting UpdateAdvertizement(AdvertizementSetting recordData);
      List<AdvertizementSetting> GetAdvertizementlist();
    }


   public class AdvertizementSettingsRepository: BaseRepository<AdvertizementSetting, CentralDBEntities>, IAdvertizementSettingsRepository, IDisposable
    {

        private CentralDBEntities _dbContext;
        public AdvertizementSettingsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public AdvertizementSetting Insertadvertizement(AdvertizementSetting Settings)
        {
            try
            {
                _dbContext.AdvertizementSettings.Add(Settings);
                _dbContext.SaveChanges();
                return Settings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public AdvertizementSetting UpdateAdvertizement(AdvertizementSetting recordData)

        {   
            var entry = _dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
          
            return recordData;
        }
        public List<AdvertizementSetting>GetAdvertizementlist()
        {
            return _dbContext.AdvertizementSettings.Where(n => n.IsDeleted == false).OrderByDescending(c => c.ValidDateTime).ToList();
           

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
