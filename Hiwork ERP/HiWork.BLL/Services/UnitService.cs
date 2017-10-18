

/* ******************************************************************************************************************
 * Service for Master_Unit Entity
 * Date             :   02-July-2017
 * By               :   Ashis
 * Updated by       : Tamal (8/19/2017)
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
    public partial interface IUnitService : IBaseService<UnitModel, Master_Unit>
    {
        List<UnitModel> GetUnitList(BaseViewModel arg);
        List<UnitModel> SaveUnit(UnitModel uModel);
        List<UnitModel> DeleteUnit(UnitModel uModel);
    }
    public class UnitService : BaseService<UnitModel, Master_Unit>, IUnitService
    {
        private readonly IUnitRepository repository;
        public UnitService(IUnitRepository repo) : base(repo)
        {
            this.repository = repo;
        }
        public List<UnitModel> GetUnitList(BaseViewModel arg)
        {
            object pValue;
            string sValue;
            List<Master_Unit> dataList;
            UnitModel aModel;
            List<UnitModel> modList = new List<UnitModel>();

            try
            {
                dataList = repository.GetUnitList();
                if (dataList != null)
                {
                    foreach (Master_Unit data in dataList)
                    {
                        aModel = Mapper.Map<Master_Unit, UnitModel>(data);

                        pValue = Utility.GetPropertyValue(aModel, "Name", arg.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        aModel.Name = sValue;

                        aModel.CurrentCulture = arg.CurrentCulture;
                        aModel.CurrentUserID = arg.CurrentUserID;
                        modList.Add(aModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, arg.CurrentUserID);
                throw new Exception(message);
            }
            return modList;
        }

        public List<UnitModel> SaveUnit(UnitModel uModel)
        {
            List<UnitModel> units = null;
            try
            {
                Utility.SetDynamicPropertyValue(uModel, uModel.CurrentCulture);

                var unit = Mapper.Map<UnitModel, Master_Unit>(uModel);

                if (unit.ID > 0)
                {
                    unit.UpdatedBy = uModel.CurrentUserID;
                    unit.UpdatedDate = DateTime.Now;
                    repository.UpdateUnit(unit);
                }
                else
                {
                    unit.CreatedBy = uModel.CurrentUserID;
                    unit.CreatedDate = DateTime.Now;
                    repository.InsertUnit(unit);
                }
            }
            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(uModel.CurrentUserID, "Unit", message);
                throw new Exception(ex.Message);
            }

            BaseViewModel basemodel = new BaseViewModel();
            basemodel.CurrentCulture = uModel.CurrentCulture;
            basemodel.CurrentUserID = uModel.CurrentUserID;
            units = GetUnitList(basemodel);
            return units;
        }

        public List<UnitModel> DeleteUnit(UnitModel uModel)
        {
            repository.DeleteUnit(uModel.ID);
            BaseViewModel model = new BaseViewModel();
            model.CurrentCulture = uModel.CurrentCulture;
            model.CurrentUserID = uModel.CurrentUserID;
            List<UnitModel> units = GetUnitList(model);
            return units;
        }


        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "Unit", message);
            return message;
        }
    }
}
