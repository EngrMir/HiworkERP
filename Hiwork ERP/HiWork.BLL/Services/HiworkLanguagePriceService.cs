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
        public partial interface IHiworkLanguagePriceService : IBaseService<HiworkLanguagePriceModel, HiworkLanguagePrice>
    {
        List<HiworkLanguagePriceModel> SaveHiworkLanguagePrice(HiworkLanguagePriceModel model);
        List<HiworkLanguagePriceModel> GetAllHiworkLanguagePriceList(BaseViewModel model);
        List<HiworkLanguagePriceModel> DeleteHiworkLanguagePrice(HiworkLanguagePriceModel model);
    }
    public class HiworkLanguagePriceService : BaseService<HiworkLanguagePriceModel, HiworkLanguagePrice>, IHiworkLanguagePriceService
    {
        private readonly IHiworkLanguagePriceRepository _HiworkLanguagePriceRepository;
        public HiworkLanguagePriceService(IHiworkLanguagePriceRepository HiworkLanguagePriceRepository) : base(HiworkLanguagePriceRepository)
        {
            _HiworkLanguagePriceRepository = HiworkLanguagePriceRepository;
        }
        public List<HiworkLanguagePriceModel> SaveHiworkLanguagePrice(HiworkLanguagePriceModel aHiworkLanguagePriceModel)
        {
            List<HiworkLanguagePriceModel> HiworkLanguagePrices = null;
            try
            {
                Utility.SetDynamicPropertyValue(aHiworkLanguagePriceModel, aHiworkLanguagePriceModel.CurrentCulture);
                var HiworkLanguagePrice = Mapper.Map<HiworkLanguagePriceModel, HiworkLanguagePrice>(aHiworkLanguagePriceModel);

                if (aHiworkLanguagePriceModel.ID == Guid.Empty)
                {
                    HiworkLanguagePrice.ID = Guid.NewGuid();
                    HiworkLanguagePrice.SourceLanguageID = aHiworkLanguagePriceModel.SourceLanguageID;
                    HiworkLanguagePrice.TargetLanguageID = aHiworkLanguagePriceModel.TargetLanguageID;
                    HiworkLanguagePrice.CurrencyID = aHiworkLanguagePriceModel.CurrencyID;
                    HiworkLanguagePrice.CreatedBy = aHiworkLanguagePriceModel.CurrentUserID;
                    HiworkLanguagePrice.CreatedDate = DateTime.Now;
                    _HiworkLanguagePriceRepository.InsertHiworkLanguagePrice(HiworkLanguagePrice);
                }
                else
                {
                    HiworkLanguagePrice.SourceLanguageID = aHiworkLanguagePriceModel.SourceLanguageID;
                    HiworkLanguagePrice.TargetLanguageID = aHiworkLanguagePriceModel.TargetLanguageID;
                    HiworkLanguagePrice.CurrencyID = aHiworkLanguagePriceModel.CurrencyID;
                    HiworkLanguagePrice.UpdatedBy = aHiworkLanguagePriceModel.CurrentUserID;
                    HiworkLanguagePrice.UpdatedDate = DateTime.Now;
                    _HiworkLanguagePriceRepository.UpdateHiworkLanguagePrice(HiworkLanguagePrice);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aHiworkLanguagePriceModel.CurrentCulture;
                baseViewModel.CurrentUserID = aHiworkLanguagePriceModel.CurrentUserID;
                HiworkLanguagePrices = GetAllHiworkLanguagePriceList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aHiworkLanguagePriceModel.CurrentUserID, "HiworkLanguagePrice", message);
                throw new Exception(ex.Message);
            }
            return HiworkLanguagePrices;
        }
        public List<HiworkLanguagePriceModel> GetAllHiworkLanguagePriceList(BaseViewModel model)
        {
            List<HiworkLanguagePriceModel> HiworkLanguagePriceModelList = new List<HiworkLanguagePriceModel>();
            HiworkLanguagePriceModel HiworkLanguagePriceModel = new HiworkLanguagePriceModel();
            try
            {
                List<HiworkLanguagePrice> HiworkLanguagePriceList = _HiworkLanguagePriceRepository.GetAllHiworkLanguagePriceList();
                if (HiworkLanguagePriceList != null)
                {
                    HiworkLanguagePriceList.ForEach(a =>
                    {
                        HiworkLanguagePriceModel = Mapper.Map<HiworkLanguagePrice, HiworkLanguagePriceModel>(a);
                        HiworkLanguagePriceModel.SourceLanguage = Mapper.Map<Master_Language, LanguageModel>(a.Master_Language);
                        HiworkLanguagePriceModel.TargetLanguage = Mapper.Map<Master_Language, LanguageModel>(a.Master_Language1);
                        HiworkLanguagePriceModel.Currency = Mapper.Map<Master_Currency, CurrencyModel>(a.Master_Currency);

                        if (HiworkLanguagePriceModel.SourceLanguage != null)
                            HiworkLanguagePriceModel.SourceLanguage.Name = Utility.GetPropertyValue(HiworkLanguagePriceModel.SourceLanguage, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(HiworkLanguagePriceModel.SourceLanguage, "Name", model.CurrentCulture).ToString();
                        if (HiworkLanguagePriceModel.TargetLanguage != null)
                            HiworkLanguagePriceModel.TargetLanguage.Name = Utility.GetPropertyValue(HiworkLanguagePriceModel.TargetLanguage, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(HiworkLanguagePriceModel.TargetLanguage, "Name", model.CurrentCulture).ToString(); 
                        if (HiworkLanguagePriceModel.Currency != null)
                            HiworkLanguagePriceModel.Currency.Name = Utility.GetPropertyValue(HiworkLanguagePriceModel.Currency, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(HiworkLanguagePriceModel.Currency, "Name", model.CurrentCulture).ToString();

                      
                        HiworkLanguagePriceModel.CurrentUserID = model.CurrentUserID;

                        HiworkLanguagePriceModel.CurrentCulture = model.CurrentCulture;
                        HiworkLanguagePriceModelList.Add(HiworkLanguagePriceModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "HiworkLanguagePrice", message);
                throw new Exception(ex.Message);
            }

            HiworkLanguagePriceModelList.Sort(CompareHiworkLanguagePriceByName);
            return HiworkLanguagePriceModelList;
        }
        public List<HiworkLanguagePriceModel> DeleteHiworkLanguagePrice(HiworkLanguagePriceModel model)
        {
            List<HiworkLanguagePriceModel> HiworkLanguagePrices = null;
            try
            {
                _HiworkLanguagePriceRepository.DeleteHiworkLanguagePrice(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                HiworkLanguagePrices = GetAllHiworkLanguagePriceList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "HiworkLanguagePrice", message);
                throw new Exception(ex.Message);
            }
            return HiworkLanguagePrices;
        }
        private int CompareHiworkLanguagePriceByName(HiworkLanguagePriceModel arg1, HiworkLanguagePriceModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.TargetLanguageID.ToString(), arg2.GeneralPrice.ToString(), true);
            return cmpresult;
        }
    }
}
