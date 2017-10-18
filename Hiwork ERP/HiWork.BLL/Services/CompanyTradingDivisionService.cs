

/* ******************************************************************************************************************
 * Service for Master_CompanyTradingDivision Entity
 * Date             :   04-July-2017
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
    public interface ICompanyTradingDivisionService : IBaseService<CompanyTradingDivisionModel, Master_CompanyTradingDivision>
    {
        List<CompanyTradingDivisionModel> GetCompanyTradingDivisionList(BaseViewModel dataModel);
        List<CompanyTradingDivisionModel> SaveCompanyTradingDivision(CompanyTradingDivisionModel dataModel);
        List<CompanyTradingDivisionModel> DeleteCompanyTradingDivision(CompanyTradingDivisionModel dataModel);
    }
    public class CompanyTradingDivisionService :
                            BaseService<CompanyTradingDivisionModel, Master_CompanyTradingDivision>,
                            ICompanyTradingDivisionService
    {
        private readonly ICompanyTradingDivisionRepository repo;
        public CompanyTradingDivisionService(ICompanyTradingDivisionRepository repo) : base(repo)
        {
            this.repo = repo;
        }

        public List<CompanyTradingDivisionModel> GetCompanyTradingDivisionList(BaseViewModel dataModel)
        {
            object pvalue;
            string svalue;
            CompanyTradingDivisionModel model;
            List<Master_CompanyTradingDivision> datalist;
            List<CompanyTradingDivisionModel> modlist = new List<CompanyTradingDivisionModel>();

            try
            {
                datalist = repo.GetCompanyTradingDivisionList();
                if (datalist != null)
                {
                    foreach (Master_CompanyTradingDivision data in datalist)
                    {
                        model = Mapper.Map<Master_CompanyTradingDivision, CompanyTradingDivisionModel>(data);

                        pvalue = Utility.GetPropertyValue(model, "Name", dataModel.CurrentCulture);
                        svalue = pvalue == null ? string.Empty : pvalue.ToString();
                        model.Name = svalue;

                        model.CurrentCulture = dataModel.CurrentCulture;
                        model.CurrentUserID = dataModel.CurrentUserID;
                        modlist.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            modlist.Sort(CompareByName);
            return modlist;
        }

        public List<CompanyTradingDivisionModel> SaveCompanyTradingDivision(CompanyTradingDivisionModel dataModel)
        {
            Master_CompanyTradingDivision data;

            try
            {
                Utility.SetDynamicPropertyValue(dataModel, dataModel.CurrentCulture);
                data = Mapper.Map<CompanyTradingDivisionModel, Master_CompanyTradingDivision>(dataModel);

                if (dataModel.Id == Guid.Empty)
                {
                    data.ID = Guid.NewGuid();
                    data.CreatedBy = dataModel.CurrentUserID;
                    data.CreatedDate = DateTime.Now;
                    repo.InsertCompanyTradingDivision(data);
                }
                else
                {
                    data.UpdatedBy = dataModel.CurrentUserID;
                    data.UpdatedDate = DateTime.Now;
                    repo.UpdateCompanyTradingDivision(data);
                }
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = dataModel.CurrentCulture;
            md.CurrentUserID = dataModel.CurrentUserID;
            return GetCompanyTradingDivisionList(md);
        }

        public List<CompanyTradingDivisionModel> DeleteCompanyTradingDivision(CompanyTradingDivisionModel dataModel)
        {
            try
            {
                repo.DeleteCompanyTradingDivision(dataModel.Id);
            }
            catch (Exception ex)
            {
                string message = LogException(ex, dataModel.CurrentUserID);
                throw new Exception(message);
            }

            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = dataModel.CurrentCulture;
            md.CurrentUserID = dataModel.CurrentUserID;
            return GetCompanyTradingDivisionList(md);
        }
        

        private string LogException(Exception ex, long userid)
        {
            IErrorLogService errorLog = new ErrorLogService();
            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
            errorLog.SetErrorLog(userid, "CompanyTradingDivision", message);
            return message;
        }

        private int CompareByName(CompanyTradingDivisionModel dataModel1, CompanyTradingDivisionModel dataModel2)
        {
            int cmpresult;
            cmpresult = string.Compare(dataModel1.Name, dataModel2.Name, true);
            return cmpresult;
        }
    }
}
