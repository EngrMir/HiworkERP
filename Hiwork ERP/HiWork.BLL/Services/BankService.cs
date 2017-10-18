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
    public partial interface IBankService : IBaseService<BankModel, Master_Bank>
    {
        List<BankModel> SaveBank(BankModel model);
        List<BankModel> GetAllBankList(BaseViewModel model);
        List<BankModel> DeleteBank(BankModel model);
    }
    public class BankService : BaseService<BankModel, Master_Bank>, IBankService
    {
        private readonly IBankRepository _bankRepository;
        public BankService(IBankRepository bankRepository) : base(bankRepository)
        {
            _bankRepository = bankRepository;
        }     
        public List<BankModel> SaveBank(BankModel aBankModel)
        {
            List<BankModel> banks = null;
            try
              {
                Utility.SetDynamicPropertyValue(aBankModel, aBankModel.CurrentCulture);
                var bank = Mapper.Map<BankModel, Master_Bank>(aBankModel);

                if (aBankModel.Id==Guid.Empty)
                 {
                    bank.ID = Guid.NewGuid();
                    bank.CountryID = aBankModel.CountryId;
                    bank.CreatedBy = aBankModel.CurrentUserID;
                    bank.CreatedDate = DateTime.Now;
                    _bankRepository.InsertBank(bank);
                 }
                else
                 {
                    bank.CountryID = aBankModel.CountryId;
                    bank.UpdatedBy = aBankModel.CurrentUserID;
                    bank.UpdatedDate = DateTime.Now;
                    _bankRepository.UpdateBank(bank);
                 }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aBankModel.CurrentCulture;
                baseViewModel.CurrentUserID = aBankModel.CurrentUserID;
                banks = GetAllBankList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aBankModel.CurrentUserID, "Bank", message);
                throw new Exception(ex.Message);
            }
            return banks;
        }
        public List<BankModel> GetAllBankList(BaseViewModel model)
        {
            List<BankModel> bankModelList = new List<BankModel>();
            BankModel bankModel = new BankModel();
            try
            {
                List<Master_Bank> bankList = _bankRepository.GetAllBankList();
            if (bankList != null)
            {
                bankList.ForEach(a =>
                {
                    bankModel = Mapper.Map<Master_Bank, BankModel>(a);
                    bankModel.Country = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);
                    bankModel.Currency = Mapper.Map<Master_Currency, CurrencyModel>(a.Master_Currency);

                    if (bankModel.Country != null)
                        bankModel.Country.Name = Utility.GetPropertyValue(bankModel.Country, "Name", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(bankModel.Country, "Name", model.CurrentCulture).ToString();
                    if (bankModel.Currency != null)
                        bankModel.Currency.Name = Utility.GetPropertyValue(bankModel.Currency, "Name", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(bankModel.Currency, "Name", model.CurrentCulture).ToString();

                    bankModel.Name = Utility.GetPropertyValue(bankModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                          Utility.GetPropertyValue(bankModel, "Name", model.CurrentCulture).ToString();
                    bankModel.CurrentUserID = model.CurrentUserID;

                    bankModel.CurrentCulture = model.CurrentCulture;
                    bankModelList.Add(bankModel);
                });
             }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Bank", message);
                throw new Exception(ex.Message);
            }

            bankModelList.Sort(CompareBankByName);
            return bankModelList;
        }
        public List<BankModel> DeleteBank(BankModel model)
        {
            List<BankModel> banks = null;
            try
            {
                _bankRepository.DeleteBank(model.Id);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                banks = GetAllBankList(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Bank", message);
                throw new Exception(ex.Message);
            }
            return banks;
        }

        private int CompareBankByName(BankModel arg1, BankModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
