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
    public partial interface IBranchService : IBaseService<BranchModel, Master_BranchOffice>
    {
        BranchModel SaveBranch(BranchModel aBranchModel);
        List<BranchModel> GetAllBranchList(BaseViewModel aBranchModel);
        BranchModel EditBranch(BranchModel aBranchModel);
        List<BranchModel> DeleteBranch(BranchModel aBranchModel);
    }

    public class BranchService : BaseService<BranchModel, Master_BranchOffice>, IBranchService
    {
        private readonly IBranchRepository _branRepository;
        public BranchService(IBranchRepository branRepository) : base(branRepository)
        {
            _branRepository = branRepository;
        }

        public BranchModel SaveBranch(BranchModel aBranchModel)
        {
            Master_BranchOffice branch = null;

            try
            {
                Utility.SetDynamicPropertyValue(aBranchModel, aBranchModel.CurrentCulture);
                branch = Mapper.Map<BranchModel, Master_BranchOffice>(aBranchModel);

                if (aBranchModel.ID==Guid.Empty)
                {
                    branch.ID = Guid.NewGuid();
                    branch.CountryID = aBranchModel.CountryId;
                    branch.CreatedBy = aBranchModel.CurrentUserID;
                    branch.CreatedDate = DateTime.Now;
                    _branRepository.InsertBranch(branch);
                }
                else
                {
                    branch.CountryID = aBranchModel.CountryId;
                    branch.UpdatedBy = aBranchModel.CurrentUserID;
                    branch.UpdatedDate = DateTime.Now;
                    _branRepository.UpdateBranch(branch);
                }

                
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aBranchModel.CurrentUserID, "Branch", message);
                throw new Exception(ex.Message);
            }
            return aBranchModel;
        }

        public BranchModel EditBranch(BranchModel aBranchModel)
        {

            try
            {
                var branch = Mapper.Map<BranchModel, Master_BranchOffice>(aBranchModel);
                Master_BranchOffice aBranch = _branRepository.UpdateBranch(branch);
                BranchModel branchModel = Mapper.Map<Master_BranchOffice, BranchModel>(aBranch);

            }

            catch (Exception ex)
            {
                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aBranchModel.CurrentUserID, "Branch", message);
                throw new Exception(ex.Message);
            }


            return aBranchModel;
        }

        public List<BranchModel> GetAllBranchList(BaseViewModel model)
        {
            List<BranchModel> branchList = new List<BranchModel>();
            BranchModel branchModel = new BranchModel();

            try
            {

                List<Master_BranchOffice> repoBranchList = _branRepository.GetAllBranchList();
                if (repoBranchList != null)
                {
                    repoBranchList.ForEach(a =>
                    {
                        branchModel = Mapper.Map<Master_BranchOffice, BranchModel>(a);
                        branchModel.Country = Mapper.Map<Master_Country, CountryModel>(a.Master_Country);

                        if (branchModel.Country != null)
                            branchModel.Country.Name = Utility.GetPropertyValue(branchModel.Country, "Name", model.CurrentCulture) == null ? string.Empty :
                                                       Utility.GetPropertyValue(branchModel.Country, "Name", model.CurrentCulture).ToString();


                        branchModel.Name = Utility.GetPropertyValue(branchModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                           Utility.GetPropertyValue(branchModel, "Name", model.CurrentCulture).ToString();
                        branchModel.CurrentUserID = model.CurrentUserID;

                        branchModel.CurrentCulture = model.CurrentCulture;
                        branchList.Add(branchModel);
                    });
                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "Branch", message);
                throw new Exception(ex.Message);
            }

            branchList.Sort(CompareBranchByName);
            return branchList;
        }
        public List<BranchModel> DeleteBranch(BranchModel aBranchModel)
        {
            List<BranchModel> branches;
            BaseViewModel md = new BaseViewModel();
            md.CurrentCulture = aBranchModel.CurrentCulture;
            md.CurrentUserID = aBranchModel.CurrentUserID;
            try
            {
                _branRepository.DeleteBranch(aBranchModel.ID);
                branches = GetAllBranchList(md);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aBranchModel.CurrentUserID, "Branch", message);
                throw new Exception(ex.Message);
            }
            return branches;
        }

        private int CompareBranchByName(BranchModel arg1, BranchModel arg2)
        {
            int cmpresult;
            cmpresult = string.Compare(arg1.Name, arg2.Name, true);
            return cmpresult;
        }
    }
}
