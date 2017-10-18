using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
    public partial interface IEstimationSpecializedFieldService : IBaseService<EstimationSpecializedFieldModel, Master_EstimationSpecializedField>
    {
        EstimationSpecializedFieldModel SaveEstimationSpecializedField(EstimationSpecializedFieldModel aMEST);
        List<EstimationSpecializedFieldModel> GetAllEstimationSpecializedFieldList(BaseViewModel aMEST);
        List<EstimationSpecializedFieldModel> DeleteEstimationSpecializedField(EstimationSpecializedFieldModel aMEST);

    }

    public class EstimationSpecializedFieldService : BaseService<EstimationSpecializedFieldModel, Master_EstimationSpecializedField>, IEstimationSpecializedFieldService
    {
        public readonly EstimationSpecializedFieldRepository _mestRepository;

        public EstimationSpecializedFieldService(EstimationSpecializedFieldRepository mestRepository) : base(mestRepository)
        {
            _mestRepository = mestRepository;
        }



        public EstimationSpecializedFieldModel SaveEstimationSpecializedField(EstimationSpecializedFieldModel aMEST)
        {
            Master_EstimationSpecializedField mesf = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMEST, aMEST.CurrentCulture);
                mesf = Mapper.Map<EstimationSpecializedFieldModel, Master_EstimationSpecializedField>(aMEST);

                if (aMEST.ID == Guid.Empty)
                {
                    mesf.ID = Guid.NewGuid();
                    mesf.CreatedBy = aMEST.CurrentUserID;
                    mesf.CreatedDate = DateTime.Now;
                    _mestRepository.InsertEstimationSpecializedField(mesf);
                }
                else
                {
                    mesf.UpdatedBy = aMEST.CurrentUserID;
                    mesf.UpdatedDate = DateTime.Now;
                    _mestRepository.UpdateEstimationSpecializedField(mesf);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMEST.CurrentUserID, "EstimationSpecializedField", message);
                throw new Exception(ex.Message);
            }
            return aMEST;
        }

        public List<EstimationSpecializedFieldModel> GetAllEstimationSpecializedFieldList(BaseViewModel model)
        {
            List<EstimationSpecializedFieldModel> merList = new List<EstimationSpecializedFieldModel>();
            EstimationSpecializedFieldModel mseModel = new EstimationSpecializedFieldModel();

            try
            {

                List<Master_EstimationSpecializedField> repoMasterEstimationSF = _mestRepository.GetAllEstimationSpecializedFieldList();
                if (repoMasterEstimationSF != null)
                {
                    foreach (Master_EstimationSpecializedField mes in repoMasterEstimationSF)
                    {

                        mseModel = Mapper.Map<Master_EstimationSpecializedField, EstimationSpecializedFieldModel>(mes);

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
                errorLog.SetErrorLog(model.CurrentUserID, "EstimationSpecializedField", message);
                throw new Exception(ex.Message);
            }

            merList.Sort(CompareSpecializedFieldByName);
            return merList;
        }
        public List<EstimationSpecializedFieldModel> DeleteEstimationSpecializedField(EstimationSpecializedFieldModel aMECM)
        {
            List<EstimationSpecializedFieldModel> mer;
            try
            {
                _mestRepository.DeleteEstimationSpecializedField(aMECM.ID);
                BaseViewModel model = new BaseViewModel();
                model.CurrentCulture = aMECM.CurrentCulture;
                model.ApplicationId = aMECM.ApplicationId;
                model.CurrentUserID = model.CurrentUserID;
                mer = GetAllEstimationSpecializedFieldList(model);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationSpecializedField", message);
                throw new Exception(ex.Message);
            }
            return mer;
        }

        public int CompareSpecializedFieldByName(EstimationSpecializedFieldModel model1, EstimationSpecializedFieldModel model2)
        {
            int cmpresult;
            cmpresult = string.Compare(model1.Name, model2.Name, true);
            return cmpresult;
        }

    }
}
