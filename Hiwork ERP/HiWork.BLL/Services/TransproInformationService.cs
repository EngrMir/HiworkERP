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
     
    public partial interface ITransproInformationService : IBaseService<TransproInformationModel, Staff_TransproInformation>
      {
        List<TransproInformationModel> SaveTransproInformation(TransproInformationModel model);
        List<TransproInformationModel> GetAllTransproInformationList(BaseViewModel model);
        List<TransproInformationModel> DeleteTransproInformation(TransproInformationModel model);
     }

    public class TransproInformationService : BaseService<TransproInformationModel, Staff_TransproInformation>, ITransproInformationService
      {
        private readonly ITransproInformationRepository _transproInformationRepository;
        public TransproInformationService(ITransproInformationRepository TransproInformationRepository)
            : base(TransproInformationRepository)
        {
            _transproInformationRepository = TransproInformationRepository;
        }
        public List<TransproInformationModel> SaveTransproInformation(TransproInformationModel model)
          {
            try
              {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var TransproInformation = Mapper.Map<TransproInformationModel, Staff_TransproInformation>(model);
                if (TransproInformation.ID != Guid.Empty)
                {
                    _transproInformationRepository.UpdateTransproInformation(TransproInformation);
                }
                else
                {
                    TransproInformation.ID = Guid.NewGuid();
                    _transproInformationRepository.InsertTransproInformation(TransproInformation);
                }
              }
            catch (Exception ex)
              {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TransproInformation", message);
                throw new Exception(ex.Message);
              }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllTransproInformationList(md);          
        }
        public List<TransproInformationModel> GetAllTransproInformationList(BaseViewModel model)
        {
            List<TransproInformationModel> TransproInformationList = new List<TransproInformationModel>();
            TransproInformationModel TransproInformationModel = new TransproInformationModel();
            try
            {
                List<Staff_TransproInformation> TransproInformationvList = _transproInformationRepository.GetAllTransproInformationList();
                if (TransproInformationvList != null)
                {
                    TransproInformationvList.ForEach(a =>
                    {
                        TransproInformationModel = Mapper.Map<Staff_TransproInformation, TransproInformationModel>(a);
                        TransproInformationModel.CurrentUserID = model.CurrentUserID;
                        TransproInformationModel.CurrentCulture = model.CurrentCulture;
                        TransproInformationList.Add(TransproInformationModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TransproInformation", message);
                throw new Exception(ex.Message);
            }
            return TransproInformationList;
        }
        public List<TransproInformationModel> DeleteTransproInformation(TransproInformationModel model)
        {
            try
            {
                _transproInformationRepository.DeleteTransproInformation(model.ID);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TransproInformation", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = model.CurrentUserID;
            md.CurrentCulture = model.CurrentCulture;
            return GetAllTransproInformationList(md);
        }
    }
}
