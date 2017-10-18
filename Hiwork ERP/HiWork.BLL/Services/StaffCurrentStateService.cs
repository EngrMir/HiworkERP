using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{
    public partial interface IStaffCurrentStateService:IBaseService<StaffCurrentStateModel,Staff_CurrentStates>
    {
        List<StaffCurrentStateModel> SaveStaffCurrentState(StaffCurrentStateModel aModel);
        List<StaffCurrentStateModel> GetAllStaffCurrentState(BaseViewModel model);
        bool SaveStaffCurrentStateList(List<StaffCurrentStateModel> StateList);
    }


    public class StaffCurrentStateService : BaseService<StaffCurrentStateModel, Staff_CurrentStates>, IStaffCurrentStateService
    {
        private readonly IStaffCurrentStateRepository _cstateRepository;
        public StaffCurrentStateService(IStaffCurrentStateRepository cstateRepository) : base(cstateRepository)
        {
            _cstateRepository = cstateRepository;
        }

        public List<StaffCurrentStateModel> SaveStaffCurrentState(StaffCurrentStateModel aModel)
        {
            List<StaffCurrentStateModel> astateModel = null;
            try
            {
                Utility.SetDynamicPropertyValue(aModel, aModel.CurrentCulture);
                var staffcurrentstate = Mapper.Map<StaffCurrentStateModel, Staff_CurrentStates>(aModel);

                if (aModel.ID > 0)
                {

                    _cstateRepository.UpdateStaffCurrentState(staffcurrentstate);

                }
                else
                {

                    _cstateRepository.InsertStaffCurrentState(staffcurrentstate);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = aModel.CurrentCulture;
                baseViewModel.CurrentUserID = aModel.CurrentUserID;
                astateModel = GetAllStaffCurrentState(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aModel.CurrentUserID, "StaffCurrentState", message);
                throw new Exception(ex.Message);
            }
            return astateModel;
        }

        public bool SaveStaffCurrentStateList(List<StaffCurrentStateModel> StateList)
        {
            bool IsSuccessful;
            try
            {
                foreach (StaffCurrentStateModel aModel in StateList)
                {
                    Utility.SetDynamicPropertyValue(aModel, aModel.CurrentCulture);
                    var staffcurrentstate = Mapper.Map<StaffCurrentStateModel, Staff_CurrentStates>(aModel);

                    if (aModel.ID > 0)
                    {
                        _cstateRepository.UpdateStaffCurrentState(staffcurrentstate);
                    }
                    else
                    {
                        _cstateRepository.InsertStaffCurrentState(staffcurrentstate);
                    }
                }
                IsSuccessful = true;
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(0, "StaffCurrentState", message);
                throw new Exception(ex.Message);
            }
            return IsSuccessful;
        }

        public List<StaffCurrentStateModel> GetAllStaffCurrentState(BaseViewModel model)
        {
            List<StaffCurrentStateModel> usModel = new List<StaffCurrentStateModel>();
            StaffCurrentStateModel sModel = new StaffCurrentStateModel();
            try
            {
                List<Staff_CurrentStates> statelist = _cstateRepository.GetAllStaffCurrentStateList();
                if (statelist != null)
                {
                    statelist.ForEach(a =>
                    {
                        sModel = Mapper.Map<Staff_CurrentStates, StaffCurrentStateModel>(a);

                        sModel.CurrentUserID = model.CurrentUserID;
                        sModel.CurrentCulture = model.CurrentCulture;
                        usModel.Add(sModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CurrentState", message);
                throw new Exception(ex.Message);
            }

            return usModel;
        }

    }
}
