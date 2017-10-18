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
    public partial interface IStaffNarrationTypeService : IBaseService<StaffNarrationTypeModel, Master_StaffNarrationType>
    {
        List<StaffNarrationTypeModel> SaveStaffNarrationType(StaffNarrationTypeModel model);
        List<StaffNarrationTypeModel> GetAllStaffNarrationTypeList(BaseViewModel model);
        List<StaffNarrationTypeModel> DeleteStaffNarrationType(StaffNarrationTypeModel model);
    }
    public class StaffNarrationTypeService : BaseService<StaffNarrationTypeModel, Master_StaffNarrationType>, IStaffNarrationTypeService
    {
        private readonly IStaffNarrationTypeRepository _staffNarrationTypeRepository;
        public StaffNarrationTypeService(IStaffNarrationTypeRepository StaffNarrationTypeRepository)
            : base(StaffNarrationTypeRepository)
        {
            _staffNarrationTypeRepository = StaffNarrationTypeRepository;
        }
        public List<StaffNarrationTypeModel> SaveStaffNarrationType(StaffNarrationTypeModel model)
        {
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var staffNarrationType = Mapper.Map<StaffNarrationTypeModel, Master_StaffNarrationType>(model);
                if (staffNarrationType.ID != Guid.Empty)
                {
                    _staffNarrationTypeRepository.UpdateStaffNarrationType(staffNarrationType);
                }
                else
                {
                    staffNarrationType.ID = Guid.NewGuid();
                    _staffNarrationTypeRepository.InsertStaffNarrationType(staffNarrationType);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffNarrationType", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllStaffNarrationTypeList(md);
        }
        public List<StaffNarrationTypeModel> GetAllStaffNarrationTypeList(BaseViewModel model)
        {
            List<StaffNarrationTypeModel> staffNarrationTypeList = new List<StaffNarrationTypeModel>();
            StaffNarrationTypeModel staffNarrationTypeModel = new StaffNarrationTypeModel();
            try
            {
                List<Master_StaffNarrationType> staffNarrationTypevList = _staffNarrationTypeRepository.GetAllStaffNarrationTypeList();
                if (staffNarrationTypevList != null)
                {
                    staffNarrationTypevList.ForEach(a =>
                    {
                        staffNarrationTypeModel = Mapper.Map<Master_StaffNarrationType, StaffNarrationTypeModel>(a);
                        staffNarrationTypeModel.CurrentUserID = model.CurrentUserID;
                        staffNarrationTypeModel.CurrentCulture = model.CurrentCulture;
                        staffNarrationTypeList.Add(staffNarrationTypeModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffNarrationType", message);
                throw new Exception(ex.Message);
            }
            return staffNarrationTypeList;
        }
        public List<StaffNarrationTypeModel> DeleteStaffNarrationType(StaffNarrationTypeModel model)
        {
            try
            {
                _staffNarrationTypeRepository.DeleteStaffNarrationType(model.ID);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffNarrationType", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllStaffNarrationTypeList(md);
        }
    }
}
