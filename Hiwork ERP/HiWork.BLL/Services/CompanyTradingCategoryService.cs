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

    public partial interface ICompanyTradingCategoryService : IBaseService<CompanyTradingCategoryModel, Master_CompanyTradingCategory>
    {
        CompanyTradingCategoryModel SaveCompanyTradingCategory(CompanyTradingCategoryModel aMCTC);
        List<CompanyTradingCategoryModel> GetAllCompanyTradingCategoryList(CompanyTradingCategoryModel aMCTC);
        List<CompanyTradingCategoryModel> DeleteCompanyTradingCategory(CompanyTradingCategoryModel aMCTC);
    }



    public class CompanyTradingCategoryService : BaseService<CompanyTradingCategoryModel, Master_CompanyTradingCategory>, ICompanyTradingCategoryService
    {
        private readonly ICompanyTradingCategoryRepository _ecRepository;
        public CompanyTradingCategoryService(ICompanyTradingCategoryRepository ecRepository) : base(ecRepository)
        {
            _ecRepository = ecRepository;
        }

        public CompanyTradingCategoryModel SaveCompanyTradingCategory(CompanyTradingCategoryModel aMCTC)
        {
            Master_CompanyTradingCategory mec = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMCTC, aMCTC.CurrentCulture);
                mec = Mapper.Map<CompanyTradingCategoryModel, Master_CompanyTradingCategory>(aMCTC);

                if (aMCTC.IsNew())
                {
                    mec.ID = Guid.NewGuid();
                    mec.CreatedBy = aMCTC.CurrentUserID;
                    mec.CreatedDate = DateTime.Now;
                    _ecRepository.InsertCompanyTradingCategory(mec);
                }
                else
                {
                    mec.UpdatedBy = aMCTC.CurrentUserID;
                    mec.UpdatedDate = DateTime.Now;
                    _ecRepository.UpdateCompanyTradingCategory(mec);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMCTC.CurrentUserID, "CompanyTradingCategory", message);
                throw new Exception(ex.Message);
            }
            return aMCTC;
        }



        public List<CompanyTradingCategoryModel> GetAllCompanyTradingCategoryList(CompanyTradingCategoryModel model)
        {
            List<CompanyTradingCategoryModel> mctctList = new List<CompanyTradingCategoryModel>();
            CompanyTradingCategoryModel mctcModel = new CompanyTradingCategoryModel();

            try
            {

                List<Master_CompanyTradingCategory> repoMasterCTC = _ecRepository.GetAllCompanyTradingCategoryList();
                if (repoMasterCTC != null)
                {
                    foreach (Master_CompanyTradingCategory mctc in repoMasterCTC)
                    {

                        mctcModel = Mapper.Map<Master_CompanyTradingCategory, CompanyTradingCategoryModel>(mctc);

                        mctcModel.Name = Utility.GetPropertyValue(mctcModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                                              Utility.GetPropertyValue(mctcModel, "Name", model.CurrentCulture).ToString();

                        mctcModel.CurrentUserID = model.CurrentUserID;
                        mctcModel.CurrentCulture = mctcModel.CurrentCulture;
                        mctcModel.CurrentUserID = mctcModel.CurrentUserID;


                        mctctList.Add(mctcModel);
                    }

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyTradingCategory", message);
                throw new Exception(ex.Message);
            }

            return mctctList;
        }
        public List<CompanyTradingCategoryModel> DeleteCompanyTradingCategory(CompanyTradingCategoryModel aMCTC)
        {
            List<CompanyTradingCategoryModel> mctc;
            try
            {
                _ecRepository.DeleteCompanyTradingCategory(aMCTC.ID);
                mctc = GetAllCompanyTradingCategoryList(aMCTC);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMCTC.CurrentUserID, "CompanyTradingCategory", message);
                throw new Exception(ex.Message);
            }
            return mctc;
        }



    }
}
