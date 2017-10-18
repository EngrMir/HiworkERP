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
    public partial interface IErrorReportWebService : IBaseService<ErrorReportWebModel, ErrorReportWeb>
    {
        List<ErrorReportWebModel> SaveErrorReportWeb(ErrorReportWebModel model);
        List<ErrorReportWebModel> GetErrorReportWeb(BaseViewModel model);
        List<ErrorReportWebModel> DeleteErrorReportWeb(ErrorReportWebModel model);
    }
    public class ErrorReportWebService : BaseService<ErrorReportWebModel,ErrorReportWeb>, IErrorReportWebService
    {
        private readonly IErrorReportWebRepository _errRepRepository;
        public ErrorReportWebService(IErrorReportWebRepository errRepRepository) : base(errRepRepository)
        {
            _errRepRepository = errRepRepository;
        }
        public List<ErrorReportWebModel> SaveErrorReportWeb(ErrorReportWebModel model)
        {
            List<ErrorReportWebModel> errReportWeb = null;
            //BaseViewModel bvModel = new BaseViewModel();
            //bvModel.ApplicationId = model.ApplicationId;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);
                var ErrorWeb = Mapper.Map<ErrorReportWebModel, ErrorReportWeb>(model);

                if (model.ID>0)
                { 
                    _errRepRepository.UpdateErrorReport(ErrorWeb);
                }
                else
                {
                    ErrorWeb.CreatedDate = DateTime.Now;
                    _errRepRepository.InsertErrorReport(ErrorWeb);
                }
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID;
                errReportWeb = GetErrorReportWeb(baseViewModel);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "ErrorReportWeb", message);
                throw new Exception(ex.Message);
            }
            return errReportWeb;
        }
       public  List<ErrorReportWebModel> GetErrorReportWeb(BaseViewModel model)
        {
            List<ErrorReportWebModel> erModel = new List<ErrorReportWebModel>();
            ErrorReportWebModel eModel = new ErrorReportWebModel();

            try
            {
                List<ErrorReportWeb> ErrWeb = _errRepRepository.GetErrorReport();
                if (ErrWeb !=null)
                {
                    ErrWeb.ForEach(a =>
                    {
                        eModel = Mapper.Map<ErrorReportWeb, ErrorReportWebModel>(a);
                        erModel.Add(eModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "ErrorReportWeb", message);
                throw new Exception(ex.Message);
            }
            return erModel;
        }
       public List<ErrorReportWebModel> DeleteErrorReportWeb(ErrorReportWebModel model)
        {
            List<ErrorReportWebModel> ErrorReport = null;
            try
            {
                _errRepRepository.DeleteErrorReport(model.ID);
                BaseViewModel baseViewModel = new BaseViewModel();
                baseViewModel.CurrentCulture = model.CurrentCulture;
                baseViewModel.CurrentUserID = model.CurrentUserID; 
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "ErrorReportWeb", message);
                throw new Exception(ex.Message);
            }
            return ErrorReport;
        }

    }
}
