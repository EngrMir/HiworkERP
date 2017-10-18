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
    public partial interface IVisaTypeService : IBaseService<VisaTypeModel,Master_StaffVisaType>
    {
        List<VisaTypeModel> SaveVisaType(VisaTypeModel model);
        List<VisaTypeModel> GetAllVisaTypeList(VisaTypeModel model);
        List<VisaTypeModel> DeleteVisaType(VisaTypeModel model);
    }

    public class VisaTypeService : BaseService<VisaTypeModel, Master_StaffVisaType>, IVisaTypeService
    {
        private readonly IVisaTypeRepository _visaTypeRepository;
        public VisaTypeService(IVisaTypeRepository visaTypeRepository)
            : base(visaTypeRepository)
        {
            _visaTypeRepository = visaTypeRepository;
        }
        public List<VisaTypeModel> SaveVisaType(VisaTypeModel model)
        {
            List<VisaTypeModel> visaTypes = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

            var visaType = Mapper.Map<VisaTypeModel, Master_StaffVisaType>(model);
            if (visaType.ID != Guid.Empty)
            {
                visaType.UpdatedBy = model.CurrentUserID;
                visaType.UpdatedDate = DateTime.Now;
                _visaTypeRepository.UpdateVisaType(visaType);
            }
            else
            {
                    visaType.ID = Guid.NewGuid();
                    visaType.CreatedBy = model.CurrentUserID;
                visaType.CreatedDate = DateTime.Now;
                _visaTypeRepository.InsertVisaType(visaType);
            }
            visaTypes = GetAllVisaTypeList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "VisaType", message);
                throw new Exception(ex.Message);
            }
            return visaTypes;
        }
        public List<VisaTypeModel> GetAllVisaTypeList(VisaTypeModel model)
        {
            List<VisaTypeModel> visaTypeList = new List<VisaTypeModel>();
            VisaTypeModel visaTypeModel = new VisaTypeModel();
            try
            {
                List<Master_StaffVisaType> visaTypevList = _visaTypeRepository.GetAllVisaTypeList();
            if (visaTypevList != null)
            {
                visaTypevList.ForEach(a =>
                {
                    visaTypeModel = Mapper.Map<Master_StaffVisaType, VisaTypeModel>(a);
                    visaTypeModel.Name = Utility.GetPropertyValue(visaTypeModel, "Name", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(visaTypeModel, "Name", model.CurrentCulture).ToString();
                    visaTypeModel.Description = Utility.GetPropertyValue(visaTypeModel, "Description", model.CurrentCulture) == null ? string.Empty :
                        Utility.GetPropertyValue(visaTypeModel, "Description", model.CurrentCulture).ToString();
                    visaTypeModel.CurrentUserID = model.CurrentUserID;
                    visaTypeModel.CurrentCulture = model.CurrentCulture;
                    visaTypeList.Add(visaTypeModel);
                });
            }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "VisaType", message);
                throw new Exception(ex.Message);
            }
            return visaTypeList;
        }        
        public List<VisaTypeModel> DeleteVisaType(VisaTypeModel model)
        {
            List<VisaTypeModel> visaTypes = null;
            try
            {
                _visaTypeRepository.DeleteVisaType(model.ID);
                visaTypes = GetAllVisaTypeList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "VisaType", message);
                throw new Exception(ex.Message);
            }
            return visaTypes;
        }
    }
}
