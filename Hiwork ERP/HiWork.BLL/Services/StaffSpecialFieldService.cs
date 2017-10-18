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
     public partial interface IStaffSpecialFieldService : IBaseService<StaffSpecialFieldModel, Master_StaffSpecialField>
      {
        List<StaffSpecialFieldModel> SaveStaffSpecialField(StaffSpecialFieldModel model);
        List<StaffSpecialFieldModel> GetAllStaffSpecialFieldList(BaseViewModel model);
        List<StaffSpecialFieldModel> DeleteStaffSpecialField(StaffSpecialFieldModel model);
     }
    public class StaffSpecialFieldService : BaseService<StaffSpecialFieldModel, Master_StaffSpecialField>, IStaffSpecialFieldService
    {
        private readonly IStaffSpecialFieldRepository _staffSpecialFieldRepository;
        public StaffSpecialFieldService(IStaffSpecialFieldRepository staffSpecialFieldRepository) : base(staffSpecialFieldRepository)
        {
            _staffSpecialFieldRepository = staffSpecialFieldRepository;
        }
        public List<StaffSpecialFieldModel> SaveStaffSpecialField(StaffSpecialFieldModel model)
        {
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var staffSpecialField = Mapper.Map<StaffSpecialFieldModel, Master_StaffSpecialField>(model);

                if (model.ID == Guid.Empty)
                {
                    staffSpecialField.ID = Guid.NewGuid();
                    _staffSpecialFieldRepository.InsertStaffSpecialField(staffSpecialField);
                }
                else
                {
                    _staffSpecialFieldRepository.UpdateStaffSpecialField(staffSpecialField);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffSpecialField", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            return GetAllStaffSpecialFieldList(baseViewModel);
        }
        public List<StaffSpecialFieldModel> GetAllStaffSpecialFieldList(BaseViewModel model)
        {
            List<StaffSpecialFieldModel> staffSpecialFieldModelList = new List<StaffSpecialFieldModel>();
            StaffSpecialFieldModel staffSpecialFieldModel = new StaffSpecialFieldModel();
            try
            {
                List<Master_StaffSpecialField> staffSpecialFieldList = _staffSpecialFieldRepository.GetAllStaffSpecialFieldList();
                if (staffSpecialFieldList != null)
                {
                    staffSpecialFieldList.ForEach(a =>
                    {
                        staffSpecialFieldModel = Mapper.Map<Master_StaffSpecialField, StaffSpecialFieldModel>(a);
                        staffSpecialFieldModel.Name = Utility.GetPropertyValue(staffSpecialFieldModel, "Name", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(staffSpecialFieldModel, "Name", model.CurrentCulture).ToString();
                        staffSpecialFieldModel.Description = Utility.GetPropertyValue(staffSpecialFieldModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(staffSpecialFieldModel, "Description", model.CurrentCulture).ToString();
                        staffSpecialFieldModel.CurrentUserID = model.CurrentUserID;
                        staffSpecialFieldModel.CurrentCulture = model.CurrentCulture;
                        staffSpecialFieldModelList.Add(staffSpecialFieldModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffSpecialField", message);
                throw new Exception(ex.Message);
            }

            staffSpecialFieldModelList.Sort(CompareStaffSpecialFieldByName);
            return staffSpecialFieldModelList;
        }
        public List<StaffSpecialFieldModel> DeleteStaffSpecialField(StaffSpecialFieldModel model)
        {
            List<StaffSpecialFieldModel> staffSpecialFields = null;
            try
            {
                _staffSpecialFieldRepository.DeleteStaffSpecialField(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                staffSpecialFields = GetAllStaffSpecialFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffSpecialField", message);
                throw new Exception(ex.Message);
            }
            return staffSpecialFields;
        }

        private int CompareStaffSpecialFieldByName(StaffSpecialFieldModel arg1, StaffSpecialFieldModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Description, true);
            return cmpresult;
        }
    }
}
