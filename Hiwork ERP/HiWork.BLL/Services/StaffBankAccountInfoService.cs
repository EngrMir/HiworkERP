

/* ******************************************************************************************************************
 * Service for Staff_BankAccountInfo Entity
 * Date             :   14-July-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using AutoMapper;
using System;
using System.Collections.Generic;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.BLL.Models;
using HiWork.DAL.Database;

namespace HiWork.BLL.Services
{
    public interface IStaffBankAccountInfoService : IBaseService<StaffBankAccountInfoModel, Staff_BankAccountInfo>
    {
        List<StaffBankAccountInfoModel> GetStaffBankAccountInfoList(BaseViewModel sModel);
        List<StaffBankAccountInfoModel> SaveStaffBankAccountInfo(StaffBankAccountInfoModel sModel);
        List<StaffBankAccountInfoModel> DeleteStaffBankAccountInfo(StaffBankAccountInfoModel sModel);
    }

    public class StaffBankAccountInfoService : BaseService<StaffBankAccountInfoModel, Staff_BankAccountInfo>, IStaffBankAccountInfoService
    {
        private readonly IStaffBankAccountInfoRepository repository;
        public StaffBankAccountInfoService(IStaffBankAccountInfoRepository repo) : base(repo)
        {
            repository = repo;
        }

        private void LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "StaffBankAccountInfo", message);
            return;
        }

        public List<StaffBankAccountInfoModel> GetStaffBankAccountInfoList(BaseViewModel sModel)
        {
            List<Staff_BankAccountInfo> masterList;
            List<StaffBankAccountInfoModel> modelList = new List<StaffBankAccountInfoModel>();
            StaffBankAccountInfoModel aModel;

            try
            {
                masterList = repository.GetStaffBankAccountInfoList();
                if (masterList != null)
                {
                    foreach (Staff_BankAccountInfo masterModel in masterList)
                    {
                        aModel = Mapper.Map<Staff_BankAccountInfo, StaffBankAccountInfoModel>(masterModel);

                        aModel.CurrentCulture = sModel.CurrentCulture;
                        aModel.CurrentUserID = sModel.CurrentUserID;
                        modelList.Add(aModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex, sModel.CurrentUserID);
                throw new Exception(ex.Message);
            }
            return modelList;
        }

        public List<StaffBankAccountInfoModel> SaveStaffBankAccountInfo(StaffBankAccountInfoModel sModel)
        {
            Staff_BankAccountInfo masterModel;

            try
            {
                Utility.SetDynamicPropertyValue(sModel, sModel.CurrentCulture);
                masterModel = Mapper.Map<StaffBankAccountInfoModel, Staff_BankAccountInfo>(sModel);

                if (sModel.ID == Guid.Empty)
                {
                    masterModel.ID = Guid.NewGuid();
                    repository.InsertStaffBankAccountInfo(masterModel);
                }
                else
                {
                    repository.UpdateStaffBankAccountInfo(masterModel);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, sModel.CurrentUserID);
                throw new Exception(ex.Message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = sModel.CurrentUserID;
            md.CurrentCulture = sModel.CurrentCulture;
            return GetStaffBankAccountInfoList(md);
        }

        public List<StaffBankAccountInfoModel> DeleteStaffBankAccountInfo(StaffBankAccountInfoModel sModel)
        {
            try
            {
                repository.DeleteStaffBankAccountInfo(sModel.ID);
            }
            catch (Exception ex)
            {
                LogException(ex, sModel.CurrentUserID);
                throw new Exception(ex.Message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = sModel.CurrentUserID;
            md.CurrentCulture = sModel.CurrentCulture;
            return GetStaffBankAccountInfoList(md);
        }
        
    }
}
