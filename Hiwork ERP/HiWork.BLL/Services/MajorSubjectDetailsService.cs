using AutoMapper;
using HiWork.BLL.Models;
using HiWork.BLL.Services;
using HiWork.DAL.Database;
using HiWork.DAL.Repositories;
using HiWork.Utils;
using HiWork.Utils.Infrastructure;
using HiWork.Utils.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.BLL.Services
{
    public partial interface IMajorSubjectDetailsService : IBaseService<MajorSubjectDetailsModel, Master_StaffMajorSubjectDetails>
    {
        MajorSubjectDetailsModel SaveMajorSubjectDetails(MajorSubjectDetailsModel aMajorSubjectDetailsModel);
        List<MajorSubjectDetailsModel> GetAllMajorSubjectDetailsList(MajorSubjectDetailsModel aMajorSubjectDetailsModel);
        
        List<MajorSubjectDetailsModel> DeleteMajorSubjectDetails(MajorSubjectDetailsModel aMajorSubjectDetailsModel);
    }


    public class MajorSubjectDetailsService : BaseService<MajorSubjectDetailsModel, Master_StaffMajorSubjectDetails>, IMajorSubjectDetailsService
    {
        public readonly IMajorSubjectDetailsRepository _msdRepository;
        public MajorSubjectDetailsService(IMajorSubjectDetailsRepository msdRepositoty) : base(msdRepositoty)
        {
            _msdRepository = msdRepositoty;

        }

        public MajorSubjectDetailsModel SaveMajorSubjectDetails(MajorSubjectDetailsModel aMajorSubjectDetailsModel)
        {
            Master_StaffMajorSubjectDetails msmsd = null;


            try
            {
                Utility.SetDynamicPropertyValue(aMajorSubjectDetailsModel, aMajorSubjectDetailsModel.CurrentCulture);
                msmsd = Mapper.Map<MajorSubjectDetailsModel, Master_StaffMajorSubjectDetails>(aMajorSubjectDetailsModel);


                if (aMajorSubjectDetailsModel.IsNew())
                {
                    msmsd.ID = Guid.NewGuid();
                    msmsd.StaffMajorSubjectID = aMajorSubjectDetailsModel.StaffMajorSubjectID;
                    msmsd.CreatedBy = aMajorSubjectDetailsModel.CurrentUserID;
                    msmsd.CreatedDate = DateTime.Now;
                    _msdRepository.InsertMajorSubjectDetails(msmsd);

                }

                else {
                    msmsd.StaffMajorSubjectID = aMajorSubjectDetailsModel.StaffMajorSubjectID;
                    msmsd.UpdatedBy = aMajorSubjectDetailsModel.CurrentUserID;
                    msmsd.UpdatedDate = DateTime.Now;
                    _msdRepository.UpdateMajorSubjectDetails(msmsd);

                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMajorSubjectDetailsModel.CurrentUserID, "MajorSubjectDetails", message);
                throw new Exception(ex.Message);
            }
            return aMajorSubjectDetailsModel;
        }


        public List<MajorSubjectDetailsModel> GetAllMajorSubjectDetailsList(MajorSubjectDetailsModel model)
        {
            List<MajorSubjectDetailsModel> msdList = new List<MajorSubjectDetailsModel>();
            MajorSubjectDetailsModel msdModel = new MajorSubjectDetailsModel();

            try
            {

                List<Master_StaffMajorSubjectDetails> repoMSDList = _msdRepository.GetAllMajorSubjectDetailsList();
                if (repoMSDList != null)
                {
                    repoMSDList.ForEach(a =>
                    {
                        msdModel = Mapper.Map<Master_StaffMajorSubjectDetails, MajorSubjectDetailsModel>(a);
                        msdModel.MajorSubject = Mapper.Map<Master_StaffMajorSubject, MajorSubjectModel>(a.Master_StaffMajorSubject);

                if (msdModel.MajorSubject != null)
                 msdModel.MajorSubject.Name = Utility.GetPropertyValue(msdModel.MajorSubject, "Name", model.CurrentCulture) == null ? string.Empty :
                                              Utility.GetPropertyValue(msdModel.MajorSubject, "Name", model.CurrentCulture).ToString();


                        msdModel.Name = Utility.GetPropertyValue(msdModel, "Name", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(msdModel, "Name", model.CurrentCulture).ToString();

                 msdModel.Description = Utility.GetPropertyValue(msdModel, "Description", model.CurrentCulture) == null ? string.Empty :
                                        Utility.GetPropertyValue(msdModel, "Description", model.CurrentCulture).ToString();

                  
                        msdModel.CurrentUserID = model.CurrentUserID;

                        msdModel.CurrentCulture = model.CurrentCulture;
                        msdList.Add(msdModel);
                    });
                }
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(model.CurrentUserID, "MajorSubjectDetails", message);
                throw new Exception(ex.Message);
            }

            return msdList;
        }
        public List<MajorSubjectDetailsModel> DeleteMajorSubjectDetails(MajorSubjectDetailsModel aMajorSubjectDetailsModel)
        {
            List<MajorSubjectDetailsModel> msd;
            try
            {
                _msdRepository.DeleteMajorSubjectDetails(aMajorSubjectDetailsModel.ID);
                msd = GetAllMajorSubjectDetailsList(aMajorSubjectDetailsModel);
            }
            catch (Exception ex)
            {

                IErrorLogService errorLog = new ErrorLogService();
                string message = ex.InnerException != null ? ex.InnerException.InnerException.Message : ex.Message;
                errorLog.SetErrorLog(aMajorSubjectDetailsModel.CurrentUserID, "MajorSubjectDetails", message);
                throw new Exception(ex.Message);
            }
            return msd;
        }
    }
}
    

