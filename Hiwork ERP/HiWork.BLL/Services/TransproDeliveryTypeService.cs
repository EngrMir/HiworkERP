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
    public partial interface ITransproDeliveryPlanService : IBaseService<TransproDeliveryPlanSettingModel, TransproDeliveryPlanSetting>
    {
        List<TransproDeliveryPlanSettingModel> GetAllTransproDeliveryType(BaseViewModel aMEST);


    }

   public class TransproDeliveryPlanService : BaseService<TransproDeliveryPlanSettingModel, TransproDeliveryPlanSetting>, ITransproDeliveryPlanService
    {
        public readonly TransproDeliveryPlanRepository _tsRepository;

        public TransproDeliveryPlanService(TransproDeliveryPlanRepository tsRepository) : base(tsRepository)
        {
            _tsRepository = tsRepository;
        }

        public List<TransproDeliveryPlanSettingModel> GetAllTransproDeliveryType(BaseViewModel model)
        {
            List<TransproDeliveryPlanSettingModel> tdtList = new List<TransproDeliveryPlanSettingModel>();
            TransproDeliveryPlanSettingModel tdtModel = new TransproDeliveryPlanSettingModel();

            try
            {

                List<TransproDeliveryPlanSetting> repoTDT = _tsRepository.GetAllTransproDeliveryType();
                if (repoTDT != null)
                {
                    foreach (TransproDeliveryPlanSetting tdt in repoTDT)
                    {

                        tdtModel = Mapper.Map<TransproDeliveryPlanSetting, TransproDeliveryPlanSettingModel>(tdt);

                        tdtModel.Name = Utility.GetPropertyValue(tdtModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                    Utility.GetPropertyValue(tdtModel, "Name", model.CurrentCulture).ToString();

                        tdtModel.CurrentUserID = model.CurrentUserID;
                        tdtModel.CurrentCulture = tdtModel.CurrentCulture;
                        tdtModel.CurrentUserID = tdtModel.CurrentUserID;


                        tdtList.Add(tdtModel);
                    }

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "TransproDeliveryType", message);
                throw new Exception(ex.Message);
            }

            return tdtList;
        }

    }
}
