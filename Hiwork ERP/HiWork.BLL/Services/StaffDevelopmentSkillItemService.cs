

/* ******************************************************************************************************************
 * Service for Master_StaffDevelopmentSkillItem Entity
 * Date             :   29-Jun-2017
 * By               :   Ashis Kr. Das
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
    public interface IStaffDevelopmentSkillItemService :
                IBaseService<StaffDevelopmentSkillItemModel, Master_StaffDevelopmentSkillItem>
    {
        List<StaffDevelopmentSkillItemModel>        GetStaffDevelopmentSkillItemList(BaseViewModel dataModel);
        List<StaffDevelopmentSkillItemModel>        SaveStaffDevelopmentSkillItem(StaffDevelopmentSkillItemModel dataModel);
        List<StaffDevelopmentSkillItemModel>        DeleteStaffDevelopmentSkillItem(StaffDevelopmentSkillItemModel dataModel);
    }

    public class StaffDevelopmentSkillItemService : BaseService<StaffDevelopmentSkillItemModel, Master_StaffDevelopmentSkillItem>,
                                                                IStaffDevelopmentSkillItemService
    {
        private readonly IStaffDevelopmentSkillItemRepository repository;
        public StaffDevelopmentSkillItemService(IStaffDevelopmentSkillItemRepository repo) : base(repo)
        {
            repository = repo;
        }

        public List<StaffDevelopmentSkillItemModel> GetStaffDevelopmentSkillItemList(BaseViewModel dataModel)
        {
            object pvalue;
            StaffDevelopmentSkillItemModel model;
            List<Master_StaffDevelopmentSkillItem> datalist;
            List<StaffDevelopmentSkillItemModel> modlist = new List<StaffDevelopmentSkillItemModel>();

            try
            {
                datalist = repository.GetStaffDevelopmentSkillItemList();
                if (datalist != null)
                {
                    foreach (Master_StaffDevelopmentSkillItem data in datalist)
                    {
                        if (data.IsDeleted == true)
                            continue;

                        model = Mapper.Map<Master_StaffDevelopmentSkillItem, StaffDevelopmentSkillItemModel> (data);
                        model.DevSkillModel = Mapper.Map<Master_StaffDevelopmentSkill, StaffDevelopmentSkillModel> (data.Master_StaffDevelopmentSkill);

                        if (model.DevSkillModel != null)
                        {
                            pvalue = Utility.GetPropertyValue(model.DevSkillModel, "Name", dataModel.CurrentCulture);
                            model.DevSkillModel.Name = pvalue == null ? string.Empty : pvalue.ToString();

                            pvalue = Utility.GetPropertyValue(model.DevSkillModel, "Description", dataModel.CurrentCulture);
                            model.DevSkillModel.Description = pvalue == null ? string.Empty : pvalue.ToString();

                            model.DevSkillModel.CurrentCulture = dataModel.CurrentCulture;
                            model.DevSkillModel.CurrentUserID = dataModel.CurrentUserID;
                        }

                        pvalue = Utility.GetPropertyValue(model, "Name", dataModel.CurrentCulture);
                        model.Name = pvalue == null ? string.Empty : pvalue.ToString();

                        pvalue = Utility.GetPropertyValue(model, "Description", dataModel.CurrentCulture);
                        model.Description = pvalue == null ? string.Empty : pvalue.ToString();

                        model.CurrentCulture = dataModel.CurrentCulture;
                        model.CurrentUserID = dataModel.CurrentUserID;
                        modlist.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            modlist.Sort(CompareStaffDevelopmentSkillItemByName);
            return modlist;
        }

        public List<StaffDevelopmentSkillItemModel> SaveStaffDevelopmentSkillItem(StaffDevelopmentSkillItemModel dataModel)
        {
            Master_StaffDevelopmentSkillItem data;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                data = Mapper.Map<StaffDevelopmentSkillItemModel, Master_StaffDevelopmentSkillItem>(dataModel);

                if (dataModel.ID == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = dataModel.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    repository.InsertStaffDevelopmentSkillItem(data);
                }
                else
                {
                    data.UpdatedBy = dataModel.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    repository.UpdateStaffDevelopmentSkillItem(data);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = dataModel.CurrentCulture;
            md.CurrentUserID = dataModel.CurrentUserID;
            return GetStaffDevelopmentSkillItemList(md);
        }

        public List<StaffDevelopmentSkillItemModel> DeleteStaffDevelopmentSkillItem(StaffDevelopmentSkillItemModel dataModel)
        {
            List<StaffDevelopmentSkillItemModel> datalist;
            datalist = null;

            try
            {
                dataModel.IsDeleted = true;
                datalist = this.SaveStaffDevelopmentSkillItem(dataModel);
            }
            catch (Exception ex)
            {
                LogException(ex, dataModel.CurrentUserID);
                throw new Exception(ex.Message);
            }
            return datalist;
        }


        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "StaffDevelopmentSkillItem", message);
            return message;
        }

        private int CompareStaffDevelopmentSkillItemByName(StaffDevelopmentSkillItemModel dataModel1, StaffDevelopmentSkillItemModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
