

using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure;
using System;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using AutoMapper;
using HiWork.Utils;

namespace HiWork.BLL.Services
{
    public interface IApplicationService : IBaseService<ApplicationModel, Application>
    {
        List<ApplicationModel>   SaveApplication(ApplicationModel model);
        List<ApplicationModel>  GetApplicationList(BaseViewModel model);
        string   GetApplicationCode(long AppId);
        bool   IsApplicationActive(long Id);
        ApplicationModel GetApplicationinfo(long id);
    }

    public class ApplicationService : BaseService<ApplicationModel, Application>, IApplicationService
    {
        public IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository dbRepository) : base(dbRepository)
        {
            _applicationRepository = dbRepository;
        }

        public List<ApplicationModel> GetApplicationList(BaseViewModel model)
        {
            ApplicationModel appModel;
            List<ApplicationModel> appList = new List<ApplicationModel>();
            List<Application> datalist;

            try
            {
                datalist = _applicationRepository.GetApplicationList();
                if (datalist != null)
                {
                    foreach (Application appEntity in datalist)
                    {
                        appModel = Mapper.Map<Application, ApplicationModel>(appEntity);
                        appModel.CurrentUserID = model.CurrentUserID;
                        appModel.CurrentCulture = model.CurrentCulture;
                        appList.Add(appModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex, model.CurrentUserID);
                throw new Exception(ex.Message);
            }

            appList.Sort(CompareApplicationByName);
            return appList;
        }
        public ApplicationModel GetApplicationinfo(long id)
        {
            ApplicationModel appModel = null;
            List<ApplicationModel> appList = new List<ApplicationModel>();
            List<Application> datalist;

            try
            {
                datalist = _applicationRepository.GetApplicationList();
                if (datalist != null)
                {
                    var appObj = datalist.Find(f => f.Id == id);                   
                    appModel = Mapper.Map<Application, ApplicationModel>(appObj);                 
                }
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message);
            }

            return appModel;
        }
        public string GetApplicationCode(long AppId)
        {
            string result;

            try
            {
                result = _applicationRepository.GetApplicationCode(AppId);
            }
            catch (Exception ex)
            {
                LogException(ex, AppId);
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool IsApplicationActive(long Id)
        {
            bool result;

            try
            {
                result = _applicationRepository.IsApplicationActive(Id);
            }
            catch (Exception ex)
            {
                LogException(ex, Id);
                throw new Exception(ex.Message);
            }
            return result;
        }

        public List<ApplicationModel> SaveApplication(ApplicationModel model)
        {
            Application data;
            
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                data = Mapper.Map<ApplicationModel, Application>(model);

                if (model.Id <= 0)
                {
                    data.CreatedDate = DateTime.Now;
                    data.CreatedBy = model.CurrentUserID;
                    _applicationRepository.InsertApplication(data);
                }
                else
                {
                    data.UpdatedDate = DateTime.Now;
                    data.UpdatedBy = model.CurrentUserID;
                    _applicationRepository.UpdateApplication(data);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, model.CurrentUserID);
                throw new Exception(ex.Message);
            }

            BaseViewModel BaseModel = new BaseViewModel();
            BaseModel.CurrentCulture = model.CurrentCulture;
            BaseModel.CurrentUserID = model.CurrentUserID;
            return GetApplicationList(BaseModel);
        }

        private int CompareApplicationByName(ApplicationModel model1, ApplicationModel model2)
        {
            int cmpresult;
            cmpresult = string.Compare(model1.Name, model2.Name, true);
            return cmpresult;
        }

        private void LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "Application", message);
            return;
        }
    }
}

