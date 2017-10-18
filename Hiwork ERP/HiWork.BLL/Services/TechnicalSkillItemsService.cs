

/* ******************************************************************************************************************
 * Service for Master_StaffTechnicalSkillItems Entity
 * Date             :   09-Jun-2017
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
    public interface ITechnicalSkillItemsService : IBaseService
                                                       <TechnicalSkillItemsModel, Master_StaffTechnicalSkillItems>
    {
        List<TechnicalSkillItemsModel>                      SaveItem(TechnicalSkillItemsModel dataModel);
        List<TechnicalSkillItemsModel>                      GetItemList(BaseViewModel dataModel);
        List<TechnicalSkillItemsModel>                      DeleteItem(TechnicalSkillItemsModel dataModel);
    }
    public class TechnicalSkillItemsService : BaseService <TechnicalSkillItemsModel, Master_StaffTechnicalSkillItems>,
                                                ITechnicalSkillItemsService
    {
        private readonly ITechnicalSkillItemsRepository repository;
        public TechnicalSkillItemsService(ITechnicalSkillItemsRepository repo) : base(repo)
        {
            this.repository = repo;
        }

        public List<TechnicalSkillItemsModel> SaveItem(TechnicalSkillItemsModel dataModel)
        {
            Master_StaffTechnicalSkillItems masterModel;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                masterModel = Mapper.Map<TechnicalSkillItemsModel, Master_StaffTechnicalSkillItems>(dataModel);

                if (dataModel.Id == Guid.Empty)
                {
                    masterModel.ID = Guid.NewGuid();
                    masterModel.TechnicalSkillCategoryID = dataModel.TechnicalSkillCategoryId;
                    masterModel.CreatedBy = dataModel.CurrentUserID;
                    masterModel.CreatedDate = DateTime.Now;
                    this.repository.InsertItem(masterModel);
                }
                else
                {
                    masterModel.TechnicalSkillCategoryID = dataModel.TechnicalSkillCategoryId;
                    masterModel.UpdatedBy = dataModel.CurrentUserID;
                    masterModel.UpdatedDate = DateTime.Now;
                    this.repository.UpdateItem(masterModel);
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
            return GetItemList(md);
        }

        public List<TechnicalSkillItemsModel> GetItemList(BaseViewModel dataModel)
        {
            object pValue;              // Value of a Property
            string sValue;              // Value of type String

            List<Master_StaffTechnicalSkillItems> masterList;
            List<TechnicalSkillItemsModel> itemList = new List<TechnicalSkillItemsModel>();
            TechnicalSkillItemsModel aModel = new TechnicalSkillItemsModel();

            try
            {
                masterList = this.repository.GetItemList();
                foreach (Master_StaffTechnicalSkillItems masterItem in masterList)
                {
                    aModel = Mapper.Map
                                                <Master_StaffTechnicalSkillItems, TechnicalSkillItemsModel>
                                                (masterItem);
                    aModel.TSCategory = Mapper.Map
                                                <Master_StaffTechnicalSkillCategory, TechnicalSkillCategoryModel>
                                                (masterItem.Master_StaffTechnicalSkillCategory);

                    if (aModel.TSCategory != null)
                    {
                        pValue = Utility.GetPropertyValue(aModel.TSCategory, "Name", dataModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.TSCategory.Name = sValue;
                    }

                    pValue = Utility.GetPropertyValue(aModel, "Name", dataModel.CurrentCulture);
                    sValue = pValue == null ? string.Empty : pValue.ToString();
                    aModel.Name = sValue;

                    pValue = Utility.GetPropertyValue(aModel, "Description", dataModel.CurrentCulture);
                    sValue = pValue == null ? string.Empty : pValue.ToString();
                    aModel.Description = sValue;

                    aModel.CurrentCulture = dataModel.CurrentCulture;
                    aModel.CurrentUserID = dataModel.CurrentUserID;
                    itemList.Add(aModel);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            itemList.Sort(CompareTechnicalSkillItemsByName);
            return itemList;
        }

        public List<TechnicalSkillItemsModel> DeleteItem(TechnicalSkillItemsModel dataModel)
        {
            try
            {
                this.repository.DeleteItem(dataModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = dataModel.CurrentUserID;
            md.CurrentCulture = dataModel.CurrentCulture;
            return GetItemList(md);
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errLog.SetErrorLog(userid, "TechnicalSkillItems", message);
            return message;
        }

        private int CompareTechnicalSkillItemsByName(TechnicalSkillItemsModel dataModel1, TechnicalSkillItemsModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
