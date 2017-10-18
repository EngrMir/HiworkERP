using AutoMapper;
using HiWork.BLL.Models;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{
    public interface IBankAccountService : IBaseService<BankAccountModel, Master_BankAccount>
    {
        List<BankAccountModel> SaveBankAccount(BankAccountModel model);
        List<BankAccountModel> GetAllBankAccountList(BankAccountModel model);
        List<BankAccountModel> DeleteBankAccount(BankAccountModel model);
        List<BankAccountModel> UpdateBankAccount(BankAccountModel model);
        List<BankAccountTypeModel> GetAllBankAccountType(BaseViewModel model);
    }


    public class BankAccountService : BaseService<BankAccountModel, Master_BankAccount>, IBankAccountService
    {

        public IBankAccountRepository _bankAccountRepository;
        public BankAccountService(IBankAccountRepository bankAccountRepository) : base(bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public List<BankAccountModel> DeleteBankAccount(BankAccountModel model)
        {
            _bankAccountRepository.DeleteBankAccount(model.ID);
            var BankAccounts = GetAllBankAccountList(model);
            return BankAccounts;
        }

        public List<BankAccountModel> GetAllBankAccountList(BankAccountModel model)
        {
              List<BankAccountModel> bankAccList = new List<BankAccountModel>();
              BankAccountModel BankAccount = new BankAccountModel();


            try
            {
                List<Master_BankAccount> bankAccountList = _bankAccountRepository.GetAllBankAccountList();

                if (bankAccountList != null)
                {
                    bankAccountList.ForEach(a =>
                    {
                        BankAccount = Mapper.Map<Master_BankAccount, BankAccountModel>(a);

                        BankAccount.bankModel = Mapper.Map<Master_Bank, BankModel>(a.Master_Bank);

                        BankAccount.AccountTypeModel = Mapper.Map<Master_BankAccountType, BankAccountTypeModel>(a.Master_BankAccountType);

                        BankAccount.branchModel = Mapper.Map<Master_BankBranch, BankBranchModel>(a.Master_BankBranch);

                        if (BankAccount.bankModel != null)
                            BankAccount.bankModel.Name = Utility.GetPropertyValue(BankAccount.bankModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(BankAccount.bankModel, "Name", model.CurrentCulture).ToString();

                        if (BankAccount.AccountTypeModel != null)
                            BankAccount.AccountTypeModel.Name = Utility.GetPropertyValue(BankAccount.AccountTypeModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(BankAccount.AccountTypeModel, "Name", model.CurrentCulture).ToString();

                        if (BankAccount.branchModel != null)
                            BankAccount.branchModel.Name = Utility.GetPropertyValue(BankAccount.branchModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(BankAccount.branchModel, "Name", model.CurrentCulture).ToString();

                        BankAccount.AccountName = Utility.GetPropertyValue(BankAccount, "AccountName", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(BankAccount, "AccountName", model.CurrentCulture).ToString();


                        BankAccount.CurrentUserID = model.CurrentUserID;
                        BankAccount.ID = a.ID;
                        BankAccount.CurrentCulture = model.CurrentCulture;
                        bankAccList.Add(BankAccount);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Bank Account", message);
                throw new Exception(ex.Message);
            }

            bankAccList.Sort(CompareBankAccountByName);
            return bankAccList;
        }

        public List<BankAccountModel> SaveBankAccount(BankAccountModel model)
        {
            Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

            var bankAccount = Mapper.Map<BankAccountModel, Master_BankAccount>(model);

            if(model.ID==Guid.Empty)
            {
                bankAccount.ID = Guid.NewGuid();
                bankAccount.CreatedBy = model.CreatedBy;
                bankAccount.CreatedDate = DateTime.Now;
                _bankAccountRepository.InsertBankAccount(bankAccount);
            }
            else
            {
               
                bankAccount.UpdatedBy = model.UpdatedBy;
                bankAccount.UpdatedDate = DateTime.Now;
                _bankAccountRepository.UpdateBankAccount(bankAccount);

            }
          

            var BankAccounts = GetAllBankAccountList(model);
            return BankAccounts;
        }

        public List<BankAccountModel> UpdateBankAccount(BankAccountModel model)
        {
            var BankAccount = Mapper.Map<BankAccountModel, Master_BankAccount>(model);
            var aCountry = _bankAccountRepository.UpdateBankAccount(BankAccount);


            var BankAccounts = GetAllBankAccountList(model);
            return BankAccounts;
        }

        public List<BankAccountTypeModel> GetAllBankAccountType(BaseViewModel model)
        {
            List<BankAccountTypeModel> bankAccList = new List<BankAccountTypeModel>();
            BankAccountTypeModel BankAccountType = new BankAccountTypeModel();

            List<Master_BankAccountType> bankAccountList = _bankAccountRepository.GetAllBankAccountType();

            try
            {
                if (bankAccountList != null)
                {
                    bankAccountList.ForEach(a => {

                        BankAccountType = Mapper.Map<Master_BankAccountType, BankAccountTypeModel>(a);

                        BankAccountType.Name = Utility.GetPropertyValue(BankAccountType, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                  Utility.GetPropertyValue(BankAccountType, "Name", model.CurrentCulture).ToString();
                        bankAccList.Add(BankAccountType);
                    });
                }

            }
            catch(Exception ex) { ex.Message.ToString(); }

            bankAccList.Sort(CompareBankAccountTypeByName);
            return bankAccList;
        }

        private int CompareBankAccountByName(BankAccountModel arg1, BankAccountModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.AccountName, arg2.AccountName, true);
            return cmpresult;
        }

        private int CompareBankAccountTypeByName(BankAccountTypeModel arg1, BankAccountTypeModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
