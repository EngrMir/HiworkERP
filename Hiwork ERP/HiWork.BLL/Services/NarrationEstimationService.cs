using AutoMapper;
using HiWork.BLL.Models;
using HiWork.BLL.ServiceHelper;
using HiWork.DAL.Database;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{

    public partial interface INarrationEstimationService
    {
        bool Save(CommonModelHelper model);
        List<EstimationModel> GetAllNarrationEstimationList(BaseViewModel model);
        List<EstimationModel> DeleteNarrationEstimation(EstimationModel model);
        List<EstimationWorkContent> GetWorkContentList(Guid estimationID);
        List<EstimationDetail> GetEstimationDetailsListByID(BaseViewModel model, Guid EstimationID);
        List<EstimationNarrationExpense> GetEstimationNarrationExpenseListByID(BaseViewModel model, Guid EstimationID);
    }

    public class NarrationEstimationService : INarrationEstimationService, IDisposable
    {
        private CentralDBEntities _dbContext;
        private readonly ISqlConnectionService _sqlConnService;
        private BaseViewModel BaseModel;

        public NarrationEstimationService()
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
            IApplicationService appService = new ApplicationService(new DAL.Repositories.ApplicationRepository(new UnitOfWork()));

            RegistrationIdNext = GetNextRegistrationID();
            AppCode = appService.GetApplicationCode(appid);
            Today = DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
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
                model.Estimation.EstimationType = (int)EstimationType.Narration;
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
                foreach (var item in model.EstimationDetails)
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
                }
                //Save file type           
                var existingWorkContent = _dbContext.EstimationWorkContents.Where(x => x.Estimation.ID == model.Estimation.ID).ToList();

                existingWorkContent?.ForEach(ewc =>
                {
                    _dbContext.EstimationWorkContents.Remove(ewc);
                });

                //Save work content
                model.WorkContents?.ForEach(wc =>
                {
                    var content = new EstimationWorkContent
                    {
                        ID = Guid.NewGuid(),
                        Estimation = model.Estimation,
                        WorkContent = wc.WorkContent,
                        IsDeleted = false
                    };
                    _dbContext.EstimationWorkContents.Add(content);
                });

                model.estimationCompetencies?.ForEach(ec =>
                {
                    var content = new EstimationCompetency
                    {
                        ID = Guid.NewGuid(),
                        Estimation = model.Estimation,
                        CompetencyType = ec.CompetencyType,
                        CompetencyDetail = ec.CompetencyDetail
                    };
                    _dbContext.EstimationCompetencies.Add(content);
                });

                if (model.EstimationNarrationExpense.ID == Guid.Empty)
                {

                    model.EstimationNarrationExpense.ID = Guid.NewGuid();
                    model.EstimationNarrationExpense.EstimationID = model.Estimation.ID;
                    _dbContext.EstimationNarrationExpenses.Add(model.EstimationNarrationExpense);
                }
                else
                {
                    _dbContext.Entry(model.EstimationNarrationExpense).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "NarrationEstimation", message);
                throw new Exception(message);
            }
            return isSuccessful;
        }


        public List<EstimationWorkContent> GetWorkContentList(Guid estimationID)
        {
            var wc = _dbContext.EstimationWorkContents.Where(w => w.Estimation.ID == estimationID).ToList();
            return wc;
        }

        private string GetNextRegistrationID()
        {
            var item = _dbContext.Estimations.OrderByDescending(e => e.RegistrationID).Select(e => e.RegistrationID).FirstOrDefault();
            return (item + 1).ToString();
        }

        public List<EstimationModel> GetAllNarrationEstimationList(BaseViewModel model)
        {
            EstimationModel estimationModel;
            var EstimationModelList = new List<EstimationModel>();

            try
            {
                var masterDataList = _dbContext.Estimations.ToList();
                if (masterDataList != null)
                {
                    foreach (Estimation MasterData in masterDataList)
                    {
                        estimationModel = Mapper.Map<Estimation, EstimationModel>(MasterData);
                        estimationModel.CurrentUserID = model.CurrentUserID;
                        estimationModel.CurrentCulture = model.CurrentCulture;
                        EstimationModelList.Add(estimationModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "DTPEstimation", message);
                throw new Exception(message);
            }
            return EstimationModelList;
        }

        public List<EstimationDetail> GetEstimationDetailsListByID(BaseViewModel model, Guid EstimationID)
        {
            var items = _dbContext.EstimationDetails.Where(e => e.Estimation.ID == EstimationID).ToList();
            return items;
        }

        public List<EstimationNarrationExpense> GetEstimationNarrationExpenseListByID(BaseViewModel model, Guid EstimationID)
        {
            var items = _dbContext.EstimationNarrationExpenses.Where(e => e.EstimationID == EstimationID).ToList();
            return items;
        }

        public List<EstimationModel> DeleteNarrationEstimation(EstimationModel model)
        {
            try
            {
                var estimation = _dbContext.Estimations.Find(model.ID);
                if (estimation != null)
                {
                    estimation.IsDeleted = true;
                    _dbContext.Entry(estimation).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "DTPEstimation", message);
                throw new Exception(message);
            }

            BaseModel.CurrentCulture = model.CurrentCulture;
            BaseModel.CurrentUserID = model.CurrentUserID;
            return GetAllNarrationEstimationList(BaseModel);
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
