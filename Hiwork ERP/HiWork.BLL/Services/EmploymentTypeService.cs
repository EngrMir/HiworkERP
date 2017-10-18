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
     
    public partial interface IEmploymentTypeService : IBaseService<EmploymentTypeModel, Master_StaffEmploymentType>
    {
        List<EmploymentTypeModel> SaveEmploymentType(EmploymentTypeModel model);
        List<EmploymentTypeModel> GetAllEmploymentTypeList(EmploymentTypeModel model);
        List<EmploymentTypeModel> DeleteEmploymentType(EmploymentTypeModel model);
    }

    public class EmploymentTypeService : BaseService<EmploymentTypeModel, Master_StaffEmploymentType>, IEmploymentTypeService
    {
        private readonly IEmploymentTypeRepository _EmploymentTypeRepository;
        public EmploymentTypeService(IEmploymentTypeRepository EmploymentTypeRepository)
            : base(EmploymentTypeRepository)
        {
            _EmploymentTypeRepository = EmploymentTypeRepository;
        }
        public List<EmploymentTypeModel> SaveEmploymentType(EmploymentTypeModel model)
        {
            List<EmploymentTypeModel> EmploymentTypes = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var EmploymentType = Mapper.Map<EmploymentTypeModel, Master_StaffEmploymentType>(model);
                if (EmploymentType.ID != Guid.Empty)
                {
                    EmploymentType.UpdatedBy = model.CurrentUserID;
                    EmploymentType.UpdatedDate = DateTime.Now;
                    _EmploymentTypeRepository.UpdateEmploymentType(EmploymentType);
                }
                else
                {
                    EmploymentType.ID = Guid.NewGuid();
                    EmploymentType.CreatedBy = model.CurrentUserID;
                    EmploymentType.CreatedDate = DateTime.Now;
                    _EmploymentTypeRepository.InsertEmploymentType(EmploymentType);
                }
                EmploymentTypes = GetAllEmploymentTypeList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EmploymentType", message);
                throw new Exception(ex.Message);
            }
            return EmploymentTypes;
        }
        public List<EmploymentTypeModel> GetAllEmploymentTypeList(EmploymentTypeModel model)
        {
            List<EmploymentTypeModel> EmploymentTypeList = new List<EmploymentTypeModel>();
            EmploymentTypeModel EmploymentTypeModel = new EmploymentTypeModel();
            try
            {
                List<Master_StaffEmploymentType> EmploymentTypevList = _EmploymentTypeRepository.GetAllEmploymentTypeList();
                if (EmploymentTypevList != null)
                {
                    EmploymentTypevList.ForEach(a =>
                    {
                        EmploymentTypeModel = Mapper.Map<Master_StaffEmploymentType, EmploymentTypeModel>(a);
                        EmploymentTypeModel.Name = Utility.GetPropertyValue(EmploymentTypeModel, "Name", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(EmploymentTypeModel, "Name", model.CurrentCulture).ToString();
                        EmploymentTypeModel.Description = Utility.GetPropertyValue(EmploymentTypeModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(EmploymentTypeModel, "Description", model.CurrentCulture).ToString();
                        EmploymentTypeModel.CurrentUserID = model.CurrentUserID;
                        EmploymentTypeModel.CurrentCulture = model.CurrentCulture;
                        EmploymentTypeList.Add(EmploymentTypeModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EmploymentType", message);
                throw new Exception(ex.Message);
            }
            return EmploymentTypeList;
        }
        public List<EmploymentTypeModel> DeleteEmploymentType(EmploymentTypeModel model)
        {
            List<EmploymentTypeModel> EmploymentTypes = null;
            try
            {
                _EmploymentTypeRepository.DeleteEmploymentType(model.ID);
                EmploymentTypes = GetAllEmploymentTypeList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "EmploymentType", message);
                throw new Exception(ex.Message);
            }
            return EmploymentTypes;
        }
    }
}
