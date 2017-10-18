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
    public partial interface INarrationInformationService : IBaseService<NarrationInformationModel, Staff_NarrationInformation>
    {
        List<NarrationInformationModel> SaveNarrationInformation(NarrationInformationModel model);
        List<NarrationInformationModel> GetAllNarrationInformationList(BaseViewModel model);
        List<NarrationInformationModel> DeleteNarrationInformation(NarrationInformationModel model);
    }
    public class NarrationInformationService : BaseService<NarrationInformationModel, Staff_NarrationInformation>, INarrationInformationService
    {
        private readonly INarrationInformationRepository _NarrationInformationRepository;
        public NarrationInformationService(INarrationInformationRepository NarrationInformationRepository) : base(NarrationInformationRepository)
        {
            _NarrationInformationRepository = NarrationInformationRepository;
        }
        public List<NarrationInformationModel> SaveNarrationInformation(NarrationInformationModel model)
        {
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var NarrationInformation = Mapper.Map<NarrationInformationModel, Staff_NarrationInformation>(model);

                if (model.ID == Guid.Empty)
                {
                    NarrationInformation.ID = Guid.NewGuid();                
                    _NarrationInformationRepository.InsertNarrationInformation(NarrationInformation);
                }                
                else
                {
                    _NarrationInformationRepository.UpdateNarrationInformation(NarrationInformation);
                }                
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "NarrationInformation", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            return GetAllNarrationInformationList(baseViewModel);          
        }
        public List<NarrationInformationModel> GetAllNarrationInformationList(BaseViewModel model)
        {
            List<NarrationInformationModel> NarrationInformationModelList = new List<NarrationInformationModel>();
            NarrationInformationModel NarrationInformationModel = new NarrationInformationModel();
            try
            {
                List<Staff_NarrationInformation> NarrationInformationList = _NarrationInformationRepository.GetAllNarrationInformationList();
                if (NarrationInformationList != null)
                {
                    NarrationInformationList.ForEach(a =>
                    {
                        NarrationInformationModel = Mapper.Map<Staff_NarrationInformation, NarrationInformationModel>(a);
                        NarrationInformationModel.CurrentUserID = model.CurrentUserID;
                        NarrationInformationModel.CurrentCulture = model.CurrentCulture;
                        NarrationInformationModelList.Add(NarrationInformationModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "NarrationInformation", message);
                throw new Exception(ex.Message);
            }

            NarrationInformationModelList.Sort(CompareNarrationInformationByName);
            return NarrationInformationModelList;
        }
        public List<NarrationInformationModel> DeleteNarrationInformation(NarrationInformationModel model)
        {
            List<NarrationInformationModel> NarrationInformations = null;
            try
            {
                _NarrationInformationRepository.DeleteNarrationInformation(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                NarrationInformations = GetAllNarrationInformationList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "NarrationInformation", message);
                throw new Exception(ex.Message);
            }
            return NarrationInformations;
        }

        private int CompareNarrationInformationByName(NarrationInformationModel arg1, NarrationInformationModel arg2)
        {
            int cmpresult; 
             cmpresult = string.Compare(arg1.SceneOrPurposes, arg2.VoiceSampleFile1, true);
            return cmpresult;
        }
    }
}
