using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using HiWork.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using HiWork.DAL.Repositories;
using System.Globalization;
using System.IO;
using HiWork.BLL.ServiceHelper;

namespace HiWork.BLL.Services
{
    public partial interface IOverheadCostQuotationService
    {
        bool Save(CommonModelHelper model);
        EstimationModel GetTaskQuotation(string id);
        List<EstimationDetail> GetEstimationDetailsListByID(BaseViewModel model, Guid EstimationID);
        List<EstimationDetail> GetTaskQuotationDetailList(Guid id);
    }

    public class OverheadCostQuotationService : IOverheadCostQuotationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public OverheadCostQuotationService()
        {
            _dbContext = new CentralDBEntities();
            _sqlConnService = new SqlConnectionService();
            BaseModel = new BaseViewModel();
        }

        private string GenerateEstimationNumber(long appid)
        {
            string RegistrationIdNext;
            string Today, AppCode;
            StringBuilder buffer = new StringBuilder();
            IApplicationService appService = new ApplicationService(new ApplicationRepository(new UnitOfWork()));

            RegistrationIdNext = GetNextRegistrationID();
            AppCode = appService.GetApplicationCode(appid);
            Today = DateTime.Now.ToString("yyyyMMdd", CultureInfo.CurrentCulture);
            buffer.Append(Today);
            buffer.Append(AppCode);
            buffer.Append(RegistrationIdNext);
            return buffer.ToString();
        }

        public bool Save(CommonModelHelper model)
        {
            var isSuccessful = true;
            try
            {
                var culturalItems = new List<string> { "BillingAddress", "ClientAddress", "BillingCompanyName", "DeliveryCompanyName", "DeliveryAddress", "Remarks" };
                ModelBinder.SetCulturalValue(model.Estimation, model, culturalItems);
                model.Estimation.EstimationNo = GenerateEstimationNumber(model.ApplicationID);
                model.Estimation.EstimationType = (int)EstimationType.OverheadCost;
                model.Estimation.EstimationStatus = (int)EstimationStatus.Ordered;
                ModelBinder.ModifyGuidValue(model.Estimation);
                //cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                if (model.Estimation.ID == Guid.Empty)
                {
                    model.Estimation.ID = Guid.NewGuid();
                    model.Estimation.RegistrationDate = DateTime.Now;
                    _dbContext.Estimations.Add(model.Estimation);
                }
                else
                {
                    _dbContext.Entry(model.Estimation).State = EntityState.Modified;
                }
                //Save or update Estimation details
                model.EstimationDetails.ForEach(item =>
                {
                    item.EstimationID = model.Estimation.ID;
                    ModelBinder.ModifyGuidValue(item);
                    if (item.ID == Guid.Empty)
                    {
                        item.ID = Guid.NewGuid();
                        _dbContext.EstimationDetails.Add(item);
                    }
                    else
                    {
                        _dbContext.Entry(item).State = EntityState.Modified;
                    }
                });
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "OverheadCostQuotation", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }

        private string GetNextRegistrationID()
        {
            var item = _dbContext.Estimations.OrderByDescending(e => e.RegistrationID).Select(e => e.RegistrationID).FirstOrDefault();
            return (item + 1).ToString();
        }

        public EstimationModel GetTaskQuotation(string id)
        {
            var item = _dbContext.Estimations.Find(id);
            var estimationModel = Mapper.Map<Estimation, EstimationModel>(item);
            return estimationModel;
        }

        public List<EstimationDetail> GetEstimationDetailsListByID(BaseViewModel model, Guid EstimationID)
        {
            var items = _dbContext.EstimationDetails.Where(e => e.Estimation.ID == EstimationID).ToList();
            return items;
        }

        public List<EstimationDetail> GetTaskQuotationDetailList(Guid id)
        {
            var items = _dbContext.EstimationDetails.Where(e => e.Estimation.ID == id).ToList();
            return items;
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
