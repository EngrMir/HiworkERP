

/* ******************************************************************************************************************
 * Service for Master_StaffOtherExperiences Entity
 * Date             :   16-Jun-2017
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
    //public interface IOtherExperiencesService :
    //                    IBaseService<OtherExperiencesModel, Master_StaffOtherExperiences>
    //{
    //    List<OtherExperiencesModel> SaveOtherExperiences(OtherExperiencesModel dataModel);
    //    List<OtherExperiencesModel> GetOtherExperiencesList(BaseViewModel dataModel);
    //    List<OtherExperiencesModel> DeleteOtherExperiences(OtherExperiencesModel dataModel);
    //}

    //public class OtherExperiencesService :
    //                    BaseService<OtherExperiencesModel, Master_StaffOtherExperiences>,
    //                    IOtherExperiencesService
    //{
    //    private readonly IOtherExperiencesRepository repository;
    //    public OtherExperiencesService(IOtherExperiencesRepository repo) : base(repo)
    //    {
    //        repository = repo;
    //    }

    //    public List<OtherExperiencesModel> SaveOtherExperiences(OtherExperiencesModel dataModel)
    //    {
    //        Master_StaffOtherExperiences data;

    //        try
    //        {
    //            Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
    //            data = Mapper.Map<OtherExperiencesModel, Master_StaffOtherExperiences>(dataModel);

    //            if (dataModel.Id == Guid.Empty)
    //            {
    //                data.ID = Guid.NewGuid();
    //                data.CreatedBy = dataModel.CurrentUserID;
    //                data.CreatedDate = DateTime.Now;
    //                repository.InsertOtherExperiences(data);
    //            }
    //            else
    //            {
    //                data.UpdatedBy = dataModel.CurrentUserID;
    //                data.UpdatedDate = DateTime.Now;
    //                repository.UpdateOtherExperiences(data);
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
    //        return GetOtherExperiencesList(md);
    //    }

    //    public List<OtherExperiencesModel> GetOtherExperiencesList(BaseViewModel dataModel)
    //    {
    //        object pValue;
    //        string sValue;
    //        List<Master_StaffOtherExperiences> dataList;
    //        OtherExperiencesModel aModel;
    //        List<OtherExperiencesModel> modList = new List<OtherExperiencesModel>();

    //        try
    //        {
    //            dataList = repository.GetOtherExperiencesList();
    //            if (dataList != null)
    //            {
    //                foreach(Master_StaffOtherExperiences data in dataList)
    //                {
    //                    aModel = Mapper.Map<Master_StaffOtherExperiences, OtherExperiencesModel>(data);

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

    //    public List<OtherExperiencesModel> DeleteOtherExperiences(OtherExperiencesModel dataModel)
    //    {
    //        try
    //        {
    //            repository.DeleteOtherExperiences(dataModel.Id);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogException(ex, dataModel.CurrentUserID);
    //            throw new Exception(ex.Message);
    //        }
            
    //        BaseViewModel md = new BaseViewModel();
    //        md.CurrentUserID = dataModel.CurrentUserID;
    //        md.CurrentCulture = dataModel.CurrentCulture;
    //        return GetOtherExperiencesList(md);
    //    }

    //    private void LogException(Exception ex, long userid)
    //    {
    //        IErrorLogService errorLog = new ErrorLogService();
    //        string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //        errorLog.SetErrorLog(userid, "OtherExperiences", message);
    //        return;
    //    }

    //    private int CompareByName(OtherExperiencesModel dataModel1, OtherExperiencesModel dataModel2)
    //    {
    //        int cmpresult;
    //        cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
    //        return cmpresult;
    //    }
    //}
}
