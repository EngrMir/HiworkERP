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

    public partial interface IBusinessCategoryDetailsService : IBaseService<BusinessCategoryDetailsModel, Master_StaffBusinessCategoryDetails>
    {
        List<BusinessCategoryDetailsModel> SaveBusinessCategoryDetails(BusinessCategoryDetailsModel model);
        List<BusinessCategoryDetailsModel> GetAllBusinessCategoryDetailsList(BusinessCategoryDetailsModel model);
        List<BusinessCategoryDetailsModel> DeleteBusinessCategoryDetails(BusinessCategoryDetailsModel model);
    }

    public class BusinessCategoryDetailsService : BaseService<BusinessCategoryDetailsModel, Master_StaffBusinessCategoryDetails>, IBusinessCategoryDetailsService
    {


        private readonly IBusinessCategoryDetailsRepository _businessCategoryDetailsRepository;
        public BusinessCategoryDetailsService(IBusinessCategoryDetailsRepository businessCategoryDetailsRepository)
            : base(businessCategoryDetailsRepository)
        {
            _businessCategoryDetailsRepository = businessCategoryDetailsRepository;
        }
        public List<BusinessCategoryDetailsModel> SaveBusinessCategoryDetails(BusinessCategoryDetailsModel model)
        {
            List<BusinessCategoryDetailsModel> BusinessCategoryDetailss = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var BusinessCategoryDetails = Mapper.Map<BusinessCategoryDetailsModel, Master_StaffBusinessCategoryDetails>(model);
                if (BusinessCategoryDetails.ID != Guid.Empty)
                {
                    BusinessCategoryDetails.UpdatedBy = model.CurrentUserID;
                    BusinessCategoryDetails.UpdatedDate = DateTime.Now;
                    _businessCategoryDetailsRepository.UpdateBusinessCategoryDetails(BusinessCategoryDetails);
                }
                else
                {
                    BusinessCategoryDetails.ID = Guid.NewGuid();
                    BusinessCategoryDetails.CreatedBy = model.CurrentUserID;
                    BusinessCategoryDetails.CreatedDate = DateTime.Now;
                    _businessCategoryDetailsRepository.InsertBusinessCategoryDetails(BusinessCategoryDetails);
                }
                BusinessCategoryDetailss = GetAllBusinessCategoryDetailsList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BusinessCategoryDetails", message);
                throw new Exception(ex.Message);
            }
            return BusinessCategoryDetailss;
        }
        public List<BusinessCategoryDetailsModel> GetAllBusinessCategoryDetailsList(BusinessCategoryDetailsModel model)
        {
            List<BusinessCategoryDetailsModel> BusinessCategoryDetailsList = new List<BusinessCategoryDetailsModel>();
            BusinessCategoryDetailsModel BusinessCategoryDetailsModel = new BusinessCategoryDetailsModel();
            try
            {
                List<Master_StaffBusinessCategoryDetails> BusinessCategoryDetailsvList = _businessCategoryDetailsRepository.GetAllBusinessCategoryDetailsList();
                if (BusinessCategoryDetailsvList != null)
                {
                    BusinessCategoryDetailsvList.ForEach(a =>
                    {
                        BusinessCategoryDetailsModel = Mapper.Map<Master_StaffBusinessCategoryDetails, BusinessCategoryDetailsModel>(a);
                     
                        BusinessCategoryDetailsModel.BusinessCategory = Mapper.Map<Master_StaffBusinessCategory, BusinessCategoryModel>(a.Master_StaffBusinessCategory);
                        if (BusinessCategoryDetailsModel.BusinessCategory != null)
                            BusinessCategoryDetailsModel.BusinessCategory.Name = Utility.GetPropertyValue(BusinessCategoryDetailsModel.BusinessCategory, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(BusinessCategoryDetailsModel.BusinessCategory, "Name", model.CurrentCulture).ToString();

                        BusinessCategoryDetailsModel.Description = Utility.GetPropertyValue(BusinessCategoryDetailsModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(BusinessCategoryDetailsModel, "Description", model.CurrentCulture).ToString();
                        BusinessCategoryDetailsModel.CurrentUserID = model.CurrentUserID;
                        BusinessCategoryDetailsModel.CurrentCulture = model.CurrentCulture;
                        BusinessCategoryDetailsList.Add(BusinessCategoryDetailsModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BusinessCategoryDetails", message);
                throw new Exception(ex.Message);
            }
            return BusinessCategoryDetailsList;
        }

        public List<BusinessCategoryDetailsModel> DeleteBusinessCategoryDetails(BusinessCategoryDetailsModel model)
        {
            List<BusinessCategoryDetailsModel> BusinessCategoryDetails = null;
            try
            {
                _businessCategoryDetailsRepository.DeleteBusinessCategoryDetails(model.ID);
                BusinessCategoryDetails = GetAllBusinessCategoryDetailsList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BusinessCategoryDetails", message);
                throw new Exception(ex.Message);
            }
            return BusinessCategoryDetails;
        }
    }
}
