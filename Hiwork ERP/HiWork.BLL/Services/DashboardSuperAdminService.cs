using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HiWork.BLL.Services
{
    public partial interface IDashboardSuperAdminService
    {
        List<DashboardSAdminModel> GetSAdminDashboardData(BaseViewModel model);
    }

    public class DashboardSuperAdminService : IDashboardSuperAdminService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public DashboardSuperAdminService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        public List<DashboardSAdminModel> GetSAdminDashboardData(BaseViewModel model)
        {
            var retList = new List<DashboardSAdminModel>();
            try
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                var CurrentUserID = new SqlParameter("CurrentUserID", model.CurrentUserID);
                var ApplicationID = new SqlParameter("ApplicationID", model.ApplicationId);
                var CultureId = new SqlParameter("CultureId", model.CurrentCulture);
                IQueryable<DashboardSAdminModel> query = _dbContext.Database.SqlQuery<DashboardSAdminModel>("exec SP_Dashboard_SAdmin @CurrentUserID, @ApplicationID, @CultureId ",
                                                                                                CurrentUserID, ApplicationID, CultureId).AsQueryable();
                retList = query.ToList();
                /* retList.ForEach(el => {
                    el.EstimationStatusName = Enum.GetName(typeof(EstimationStatus), el.EstimationStatus);
                    el.PageButtonAttribute = new PageAttributes(el.EstimationStatusName);
                    el.CurrentCulture = model.CurrentCulture;
                    el.CurrentUserID = model.CurrentUserID;
                    el.EstimationStatusID = el.EstimationStatusID;
                }); */
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Dashboard", message);
                throw new Exception(message);
            }
            finally
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return retList;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
                return;
            if (this._dbContext == null)
                return;
            this._dbContext.Dispose();
            this._dbContext = null;
            return;
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}
