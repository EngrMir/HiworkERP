

/* ******************************************************************************************************************
 * Service for Master_CompanyIndustryClassification Entity
 * Date             :   24-Jun-2017
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
    public interface ICompanyIndustryClassificationService :
            IBaseService<CompanyIndustryClassificationModel, Master_CompanyIndustryClassification>
    {
        List<CompanyIndustryClassificationModel>
                SaveCompanyIndustryClassification(CompanyIndustryClassificationModel arg);
        List<CompanyIndustryClassificationModel>
                GetCompanyIndustryClassificationList(BaseViewModel arg);
        List<CompanyIndustryClassificationModel>
                DeleteCompanyIndustryClassification(CompanyIndustryClassificationModel arg);
    }
    public class CompanyIndustryClassificationService :
            BaseService<CompanyIndustryClassificationModel, Master_CompanyIndustryClassification>,
            ICompanyIndustryClassificationService
    {
        private readonly ICompanyIndustryClassificationRepository repository;
        public CompanyIndustryClassificationService(ICompanyIndustryClassificationRepository arg) : base(arg)
        {
            repository = arg;
        }

        public List<CompanyIndustryClassificationModel>
            SaveCompanyIndustryClassification(CompanyIndustryClassificationModel arg)
        {
            Master_CompanyIndustryClassification data;

            try
            {
                Utility.SetDynamicPropertyValue(arg, arg.CurrentCulture);
                data = Mapper.Map<CompanyIndustryClassificationModel, Master_CompanyIndustryClassification>(arg);

                if (arg.Id == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = arg.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    repository.InsertCompanyIndustryClassification(data);
                }
                else
                {
                    data.UpdatedBy = arg.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    repository.UpdateCompanyIndustryClassification(data);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, arg.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = arg.CurrentUserID;
            md.CurrentCulture = arg.CurrentCulture;
            return this.GetCompanyIndustryClassificationList(md);
        }

        public List<CompanyIndustryClassificationModel> GetCompanyIndustryClassificationList(BaseViewModel _baseViewModel)
        {

            List<CompanyIndustryClassificationModel> modelList = new List<CompanyIndustryClassificationModel>();
            CompanyIndustryClassificationModel objModel;
            CompanyIndustryClassificationItemModel objItemModel;
            List<CompanyIndustryClassificationItemModel> objItemModelList;

            try
            {
              var  datalist = repository.GetCompanyIndustryClassificationList();
                if (datalist != null)
                {
                    datalist.ForEach(a => {

                        objModel = new CompanyIndustryClassificationModel();
                        objModel = Mapper.Map<Master_CompanyIndustryClassification, CompanyIndustryClassificationModel>(a);
                        objModel.Name = Utility.GetPropertyValue(objModel, "Name", _baseViewModel.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(objModel, "Name", _baseViewModel.CurrentCulture).ToString();
                        objModel.Description= Utility.GetPropertyValue(objModel, "Description", _baseViewModel.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(objModel, "Description", _baseViewModel.CurrentCulture).ToString();
                        
                        objItemModelList = new List<CompanyIndustryClassificationItemModel>();

                        foreach (var childItem in a.Master_CompanyIndustryClassificationItem)
                        {
                            objItemModel = new CompanyIndustryClassificationItemModel();
                            objItemModel = Mapper.Map<Master_CompanyIndustryClassificationItem, CompanyIndustryClassificationItemModel>(childItem);
                            objItemModel.Name = Utility.GetPropertyValue(objItemModel, "Name", _baseViewModel.CurrentCulture) == null ? string.Empty :
                                                        Utility.GetPropertyValue(objItemModel, "Name", _baseViewModel.CurrentCulture).ToString();

                            objItemModel.CurrentCulture = _baseViewModel.CurrentCulture;
                            objItemModel.CurrentUserID = _baseViewModel.CurrentUserID;
                            objItemModelList.Add(objItemModel);
                        }

                        // var childList = a.Master_CompanyIndustryClassificationItem.GetEnumerator();
                        objItemModelList.Sort(CompareCompanyIndustryClassificationItemByName);
                        objModel.itemList = objItemModelList;

                        objModel.CurrentCulture = _baseViewModel.CurrentCulture;
                        objModel.CurrentUserID = _baseViewModel.CurrentUserID;
                        modelList.Add(objModel);
                    });
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, _baseViewModel.CurrentUserID);
                throw new Exception(message);
            }

            modelList.Sort(CompareCompanyIndustryClassificationByName);
            return modelList;
        }

        public List<CompanyIndustryClassificationModel>
                DeleteCompanyIndustryClassification(CompanyIndustryClassificationModel arg)
        {
            try
            {
                repository.DeleteCompanyIndustryClassification(arg.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, arg.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = arg.CurrentUserID;
            md.CurrentCulture = arg.CurrentCulture;
            return this.GetCompanyIndustryClassificationList(md);
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "CompanyIndustryClassification", message);
            return message;
        }

        private int CompareCompanyIndustryClassificationByName(CompanyIndustryClassificationModel arg1, CompanyIndustryClassificationModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
        private int CompareCompanyIndustryClassificationItemByName(CompanyIndustryClassificationItemModel dataModel1, CompanyIndustryClassificationItemModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
