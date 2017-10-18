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
    public partial interface IStaffTranslationFieldsService : IBaseService<StaffTranslationFieldsModel, Master_StaffTranslationFields>
    {
        List<StaffTranslationFieldsModel> SaveStaffTranslationFields(StaffTranslationFieldsModel aStaffTranslationFieldsModel);
        List<StaffTranslationFieldsModel> GetAllStaffTranslationFieldsList(BaseViewModel aStaffTranslationFieldsModel);
        List<StaffTranslationFieldsModel> EditStaffTranslationFields(StaffTranslationFieldsModel aStaffTranslationFieldsModel);
        List<StaffTranslationFieldsModel> DeleteStaffTranslationFields(StaffTranslationFieldsModel aStaffTranslationFieldsModel);
    }

    public class StaffTranslationFieldsService : BaseService<StaffTranslationFieldsModel, Master_StaffTranslationFields>, IStaffTranslationFieldsService
    {
        private readonly IStaffTranslationFieldsRepository _translationFieldsRepository;
        public StaffTranslationFieldsService(IStaffTranslationFieldsRepository translationFieldsRepository) : base(translationFieldsRepository)
        {
            _translationFieldsRepository = translationFieldsRepository;
        }

        public List<StaffTranslationFieldsModel> SaveStaffTranslationFields(StaffTranslationFieldsModel aStaffTranslationFieldsModel)
        {
            Master_StaffTranslationFields translationfields = null;

            try
            {
                Utility.SetDynamicPropertyValue(aStaffTranslationFieldsModel, aStaffTranslationFieldsModel.CurrentCulture);
                translationfields = Mapper.Map<StaffTranslationFieldsModel, Master_StaffTranslationFields>(aStaffTranslationFieldsModel);

                if (aStaffTranslationFieldsModel.IsNew())
                {
                    translationfields.ID = Guid.NewGuid();
                    translationfields.CreatedBy = aStaffTranslationFieldsModel.CurrentUserID;
                    translationfields.CreatedDate = DateTime.Now;
                    _translationFieldsRepository.InserStaffTranslationFields(translationfields);
                }
                else
                {
                    translationfields.UpdatedBy = aStaffTranslationFieldsModel.CurrentUserID;
                    translationfields.UpdatedDate = DateTime.Now;
                    _translationFieldsRepository.UpdateStaffTranslationFields(translationfields);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffTranslationFieldsModel.CurrentUserID, "StaffTranslationFields", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = aStaffTranslationFieldsModel.CurrentUserID;
            md.CurrentCulture = aStaffTranslationFieldsModel.CurrentCulture;
            return GetAllStaffTranslationFieldsList(md);
        }

        public List<StaffTranslationFieldsModel> EditStaffTranslationFields(StaffTranslationFieldsModel aStaffTranslationFieldsModel)
        {
            try
            {
                var transfields = Mapper.Map<StaffTranslationFieldsModel, Master_StaffTranslationFields>(aStaffTranslationFieldsModel);
                Master_StaffTranslationFields aStaffTranslationFields = _translationFieldsRepository.UpdateStaffTranslationFields(transfields);
                StaffTranslationFieldsModel majorsubModel = Mapper.Map<Master_StaffTranslationFields, StaffTranslationFieldsModel>(aStaffTranslationFields);

            }

            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffTranslationFieldsModel.CurrentUserID, "StaffTranslationFields", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = aStaffTranslationFieldsModel.CurrentUserID;
            md.CurrentCulture = aStaffTranslationFieldsModel.CurrentCulture;
            return GetAllStaffTranslationFieldsList(md);
        }

        public List<StaffTranslationFieldsModel> GetAllStaffTranslationFieldsList(BaseViewModel model)
        {
            List<StaffTranslationFieldsModel> translationFieldsList = new List<StaffTranslationFieldsModel>();
            StaffTranslationFieldsModel StafftransModel = new StaffTranslationFieldsModel();

            try
            {

                List<Master_StaffTranslationFields> repoStaffTranslationFieldsList = _translationFieldsRepository.GetAllStaffTranslationFieldsList();
                if (repoStaffTranslationFieldsList != null)
                {
                    foreach (Master_StaffTranslationFields msm in repoStaffTranslationFieldsList)
                    {

                        StafftransModel = Mapper.Map<Master_StaffTranslationFields, StaffTranslationFieldsModel>(msm);

                        StafftransModel.Name = Utility.GetPropertyValue(StafftransModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StafftransModel, "Name", model.CurrentCulture).ToString();
                        StafftransModel.Description = Utility.GetPropertyValue(StafftransModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(StafftransModel, "Description", model.CurrentCulture).ToString();
                        StafftransModel.CurrentUserID = model.CurrentUserID;
                        StafftransModel.CurrentCulture = StafftransModel.CurrentCulture;
                        StafftransModel.CurrentUserID = StafftransModel.CurrentUserID;


                        translationFieldsList.Add(StafftransModel);
                    }

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "StaffTranslationFields", message);
                throw new Exception(ex.Message);
            }

            return translationFieldsList;
        }
        public List<StaffTranslationFieldsModel> DeleteStaffTranslationFields(StaffTranslationFieldsModel aStaffTranslationFieldsModel)
        {
            try
            {
                _translationFieldsRepository.DeleteStaffTranslationFields(aStaffTranslationFieldsModel.ID);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aStaffTranslationFieldsModel.CurrentUserID, "StaffTranslationFields", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = aStaffTranslationFieldsModel.CurrentUserID;
            md.CurrentCulture = aStaffTranslationFieldsModel.CurrentCulture;
            return GetAllStaffTranslationFieldsList(md);
        }
    }
}
