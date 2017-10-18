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
      public partial interface IStaffMedicalFieldService : IBaseService<StaffMedicalFieldModel, Master_StaffMedicalField>
    {
        List<StaffMedicalFieldModel> SaveStaffMedicalField(StaffMedicalFieldModel model);
        List<StaffMedicalFieldModel> GetAllStaffMedicalFieldList(BaseViewModel model);
        List<StaffMedicalFieldModel> DeleteStaffMedicalField(StaffMedicalFieldModel model);
    }
    public class StaffMedicalFieldService : BaseService<StaffMedicalFieldModel, Master_StaffMedicalField>, IStaffMedicalFieldService
    {
        private readonly IStaffMedicalFieldRepository _StaffMedicalFieldRepository;
        public StaffMedicalFieldService(IStaffMedicalFieldRepository StaffMedicalFieldRepository) : base(StaffMedicalFieldRepository)
        {
            _StaffMedicalFieldRepository = StaffMedicalFieldRepository;
        }
        public List<StaffMedicalFieldModel> SaveStaffMedicalField(StaffMedicalFieldModel aStaffMedicalFieldModel)
        {
            List<StaffMedicalFieldModel> StaffMedicalFields = null;
            try
            {
                Utility.SetDynamicPropertyValue(aStaffMedicalFieldModel, aStaffMedicalFieldModel.CurrentCulture);
                var StaffMedicalField = Mapper.Map<StaffMedicalFieldModel, Master_StaffMedicalField>(aStaffMedicalFieldModel);

                if (aStaffMedicalFieldModel.ID == Guid.Empty)
                {
                    StaffMedicalField.ID = Guid.NewGuid();

                    StaffMedicalField.CreatedBy = aStaffMedicalFieldModel.CurrentUserID;
                    StaffMedicalField.CreatedDate = DateTime.Now;
                    _StaffMedicalFieldRepository.InsertStaffMedicalField(StaffMedicalField);
                }
                else
                {

                    StaffMedicalField.UpdatedBy = aStaffMedicalFieldModel.CurrentUserID;
                    StaffMedicalField.UpdatedDate = DateTime.Now;
                    _StaffMedicalFieldRepository.UpdateStaffMedicalField(StaffMedicalField);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aStaffMedicalFieldModel.CurrentCulture;
                baseViewModel.CurrentUserID = aStaffMedicalFieldModel.CurrentUserID;
                StaffMedicalFields = GetAllStaffMedicalFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffMedicalFieldModel.CurrentUserID, "StaffMedicalField", message);
                throw new Exception(ex.Message);
            }
            return StaffMedicalFields;
        }
        public List<StaffMedicalFieldModel> GetAllStaffMedicalFieldList(BaseViewModel model)
        {
            List<StaffMedicalFieldModel> StaffMedicalFieldModelList = new List<StaffMedicalFieldModel>();
            StaffMedicalFieldModel StaffMedicalFieldModel = new StaffMedicalFieldModel();
            try
            {
                List<Master_StaffMedicalField> StaffMedicalFieldList = _StaffMedicalFieldRepository.GetAllStaffMedicalFieldList();
                if (StaffMedicalFieldList != null)
                {
                    StaffMedicalFieldList.ForEach(a =>
                    {
                        StaffMedicalFieldModel = Mapper.Map<Master_StaffMedicalField, StaffMedicalFieldModel>(a);



                        StaffMedicalFieldModel.Name = Utility.GetPropertyValue(StaffMedicalFieldModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StaffMedicalFieldModel, "Name", model.CurrentCulture).ToString();
                        StaffMedicalFieldModel.Description = Utility.GetPropertyValue(StaffMedicalFieldModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StaffMedicalFieldModel, "Description", model.CurrentCulture).ToString();
                        StaffMedicalFieldModel.CurrentUserID = model.CurrentUserID;

                        StaffMedicalFieldModel.CurrentCulture = model.CurrentCulture;
                        StaffMedicalFieldModelList.Add(StaffMedicalFieldModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffMedicalField", message);
                throw new Exception(ex.Message);
            }

            StaffMedicalFieldModelList.Sort(CompareStaffMedicalFieldByName);
            return StaffMedicalFieldModelList;
        }
        public List<StaffMedicalFieldModel> DeleteStaffMedicalField(StaffMedicalFieldModel model)
        {
            List<StaffMedicalFieldModel> StaffMedicalFields = null;
            try
            {
                _StaffMedicalFieldRepository.DeleteStaffMedicalField(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                StaffMedicalFields = GetAllStaffMedicalFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffMedicalField", message);
                throw new Exception(ex.Message);
            }
            return StaffMedicalFields;
        }

        private int CompareStaffMedicalFieldByName(StaffMedicalFieldModel arg1, StaffMedicalFieldModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
