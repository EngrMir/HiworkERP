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
using System.Data.SqlClient;

namespace HiWork.BLL.Services
{
    //public partial interface ITechnicalSkillLevelService : IBaseService<TechnicalSkillLevelModel, Master_StaffTechnicalSkillLevel>
    //{
    //    List<TechnicalSkillLevelModel> SaveStaffTechnicalSkillLevel(TechnicalSkillLevelModel model);
    //    List<TechnicalSkillLevelModel> GetAllStaffTechnicalSkillLevelList(TechnicalSkillLevelModel model);
    //    List<TechnicalSkillLevelModel> DeleteStaffTechnicalSkillLevel(TechnicalSkillLevelModel model);
    //}
    //public class TechnicalSkillLevelService : BaseService<TechnicalSkillLevelModel, Master_StaffTechnicalSkillLevel>, ITechnicalSkillLevelService
    //{
    //    private readonly ITechnicalSkillLevelRepository _technicalSkillLevelRepository;
    //    public TechnicalSkillLevelService(ITechnicalSkillLevelRepository technicalSkillLevelRepository) : base(technicalSkillLevelRepository)
    //    {
    //        _technicalSkillLevelRepository = technicalSkillLevelRepository;
    //    }
    //    public List<TechnicalSkillLevelModel> SaveStaffTechnicalSkillLevel(TechnicalSkillLevelModel model)
    //    {
    //        List<TechnicalSkillLevelModel> technicalSkillLevels = null;
    //        try
    //        {
    //            Utility.SetDynamicPropertyValue(model, model.CurrentCulture);

    //            var technicalSkillLevel = Mapper.Map<TechnicalSkillLevelModel, Master_StaffTechnicalSkillLevel>(model);
    //            if (technicalSkillLevel.ID != Guid.Empty)
    //            {
    //                technicalSkillLevel.UpdatedBy = model.CurrentUserID;
    //                technicalSkillLevel.UpdatedDate = DateTime.Now;
    //                _technicalSkillLevelRepository.UpdateStaffTechnicalSkillLevel(technicalSkillLevel);
    //            }
    //            else
    //            {
    //                technicalSkillLevel.ID = Guid.NewGuid();
    //                technicalSkillLevel.CreatedBy = model.CurrentUserID;
    //                technicalSkillLevel.CreatedDate = DateTime.Now;
    //                _technicalSkillLevelRepository.InsertStaffTechnicalSkillLevel(technicalSkillLevel);
    //            }
    //            technicalSkillLevels = GetAllStaffTechnicalSkillLevelList(model);
    //        }
    //        catch (Exception ex)
    //        {
    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "TechnicalSkillLevel", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return technicalSkillLevels;
    //    }
    //    public List<TechnicalSkillLevelModel> GetAllStaffTechnicalSkillLevelList(TechnicalSkillLevelModel model)
    //    {
    //        List<TechnicalSkillLevelModel> technicalSkillLevelModelList = new List<TechnicalSkillLevelModel>();
    //        TechnicalSkillLevelModel technicalSkillLevelModel;
    //        try
    //        {
    //            List<Master_StaffTechnicalSkillLevel> technicalSkillLevelList = _technicalSkillLevelRepository.GetAllStaffTechnicalSkillLevelList();
    //            if (technicalSkillLevelList != null)
    //            {
    //                technicalSkillLevelList.ForEach(a =>
    //                {
    //                    technicalSkillLevelModel = Mapper.Map<Master_StaffTechnicalSkillLevel, TechnicalSkillLevelModel>(a);
    //                    technicalSkillLevelModel.Name = Utility.GetPropertyValue(technicalSkillLevelModel, "Name", model.CurrentCulture) == null ? null :
    //                                                          Utility.GetPropertyValue(technicalSkillLevelModel, "Name", model.CurrentCulture).ToString();
    //                    technicalSkillLevelModel.CurrentUserID = model.CurrentUserID;
    //                    technicalSkillLevelModel.CurrentCulture = model.CurrentCulture;
    //                    technicalSkillLevelModelList.Add(technicalSkillLevelModel);
    //                });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "TechnicalSkillLevel", message);
    //            throw new Exception(ex.Message);
    //        }

    //        return technicalSkillLevelModelList;
    //    }
    //    public List<TechnicalSkillLevelModel> DeleteStaffTechnicalSkillLevel(TechnicalSkillLevelModel model)
    //    {
    //        List<TechnicalSkillLevelModel> technicalSkillLevelModelList = null;
    //        try
    //        {
    //            _technicalSkillLevelRepository.DeleteStaffTechnicalSkillLevel(model.ID);
    //            technicalSkillLevelModelList = GetAllStaffTechnicalSkillLevelList(model);
    //        }
    //        catch (Exception ex)
    //        {
    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "TechnicalSkillLevel", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return technicalSkillLevelModelList;
    //    }

    //}
}
