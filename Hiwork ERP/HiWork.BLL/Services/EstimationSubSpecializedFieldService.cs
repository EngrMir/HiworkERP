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
    public partial interface IEstimationSubSpecializedFieldService : IBaseService<EstimationSubSpecializedFieldModel, Master_EstimationSubSpecializedField>
    {
        EstimationSubSpecializedFieldModel SaveEstimationSubSpecializedField(EstimationSubSpecializedFieldModel essfM);
        List<EstimationSubSpecializedFieldModel> GetAllEstimationSubSpecializedFieldList(BaseViewModel essfM);
        bool DeleteEstimationSubSpecializedField(EstimationSubSpecializedFieldModel essfM);

    }

    public class EstimationSubSpecializedFieldService : BaseService<EstimationSubSpecializedFieldModel, Master_EstimationSubSpecializedField>, IEstimationSubSpecializedFieldService
    {
        public readonly EstimationSubSpecializedFieldRepository _messfRepository;

        public EstimationSubSpecializedFieldService(EstimationSubSpecializedFieldRepository messfRepository) : base(messfRepository)
        {
            _messfRepository = messfRepository;
        }



        public EstimationSubSpecializedFieldModel SaveEstimationSubSpecializedField(EstimationSubSpecializedFieldModel aMEST)
        {
            Master_EstimationSubSpecializedField mesf = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMEST, aMEST.CurrentCulture);
                mesf = Mapper.Map<EstimationSubSpecializedFieldModel, Master_EstimationSubSpecializedField>(aMEST);

                if (aMEST.IsNew())
                {
                    mesf.ID = Guid.NewGuid();
                    mesf.SpecializedField = aMEST.SpecializedField;
                    mesf.CreatedBy = aMEST.CurrentUserID;
                    mesf.CreatedDate = DateTime.Now;
                    _messfRepository.InsertEstimationSubSpecializedField(mesf);
                }
                else
                {
                    mesf.SpecializedField = aMEST.SpecializedField;
                    mesf.UpdatedBy = aMEST.CurrentUserID;
                    mesf.UpdatedDate = DateTime.Now;
                    _messfRepository.UpdateEstimationSubSpecializedField(mesf);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMEST.CurrentUserID, "EstimationSubSpecializedField", message);
                throw new Exception(ex.Message);
            }
            return aMEST;
        }

        public List<EstimationSubSpecializedFieldModel> GetAllEstimationSubSpecializedFieldList(BaseViewModel model)
        {
            List<EstimationSubSpecializedFieldModel> merList = new List<EstimationSubSpecializedFieldModel>();
            EstimationSubSpecializedFieldModel mseModel = new EstimationSubSpecializedFieldModel();

            try
            {

                List<Master_EstimationSubSpecializedField> repoMasterEstimationSF = _messfRepository.GetAllEstimationSubSpecializedFieldList();
                if (repoMasterEstimationSF != null)
                {
                    repoMasterEstimationSF.ForEach(mes =>
                    {
                        mseModel = Mapper.Map<Master_EstimationSubSpecializedField, EstimationSubSpecializedFieldModel>(mes);
                        mseModel.EstimationSpecializedField = Mapper.Map<Master_EstimationSpecializedField, EstimationSpecializedFieldModel>(mes.Master_EstimationSpecializedField);

                 if (mseModel.EstimationSpecializedField != null)

                        mseModel.EstimationSpecializedField.Name = Utility.GetPropertyValue(mseModel.EstimationSpecializedField, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                  Utility.GetPropertyValue(mseModel.EstimationSpecializedField, "Name", model.CurrentCulture).ToString();                        mseModel.Name = Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(mseModel, "Name", model.CurrentCulture).ToString();
                        mseModel.CurrentUserID = model.CurrentUserID;
                        mseModel.CurrentCulture = model.CurrentCulture;
                        merList.Add(mseModel);
                    });

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EstimationSubSpecializedField", message);
                throw new Exception(ex.Message);
            }

            merList.Sort(CompareSubSpecializedFieldByName);
            return merList;
        }
        public bool DeleteEstimationSubSpecializedField(EstimationSubSpecializedFieldModel aMECM)
        {
            try
            {
                _messfRepository.DeleteEstimationSubSpecializedField(aMECM.ID);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMECM.CurrentUserID, "EstimationSubSpecializedField", message);
                throw new Exception(ex.Message);
            }
            return true;
        }

        public int CompareSubSpecializedFieldByName(EstimationSubSpecializedFieldModel model1, EstimationSubSpecializedFieldModel model2)
        {
            int cmpresult;
            cmpresult = string.Compare(model1.Name, model2.Name, true);
            return cmpresult;
        }

    }
}
