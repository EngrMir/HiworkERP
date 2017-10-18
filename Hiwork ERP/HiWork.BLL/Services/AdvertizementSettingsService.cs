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

public partial interface IAdvertizementSettingsService: IBaseService<AdvertizementSettingsModel,AdvertizementSetting>
    {
        AdvertizementSettingsModel SaveAdvertizementSettings(AdvertizementSettingsModel advertizement);
        List<AdvertizementSettingsModel> GetAdvertizementlist(BaseViewModel model);
        List<AdvertizementSettingsModel> DeleteAdvertizement(AdvertizementSettingsModel advertizement);

    }


  public class AdvertizementSettingsService: BaseService<AdvertizementSettingsModel, AdvertizementSetting>, IAdvertizementSettingsService
    {
        private readonly IAdvertizementSettingsRepository _advertizementRepository;
        public AdvertizementSettingsService(IAdvertizementSettingsRepository advertizementRepository) :base(advertizementRepository)

        {
            _advertizementRepository = advertizementRepository;
        }
       public  AdvertizementSettingsModel SaveAdvertizementSettings(AdvertizementSettingsModel advertizement)
        {
            Utility.SetDynamicPropertyValue(advertizement, advertizement.CurrentCulture);
            var Settings = Mapper.Map<AdvertizementSettingsModel, AdvertizementSetting>(advertizement);

            if (advertizement.ID == Guid.Empty)
            {
                Settings.CreatedDate = DateTime.Now;
                Settings.ID = Guid.NewGuid();
                _advertizementRepository.Insertadvertizement(Settings);
            }
            else
            {

                Settings.UpdatedBy = advertizement.CurrentUserID;
                Settings.UpdatedDate = DateTime.Now;
                _advertizementRepository.UpdateAdvertizement(Settings);
            }
            return advertizement;

        }
        public List<AdvertizementSettingsModel> GetAdvertizementlist(BaseViewModel model)
        {
            List<AdvertizementSettingsModel> Advertizementlist = new List<AdvertizementSettingsModel>();
            AdvertizementSettingsModel Advertizementmodel = new AdvertizementSettingsModel();
            try
            {
                List<AdvertizementSetting> repoAdvertizement = _advertizementRepository.GetAdvertizementlist();
                if(repoAdvertizement!=null)
                {
                    foreach(AdvertizementSetting advertize in repoAdvertizement)
                    {
                        Advertizementmodel = Mapper.Map<AdvertizementSetting, AdvertizementSettingsModel>(advertize);
                        Advertizementmodel.Title = Utility.GetPropertyValue(Advertizementmodel, "Title", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(Advertizementmodel, "Title", model.CurrentCulture).ToString();
                        Advertizementmodel.Description = Utility.GetPropertyValue(Advertizementmodel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(Advertizementmodel, "Description", model.CurrentCulture).ToString();

                        Advertizementmodel.CurrentUserID = model.CurrentUserID;
                        Advertizementmodel.CurrentCulture = model.CurrentCulture;
                        Advertizementlist.Add(Advertizementmodel);
                    }

                    
                }
            }
            catch(Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Advertizement", message);
                throw new Exception(ex.Message);

            }
            return Advertizementlist;

        }
        public List<AdvertizementSettingsModel> DeleteAdvertizement(AdvertizementSettingsModel advertizement)
        {
            try
            {
                advertizement.IsDeleted = true;
                this.SaveAdvertizementSettings(advertizement);
            }
            catch(Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(advertizement.CurrentUserID, "", message);
                throw new Exception(ex.Message);
            }
            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = advertizement.CurrentCulture;
            model.CurrentUserID = advertizement.CurrentUserID;
            return this.GetAdvertizementlist(model);

        }



    }
}
