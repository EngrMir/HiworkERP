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

    public partial interface ICountryService:  IBaseService<CountryModel, Master_Country>
    {
        List<CountryModel> GetAllCountryList(BaseViewModel model);
        List<CountryModel> SaveCountry(CountryModel aCountryModel);
        CountryModel EditCountry(CountryModel aCountryModel);
        List<CountryModel> DeleteCountry(CountryModel aCountryModel);
        List<CountryModel> GetTradinCountry(BaseViewModel model);

    }
    public class CountryService : BaseService<CountryModel, Master_Country>, ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository) : base(countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<CountryModel> SaveCountry(CountryModel aCountryModel)
        {
            List<CountryModel> countries = null;
            Utility.SetDynamicPropertyValue(aCountryModel, aCountryModel.CurrentCulture);

            var country = Mapper.Map<CountryModel, Master_Country>(aCountryModel);

            if (country.ID > 0)
            {

                country.CurrencyID = aCountryModel.CurrencyID;
                country.UpdatedBy = aCountryModel.CurrentUserID;
                country.UpdatedDate = DateTime.Now;
                _countryRepository.UpdateCountry(country);
            }
            else
            {
                country.CurrencyID = aCountryModel.CurrencyID;
                country.CreatedBy = aCountryModel.CurrentUserID;
                country.CreatedDate = DateTime.Now;
                _countryRepository.InsertCountry(country);
            }
            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = aCountryModel.CurrentCulture;
            model.CurrentUserID = aCountryModel.CurrentUserID;
            countries = GetAllCountryList(model);
            return countries;
        }
        public CountryModel EditCountry(CountryModel aCountryModel)
        {
            var country = Mapper.Map<CountryModel, Master_Country>(aCountryModel);
            var aCountry= _countryRepository.UpdateCountry(country);
            var countryModel = Mapper.Map<Master_Country, CountryModel>(aCountry);
            return countryModel;
        }

        //public List<CountryModel> GetAllCountryList(BaseViewModel model)
        //{
        //    List<CountryModel> countryList = new List<CountryModel>();
        //    CountryModel countryModel = new CountryModel();
        //    List<Master_Country> dbcountryList = _countryRepository.GetAllCountryList();
        //    if (dbcountryList != null)
        //    {
        //        dbcountryList.ForEach(a =>
        //        {
        //            countryModel = Mapper.Map<Master_Country, CountryModel>(a);

        //            countryModel.Name = Utility.GetPropertyValue(countryModel, "Name", model.CurrentCulture) == null ? string.Empty :
        //                                                  Utility.GetPropertyValue(countryModel, "Name", model.CurrentCulture).ToString();
                   
        //            countryModel.CurrentUserID = model.CurrentUserID;
        //            countryModel.CurrentCulture = model.CurrentCulture;
        //            countryList.Add(countryModel);
        //        });
        //    }

        //    countryList.Sort(CompareCountryByName);
        //    return countryList;
        //}
        public List<CountryModel> GetTradinCountry(BaseViewModel model)
        {
            List<CountryModel> countryList = new List<CountryModel>();
            CountryModel countryModel = new CountryModel();
            List<Master_Country> dbcountryList = _countryRepository.GetTradinCountry();
            if (dbcountryList != null)
            {
                dbcountryList.ForEach(a =>
                {
                    countryModel = Mapper.Map<Master_Country, CountryModel>(a);

                    countryModel.Name = Utility.GetPropertyValue(countryModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(countryModel, "Name", model.CurrentCulture).ToString();

                    countryModel.CurrentUserID = model.CurrentUserID;
                    countryModel.CurrentCulture = model.CurrentCulture;
                    countryList.Add(countryModel);
                });
            }

            countryList.Sort(CompareCountryByName);
            return countryList;
        }
        public List<CountryModel> DeleteCountry(CountryModel aCountryModel)
        {
            _countryRepository.DeleteCountry(aCountryModel.ID);
            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = aCountryModel.CurrentCulture;
            model.CurrentUserID = aCountryModel.CurrentUserID;
            List<CountryModel> countries = GetAllCountryList(model);
            return countries;
        }


        private int CompareCountryByName(CountryModel arg1, CountryModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }


        public List<CountryModel> GetAllCountryList(BaseViewModel model)
        {
            List<CountryModel> merList = new List<CountryModel>();
            CountryModel ccModel = new CountryModel();

            try
            {

                List<Master_Country> repoMasterEstimationSF = _countryRepository.GetAllCountryList();
                if (repoMasterEstimationSF != null)
                {
                    repoMasterEstimationSF.ForEach(a =>
                    {
                        ccModel = Mapper.Map<Master_Country, CountryModel>(a);
                        ccModel.CurrencyM = Mapper.Map<Master_Currency, CurrencyModel>(a.Master_Currency);

                        if (ccModel.CurrencyM != null)

                            ccModel.CurrencyM.Name = Utility.GetPropertyValue(ccModel.CurrencyM, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                       Utility.GetPropertyValue(ccModel.CurrencyM, "Name", model.CurrentCulture).ToString();

                        ccModel.Name = Utility.GetPropertyValue(ccModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(ccModel, "Name", model.CurrentCulture).ToString();


                        ccModel.CurrentUserID = model.CurrentUserID;
                        ccModel.CurrentCulture = model.CurrentCulture;

                        merList.Add(ccModel);
                    });

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Country", message);
                throw new Exception(ex.Message);
            }
            merList.Sort(CompareCountryByName);
            return merList;
        }

        

    }


}

