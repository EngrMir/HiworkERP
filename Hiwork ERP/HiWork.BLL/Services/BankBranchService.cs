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
    public interface IBankBranchService : IBaseService<BankBranchModel, Master_BankBranch>
    {
        List<BankBranchModel> SaveBankBranch(BankBranchModel model);
        List<BankBranchModel> GetAllBankBranchList(BaseViewModel model);
        List<BankBranchModel> DeleteBankBranch(BankBranchModel model);
        List<BankBranchModel> UpdateBankBranch(BankBranchModel model);
    }

    public class BankBranchSerivce : BaseService<BankBranchModel, Master_BankBranch>, IBankBranchService
    {

        private IBankBranchRepository _bankBranchRepository;
        public BankBranchSerivce(IBankBranchRepository dbRepository) : base(dbRepository)
        {
            _bankBranchRepository = dbRepository;
        }

        public List<BankBranchModel> DeleteBankBranch(BankBranchModel model)
        {
            _bankBranchRepository.DeleteBankBranch(model.ID);
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            var BankBranch = GetAllBankBranchList(baseViewModel);
            return BankBranch;
        }

        public List<BankBranchModel> GetAllBankBranchList(BaseViewModel model)
        {
            List<BankBranchModel> bankBranchList = new List<BankBranchModel>();
            BankBranchModel bankBranch = new BankBranchModel();
            try
            {
                List<Master_BankBranch> bankbranchList = _bankBranchRepository.GetAllBankBranchList();

                if (bankbranchList != null)
                {
                    bankbranchList.ForEach(a =>
                    {
                        bankBranch = Mapper.Map<Master_BankBranch, BankBranchModel>(a);
                        bankBranch.bankModel = Mapper.Map<Master_Bank, BankModel>(a.Master_Bank);
                      
                        if (bankBranch.bankModel != null)
                            bankBranch.bankModel.Name = Utility.GetPropertyValue(bankBranch.bankModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(bankBranch.bankModel, "Name", model.CurrentCulture).ToString();

                        bankBranch.Name = Utility.GetPropertyValue(bankBranch, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(bankBranch, "Name", model.CurrentCulture).ToString();

                        bankBranch.Address = Utility.GetPropertyValue(bankBranch, "Address", model.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(bankBranch, "Address", model.CurrentCulture).ToString();

                        bankBranch.ID = a.ID;
                        bankBranch.CurrentUserID = model.CurrentUserID;

                        bankBranch.CurrentCulture = model.CurrentCulture;
                        bankBranchList.Add(bankBranch);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "BankBranch", message);
                throw new Exception(ex.Message);
            }

            bankBranchList.Sort(CompareBankBranchByName);
            return bankBranchList;
        }

        public List<BankBranchModel> SaveBankBranch(BankBranchModel model)
        {
            Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

            var bankBranch = Mapper.Map<BankBranchModel, Master_BankBranch>(model);

            if(model.ID==Guid.Empty)
            {
                bankBranch.ID = Guid.NewGuid();
                bankBranch.CreatedBy = model.CreatedBy;
                bankBranch.CreatedDate = DateTime.Now;
                _bankBranchRepository.InsertBankBranch(bankBranch);
            }
            else
            {
                
                bankBranch.UpdatedBy = model.CreatedBy;
                bankBranch.UpdatedDate = DateTime.Now;
                _bankBranchRepository.UpdateBankBranch(bankBranch);
            }


            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            var BankBranch = GetAllBankBranchList(baseViewModel);
            return BankBranch;
        }

        public List<BankBranchModel> UpdateBankBranch(BankBranchModel model)
        {
            var bankbranch = Mapper.Map<BankBranchModel, Master_BankBranch>(model);
             _bankBranchRepository.UpdateBankBranch(bankbranch);
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.CurrentCulture = model.CurrentCulture;
            baseViewModel.CurrentUserID = model.CurrentUserID;
            var BankBranch = GetAllBankBranchList(baseViewModel);
            return BankBranch;
        }

        private int CompareBankBranchByName(BankBranchModel arg1, BankBranchModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
