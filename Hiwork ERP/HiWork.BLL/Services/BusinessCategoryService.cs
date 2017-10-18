using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure.Contract;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;
using System;
using HiWork.DAL.Repositories;
using AutoMapper;
using HiWork.Utils;

namespace HiWork.BLL.Services
{
  public partial interface IBusinessCategoryService : IBaseService<BusinessCategoryModel, Master_StaffBusinessCategory>
    {
        List<BusinessCategoryModel> SaveBusinessCategory(BusinessCategoryModel model);
        List<BusinessCategoryModel> GetAllBusinessCategoryList(BusinessCategoryModel model);
        List<BusinessCategoryModel> DeleteBusinessCategory(BusinessCategoryModel model);
    }
    public class BusinessCategoryService : BaseService<BusinessCategoryModel, Master_StaffBusinessCategory>, IBusinessCategoryService
    {
        private readonly IBusinessCategoryRepository _BusinessCategoryRepository;
        public BusinessCategoryService(IBusinessCategoryRepository BusinessCategoryRepository)
            : base(BusinessCategoryRepository)
        {
            _BusinessCategoryRepository = BusinessCategoryRepository;
        }
        public List<BusinessCategoryModel> SaveBusinessCategory(BusinessCategoryModel model)
        {
            List<BusinessCategoryModel> BusinessCategorys = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var BusinessCategory = Mapper.Map<BusinessCategoryModel, Master_StaffBusinessCategory>(model);
                if (BusinessCategory.ID != Guid.Empty)
                {
                    BusinessCategory.UpdatedBy = model.CurrentUserID;
                    BusinessCategory.UpdatedDate = DateTime.Now;
                    _BusinessCategoryRepository.UpdateBusinessCategory(BusinessCategory);
                }
                else
                {
                    BusinessCategory.ID = Guid.NewGuid();
                    BusinessCategory.CreatedBy = model.CurrentUserID;
                    BusinessCategory.CreatedDate = DateTime.Now;
                    _BusinessCategoryRepository.InsertBusinessCategory(BusinessCategory);
                }
                BusinessCategorys = GetAllBusinessCategoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BusinessCategory", message);
                throw new Exception(ex.Message);
            }
            return BusinessCategorys;
        }
        public List<BusinessCategoryModel> GetAllBusinessCategoryList(BusinessCategoryModel model)
        {
            List<BusinessCategoryModel> BusinessCategoryList = new List<BusinessCategoryModel>();
            BusinessCategoryModel BusinessCategoryModel = new BusinessCategoryModel();
            try
            {
                List<Master_StaffBusinessCategory> BusinessCategoryvList = _BusinessCategoryRepository.GetAllBusinessCategoryList();
                if (BusinessCategoryvList != null)
                {
                    BusinessCategoryvList.ForEach(a =>
                    {
                        BusinessCategoryModel = Mapper.Map<Master_StaffBusinessCategory, BusinessCategoryModel>(a);
                        BusinessCategoryModel.Name = Utility.GetPropertyValue(BusinessCategoryModel, "Name", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(BusinessCategoryModel, "Name", model.CurrentCulture).ToString();
                        BusinessCategoryModel.CurrentUserID = model.CurrentUserID;
                        BusinessCategoryModel.CurrentCulture = model.CurrentCulture;
                        BusinessCategoryList.Add(BusinessCategoryModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BusinessCategory", message);
                throw new Exception(ex.Message);
            }
            return BusinessCategoryList;
        }

        public List<BusinessCategoryModel> DeleteBusinessCategory(BusinessCategoryModel model)
        {
            List<BusinessCategoryModel> BusinessCategorys = null;
            try
            {
                _BusinessCategoryRepository.DeleteBusinessCategory(model.ID);
                BusinessCategorys = GetAllBusinessCategoryList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BusinessCategory", message);
                throw new Exception(ex.Message);
            }
            return BusinessCategorys;
        }

    }
}


