

/* ******************************************************************************************************************
 * Service for Master_StaffLanguageSkillLevel Entity
 * Date             :   15-Jun-2017
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
    //public interface ILanguageSkillLevelService :
    //        IBaseService<LanguageSkillLevelModel, Master_StaffLanguageSkillLevel>
    //{
    //    List<LanguageSkillLevelModel>       GetLanguageSkillLevelList(BaseViewModel dataModel);
    //    List<LanguageSkillLevelModel>       SaveLanguageSkillLevel(LanguageSkillLevelModel dataModel);
    //    List<LanguageSkillLevelModel>       DeleteLanguageSkillLevel(LanguageSkillLevelModel dataModel);
    //}

    //public class LanguageSkillLevelService :
    //        BaseService<LanguageSkillLevelModel, Master_StaffLanguageSkillLevel>,
    //        ILanguageSkillLevelService
    //{
    //    private readonly ILanguageSkillLevelRepository repository;
    //    public LanguageSkillLevelService(ILanguageSkillLevelRepository repo) : base(repo)
    //    {
    //        repository = repo;
    //    }

    //    public List<LanguageSkillLevelModel> GetLanguageSkillLevelList(BaseViewModel dataModel)
    //    {
    //        object pValue;
    //        string sValue;
    //        List<Master_StaffLanguageSkillLevel> dataList;
    //        List<LanguageSkillLevelModel> modList = new List<LanguageSkillLevelModel>();
    //        LanguageSkillLevelModel aModel;

    //        try
    //        {
    //            dataList = repository.GetLanguageSkillLevelList();
    //            if (dataList != null)
    //            {
    //                foreach (Master_StaffLanguageSkillLevel data in dataList)
    //                {
    //                    aModel = Mapper.Map<Master_StaffLanguageSkillLevel, LanguageSkillLevelModel>(data);

    //                    pValue = Utility.GetPropertyValue(aModel, "Name", dataModel.CurrentCulture);
    //                    sValue = pValue == null ? string.Empty : pValue.ToString();
    //                    aModel.Name = sValue;

    //                    pValue = Utility.GetPropertyValue(aModel, "Description", dataModel.CurrentCulture);
    //                    sValue = pValue == null ? string.Empty : pValue.ToString();
    //                    aModel.Description = sValue;

    //                    aModel.CurrentCulture = dataModel.CurrentCulture;
    //                    aModel.CurrentUserID = dataModel.CurrentUserID;
    //                    modList.Add(aModel);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogException(ex, dataModel.CurrentUserID);
    //            throw new Exception(ex.Message);
    //        }

    //        modList.Sort(CompareByName);
    //        return modList;
    //    }

    //    public List<LanguageSkillLevelModel> SaveLanguageSkillLevel(LanguageSkillLevelModel dataModel)
    //    {
    //        Master_StaffLanguageSkillLevel data;

    //        try
    //        {
    //            Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
    //            data = Mapper.Map<LanguageSkillLevelModel, Master_StaffLanguageSkillLevel>(dataModel);

    //            if (dataModel.Id == Guid.Empty)
    //            {
    //                data.ID = Guid.NewGuid();
    //                data.CreatedBy = dataModel.CurrentUserID;
    //                data.CreatedDate = DateTime.Now;
    //                repository.InsertLanguageSkillLevel(data);
    //            }
    //            else
    //            {
    //                data.UpdatedBy = dataModel.CurrentUserID;
    //                data.UpdatedDate = DateTime.Now;
    //                repository.UpdateLanguageSkillLevel(data);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogException(ex, dataModel.CurrentUserID);
    //            throw new Exception(ex.Message);
    //        }

    //        BaseViewModel md = new BaseViewModel();
    //        md.CurrentUserID = dataModel.CurrentUserID;
    //        md.CurrentCulture = dataModel.CurrentCulture;
    //        return GetLanguageSkillLevelList(md);
    //    }

    //    public List<LanguageSkillLevelModel> DeleteLanguageSkillLevel(LanguageSkillLevelModel dataModel)
    //    {
    //        try
    //        {
    //            repository.DeleteLanguageSkillLevel(dataModel.Id);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogException(ex, dataModel.CurrentUserID);
    //            throw new Exception(ex.Message);
    //        }

    //        BaseViewModel md = new BaseViewModel();
    //        md.CurrentUserID = dataModel.CurrentUserID;
    //        md.CurrentCulture = dataModel.CurrentCulture;
    //        return GetLanguageSkillLevelList(md);
    //    }

    //    private void LogException(Exception ex, long userid)
    //    {
    //        IErrorLogService errorLog = new ErrorLogService();
    //        string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //        errorLog.SetErrorLog(userid, "LanguageSkillLevel", message);
    //        return;
    //    }

    //    private int CompareByName(LanguageSkillLevelModel dataModel1, LanguageSkillLevelModel dataModel2)
    //    {
    //        int cmpresult;
    //        cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
    //        return cmpresult;
    //    }
    //}
}
