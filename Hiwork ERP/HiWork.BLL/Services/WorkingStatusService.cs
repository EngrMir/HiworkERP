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
    public partial interface IWorkingStatusService : IBaseService<WorkingStatusModel, Master_WorkingStatus>
    {
        List<WorkingStatusModel> GetAllList(BaseViewModel model);
    }
    public class WorkingStatusService : BaseService<WorkingStatusModel, Master_WorkingStatus>, IWorkingStatusService
    {
        private readonly IWorkingStatusRepository _workingStatusRepository;
        public WorkingStatusService(IWorkingStatusRepository workingStatusRepository)
            : base(workingStatusRepository)
        {
            _workingStatusRepository = workingStatusRepository;
        }
        public List<WorkingStatusModel> GetAllList(BaseViewModel model)
        {
            var itemList = new List<WorkingStatusModel>();
            var item = new WorkingStatusModel();
            try
            {
                var items = _workingStatusRepository.GetAllList();
                if (items != null)
                {
                    items.ForEach(a =>
                    {
                        item.Name = a.GetType().GetProperty($"Name_{model.CurrentCulture}").GetValue(a, null)?.ToString();
                        item.CurrentUserID = model.CurrentUserID;
                        item.CurrentCulture = model.CurrentCulture;
                        itemList.Add(item);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "WorkingStatus", message);
                throw new Exception(ex.Message);
            }
            return itemList;
        }
    }
}
