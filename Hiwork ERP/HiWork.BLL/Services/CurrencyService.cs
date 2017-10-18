using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.Utils.Infrastructure.Contract;
using System.Collections.Generic;
using HiWork.Utils.Infrastructure;
using System;
using HiWork.DAL.Repositories;
using AutoMapper;
using HiWork.Utils;

namespace HiWork.BLL.Services
{
    public partial interface ICurrencyService : IBaseService<CurrencyModel, Master_Currency>
    {
        List<CurrencyModel> GetAllCurrencyList(BaseViewModel aCurrencyModel);
        List<CurrencyModel> SaveCurrency(CurrencyModel aCurrencyModel);
        CurrencyModel EditCurrency(CurrencyModel aCurrencyModel);
        List<CurrencyModel> DeleteCurrency(CurrencyModel aCurrencyModel);

    }
    public class CurrencyService : BaseService<CurrencyModel, Master_Currency>, ICurrencyService
    {
        private readonly ICurrencyRepository _CurrencyRepository;
        public CurrencyService(ICurrencyRepository CurrencyRepository) : base(CurrencyRepository)
        {
            _CurrencyRepository = CurrencyRepository;
        }

        public List<CurrencyModel> SaveCurrency(CurrencyModel aCurrencyModel)
        {
            List<CurrencyModel> currencies = null;
            Utility.SetDynamicPropertyValue(aCurrencyModel, aCurrencyModel.CurrentCulture);

            var Currency = Mapper.Map<CurrencyModel, Master_Currency>(aCurrencyModel);

            if (Currency.ID > 0)
            {
                Currency.UpdatedBy = aCurrencyModel.CurrentUserID;
                Currency.UpdatedDate = DateTime.Now;
                _CurrencyRepository.UpdateCurrency(Currency);
            }
            else
            {
                Currency.CreatedBy = aCurrencyModel.CurrentUserID;
                Currency.CreatedDate = DateTime.Now;
                _CurrencyRepository.InsertCurrency(Currency);
            }

            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = aCurrencyModel.CurrentCulture;
            model.CurrentUserID = aCurrencyModel.CurrentUserID;
            currencies = GetAllCurrencyList(model);
            return currencies;
        }
        public CurrencyModel EditCurrency(CurrencyModel aCurrencyModel)
        {
            var Currency = Mapper.Map<CurrencyModel, Master_Currency>(aCurrencyModel);
            var aCurrency = _CurrencyRepository.UpdateCurrency(Currency);
            var CurrencyModel = Mapper.Map<Master_Currency, CurrencyModel>(aCurrency);
            return CurrencyModel;
        }

        public List<CurrencyModel> GetAllCurrencyList(BaseViewModel aCurrencyModel)
        {
            List<CurrencyModel> CurrencyList = new List<CurrencyModel>();
            CurrencyModel CurrencyModel = new CurrencyModel();
            List<Master_Currency> dbCurrencyList = _CurrencyRepository.GetAllCurrencyList();
            if (CurrencyList != null)
            {
                dbCurrencyList.ForEach(a =>
                {
                    CurrencyModel = Mapper.Map<Master_Currency, CurrencyModel>(a);

                    CurrencyModel.Name = Utility.GetPropertyValue(CurrencyModel, "Name", aCurrencyModel.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(CurrencyModel, "Name", aCurrencyModel.CurrentCulture).ToString();
                    
                    CurrencyModel.CurrentUserID = aCurrencyModel.CurrentUserID;
                    CurrencyModel.CurrentCulture = aCurrencyModel.CurrentCulture;
                    CurrencyList.Add(CurrencyModel);
                });
            }

            CurrencyList.Sort(CompareCurrencyByName);
            return CurrencyList;
        }
        public List<CurrencyModel> DeleteCurrency(CurrencyModel aCurrencyModel)
        {
            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = aCurrencyModel.CurrentCulture;
            model.CurrentUserID = aCurrencyModel.CurrentUserID;
            _CurrencyRepository.DeleteCurrency(aCurrencyModel.Id);
            List<CurrencyModel> currencies = GetAllCurrencyList(model);
            return currencies;
        }

        private int CompareCurrencyByName(CurrencyModel arg1, CurrencyModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
