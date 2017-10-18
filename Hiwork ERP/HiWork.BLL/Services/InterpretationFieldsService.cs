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
    public partial interface IInterpretationFieldsService : IBaseService<InterpretationFieldsModel, Master_StaffInterpretationFields>
    {
        List<InterpretationFieldsModel> SaveInterpretationFields(InterpretationFieldsModel model);
        List<InterpretationFieldsModel> GetAllInterpretationFieldsList(InterpretationFieldsModel model);
        List<InterpretationFieldsModel> DeleteInterpretationFields(InterpretationFieldsModel model);
    }

    public class InterpretationFieldsService : BaseService<InterpretationFieldsModel, Master_StaffInterpretationFields>, IInterpretationFieldsService
    {
        private readonly IInterpretationFieldsRepository _InterpretationFieldsRepository;
        public InterpretationFieldsService(IInterpretationFieldsRepository InterpretationFieldsRepository)
            : base(InterpretationFieldsRepository)
        {
            _InterpretationFieldsRepository = InterpretationFieldsRepository;
        }
        public List<InterpretationFieldsModel> SaveInterpretationFields(InterpretationFieldsModel model)
        {
            List<InterpretationFieldsModel> InterpretationFieldss = null;
            try
            {
                Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

                var InterpretationFields = Mapper.Map<InterpretationFieldsModel, Master_StaffInterpretationFields>(model);
                if (InterpretationFields.ID != Guid.Empty)
                {
                    InterpretationFields.UpdatedBy = model.CurrentUserID;
                    InterpretationFields.UpdatedDate = DateTime.Now;
                    _InterpretationFieldsRepository.UpdateInterpretationFields(InterpretationFields);
                }
                else
                {
                    InterpretationFields.ID = Guid.NewGuid();
                    InterpretationFields.CreatedBy = model.CurrentUserID;
                    InterpretationFields.CreatedDate = DateTime.Now;
                    _InterpretationFieldsRepository.InsertInterpretationFields(InterpretationFields);
                }
                InterpretationFieldss = GetAllInterpretationFieldsList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "InterpretationFields", message);
                throw new Exception(ex.Message);
            }
            return InterpretationFieldss;
        }
        public List<InterpretationFieldsModel> GetAllInterpretationFieldsList(InterpretationFieldsModel model)
        {
            List<InterpretationFieldsModel> InterpretationFieldsList = new List<InterpretationFieldsModel>();
            InterpretationFieldsModel InterpretationFieldsModel = new InterpretationFieldsModel();
            try
            {
                List<Master_StaffInterpretationFields> InterpretationFieldsvList = _InterpretationFieldsRepository.GetAllInterpretationFieldsList();
                if (InterpretationFieldsvList != null)
                {
                    InterpretationFieldsvList.ForEach(a =>
                    {
                        InterpretationFieldsModel = Mapper.Map<Master_StaffInterpretationFields, InterpretationFieldsModel>(a);
                        InterpretationFieldsModel.Name = Utility.GetPropertyValue(InterpretationFieldsModel, "Name", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(InterpretationFieldsModel, "Name", model.CurrentCulture).ToString();
                        InterpretationFieldsModel.Description = Utility.GetPropertyValue(InterpretationFieldsModel, "Description", model.CurrentCulture) == null ? string.Empty :
                            Utility.GetPropertyValue(InterpretationFieldsModel, "Description", model.CurrentCulture).ToString();
                        InterpretationFieldsModel.CurrentUserID = model.CurrentUserID;
                        InterpretationFieldsModel.CurrentCulture = model.CurrentCulture;
                        InterpretationFieldsList.Add(InterpretationFieldsModel);
                    });
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "InterpretationFields", message);
                throw new Exception(ex.Message);
            }
            return InterpretationFieldsList;
        }
        public List<InterpretationFieldsModel> DeleteInterpretationFields(InterpretationFieldsModel model)
        {
            List<InterpretationFieldsModel> InterpretationFieldss = null;
            try
            {
                _InterpretationFieldsRepository.DeleteInterpretationFields(model.ID);
                InterpretationFieldss = GetAllInterpretationFieldsList(model);
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "InterpretationFields", message);
                throw new Exception(ex.Message);
            }
            return InterpretationFieldss;
        }
    }
}
