

/* ******************************************************************************************************************
 * Service for Master_StaffLanguageQualifications Entity
 * Date             :   15-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using AutoMapper;
using System;
using System.Collections.Generic;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.BLL.Models;
using HiWork.DAL.Database;

namespace HiWork.BLL.Services
{
    public interface ILanguageQualificationService :
            IBaseService<LanguageQualificationModel, Master_StaffLanguageQualifications>
    {
        List<LanguageQualificationModel>    GetLanguageQualificationList(BaseViewModel dataModel);
        List<LanguageQualificationModel>    SaveLanguageQualification(LanguageQualificationModel dataModel);
        List<LanguageQualificationModel>    DeleteLanguageQualification(LanguageQualificationModel dataModel);
    }

    public class LanguageQualificationService :
            BaseService<LanguageQualificationModel, Master_StaffLanguageQualifications>,
            ILanguageQualificationService
    {
        private readonly ILanguageQualificationRepository repository;
        public LanguageQualificationService(ILanguageQualificationRepository repo) : base(repo)
        {
            repository = repo;
        }

        public List<LanguageQualificationModel> GetLanguageQualificationList(BaseViewModel dataModel)
        {
            object pValue;
            string sValue;
            List<Master_StaffLanguageQualifications> dataList;
            List<LanguageQualificationModel> modList = new List<LanguageQualificationModel>();
            LanguageQualificationModel aModel;

            try
            {
                dataList = repository.GetLanguageQualificationList();
                if (dataList != null)
                {
                    foreach(Master_StaffLanguageQualifications data in dataList)
                    {
                        aModel = Mapper.Map<Master_StaffLanguageQualifications, LanguageQualificationModel>(data);

                        pValue = Utility.GetPropertyValue(aModel, "Name", dataModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.Name = sValue;

                        pValue = Utility.GetPropertyValue(aModel, "Description", dataModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.Description = sValue;

                        aModel.CurrentCulture = dataModel.CurrentCulture;
                        aModel.CurrentUserID = dataModel.CurrentUserID;
                        modList.Add(aModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            modList.Sort(CompareLanguageQualificationByName);
            return modList;
        }

        public List<LanguageQualificationModel> SaveLanguageQualification(LanguageQualificationModel dataModel)
        {
            Master_StaffLanguageQualifications data;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                data = Mapper.Map<LanguageQualificationModel, Master_StaffLanguageQualifications>(dataModel);

                if (dataModel.Id == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = dataModel.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    repository.InsertLanguageQualification(data);
                }
                else
                {
                    data.UpdatedBy = dataModel.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    repository.UpdateLanguageQualification(data);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = dataModel.CurrentUserID;
            md.CurrentCulture = dataModel.CurrentCulture;
            return GetLanguageQualificationList(md);
        }

        public List<LanguageQualificationModel> DeleteLanguageQualification(LanguageQualificationModel dataModel)
        {
            try
            {
                repository.DeleteLanguageQualification(dataModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = dataModel.CurrentUserID;
            md.CurrentCulture = dataModel.CurrentCulture;
            return GetLanguageQualificationList(md);
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "LanguageQualification", message);
            return message;
        }

        private int CompareLanguageQualificationByName(LanguageQualificationModel dataModel1, LanguageQualificationModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
