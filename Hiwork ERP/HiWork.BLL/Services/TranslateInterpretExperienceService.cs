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
      public partial interface ITranslateInterpretExperienceService : IBaseService<TranslateInterpretExperienceModel, Staff_TranslateInterpretExperience>
    {
        List<TranslateInterpretExperienceModel> SaveTranslateInterpretExperience(TranslateInterpretExperienceModel model);
        List<TranslateInterpretExperienceModel> GetAllTranslateInterpretExperienceList(BaseViewModel model);
        List<TranslateInterpretExperienceModel> DeleteTranslateInterpretExperience(TranslateInterpretExperienceModel model);
    }

    public class TranslateInterpretExperienceService : BaseService<TranslateInterpretExperienceModel, Staff_TranslateInterpretExperience>, ITranslateInterpretExperienceService
    {
        private readonly ITranslateInterpretExperienceRepository _translateInterpretExperienceRepository;
        public TranslateInterpretExperienceService(ITranslateInterpretExperienceRepository TranslateInterpretExperienceRepository)
            : base(TranslateInterpretExperienceRepository)
        {
            _translateInterpretExperienceRepository = TranslateInterpretExperienceRepository;
        }
        public List<TranslateInterpretExperienceModel> SaveTranslateInterpretExperience(TranslateInterpretExperienceModel model)
        {
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var TranslateInterpretExperience = Mapper.Map<TranslateInterpretExperienceModel, Staff_TranslateInterpretExperience>(model);
                if (TranslateInterpretExperience.ID != Guid.Empty)
                {                    
                    _translateInterpretExperienceRepository.UpdateTranslateInterpretExperience(TranslateInterpretExperience);
                }
                else
                {
                    TranslateInterpretExperience.ID = Guid.NewGuid();
                    _translateInterpretExperienceRepository.InsertTranslateInterpretExperience(TranslateInterpretExperience);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TranslateInterpretExperience", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllTranslateInterpretExperienceList(md);
        }
        public List<TranslateInterpretExperienceModel> GetAllTranslateInterpretExperienceList(BaseViewModel model)
        {
            List<TranslateInterpretExperienceModel> translateInterpretExperienceList = new List<TranslateInterpretExperienceModel>();
            TranslateInterpretExperienceModel translateInterpretExperienceModel = new TranslateInterpretExperienceModel();
            try
            {
                List<Staff_TranslateInterpretExperience> translateInterpretExperiencevList = _translateInterpretExperienceRepository.GetAllTranslateInterpretExperienceList();
                if (translateInterpretExperiencevList != null)
                {
                    translateInterpretExperiencevList.ForEach(a =>
                    {
                        translateInterpretExperienceModel = Mapper.Map<Staff_TranslateInterpretExperience, TranslateInterpretExperienceModel>(a);
                        translateInterpretExperienceModel.CurrentUserID = model.CurrentUserID;
                        translateInterpretExperienceModel.CurrentCulture = model.CurrentCulture;
                        translateInterpretExperienceList.Add(translateInterpretExperienceModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TranslateInterpretExperience", message);
                throw new Exception(ex.Message);
            }
            return translateInterpretExperienceList;
        }
        public List<TranslateInterpretExperienceModel> DeleteTranslateInterpretExperience(TranslateInterpretExperienceModel model)
        {
            try
            {
                _translateInterpretExperienceRepository.DeleteTranslateInterpretExperience(model.ID);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TranslateInterpretExperience", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllTranslateInterpretExperienceList(md);
        }
    }
}
