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

    public partial interface IEstimationTypeService : IBaseService<EstimationTypeModel, Master_EstimationType>
    {
        EstimationTypeModel SaveEstimationType(EstimationTypeModel aMECM);
        List<EstimationTypeModel> GetAllEstimationTypeList(BaseViewModel aMECM);
        List<EstimationTypeModel> DeleteEstimationType(EstimationTypeModel aMECM);
    }



    public class EstimationTypeService : BaseService<EstimationTypeModel, Master_EstimationType>, IEstimationTypeService
    {
        private readonly IEstimationTypeRepository _mecRepository;
        public EstimationTypeService(IEstimationTypeRepository mecRepository) : base(mecRepository)
        {
            _mecRepository = mecRepository;
        }

        public EstimationTypeModel SaveEstimationType(EstimationTypeModel aMECM)
        {
            Master_EstimationType mec = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMECM, aMECM.CurrentCulture);
                mec = Mapper.Map<EstimationTypeModel, Master_EstimationType>(aMECM);

                if (aMECM.IsNew()) { 
                    mec.CreatedBy = aMECM.CurrentUserID;
                    mec.CreatedDate = DateTime.Now;
                    _mecRepository.InsertEstimationType(mec);
                }
                else
                {
                    mec.UpdatedBy = aMECM.CurrentUserID;
                    mec.UpdatedDate = DateTime.Now;
                    _mecRepository.UpdateEstimationType(mec);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationType", message);
                throw new Exception(ex.Message);
            }
            return aMECM;
        }


        public List<EstimationTypeModel> GetAllEstimationTypeList(BaseViewModel model)
        {
            List<EstimationTypeModel> mecList = new List<EstimationTypeModel>();
            EstimationTypeModel mseModel = new EstimationTypeModel();

            try
            {
                List<Master_EstimationType> repoMasterEstimation = _mecRepository.GetAllEstimationTypeList();
                if (repoMasterEstimation != null)
                {
                    foreach (Master_EstimationType mse in repoMasterEstimation)
                    {
                        mseModel = Mapper.Map<Master_EstimationType, EstimationTypeModel>(mse);
                        mseModel.Name = Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture).ToString();
                        
                        mseModel.CurrentUserID = model.CurrentUserID;
                        mseModel.CurrentCulture = mseModel.CurrentCulture;
                        mseModel.CurrentUserID = mseModel.CurrentUserID;


                        mecList.Add(mseModel);
                    }

                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EstimationType", message);
                throw new Exception(ex.Message);
            }

            return mecList;
        }
        public List<EstimationTypeModel> DeleteEstimationType(EstimationTypeModel aMECM)
        {
            List<EstimationTypeModel> mec;
            try
            {
                _mecRepository.DeleteEstimationType(aMECM.ID);
                BaseViewModel model = new BaseViewModel();
                model.CurrentCulture = aMECM.CurrentCulture;
                model.ApplicationId = aMECM.ApplicationId;
                mec = GetAllEstimationTypeList(model);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationType", message);
                throw new Exception(ex.Message);
            }
            return mec;
        }



    }
}
