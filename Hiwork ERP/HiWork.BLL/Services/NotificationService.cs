using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HiWork.BLL.Services
{
    public partial interface INotificationService
    {
        List<NotificationModel> GetUnapprovedList(BaseViewModel model);
        bool UpdateNotificationsAsRead(BaseViewModel model);
        int CountNotification(BaseViewModel model, int status);
    }
    public class NotificationService : INotificationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        public NotificationService()
        {
            _dbContext = new CentralDBEntities();
        }
        public List<NotificationModel> GetUnapprovedList(BaseViewModel model)
        {
            var itemList = new List<NotificationModel>();
            try
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = false;
                var user = _dbContext.UserInformations.Find(model.CurrentUserID);
                var status = (int)EstimationApprovalStatus.Approved;
                itemList = (from ea in _dbContext.EstimationApprovals
                            join e in _dbContext.Estimations on ea.Estimation.ID equals e.ID
                            where ea.Status != status && ea.ApproverID == user.EmployeeID
                            select new NotificationModel
                            {
                                ID = ea.ID,
                                ApplicationID = ea.ApplicationID,
                                ApproverID = ea.ApproverID,
                                EstimationID = ea.EstimationID,
                                OrderID = ea.OrderID,
                                Description = ea.Description,
                                Status = ea.Status,
                                EstimationType = e.EstimationType
                            }).ToList();
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Notification", message);
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return itemList;
        }

        public bool UpdateNotificationsAsRead(BaseViewModel model)
        {
            var flag = true;
            try
            {
                var user = _dbContext.UserInformations.Find(model.CurrentUserID);
                var approveStatus = (int)EstimationApprovalStatus.Approved;
                var readStatus = (int)EstimationApprovalStatus.Approved;
                var items = _dbContext.EstimationApprovals.Where(x => x.ApproverID == user.EmployeeID && x.Status != approveStatus && x.Status != readStatus).ToList();
                items.ForEach(item =>
                {
                    item.Status = (int)EstimationApprovalStatus.Read;
                    _dbContext.Entry(item).State = EntityState.Modified;
                });
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                flag = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "UpdateNotificationsAsRead", message);
                throw new Exception(ex.Message);
            }
            return flag;
        }

        public int CountNotification(BaseViewModel model, int status)
        {
            var count = 0;
            try
            {
                var user = _dbContext.UserInformations.Find(model.CurrentUserID);
                if (status == (int)EstimationApprovalStatus.Unread)
                {
                    count = (from ea in _dbContext.EstimationApprovals
                             where ea.Status == status && ea.ApproverID == user.EmployeeID
                             select ea.ID).Count();
                }
                else if (status == (int)EstimationApprovalStatus.Approved)
                {
                    count = (from ea in _dbContext.EstimationApprovals
                             where ea.Status != status && ea.ApproverID == user.EmployeeID
                             select ea.ID).Count();
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CountUnreadNotification", message);
                throw new Exception(ex.Message);
            }
            return count;
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
