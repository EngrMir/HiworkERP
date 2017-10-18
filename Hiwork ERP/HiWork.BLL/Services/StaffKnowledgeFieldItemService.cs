using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
    public interface IStaffKnowledgeFieldItemService : IBaseService<StaffKnowledgeFieldItemModel, Master_StaffKnowledgeFieldItem>
     {
        List<StaffKnowledgeFieldItemModel> SaveItem(StaffKnowledgeFieldItemModel dataModel);
        List<StaffKnowledgeFieldItemModel> GetItemList(BaseViewModel dataModel);
        List<StaffKnowledgeFieldItemModel> DeleteItem(StaffKnowledgeFieldItemModel dataModel);
    }
    public class StaffKnowledgeFieldItemService : BaseService<StaffKnowledgeFieldItemModel, Master_StaffKnowledgeFieldItem>,IStaffKnowledgeFieldItemService
    {
        private readonly IStaffKnowledgeFieldItemRepository repository;
        public StaffKnowledgeFieldItemService(IStaffKnowledgeFieldItemRepository repo) : base(repo)
        {
            this.repository = repo;
        }

        public List<StaffKnowledgeFieldItemModel> SaveItem(StaffKnowledgeFieldItemModel dataModel)
        {
            Master_StaffKnowledgeFieldItem masterModel;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                masterModel = Mapper.Map<StaffKnowledgeFieldItemModel, Master_StaffKnowledgeFieldItem>(dataModel);

                if (dataModel.ID == Guid.Empty)
                {
                    masterModel.ID = Guid.NewGuid();
                    masterModel.KnowledgeFieldID = dataModel.KnowledgeFieldID;
                    masterModel.CreatedBy = dataModel.CurrentUserID;
                    masterModel.CreatedDate = DateTime.Now;
                    this.repository.InsertItem(masterModel);
                }
                else
                {
                    masterModel.KnowledgeFieldID = dataModel.KnowledgeFieldID;
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

        public List<StaffKnowledgeFieldItemModel> GetItemList(BaseViewModel dataModel)
        {
            object pValue;              // Value of a Property
            string sValue;              // Value of type String

            List<Master_StaffKnowledgeFieldItem> masterList;
            List<StaffKnowledgeFieldItemModel> itemList = new List<StaffKnowledgeFieldItemModel>();
            StaffKnowledgeFieldItemModel aModel = new StaffKnowledgeFieldItemModel();

            try
            {
                masterList = this.repository.GetItemList();
                foreach (Master_StaffKnowledgeFieldItem masterItem in masterList)
                {
                    aModel = Mapper.Map<Master_StaffKnowledgeFieldItem, StaffKnowledgeFieldItemModel>(masterItem);
                    aModel.SKField = Mapper.Map<Master_StaffKnowledgeField, StaffKnowledgeFieldModel>(masterItem.Master_StaffKnowledgeField);

                    if (aModel.SKField != null)
                    {
                        pValue = Utility.GetPropertyValue(aModel.SKField, "Name", dataModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.SKField.Name = sValue;
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

            itemList.Sort(CompareStaffKnowledgeFieldItemByName);
            return itemList;
        }

        public List<StaffKnowledgeFieldItemModel> DeleteItem(StaffKnowledgeFieldItemModel dataModel)
        {
            try
            {
                this.repository.DeleteItem(dataModel.ID);
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
            errLog.SetErrorLog(userid, "StaffKnowledgeFieldItem", message);
            return message;
        }

        private int CompareStaffKnowledgeFieldItemByName(StaffKnowledgeFieldItemModel dataModel1, StaffKnowledgeFieldItemModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
