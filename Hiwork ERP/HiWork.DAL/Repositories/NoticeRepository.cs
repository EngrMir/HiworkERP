
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
    public partial interface INoticeRepository : IBaseRepository<Notice>
    {
        Notice InsertNotice(Notice notice);
        List<Notice> GetNoticelist();
        Notice UpdateNotice(Notice recordData);
    }
    

   public class NoticeRepository: BaseRepository<Notice, CentralDBEntities>,INoticeRepository, IDisposable
    {

        private CentralDBEntities _dbContext;
        public NoticeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }
        public Notice InsertNotice(Notice notice)
        {
            try
            {
                _dbContext.Notices.Add(notice);
                _dbContext.SaveChanges();
                return notice;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Notice UpdateNotice(Notice recordData)
        {
            var entry = _dbContext.Entry(recordData);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
           
            return recordData;
        }

        public List<Notice> GetNoticelist()
        {
            return _dbContext.Notices.Where(n => n.IsDeleted == false).OrderByDescending(c => c.RegisteredDate).ToList();
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
