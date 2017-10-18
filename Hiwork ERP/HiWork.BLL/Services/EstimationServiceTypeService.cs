

using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{
    public partial interface IEstimationServiceTypeService : IBaseService<EstimationServiceTypeModel , Master_EstimationServiceType>
    {
        EstimationServiceTypeModel SaveEstimationServiceType(EstimationServiceTypeModel aMEST);
        List<EstimationServiceTypeModel> GetAllEstimationServiceTypeList(EstimationServiceTypeModel aMEST, string type);
        List<EstimationServiceTypeModel> GetAllEstimationService(BaseViewModel aMEST);
        List<EstimationServiceTypeModel> DeleteEstimationServiceType(EstimationServiceTypeModel aMEST);
    }
    
    public class EstimationServiceTypeService :BaseService <EstimationServiceTypeModel, Master_EstimationServiceType>,IEstimationServiceTypeService
    {
        public readonly EstimationServiceTypeRepository _mestRepository;

        public EstimationServiceTypeService(EstimationServiceTypeRepository mestRepository) : base(mestRepository)
        {
            _mestRepository = mestRepository;
        }

        public EstimationServiceTypeModel SaveEstimationServiceType(EstimationServiceTypeModel aMEST)
        {
            Master_EstimationServiceType mec = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMEST, aMEST.CurrentCulture);
                mec = Mapper.Map<EstimationServiceTypeModel, Master_EstimationServiceType>(aMEST);

                if (aMEST.ID == Guid.Empty)
                {
                    mec.ID = Guid.NewGuid();
                    mec.CreatedBy = aMEST.CurrentUserID;
                    mec.CreatedDate = DateTime.Now;
                    _mestRepository.InsertEstimationServiceType(mec);
                }
                else
                {
                    mec.UpdatedBy = aMEST.CurrentUserID;
                    mec.UpdatedDate = DateTime.Now;
                    _mestRepository.UpdateEstimationServiceType(mec);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMEST.CurrentUserID, "EstimationServiceType", message);
                throw new Exception(ex.Message);
            }
            return aMEST;
        }

        public List<EstimationServiceTypeModel> GetAllEstimationServiceTypeList(EstimationServiceTypeModel model, string type)
        {
            object pvalue;
            List<EstimationServiceTypeModel> merList = new List<EstimationServiceTypeModel>();
            EstimationServiceTypeModel mseModel = new EstimationServiceTypeModel();

            try
            {
                List<Master_EstimationServiceType> repoMasterEstimationRoutes = _mestRepository.GetAllEstimationServiceTypeList();
                if (repoMasterEstimationRoutes != null)
                {
                    foreach (Master_EstimationServiceType mes in repoMasterEstimationRoutes)
                    {
                        if (type != null)
                        {
                            if (mes.Master_EstimationType.Code != type)
                                continue;
                        }

                        mseModel = Mapper.Map<Master_EstimationServiceType, EstimationServiceTypeModel>(mes);
                        mseModel.EstimationType = new EstimationTypeModel();
                        mseModel.EstimationType = Mapper.Map<Master_EstimationType, EstimationTypeModel>(mes.Master_EstimationType);

                        pvalue = Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture);
                        mseModel.Name = pvalue == null ? string.Empty : pvalue.ToString();

                        pvalue = Utility.GetPropertyValue(mseModel.EstimationType, "Name", model.CurrentCulture);
                        mseModel.EstimationType.Name = pvalue == null ? string.Empty : pvalue.ToString();

                        mseModel.CurrentUserID = model.CurrentUserID;
                        mseModel.CurrentCulture = model.CurrentCulture;
                        merList.Add(mseModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EstimationRoutes", message);
                throw new Exception(ex.Message);
            }
            return merList;
        }



        public List<EstimationServiceTypeModel> GetAllEstimationService(BaseViewModel model)
        {
            object pvalue;
            List<EstimationServiceTypeModel> merList = new List<EstimationServiceTypeModel>();
            EstimationServiceTypeModel mseModel = new EstimationServiceTypeModel();

            try
            {
                List<Master_EstimationServiceType> repoMasterEstimationRoutes = _mestRepository.GetAllEstimationServiceTypeList();
                if (repoMasterEstimationRoutes != null)
                {
                    foreach (Master_EstimationServiceType mes in repoMasterEstimationRoutes)
                    {
                       

                        mseModel = Mapper.Map<Master_EstimationServiceType, EstimationServiceTypeModel>(mes);
                        mseModel.EstimationType = new EstimationTypeModel();
                        mseModel.EstimationType = Mapper.Map<Master_EstimationType, EstimationTypeModel>(mes.Master_EstimationType);

                        pvalue = Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture);
                        mseModel.Name = pvalue == null ? string.Empty : pvalue.ToString();

                        pvalue = Utility.GetPropertyValue(mseModel.EstimationType, "Name", model.CurrentCulture);
                        mseModel.EstimationType.Name = pvalue == null ? string.Empty : pvalue.ToString();

                        mseModel.CurrentUserID = model.CurrentUserID;
                        mseModel.CurrentCulture = model.CurrentCulture;
                        merList.Add(mseModel);
                    }
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EstimationRoutes", message);
                throw new Exception(ex.Message);
            }
            return merList;
        }




        public List<EstimationServiceTypeModel> DeleteEstimationServiceType(EstimationServiceTypeModel aMECM)
        {
            List<EstimationServiceTypeModel> mer;
            try
            {
                _mestRepository.DeleteEstimationServiceType(aMECM.ID);
                mer = GetAllEstimationServiceTypeList(aMECM, null);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationServiceType", message);
                throw new Exception(ex.Message);
            }
            return mer;
        }
    }
}
