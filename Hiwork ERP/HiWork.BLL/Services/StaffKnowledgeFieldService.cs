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
    public partial interface IStaffKnowledgeFieldService : IBaseService<StaffKnowledgeFieldModel, Master_StaffKnowledgeField>
    {
        List<StaffKnowledgeFieldModel> SaveStaffKnowledgeField(StaffKnowledgeFieldModel model);
        List<StaffKnowledgeFieldModel> GetAllStaffKnowledgeFieldList(BaseViewModel model);
        List<StaffKnowledgeFieldModel> DeleteStaffKnowledgeField(StaffKnowledgeFieldModel model);
    }
    public class StaffKnowledgeFieldService : BaseService<StaffKnowledgeFieldModel, Master_StaffKnowledgeField>, IStaffKnowledgeFieldService
    {
        private readonly IStaffKnowledgeFieldRepository _StaffKnowledgeFieldRepository;
        public StaffKnowledgeFieldService(IStaffKnowledgeFieldRepository StaffKnowledgeFieldRepository) : base(StaffKnowledgeFieldRepository)
        {
            _StaffKnowledgeFieldRepository = StaffKnowledgeFieldRepository;
        }
        public List<StaffKnowledgeFieldModel> SaveStaffKnowledgeField(StaffKnowledgeFieldModel aStaffKnowledgeFieldModel)
        {
            List<StaffKnowledgeFieldModel> StaffKnowledgeFields = null;
            try
            {
                Utility.SetDynamicPropertyValue(aStaffKnowledgeFieldModel, aStaffKnowledgeFieldModel.CurrentCulture);
                var StaffKnowledgeField = Mapper.Map<StaffKnowledgeFieldModel, Master_StaffKnowledgeField>(aStaffKnowledgeFieldModel);

                if (aStaffKnowledgeFieldModel.ID == Guid.Empty)
                {
                    StaffKnowledgeField.ID = Guid.NewGuid();
                    
                    StaffKnowledgeField.CreatedBy = aStaffKnowledgeFieldModel.CurrentUserID;
                    StaffKnowledgeField.CreatedDate = DateTime.Now;
                    _StaffKnowledgeFieldRepository.InsertStaffKnowledgeField(StaffKnowledgeField);
                }
                else
                {
                   
                    StaffKnowledgeField.UpdatedBy = aStaffKnowledgeFieldModel.CurrentUserID;
                    StaffKnowledgeField.UpdatedDate = DateTime.Now;
                    _StaffKnowledgeFieldRepository.UpdateStaffKnowledgeField(StaffKnowledgeField);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aStaffKnowledgeFieldModel.CurrentCulture;
                baseViewModel.CurrentUserID = aStaffKnowledgeFieldModel.CurrentUserID;
                StaffKnowledgeFields = GetAllStaffKnowledgeFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffKnowledgeFieldModel.CurrentUserID, "StaffKnowledgeField", message);
                throw new Exception(ex.Message);
            }
            return StaffKnowledgeFields;
        }
        public List<StaffKnowledgeFieldModel> GetAllStaffKnowledgeFieldList(BaseViewModel model)
        {
            List<StaffKnowledgeFieldModel> StaffKnowledgeFieldModelList = new List<StaffKnowledgeFieldModel>();
            StaffKnowledgeFieldModel StaffKnowledgeFieldModel = new StaffKnowledgeFieldModel();
            try
            {
                List<Master_StaffKnowledgeField> StaffKnowledgeFieldList = _StaffKnowledgeFieldRepository.GetAllStaffKnowledgeFieldList();
                if (StaffKnowledgeFieldList != null)
                {
                    StaffKnowledgeFieldList.ForEach(a =>
                    {
                        StaffKnowledgeFieldModel = Mapper.Map<Master_StaffKnowledgeField, StaffKnowledgeFieldModel>(a);
                      


                        StaffKnowledgeFieldModel.Name = Utility.GetPropertyValue(StaffKnowledgeFieldModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StaffKnowledgeFieldModel, "Name", model.CurrentCulture).ToString();
                        StaffKnowledgeFieldModel.Description = Utility.GetPropertyValue(StaffKnowledgeFieldModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StaffKnowledgeFieldModel, "Description", model.CurrentCulture).ToString();
                        StaffKnowledgeFieldModel.CurrentUserID = model.CurrentUserID;

                        StaffKnowledgeFieldModel.CurrentCulture = model.CurrentCulture;
                        StaffKnowledgeFieldModelList.Add(StaffKnowledgeFieldModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffKnowledgeField", message);
                throw new Exception(ex.Message);
            }

            StaffKnowledgeFieldModelList.Sort(CompareStaffKnowledgeFieldByName);
            return StaffKnowledgeFieldModelList;
        }
        public List<StaffKnowledgeFieldModel> DeleteStaffKnowledgeField(StaffKnowledgeFieldModel model)
        {
            List<StaffKnowledgeFieldModel> StaffKnowledgeFields = null;
            try
            {
                _StaffKnowledgeFieldRepository.DeleteStaffKnowledgeField(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                StaffKnowledgeFields = GetAllStaffKnowledgeFieldList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffKnowledgeField", message);
                throw new Exception(ex.Message);
            }
            return StaffKnowledgeFields;
        }

        private int CompareStaffKnowledgeFieldByName(StaffKnowledgeFieldModel arg1, StaffKnowledgeFieldModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
