

/* ******************************************************************************************************************
 * Service for Master_StaffProfessionalSpeciality Entity
 * Date             :   16-Jun-2017
 * By               :   Ashis Kr. Das
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
    //public interface IProfessionalSpecialityService :
    //                        IBaseService<ProfessionalSpecialityModel, Master_StaffProfessionalSpeciality>
    //{
    //    List<ProfessionalSpecialityModel>   SaveProfessionalSpeciality(ProfessionalSpecialityModel dataModel);
    //    List<ProfessionalSpecialityModel>   GetProfessionalSpecialityList(BaseViewModel dataModel);
    //    List<ProfessionalSpecialityModel>   DeleteProfessionalSpeciality(ProfessionalSpecialityModel dataModel);
    //}

    //public class ProfessionalSpecialityService :
    //        BaseService<ProfessionalSpecialityModel, Master_StaffProfessionalSpeciality>,
    //        IProfessionalSpecialityService
    //{
    //    private readonly IProfessionalSpecialityRepository repository;
    //    public ProfessionalSpecialityService(IProfessionalSpecialityRepository repo) : base(repo)
    //    {
    //        repository = repo;
    //    }

    //    public List<ProfessionalSpecialityModel> SaveProfessionalSpeciality(ProfessionalSpecialityModel dataModel)
    //    {
    //        Master_StaffProfessionalSpeciality data;

    //        try
    //        {
    //            Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
    //            data = Mapper.Map<ProfessionalSpecialityModel, Master_StaffProfessionalSpeciality>(dataModel);

    //            if (dataModel.Id == Guid.Empty)
    //            {
    //                data.ID = Guid.NewGuid();
    //                data.CreatedBy = dataModel.CurrentUserID;
    //                data.CreatedDate = DateTime.Now;
    //                repository.InsertProfessionalSpeciality(data);
    //            }
    //            else
    //            {
    //                data.UpdatedBy = dataModel.CurrentUserID;
    //                data.UpdatedDate = DateTime.Now;
    //                repository.UpdateProfessionalSpeciality(data);
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
    //        return GetProfessionalSpecialityList(md);
    //    }

    //    public List<ProfessionalSpecialityModel> GetProfessionalSpecialityList(BaseViewModel dataModel)
    //    {
    //        object pValue;
    //        string sValue;
    //        List<Master_StaffProfessionalSpeciality> dataList;
    //        ProfessionalSpecialityModel aModel;
    //        List<ProfessionalSpecialityModel> modList = new List<ProfessionalSpecialityModel>();

    //        try
    //        {
    //            dataList = repository.GetProfessionalSpecialityList();
    //            if (dataList != null)
    //            {
    //                foreach(Master_StaffProfessionalSpeciality data in  dataList)
    //                {
    //                    aModel = Mapper.Map<Master_StaffProfessionalSpeciality, ProfessionalSpecialityModel>(data);

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

    //    public List<ProfessionalSpecialityModel> DeleteProfessionalSpeciality(ProfessionalSpecialityModel dataModel)
    //    {
    //        try
    //        {
    //            repository.DeleteProfessionalSpeciality(dataModel.Id);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogException(ex, dataModel.CurrentUserID);
    //            throw new Exception(ex.Message);
    //        }

    //        BaseViewModel md = new BaseViewModel();
    //        md.CurrentUserID = dataModel.CurrentUserID;
    //        md.CurrentCulture = dataModel.CurrentCulture;
    //        return GetProfessionalSpecialityList(md);
    //    }

    //    private void LogException(Exception ex, long userid)
    //    {
    //        IErrorLogService errorLog = new ErrorLogService();
    //        string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //        errorLog.SetErrorLog(userid, "ProfessionalSpeciality", message);
    //        return;
    //    }

    //    private int CompareByName(ProfessionalSpecialityModel dataModel1, ProfessionalSpecialityModel dataModel2)
    //    {
    //        int cmpresult;
    //        cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
    //        return cmpresult;
    //    }
    //}
}
