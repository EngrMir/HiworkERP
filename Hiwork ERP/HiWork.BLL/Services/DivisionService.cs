

/* ******************************************************************************************************************
 * Service for Master_Division Entity
 * Date             :   04-Jun-2017
 * By               :   Ashis
 * *****************************************************************************************************************/


using AutoMapper;
using HiWork.BLL.Models;
using HiWork.Utils;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;

namespace HiWork.BLL.Services
{
    public interface IDivisionService : IBaseService<DivisionModel, Master_Division>
    {
        List<DivisionModel> SaveDivision(DivisionModel aDivisionModel);
        List<DivisionModel> GetDivisionList(BaseViewModel model);
        List<DivisionModel> DeleteDivision(DivisionModel aDivisionModel);
    }

    public class DivisionService : BaseService<DivisionModel, Master_Division>, IDivisionService
    {
        private readonly IDivisionRepository _divRepository;
        public DivisionService(IDivisionRepository divRepository) : base(divRepository)
        {
            _divRepository = divRepository;
        }

        public List<DivisionModel> SaveDivision(DivisionModel aDivisionModel)
        {
            Master_Division division = null;

            try
            {
                Utility.SetDynamicPropertyValue(aDivisionModel, aDivisionModel.CurrentCulture);
                division = Mapper.Map<DivisionModel, Master_Division>(aDivisionModel);

                if (aDivisionModel.Id == Guid.Empty)
                {
                    division.ID = Guid.NewGuid();
                    division.CountryID = aDivisionModel.CountryId;
                    division.CreatedBy = aDivisionModel.CurrentUserID;
                    division.CreatedDate = DateTime.Now;
                    _divRepository.InsertDivision(division);
                }
                else
                {
                    division.CountryID = aDivisionModel.CountryId;
                    division.UpdatedBy = aDivisionModel.CurrentUserID;
                    division.UpdatedDate = DateTime.Now;
                    _divRepository.UpdateDivision(division);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, aDivisionModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = aDivisionModel.CurrentCulture;
            md.CurrentUserID = aDivisionModel.CurrentUserID;
            return GetDivisionList(md);
        }

        public List<DivisionModel> GetDivisionList(BaseViewModel model)
        {
            object pValue;
            string sValue;

            List<Master_Division> dataList;
            List<DivisionModel> divisionList = new List<DivisionModel>();
            DivisionModel divisionModel = new DivisionModel();

            try
            {
                dataList = _divRepository.GetDivisionList();
                if (dataList != null)
                {
                    foreach(Master_Division a in dataList)
                    {
                        divisionModel = Mapper.Map<Master_Division, DivisionModel>(a);
                        divisionModel.Country = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);

                        if (divisionModel.Country != null)
                        {
                            pValue = Utility.GetPropertyValue(divisionModel.Country, "Name", model.CurrentCulture);
                            sValue = pValue == null ? string.Empty : pValue.ToString();
                            divisionModel.Country.Name = sValue;
                        }

                        pValue = Utility.GetPropertyValue(divisionModel, "Name", model.CurrentCulture);
                        sValue = pValue == null ? string.Empty : pValue.ToString();
                        divisionModel.Name = sValue;
                        
                        divisionModel.CurrentUserID = model.CurrentUserID;
                        divisionModel.CurrentCulture = model.CurrentCulture;
                        divisionList.Add(divisionModel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, model.CurrentUserID);
                throw new Exception(message);
            }

            divisionList.Sort(CompareDivisionByName);
            return divisionList;
        }

        public List<DivisionModel> DeleteDivision(DivisionModel aDivisionModel)
        {
            try
            {
                _divRepository.DeleteDivision(aDivisionModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, aDivisionModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = aDivisionModel.CurrentCulture;
            md.CurrentUserID = aDivisionModel.CurrentUserID;
            return GetDivisionList(md);
        }

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "Division", message);
            return message;
        }

        private int CompareDivisionByName(DivisionModel dataModel1, DivisionModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
