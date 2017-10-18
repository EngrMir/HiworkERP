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

    //public partial interface IMasterStaffTechnicalFieldsService : IBaseService<MasterStaffTechnicalFieldsModel, Master_StaffTechnicalFields>
    //{
    //    MasterStaffTechnicalFieldsModel SaveMasterStaffTechnicalFields(MasterStaffTechnicalFieldsModel aMSTFM);
    //    List<MasterStaffTechnicalFieldsModel> GetAllMasterStaffTechnicalFieldsList(MasterStaffTechnicalFieldsModel aMSTFM);
    //    List<MasterStaffTechnicalFieldsModel> DeleteMasterStaffTechnicalFields(MasterStaffTechnicalFieldsModel aMSTFM);
    //}



    //public class MasterStaffTechnicalFieldsService : BaseService<MasterStaffTechnicalFieldsModel, Master_StaffTechnicalFields>, IMasterStaffTechnicalFieldsService
    //{
    //    private readonly IMasterStaffTechnicalFieldsRepository _msfRepository;
    //    public MasterStaffTechnicalFieldsService(IMasterStaffTechnicalFieldsRepository msfRepository) : base(msfRepository)
    //    {
    //        _msfRepository = msfRepository;
    //    }

    //    public MasterStaffTechnicalFieldsModel SaveMasterStaffTechnicalFields(MasterStaffTechnicalFieldsModel aMSTFM)
    //    {
    //        Master_StaffTechnicalFields mec = null;

    //        try
    //        {
    //            Utility.SetDynamicPropertyValue(aMSTFM, aMSTFM.CurrentCulture);
    //            mec = Mapper.Map<MasterStaffTechnicalFieldsModel, Master_StaffTechnicalFields>(aMSTFM);

    //            if (aMSTFM.IsNew())
    //            {
    //                mec.ID = Guid.NewGuid();
    //                mec.CreatedBy = aMSTFM.CurrentUserID;
    //                mec.CreatedDate = DateTime.Now;
    //                _msfRepository.InsertMasterStaffTechnicalFields(mec);
    //            }
    //            else
    //            {
    //                mec.UpdatedBy = aMSTFM.CurrentUserID;
    //                mec.UpdatedDate = DateTime.Now;
    //                _msfRepository.UpdateMasterStaffTechnicalFields(mec);
    //            }


    //        }
    //        catch (Exception ex)
    //        {

    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(aMSTFM.CurrentUserID, "MasterStaffTechnicalFields", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return aMSTFM;
    //    }

    //    public List<MasterStaffTechnicalFieldsModel> GetAllMasterStaffTechnicalFieldsList(MasterStaffTechnicalFieldsModel model)
    //    {
    //        List<MasterStaffTechnicalFieldsModel> mstfList = new List<MasterStaffTechnicalFieldsModel>();
    //        MasterStaffTechnicalFieldsModel mstfModel = new MasterStaffTechnicalFieldsModel();

    //        try
    //        {

    //            List<Master_StaffTechnicalFields> repoMasterSTF = _msfRepository.GetAllMasterStaffTechnicalFieldsList();
    //            if (repoMasterSTF != null)
    //            {
    //                foreach (Master_StaffTechnicalFields mctc in repoMasterSTF)
    //                {

    //                    mstfModel = Mapper.Map<Master_StaffTechnicalFields, MasterStaffTechnicalFieldsModel>(mctc);

    //                    mstfModel.Name = Utility.GetPropertyValue(mstfModel, "Name", model.CurrentCulture) == null ? string.Empty :
    //                                                          Utility.GetPropertyValue(mstfModel, "Name", model.CurrentCulture).ToString();

    //                    mstfModel.CurrentUserID = model.CurrentUserID;
    //                    mstfModel.CurrentCulture = mstfModel.CurrentCulture;
    //                    mstfModel.CurrentUserID = mstfModel.CurrentUserID;


    //                    mstfList.Add(mstfModel);
    //                }

    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(model.CurrentUserID, "MasterStaffTechnicalFields", message);
    //            throw new Exception(ex.Message);
    //        }

    //        return mstfList;
    //    }
    //    public List<MasterStaffTechnicalFieldsModel> DeleteMasterStaffTechnicalFields(MasterStaffTechnicalFieldsModel aMCTC)
    //    {
    //        List<MasterStaffTechnicalFieldsModel> mctc;
    //        try
    //        {
    //            _msfRepository.DeleteMasterStaffTechnicalFields(aMCTC.ID);
    //            mctc = GetAllMasterStaffTechnicalFieldsList(aMCTC);
    //        }
    //        catch (Exception ex)
    //        {

    //            IErrorLogService errorLog = new ErrorLogService();
    //            string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
    //            errorLog.SetErrorLog(aMCTC.CurrentUserID, "MasterStaffTechnicalFields", message);
    //            throw new Exception(ex.Message);
    //        }
    //        return mctc;
    //    }
    //}
}
