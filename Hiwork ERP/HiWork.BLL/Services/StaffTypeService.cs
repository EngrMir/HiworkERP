

/* ******************************************************************************************************************
 * Service for StaffType Entity
 * Date             :   13-Jun-2017
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
    public interface IStaffTypeService : IBaseService<StaffTypeModel, Master_StaffType>
    {
        List<StaffTypeModel>                    GetStaffTypeList(BaseViewModel sModel);
        List<StaffTypeModel>                    SaveStaffType(StaffTypeModel sModel);
        List<StaffTypeModel>                    DeleteStaffType(StaffTypeModel sModel);
    }

    public class StaffTypeService : BaseService <StaffTypeModel, Master_StaffType>, IStaffTypeService
    {
        private readonly IStaffTypeRepository repository;
        public StaffTypeService(IStaffTypeRepository repo) : base(repo)
        {
            repository = repo;
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "StaffType", message);
            return message;
        }

        public List<StaffTypeModel> GetStaffTypeList(BaseViewModel sModel)
        {
            object pValue;
            string sValue;
            List<Master_StaffType> masterList;
            List<StaffTypeModel> modelList = new List<StaffTypeModel>();
            StaffTypeModel aModel;

            try
            {
                masterList = repository.GetStaffTypeList();
                if (masterList != null)
                {
                    foreach(Master_StaffType masterModel in masterList)
                    {
                        aModel = Mapper.Map<Master_StaffType, StaffTypeModel>(masterModel);

                        pValue = Utility.GetPropertyValue(aModel, "Name", sModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.Name = sValue;

                        pValue = Utility.GetPropertyValue(aModel, "Description", sModel.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.Description = sValue;

                        aModel.CurrentCulture = sModel.CurrentCulture;
                        aModel.CurrentUserID = sModel.CurrentUserID;
                        modelList.Add(aModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, sModel.CurrentUserID);
                throw new Exception(message);
            }

            modelList.Sort(CompareByName);
            return modelList;
        }

        public List<StaffTypeModel> SaveStaffType(StaffTypeModel sModel)
        {
            Master_StaffType masterModel;

            try
            {
                Utility.SetDynamicPropertyValue(sModel, sModel.CurrentCulture);
                masterModel = Mapper.Map<StaffTypeModel, Master_StaffType>(sModel);

                if (sModel.Id == 0)
                {
                   // masterModel.ID = Guid.NewGuid();
                    masterModel.CreatedBy = sModel.CurrentUserID;
                    masterModel.CreatedDate = DateTime.Now;
                    repository.InsertStaffType(masterModel);
                }
                else
                {
                    masterModel.UpdatedBy = sModel.CurrentUserID;
                    masterModel.UpdatedDate = DateTime.Now;
                    repository.UpdateStaffType(masterModel);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, sModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = sModel.CurrentUserID;
            md.CurrentCulture = sModel.CurrentCulture;
            return GetStaffTypeList(md);
        }

        public List<StaffTypeModel> DeleteStaffType(StaffTypeModel sModel)
        {
            try
            {
                repository.DeleteStaffType(sModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, sModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentUserID = sModel.CurrentUserID;
            md.CurrentCulture = sModel.CurrentCulture;
            return GetStaffTypeList(md);
        }

        private int CompareByName(StaffTypeModel dataModel1, StaffTypeModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
