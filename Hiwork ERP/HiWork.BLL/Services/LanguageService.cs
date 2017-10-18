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
    public partial interface ILanguageService : IBaseService<LanguageModel, Master_Language>
    {
        LanguageModel SaveLanguage(LanguageModel aLanguageModel);
        List<LanguageModel> GetAllLanguageList(BaseViewModel model);
        LanguageModel EditLanguage(LanguageModel aLanguageModel);
        List<LanguageModel> DeleteLanguage(LanguageModel aLanguageModel);
    }

    public class LanguageService : BaseService<LanguageModel, Master_Language>, ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        public LanguageService(ILanguageRepository languageRepository) : base(languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public LanguageModel SaveLanguage(LanguageModel aLanguageModel)
        {
            Master_Language language = null;

            try
            {
                Utility.SetDynamicPropertyValue(aLanguageModel, aLanguageModel.CurrentCulture);
                language = Mapper.Map<LanguageModel, Master_Language>(aLanguageModel);

                if (aLanguageModel.ID==Guid.Empty)
                {
                    language.ID = Guid.NewGuid();
                    
                    language.CreatedBy = aLanguageModel.CurrentUserID;
                    language.CreatedDate = DateTime.Now;
                    _languageRepository.InsertLanguage(language);
                }
                else
                {
                    language.UpdatedBy = aLanguageModel.CurrentUserID;
                    language.UpdatedDate = DateTime.Now;
                    _languageRepository.UpdateLanguage(language);
                }

                
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aLanguageModel.CurrentUserID, "Language", message);
                throw new Exception(ex.Message);
            }
            return aLanguageModel;
        }

  

        public List<LanguageModel> GetAllLanguageList(BaseViewModel model)
        {
            List<LanguageModel> languageList = new List<LanguageModel>();
            LanguageModel languageModel = new LanguageModel();

            try
            {

                List<Master_Language> repoLangList = _languageRepository.GetAllLanguageList();
                if (repoLangList != null)
                {
                    repoLangList.ForEach(a =>
                    {
                        languageModel = Mapper.Map<Master_Language, LanguageModel>(a);
                       
                        languageModel.Name = Utility.GetPropertyValue(languageModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(languageModel, "Name", model.CurrentCulture).ToString();
                        languageModel.CurrentUserID = model.CurrentUserID;

                        languageModel.CurrentCulture = model.CurrentCulture;
                        languageList.Add(languageModel);
                    });
                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Language", message);
                throw new Exception(ex.Message);
            }

            languageList.Sort(CompareLanguageByName);
            return languageList;
        }

        public LanguageModel EditLanguage(LanguageModel aLanguageModel)
        {
            var language = Mapper.Map<LanguageModel, Master_Language>(aLanguageModel);
            var aLanguage = _languageRepository.UpdateLanguage(language);
            var langModel = Mapper.Map<Master_Language, LanguageModel>(aLanguage);
            return langModel;
        }

        public List<LanguageModel> DeleteLanguage(LanguageModel aLanguageModel)
        {
            List<LanguageModel> Language=null;
            try
            {
                _languageRepository.DeleteLanguage(aLanguageModel.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aLanguageModel.CurrentCulture;
                baseViewModel.CurrentUserID = aLanguageModel.CurrentUserID;
                Language = GetAllLanguageList(baseViewModel);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aLanguageModel.CurrentUserID, "Language", message);
                throw new Exception(ex.Message);
            }
            return Language;
        }

        private int CompareLanguageByName(LanguageModel dataModel1, LanguageModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
