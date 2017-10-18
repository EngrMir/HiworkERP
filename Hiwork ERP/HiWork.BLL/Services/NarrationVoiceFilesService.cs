using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{
    public partial interface INarrationVoiceFilesService : IBaseService<NarrationVoiceFilesModel, Staff_NarrationVoiceFiles>
    {
        List<NarrationVoiceFilesModel> SaveNarrationVoiceFiles(NarrationVoiceFilesModel narrationVoiceFilesModel);
        List<NarrationVoiceFilesModel> GetAllNarrationVoiceFileList(BaseViewModel model);
        List<NarrationVoiceFilesModel> DeleteNarrationVoiceFile(NarrationVoiceFilesModel model);
    }
    public class NarrationVoiceFilesService : BaseService<NarrationVoiceFilesModel, Staff_NarrationVoiceFiles>, INarrationVoiceFilesService
    {
        private readonly INarrationVoiceFilesRepository _NarrationVoiceFilesRepository;
        public NarrationVoiceFilesService(INarrationVoiceFilesRepository NarrationVoiceFilesRepository) : base(NarrationVoiceFilesRepository)
        {
            _NarrationVoiceFilesRepository = NarrationVoiceFilesRepository;
        }
        public List<NarrationVoiceFilesModel> SaveNarrationVoiceFiles(NarrationVoiceFilesModel narrationVoiceFilesModel)
        {
            List<NarrationVoiceFilesModel> narrationVoiceFile = null;
            try
            {
                Utility.SetDynamicPropertyValue(narrationVoiceFilesModel, narrationVoiceFilesModel.CurrentCulture);
                var narrationVoiceFiles = Mapper.Map<NarrationVoiceFilesModel, Staff_NarrationVoiceFiles>(narrationVoiceFilesModel);

                if (narrationVoiceFilesModel.ID == Guid.Empty)
                {
                    narrationVoiceFilesModel.ID = Guid.NewGuid();
                    _NarrationVoiceFilesRepository.InsertNarrationVoiceFiles(narrationVoiceFiles);
                }                
                else
                {
                    _NarrationVoiceFilesRepository.UpdateNarrationVoiceFiles(narrationVoiceFiles);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = narrationVoiceFilesModel.CurrentCulture;
                baseViewModel.CurrentUserID = narrationVoiceFilesModel.CurrentUserID;
                narrationVoiceFile = GetAllNarrationVoiceFileList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(narrationVoiceFilesModel.CurrentUserID, "NarrationVoiceFiles", message);
                throw new Exception(ex.Message);
            }
            return narrationVoiceFile;
        }

        public List<NarrationVoiceFilesModel> GetAllNarrationVoiceFileList(BaseViewModel model)
        {
            List<NarrationVoiceFilesModel> narrationVoiceFileList = new List<NarrationVoiceFilesModel>();
            NarrationVoiceFilesModel narrationVoiceFilesModel = new NarrationVoiceFilesModel();
            try
            {
                List<Staff_NarrationVoiceFiles> nvFileList = _NarrationVoiceFilesRepository.GetAllNarrationVoiceFileList();
                if (nvFileList != null)
                {
                    nvFileList.ForEach(a =>
                    {
                        narrationVoiceFilesModel = Mapper.Map<Staff_NarrationVoiceFiles, NarrationVoiceFilesModel>(a);
                        narrationVoiceFilesModel.CurrentUserID = model.CurrentUserID;
                        narrationVoiceFilesModel.CurrentCulture = model.CurrentCulture;
                        narrationVoiceFileList.Add(narrationVoiceFilesModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "NarrationVoiceFiles", message);
                throw new Exception(ex.Message);
            }

            //narrationVoiceFileList.Sort(CompareNarrationVoiceFilesByName);
            return narrationVoiceFileList;
        }
        public List<NarrationVoiceFilesModel> DeleteNarrationVoiceFile(NarrationVoiceFilesModel model)
        {
            List<NarrationVoiceFilesModel> narrationVoiceFiles = null;
            try
            {
                _NarrationVoiceFilesRepository.DeleteNarrationVoiceFiles(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                narrationVoiceFiles = GetAllNarrationVoiceFileList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "NarrationVoiceFiles", message);
                throw new Exception(ex.Message);
            }
            return narrationVoiceFiles;
        }

        //private int CompareNarrationVoiceFilesByName(NarrationVoiceFilesModel arg1, NarrationVoiceFilesModel arg2)
        //{
        //    int cmpresult;
        //    cmpresult = string.Compare(arg1.SceneOrPurposes, arg2.VoiceSampleFile1, true);
        //    return cmpresult;
        //}
    }
}
