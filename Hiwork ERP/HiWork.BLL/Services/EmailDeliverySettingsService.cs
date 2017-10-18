using AutoMapper;
using HiWork.BLL.Models;
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
    public partial interface IEmailDeliverySettingsService : IBaseService<EmailDeliverySettingsModel, EmailDeliverySetting>
    {
        EmailDeliverySettingsModel SaveUpdateEntity(EmailDeliverySettingsModel model);
        EmailDeliverySettingsModel GetByID(EmailDeliverySettingsModel model);
       
    }
    public class EmailDeliverySettingsService : BaseService<EmailDeliverySettingsModel, EmailDeliverySetting>, IEmailDeliverySettingsService
    {
        public IEmailDeliverySettingsRepository emailDeliverySettingRepo;
        public EmailDeliverySettingsService(IEmailDeliverySettingsRepository _dbRepository) : base(_dbRepository)
        {
            emailDeliverySettingRepo = _dbRepository;
        }

        public EmailDeliverySettingsModel SaveUpdateEntity(EmailDeliverySettingsModel model)
        {
            var Entity = Mapper.Map<EmailDeliverySettingsModel, EmailDeliverySetting>(model);

            if(model.ID==Guid.Empty)
            {
                Entity.ID = Guid.NewGuid();
               emailDeliverySettingRepo.SaveEntity(Entity);
                model.ID = Entity.ID;
            }
            else
            {
                emailDeliverySettingRepo.UpdateEntity(Entity);
            }
            return model;
        }


        public EmailDeliverySettingsModel GetByID(EmailDeliverySettingsModel model)
        {
            var Entity = Mapper.Map<EmailDeliverySettingsModel, EmailDeliverySetting>(model);

             var result = emailDeliverySettingRepo.GetEntityByID(Entity);

             model = Mapper.Map<EmailDeliverySetting, EmailDeliverySettingsModel>(result);

            return model;
        }

    }
}
