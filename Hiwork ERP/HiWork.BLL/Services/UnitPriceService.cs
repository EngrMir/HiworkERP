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
    public partial interface IUnitPriceService : IBaseService<UnitPriceModel, Master_UnitPriceSetting>
    {
        List<UnitPriceModel> SaveUnitPrice(UnitPriceModel model);
        List<UnitPriceModel> GetAllUnitPrice(BaseViewModel model);
        List<UnitPriceModel> DeleteUnitPrice(UnitPriceModel model);
        //List<UnitPriceModel> GetGeneralUnitPriceByID(UnitPriceModel model);
    }
    public class UnitPriceService : BaseService<UnitPriceModel, Master_UnitPriceSetting>, IUnitPriceService
    {
        private readonly IUnitPriceRepository _unitPriceRepository;
        public UnitPriceService(IUnitPriceRepository UnitPriceRepository) : base(UnitPriceRepository)
        {
            _unitPriceRepository = UnitPriceRepository;
        }
        public List<UnitPriceModel> SaveUnitPrice(UnitPriceModel aUnitPriceModel)
        {
            List<UnitPriceModel> aUnitPrice = null;
            bool Exist;
            
            try
            {
                Utility.SetDynamicPropertyValue(aUnitPriceModel, aUnitPriceModel.CurrentCulture);
                var UnitPrice = Mapper.Map<UnitPriceModel, Master_UnitPriceSetting>(aUnitPriceModel);

                Exist = _unitPriceRepository.MatchedByID(aUnitPriceModel.SourceLanguageID, aUnitPriceModel.TargetLanguageID,aUnitPriceModel.UnitID,aUnitPriceModel.EstimationTypeID);
                if (Exist == true)
                {
                    return null;
                }

                if (aUnitPriceModel.ID> 0)
                {
                   
                    UnitPrice.SourceLanguageID = aUnitPriceModel.SourceLanguageID;
                    UnitPrice.TargetLanguageID = aUnitPriceModel.TargetLanguageID;
                    UnitPrice.CurrencyID = aUnitPriceModel.CurrencyID;
                    UnitPrice.UnitID = aUnitPriceModel.UnitID;
                    UnitPrice.EstimationTypeID = aUnitPriceModel.EstimationTypeID;
                    UnitPrice.CreatedBy = aUnitPriceModel.CurrentUserID;
                    UnitPrice.CreatedDate = DateTime.Now;
                    _unitPriceRepository.UpdateUnitPrice(UnitPrice);
                   
                }
                else
                {
                    UnitPrice.SourceLanguageID = aUnitPriceModel.SourceLanguageID;
                    UnitPrice.TargetLanguageID = aUnitPriceModel.TargetLanguageID;
                    UnitPrice.CurrencyID = aUnitPriceModel.CurrencyID;
                    UnitPrice.UnitID = aUnitPriceModel.UnitID;
                    UnitPrice.EstimationTypeID = aUnitPriceModel.EstimationTypeID;
                    UnitPrice.UpdatedBy = aUnitPriceModel.CurrentUserID;
                    UnitPrice.UpdatedDate = DateTime.Now;
                    _unitPriceRepository.InsertUnitPrice(UnitPrice);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aUnitPriceModel.CurrentCulture;
                baseViewModel.CurrentUserID = aUnitPriceModel.CurrentUserID;
                aUnitPrice = GetAllUnitPrice(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aUnitPriceModel.CurrentUserID, "UnitPrice", message);
                throw new Exception(ex.Message);
            }
            return aUnitPrice;
        }
        public List<UnitPriceModel> GetAllUnitPrice(BaseViewModel model)
        {
            List<UnitPriceModel> UnitPriceListModel = new List<UnitPriceModel>();
            UnitPriceModel aUnitPriceModel = new UnitPriceModel();
            try
            {
                List<Master_UnitPriceSetting> UnitPriceList = _unitPriceRepository.GetAllUnitPrice();
                if (UnitPriceList != null)
                {
                    UnitPriceList.ForEach(a =>
                    {
                        aUnitPriceModel = Mapper.Map<Master_UnitPriceSetting, UnitPriceModel>(a);
                        aUnitPriceModel.SourceLanguage = Mapper.Map<Master_Language, LanguageModel>(a.Master_Language);
                        aUnitPriceModel.TargetLanguage = Mapper.Map<Master_Language, LanguageModel>(a.Master_Language1);
                        aUnitPriceModel.Currency = Mapper.Map<Master_Currency, CurrencyModel>(a.Master_Currency);
                        aUnitPriceModel.EstimationType = Mapper.Map<Master_EstimationType, EstimationTypeModel>(a.Master_EstimationType);
                        aUnitPriceModel.Unit = Mapper.Map<Master_Unit, UnitModel>(a.Master_Unit);
                        if (aUnitPriceModel.SourceLanguage != null)
                            aUnitPriceModel.SourceLanguage.Name = Utility.GetPropertyValue(aUnitPriceModel.SourceLanguage, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                  Utility.GetPropertyValue(aUnitPriceModel.SourceLanguage, "Name", model.CurrentCulture).ToString();
                        if (aUnitPriceModel.TargetLanguage != null)
                            aUnitPriceModel.TargetLanguage.Name = Utility.GetPropertyValue(aUnitPriceModel.TargetLanguage, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                  Utility.GetPropertyValue(aUnitPriceModel.TargetLanguage, "Name", model.CurrentCulture).ToString();
                        if (aUnitPriceModel.Currency != null)
                            aUnitPriceModel.Currency.Name = Utility.GetPropertyValue(aUnitPriceModel.Currency, "Name", model.CurrentCulture) == null ? string.Empty :
                                                            Utility.GetPropertyValue(aUnitPriceModel.Currency, "Name", model.CurrentCulture).ToString();
                        if (aUnitPriceModel.EstimationType != null)
                            aUnitPriceModel.EstimationType.Name = Utility.GetPropertyValue(aUnitPriceModel.EstimationType, "Name", model.CurrentCulture) == null ? string.Empty :
                                                            Utility.GetPropertyValue(aUnitPriceModel.EstimationType, "Name", model.CurrentCulture).ToString();
                        if (aUnitPriceModel.Unit !=null)
                            aUnitPriceModel.Unit.Name= Utility.GetPropertyValue(aUnitPriceModel.Unit, "Name", model.CurrentCulture) == null ? string.Empty :
                                                       Utility.GetPropertyValue(aUnitPriceModel.Unit, "Name", model.CurrentCulture).ToString();

                      
                        aUnitPriceModel.CurrentUserID = model.CurrentUserID;
                        aUnitPriceModel.CurrentCulture = model.CurrentCulture;
                        UnitPriceListModel.Add(aUnitPriceModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Unit Price", message);
                throw new Exception(ex.Message);
            }
            
            return UnitPriceListModel;
        }
        
        public List<UnitPriceModel> DeleteUnitPrice(UnitPriceModel model)
        {
            List<UnitPriceModel> UnitPrice = null;
            try
            {
                _unitPriceRepository.DeleteUnitPrice(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                UnitPrice = GetAllUnitPrice(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "UnitPrice", message);
                throw new Exception(ex.Message);
            }
            return UnitPrice;
        }
        //public List<UnitPriceModel> GetGeneralUnitPriceByID(UnitPriceModel model)
        //{
        //    List<Master_UnitPriceSetting> GeneralUnitPrice = _unitPriceRepository.GetGeneralUnitPriceByID(model.SourceLanguageID, model.TargetLanguageID); 
        //    UnitPriceModel aUnitPriceModelByID = new UnitPriceModel();
        //    List<UnitPriceModel> List = new List<UnitPriceModel>();
        //    try
        //    {
        //        if (GeneralUnitPrice != null)
        //            GeneralUnitPrice.ForEach(a =>

        //                             {
        //                                 aUnitPriceModelByID = Mapper.Map<Master_UnitPriceSetting, UnitPriceModel>(a);
        //                                 List.Add(aUnitPriceModelByID);
        //                             });


        //    }
        //    catch (Exception ex)
        //    {
        //        IErrorLogService errorLog = new ErrorLogService();
        //        string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
        //        errorLog.SetErrorLog(model.CurrentUserID, "GeneralUnitPrice", message);
        //        throw new Exception(ex.Message);

        //    }
        //    return List;
        //}
    }

}
