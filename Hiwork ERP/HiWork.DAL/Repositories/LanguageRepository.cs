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
    public partial interface ILanguageRepository : IBaseRepository<Master_Language>
    {
        List<Master_Language> GetAllLanguageList();

        Master_Language GetLanguage(Guid langId);

        Master_Language InsertLanguage(Master_Language branch);

        Master_Language UpdateLanguage(Master_Language branch);

        bool DeleteLanguage(Guid langId);
    }


    public class LanguageRepository : BaseRepository<Master_Language, CentralDBEntities>, ILanguageRepository, IDisposable
    {
        private  CentralDBEntities _dbContext;
        public LanguageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }


        public List<Master_Language> GetAllLanguageList()
        {
                return _dbContext.Master_Language.Where(l=>l.IsDeleted==false).ToList();
           
           
        }

        public Master_Language GetLanguage(Guid langId)
        {

             return _dbContext.Master_Language.FirstOrDefault(b => b.ID ==langId && b.IsDeleted == false);
         
        }

        public Master_Language InsertLanguage(Master_Language lang)
        {
            
            var result = _dbContext.Master_Language.Add(lang);
            _dbContext.SaveChanges();

            return result;
        }
          
            
        

        public Master_Language UpdateLanguage(Master_Language lang)
        {
           
                _dbContext.Entry(lang).State = EntityState.Modified;
                _dbContext.SaveChanges();
                 return lang;
            
          
            
        }
        public bool DeleteLanguage(Guid langId)
        {
            
                var Lang = _dbContext.Master_Language.ToList().Find(b => b.ID == langId);
                if (Lang != null)
                {
                    _dbContext.Master_Language.Remove(Lang);
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
