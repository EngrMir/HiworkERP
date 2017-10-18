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

    public partial interface IErrorReportWebRepository:IBaseRepository<ErrorReportWeb>
    {
        ErrorReportWeb InsertErrorReport(ErrorReportWeb errorReport);
        ErrorReportWeb UpdateErrorReport(ErrorReportWeb errorReport);
        List<ErrorReportWeb> GetErrorReport();
        bool DeleteErrorReport(long ID);
    }

    public class ErrorReportWebRepository :BaseRepository<ErrorReportWeb,CentralDBEntities>, IErrorReportWebRepository
    {
        private CentralDBEntities _dbContext;


        public ErrorReportWebRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = new CentralDBEntities();
        }

        public ErrorReportWeb InsertErrorReport(ErrorReportWeb errorReport)
        {
            try
            {
                var result = _dbContext.ErrorReportWebs.Add(errorReport);
                _dbContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ErrorReportWeb UpdateErrorReport(ErrorReportWeb errorReport)
        {
            try
            {
                _dbContext.Entry(errorReport).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return errorReport;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<ErrorReportWeb> GetErrorReport()
        {
            try
            {
                return _dbContext.ErrorReportWebs.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        public bool DeleteErrorReport(long ID)
        {
            try
            {
                var rep = _dbContext.ErrorReportWebs.ToList().Find(C => C.ID == ID);
                if (rep != null)
                {
                    _dbContext.ErrorReportWebs.Remove(rep);
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
