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

    public partial interface ICompanyTradingCategoryItemService : IBaseService<CompanyTradingCategoryItemModel, Master_CompanyTradingCategoryItem>
    {
        CompanyTradingCategoryItemModel SaveCompanyTradingCategoryItem(CompanyTradingCategoryItemModel aMCTCI);
        List<CompanyTradingCategoryItemModel> GetAllCompanyTradingCategoryItemList(CompanyTradingCategoryItemModel aMCTCI);
        List<CompanyTradingCategoryItemModel> DeleteCompanyTradingCategoryItem(CompanyTradingCategoryItemModel aMCTCI);
    }



    public class CompanyTradingCategoryItemService : BaseService<CompanyTradingCategoryItemModel, Master_CompanyTradingCategoryItem>, ICompanyTradingCategoryItemService
    {
        private readonly ICompanyTradingCategoryItemRepository _mecRepository;
        public CompanyTradingCategoryItemService(ICompanyTradingCategoryItemRepository mecRepository) : base(mecRepository)
        {
            _mecRepository = mecRepository;
        }

        public CompanyTradingCategoryItemModel SaveCompanyTradingCategoryItem(CompanyTradingCategoryItemModel aMCTCI)
        {
            Master_CompanyTradingCategoryItem mec = null;

            try
            {
                Utility.SetDynamicPropertyValue(aMCTCI, aMCTCI.CurrentCulture);
                mec = Mapper.Map<CompanyTradingCategoryItemModel, Master_CompanyTradingCategoryItem>(aMCTCI);

                if (aMCTCI.IsNew())
                {
                    mec.ID = Guid.NewGuid();
                    mec.TradingCategoryID = aMCTCI.TradingCategoryID;
                    mec.CreatedBy = aMCTCI.CurrentUserID;
                    mec.CreatedDate = DateTime.Now;
                    _mecRepository.InsertCompanyTradingCategoryItem(mec);
                }
                else
                {
                    mec.TradingCategoryID = aMCTCI.TradingCategoryID;
                    mec.UpdatedBy = aMCTCI.CurrentUserID;
                    mec.UpdatedDate = DateTime.Now;
                    _mecRepository.UpdateCompanyTradingCategoryItem(mec);
                }


            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMCTCI.CurrentUserID, "CompanyTradingCategoryItem", message);
                throw new Exception(ex.Message);
            }
            return aMCTCI;
        }


        public List<CompanyTradingCategoryItemModel> GetAllCompanyTradingCategoryItemList(CompanyTradingCategoryItemModel model)
        {
            List<CompanyTradingCategoryItemModel> mctctiList = new List<CompanyTradingCategoryItemModel>();
           CompanyTradingCategoryItemModel mctciModel = new CompanyTradingCategoryItemModel();

            try
            {

                List<Master_CompanyTradingCategoryItem> repoMasterCTC = _mecRepository.GetAllCompanyTradingCategoryItemList();
                if (repoMasterCTC != null)
                {

                    repoMasterCTC.ForEach(a =>
                    {
                        mctciModel = Mapper.Map<Master_CompanyTradingCategoryItem, CompanyTradingCategoryItemModel>(a);
                        mctciModel.CompanyTradingCategory = Mapper.Map<Master_CompanyTradingCategory, CompanyTradingCategoryModel>(a.Master_CompanyTradingCategory);

                        if (mctciModel.CompanyTradingCategory != null)
                            mctciModel.CompanyTradingCategory.Name = Utility.GetPropertyValue(mctciModel.CompanyTradingCategory, "Name", model.CurrentCulture) == null ? string.Empty :
                                                                     Utility.GetPropertyValue(mctciModel.CompanyTradingCategory, "Name", model.CurrentCulture).ToString();

                        mctciModel.Name = Utility.GetPropertyValue(mctciModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                          Utility.GetPropertyValue(mctciModel, "Name", model.CurrentCulture).ToString();
                        mctciModel.Description = Utility.GetPropertyValue(mctciModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                                 Utility.GetPropertyValue(mctciModel, "Description", model.CurrentCulture).ToString();


                        mctciModel.CurrentUserID = model.CurrentUserID;
                        mctciModel.CurrentCulture = model.CurrentCulture;



                        mctctiList.Add(mctciModel);


                    });
                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "CompanyTradingCategoryItem", message);
                throw new Exception(ex.Message);
            }

            return mctctiList;
        }
        public List<CompanyTradingCategoryItemModel> DeleteCompanyTradingCategoryItem(CompanyTradingCategoryItemModel aMCTCI)
        {
            List<CompanyTradingCategoryItemModel> mctc;
            try
            {
                _mecRepository.DeleteCompanyTradingCategoryItem(aMCTCI.ID);
                mctc = GetAllCompanyTradingCategoryItemList(aMCTCI);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMCTCI.CurrentUserID, "CompanyTradingCategoryItem", message);
                throw new Exception(ex.Message);
            }
            return mctc;
        } 
    }
}
