

/* ******************************************************************************************************************
 * Service for TechnicalSkillCategory Entity
 * Date             :   08-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{

    public interface ITechnicalSkillCategoryService : IBaseService
                                                    <TechnicalSkillCategoryModel, Master_StaffTechnicalSkillCategory>
    {
        List<TechnicalSkillCategoryModel>           SaveCategory(TechnicalSkillCategoryModel dataModel);
        List<TechnicalSkillCategoryModel>           GetCategoryList(BaseViewModel dataModel);
        List<TechnicalSkillCategoryModel>           DeleteCategory(TechnicalSkillCategoryModel dataModel);
    }

    public class TechnicalSkillCategoryService : BaseService<TechnicalSkillCategoryModel, Master_StaffTechnicalSkillCategory>,
                                                ITechnicalSkillCategoryService
    {
        private readonly ITechnicalSkillCategoryRepository repository;
        public TechnicalSkillCategoryService(ITechnicalSkillCategoryRepository repo) : base(repo)
        {
            this.repository = repo;
        }

        public List<TechnicalSkillCategoryModel> SaveCategory(TechnicalSkillCategoryModel dataModel)
        {
            Master_StaffTechnicalSkillCategory masterModel;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                masterModel = Mapper.Map<TechnicalSkillCategoryModel, Master_StaffTechnicalSkillCategory>(dataModel);

                if (dataModel.Id == Guid.Empty)
                {
                    masterModel.ID = Guid.NewGuid();
                    masterModel.CreatedBy = dataModel.CurrentUserID;
                    masterModel.CreatedDate = DateTime.Now;
                    this.repository.InsertCategory(masterModel);
                }
                else
                {
                    masterModel.UpdatedBy = dataModel.CurrentUserID;
                    masterModel.UpdatedDate = DateTime.Now;
                    this.repository.UpdateCategory(masterModel);
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
            return GetCategoryList(md);
        }

        public List<TechnicalSkillCategoryModel> GetCategoryList(BaseViewModel dataModel)
        {
            object pValue;              // Value of a Property
            string sValue;              // Value of type String

            List<Master_StaffTechnicalSkillCategory> masterList;
            List<TechnicalSkillCategoryModel> categoryList = new List<TechnicalSkillCategoryModel>();
            TechnicalSkillCategoryModel aModel = new TechnicalSkillCategoryModel();

            try
            {
                masterList = this.repository.GetCategoryList();
                if (masterList != null)
                {
                    foreach (Master_StaffTechnicalSkillCategory masterModel in masterList)
                    {
                        aModel = Mapper.Map<Master_StaffTechnicalSkillCategory, TechnicalSkillCategoryModel>(masterModel);

                        pValue = Utility.GetPropertyValue(aModel, "Name", dataModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.Name = sValue;

                        aModel.CurrentCulture = dataModel.CurrentCulture;
                        aModel.CurrentUserID = dataModel.CurrentUserID;
                        categoryList.Add(aModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            //return Utility.SortList((List<dynamic>)categoryList, "Name");
            categoryList.Sort(CompareTechnicalSkillCategoryByName);
            return categoryList;
        }

        public List<TechnicalSkillCategoryModel> DeleteCategory(TechnicalSkillCategoryModel dataModel)
        {
            try
            {
                this.repository.DeleteCategory(dataModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = dataModel.CurrentUserID;
            md.CurrentCulture = dataModel.CurrentCulture;
            return GetCategoryList(md);
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "TechnicalSkillCategory", message);
            return message;
        }

        private int CompareTechnicalSkillCategoryByName(TechnicalSkillCategoryModel dataModel1, TechnicalSkillCategoryModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
