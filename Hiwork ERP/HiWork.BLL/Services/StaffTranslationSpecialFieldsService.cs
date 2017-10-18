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
    public partial interface IStaffTranslationSpecialFieldsService : IBaseService<StaffTranslationSpecialFieldsModel, Staff_TranslationSpecialFields>
    {
        List<StaffTranslationSpecialFieldsModel> SaveStaffTranslationSpecialFields(StaffTranslationSpecialFieldsModel model);
        List<StaffTranslationSpecialFieldsModel> GetAllStaffTranslationSpecialFieldsList(BaseViewModel model);
        List<StaffTranslationSpecialFieldsModel> DeleteStaffTranslationSpecialFields(StaffTranslationSpecialFieldsModel model);
    }
    public class StaffTranslationSpecialFieldsService : BaseService<StaffTranslationSpecialFieldsModel, Staff_TranslationSpecialFields>, IStaffTranslationSpecialFieldsService
    {
        private readonly IStaffTranslationSpecialFieldsRepository _staffTranslationSpecialFieldsRepository;
        public StaffTranslationSpecialFieldsService(IStaffTranslationSpecialFieldsRepository staffTranslationSpecialFieldsRepository)
            : base(staffTranslationSpecialFieldsRepository)
        {
            _staffTranslationSpecialFieldsRepository = staffTranslationSpecialFieldsRepository;
        }
        public List<StaffTranslationSpecialFieldsModel> SaveStaffTranslationSpecialFields(StaffTranslationSpecialFieldsModel model)
        {
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var staffTranslationSpecialFields = Mapper.Map<StaffTranslationSpecialFieldsModel, Staff_TranslationSpecialFields>(model);
                if (staffTranslationSpecialFields.ID != Guid.Empty)
                {
                    _staffTranslationSpecialFieldsRepository.UpdateStaffTranslationSpecialFields(staffTranslationSpecialFields);
                }
                else
                {
                    staffTranslationSpecialFields.ID = Guid.NewGuid();
                    _staffTranslationSpecialFieldsRepository.InsertStaffTranslationSpecialFields(staffTranslationSpecialFields);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffTranslationSpecialFields", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllStaffTranslationSpecialFieldsList(md);
        }
        public List<StaffTranslationSpecialFieldsModel> GetAllStaffTranslationSpecialFieldsList(BaseViewModel model)
        {
            List<StaffTranslationSpecialFieldsModel> staffTranslationSpecialFieldsList = new List<StaffTranslationSpecialFieldsModel>();
            StaffTranslationSpecialFieldsModel staffTranslationSpecialFieldsModel = new StaffTranslationSpecialFieldsModel();
            try
            {
                List<Staff_TranslationSpecialFields> StaffTranslationSpecialFieldsvList = _staffTranslationSpecialFieldsRepository.GetAllStaffTranslationSpecialFieldsList();
                if (StaffTranslationSpecialFieldsvList != null)
                {
                    StaffTranslationSpecialFieldsvList.ForEach(a =>
                    {
                        staffTranslationSpecialFieldsModel = Mapper.Map<Staff_TranslationSpecialFields, StaffTranslationSpecialFieldsModel>(a);
                        staffTranslationSpecialFieldsModel.CurrentUserID = model.CurrentUserID;
                        staffTranslationSpecialFieldsModel.CurrentCulture = model.CurrentCulture;
                        staffTranslationSpecialFieldsList.Add(staffTranslationSpecialFieldsModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffTranslationSpecialFields", message);
                throw new Exception(ex.Message);
            }
            return staffTranslationSpecialFieldsList;
        }
        public List<StaffTranslationSpecialFieldsModel> DeleteStaffTranslationSpecialFields(StaffTranslationSpecialFieldsModel model)
        {
            try
            {
                _staffTranslationSpecialFieldsRepository.DeleteStaffTranslationSpecialFields(model.ID);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffTranslationSpecialFields", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllStaffTranslationSpecialFieldsList(md);
        }
    }
}
