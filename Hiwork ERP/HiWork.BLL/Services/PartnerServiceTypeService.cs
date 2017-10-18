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
    public partial interface IPartnerServiceTypeService: IBaseService<PartnerServiceTypeModel, Master_PartnerServiceType>
    {
        List<PartnerServiceTypeModel> GetAll(BaseViewModel model);
    }
    public class PartnerServiceTypeService : BaseService<PartnerServiceTypeModel, Master_PartnerServiceType>, IPartnerServiceTypeService
    {
        private readonly IPartnerServiceTypeRepository servicetypeRepo;
        public PartnerServiceTypeService(IPartnerServiceTypeRepository _servicetypeRepo) : base(_servicetypeRepo)
        {
            servicetypeRepo = _servicetypeRepo;
        }
        public List<PartnerServiceTypeModel> GetAll(BaseViewModel model)
        {
            List<PartnerServiceTypeModel> serviceTypeList = new List<PartnerServiceTypeModel>();
            try
            {
                var dbServiceList = servicetypeRepo.GetAllList();
                serviceTypeList = Mapper.Map<List<Master_PartnerServiceType>, List<PartnerServiceTypeModel>>(dbServiceList);
                serviceTypeList.ForEach(a =>
                {
                    a.Name= Utility.GetPropertyValue(a, "Name", model.CurrentCulture) == null ? string.Empty :
                                           Utility.GetPropertyValue(a, "Name", model.CurrentCulture).ToString();
                });

            }
            catch(Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "PartnerServiceType", message);
                throw new Exception(ex.Message);
            }

            return serviceTypeList;
        }
    }
}
