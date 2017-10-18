

/* ******************************************************************************************************************
 * Service for Master_CompanyIndustryClassificationItem Entity
 * Date             :   29-Jun-2017
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
    public interface ICompanyIndustryClassificationItemService :
                IBaseService<CompanyIndustryClassificationItemModel, Master_CompanyIndustryClassificationItem>
    {
        List<CompanyIndustryClassificationItemModel>
                    GetCompanyIndustryClassificationItemList(BaseViewModel dataModel);
        List<CompanyIndustryClassificationItemModel>
                    SaveCompanyIndustryClassificationItem(CompanyIndustryClassificationItemModel dataModel);
        List<CompanyIndustryClassificationItemModel>
            DeleteCompanyIndustryClassificationItem(CompanyIndustryClassificationItemModel dataModel);
    }

    public class CompanyIndustryClassificationItemService :
                BaseService <CompanyIndustryClassificationItemModel, Master_CompanyIndustryClassificationItem>,
                ICompanyIndustryClassificationItemService
    {
        private readonly ICompanyIndustryClassificationItemRepository repository;
        public CompanyIndustryClassificationItemService(ICompanyIndustryClassificationItemRepository repo) : base(repo)
        {
            repository = repo;
        }

        public List<CompanyIndustryClassificationItemModel>
                    GetCompanyIndustryClassificationItemList(BaseViewModel dataModel)
        {
            object pvalue;
            string svalue;
            CompanyIndustryClassificationItemModel model;
            List<Master_CompanyIndustryClassificationItem> datalist;
            List<CompanyIndustryClassificationItemModel> modlist = new List<CompanyIndustryClassificationItemModel>();

            try
            {
                datalist = repository.GetCompanyIndustryClassificationItemList();
                if (datalist != null)
                {
                    foreach(Master_CompanyIndustryClassificationItem data in datalist)
                    {
                        model = Mapper.Map<Master_CompanyIndustryClassificationItem, CompanyIndustryClassificationItemModel>
                                                (data);
                        model.CompanyIndustry = Mapper.Map<Master_CompanyIndustryClassification, CompanyIndustryClassificationModel>
                                                (data.Master_CompanyIndustryClassification);

                        if (model.CompanyIndustry != null)
                        {
                            pvalue = Utility.GetPropertyValue(model.CompanyIndustry, "Name", dataModel.CurrentCulture);
                            svalue = pvalue == null ? string.Empty : pvalue.ToString();
                            model.CompanyIndustry.Name = svalue;
                        }

                        pvalue = Utility.GetPropertyValue(model, "Name", dataModel.CurrentCulture);
                        svalue = pvalue == null ? string.Empty : pvalue.ToString();
                        model.Name = svalue;

                        pvalue = Utility.GetPropertyValue(model, "Description", dataModel.CurrentCulture);
                        svalue = pvalue == null ? string.Empty : pvalue.ToString();
                        model.Description = svalue;

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

            modlist.Sort(CompareCompanyIndustryClassificationItemByName);
            return modlist;
        }

        public List<CompanyIndustryClassificationItemModel>
                    SaveCompanyIndustryClassificationItem(CompanyIndustryClassificationItemModel dataModel)
        {
            Master_CompanyIndustryClassificationItem data;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                data = Mapper.Map
                            <CompanyIndustryClassificationItemModel, Master_CompanyIndustryClassificationItem>(dataModel);

                if (dataModel.Id == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = dataModel.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    repository.InsertCompanyIndustryClassificationItem(data);
                }
                else
                {
                    data.UpdatedBy = dataModel.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    repository.UpdateCompanyIndustryClassificationItem(data);
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
            return GetCompanyIndustryClassificationItemList(md);
        }

        public List<CompanyIndustryClassificationItemModel>
                    DeleteCompanyIndustryClassificationItem(CompanyIndustryClassificationItemModel dataModel)
        {
            try
            {
                repository.DeleteCompanyIndustryClassificationItem(dataModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = dataModel.CurrentCulture;
            md.CurrentUserID = dataModel.CurrentUserID;
            return GetCompanyIndustryClassificationItemList(md);
        }


        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "CompanyIndustryClassificationItem", message);
            return message;
        }

        private int CompareCompanyIndustryClassificationItemByName(CompanyIndustryClassificationItemModel dataModel1, CompanyIndustryClassificationItemModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
