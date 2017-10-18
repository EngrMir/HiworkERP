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
     
      public partial interface IStaffPatentFieldService : IBaseService<StaffPatentFieldModel, Master_StaffPatentField>
    {
        List<StaffPatentFieldModel> SaveStaffPatentField(StaffPatentFieldModel model);
        List<StaffPatentFieldModel> GetAllStaffPatentFieldList(BaseViewModel model);
        List<StaffPatentFieldModel> DeleteStaffPatentField(StaffPatentFieldModel model);
    }
    public class StaffPatentFieldService : BaseService<StaffPatentFieldModel, Master_StaffPatentField>, IStaffPatentFieldService
    {
        private readonly IStaffPatentFieldRepository _StaffPatentFieldRepository;
        public StaffPatentFieldService(IStaffPatentFieldRepository StaffPatentFieldRepository) : base(StaffPatentFieldRepository)
        {
            _StaffPatentFieldRepository = StaffPatentFieldRepository;
        }
        public List<StaffPatentFieldModel> SaveStaffPatentField(StaffPatentFieldModel aStaffPatentFieldModel)
        {
            List<StaffPatentFieldModel> StaffPatentFields = null;
            try
            {
                Utility.SetDynamicPropertyValue(aStaffPatentFieldModel, aStaffPatentFieldModel.CurrentCulture);
                var StaffPatentField = Mapper.Map<StaffPatentFieldModel, Master_StaffPatentField>(aStaffPatentFieldModel);

                if (aStaffPatentFieldModel.ID == Guid.Empty)
                {
                    StaffPatentField.ID = Guid.NewGuid();

                    StaffPatentField.CreatedBy = aStaffPatentFieldModel.CurrentUserID;
                    StaffPatentField.CreatedDate = DateTime.Now;
                    _StaffPatentFieldRepository.InsertStaffPatentField(StaffPatentField);
                }
                else
                {

                    StaffPatentField.UpdatedBy = aStaffPatentFieldModel.CurrentUserID;
                    StaffPatentField.UpdatedDate = DateTime.Now;
                    _StaffPatentFieldRepository.UpdateStaffPatentField(StaffPatentField);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aStaffPatentFieldModel.CurrentCulture;
                baseViewModel.CurrentUserID = aStaffPatentFieldModel.CurrentUserID;
                StaffPatentFields = GetAllStaffPatentFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffPatentFieldModel.CurrentUserID, "StaffPatentField", message);
                throw new Exception(ex.Message);
            }
            return StaffPatentFields;
        }
        public List<StaffPatentFieldModel> GetAllStaffPatentFieldList(BaseViewModel model)
        {
            List<StaffPatentFieldModel> StaffPatentFieldModelList = new List<StaffPatentFieldModel>();
            StaffPatentFieldModel StaffPatentFieldModel = new StaffPatentFieldModel();
            try
            {
                List<Master_StaffPatentField> StaffPatentFieldList = _StaffPatentFieldRepository.GetAllStaffPatentFieldList();
                if (StaffPatentFieldList != null)
                {
                    StaffPatentFieldList.ForEach(a =>
                    {
                        StaffPatentFieldModel = Mapper.Map<Master_StaffPatentField, StaffPatentFieldModel>(a);



                        StaffPatentFieldModel.Name = Utility.GetPropertyValue(StaffPatentFieldModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StaffPatentFieldModel, "Name", model.CurrentCulture).ToString();
                        StaffPatentFieldModel.Description = Utility.GetPropertyValue(StaffPatentFieldModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StaffPatentFieldModel, "Description", model.CurrentCulture).ToString();
                        StaffPatentFieldModel.CurrentUserID = model.CurrentUserID;

                        StaffPatentFieldModel.CurrentCulture = model.CurrentCulture;
                        StaffPatentFieldModelList.Add(StaffPatentFieldModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffPatentField", message);
                throw new Exception(ex.Message);
            }

            StaffPatentFieldModelList.Sort(CompareStaffPatentFieldByName);
            return StaffPatentFieldModelList;
        }
        public List<StaffPatentFieldModel> DeleteStaffPatentField(StaffPatentFieldModel model)
        {
            List<StaffPatentFieldModel> StaffPatentFields = null;
            try
            {
                _StaffPatentFieldRepository.DeleteStaffPatentField(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                StaffPatentFields = GetAllStaffPatentFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffPatentField", message);
                throw new Exception(ex.Message);
            }
            return StaffPatentFields;
        }

        private int CompareStaffPatentFieldByName(StaffPatentFieldModel arg1, StaffPatentFieldModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
