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
    public partial interface ICurrentStateService : IBaseService<CurrentStateModel, Master_StaffCurrentState>
    {
        List<CurrentStateModel> SaveCurrentState(CurrentStateModel model);
        List<CurrentStateModel> GetAllCurrentStateList(BaseViewModel model);
        List<CurrentStateModel> DeleteCurrentState(CurrentStateModel model);
    }
    public class CurrentStateService : BaseService<CurrentStateModel, Master_StaffCurrentState>, ICurrentStateService
    {
        private readonly ICurrentStateRepository _currentStateRepository;
        public CurrentStateService(ICurrentStateRepository currentStateRepository)
            : base(currentStateRepository)
        {
            _currentStateRepository = currentStateRepository;
        }
        public List<CurrentStateModel> DeleteCurrentState(CurrentStateModel model)
        {
            List<CurrentStateModel> CurrentStates = null;
            try
            {
                _currentStateRepository.DeleteCurrentState(model.ID);
                BaseViewModel BaseModel = new BaseViewModel();
                BaseModel.CurrentCulture = model.CurrentCulture;
                BaseModel.CurrentUserID = model.CurrentUserID;
                CurrentStates = GetAllCurrentStateList(BaseModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CurrentState", message);
                throw new Exception(ex.Message);
            }
            return CurrentStates;
        }

        public List<CurrentStateModel> GetAllCurrentStateList(BaseViewModel model)
        {
            List<CurrentStateModel> CurrentStateList = new List<CurrentStateModel>();
            CurrentStateModel CurrentStateModel = new CurrentStateModel();
            try
            {
                List<Master_StaffCurrentState> CurrentStatevList = _currentStateRepository.GetAllCurrentStateList();
                if (CurrentStatevList != null)
                {
                    CurrentStatevList.ForEach(a =>
                    {
                        CurrentStateModel = Mapper.Map<Master_StaffCurrentState, CurrentStateModel>(a);
                        CurrentStateModel.Name = Utility.GetPropertyValue(CurrentStateModel, "Name", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(CurrentStateModel, "Name", model.CurrentCulture).ToString();
                        CurrentStateModel.Description = Utility.GetPropertyValue(CurrentStateModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(CurrentStateModel, "Description", model.CurrentCulture).ToString();
                        CurrentStateModel.CurrentUserID = model.CurrentUserID;
                        CurrentStateModel.CurrentCulture = model.CurrentCulture;
                        CurrentStateList.Add(CurrentStateModel);
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
            return CurrentStateList;
        }

        public List<CurrentStateModel> SaveCurrentState(CurrentStateModel model)
        {
            List<CurrentStateModel> CurrentStates = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var CurrentState = Mapper.Map<CurrentStateModel, Master_StaffCurrentState>(model);
                if (CurrentState.ID != Guid.Empty)
                {
                    CurrentState.UpdatedBy = model.CurrentUserID;
                    CurrentState.UpdatedDate = DateTime.Now;
                    _currentStateRepository.UpdateCurrentState(CurrentState);
                }
                else
                {
                    CurrentState.ID = Guid.NewGuid();
                    CurrentState.CreatedBy = model.CurrentUserID;
                    CurrentState.CreatedDate = DateTime.Now;
                    _currentStateRepository.InsertCurrentState(CurrentState);
                }
                BaseViewModel BaseModel = new BaseViewModel();
                BaseModel.CurrentCulture = model.CurrentCulture;
                BaseModel.CurrentUserID = model.CurrentUserID;
                CurrentStates = GetAllCurrentStateList(BaseModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CurrentState", message);
                throw new Exception(ex.Message);
            }
            return CurrentStates;
        }
    }
}
