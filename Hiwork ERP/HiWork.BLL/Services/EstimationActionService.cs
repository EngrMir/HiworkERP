using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using HiWork.BLL.ServiceHelper;
using System.Data.Entity;

namespace HiWork.BLL.Services
{ 
     public partial interface IEstimationActionService
    {
        bool Save(CommonModelHelper model);
        List<EstimationActionModel> GetAllActionListByEstimation(BaseViewModel model, Guid estimationId);
        bool Delete(Guid id);
    }

    public class EstimationActionService : IEstimationActionService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public EstimationActionService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        public bool Save(CommonModelHelper model)
        {
            bool isSuccessful = true;
            try
            {
                var culturalItems = new List<string> { "ActionDetails" };
                ModelBinder.SetCulturalValue(model.EstimationAction, model, culturalItems);
                model.EstimationAction.OperationDate = DateTime.Now;
                model.EstimationAction.Estimation = _dbContext.Estimations.Find(model.EstimationAction.EstimationID);
                if (model.EstimationAction.ID == Guid.Empty)
                {
                    model.EstimationAction.ID = Guid.NewGuid();
                    _dbContext.EstimationActions.Add(model.EstimationAction);
                }
                else
                {
                    _dbContext.Entry(model.EstimationAction).State = EntityState.Modified;
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EstimationAction", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }
        
        public List<EstimationActionModel> GetAllActionListByEstimation(BaseViewModel model, Guid estimationId)
        {
            var estimationModelList = new List<EstimationActionModel>();
            try
            {
                var masterDataList = _dbContext.EstimationActions.Where(e=>e.Estimation.ID == estimationId).ToList();
                masterDataList.ForEach(item => {
                    var user = _dbContext.UserInformations.Find(item.OperationBy);
                    user = user == null ? new UserInformation() : user;
                    var actionModel = new EstimationActionModel
                    {
                        ID = item.ID,
                        EstimationID = item.EstimationID,
                        NextActionDate = item.NextActionDate,
                        OperandName = $"{user.FirstName} {user.LastName}",
                        ActionDetails = item.GetType().GetProperty($"ActionDetails_{model.CurrentCulture}").GetValue(item, null)?.ToString() ?? string.Empty
                    };
                    estimationModelList.Add(actionModel);
                });
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Estimation", message);
                throw new Exception(message);
            }
            return estimationModelList;
        }

        public bool Delete(Guid id)
        {
            var obj = _dbContext.EstimationActions.Find(id);
            if (obj != null)
            {
                _dbContext.EstimationActions.Remove(obj);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
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
