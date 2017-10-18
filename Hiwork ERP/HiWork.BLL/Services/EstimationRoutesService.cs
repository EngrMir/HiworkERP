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

    public partial interface IEstimationRoutesService : IBaseService<EstimationRoutesModel, Master_EstimationRoutes>
    {
        EstimationRoutesModel SaveEstimationRoutes(EstimationRoutesModel aMECM);
        List<EstimationRoutesModel> GetAllEstimationRoutesList(EstimationRoutesModel aMECM);
        List<EstimationRoutesModel> DeleteEstimationRoutes(EstimationRoutesModel aMECM);
    }



    public class EstimationRoutesService : BaseService<EstimationRoutesModel, Master_EstimationRoutes>, IEstimationRoutesService
    {
        private readonly IEstimationRoutesRepository _mecRepository;
        public EstimationRoutesService(IEstimationRoutesRepository mecRepository) : base(mecRepository)
        {
            _mecRepository = mecRepository;
        }

        public EstimationRoutesModel SaveEstimationRoutes(EstimationRoutesModel aMECM)
        {
            Master_EstimationRoutes mec = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMECM, aMECM.CurrentCulture);
                mec = Mapper.Map<EstimationRoutesModel, Master_EstimationRoutes>(aMECM);

                if (aMECM.IsNew())
                {
                    mec.ID = Guid.NewGuid();
                    mec.CreatedBy = aMECM.CurrentUserID;
                    mec.CreatedDate = DateTime.Now;
                    _mecRepository.InsertEstimationRoutes(mec);
                }
                else
                {
                    mec.UpdatedBy = aMECM.CurrentUserID;
                    mec.UpdatedDate = DateTime.Now;
                    _mecRepository.UpdateEstimationRoutes(mec);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationRoutes", message);
                throw new Exception(ex.Message);
            }
            return aMECM;
        }

        public List<EstimationRoutesModel> GetAllEstimationRoutesList(EstimationRoutesModel model)
        {
            List<EstimationRoutesModel> merList = new List<EstimationRoutesModel>();
            EstimationRoutesModel mseModel = new EstimationRoutesModel();

            try
            {

                List<Master_EstimationRoutes> repoMasterEstimationRoutes = _mecRepository.GetAllEstimationRoutesList();
                if (repoMasterEstimationRoutes != null)
                {
                    foreach (Master_EstimationRoutes mes in repoMasterEstimationRoutes)
                    {

                        mseModel = Mapper.Map<Master_EstimationRoutes, EstimationRoutesModel>(mes);

                        mseModel.Name = Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture).ToString();
                        
                        mseModel.CurrentUserID = model.CurrentUserID;
                        mseModel.CurrentCulture = mseModel.CurrentCulture;
                        mseModel.CurrentUserID = mseModel.CurrentUserID;


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
        public List<EstimationRoutesModel> DeleteEstimationRoutes(EstimationRoutesModel aMECM)
        {
            List<EstimationRoutesModel> mer;
            try
            {
                _mecRepository.DeleteEstimationRoutes(aMECM.ID);
                mer = GetAllEstimationRoutesList(aMECM);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationRoutes", message);
                throw new Exception(ex.Message);
            }
            return mer;
        }



    }
}
